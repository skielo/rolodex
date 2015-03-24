using Rolodex.Domain.Entities;
using System.Data.Entity;


namespace Rolodex.Domain.Repository
{
    public class RolodexModelContext: DbContext
    {
        public RolodexModelContext()
        {
            Database.SetInitializer<RolodexModelContext>(null);
        }

        public DbSet<Person> People { get; set; }
    }
}
