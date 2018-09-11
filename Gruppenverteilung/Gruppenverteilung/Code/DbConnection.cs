using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gruppenverteilung.Code
{
    public class DbConnection
    {
        private string ConnectionString { get; set; }
        private SqlConnection Connection { get; set; } 

        public DbConnection()
        {
            ConnectionString = "Server=tcp:dkoob.database.windows.net,1433;Initial Catalog=StudentDistributorDb;Persist Security Info=False;User ID=Dkoob;Password=Schach100Wasserball;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            Connection = new SqlConnection(ConnectionString);
        }

        public List<Group> GetAllGroups()
        {
            List<Group> GroupsFromDatabase = new List<Group>();
            Connection.Open();
            using(SqlCommand command = new SqlCommand(@"SELECT * FROM dbo.tbl_Group;", Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Group group = new Group(reader["Name"].ToString());
                        group.Room = reader["RoomName"].ToString();
                        GroupsFromDatabase.Add(group);
                    }
                }
            }
            Connection.Close();

            return GroupsFromDatabase;
        }

        /// <summary>
        /// Insert a group into the database
        /// </summary>
        /// <param name="groupname">Name of the group</param>
        /// <param name="roomname">Name of the room</param>
        /// <returns>Affected rows, should be one by default.</returns>
        public int InsertGroup(string groupname, string roomname)
        {
            Connection.Open();
            int AffectedRows = 0;
            using (SqlCommand command = new SqlCommand(@"INSERT INTO [dbo].[tbl_Group]([Name],[RoomName]) VALUES(@grpname, @roomname);", Connection))
            {
                command.Parameters.Add("@grpname", SqlDbType.NChar);
                command.Parameters["@grpname"].Value = groupname;
                command.Parameters.Add("@roomname", SqlDbType.NChar);
                command.Parameters["@roomname"].Value = roomname;
                AffectedRows = command.ExecuteNonQuery();
            }
            Connection.Close();

            return AffectedRows;
        }
    }
}
