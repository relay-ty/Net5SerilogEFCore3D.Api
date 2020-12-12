using Microsoft.EntityFrameworkCore;
using Net5SerilogEFCore3D.Infrastructure.EF.Shared.Constants;
using Net5SerilogEFCore3D.Model.DomainModels;

namespace Net5SerilogEFCore3D.Infrastructure.EF.Shared.DbContexts
{
    public class SerilogDbContext : DbContext
    {
        public DbSet<Serilog> Serilogs { get; set; }

        public SerilogDbContext(DbContextOptions<SerilogDbContext> options) : base(options)
        {
        }
        #region InitialIdentityServer 创建数据迁移
        // dotnet ef migrations add InitialSerilogDbMigration -c SerilogDbContext -o Migrations/Serilog
        // update-database InitialSerilogDbMigration -Context SerilogDbContext       
        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ConfigureSerilogContext(builder);
            base.OnModelCreating(builder);
        }

        private static void ConfigureSerilogContext(ModelBuilder builder)
        {
            builder.Entity<Serilog>(log =>
            {
                log.ToTable(TableConsts.Logging);
                log.HasKey(x => x.Id);
                log.Property(x => x.Level).HasMaxLength(128);
            });
        }
    }
}
