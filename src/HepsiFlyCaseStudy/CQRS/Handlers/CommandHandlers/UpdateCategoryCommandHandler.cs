using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Common;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.CommandHandlers;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommandRequest, EmptyResponse?>
{
    private readonly MongoDBContext _context;
    private readonly IRedisCacheClient _redisCache;

    public UpdateCategoryCommandHandler(MongoDBContext context, IRedisCacheClient redisCache)
    {
        _context = context;
        _redisCache = redisCache;
    }

    public async Task<EmptyResponse?> Handle(UpdateCategoryCommandRequest request,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Category>.Filter.Eq("Id", request.Id);
        var update = Builders<Category>.Update
            .Set("Name", request.Name)
            .Set("Description", request.Description);

        var result = await _context.Categories.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        await _redisCache.Db0.RemoveAllAsync(new[] {"CATEGORIES", $"CATEGORY_{request.Id}"});

        return result.ModifiedCount == 0 ? null : EmptyResponse.Default;
    }
}