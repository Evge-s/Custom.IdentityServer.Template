﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Identity.Api.Models.ServiceData.Tokens;
using Identity.Api.Models.ServiceData.UserData;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Services.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        
        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        
        public string GenerateJwtToken(Account account)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSecrets:TokenSecret").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public string? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSecrets:TokenSecret").Value);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 45 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var accountId = jwtToken.Claims.First(x => x.Type is ClaimTypes.NameIdentifier).Value;

                // return user id from JWT token if validation successful
                return accountId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public RefreshToken GenerateRefreshToken(Account account, string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Account = account,
                AccountId = account.Id,
                Token = getUniqueToken(account),
                // token is valid for 30 days
                Expires = DateTime.UtcNow.AddDays(30),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };

            return refreshToken;
        }

        private string getUniqueToken(Account account)
        {
            // token is a cryptographically strong random sequence of values
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            // ensure token is unique          
            if (account.RefreshTokens is not null
                && account.RefreshTokens.Count > 0
                && !account.RefreshTokens.Any(x => x.Token.Equals(token)))
                return getUniqueToken(account);

            return token;
        }
    }
}
