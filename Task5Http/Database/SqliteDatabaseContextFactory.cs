using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Task5Http.Database
{
    public class SqliteDatabaseContextFactory : IDesignTimeDbContextFactory<SqliteDatabaseContext>
    {
        public SqliteDatabaseContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SqliteDatabaseContext>();
            optionsBuilder.UseSqlite("Data Source=data/hackatondb.db");
            return new SqliteDatabaseContext(optionsBuilder.Options);
        }
    }
}
