using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace HiringOperations.Models
{
    public class StudentModel
    {

        public int Sno { get; set; }
        public string Hall_ticket_no { get; set; }
        public string Name_of_the_student { get; set; }
        public string Emailid { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public long PH_No { get; set; }
        public long Aadhar_no { get; set; }
        public string School_Name { get; set; }
        public int ssc_Year_of_Pass_out { get; set; }
        public string Ssc_Aggregate { get; set; }
        public string Junior_College_Name { get; set; }
        public int inter_Year_of_Pass_out { get; set; }
        public string inter_Aggregate { get; set; }
        public string Engineering_College_Name { get; set; }
        public string Branch { get; set; }
        public int Btech_Year_of_Pass_out { get; set; }
        public int Total_backlogs { get; set; }
        public string Graduation_Aggregate { get; set; }
        public string Fathers_name { get; set; }
        public string Fathers_occupation { get; set; }
        public string Permanent_address { get; set; }
        public string Present_address { get; set; }
        public string Fathers_Mobile_No { get; set; }
        public string Mothers_Name { get; set; }
        public string Mothers_Occupation { get; set; }
        public string Resume_Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string Status { get; set; }

    }
}
