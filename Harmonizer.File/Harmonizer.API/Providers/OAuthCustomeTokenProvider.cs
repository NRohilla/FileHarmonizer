using Harmonizer.API.Service;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Harmonizer.API.Providers
{
    public class OAuthCustomeTokenProvider: OAuthAuthorizationServerProvider
    {
        #region[GrantResourceOwnerCredentials]
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                var userName = context.UserName;
                var password = context.Password;
                //var userService = new UserService();
                //var user = userService.Validate(userName, password);
                var userService = new LoginService();
                var user = userService.GetUser(userName, password);
                if (user != null)
                {
                    var commonData = userService.GetCommanData(userName);
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid, Convert.ToString(user.ID)),
                        new Claim(ClaimTypes.Name, user.UserID),
                        new Claim(ClaimTypes.Email, user.EmailID),
                        new Claim(ClaimTypes.Role, Convert.ToString(user.Role)),
                        new Claim("BPID",user.BPID),
                    };

                    var data = new Dictionary<string, string>
                    {
                        { "UserID", user.UserID },
                        { "BPID", user.BPID },
                        { "Role", Convert.ToString(user.Role)},
                        { "Email", user.EmailID},
                        { "SECID", user.SECID },
                        { "Partner", user.Partner },
                        { "BPType", user.BPType },
                        { "IsActive",Convert.ToString(user.IsActive)},
                        { "ExpireDate",Convert.ToString(user.ExpireDate) },
                        { "Sector", commonData.Sector },
                        { "HarmonizerValue", commonData.HarmonizerValue }

                    };
                    var properties = new AuthenticationProperties(data);

                    ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                        Startup.OAuthOptions.AuthenticationType);

                    var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                }
                else
                {
                    context.SetError("invalid_grant", "Either userid or password is incorrect");
                }
            });
        }
        #endregion

        #region[GrantRefreshToken]
        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            // Change authentication ticket for refresh token requests  
            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);
            // newIdentity.AddClaim(new Claim("newClaim", "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[ValidateClientAuthentication]
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        } /**/
        #endregion

        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
        }
        #endregion
    }
}
