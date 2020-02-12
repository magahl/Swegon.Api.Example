using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace Swegon.Api.Example
{
    public class TokenRetriever
    {
        private readonly IConfiguration opts;

        public TokenRetriever(IConfiguration opts)
        {
            this.opts = opts;
        }

        public async Task<AuthenticationResult> AuthResult()
        {
            var authContext = new AuthenticationContext($"{opts["Instance"]}/{opts["Domain"]}");
            var cred = new ClientCredential(opts["ClientId"], opts["ClientSecret"]);
            return await authContext.AcquireTokenAsync(opts["ResourceId"], cred);
        }
    }
}