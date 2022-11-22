using Microsoft.AspNetCore.Mvc;
using HiringOperations.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Configuration;

using HiringOperations.BussinessLogic;
namespace HiringOperations.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel obj)
        {
            if (ModelState.IsValid)
            {
                DataTable dt = new DataTable();
                dt = LoginBl.login(obj);
                if (dt.Rows.Count > 0)
                {

                    HttpContext.Session.SetString("UserName", dt.Rows[0]["EmailID"].ToString());
                    HttpContext.Session.SetString("LoginName", dt.Rows[0]["FirstName"].ToString());
                    //HttpContext.Session.SetString("LoginName", dt.Rows[0]["FirstName"].ToString());
                    HttpContext.Session.SetString("Time", System.DateTime.Now.ToShortTimeString());
                    if (dt.Rows[0]["Role"].ToString() == "CEO")
                    {




                        return RedirectToAction("AdminHome", "Login");



                    }
                    if (dt.Rows[0]["Role"].ToString() != "Admin")
                    {
                        return RedirectToAction("TrainerHome", "Login");
                    }



                }
                else
                {
                    ViewBag.Message = String.Format("Your Emailid or Password is Incorrect");
                    return View();
                }



            }



            else
            {
                ViewBag.Message = String.Format("Your Emailid or Password is Incorrect");
                return View();
            }




            return View();


        }

        public IActionResult AdminHome()
        {
            return View();
        }


        public IActionResult TrainerHome()
        {
            return View();
        }
        [HttpGet]

        public IActionResult AddNewUsers()
        {
            return View();
        }
        [HttpPost]

        public IActionResult AddNewUsers(AddnewUserModel obj)
        {
            if (ModelState.IsValid)
            {
                //bool res = LoginBl.insertdata(obj);
                
                
                    //return RedirectToAction("Login", "Login");
                    bool res = LoginBl.insertdata(obj);
                    if (res)
                    {
                        ViewBag.Message = "Your Data Inserted Sucessfully..!!";
                    }
                    else
                    {
                        ViewBag.Message = "Data Insertion Failed";
                    }
                    return View(obj);               

            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddNewStudents()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewStudents(List<IFormFile> PostedFiles, StudentModel obj)
        {
            foreach (IFormFile PostedFile in PostedFiles)
            {
                string fileName = Path.GetFileName(PostedFile.FileName);
                string type = PostedFile.ContentType;
                byte[] bytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    PostedFile.CopyTo(ms);
                    bytes = ms.ToArray();
                }
                var dbconfig = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
                string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
                using (SqlConnection con = new SqlConnection(dbconnectionstr))
                {
                    SqlCommand cmd = new SqlCommand("sp_hiringSTD", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Hall_ticket_no", obj.Hall_ticket_no);
                    cmd.Parameters.AddWithValue("@Name_of_the_student", obj.Name_of_the_student);
                    cmd.Parameters.AddWithValue("@Emailid", obj.Emailid);
                    cmd.Parameters.AddWithValue("@Dob", Convert.ToDateTime(obj.Dob));
                    cmd.Parameters.AddWithValue("@Gender", obj.Gender);
                    cmd.Parameters.AddWithValue("@PH_No", Convert.ToInt64(obj.PH_No));
                    cmd.Parameters.AddWithValue("@Aadhar_no", Convert.ToInt64(obj.Aadhar_no));
                    cmd.Parameters.AddWithValue("@School_Name", obj.School_Name);
                    cmd.Parameters.AddWithValue("@ssc_Year_of_Pass_out", Convert.ToInt32(obj.ssc_Year_of_Pass_out));
                    cmd.Parameters.AddWithValue("@Ssc_Aggregate", obj.Ssc_Aggregate);
                    cmd.Parameters.AddWithValue("@Junior_College_Name", obj.Junior_College_Name);
                    cmd.Parameters.AddWithValue("@inter_Year_of_Pass_out", Convert.ToInt32(obj.inter_Year_of_Pass_out));
                    cmd.Parameters.AddWithValue("@inter_Aggregate", obj.inter_Aggregate);
                    cmd.Parameters.AddWithValue("@Engineering_College_Name", obj.Engineering_College_Name);
                    cmd.Parameters.AddWithValue("@Branch", obj.Branch);
                    cmd.Parameters.AddWithValue("@Btech_Year_of_Pass_out", Convert.ToInt32(obj.Btech_Year_of_Pass_out));

                    cmd.Parameters.AddWithValue("@Total_backlogs", Convert.ToInt32(obj.Total_backlogs));
                    cmd.Parameters.AddWithValue("@Graduation_Aggregate", obj.Graduation_Aggregate);
                    cmd.Parameters.AddWithValue("@Fathers_name", obj.Fathers_name);
                    cmd.Parameters.AddWithValue("@Fathers_occupation", obj.Fathers_occupation);
                    cmd.Parameters.AddWithValue("@Permanent_address", obj.Permanent_address);
                    cmd.Parameters.AddWithValue("@Present_address", obj.Present_address);
                    cmd.Parameters.AddWithValue("@Fathers_Mobile_No", obj.Fathers_Mobile_No);
                    cmd.Parameters.AddWithValue("@Mothers_Name", obj.Mothers_Name);
                    cmd.Parameters.AddWithValue("@Mothers_Occupation", obj.Mothers_Occupation);
                    cmd.Parameters.AddWithValue("@Name", fileName);
                    cmd.Parameters.AddWithValue("@ContentType", type);
                    cmd.Parameters.AddWithValue("@Data", bytes);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        [HttpGet]
        public IActionResult DisplayStudents()
        {
            return View(LoginBl.GetALlStudentData());
        }
        [HttpPost]
        public IActionResult DisplayStudents(string Hall_ticket_no, string REMARKS, int SCORE, string To)
        {

            string LoginName = HttpContext.Session.GetString("LoginName");
            string Status = "";
            //string To = "";
            string Result = "";
            if (SCORE >= 60)
            {
                Status = "Waiting For L2 Interview";
                Result = "Dear Student,Congratulations you are Selected In First Round";
              
            }
            else
            {
                Result = "Dear Student, you are Rejected In First Round";
                Status = "L1 Rejected";
            }

            var dbconfig = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_interview_records_std", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hall_ticket_no", Hall_ticket_no);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@SCORE", SCORE);
                cmd.Parameters.AddWithValue("@REMARKS", REMARKS);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress("divyabharathi.bathina@gmail.com");
                mail.Subject = "Interview Result";
                string Body = Result;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("divyabharathi.bathina@gmail.com", "tpjezobsppxznvpp"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                int x = cmd.ExecuteNonQuery();

                con.Close();
                if (x > 0)
                {
                    return RedirectToAction("L2Module", "Login");
                }
                return View();
            }
        }
        public IActionResult DisplayUsers()
        {
            return View(LoginBl.GetALlUserData());
        }
        [HttpGet]
        public ActionResult Model()
        {
            return View(LoginBl.GetALlStudentData());
        }
        //[HttpPost]
        //public ActionResult Model(string StudentName, string Remarks, int Score)
        //{
        //    var dbconfig = new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json").Build();
        //    string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
        //    using (SqlConnection con = new SqlConnection(dbconnectionstr))
        //    {

        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("insert into ModelpopUp values(@StudentName,@Remarks,@Score)", con);
        //        cmd.Parameters.AddWithValue("@StudentName", StudentName);
        //        cmd.Parameters.AddWithValue("@Remarks", Remarks);

        //        cmd.Parameters.AddWithValue("@Score", Score);


        //        int x = cmd.ExecuteNonQuery();
        //        con.Close();
        //        if (x > 0)
        //        {
        //            return RedirectToAction("Login", "Login");
        //        }
        //        return View();
        //    }
        //}

        [HttpGet]
        public IActionResult Edit(int? userid)
        {
            return View(LoginBl.GetDataByID((int)userid));
        }
        [HttpPost]
       
        public IActionResult Edit(AddnewUserModel obj)
        {
            if (ModelState.IsValid)
            {
                bool res = LoginBl.Updatedata(obj);

                if (res == true)
                {
                    return RedirectToAction("DisplayUsers", "login");
                }
                else
                {
                    return View(obj);
                }


            }
            return View();

        }
        [HttpGet]
        public IActionResult Delete(int? userid)
        {
            bool res = LoginBl.Deletedata((int)userid);
            if (res == true)
            {
                return RedirectToAction("DisplayUsers");
            }
            else
            {
                return View();
            }
            return View();

        }
        [HttpGet]
        public IActionResult L2Module()
        {

            return View(LoginBl.GetALlStudentData2());
        }
        [HttpPost]
        public IActionResult L2Module(string Hall_ticket_no, string REMARKS, int SCORE, string To)
        {

            string LoginName = HttpContext.Session.GetString("LoginName");
            string Status = "";
            //string To = "";
            string Result = "";
            if (SCORE >= 60)
            {
                Status = "Waiting For L3 Interview";
                Result = "Dear Student,Congratulations you are Selected In Second Round";

            }
            else
            {
                Result = "Dear Student, you are Rejected In Second Round";
                Status = "L2 Rejected";
            }

            var dbconfig = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_L2_records_std", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hall_ticket_no", Hall_ticket_no);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@SCORE", SCORE);
                cmd.Parameters.AddWithValue("@REMARKS", REMARKS);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress("divyabharathi.bathina@gmail.com");
                mail.Subject = "Interview Result";
                string Body = Result;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("divyabharathi.bathina@gmail.com", "tpjezobsppxznvpp"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                int x = cmd.ExecuteNonQuery();

                con.Close();
                if (x > 0)
                {
                    return RedirectToAction("L3Module", "Login");
                }
                return View();
            }


        }
        [HttpGet]
        public IActionResult L3Module()
        {

            return View(LoginBl.GetALlStudentData3());
        }
        [HttpPost]
        public IActionResult L3Module(string Hall_ticket_no, string REMARKS, int SCORE, string To)
        {

            string LoginName = HttpContext.Session.GetString("LoginName");
            string Status = "";
            //string To = "";
            string Result = "";
            if (SCORE >= 60)
            {
                Status = "Waiting For Onboarding";
                Result = "Dear Student,Congratulations you are Selected In Third Round";

            }
            else
            {
                Result = "Dear Student, you are Rejected In Third Round";
                Status = "Final Round Rejected";
            }

            var dbconfig = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json").Build();
            string dbconnectionstr = dbconfig["ConnectionStrings:DefaultConnection"];
            using (SqlConnection con = new SqlConnection(dbconnectionstr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_L3_records_std", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Hall_ticket_no", Hall_ticket_no);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@SCORE", SCORE);
                cmd.Parameters.AddWithValue("@REMARKS", REMARKS);
                cmd.Parameters.AddWithValue("@LoginName", LoginName);
                MailMessage mail = new MailMessage();
                mail.To.Add(To);
                mail.From = new MailAddress("divyabharathi.bathina@gmail.com");
                mail.Subject = "Interview Result";
                string Body = Result;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("divyabharathi.bathina@gmail.com", "tpjezobsppxznvpp"); // Enter seders User name and password       
                smtp.EnableSsl = true;
                smtp.Send(mail);
                int x = cmd.ExecuteNonQuery();

                con.Close();
                if (x > 0)
                {
                    return RedirectToAction("L3Module", "Login");
                }
                return View();
            }


        }




    }
}



