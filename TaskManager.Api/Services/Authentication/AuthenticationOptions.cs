using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TaskManager.Api.Services.Authentication
{
    public static class AuthenticationOptions
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration _config)
        {
            string key = _config["SymmetricSecurityKeyPhrase"];

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }

        public static string GetIssuer(IConfiguration config)
        {
            return config.GetSection("AuthenticationOptions")["Issuer"];
        }
        public static string GetAudience(IConfiguration config)
        {
            return config.GetSection("AuthenticationOptions")["Audience"];
        }

        public static int GetLifetime(IConfiguration config)
        {
            int lifeTime = 1;

            try
            {
                lifeTime = Convert.ToInt32(config.GetSection("AuthenticationOptions")["Lifetime"]);
            }
            catch (Exception)
            {
            }
            return lifeTime;
        }
    }
}
