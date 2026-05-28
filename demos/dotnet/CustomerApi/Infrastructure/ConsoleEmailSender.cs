using CustomerApi.Services;

namespace CustomerApi.Infrastructure;

// "Riktig" mail-sender för demot: skriver bara till loggen.
// I integrationstestet ersätter vi den här med en NSubstitute-mock så att
// vi kan verifiera att mail skulle ha skickats — utan en riktig SMTP-server.
public class ConsoleEmailSender : IEmailSender
{
    private readonly ILogger<ConsoleEmailSender> _logger;

    public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger) => _logger = logger;

    public Task SendAsync(string to, string subject, string body)
    {
        _logger.LogInformation("EMAIL -> {To} | {Subject} | {Body}", to, subject, body);
        return Task.CompletedTask;
    }
}
