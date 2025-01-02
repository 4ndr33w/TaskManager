using System.Security.Claims;
using System.Text;

using Microsoft.EntityFrameworkCore;

using TaskManager.Api.DbContexts;
using TaskManager.Api.Services.SecurityOptions;
using TaskManager.Models;

namespace TaskManager.Api.Services
{
    public class AccountService
    {
        private readonly NpgDbContext _npgDbContext;
        private readonly IConfiguration _configuration;
        private readonly SecurityService _securityService;

        public AccountService(NpgDbContext npgDbContext, IConfiguration configuration, SecurityService securityService)
        {
            _npgDbContext = npgDbContext;
            _configuration = configuration;
            _securityService = securityService;
        }

        public Tuple<string, string> GetLoginAndPassFromBasicAuth(HttpRequest request)
        {
            string userLogin = "";
            string userPassword = "";
            string encodingType = Services.Authentication.AuthenticationOptions.GetEncodingType(_configuration);
            string authHeader = request.Headers.Authorization;

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding(encodingType);

                string[] nameAndPassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');

                userLogin = nameAndPassArray[0];
                userPassword = nameAndPassArray[1];
            }
            return new Tuple<string, string>(userLogin, userPassword);
        }

        public async Task<User> GetUser(string login, string password)
        {
            try
            {
                var existingUser = await _npgDbContext.Users.FirstOrDefaultAsync(u => u.Email == login);

                if (existingUser != null)
                {
                    bool isPasswordHashquals = _securityService.VerifyHashedPassword(password, existingUser.Password);
                    if (isPasswordHashquals)
                    {
                        return existingUser;
                    }
                }
            }
            catch (Exception) { }
            return null;
        }
        public async Task<User> GetUser(string login)
        {
            try
            {
                var user = await _npgDbContext.Users.FirstOrDefaultAsync(u => u.Email == login);
                return user;
            }
            catch (Exception)
            {
            }
            return null;
        }

        public async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            User currentUser = await GetUser(login, password);

            if (currentUser != null || currentUser != default)
            {
                currentUser.LastLoginDate = DateTime.UtcNow;
                _npgDbContext.Users.Update(currentUser);
                await _npgDbContext.SaveChangesAsync();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.UserStatus.ToString()),
                    new Claim(ClaimsIdentity.DefaultIssuer, currentUser.Id.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }
            return null;
        }

        public async Task<bool> IsCurrentEmailAlreadyUsedByOtherUser(string login)
        {
            var result = await _npgDbContext.Users.Select(u => u.Email).FirstOrDefaultAsync(e => e == login);

            if (result == string.Empty || result == null)
            {
                return true;
            }
            return false;
        }
    }
}
