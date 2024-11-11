
using Microsoft.EntityFrameworkCore;
using WebApi.Modelos;

namespace WebApi.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
               
             public DbSet<UsEq> Tablaequipos { get; set; }
    }
}