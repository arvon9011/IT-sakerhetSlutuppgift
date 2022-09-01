using PillMedTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillMedTech.Models
{
  public interface IPillMedTechRepository
  {
    //Tabeller i databasen
    IQueryable<Employee> Employees { get; }
    IQueryable<SickErrand> SickErrands { get; }
    IQueryable<Logger> Loggers { get; }

    //Sökning av anställds sjukskrivning
        List<SickErrand> SortedErrands(string employeeId);


    //Sjukskrivningar
    void ReportVAB(SickErrand errand);
    void ReportSickDay();
    void ReportSick(SickErrand errand);

        void LoggActivity(Logger log);

        List<Logger> decryptedLoggs();

  }
}
