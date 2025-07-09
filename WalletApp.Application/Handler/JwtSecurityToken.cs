using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace WalletApp.Application.Handler.LoginUserCommandHandler
{
    internal class JwtSecurityToken
    {
        private object issuer;
        private object audience;
        private Claim[] claims;
        private object expires;
        private SigningCredentials signingCredentials;

        public JwtSecurityToken(object issuer, object audience, Claim[] claims, object expires, SigningCredentials signingCredentials)
        {
            this.issuer = issuer;
            this.audience = audience;
            this.claims = claims;
            this.expires = expires;
            this.signingCredentials = signingCredentials;
        }
    }
}