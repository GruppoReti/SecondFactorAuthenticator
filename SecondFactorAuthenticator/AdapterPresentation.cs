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
        private readonly static int defaultLcid = new CultureInfo("en-us").LCID;

        public string GetPageTitle(int lcid)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            result.Add(new CultureInfo("en-us").LCID, "Check of the association user - hostname");
            result.Add(new CultureInfo("it").LCID, "Controllo associazione utente - hostname");
            return result[lcid] ?? result[defaultLcid];
        }

        public string GetFormHtml(int lcid)
        {
            // TODO: support multilanguage for these 2 HTML pages
            if (!this.isPermanentFailure)
            {
                if (string.IsNullOrEmpty(message))
                {
                    return Resources.PageHtmlForm;
                }
                else
                {
                    return string.Format(Resources.PageHtmlMessage, this.message);
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
            this.message = message ?? string.Empty;
            this.isPermanentFailure = isPermanentFailure;
        }
    }
}
