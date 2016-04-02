using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;
using System.Security.Cryptography;

namespace SecondFactorAuthenticator.Database
{
    class JsonDatabase : IDatabase
    {
        public static readonly string ID = "JSON";
        string jsonString = null;
        string fileHash = null;

        /// <summary>
        /// Method that starts the database connection verifying proper configuration.
        /// </summary>
        public void StartConnection()
        {
            if (Resources.Database.ConnectionString == null)
            {
                throw new DatabaseException(AdapterMessages.WRONG_DB);
            }


            if (!File.Exists(Resources.Database.ConnectionString))
            {
                throw new DatabaseException(AdapterMessages.WRONG_DB);
            }

            readJsonFile(Resources.Database.ConnectionString);
        }

        private void readJsonFile(string filename)
        {
            jsonString = File.ReadAllText(filename);
            fileHash = computeFileHash(filename);
        }

        private string computeFileHash(string filename)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var buffer = md5.ComputeHash(File.ReadAllBytes(filename));
                var sb = new StringBuilder();
                for (var i = 0; i < buffer.Length; i++)
                {
                    sb.Append(buffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Method that returns a list of allowed hosts for the specified user.
        /// </summary>
        /// <param name="username">The username to search in the database.</param>
        /// <returns>A list of hosts allowed for the specified user.</returns>
        public List<string> GetAllowedHosts(string username)
        {
            // Check if the file changed, in case reload JSON file.
            if (fileHash != computeFileHash(Resources.Database.ConnectionString))
            {
                readJsonFile(Resources.Database.ConnectionString);
            }

            // Parse the file to retrieve the list of hosts associated to the user.
            Dictionary<string, List<string>> values = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(jsonString);
            if (!values.ContainsKey(username))
            {
                throw new DatabaseException(AdapterMessages.USER_UNKNOWN);
            }
            
            return values[username];
        }

        /// <summary>
        /// Method that closes all connections to the database.
        /// </summary>
        public void CloseConnection()
        {
            jsonString = null;
            fileHash = null;
        }
    }
}
