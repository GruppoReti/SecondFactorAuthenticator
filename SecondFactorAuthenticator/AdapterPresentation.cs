using System;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.IdentityServer.Web.Authentication.External;

namespace SecondFactorAuthenticator
{
    class AdapterPresentation : IAdapterPresentation, IAdapterPresentationForm
    {
        private readonly string message;
        private readonly bool isPermanentFailure;

        public string GetPageTitle(int lcid)
        {
            Dictionary<int, string> result = new Dictionary<int, string>() {
                { new CultureInfo("en-us").LCID, "Check of the association user - hostname" },
                { new CultureInfo("it").LCID, "Controllo associazione utente - hostname" }
            };
            return result[lcid] ?? result[Messages.defaultLcid];
        }

        public string GetFormHtml(int lcid)
        {
            // TODO: support multilanguage for these 2 HTML pages
            if (!this.isPermanentFailure)
            {
                if (String.IsNullOrEmpty(this.message))
                {
                    return Resources.PageHtmlForm;
                }
                else
                {
                    string localizedMessage = Messages.getMessage(this.message, lcid);
                    return string.Format(Resources.PageHtmlMessage, localizedMessage);
                }
            }

            return String.Empty;
        }

        public string GetFormPreRenderHtml(int lcid)
        {
            return string.Empty;
        }

        public AdapterPresentation(string message = null, bool isPermanentFailure = false)
        {
            this.message = message ?? String.Empty;
            this.isPermanentFailure = isPermanentFailure;
        }
    }
}
