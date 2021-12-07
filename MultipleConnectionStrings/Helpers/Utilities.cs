using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MultipleConnectionStrings.Helpers
{
    static public class Utilities
    {
        public static SqlConnection GetStringConnection(string databaseId, DbContext _context)
        {
            Regex r = new Regex("Database=[^;]*");
            string newDB = $"Database={databaseId}";
            string cs = _context.Database.GetDbConnection().ConnectionString;
            string newCS = r.Replace(cs, newDB);
            return new SqlConnection(newCS);
        }

        static public void SetContext(this DbContext _context, ClaimsIdentity user)
        {
            string clientCode = UserClaim(user, "clientDatabase");
            _context.Database.SetDbConnection(Utilities.GetStringConnection(clientCode, _context));
        }

        public static string UserClaim(ClaimsIdentity identity, string claimInfo)
        {
            string userClaim = identity.Claims.FirstOrDefault(p => p.Type == claimInfo)?.Value;

            return userClaim;
        }

    }
}
