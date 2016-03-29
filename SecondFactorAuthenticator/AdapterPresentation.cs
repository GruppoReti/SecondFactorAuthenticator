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

        /// <summary>
        /// Method to retrieve the page title to be shown to the user for authentication.
        /// </summary>
        /// <param name="lcid">The localization ID of the culture to be used.</param>
        /// <returns>The page title.</returns>
        public string GetPageTitle(int lcid)
        {
            return AdapterMessages.getMessage(AdapterMessages.PAGE_TITLE, lcid);
        }

        /// <summary>
        /// Method to retrieve HTML of the form to be shown to the user for second factor authentication.
        /// The method should return the string, plain old HTML, that represents the code for whatever you want to do in
        /// the sing-in page.
        /// </summary>
        /// <param name="lcid">The localization ID of the culture to be used.</param>
        /// <returns>String containing the HTML page.</returns>
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

        /// <summary>
        /// This method is used to allow the Authentication Adapter to insert any special tags etc. in the <head> element of the
        /// AD FS sign-in page. Again, with the same lcid value to localize your pages.
        /// The method should return the HTML code you want to insert in the HTML <head> element of AD FS the sign-in page.
        /// </summary>
        /// <param name="lcid">The localization ID of the culture to be used.</param>
        /// <returns></returns>
        public string GetFormPreRenderHtml(int lcid)
        {
            return string.Empty;
        }

        /// <summary>
        /// Constructor for the class.
        /// </summary>
        /// <param name="message">Eventual message to be shown to the user.</param>
        /// <param name="isPermanentFailure">Flag indicating whether the failure is permanent or the user can try to authenticate again.</param>
        public AdapterPresentation(string message = null, bool isPermanentFailure = false)
        {
            this.message = message ?? String.Empty;
            this.isPermanentFailure = isPermanentFailure;
        }
    }
}
