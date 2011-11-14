using System.ComponentModel.DataAnnotations;

namespace TimeTracking.Models
{
    public class SignInViewModel
    {
        [Required(ErrorMessage="Unesite korisnicko ime")]
        public string UserName { get; set; }
        [Required(ErrorMessage="Unesite sifru")]
        public string Password { get; set; }
    }
}
