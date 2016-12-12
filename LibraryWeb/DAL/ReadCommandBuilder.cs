using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace LibraryWeb
{
    public class ReadCommandBuilder
    {
        public SqlCommand BuildSecureCommand(string commandText, SqlConnection connection, string ordering, params Pair[] pairs)
        {
            if (pairs == null || pairs.Count() == 0)
            {
                return new SqlCommand(String.Format(commandText, ""), connection);
            }

            var aliases = pairs.Select((x, index) => new Pair(x.Key, x.Condition, $"@value{index}")).ToList();
            string whereConditions = String.Concat(" WHERE ", String.Join(" AND ", aliases.Select(a => a.BuildCondition())));
            string updatedCommandText = String.Format("{0}{1}{2}", commandText, whereConditions, ordering);
            SqlCommand rawCommand = new SqlCommand(updatedCommandText, connection);
            for (int i = 0; i < aliases.Count(); i++)
            {
                rawCommand.Parameters.AddWithValue(aliases[i].Value, pairs[i].Value);
            }

            return rawCommand;
        }

        public SqlCommand BuildNotSecureCommand(string commandText, SqlConnection connection, string ordering, params string[] conditions)
        {
            var fileterConditions = conditions.Count() == 0
                ? null : conditions.Where(c => !String.IsNullOrEmpty(c));

            string conditionText = fileterConditions == null || fileterConditions.Count() == 0
                ? "" : String.Concat(" WHERE ", String.Join(" AND ", fileterConditions));

            string updatedCommandText = String.Format("{0}{1}{2}", commandText, conditionText, ordering);
            return new SqlCommand(updatedCommandText, connection);
        }
    }
}