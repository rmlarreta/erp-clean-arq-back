using AutoMapper;
using Erp.Api.Application.Dtos.Users;
using Erp.Api.Domain.Entities;
using Erp.Api.Infraestructure.UnitOfWorks;
using Erp.Api.Infrastructure.Data.Services;
using Erp.Api.SecurityService.Extensions;
using Erp.Api.SecurityService.Interfaces;
using System.Linq.Expressions;

namespace Erp.Api.SecurityService.Application
{
    public class UserService : Service<SecUser>, IUserService
    {
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task CreateUser(UserInsertDto user)
        {
            if (string.IsNullOrWhiteSpace(user.PassWord))
            {
                throw new ArgumentException("Falta la contraseña");
            }

            if (await GetUserByName(user.UserName!) != null)
            {
                throw new Exception("El usuario\"" + user.UserName + "\" ya está en uso");
            }

            ExtensionMethods.CreatePasswordHash(user.PassWord, out byte[] passwordHash, out byte[] passwordSalt);
            SecUser newuser = new();
            newuser.UserName = user.UserName!;
            newuser.RealName = user.RealName!;
            newuser.PasswordHash = passwordHash;
            newuser.PasswordSalt = passwordSalt;
            newuser.EndOfLife = DateTime.Today.AddDays(-1);
            newuser.Active = false;
            newuser.Role = user.Role;
            newuser.Id = Guid.NewGuid();
            await Add(newuser);
        }

        public async Task DeleteUser(Guid id)
        {
            SecUser user = await Get(id);
            await Delete(user);
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            Expression<Func<SecUser, object>>[] includeProperties = new Expression<Func<SecUser, object>>[]
              {
                    u=>u.RoleNavigation
              };
            List<SecUser>? users = GetAll(includeProperties).ToList();
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<SecUser> GetUserBaseByName(string username)
        {
            Expression<Func<SecUser, bool>> expression = c => c.UserName == username;
            Expression<Func<SecUser, object>>[] includeProperties = new Expression<Func<SecUser, object>>[]
            {
                    u=>u.RoleNavigation
            };
            return await Get(expression, includeProperties);
        }

        public async Task<UserDto> GetUserById(Guid id)
        {
            Expression<Func<SecUser, object>>[] includeProperties = new Expression<Func<SecUser, object>>[]
            {
                    u=>u.RoleNavigation
            };
            SecUser? user = await Get(id, includeProperties);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByName(string name)
        {
            Expression<Func<SecUser, bool>> expression = c => c.UserName == name;
            Expression<Func<SecUser, object>>[] includeProperties = new Expression<Func<SecUser, object>>[]
            {
                    u=>u.RoleNavigation
            };
            SecUser? user = await Get(expression, includeProperties);
            return _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUser(UserUpdateDto user)
        {
            await Update(_mapper.Map<SecUser>(user));
        }
    }
}
