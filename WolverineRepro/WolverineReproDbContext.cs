using Microsoft.EntityFrameworkCore;

namespace WolverineRepro
{
    public class WolverineReproDbContext : DbContext
    {
        public WolverineReproDbContext(DbContextOptions<WolverineReproDbContext> options) : base(options) { }
    }
}
