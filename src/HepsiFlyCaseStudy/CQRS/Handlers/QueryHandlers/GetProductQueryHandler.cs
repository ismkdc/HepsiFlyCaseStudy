using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Queries.Request;
using HepsiFlyCaseStudy.CQRS.Queries.Response;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.QueryHandlers;

public class
    GetProductQueryHandler : IRequestHandler<GetProductQueryRequest, GetProductQueryResponse>
{
    private readonly MongoDBContext _context;
    private readonly IMapper _mapper;
    private readonly IRedisCacheClient _redisCache;

    public GetProductQueryHandler(MongoDBContext context, IMapper mapper, IRedisCacheClient redisCache)
    {
        _context = context;
        _mapper = mapper;
        _redisCache = redisCache;
    }

    public async Task<GetProductQueryResponse> Handle(GetProductQueryRequest request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"PRODUCT_{request.Id}";
        var cachedData = await _redisCache.Db0.GetAsync<GetProductQueryResponse>(cacheKey);
        if (cachedData != null)
            return cachedData;

        var product = await _context
            .Products
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var result = _mapper.Map<GetProductQueryResponse>(product);

        await _redisCache.Db0.AddAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}