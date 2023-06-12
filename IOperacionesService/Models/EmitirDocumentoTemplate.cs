﻿using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using static Aramis.Api.Repository.Enums.EstadoDocumentos;
using static Aramis.Api.Repository.Enumss.TipoDocumentos;

namespace Erp.Api.OperacionesService.Models
{
    public abstract partial class OperacionTemplate<T> where T : class
    {
        public virtual async Task<Guid> Emitir(T? documento = default)
        {
            try
            {
                BusOperacionInsert? nDocumento = await PrepararDocumento();
                _operaciones.Add(_mapper.Map<BusOperacion>(nDocumento));
                _unitOfWork.Commit();
                return nDocumento.Id;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        protected virtual async Task<BusOperacionInsert> PrepararDocumento()
        {
            OpCustomerDto cliente = await ClienteDelDocumento();
            NumeroLetra numeroLetra = await NumeroLetraDocumento();
            BusOperacionInsert operacion = new()
            {
                Operador = _security.GetUserAuthenticated(),
                CodAut = "",
                ClienteId = cliente.Id,
                EstadoId = await EstadoDelDocumento(),
                Numero = numeroLetra.Numero,
                Fecha = DateTime.Now,
                Razon = cliente.Razon,
                Pos = 0,
                TipoDocId = await TipoDelDocunento(),
                Vence = DateTime.Now,
                Id = Guid.NewGuid()
            };
            return operacion;
        }

        protected virtual async Task<OpCustomerDto> ClienteDelDocumento()
        {
            return await _customer.GetByCui("0");
        }

        protected virtual async Task<Guid> EstadoDelDocumento()
        {
            BusEstadoDto estado = await _estado.GetByName(Estado.ABIERTO.Name);
            return estado.Id;
        }

        protected virtual async Task<Guid> TipoDelDocunento()
        {
            TipoOperacionDto tipo = await _tipos.GetByName(TipoDocumento.PRESUPUESTO.Name);
            return tipo.Id;
        }

        protected virtual async Task<NumeroLetra> NumeroLetraDocumento()
        {
            SystemIndex? index = await _indexs.GetAll().OrderBy(x => x.Id).FirstOrDefaultAsync();
            NumeroLetra numeroLetra = new()
            {
                Letra = TipoDocumento.PRESUPUESTO.Code,
                Numero = index!.Presupuesto += 1
            };
            _indexs.Update(index);
            return numeroLetra;
        }

        #region Clases Privadas
        protected class NumeroLetra
        {
            public string? Letra { get; set; }
            public int Numero { get; set; }
        }
        #endregion
    }
}
