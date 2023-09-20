using AsyncOperationBuilder.AsyncOperations.Models;

namespace AsyncOperationBuilder.AsyncOperations.Interfaces;

public interface IAsyncOperationPreviewTicketScopedBuilder<TAsyncOperationInitData, TStatusFetchResult>
{
    public void SetInitPreviewTicket(AsyncOperationTicket operationTicket);
    
    public IAsyncOperation<TAsyncOperationInitData, TStatusFetchResult> Build();
}