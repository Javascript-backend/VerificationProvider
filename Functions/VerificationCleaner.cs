using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VerificationProvider.Services;

namespace VerificationProvider.Functions;

public class VerificationCleaner(ILogger<VerificationCleaner> loggerFactory, VerificationCleanerService service)
{
    private readonly ILogger<VerificationCleaner> _logger = loggerFactory;
    private readonly VerificationCleanerService _service = service;


    [Function("VerificationCleaner")]
    public async Task Run([TimerTrigger("0 */1 * * * *")] TimerInfo myTimer)
    {
        try
        {
            await _service.RemoveExpiredRecordsAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : VerificationCleaner.Run  :: {ex.Message}");
        }
    }
}
