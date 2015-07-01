using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin.Security.Cookies;

namespace ResourceMetadata.API
{
    public class AuthRepository
    {
        private const string DomainName = "quadservices";

        public bool Validate(string userName, string password, out ClaimsIdentity oAuthIdentity, out ClaimsIdentity cookiesIdentity)
        {
            using (var ctx = new PrincipalContext(ContextType.Domain, DomainName))
            {
                bool isValid = ctx.ValidateCredentials(userName, password);
                if (isValid)
                {
                    oAuthIdentity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
                    cookiesIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationType);
                    var groups=GetUserGroups(userName);
                    oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
                    oAuthIdentity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",",groups)));
                    cookiesIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
                    cookiesIdentity.AddClaim(new Claim(ClaimTypes.Role, string.Join(",", groups)));
                }
                else
                {
                    oAuthIdentity = null;
                    cookiesIdentity = null;
                }

                return isValid;
            }
        }

        private List<string> GetUserGroups(string username)
        {
            var groups=new List<string>();
            UserPrincipal user = UserPrincipal.FindByIdentity(new PrincipalContext(ContextType.Domain, DomainName), IdentityType.SamAccountName, username);
            if (user != null) groups=user.GetGroups().AsQueryable().Select(l => l.ToString()).ToList();
            return groups;
        }
    }
}