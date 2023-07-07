using AutoMapper;
using Erp.Api.Application.Dtos.Commons;
using Erp.Api.Application.Dtos.Customers;
using Erp.Api.Application.Dtos.Flow;
using Erp.Api.Application.Dtos.Operaciones;
using Erp.Api.Application.Dtos.Operaciones.Commons;
using Erp.Api.Application.Dtos.Operaciones.Detalles;
using Erp.Api.Application.Dtos.Productos;
using Erp.Api.Application.Dtos.Security;
using Erp.Api.Application.Dtos.Users;
using Erp.Api.Application.Dtos.Users.Commons;
using Erp.Api.Domain.Entities;

namespace Erp.Api.Application.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Security
            CreateMap<SecUser, UserAuth>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleNavigation.Name))
                .ReverseMap();

            CreateMap<SecUser, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.RoleNavigation.Name))
                .ReverseMap();

            CreateMap<SecUser, UserUpdateDto>()
                .ReverseMap();

            CreateMap<SecRole, SecRoleDto>()
                .ReverseMap();
            #endregion

            #region Operaciones
            CreateMap<BusOperacion, BusOperacionDto>()
                .ForMember(dest => dest.TipoDocName, opt => opt.MapFrom(src => src.TipoDoc.Name))
                .ForMember(dest => dest.EstadoName, opt => opt.MapFrom(src => src.Estado.Name))
                .ForMember(dest => dest.Cui, opt => opt.MapFrom(src => src.Cliente.Cui))
                .ForMember(dest => dest.Domicilio, opt => opt.MapFrom(src => src.Cliente.Domicilio))
                .ForMember(dest => dest.Resp, opt => opt.MapFrom(src => src.Cliente.RespNavigation.Name));

            CreateMap<BusOperacion, BusOperacionInsert>()
                .ReverseMap();

            CreateMap<BusOperacion, BusOperacionSumaryDto>()
                .ForMember(dest => dest.TipoDocName, opt => opt.MapFrom(src => src.TipoDoc.Name))
                .ForMember(dest => dest.EstadoName, opt => opt.MapFrom(src => src.Estado.Name))
                .ForMember(dest => dest.Cui, opt => opt.MapFrom(src => src.Cliente.Cui))
                .ForMember(dest => dest.Domicilio, opt => opt.MapFrom(src => src.Cliente.Domicilio))
                .ForMember(dest => dest.Resp, opt => opt.MapFrom(src => src.Cliente.RespNavigation.Name))
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.BusOperacionDetalles))
                .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.BusOperacionObservacions))
                .ForMember(dest => dest.Pagos, opt => opt.MapFrom(src => src.BusOperacionPagos));

            CreateMap<BusOperacionDto, BusOperacionSumaryDto>()
                .ReverseMap();
            #endregion

            #region Detalles
            CreateMap<BusOperacionDetalle, BusOperacionDetalleDto>()
                .ForMember(dest => dest.Detalle, opt => opt.MapFrom(src => src.Detalle))
                .ReverseMap();

            CreateMap<BusOperacionDetalle, BusOperacionDetalleSumaryDto>()
                .ForMember(dest => dest.Detalle, opt => opt.MapFrom(src => src.Detalle))
                .ReverseMap();
            #endregion

            #region Commons Operaciones
            CreateMap<BusOperacionTipo, TipoOperacionDto>()
                .ReverseMap();

            CreateMap<BusEstado, BusEstadoDto>()
                .ReverseMap();
            #endregion

            #region Systems
            CreateMap<SystemIndex, SysIndexDto>()
                .ReverseMap();

            CreateMap<SystemEmpresa, SysEmpresaDto>()
                .ReverseMap();
            #endregion

            #region Customers

            CreateMap<OpCliente, OpCustomerInsert>() 
               .ReverseMap();

            CreateMap<OpCliente, OpCustomerDto>()
                .ForMember(dest => dest.PaisName, opt => opt.MapFrom(src => src.PaisNavigation.Name))
                .ForMember(dest => dest.GenderName, opt => opt.MapFrom(src => src.GenderNavigation.Name))
                .ForMember(dest => dest.RespName, opt => opt.MapFrom(src => src.RespNavigation.Name))
                .ReverseMap();
            #endregion

            #region Productos
            CreateMap<StockProduct, ProductoDto>()
               .ReverseMap();

            CreateMap<StockProduct, ProductoSummaryDto>()
               .ForMember(dest => dest.IvaValue, opt => opt.MapFrom(src => src.IvaNavigation.Value))
               .ForMember(dest => dest.RubroName, opt => opt.MapFrom(src => src.RubroNavigation.Name))
               .ReverseMap();

            CreateMap<StockRubro, RubroDto>()
                .ReverseMap();
            CreateMap<StockIva, IvaDto>()
                .ReverseMap();

            #endregion

            #region Cobranzas
            CreateMap<CobRecibo, CobReciboInsert>()
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.CobReciboDetalles))
                .ReverseMap();

            CreateMap<CobReciboDetalle, CobReciboDetallesInsert>() 
               .ReverseMap();

            CreateMap<CobTipoPago, CobTipoPagoDto>()
                .ReverseMap();

            CreateMap<CobPo, PosDto>()
              .ReverseMap();

            CreateMap<BusOperacionPago, BusOperacionPagoDto>()
                .ReverseMap();
            #endregion
        }
    }
}
