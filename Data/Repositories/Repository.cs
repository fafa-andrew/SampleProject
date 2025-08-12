using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Repositories.Contracts;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Indexes;

namespace Data.Repositories
{
    [AutoRegister]
    public class Repository<T> : IRepository<T> where T : IdObject
    {
        private readonly IAsyncDocumentSession _documentSession;

        public Repository(IAsyncDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task StoreAsync(T entity, CancellationToken ct) => await _documentSession.StoreAsync(entity, ct);

        public async Task SaveAsync(CancellationToken ct) => await _documentSession.SaveChangesAsync(ct);

        public async Task DeleteAsync(T entity)
        {
            _documentSession.Delete(entity);
            await Task.CompletedTask;
        }

        public async Task<T> GetAsync(Guid id, CancellationToken ct)
        {
            return await _documentSession.LoadAsync<T>(id, ct);
        }

        protected async Task DeleteAllByIndexAsync<TIndex>() where TIndex : AbstractIndexCreationTask<T>
        {
            var operation = await _documentSession.Advanced.DocumentStore
                .AsyncDatabaseCommands
                .DeleteByIndexAsync(typeof(TIndex).Name, new IndexQuery());

            //We wait to be sure the delete all operation runs to completion
            await operation.WaitForCompletionAsync();
        }
    }
}