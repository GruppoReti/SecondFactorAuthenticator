using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.IdentityServer.Web.Authentication.External;
using Newtonsoft.Json;

namespace SecondFactorAuthenticator
{
    public class AuthenticationAdapter : IAuthenticationAdapter
    {
        private string GetHostName(string ipAddress)
        {
            try
            {
                IPHostEntry entry = Dns.GetHostEntry(ipAddress);
                return (entry != null) ? entry.HostName : "unknown";
            }
            catch
            {
                return "unknown";
            }
        }

        public IAdapterPresentation BeginAuthentication(Claim identityClaim, HttpListenerRequest request, IAuthenticationContext context)
        {
            context.Data.Add("userid", identityClaim.Value);
            return new AdapterPresentation();
        }

        public bool IsAvailableForUser(Claim identityClaim, IAuthenticationContext context)
        {
            return true;
        }

        public IAuthenticationAdapterMetadata Metadata
        {
            get { return new AuthenticationAdapterMetadata(); }
        }

        public void OnAuthenticationPipelineLoad(IAuthenticationMethodConfigData configData)
        {

        }

        public void OnAuthenticationPipelineUnload()
        {

        }

        public IAdapterPresentation OnError(HttpListenerRequest request, ExternalAuthenticationException ex)
        {
            return new AdapterPresentation(ex.Message, true);
        }

        public IAdapterPresentation TryEndAuthentication(IAuthenticationContext context, IProofData proofData, HttpListenerRequest request, out Claim[] claims)
        {
            claims = null;

            IAdapterPresentation result = new AdapterPresentation(AdapterMessages.UNSUPPORTED_DB, false);
            string userid = (string)context.Data["userid"];
            string hostname = GetHostName(request.RemoteEndPoint.Address.ToString());
            
            if (Resources.Database.DatabaseType == "JSON")
            {
                if (Resources.Database.ConnectionString == null)
                {
                    result = new AdapterPresentation(AdapterMessages.WRONG_DB, false);
                }
                else
                {
                    string jsonString = File.ReadAllText(Resources.Database.ConnectionString);
                    Dictionary<string, List<string>> values = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonString);
                    if (!values.ContainsKey(userid))
                    {
                        result = new AdapterPresentation(AdapterMessages.USER_UNKNOWN, false);
                    }
                    else
                    {
                        if (!values[userid].Contains(hostname))
                        {
                            result = new AdapterPresentation(AdapterMessages.USER_UNREGISTERED, false);
                        }
                        else
                        {
                            Claim claim = new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2012/12/authmethod/otp");
                            claims = new Claim[] { claim };
                            result = null;
                        }
                    }
                }
            }
            return result;
        }
    }
}
