using Maqha.Core.Entities;
using Maqha.Utilities.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Repository.Data
{
    public class MaqhaDbContext:DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MaqhaDbContext> _logger;

        public MaqhaDbContext(DbContextOptions<MaqhaDbContext>options,IHttpContextAccessor httpContextAccessor, ILogger<MaqhaDbContext> logger):base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        //CafeInfo Dbset 
        public DbSet<CafeInfo> CafeInfos { get; set; }
        //MenuItem Dbset
        public DbSet<MenuItem> MenuItems { get; set; }
        //Category Dbset
        public DbSet<Category> Categories { get; set; }
        //Order Dbset
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        //on model creating method to set the table name and schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MaqhaDbContext).Assembly);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //set CreatedAt fo new entities that inhert from BaseEntitty
          
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }   
            var auditEntries = PrepareAuditEntries();
            var result = await base.SaveChangesAsync(cancellationToken);
            WriteAuditLogsToSeq(auditEntries);
            return result;
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
        private List<AuditEntry> PrepareAuditEntries()
        {
            ChangeTracker.DetectChanges();
            var entries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Unchanged || entry.State == EntityState.Detached)
                    continue;

                var auditEntry = new AuditEntry
                {
                    TableName = entry.Metadata.GetTableName(),
                    Action = entry.State.ToString(),
                    User = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous",
                    IP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    Time = DateTime.UtcNow
                };

                foreach (var prop in entry.Properties)
                {
                    var name = prop.Metadata.Name;

                    if (entry.State == EntityState.Added)
                    {
                        auditEntry.NewValues[name] = prop.CurrentValue;
                    }
                    else if (entry.State == EntityState.Modified && prop.IsModified)
                    {
                        auditEntry.OldValues[name] = prop.OriginalValue;
                        auditEntry.NewValues[name] = prop.CurrentValue;
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        auditEntry.OldValues[name] = prop.OriginalValue;
                    }
                }

                entries.Add(auditEntry);
            }

            return entries;
        }
        private void WriteAuditLogsToSeq(List<AuditEntry> entries)
        {
            foreach (var entry in entries)
            {
                _logger.LogInformation("AUDIT | Table: {Table} | Action: {Action} | User: {User} | IP: {IP} | Old: {@Old} | New: {@New} | Time: {Time}",
                    entry.TableName,
                    entry.Action,
                    entry.User,
                    entry.IP,
                    entry.OldValues,
                    entry.NewValues,
                    entry.Time
                );
            }
        }

    }
}
