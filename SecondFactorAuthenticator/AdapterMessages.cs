using System;
using System.Globalization;

namespace SecondFactorAuthenticator
{
    static class AdapterMessages
    {
        public readonly static int defaultLcid = new CultureInfo("en-us").LCID;

        public readonly static string UNSUPPORTED_DB = "UNSUPPORTED_DB";
        public readonly static string WRONG_DB = "WRONG_DB";
        public readonly static string USER_UNKNOWN = "USER_UNKNOWN";
        public readonly static string USER_UNREGISTERED = "USER_UNREGISTERED";
        public readonly static string ADAPTER_DESCRIPTION = "USER_UNREGISTERED";
        public readonly static string PAGE_TITLE = "PAGE_TITLE";

        public static string getMessage(string MessageId, int lcid)
        {
            try
            {
                string message = Resources.Messages.ResourceManager.GetString(MessageId, new CultureInfo(lcid));
                if (message == null) throw new Exception("Message is null in resource file.");
                return message;
            }
            catch
            {
                return MessageId;
            }
        }
    }
}
