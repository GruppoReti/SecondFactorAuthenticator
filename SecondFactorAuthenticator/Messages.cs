using System.Globalization;
using System.Collections.Generic;

namespace SecondFactorAuthenticator
{
    static class Messages
    {
        public readonly static int defaultLcid = new CultureInfo("en-us").LCID;

        public readonly static string UNSUPPORTED_DB = "UNSUPPORTED_DB";
        public readonly static string WRONG_DB = "WRONG_DB";
        public readonly static string USER_UNKNOWN = "USER_UNKNOWN";
        public readonly static string USER_UNREGISTERED = "USER_UNREGISTERED";

        private readonly static Dictionary<string, Dictionary<int, string>> messages = new Dictionary<string, Dictionary<int, string>>()
        {
            {
                "UNSUPPORTED_DB", new Dictionary<int, string> {
                    { new CultureInfo("en-us").LCID, "Unsupported database type in configuration." },
                    { new CultureInfo("it").LCID, "Tipo di database nella configurazione non supportato." }
                }
            },
            {
                "WRONG_DB", new Dictionary<int, string> {
                    { new CultureInfo("en-us").LCID, "Wrong database connection string in configuration." },
                    { new CultureInfo("it").LCID, "String di connessione al database non corretta nella configurazione." }
                }
            },
            {
                "USER_UNKNOWN", new Dictionary<int, string> {
                    { new CultureInfo("en-us").LCID, "Unknown user in database." },
                    { new CultureInfo("it").LCID, "Utente sconosciuto nel database." }
                }
            },
            {
                "USER_UNREGISTERED", new Dictionary<int, string> {
                    { new CultureInfo("en-us").LCID, "User trying to access from client not registerd into database." },
                    { new CultureInfo("it").LCID, "L'utente sta effettuando l'accesso da un client che non &egrave; tra quelli registrati nel database." }
                }
            }
        };

        public static string getMessage(string MessageId, int lcid)
        {
            if (messages.ContainsKey(MessageId))
            {
                return messages[MessageId][lcid] ?? messages[MessageId][defaultLcid];
            }
            else
            {
                return MessageId;
            }
        }
    }
}
