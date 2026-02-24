using MasPatas.Application.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace MasPatas.Infrastructure.Resilience;

public class PollyResiliencePolicyExecutor : IResiliencePolicyExecutor
{
    private readonly AsyncPolicyWrap _policyWrap;

    public PollyResiliencePolicyExecutor()
    {
        var retry = Policy
            .Handle<TimeoutRejectedException>()
            .Or<MongoDB.Driver.MongoException>()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromMilliseconds(150 * attempt));

        var circuit = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

        var bulkhead = Policy.BulkheadAsync(50, int.MaxValue);
        var timeout = Policy.TimeoutAsync(TimeSpan.FromSeconds(8));

        var fallback = Policy
            .Handle<BrokenCircuitException>()
            .Or<MongoDB.Driver.MongoConnectionException>()
            .Or<TimeoutRejectedException>()
            .FallbackAsync(async ct => throw new InvalidOperationException("Servicio temporalmente no disponible. Reintente en unos segundos."));

        _policyWrap = Policy.WrapAsync(fallback, retry, circuit, bulkhead, timeout);
    }

    public Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken = default)
        => _policyWrap.ExecuteAsync(action, cancellationToken);
}
