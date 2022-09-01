using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PillMedTech.Models;

namespace PillMedTech.Controllers
{
  public class AdminController : Controller
  {
    private IPillMedTechRepository repository;
    private IHttpContextAccessor contextAcc;

    public AdminController(IPillMedTechRepository repo, IHttpContextAccessor ctxAcc)
    {
      repository = repo;
      contextAcc = ctxAcc;
        }
       // 5.1. Begränsa behörighet(inte mer än man behöver tillgång till)

        //3.2. Jobba med Razor View(@variabelnamn = automatisk html encode för output)
        //Visa upp söksidan för HR-personal
        [Authorize(Roles = "HRStaff")]
       
        public ViewResult HRStaff()
    {
      return View();
        }
       // 4.1. Jobba med IdentityUser och IdentityRole
    //Resultatet av sökningen
    [Authorize(Roles = "HRStaff")]
       
        public ViewResult EmployeeInfo(SickErrand errand)
    {
            
       var errandsList = repository.SortedErrands(errand.EmployeeID);
        currentUser("Employee Search");
        return View(errandsList);
            
         
    }

        // 5.1. Begränsa behörighet(inte mer än man behöver tillgång till)
        [Authorize(Roles = "ITStaff")]
        
        public ViewResult ITStaff()
    {
            
                currentUser("Loaded Logg");
                var listOfLoggs = repository.decryptedLoggs();
                return View(listOfLoggs);

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
