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

        #region GetData...
        public int GetGroupIDByName(string groupname)
        {
            int id = -1;
            Connection.Open();
            using (SqlCommand command = new SqlCommand(String.Format(@"SELECT GroupId FROM dbo.tbl_Group WHERE Name = '{0}'", groupname), Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["GroupId"]);
                    }
                }
            }
            Connection.Close();

            return id;
        }

        public int GetLastMemberID()
        {
            int id = -1;
            Connection.Open();
            using (SqlCommand command = new SqlCommand(@"SELECT TOP 1 MemberId FROM dbo.tbl_Member ORDER BY Memberid DESC", Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        id = Convert.ToInt32(reader["MemberId"]);
                    }
                }
            }
            Connection.Close();

            return id;
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

        public List<Member> GetAllMember()
        {
            List<Member> MemberFromDatabase = new List<Member>();
            Connection.Open();
            using (SqlCommand command = new SqlCommand(@"SELECT * FROM dbo.tbl_Member;", Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        //TODO: Auslagern
                        Geschlecht gender;
                        if (reader["Gender"].ToString() == Geschlecht.Maennlich.ToString())
                        {
                            gender = Geschlecht.Maennlich;
                        }
                        else
                        {
                            gender = Geschlecht.Weiblich;
                        }
                        Studiengang course;
                        if (reader["Course"].ToString() == Studiengang.Elektrotechnik.ToString())
                        {
                            course = Studiengang.Elektrotechnik;
                        }
                        else if(reader["Course"].ToString() == Studiengang.Informatik.ToString())
                        {
                            course = Studiengang.Informatik;
                        }
                        else if (reader["Course"].ToString() == Studiengang.Wirtschaftsinformatik.ToString())
                        {
                            course = Studiengang.Wirtschaftsinformatik;
                        }
                        else
                        {
                            course = Studiengang.MCD;
                        }
                        Member member = new Member(reader["FirstName"].ToString(), Convert.ToInt32(reader["Age"]), course, gender);
                        MemberFromDatabase.Add(member);
                    }
                }
            }
            Connection.Close();

            return MemberFromDatabase;
        }

        public string GetMembersGroupName(Member ersti)
        {
            string groupname = "";

            Connection.Open();
            using (SqlCommand command = new SqlCommand(String.Format(@"SELECT grp.[Name] FROM tbl_GroupMember AS grpmem
                                                                    JOIN tbl_Group AS grp ON grpmem.GroupId = grp.GroupId
                                                                    WHERE grpmem.MemberId = (SELECT MemberId FROM tbl_Member WHERE FirstName = '{0}' AND 
																                                                                    LastName = '{1}' AND 
																                                                                    Age = {2} AND 
																                                                                    Course ='{3}' AND 
																                                                                    Gender = '{4}');",
                                                                                                                                    ersti.Vorname,
                                                                                                                                    ersti.Name,
                                                                                                                                    ersti.Age,
                                                                                                                                    ersti.Studiengang.ToString(),
                                                                                                                                    ersti.Geschlecht.ToString()), Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groupname = reader["Name"].ToString().Replace(" ", "");
                    }
                }
            }              
            Connection.Close();

            return groupname;
        }
        #endregion

        #region Insert...
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

        /// <summary>
        /// Insert a Member into the database
        /// </summary>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="age"></param>
        /// <param name="gender"></param>
        /// <param name="course"></param>
        /// <returns>Affected rows, should be one by default.</returns>
        public int InsertMember(string firstname, string lastname, int age, Geschlecht gender, Studiengang course)
        {
            Connection.Open();
            int AffectedRows = 0;
            using (SqlCommand command = new SqlCommand(@"INSERT INTO [dbo].[tbl_Member]([FirstName], [LastName], [Age], [Gender], [Course]) VALUES(@firstname,  @lastname, @age, @gender, @course);", Connection))
            {
                command.Parameters.Add("@firstname", SqlDbType.NChar);
                command.Parameters["@firstname"].Value = firstname;
                command.Parameters.Add("@lastname", SqlDbType.NChar);
                command.Parameters["@lastname"].Value = lastname;
                command.Parameters.Add("@age", SqlDbType.Int);
                command.Parameters["@age"].Value = age;
                command.Parameters.Add("@gender", SqlDbType.NChar);
                command.Parameters["@gender"].Value = gender.ToString();
                command.Parameters.Add("@course", SqlDbType.NChar);
                command.Parameters["@course"].Value = course.ToString();

                AffectedRows = command.ExecuteNonQuery();
            }
            Connection.Close();

            return AffectedRows;
        }

        /// <summary>
        /// Assign a Member to a group.
        /// </summary>
        /// <param name="memberid">Database MemberID</param>
        /// <param name="groupid">Database GroupID</param>
        /// <returns></returns>
        public int AssignMemberToGroup(int memberid, int groupid)
        {
            Connection.Open();
            int AffectedRows = 0;
            using (SqlCommand command = new SqlCommand(@"INSERT INTO [dbo].[tbl_GroupMember]([MemberId], [GroupId]) VALUES(@memberid,  @groupid);", Connection))
            {
                command.Parameters.Add("@memberid", SqlDbType.Int);
                command.Parameters["@memberid"].Value = memberid;
                command.Parameters.Add("@groupid", SqlDbType.Int);
                command.Parameters["@groupid"].Value = groupid;

                AffectedRows = command.ExecuteNonQuery();
            }
            Connection.Close();

            return AffectedRows;
        }
        #endregion

        public bool MemberIsAlreadyInDatabase(Member ersti)
        {
            bool IsAlreadyInDarabase = false;

            Connection.Open();
            using (SqlCommand command = new SqlCommand(String.Format(@"SELECT * FROM [dbo].[tbl_Member] WHERE 
								                                                        FirstName = '{0}' AND 
								                                                        LastName = '{1}' AND
								                                                        Age = {2} AND
								                                                        Gender = '{3}' AND
								                                                        Course = '{4}'; ", ersti.Vorname, ersti.Name, ersti.Age, ersti.Geschlecht.ToString(), ersti.Studiengang.ToString()), Connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IsAlreadyInDarabase = true;
                    }
                }
            }
            Connection.Close();

            return IsAlreadyInDarabase;
        }
       
    }
}
