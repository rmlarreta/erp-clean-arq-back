using AutoMapper;
using Erp.Api.Application.CommonService.Interfaces;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Domain.Entities;
using Erp.Api.Domain.Repositories;
using Erp.Api.FlowService.Business;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq.Expressions;

namespace Erp.Api.FlowService.Service
{
    public class Recibo : Service<CobRecibo>, IRecibo
    {
        private readonly IRepository<CobPo> _pos;
        private readonly ISysEmpresaService _empresa;
        private readonly IRepository<OpCliente> _customer;
        private readonly IRepository<SystemIndex> _sysIndexService;
        private readonly IRepository<CobTipoPago> _tipos;
        private readonly IRepository<CobCuentum> _cuentas;
        private readonly ISecurityService _security;
        private readonly IMapper _mapper;
        public Recibo(IUnitOfWork unitOfWork, ISecurityService security, ISysEmpresaService empresa, IMapper mapper) : base(unitOfWork)
        {
            _pos = _unitOfWork.GetRepository<CobPo>();
            _sysIndexService = _unitOfWork.GetRepository<SystemIndex>();
            _customer = _unitOfWork.GetRepository<OpCliente>();
            _tipos = _unitOfWork.GetRepository<CobTipoPago>();
            _empresa = empresa;
            _cuentas = _unitOfWork.GetRepository<CobCuentum>();
            _security = security;

            _mapper = mapper;
        }

        public async Task<Guid> InsertRecibo(CobReciboInsert recibo)
        {
            if (recibo.Operacion != null)
            {
                if (await CheckCuiAndPay(recibo)) throw new Exception("Este cliente no puede tener Cuenta Corriente");
            }
            recibo = ReordenarRecibo(recibo);
            var empresa = await GetEmpresa();
            var indexs = await _sysIndexService.GetAll().OrderBy(x => x.Id).FirstOrDefaultAsync();
            if (recibo.Detalles!.Any(x => x.PosId != null))
            {
                var monto = Math.Round(recibo.Detalles!.Where(x => x.PosId != null).Sum(x => x.Monto), 2) * 100;
                Guid posId = recibo.Detalles!.Where(x => x.PosId != null).FirstOrDefault()!.PosId!.Value;
                CobPo point = await _pos.Get(posId);
                PaymentIntent paymentIntent = new()
                {
                    Amount = (int)monto,
                    Additional_info = new()
                    {
                        External_reference = empresa.Fantasia,
                        Print_on_terminal = true,
                        Ticket_number = $"Recibo Nro: {indexs!.Recibo += 1}"
                    }
                };
                PaymentMP paymentMP = new(paymentIntent, point, indexs!.Production);
                PaymentIntentResponse? response = await paymentMP.PagoMP();
                switch (response.Status)
                {
                    case "OPEN" or "PROCESSING" or "ON_TERMINAL" or "PROCESSED":
                        throw new Exception("El tiempo para procesar el pago expiró");
                    case "CANCELED" or "ERROR":
                        throw new Exception("Ocurrió un error, vuelva a intentar");
                    case "FINISHED":
                        foreach (var item in recibo.Detalles!)
                        {
                            if (item.PosId != null)
                            {
                                item.CodAut = response.Id;
                                item.Observacion = "MERCADO PAGO";
                            }
                        }
                        break;
                    default:
                        throw new Exception("Ocurrió un error, vuelva a intentar");
                }
            }
            recibo.Id = Guid.NewGuid();
            recibo.Operador = _security.GetUserAuthenticated();
            recibo.Fecha = DateTime.Now;
            recibo.Numero = indexs!.Recibo += 1;
            _sysIndexService.Update(indexs);
            recibo.Detalles!.ForEach(x => { x.ReciboId = recibo.Id; x.Id = Guid.NewGuid(); x.Cancelado = false; });
            await ActualizarCuentas(recibo.Detalles);
            Expression<Func<CobTipoPago, bool>> expression = tipopago => tipopago.Name == "CUENTA CORRIENTE";
            Expression<Func<CobTipoPago, object>>[] includeProperties = Array.Empty<Expression<Func<CobTipoPago, object>>>();
            recibo.Detalles.RemoveAll(x => x.Tipo == _tipos.Get(expression, includeProperties).Result.Id);
            await Add(_mapper.Map<CobRecibo>(recibo));
            return recibo.Id.Value;
        }

