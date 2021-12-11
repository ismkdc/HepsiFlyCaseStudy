using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Common;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.CommandHandlers;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, EmptyResponse?>
{
    private readonly MongoDBContext _context;
    private readonly IRedisCacheClient _redisCache;

    public UpdateProductCommandHandler(MongoDBContext context, IRedisCacheClient redisCache)
    {
        _context = context;
        _redisCache = redisCache;
    }

    public async Task<EmptyResponse?> Handle(UpdateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var isCategoryExists = await _context.Categories.CountDocumentsAsync(x => x.Id == request.CategoryId,
            cancellationToken: cancellationToken) > 0;

        if (!isCategoryExists)
            return null;

        var filter = Builders<Product>.Filter.Eq("Id", request.Id);
        var update = Builders<Product>.Update
            .Set("Name", request.Name)
            .Set("Description", request.Description)
            .Set("CategoryId", request.CategoryId)
            .Set("Price", request.Price)
            .Set("Currency", request.Currency);

        var result = await _context.Products.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        await _redisCache.Db0.RemoveAllAsync(new[] {"PRODUCTS", $"PRODUCT_{request.Id}"});

        return result.ModifiedCount == 0 ? null : EmptyResponse.Default;
    }
}