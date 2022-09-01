using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using PillMedTech.Models;

namespace PillMedTech.Models
{
    public class EFPillMedTechRepository : IPillMedTechRepository
    {
        private ApplicationDbContext appContext;
        private LoggingDbContext appLogger;
        private IHttpContextAccessor contextAcc;
        IDataProtector protector;
        private int tempnmr = 0;



        public EFPillMedTechRepository(ApplicationDbContext ctx, IHttpContextAccessor cont, LoggingDbContext lo, IDataProtectionProvider provider)
        {
            appContext = ctx;
            contextAcc = cont;
            appLogger = lo;
            protector = provider.CreateProtector("temp");
        }

        public IQueryable<Employee> Employees => appContext.Employees;
        public IQueryable<SickErrand> SickErrands => appContext.SickErrands;
        public IQueryable<Logger> Loggers => appLogger.Loggers;

        //Går igenom listan för att hitta ärenden gällande en specifik anställd
        //1.1. Jobba med ORM och LINQ för automatiserad parameterisering
       // 3.1. Jobba med Linq mot databasen(se 1.1)
        public List<SickErrand> SortedErrands(string employeeId)
        {
            var currentEmp = SickErrands.Where(emp => emp.EmployeeID.Equals(employeeId)).FirstOrDefault();

            List<SickErrand> errands = new List<SickErrand>();

            foreach (SickErrand err in SickErrands)
            {
                if (err.EmployeeID.Equals(employeeId))
                {
                    
                    if ((err.ChildRefNo).Length > 13)
                    {
                        err.ChildRefNo = protector.Unprotect(err.ChildRefNo);
                    }
                    errands.Add(err);
                }
            }
            return errands;
        }


        //1.1. Jobba med ORM och LINQ för automatiserad parameterisering
        // 3.1. Jobba med Linq mot databasen(se 1.1)
        public void ReportVAB(SickErrand errand)
        {
            //7.1.Validera att parametrar har innehåll
            if (!errand.Equals(null))
            {
                if (errand.SickErrandID.Equals(0))
                {
                    DateTime endDate = errand.HomeFrom.AddDays(1);
                    errand.HomeUntil = endDate;
                    errand.TypeOfAbsence = "VAB";
                    // 1.2. Kryptera känslig data
                    errand.ChildRefNo = protector.Protect(errand.ChildRefNo);
                    appContext.SickErrands.Add(errand);
                }
            }
            appContext.SaveChanges();

        }
        //1.1. Jobba med ORM och LINQ för automatiserad parameterisering
        // 3.1. Jobba med Linq mot databasen(se 1.1)
        public void ReportSickDay()
        {
            var user = contextAcc.HttpContext.User.Identity.Name;
            SickErrand errand = new SickErrand { EmployeeID = user, ChildRefNo = "ej aktuellt", HomeFrom = DateTime.Today, TypeOfAbsence = "Sjuk utan intyg" };
            DateTime endDate = errand.HomeFrom.AddDays(1);
            errand.HomeUntil = endDate;
            appContext.SickErrands.Add(errand);

            appContext.SaveChanges();
        }
        //1.1. Jobba med ORM och LINQ för automatiserad parameterisering
        // 3.1. Jobba med Linq mot databasen(se 1.1)
        public void ReportSick(SickErrand errand)
        {
            //7.1.Validera att parametrar har innehåll
            if (!errand.Equals(null))
            {
                if (errand.SickErrandID.Equals(0))
                {
                    errand.ChildRefNo = "ej aktuellt";
                    errand.TypeOfAbsence = "Sjuk med intyg";
                    appContext.SickErrands.Add(errand);
                }
            }
            appContext.SaveChanges();
        }

        // 1.2. Kryptera känslig data
        // 8.3. Kryptera loggningarna
        //MEtoden krypterar loggarna och skickar till databasen
        //1.1. Jobba med ORM och LINQ för automatiserad parameterisering
        // 3.1. Jobba med Linq mot databasen(se 1.1)
        public void LoggActivity(Logger log)
        {
            //7.1.Validera att parametrar har innehåll
            if (!log.Equals(null))
            {
                log.Lognmr = tempnmr;
                tempnmr++;
                log.EmployeeId = protector.Protect(log.EmployeeId);
                log.Time = protector.Protect(log.Time);
                log.Ip = protector.Protect(log.Ip);
                log.Action = protector.Protect(log.Action);

                appLogger.Loggers.Add(log);

            }

            appLogger.SaveChanges();
        }

        // 8.3.1 Avkryptera loggningarna
        //1.1. Jobba med ORM och LINQ för automatiserad parameterisering
        // 3.1. Jobba med Linq mot databasen(se 1.1)
        public List<Logger> decryptedLoggs()
        {

            List<Logger> logger = new List<Logger>();
            foreach (Logger prolog in Loggers)
            {
                Logger unproLog = new Logger();
                unproLog.EmployeeId = protector.Unprotect(prolog.EmployeeId);
                unproLog.Time = protector.Unprotect(prolog.Time);
                unproLog.Ip = protector.Unprotect(prolog.Ip);
                unproLog.Action = protector.Unprotect(prolog.Action);

                logger.Add(unproLog);
            }



            return logger;
        }

    }
}