        public async Task<CobRecibo> GetRecibo(Guid guid)
        {
            Expression<Func<CobRecibo, object>>[] includeProperties = new Expression<Func<CobRecibo, object>>[]
            {
              o => o.Cliente,
              o => o.Cliente.RespNavigation,
              o => o.BusOperacionPagos,
              o => o.CobReciboDetalles
            };
            var recibo = await Get(guid, includeProperties);
            foreach (var det in recibo.CobReciboDetalles)
            {
                if (det.PosId != null) det.Pos = await _pos.Get((Guid)det.PosId!);
                det.TipoNavigation = await _tipos.Get(det.Tipo);
                if (det.TipoNavigation.CuentaId != null) det.TipoNavigation.Cuenta = await _cuentas.Get((Guid)det.TipoNavigation.CuentaId!);
            }
            return recibo;
        }

        private async Task<bool> CheckCuiAndPay(CobReciboInsert recibo)
        {
            if (await Task.FromResult(_customer.Get(recibo.ClienteId).Result.Cui == "0" && recibo.Detalles!.Any(x => x.Tipo == _tipos.GetAll().OrderBy(x => x.Name).Where(x => x.Name == "CUENTA CORRIENTE").FirstOrDefaultAsync().Result!.Id)))
                return true;
            return false;
        }

        private static CobReciboInsert ReordenarRecibo(CobReciboInsert recibo)
        {
            var detallesAgrupados = recibo.Detalles!.GroupBy(d => d.Tipo);
            List<CobReciboDetallesInsert> detalles = new();
            foreach (var grupo in detallesAgrupados)
            {
                CobReciboDetallesInsert detallegrouped = new();
                detallegrouped.ReciboId = grupo.FirstOrDefault().Id;
                detallegrouped.Tipo = grupo.FirstOrDefault().Tipo;
                detallegrouped.Monto = grupo.Sum(x => x.Monto);
                detallegrouped.Observacion = grupo.FirstOrDefault().Observacion;
                detallegrouped.PosId = grupo.FirstOrDefault().PosId;
                detallegrouped.Cancelado = grupo.FirstOrDefault().Cancelado;
                detallegrouped.CodAut = grupo.FirstOrDefault().CodAut;
                detalles.Add(detallegrouped);
            }
            recibo.Detalles = detalles;
            return recibo;
        }

        private async Task ActualizarCuentas(List<CobReciboDetallesInsert> detalles)
        {
            List<CobCuentum> cuentas = new();
            foreach (var detalle in detalles)
            {
                var tipo = await _tipos.Get(detalle.Tipo);
                var cuenta = await _cuentas.Get((Guid)tipo.CuentaId!);
                cuenta.Saldo += detalle.Monto;
                cuentas.Add(cuenta);
            };
            _cuentas.UpdateRange(cuentas);
        }

