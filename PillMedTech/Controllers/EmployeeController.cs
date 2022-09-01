using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PillMedTech.Models;

namespace PillMedTech.Controllers
{
    //3.2. Jobba med Razor View(@variabelnamn = automatisk html encode för output)
    [Authorize(Roles = "Employee")]
  public class EmployeeController : Controller
  {
    private IPillMedTechRepository repository;
    private IHttpContextAccessor contextAcc;

    //Konstruktor

    public EmployeeController(IPillMedTechRepository repo, IHttpContextAccessor ctxAcc)
    {
      repository = repo;
      contextAcc = ctxAcc;
    }

        //VISAR UPP SIDOR DÄR NÅGOT KAN GÖRAS:
        //Val av sjukskrivning 
  
    public ViewResult StartEmployee()
    {
      return View();
    }

        //Sida för att rapportera in VAB
      
        public ViewResult ReportSickChild()
    {
      return View();
    }

        //Sida för att rapportera in sjukskrivning med intyg
       
        public ViewResult ReportSick()
    {
      return View();
    }

        //Tack-sidan när rapporteringen sparats
       
        public ViewResult ThankYou()
    {
      return View();
    }



    //HANTERING AV SJUKSKRIVNING

    //Hantera VAB
    [HttpPost]
    public ViewResult ReportSickChild(SickErrand errand)
        {
            //7.2  Viktig kod ska ligga i try-catch
            try
            {
                repository.ReportVAB(errand);
                currentUser("Report VAB");
                return View("ThankYou");
            }
           // 7.3.Hantera exceptions korrekt
            catch (Exception e)
            {
                currentUser("Report VAB Failed");
                throw;
            }
        }

    //Hantera sjukskrivning en dag
    public IActionResult ReportSickDay()
    {
            //7.2  Viktig kod ska ligga i try-catch
            try
            {
                repository.ReportSickDay();
                currentUser("Report Sick 1 Day");
                return View("ThankYou");
            }
            catch (Exception e) {
                currentUser("Report Sick 1 Day Failed");
                throw;
            }
    }

    //Hantera sjukskrivning med intyg
    [HttpPost]
    public ViewResult ReportSick(SickErrand errand)
    {
            //7.2  Viktig kod ska ligga i try-catch
            try
            { 
  repository.ReportSick(errand);
            currentUser("Report Sick");
      return View("ThankYou");
            }
            catch (Exception e)
            {
                currentUser("Report Sick Failed");
                throw;
            }
        }

        //8.1. Logga viktiga saker (inloggningar, vad som gör, felmeddelanden) & 8.2. Inkludera tid, ip-adress, vem och vad som gjordes
        private void currentUser(string action)
        {

            Logger temp = new Logger();
            temp.EmployeeId = contextAcc.HttpContext.User.Identity.Name;
            temp.Action = action;
            temp.Time = System.DateTime.Now.ToString();
            temp.Ip = contextAcc.HttpContext.Connection.RemoteIpAddress.ToString();


            repository.LoggActivity(temp);
        }
    }
}
