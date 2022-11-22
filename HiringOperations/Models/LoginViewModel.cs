using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace HiringOperations.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "what's your Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
       ErrorMessage = "Invalid email format")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "what's your Password")]
        //[RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{5,}", ErrorMessage = "Your password must be at least 5 characters long and contain at least 1 letter and 1 number")]
        public string Password { get; set; }
    }
}
