using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace TaskManager.Api.Services.SecurityOptions
{
    public class SecurityService
    {
        private readonly IConfiguration _configuration;
        public SecurityService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string CreatePasswordHash(string password)
        {
            byte[] salt;
            byte[] buffer2;

            if (password != null)
            {
                using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
                {
                    salt = bytes.Salt;
                    buffer2 = bytes.GetBytes(0x20);
                }
                byte[] dst = new byte[0x31];
                Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
                Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
                return Convert.ToBase64String(dst);
            }
            else
            {
                return string.Empty;
            }
        }

        public bool VerifyPasswordIfHashed(string userHashedPassword, string hashedPassword)
        {
            if (hashedPassword == null || userHashedPassword == null)
            {
                return false;
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);

            byte[] src1 = Convert.FromBase64String(hashedPassword);
            byte[] dst1 = new byte[0x10];
            Buffer.BlockCopy(src1, 1, dst1, 0, 0x10);
            byte[] buffer4 = new byte[0x20];
            Buffer.BlockCopy(src1, 0x11, buffer4, 0, 0x20);

            return ByteArrayEquals(buffer3, buffer4);
        }

        public bool VerifyHashedPassword(string userPassword, string hashedPassword)
        {
            byte[] buffer4;
            if (hashedPassword == null || userPassword == null)
            {
                return false;
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(userPassword, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArrayEquals(buffer3, buffer4);
        }

        private bool ByteArrayEquals(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        public string GetEncodedJwt(ClaimsIdentity identity)
        {
            var now = DateTime.Now;
            var jwt = new JwtSecurityToken(
                issuer: Services.Authentication.AuthenticationOptions.GetIssuer(_configuration),
            audience: Services.Authentication.AuthenticationOptions.GetAudience(_configuration),
            notBefore: now,
                claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(Services.Authentication.AuthenticationOptions.GetLifetime(_configuration))),
            signingCredentials: new SigningCredentials(Services.
                Authentication.
                AuthenticationOptions.
                GetSymmetricSecurityKey(_configuration), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
