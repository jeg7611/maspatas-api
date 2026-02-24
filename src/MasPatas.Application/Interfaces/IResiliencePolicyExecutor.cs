namespace MasPatas.Application.Interfaces;

public interface IResiliencePolicyExecutor
{
    Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken = default);
}
