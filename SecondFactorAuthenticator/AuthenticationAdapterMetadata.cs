using System.Globalization;
using System.Collections.Generic;
using Microsoft.IdentityServer.Web.Authentication.External;

namespace SecondFactorAuthenticator
{
    class AuthenticationAdapterMetadata : IAuthenticationAdapterMetadata
    {
        public string AdminName
        {
            get { return "User-hostname association"; }
        }

        public string[] AuthenticationMethods
        {
            get { return new string[] { "http://schemas.microsoft.com/ws/2012/12/authmethod/otp" }; }
        }

        public int[] AvailableLcids
        {
            get { return new int[] { new CultureInfo("en-us").LCID, new CultureInfo("it").LCID }; }
        }

        public Dictionary<int, string> Descriptions
        {
            get
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                result.Add(new CultureInfo("en-us").LCID, "Authentication based on user-hostname association");
                result.Add(new CultureInfo("it").LCID, "Autenticazione basata sull'associazione utente-hostname");
                return result;
            }
        }

        public Dictionary<int, string> FriendlyNames
        {
            get
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                result.Add(new CultureInfo("en-us").LCID, "Authentication based on user-hostname association");
                result.Add(new CultureInfo("it").LCID, "Autenticazione basata sull'associazione utente-hostname");
                return result;
            }
        }

        /// Returns an array indicating the type of claim that that the adapter uses to identify the user being authenticated.
        /// Note that although the property is an array, only the first element is currently used.
        /// MUST BE ONE OF THE FOLLOWING
        /// "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname"
        /// "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn"
        /// "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        /// "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"
        public string[] IdentityClaims
        {
            get { return new string[] { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn" }; }
        }

        public bool RequiresIdentity
        {
            get { return true; }
        }
        public AuthenticationAdapterMetadata()
        {

        }
    }
}
