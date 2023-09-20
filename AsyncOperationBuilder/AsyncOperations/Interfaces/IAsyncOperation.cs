using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Models;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IAsyncOperation<TAsyncOperationData, TResult>
{
    public Task<AsyncOperationTicket> Start(TAsyncOperationData asyncOperationData, CancellationToken ct = default);
    
    public Task<AsyncOperationStatusResult<TResult>> GetStatus(CancellationToken ct = default);
}