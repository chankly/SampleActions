using Microsoft.EntityFrameworkCore;

namespace HSoft.SampleActions.IntegrationTest
{
    public class Tests
    {
        [OneTimeSetUp]
        public void Start()
        {
            var options = new DbContextOptionsBuilder<MySqlDbContext>().UseMySQL("server=localhost;database=testdb;user=root;password=root_test").Options;

            using (var context = new MySqlDbContext(options))
            {
                context.Database.EnsureCreated();
                var customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Name = "John",
                    LastName = "Doe"
                };
                context.Customers.Add(customer);
                context.SaveChanges();
            }
        }

        [Category("Integration")]
        [Test]
        public void CustomerAdd_HappyCase()
        {
            var options = new DbContextOptionsBuilder<MySqlDbContext>().UseMySQL("server=localhost;database=testdb;user=root;password=root_test").Options;
            using (var context = new MySqlDbContext(options))
            {
                var customers = context.Customers.ToList();
                Assert.IsNotNull(customers);
                Assert.That(customers.Count, Is.EqualTo(1));
            }
        }

        [Category("Integration")]
        [Test]
        public void CustomerAdd_Fail()
        {
            Assert.Fail("Failed test");
        }
    }

    public class MySqlDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                        .HasKey(c => c.Id);

            modelBuilder.Entity<Customer>().Property(c => c.Name).HasMaxLength(200);
            modelBuilder.Entity<Customer>().Property(c => c.LastName).HasMaxLength(200);

            modelBuilder.Entity<Customer>().ToTable("Customers");

            // Configure your entities here
            base.OnModelCreating(modelBuilder);
        }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}