using AutoMapper;
using Erp.Api.Application.Dtos.Security;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.Infrastructure.Helpers;
using Erp.Api.SecurityService.Extensions;
using Erp.Api.SecurityService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Erp.Api.SecurityService.Application
{
    public class SecurityService : Service<SecUser>, ISecurityService
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly HttpContext _hcontext;
        private readonly ClaimsPrincipal _cp;
        private bool _ischanging = false;
        public SecurityService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings, IHttpContextAccessor haccess) : base(unitOfWork)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _hcontext = haccess.HttpContext!;
            _cp = _hcontext.User;
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
                throw new UnauthorizedAccessException("Acceso no Autorizado");
            }

            // check if password is correct
            if (!ExtensionMethods.VerifyPasswordHash(userRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Datos Incorrectos");
            }

            EndOfLife(user.EndOfLife);

            // authentication successful
            UserAuth userAuth = _mapper.Map<UserAuth>(user);

            string? token = ExtensionMethods.GetToken(userAuth, _appSettings.Secret!);
            userAuth.Token = token;

            return userAuth;
        }

        public async Task<UserAuth> ChangePassword(UserRequest userRequest)
        {
            _ischanging = true;
            UserAuth? user = await Authenticate(userRequest);
            SecUser? secUser = new();
            if (user != null) { secUser = await _userService.GetUserBaseByName(userRequest.User!); }

            if (string.IsNullOrWhiteSpace(userRequest.NPassword))
            {
                throw new ArgumentException("Ingrese los datos solicitados");
            }

            ExtensionMethods.CreatePasswordHash(userRequest.NPassword, out byte[] passwordHash, out byte[] passwordSalt);

            secUser.PasswordHash = passwordHash;
            secUser.PasswordSalt = passwordSalt;
            secUser.EndOfLife = DateTime.Today.AddMonths(1);
            await Update(secUser);
            _ischanging = false;
            userRequest.Password = userRequest.NPassword;
            return await Authenticate(userRequest);
        }

        public string GetPerfilAuthenticated()
        {
            return ExtensionMethods.GetUserPerfil(_cp); 
        }

        public string GetUserAuthenticated()
        {
            string? user = ExtensionMethods.GetUserName(_cp);
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            return user;
        }

        #region PRIVATE       
        private void EndOfLife(DateTime endOfLife)
        {
            if (endOfLife < DateTime.Now.AddHours(-3))
            {
                if (!_ischanging)
                {
                    throw new UnauthorizedAccessException("Debes Renovar tus Datos");
                }
            }
        }
        #endregion PRIVATE
    }
}
