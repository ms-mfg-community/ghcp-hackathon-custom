using Microsoft.AspNetCore.Components.Server.Circuits;

namespace calculator.web.Services;

/// <summary>
/// Custom circuit handler for monitoring Blazor Server connections.
/// </summary>
public class CalculatorCircuitHandler : CircuitHandler
{
    private readonly ILogger<CalculatorCircuitHandler> _logger;

    public CalculatorCircuitHandler(ILogger<CalculatorCircuitHandler> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Circuit {CircuitId} connected at {Time}", circuit.Id, DateTime.UtcNow);
        return base.OnConnectionUpAsync(circuit, cancellationToken);
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Circuit {CircuitId} disconnected at {Time}", circuit.Id, DateTime.UtcNow);
        return base.OnConnectionDownAsync(circuit, cancellationToken);
    }

    public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Circuit {CircuitId} opened at {Time}", circuit.Id, DateTime.UtcNow);
        return base.OnCircuitOpenedAsync(circuit, cancellationToken);
    }

    public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Circuit {CircuitId} closed at {Time}", circuit.Id, DateTime.UtcNow);
        return base.OnCircuitClosedAsync(circuit, cancellationToken);
    }
}
