﻿using System.Linq;
using Watchman.Cqrs;
using Watchman.DomainModel.Commons.Queries.Handlers;
using Watchman.Integrations.MongoDB;

namespace Watchman.DomainModel.Messages.Queries.Handlers
{
    public class GetMessagesQueryHandler : PaginationMessagesQueryHandler, IQueryHandler<GetMessagesQuery, GetMessagesQueryResult>
    {
        private readonly ISessionFactory _sessionFactory;

        public GetMessagesQueryHandler(ISessionFactory sessionFactory)
        {
            this._sessionFactory = sessionFactory;
        }

        public GetMessagesQueryResult Handle(GetMessagesQuery query)
        {
            using var session = _sessionFactory.Create();
            var messages = session.Get<Message>();
            if (query.ServerId != 0)
            {
                messages = TakeOnlyFromOneServer(query.ServerId, messages);
            }
            if (query.ChannelId != 0)
            {
                messages = TakeOnlyFromChannel(query.ChannelId, messages);
            }
            if (query.UserId.HasValue && query.UserId != 0)
            {
                messages = TakeOnlyForUser(query.UserId.Value, messages);
            }
            var paginated = this.Paginate(query, messages);
            if (query is GetUserMessagesQuery userQuery)
            {
                paginated = paginated.Where(x => x.Author.Id == userQuery.UserId);
            }
            return new GetMessagesQueryResult(paginated);
        }

        private IQueryable<Message> TakeOnlyFromOneServer(ulong serverId, IQueryable<Message> messages)
        {
            return messages.Where(x => x.Server.Id == serverId);
        }
        private IQueryable<Message> TakeOnlyFromChannel(ulong channelId, IQueryable<Message> messages)
        {
            return messages.Where(x => x.Channel.Id == channelId);
        }
        private IQueryable<Message> TakeOnlyForUser(ulong? userId, IQueryable<Message> messages)
        {
            return messages.Where(x => x.Author.Id == userId.Value);
        }
    }
}
