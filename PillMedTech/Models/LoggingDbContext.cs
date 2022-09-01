using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PillMedTech.Models;

namespace PillMedTech.Models
{
    //8.4. Förvara loggningarna i annan databas (och annan server)
    public class LoggingDbContext : DbContext
    {
    public LoggingDbContext(DbContextOptions<LoggingDbContext> options) : base(options) { }
    public DbSet<Logger> Loggers { get; set; }
    }
}