        public async Task<FileStreamResult> Imprimir(Guid guid)
        {
            MemoryStream? stream = new();
            var recibo = await GetRecibo(guid);
            var dto = _mapper.Map<CobReciboInsert>(recibo);
            var empresa = await GetEmpresa();
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(5, Unit.Millimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(20));
                    page.DefaultTextStyle(x => x.FontFamily("Arial"));
                    page.Header().Column(column =>
                    {
                        column.Item().Height(35, Unit.Millimetre).Row(row =>
                        {
                            row.RelativeItem(1).Image("Images/logo.jpg");
                            row.RelativeItem(1).DefaultTextStyle(x => x.FontSize(12))
                                    .DefaultTextStyle(f => f.Bold())
                                    .AlignCenter()
                                    .Text(text =>
                                    {
                                        text.Span("Página ");
                                        text.CurrentPageNumber();
                                        text.Span(" de ");
                                        text.TotalPages();
                                    });

                            row.RelativeItem(1).Border(1, Unit.Point)
                            .Column(c =>
                            {
                                c.Item().Row(r =>
                                {
                                    r.RelativeItem()
                                    .Text(t =>
                                    {
                                        t.AlignCenter();
                                        t.Line("R").FontSize(35).Bold();
                                        t.Span("Recibo").FontSize(10).Bold();
                                    });
                                });
                            });
                            row.Spacing(10);
                            row.RelativeItem(2).Border(1, Unit.Point)
                                    .Padding(5, Unit.Millimetre)
                                    .DefaultTextStyle(x => x.FontSize(12))
                                    .DefaultTextStyle(f => f.Bold())
                                    .Column(c =>
                                    {
                                        c.Item().Row(r =>
                                        {
                                            r.RelativeItem()
                                            .Text(t =>
                                            {
                                                t.AlignRight();
                                                t.Span("RECIBO").NormalWeight().NormalPosition();
                                                t.Line("\n Nº: " + recibo.Numero.ToString()!.PadLeft(8, '0')).Bold();
                                                t.Line("FECHA: " + recibo.Fecha.ToShortDateString()).NormalWeight().FontSize(10);
                                            });
                                        });
                                    });
                        });
                        column.Item().Row(row =>
                        {
                            row.RelativeItem(1)
                            .PaddingVertical(3, Unit.Millimetre)
                            .DefaultTextStyle(f => f.FontSize(10))
                            .DefaultTextStyle(t => t.FontColor("#1f66ff"))
                            .Text("Razón Social: " + empresa.Razon
                            + "\n" + empresa.Fantasia
                            + "\n" + empresa.Domicilio
                            + "\n" + empresa.Respo
                            );

                            row.RelativeItem(1)
                           .PaddingVertical(3, Unit.Millimetre)
                           .DefaultTextStyle(f => f.FontSize(10))
                           .DefaultTextStyle(t => t.FontColor("#1f66ff"))
                           .Text("Cuit: " + empresa.Cuit
                           + "\n" + "IIBB: " + empresa.Iibb
                           + "\n" + "I. Actividades: " + empresa.Inicio.ToShortDateString()
                           );
                        });

                        column.Item().Row(row =>
                        {
                            row.RelativeItem(1)
                           .DefaultTextStyle(f => f.FontSize(10))
                           .BorderTop(1, Unit.Millimetre).BorderColor("#858796")
                           .PaddingVertical(2, Unit.Millimetre)
                           .DefaultTextStyle(t => t.SemiBold())
                           .Text("Cliente: " + recibo.Cliente.Razon
                           + "\n" + recibo.Cliente.Domicilio
                           );

                            row.RelativeItem(1)
                            .DefaultTextStyle(f => f.FontSize(10))
                            .BorderTop(1, Unit.Millimetre).BorderColor("#858796")
                            .PaddingVertical(2, Unit.Millimetre)
                            .DefaultTextStyle(t => t.SemiBold())
                            .Text("Cuit: " + recibo.Cliente.Cui
                            + "\n" + "IVA: " + recibo.Cliente.RespNavigation.Name
                            );
                        });
                    });
                    page.Content().PaddingVertical(1, Unit.Millimetre)
                  .Table(table =>
                  {
                      table.ColumnsDefinition(columns =>
                      {
                          columns.RelativeColumn(3);
                          columns.RelativeColumn(2);
                          columns.RelativeColumn(5);
                          columns.RelativeColumn(2);
                      });
                      table.Cell().Padding(0)
                     .Background("#9ca4df").AlignCenter()
                     .Text("Tipo");
                      table.Cell().Padding(0)
                     .Background("#9ca4df").AlignCenter()
                     .Text("#");
                      table.Cell().Padding(0)
                     .Background("#9ca4df").AlignCenter()
                     .Text("#");
                      table.Cell().Padding(0)
                      .Background("#9ca4df").AlignCenter()
                     .Text("Monto");
                      foreach (var c in recibo.CobReciboDetalles)
                      {
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(c.TipoNavigation.Name);
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignLeft().Text(c.Pos != null ? c.Pos?.Name + $" {c.CodAut}" : "");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(c.Observacion);
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text("$ " + c.Monto.ToString());
                      }
                  });
                    page.Footer()
                .BorderTop(1, Unit.Point)
                .Column(c =>
                {
                    c.Item().BorderTop(1, Unit.Point);
                    c.Item().Row(r =>
                    {
                        r.RelativeItem()
                       .DefaultTextStyle(t => t.FontSize(10))
                       .DefaultTextStyle(x => x.Bold())
                       .Text(
                        "\n" + "\n" + "Ud. fue atendido por " + dto.Operador
                       );
                        r.RelativeItem(2)
                       .DefaultTextStyle(t => t.FontSize(12))
                       .DefaultTextStyle(x => x.Bold())
                       .AlignRight()
                       .Text(text =>
                       {
                           text.Line("TOTAL $: " + Math.Round(recibo.CobReciboDetalles.Sum(x => x.Monto)!, 2));
                           text.Line(dto.TotalLetras!).FontSize(8);
                       });
                    });
                    c.Item().BorderTop(1, Unit.Point);
                    c.Item()
                    .Row(r =>
                    {
                        r.RelativeItem()
                       .DefaultTextStyle(t => t.FontSize(10))
                       .DefaultTextStyle(x => x.Bold())
                       .AlignLeft()
                       .Text(text =>
                       {
                           text.Span("2023 © Desarrollado por Aramis Sistemas");
                       });
                        r.RelativeItem()
                     .DefaultTextStyle(t => t.FontSize(10))
                     .DefaultTextStyle(x => x.Bold())
                     .AlignLeft()
                     .Text(text =>
                     {
                         text.Span("Página ");
                         text.CurrentPageNumber();
                         text.Span(" de ");
                         text.TotalPages();
                     });
                    });
                });
                });
            })
            .GeneratePdf(stream);
            stream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(stream, "application/pdf");
        }

        private async Task<SysEmpresaDto> GetEmpresa()
        {
            return await _empresa.GetEmpresas();
        }
    }
}