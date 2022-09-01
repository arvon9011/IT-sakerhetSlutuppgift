using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace PillMedTech.Models
{

    // Ta bort innan sidan driftsätts (9.1)
    public class IdentityInitializer
  {
    public static async Task EnsurePopulated(IServiceProvider service)
    {
      var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
      var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

      await CreateRoles(roleManager);
      await CreateUsers(userManager);
        }
      //  5.1. Begränsa behörighet(inte mer än man behöver tillgång till)

    private static async Task CreateRoles(RoleManager<IdentityRole> rManager)
    {
      if (!await rManager.RoleExistsAsync("Employee"))
      {
        await rManager.CreateAsync(new IdentityRole("Employee"));
      }

      if (!await rManager.RoleExistsAsync("HRStaff"))
      {
        await rManager.CreateAsync(new IdentityRole("HRStaff"));
      }

      if (!await rManager.RoleExistsAsync("ITStaff"))
      {
        await rManager.CreateAsync(new IdentityRole("ITStaff"));
      }
    }




    private static async Task CreateUsers(UserManager<IdentityUser> userManager)
    {

            IdentityUser EMP330 = new IdentityUser("EMP330");
            IdentityUser EMP430 = new IdentityUser("EMP430");
            IdentityUser EMP530 = new IdentityUser("EMP530");
            IdentityUser HRS270 = new IdentityUser("HRS270");
            IdentityUser ITS980 = new IdentityUser("ITS980");

            await userManager.CreateAsync(EMP330, "EdwardSnigelMössa");
            await userManager.CreateAsync(EMP430, "QUIDFRE45");
            await userManager.CreateAsync(EMP530, "TreÖgonPåHunden");
            await userManager.CreateAsync(HRS270, "Knasvargkille");
            await userManager.CreateAsync(ITS980, "ERKANBANAN32");

            await userManager.AddToRoleAsync(EMP330, "Employee");
            await userManager.AddToRoleAsync(EMP430, "Employee");
            await userManager.AddToRoleAsync(EMP530, "Employee");
            await userManager.AddToRoleAsync(HRS270, "HRStaff");
            await userManager.AddToRoleAsync(ITS980, "ITStaff");


        }
    }
}
