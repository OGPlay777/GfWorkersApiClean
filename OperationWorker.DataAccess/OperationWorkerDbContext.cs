using Microsoft.EntityFrameworkCore;
using OperationWorker.DataAccess.Entities;

namespace OperationWorker.DataAccess
{
    public class OperationWorkerDbContext : DbContext
    {
        public DbSet<GfWorkerEntity> GfWorkers { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }

        public DbSet<AppUserEntity> AppUsers { get; set; }

        public DbSet<OperationEntity> Operations { get; set; }

        public DbSet<EquipmentEntity> Equipment { get; set; }

        public DbSet<PaintPowderEntity> PaintPowder { get; set; }

        public OperationWorkerDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
