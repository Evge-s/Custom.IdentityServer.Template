using Identity.Api.Models.ServiceData.BuilderModels;
using Identity.Api.Models.ServiceData.Tokens;
using Identity.Api.Models.ServiceData.UserData;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Models.ServiceData
{
    public class ServiceContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ServiceContext> _logger;
        private readonly IConfiguration _configuration;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ConfirmationEmailCode> ConfirmationEmails { get; set; }
        public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }

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
            modelBuilder.ApplyConfiguration(new ConfirmationEmailEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ResetPasswordTokenEntityTypeConfiguration());
        }
    }
}
