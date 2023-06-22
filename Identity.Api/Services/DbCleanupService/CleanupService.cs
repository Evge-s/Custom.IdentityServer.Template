using Identity.Api.Models.ServiceData;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Services.DbCleanupService;

public class CleanupService : ICleanupService
{
    private readonly ServiceContext _context;

    public CleanupService(ServiceContext context)
    {
        _context = context;
    }

    public async Task CleanUpExpiredConfirmations()
    {
        var expiredCodes = await _context.ConfirmationEmails.Where(c => c.IsExpired).ToListAsync();
        _context.ConfirmationEmails.RemoveRange(expiredCodes);
        await _context.SaveChangesAsync();
    }
}