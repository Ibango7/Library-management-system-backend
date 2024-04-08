using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LmsBackend.EntityFrameworkCore
{
    public static class LmsBackendDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LmsBackendDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LmsBackendDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
