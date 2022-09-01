using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PillMedTech.Models
{
  public class SickErrand
  {
    public int SickErrandID { get; set; }
        // 2.2. Validera input med data annotations

        [Display(Name = "Användarid:")]
    [Required(ErrorMessage = "Du måste fylla i ditt anställningsid")]
    public string EmployeeID { get; set; }

    public string TypeOfAbsence { get; set; }

    [Display(Name = "Barnets personnummer:")]
    [Required(ErrorMessage = "Du måste fylla i ditt barns personnummer")]
    public string ChildRefNo { get; set; }

    [Display(Name = "Sjuk från:")]
    [Required(ErrorMessage = "Du måste välja datum när din sjukskrivning börjar")]
    [DataType(DataType.Date)]
    public DateTime HomeFrom { get; set; }

    [Display(Name = "Sjuk till:")]
    [Required(ErrorMessage = "Du måste välja datum när din sjukskrivning börjar")]
    [DataType(DataType.Date)]
    public DateTime HomeUntil { get; set; }

  }
}
