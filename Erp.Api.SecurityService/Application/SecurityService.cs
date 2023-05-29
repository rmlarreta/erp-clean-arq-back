using AutoMapper;
using Erp.Api.Application.Dtos.Security;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.Infrastructure.Helpers;
using Erp.Api.SecurityService.Extensions;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.Extensions.Options;

namespace Erp.Api.SecurityService.Application
{
    public class SecurityService : Service<SecUser>, ISecurityService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public SecurityService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<UserAuth> Authenticate(UserRequest userRequest)
        {
            if (string.IsNullOrEmpty(userRequest.User) || string.IsNullOrEmpty(userRequest.Password))
            {
                throw new ArgumentException("Ingrese los datos solicitados");
            }

            SecUser user = await _userService.GetUserBaseByName(userRequest.User);

            // check if username exists
            if (user == null || user.Active == false)
            {
                throw new UnauthorizedAccessException();
            }

            // check if password is correct
            if (!ExtensionMethods.VerifyPasswordHash(userRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException();
            }

            if (user.EndOfLife < DateTime.Now.AddHours(-3))
            {
                throw new UnauthorizedAccessException("Debes Renovar tus Datos");
            }

            // authentication successful
            UserAuth userAuth = _mapper.Map<UserAuth>(user);

            string? token = ExtensionMethods.GetToken(userAuth, _appSettings.Secret!);
            userAuth.Token = token;

            return userAuth;
        }

        public Task<UserAuth> ChangePassword(UserRequest userRequest)
        {
            throw new NotImplementedException();
        }

        public string GetPerfilAuthenticated()
        {
            throw new NotImplementedException();
        }

        public string GetUserAuthenticated()
        {
            throw new NotImplementedException();
        }
    }
}
