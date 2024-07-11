using Microsoft.Extensions.Logging;
using Telegramm.Abstractions;

namespace Telegramm.Implementations
{
    public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger) : PollingServiceBase<ReceiverService>(serviceProvider, logger)
    {
    }
}
