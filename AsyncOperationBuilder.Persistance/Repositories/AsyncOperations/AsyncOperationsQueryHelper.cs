using AsyncOperationBuilder.AsyncOperations.Models;
using AsyncOperationBuilder.Persistance.Repositories.Shared;
using Deep.Migrations.Repositories.AsyncOperations;

namespace AsyncOperationBuilder.Persistance.Repositories.AsyncOperations
{
    public class AsyncOperationsQueryHelper
    {

	    private const string CreateInitialMigrationScript = @"";
        private const string StoreAsyncOperationsQuery = @"
IF @TicketId IS NOT NULL AND EXISTS (
  SELECT TOP 1 1 FROM [dbo].[z_deepAsyncOperations] as op
  WHERE op.TicketId = @TicketId
  )
BEGIN
  WITH LastAppliedOperationState(OperationResult, OperationStatus) AS (
		SELECT TOP 1 StoredOperationState.operationResult, StoredOperationState.operationStatus
		FROM [dbo].[z_deepAsyncOperations] as StoredOperationState
		WHERE StoredOperationState.TicketId = @TicketId
		ORDER BY StoredOperationState.createdAt DESC
	)

  INSERT INTO [dbo].[z_deepAsyncOperations](TicketId, operationResult, operationStatus)
  SELECT 
	@TicketId as TicketId,
	CASE WHEN st.OperationResult = @OperationResultPayload THEN NULL ELSE @OperationResultPayload END as OperaitonResult,
	CASE WHEN st.OperationStatus = @OperationStatus THEN NULL ELSE @OperationStatus END as OperationStatus
  FROM LastAppliedOperationState as st
  -- Condition means that if both OperationResult and OperaitonStatus is null 
  WHERE NOT (OperationResult IS NULL AND OperationStatus IS NULL);
  SELECT @TicketId;
END
ELSE 
BEGIN 
	INSERT INTO [dbo].[z_deepAsyncOperations](operationStatus, contextPayload)
	OUTPUT inserted.TicketId
	VALUES(@OperationStatus, @ContextPayload)
END";

        private const string LoadAsyncOperationsQuery = @"
SELECT 
	op.TicketId as TicketId,
	op.operationStatus as OperationStatus,
	op.contextPayload as ContextPayload,
	op.operationResult as OperationResult,
	op.createdAt as CreatedAt
FROM [dbo].[z_deepAsyncOperations] as op 
WHERE op.TicketId = @TicketId;";
	        
	        
        public QueryGenerationResult<AsyncOperationStoreQueryParameters> CreateStoreQuery(AsyncOperationStoreQueryParameters parameters)
			=> new QueryGenerationResult<AsyncOperationStoreQueryParameters>
           {
               Query = StoreAsyncOperationsQuery,
               Parameters = parameters
           };


        public QueryGenerationResult<LoadAsyncOperationStateQueryParams> CreateLoadQuery(AsyncOperationTicket ticket)
	        => new QueryGenerationResult<LoadAsyncOperationStateQueryParams>
	        {
				Query = LoadAsyncOperationsQuery,
				Parameters = new LoadAsyncOperationStateQueryParams
				{
					TicketId = ticket.Id
				}
	        };
    }
}