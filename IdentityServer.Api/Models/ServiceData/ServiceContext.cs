using IdentityServer.Api.Models.Data.Tokens;
using IdentityServer.Api.Models.Data.UserData;
using IdentityServer.Api.Models.ServiceData.BuilderModels;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Api.Models.ServiceData
{
    public class ServiceContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ServiceContext> _logger;
        private readonly IConfiguration _configuration;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ServiceContext(
            DbContextOptions<ServiceContext> options,
            ILoggerFactory loggerFactory,
            IConfiguration configuration) : base(options)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ServiceContext>();
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenEntityTypeConfiguration());
        }
    }
}
