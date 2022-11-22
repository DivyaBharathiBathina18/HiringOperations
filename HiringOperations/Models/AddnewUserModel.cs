using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace HiringOperations.Models
{
    public class AddnewUserModel
    {
        public int userid { get; set; }

        [Required(ErrorMessage = "what's your FirstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "what's your LastName")]

        public string LastName { get; set; }

        [Required(ErrorMessage = "what's your Email")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "what's your Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please select Gender")]

        public string Gender { get; set; }



        [Required(ErrorMessage = "Please select DOB")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Please select Role")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Please select Status")]

        public bool Status { get; set; }
    }
}
