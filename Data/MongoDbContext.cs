using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

public class MongoDbContext : DbContext
{
    public static MongoDbContext Create(IMongoDatabase database) =>
    new(new DbContextOptionsBuilder<MongoDbContext>()
        .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
        .Options);
    public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options) {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
     }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Grade>().ToCollection("grades");
        modelBuilder.Entity<Restriction>().ToCollection("restrictions");
    }
    public DbSet<Grade> Grades { get; init; }
    public DbSet<Restriction> Restrictions { get; init; }
}
