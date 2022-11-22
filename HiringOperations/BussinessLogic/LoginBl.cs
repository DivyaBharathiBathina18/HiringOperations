using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using HiringOperations.Models;
namespace HiringOperations.BussinessLogic
{
    public class LoginBl
    {
        public static DataTable login(LoginViewModel obj)
        {
            var dconfigure = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dconfigure["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlCommand cmd = new SqlCommand("select * from adduser where Emailid=@Emailid AND Password=@Password", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Emailid", obj.EmailId);
                cmd.Parameters.AddWithValue("@Password", obj.Password);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }

        }
        public static bool insertdata(AddnewUserModel obj)
        {
            bool res = false;
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("insert into adduser(FirstName,LastName,EmailId,Password,Gender,DOB,Role,Status)values(@FirstName,@LastName,@EmailId,@Password,@Gender,@DOB,@Role,@Status)", con);
                    // cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                    cmd.Parameters.AddWithValue("@EmailId", obj.EmailId);
                    cmd.Parameters.AddWithValue("@Password", obj.Password);
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(obj.DOB));
                    cmd.Parameters.AddWithValue("@Role", obj.Role);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }
                }
                catch (Exception ex)
                {



                }
                finally
                {
                    con.Close();
                }
                return res = true;
            }
        }

        public static List<StudentModel> GetALlStudentData()
        {
            List<StudentModel> obj = new List<StudentModel>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from HiringSTD", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new StudentModel
                        {



                            Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
                            Name_of_the_student = dr["Name_of_the_student"].ToString(),
                            Emailid = dr["Emailid"].ToString(),
                             PH_No= Convert.ToInt64(dr["PH_No"].ToString()),
                            Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
                            Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
                            Btech_Year_of_Pass_out =Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
                            Status= dr["Status"].ToString(),
                        }
                        );
                }
                return obj;
            }
        }

        public static List<AddnewUserModel> GetALlUserData()
        {
            List<AddnewUserModel> obj = new List<AddnewUserModel>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select*from adduser", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new AddnewUserModel
                        {


                            userid = Convert.ToInt32(dr["userid"].ToString()),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            EmailId = dr["EmailId"].ToString(),
                            Password = dr["Password"].ToString(),
                            Gender = dr["Gender"].ToString(),
                            DOB = Convert.ToDateTime(dr["DOB"].ToString()),
                            Role = dr["Role"].ToString(),
                          Status = Convert.ToBoolean(dr["Status"].ToString())



                        }
                        );
                }
                return obj;
            }
        }

        public static AddnewUserModel GetDataByID(int userid)
        {


            AddnewUserModel obj = null;
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlCommand cmd = new SqlCommand("getDataById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);



                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    obj = new AddnewUserModel();



                    obj.userid = Convert.ToInt32(ds.Tables[0].Rows[i]["userid"].ToString());
                    obj.FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString();
                    obj.LastName = ds.Tables[0].Rows[i]["LastName"].ToString();
                    obj.EmailId = ds.Tables[0].Rows[i]["EmailId"].ToString();
                    obj.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                    obj.DOB = Convert.ToDateTime(ds.Tables[0].Rows[i]["DOB"].ToString());
                    obj.Role = ds.Tables[0].Rows[i]["Role"].ToString();
                    obj.Gender = ds.Tables[0].Rows[i]["Gender"].ToString();

                    obj.Status = Convert.ToBoolean(ds.Tables[0].Rows[i]["Status"].ToString());






                }
                return obj;




            }
        }

        public static bool Updatedata(AddnewUserModel obj)
        {
            bool res = false;
            var dbconfig = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("UpdateData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FirstName", obj.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", obj.LastName);
                    cmd.Parameters.AddWithValue("@EmailId", obj.EmailId);
                    cmd.Parameters.AddWithValue("@Password", obj.Password);
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                    cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(obj.DOB));
                    cmd.Parameters.AddWithValue("@Role", obj.Role);
                    cmd.Parameters.AddWithValue("@Status", obj.Status);
                    cmd.Parameters.AddWithValue("@userid", obj.userid);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }
                }
                catch (Exception ex)
                {



                }
                finally
                {
                    con.Close();
                }
                return res = true;
            }
        }

        public static bool Deletedata(int userid)
        {
            bool res = false;
            var dconfigure = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dconfigure["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DeleteData", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userid", userid);
                    int x = cmd.ExecuteNonQuery();
                    if (x > 0)
                    {
                        return res = true;
                    }
                    else
                    {
                        return res = false;
                    }

                }
                catch (Exception)
                {

                }
                finally
                {
                    con.Close();
                }
                return res = true;

            }
        }

        public static List<StudentModel> GetALlStudentData2()
        {
            List<StudentModel> obj = new List<StudentModel>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from dummyL1 where Status in ('Waiting For L2 Interview','Waiting For L3 Interview')", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new StudentModel
                        {



                            Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
                            Name_of_the_student = dr["Name_of_the_student"].ToString(),
                            Emailid = dr["Emailid"].ToString(),
                            PH_No = Convert.ToInt64(dr["PH_No"].ToString()),
                            Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
                            Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
                            Btech_Year_of_Pass_out = Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
                            Status = dr["Status"].ToString(),
                        }
                        );
                }
                return obj;
            }
        }

        public static List<StudentModel> GetALlStudentData3()
        {
            List<StudentModel> obj = new List<StudentModel>();
            var dbconfig = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from dummyL2 where Status in ('Waiting For L3 Interview','Waiting For Onboarding')", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    obj.Add(
                        new StudentModel
                        {



                            Hall_ticket_no = dr["Hall_ticket_no"].ToString(),
                            Name_of_the_student = dr["Name_of_the_student"].ToString(),
                            Emailid = dr["Emailid"].ToString(),
                            PH_No = Convert.ToInt64(dr["PH_No"].ToString()),
                            Engineering_College_Name = dr["Engineering_College_Name"].ToString(),
                            Total_backlogs = Convert.ToInt32(dr["Total_backlogs"].ToString()),
                            Btech_Year_of_Pass_out = Convert.ToInt32(dr["Btech_Year_of_Pass_out"].ToString()),
                            Status = dr["Status"].ToString(),
                        }
                        );
                }
                return obj;
            }
        }
    }
}
