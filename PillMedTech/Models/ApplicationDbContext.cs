using Microsoft.EntityFrameworkCore;
using PillMedTech.Models;

namespace PillMedTech.Models
{
  public class ApplicationDbContext: DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<SickErrand> SickErrands { get; set; }

  }
}
