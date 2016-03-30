using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.IdentityServer.Web.Authentication.External;
using Newtonsoft.Json;
using SecondFactorAuthenticator.Database;

namespace SecondFactorAuthenticator
{
    public class AuthenticationAdapter : IAuthenticationAdapter
    {
        IDatabase databaseConnection = null;

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

        /// <summary>
        /// This method is called by AD FS once AD FS decides that Multi-Factor Authentication is required (and available) for a user.
        /// </summary>
        /// <param name="identityClaim">The identity claim for the user being authenticated (from first factor).</param>
        /// <param name="request">The request object containing HTTP information about the client.</param>
        /// <param name="context">The authentication context.</param>
        /// <returns>The adapter presentation to be shown to the user.</returns>
        public IAdapterPresentation BeginAuthentication(Claim identityClaim, HttpListenerRequest request, IAuthenticationContext context)
        {
            context.Data.Add("userid", identityClaim.Value);
            return new AdapterPresentation();
        }

        /// <summary>
        /// The IsAvailableForUser method returns either true or false and is an indication to AD FS that your
        /// Authentication Adapter can actually perform Multi-Factor Authentication for the user.
        /// In order to decide whether to return true or false, an identity claim is passed from AD FS to the
        /// Authentication Provider.
        /// The method should return true if this Authentication Provider can handle authentication for this identity,
        /// or user, and false when it can not.
        /// </summary>
        /// <param name="identityClaim">The identity claim for the user being authenticated (from first factor).</param>
        /// <param name="context">The authentication context.</param>
        /// <returns>True if this mechanism is available for the user and false if not.</returns>
        public bool IsAvailableForUser(Claim identityClaim, IAuthenticationContext context)
        {
            return true;
        }

        /// <summary>
        /// Metadata actually is not a method but a property. It is used by AD FS to learn about your Authentication Provider
        /// (just like the FederationMetadata.xml can be used by AD FS to learn about a relying party or claims provider).
        /// The property should 'return' a variable of a type that implements IAuthenticationAdapterMetadata. 
        /// </summary>
        public IAuthenticationAdapterMetadata Metadata
        {
            get { return new AuthenticationAdapterMetadata(); }
        }

        /// <summary>
        /// The OnAuthenticationPipelineLoad method is called whenever the Authentication Provider is loaded by AD FS into it's pipeline.
        /// It allows your adapter to initialize itself. AD FS will 'tell' your adapter which information AD FS has. 
        /// </summary>
        /// <param name="configData">The configuration data for the AD FS.</param>
        public void OnAuthenticationPipelineLoad(IAuthenticationMethodConfigData configData)
        {
            if (Resources.Database.DatabaseType == JsonDatabase.ID)
            {
                databaseConnection = new JsonDatabase();
            }

            if (databaseConnection == null)
            {
                throw new DatabaseException(AdapterMessages.UNSUPPORTED_DB);
            }

            databaseConnection.StartConnection();
        }

        /// <summary>
        /// The OnAuthenticationPipelineUnload is called by AD FS whenever the Authentication Provider is unloaded from the AD FS
        /// pipeline and allows the Authentication Adapter to clean up anything it has to clean up.
        /// </summary>
        public void OnAuthenticationPipelineUnload()
        {
            if (databaseConnection != null)
            {
                databaseConnection.CloseConnection();
            }
        }

        /// <summary>
        /// The OnError method is called whenever something goes wrong in the authentication process.
        /// To be more precise; if anything goes wrong in the BeginAuthentication or TryEndAuthentication methods of the
        /// authentication adapter, and either of these methods throw an ExternalAuthenticationException, the OnError method is called.
        /// This allows your adapter to capture the error and present a nice error message to the customer.
        /// </summary>
        /// <param name="request">The request object containing HTTP information about the client.</param>
        /// <param name="ex">The exception containing description of the error.</param>
        /// <returns>The Adapter Presentation to be shown to the user, describing the error.</returns>
        public IAdapterPresentation OnError(HttpListenerRequest request, ExternalAuthenticationException ex)
        {
            return new AdapterPresentation(ex.Message, true);
        }

        /// <summary>
        /// This method is called by AD FS when the Authentication Adapter should perform the actual authentication.
        /// It will pass the IAuthenticationContext to the method, which we have seen before.
        /// </summary>
        /// <param name="context">The authentication context.</param>
        /// <param name="proofData">This is a dictionary of strings to objects, that represents whatever you have asked the customer
        /// for during the BeginAuthentication method.</param>
        /// <param name="request">The request object containing HTTP information about the client.</param>
        /// <param name="claims">The Claims to be added to the user authentication process.</param>
        /// <returns></returns>
        public IAdapterPresentation TryEndAuthentication(IAuthenticationContext context, IProofData proofData, HttpListenerRequest request, out Claim[] claims)
        {
            claims = null;

            string userid = (string)context.Data["userid"];
            string hostname = GetHostName(request.RemoteEndPoint.Address.ToString());
            
            try
            {
                List<string> hosts = databaseConnection.GetAllowedHosts(userid);
                if (!hosts.Contains(hostname))
                {
                    return new AdapterPresentation(AdapterMessages.USER_UNREGISTERED, false);
                }
                
                Claim claim = new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2012/12/authmethod/otp");
                claims = new Claim[] { claim };
                return null;
            }
            catch (DatabaseException ex)
            {
                return new AdapterPresentation(ex.Message, false);
            }
        }
    }
}
