using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PillMedTech.Models
{
  public class DBInitializer
  {
    public static void EnsurePopulated(IServiceProvider services)
    {
      var context = services.GetRequiredService<ApplicationDbContext>();

      if (!context.Employees.Any())
      {
        context.Employees.AddRange(
          new Employee { EmployeeId = "EMP330", EmployeeName = "Ada Öqvist" },
          new Employee { EmployeeId = "EMP430", EmployeeName = "Charlie Jansson" },
          new Employee { EmployeeId = "EMP530", EmployeeName = "Bertil Gustavsson" },
          new Employee { EmployeeId = "HRS270", EmployeeName = "Amelia Gundersson" },
          new Employee { EmployeeId = "ITS980", EmployeeName = "Tove Berg" }
          );
        context.SaveChanges();
      }

      if (!context.SickErrands.Any())
      {
        context.SickErrands.AddRange(
          new SickErrand { EmployeeID = "EMP330", ChildRefNo = "20011208-5696", TypeOfAbsence = "VAB", HomeFrom = new DateTime(2020, 09, 16), HomeUntil = new DateTime(2020, 09, 17) },
          new SickErrand { EmployeeID = "EMP430", ChildRefNo = "ej aktuellt", TypeOfAbsence = "Sjuk med intyg", HomeFrom = new DateTime(2020, 08, 10), HomeUntil = new DateTime(2020, 08, 20) },
          new SickErrand { EmployeeID = "EMP530", ChildRefNo = "ej aktuellt", TypeOfAbsence = "Sjuk utan intyg", HomeFrom = new DateTime(2020, 07, 22), HomeUntil = new DateTime(2020, 07, 23) },
          new SickErrand { EmployeeID = "EMP430", ChildRefNo = "20151212-1512", TypeOfAbsence = "VAB", HomeFrom = new DateTime(2020, 09, 16), HomeUntil = new DateTime(2020, 09, 17) },
          new SickErrand { EmployeeID = "EMP330", ChildRefNo = "20011208-5696", TypeOfAbsence = "VAB", HomeFrom = new DateTime(2020, 06, 15), HomeUntil = new DateTime(2020, 06, 18) },
          new SickErrand { EmployeeID = "EMP530", ChildRefNo = "ej aktuellt", TypeOfAbsence = "Sjuk utan intyg", HomeFrom = new DateTime(2020, 05, 12), HomeUntil = new DateTime(2020, 05, 13) }
          );
        context.SaveChanges();
      }
    }
  }
}
