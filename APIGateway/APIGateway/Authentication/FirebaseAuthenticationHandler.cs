using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace APIGateway.Authentication
{
    public class FirebaseAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly FirebaseApp _firebaseApp;

        public FirebaseAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, FirebaseApp firebaseApp) : base(options, logger, encoder, clock)
        {
            _firebaseApp = firebaseApp;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Context.Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.NoResult();
            }

            string bearerToken = Context.Request.Headers["Authorization"];
            if (bearerToken == null || !bearerToken.StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }

            string token = bearerToken.Substring("Bearer ".Length);

            try
            {
                FirebaseToken firebaseToken = await FirebaseAuth.GetAuth(_firebaseApp).VerifyIdTokenAsync(token);



                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new List<ClaimsIdentity>{
                new ClaimsIdentity(ToClaims(firebaseToken.Claims), nameof(FirebaseAuthenticationHandler))
            }), JwtBearerDefaults.AuthenticationScheme));
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail(e.Message);
            }


        }

        private IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
        {
            return new List<Claim>
            {
                new Claim("id", claims["user_id"].ToString()),
                new Claim("email", claims["email"].ToString()),
                new Claim("appid", claims["appid"].ToString())
                //new Claim("name", claims["name"].ToString())
            };
        }
    }
}