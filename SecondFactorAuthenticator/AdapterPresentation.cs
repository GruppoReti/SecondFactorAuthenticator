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
            return AdapterMessages.getMessage(AdapterMessages.PAGE_TITLE, lcid);
        }

        public string GetFormHtml(int lcid)
        {
            if (!this.isPermanentFailure)
            {
                if (String.IsNullOrEmpty(this.message))
                {
                    string html = AdapterMessages.getMessage("PageHtmlForm", lcid);
                    return html;
                }
                else
                {
                    string localizedMessage = AdapterMessages.getMessage(this.message, lcid);
                    string html = AdapterMessages.getMessage("PageHtmlMessage", lcid);
                    return string.Format(html, localizedMessage);
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
