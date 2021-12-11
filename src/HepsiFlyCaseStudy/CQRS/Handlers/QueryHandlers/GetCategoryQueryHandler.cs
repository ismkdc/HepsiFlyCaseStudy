using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Queries.Request;
using HepsiFlyCaseStudy.CQRS.Queries.Response;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.QueryHandlers;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQueryRequest, GetCategoryQueryResponse>
{
    private readonly MongoDBContext _context;
    private readonly IMapper _mapper;
    private readonly IRedisCacheClient _redisCache;

    public GetCategoryQueryHandler(MongoDBContext context, IMapper mapper, IRedisCacheClient redisCache)
    {
        _context = context;
        _mapper = mapper;
        _redisCache = redisCache;
    }

    public async Task<GetCategoryQueryResponse> Handle(GetCategoryQueryRequest request,
        CancellationToken cancellationToken)
    {
        var cacheKey = $"CATEGORY_{request.Id}";
        var cachedData = await _redisCache.Db0.GetAsync<GetCategoryQueryResponse>(cacheKey);
        if (cachedData != null)
            return cachedData;

        var category = await _context
            .Categories
            .Find(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);

        var result = _mapper.Map<GetCategoryQueryResponse>(category);

        await _redisCache.Db0.AddAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}