namespace Identity.Api.Services.DbCleanupService;

public interface ICleanupService
{
    Task CleanUpExpiredConfirmations();
}