using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Common;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.CommandHandlers;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommandRequest, EmptyResponse?>
{
    private readonly MongoDBContext _context;
    private readonly IRedisCacheClient _redisCache;

    public DeleteCategoryCommandHandler(MongoDBContext context, IRedisCacheClient redisCache)
    {
        _context = context;
        _redisCache = redisCache;
    }

    public async Task<EmptyResponse?> Handle(DeleteCategoryCommandRequest request,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Category>.Filter.Eq("Id", request.Id);
        var result = await _context.Categories.DeleteOneAsync(filter, cancellationToken);

        await _redisCache.Db0.RemoveAllAsync(new[] {"CATEGORIES", $"CATEGORY_{request.Id}"});

        return result.DeletedCount == 0 ? null : EmptyResponse.Default;
    }
}