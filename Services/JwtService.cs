using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TTCore.StoreProvider.Models;

namespace TTCore.StoreProvider.Services
{
    public interface IJwtService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByUsername(string userName);
        string GenerateJwtToken(string name);
        JwtSecurityToken ValidateToken(string token);
    }

    public class JwtService : IJwtService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        readonly AppSettings _appSettings = null;
        readonly JwtSecurityTokenHandler JwtTokenHandler = null;
        readonly byte[] SecretBytes = null;
        readonly SymmetricSecurityKey SecurityKey = null;
        readonly SigningCredentials SigningCredential = null;

        public JwtService(AppSettings appSettings)
        {
            _appSettings = appSettings;
            SecretBytes = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityKey = new SymmetricSecurityKey(SecretBytes);
            JwtTokenHandler = new JwtSecurityTokenHandler();
            //SigningCredential = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256Signature);
            SigningCredential = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
            // return null if user not found
            if (user == null) return null;
            // authentication successful so generate jwt token
            var token = GenerateJwtToken(user.Id.ToString());
            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        public User GetByUsername(string userName)
        {
            return _users.FirstOrDefault(x => x.Username == userName);
        }

        public JwtSecurityToken ValidateToken(string token) {
            if (string.IsNullOrWhiteSpace(token)) return null;
            
            token = token.Split(" ").Last();

            JwtTokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            if (validatedToken == null) return null;

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken;
        }

        public string GenerateJwtToken(string name)
        {
            // generate token that is valid for 7 days

            string token = string.Empty;
            if (string.IsNullOrEmpty(name)) return token;

            //var claims = new[] { new Claim(ClaimTypes.Name, name) };
            //var jwt = new JwtSecurityToken("ExampleServer", "ExampleClients", claims,
            //    expires: DateTime.Now.AddSeconds(60), signingCredentials: SigningCredential);
            //token = JwtTokenHandler.WriteToken(jwt);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", name) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = SigningCredential
            };
            var jwt = JwtTokenHandler.CreateToken(tokenDescriptor);
            token = JwtTokenHandler.WriteToken(jwt);

            return token;
        }
    }
}
