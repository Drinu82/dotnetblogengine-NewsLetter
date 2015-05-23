using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogEngine.Core.Providers
{

    public class MailListProvider 
    {

        string tablePrefix
        {
            get
            {
               return "be_";
            }
        }

        string parmPrefix
        {
            get
            {
              return "@";
            }
        }

        string connStringName
        {
            get { return "BlogEngine"; }
        }
            
        private DbConnectionHelper CreateConnection()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings[this.connStringName];
            return new DbConnectionHelper(settings);
        }

        private string FormatParamName(string parameterName)
        {
            return String.Format("{0}{1}", this.parmPrefix, parameterName);
        }

        /// <summary>
        /// This methog will store the email from news letter to ta email table
        /// </summary>
        /// <param name="blogId">Current blog Id</param>
        /// <param name="email">emailto store</param>
        /// <param name="source">the source of the email eg. newsletter</param>
        public void AddToMailList(string blogId, string email, string source)
        {
            using (var conn = CreateConnection())
            {
                if (conn.HasConnection)
                {
                    var sqlQuery =
                        string.Format(
                            "If not Exists(select 1 from {0}emails where email = {1}Email AND IsActive = 1) begin Insert Into {0}Emails(BlogId, Email, Source, IsActive, Created)Values({1}BlogId, {1}Email, {1}Source,{1}IsActive, {1}Created) end;",
                            this.tablePrefix, this.parmPrefix);

                    using (var cmd = conn.CreateTextCommand(sqlQuery))
                    {
                        cmd.Parameters.Add(conn.CreateParameter(String.Format("{0}BlogId", this.parmPrefix), blogId));
                        cmd.Parameters.Add(conn.CreateParameter(String.Format("{0}Email", this.parmPrefix), email));
                        cmd.Parameters.Add(conn.CreateParameter(String.Format("{0}Source", this.parmPrefix), source));
                        cmd.Parameters.Add(conn.CreateParameter(String.Format("{0}IsActive", this.parmPrefix), 1));
                        cmd.Parameters.Add(conn.CreateParameter(String.Format("{0}Created", this.parmPrefix), DateTime.UtcNow));

                        cmd.ExecuteNonQuery();
                    }
                }
            }

        }
    }
}