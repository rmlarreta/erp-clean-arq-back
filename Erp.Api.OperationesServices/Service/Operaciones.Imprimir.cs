using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Detalles;
using Erp.Api.Domain.Entities;
using Erp.Api.Infrastructure.Data.Services;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Erp.Api.OperacionesService.Service
{
    public abstract partial class Operaciones : Service<BusOperacion>, IOperaciones
    {
        public async Task<FileStreamResult> Imprimir(Guid guid)
        {
            MemoryStream? stream = new();
            var operacion = await GetOperacion(guid);
            var dto = _mapper.Map<BusOperacionSumaryDto>(operacion);
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
                                            t.Line(operacion.TipoDoc.Code).FontSize(35).Bold();
                                            t.Span(operacion.TipoDoc.CodeExt).FontSize(10).Bold();
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
                                                    t.Span(operacion.TipoDoc.Name).NormalWeight().NormalPosition();
                                                    t.Line("\n FC: " + operacion.Numero.ToString()!.PadLeft(8, '0')).Bold();
                                                    t.Line("FECHA: " + operacion.Fecha.ToShortDateString()).NormalWeight().FontSize(10);
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
                           .Text("Cliente: " + operacion.Razon
                           + "\n" + operacion.Cliente.Domicilio
                           );

                            row.RelativeItem(1)
                            .DefaultTextStyle(f => f.FontSize(10))
                            .BorderTop(1, Unit.Millimetre).BorderColor("#858796")
                            .PaddingVertical(2, Unit.Millimetre)
                            .DefaultTextStyle(t => t.SemiBold())
                            .Text("Cuit: " + operacion.Cliente.Cui
                            + "\n" + "RESPONSABLE " + operacion.Cliente.RespNavigation.Name
                            );
                        });
                    });
                    page.Content().PaddingVertical(1, Unit.Millimetre)
                  .Table(table =>
                  {
                      table.ColumnsDefinition(columns =>
                      {
                          columns.RelativeColumn(2);
                          columns.RelativeColumn(10);
                          columns.RelativeColumn(2);
                          columns.RelativeColumn(2);
                          columns.RelativeColumn(1);
                          columns.RelativeColumn(2);
                      });
                      table.Cell().ColumnSpan(1)
                     .Background("#9ca4df")
                     .Text("Código");
                      table.Cell().ColumnSpan(1)
                       .Background("#9ca4df")
                     .Text("Detalle");
                      table.Cell().ColumnSpan(1)
                       .Background("#9ca4df")
                     .Text("Cantidad");
                      table.Cell().ColumnSpan(1)
                       .Background("#9ca4df")
                     .Text("Unitario");
                      table.Cell().ColumnSpan(1)
                       .Background("#9ca4df")
                     .Text(empresa.Respo == "MONOTRIBUTO" ? "Bon" : "Iva");
                      table.Cell().ColumnSpan(1)
                       .Background("#9ca4df")
                     .Text("Sub Total");

                      foreach (var c in _mapper.Map<List<BusOperacionDetalleSumaryDto>>(operacion.BusOperacionDetalles))
                      {
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(c.Codigo);
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignLeft().Text(c.Detalle);
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(c.Cantidad.ToString());
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text("$ " + empresa.Respo == "MONOTRIBUTO" ? (Math.Round((decimal)(c.Total! / c.Cantidad), 2)).ToString() : c.Unitario.ToString());
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(empresa.Respo == "MONOTRIBUTO" ? "% 0" : "% " + c.IvaValue.ToString());
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignRight().Text("$ " + Math.Round((decimal)c.Total!, 2).ToString());
                      }
                      if (dto.TipoDocName == "PRESUPUESTO")
                      {
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text("OBSERVACIONES   (VÁLIDO POR 7 DÍAS)");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                      }
                      foreach (var o in operacion.BusOperacionObservacions)
                      {
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text("*");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignLeft().Text(o.Observacion);
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
                          table.Cell().Padding(0).DefaultTextStyle(x => x.FontSize(8)).DefaultTextStyle(x => x.NormalWeight()).AlignCenter().Text(" ");
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
                       .Text(empresa.Respo != "MONOTRIBUTO" ?
                         "Neto Gravado: $ " + Math.Round((decimal)dto.TotalNeto!, 2)
                       + "\n" + "Excento: $ " + Math.Round((decimal)dto.TotalExento!, 2)
                       + "\n" + "IVA 10.5%: $ " + Math.Round((decimal)dto.TotalIva10!, 2)
                       + "\n" + "IVA 21.0%: $ " + Math.Round((decimal)dto.TotalIva21!, 2)
                       + "\n" + "Imp.Internos: $ " + Math.Round((decimal)dto.TotalInternos!, 2)
                       + "\n" + "\n" + "Ud. fue atendido por " + dto.Operador :
                       "Subtotal: $ " + Math.Round((decimal)(dto.TotalNeto! + dto.TotalIva! + dto.TotalExento!), 2)
                       + "\n" + "Imp.Internos: $ " + Math.Round((decimal)dto.TotalInternos!, 2)
                       + "\n" + "\n" + "Ud. fue atendido por " + dto.Operador
                       );
                        r.RelativeItem(2)
                       .DefaultTextStyle(t => t.FontSize(12))
                       .DefaultTextStyle(x => x.Bold())
                       .AlignRight()
                       .Text(text =>
                       {
                           text.Line("TOTAL $: " + Math.Round(dto.Total!, 2));
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

        protected virtual async Task<SysEmpresaDto> GetEmpresa()
        {
            return await _empresa.GetEmpresas();
        }

    }
}