using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondFactorAuthenticator.Database
{
    interface IDatabase
    {
        /// <summary>
        /// Method that starts the database connection verifying proper configuration
        /// </summary>
        void StartConnection();

        /// <summary>
        /// Method that returns a list of allowed hosts for the specified user.
        /// </summary>
        /// <param name="username">The username to search in the database.</param>
        /// <returns>A list of hosts allowed for the specified user.</returns>
        List<string> GetAllowedHosts(string username);

        /// <summary>
        /// Method that closes all connections to the database.
        /// </summary>
        void CloseConnection();
    }
}
