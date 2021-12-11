using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Commands.Response;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.CommandHandlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse?>
{
    private readonly MongoDBContext _context;
    private readonly IMapper _mapper;
    private readonly IRedisCacheClient _redisCache;

    public CreateProductCommandHandler(MongoDBContext context, IMapper mapper, IRedisCacheClient redisCache)
    {
        _context = context;
        _mapper = mapper;
        _redisCache = redisCache;
    }

    public async Task<CreateProductCommandResponse?> Handle(CreateProductCommandRequest request,
        CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(request);

        var isCategoryExists = await _context.Categories.CountDocumentsAsync(x => x.Id == request.CategoryId,
            cancellationToken: cancellationToken) > 0;

        if (!isCategoryExists)
            return null;

        await _context.Products.InsertOneAsync(product, cancellationToken: cancellationToken);
        await _redisCache.Db0.RemoveAsync("PRODUCTS");

        return new CreateProductCommandResponse
        {
            Id = product.Id
        };
    }
}