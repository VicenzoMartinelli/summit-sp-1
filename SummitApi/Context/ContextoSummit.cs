using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SummitApi.Domain;

namespace SummitApi.Context
{
  public class ContextoSummit : IdentityDbContext<Usuario>
  {
    public ContextoSummit(DbContextOptions<ContextoSummit> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=summit_db;Trusted_Connection=True;MultipleActiveResultSets=true");
    }
  }
}
