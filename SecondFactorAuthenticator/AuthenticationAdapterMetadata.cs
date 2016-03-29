using System.Globalization;
using System.Collections.Generic;
using Microsoft.IdentityServer.Web.Authentication.External;

namespace SecondFactorAuthenticator
{
    class AuthenticationAdapterMetadata : IAuthenticationAdapterMetadata
    {
        /// <summary>
        /// Name of the module as shown inside AD FS.
        /// </summary>
        public string AdminName
        {
            get { return "User-hostname association"; }
        }

        /// <summary>
        /// Authentication methods supported by this module.
        /// </summary>
        public string[] AuthenticationMethods
        {
            get { return new string[] { "http://schemas.microsoft.com/ws/2012/12/authmethod/otp" }; }
        }

        /// <summary>
        /// Available localiation culture ID for this module.
        /// </summary>
        public int[] AvailableLcids
        {
            get {
                return new int[] {
                    new CultureInfo("en-us").LCID,
                    new CultureInfo("it-it").LCID
                };
            }
        }

        /// <summary>
        /// Dictionary of the descriptions of the module to be shown to the user.
        /// </summary>
        public Dictionary<int, string> Descriptions
        {
            get
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                foreach (int i in AvailableLcids)
                {
                    result.Add(i, AdapterMessages.getMessage(AdapterMessages.ADAPTER_DESCRIPTION, i));
                }
                return result;
            }
        }

        /// <summary>
        /// Dictionary of the friendly names of the module to be shown to the user.
        /// </summary>
        public Dictionary<int, string> FriendlyNames
        {
            get
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                foreach (int i in AvailableLcids)
                {
                    result.Add(i, AdapterMessages.getMessage(AdapterMessages.ADAPTER_DESCRIPTION, i));
                }
                return result;
            }
        }

        /// <summary>
        /// Returns an array indicating the type of claim that that the adapter uses to identify the user being authenticated.
        /// Note that although the property is an array, only the first element is currently used.
        /// MUST BE ONE OF THE FOLLOWING
        /// "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname"
        /// "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn"
        /// "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
        /// "http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid"
        /// </summary>
        public string[] IdentityClaims
        {
            get { return new string[] { "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn" }; }
        }

        /// <summary>
        /// This is an indication whether or not the Authentication Adapter requires an Identity Claim or not.
        /// If you require an Identity Claim, the claim type must be presented through the IdentityClaims property.
        /// </summary>
        public bool RequiresIdentity
        {
            get { return true; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public AuthenticationAdapterMetadata()
        {
            // Do nothing special.
        }
    }
}
