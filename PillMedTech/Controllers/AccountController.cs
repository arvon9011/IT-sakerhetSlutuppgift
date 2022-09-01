using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PillMedTech.Models;
using Microsoft.AspNetCore.Http;

namespace PillMedTech.Controllers
{
    //5.2. Jobba med Authorize och Roles, samt AllowAnonymus
    [Authorize]
//3.2. Jobba med Razor View(@variabelnamn = automatisk html encode för output)
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private IPillMedTechRepository repository;
        private IHttpContextAccessor contextAcc;

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr, IPillMedTechRepository repo, IHttpContextAccessor ctxAcc)
        {
            userManager = userMgr;
            signInManager = signInMgr;
            repository = repo;
            contextAcc = ctxAcc;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        //5.2. Jobba med Authorize och Roles, samt AllowAnonymus
        [AllowAnonymous]
        //6.3. [ValidateAntiForgeryToken] på action-method
        [ValidateAntiForgeryToken]
        //4.3. Försök till åtkomst via url går automatiskt till login-sidan
        public async Task<IActionResult> Login(LoginModel loginModel)
        {



            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.UserName);

                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    //Sista "true" tillåter lockout.
                    if ((await signInManager.PasswordSignInAsync(user,
                                    loginModel.Password, false, true)).Succeeded)

                    {
                        currentUser(user.UserName, "Log In");
                        if (await userManager.IsInRoleAsync(user, "Employee"))
                        {

                            return Redirect("/Employee/StartEmployee");
                        }
                        else if (await userManager.IsInRoleAsync(user, "HRStaff"))
                        {

                            return Redirect("/Admin/HRStaff");
                        }
                        else if (await userManager.IsInRoleAsync(user, "ITStaff"))
                        {

                            return Redirect("/Admin/ITStaff");
                        }
                        else
                        {

                            return Redirect("/Home/Index");
                        }
                    }
                    else { currentUser("No User", "Log In Failed"); }
                }
            }
            //4.2.Generellt felmeddelande vid inlogggning
            
            ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord");
            //Fördröjning på 3 sekunder. (4.4. Fördröjning och/eller captcha vid flera försök, samt lockout vid för många felaktiga försök)
            await Task.Delay(3000);
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            var user = contextAcc.HttpContext.User.Identity.Name;
            //6.4. Ta bort token cookie vid utloggning
            await signInManager.SignOutAsync();

            currentUser(user, "Log Out");
            return Redirect(returnUrl);
        }
       
        //5.3. Försök till åtkomst via url när man är inloggad leder till AccessDenied
        [AllowAnonymous]
        public ViewResult AccessDenied()
        {
            var user = contextAcc.HttpContext.User.Identity.Name;
            currentUser(user, "AccessDenied");
            return View();
        }


        //8.1. Logga viktiga saker (inloggningar, vad som gör, felmeddelanden) & 8.2. Inkludera tid, ip-adress, vem och vad som gjordes

        private void currentUser(string user, string action)
        {



            Logger temp = new Logger();
            temp.EmployeeId = user;
            temp.Action = action;
            temp.Time = DateTime.Now.ToString();
            temp.Ip = contextAcc.HttpContext.Connection.RemoteIpAddress.ToString();


            repository.LoggActivity(temp);
        }
    }
}
