using System.Threading;
using System.Threading.Tasks;
using AsyncOperationBuilder.AsyncOperations.Models;
using Deep.Logic.Repositories.AsyncOperations;
using Deep.Logic.Repositories.AsyncOperations.Models;
using Umbraco.Core.Scoping;

namespace AsyncOperationBuilder.Persistance.Repositories.AsyncOperations
{
    public class AsyncOperationsRepository<TContextData, TOperationResult> :
        IAsyncOperationRepository<TContextData, TOperationResult>
    {
        private readonly AsyncOperationsQueryHelper asyncOperationsQueryHelper = new AsyncOperationsQueryHelper();
        private readonly IScopeProvider scopeProvider;
        public AsyncOperationsRepository(
            IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public Task<AsyncOperationTicket> StoreAsyncOperationState(AsyncOperationStateMachinePersistedState asyncOperationStateMachine,
            CancellationToken ct = default)
        {
            throw new System.NotImplementedException();
        }

        public Task<AsyncOperationStateMachinePersistedState> LoadAsyncOperationState(AsyncOperationTicket ticket, CancellationToken ct = default)
        {
            throw new System.NotImplementedException();
        }
    }
}