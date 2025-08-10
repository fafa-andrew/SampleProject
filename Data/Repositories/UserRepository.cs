using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Indexes;
using Raven.Client;

namespace Data.Repositories
{
    [AutoRegister]
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IAsyncDocumentSession _documentSession;

        public UserRepository(IAsyncDocumentSession documentSession) : base(documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<IEnumerable<User>> GetAsync(
            CancellationToken ct,
            UserTypes? userType = null, 
            string name = null, 
            string email = null
            )
        {
            var query = _documentSession.Advanced.AsyncDocumentQuery<User, UsersListIndex>();

            var hasFirstParameter = false;
            if (userType != null)
            {
                query = query.WhereEquals("Type", (int)userType);
                hasFirstParameter = true;
            }

            if (name != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                else
                {
                    hasFirstParameter = true;
                }
                query = query.Where($"Name:*{name}*");
            }

            if (email != null)
            {
                if (hasFirstParameter)
                {
                    query = query.AndAlso();
                }
                query = query.WhereEquals("Email", email);
            }
            return await query.ToListAsync();
        }

        public async Task DeleteAllAsync()
        {
            await DeleteAllByIndexAsync<UsersListIndex>();
        }
    }
}