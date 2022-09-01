using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PillMedTech.Models
{
  public class LoginModel
  {
        // 2.2. Validera input med data annotations
        [Required(ErrorMessage = "Vänligen fyll i användarnamn")]
    [Display(Name = "Användarnamn:")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Vänligen fyll i lösenord")]
    [UIHint("password")]
    [Display(Name = "Lösenord:")]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
  }
}
