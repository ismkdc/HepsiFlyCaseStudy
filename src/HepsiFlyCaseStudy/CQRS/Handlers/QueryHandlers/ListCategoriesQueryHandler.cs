using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Queries.Request;
using HepsiFlyCaseStudy.CQRS.Queries.Response;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.QueryHandlers;

public class
    ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQueryRequest, IEnumerable<ListCategoriesQueryResponse>>
{
    private readonly MongoDBContext _context;
    private readonly IMapper _mapper;
    private readonly IRedisCacheClient _redisCache;

    public ListCategoriesQueryHandler(MongoDBContext context, IMapper mapper, IRedisCacheClient redisCache)
    {
        _context = context;
        _mapper = mapper;
        _redisCache = redisCache;
    }

    public async Task<IEnumerable<ListCategoriesQueryResponse>> Handle(ListCategoriesQueryRequest request,
        CancellationToken cancellationToken)
    {
        var isCacheable = false;
        const string cacheKey = "CATEGORIES";
        IFindFluent<Category, Category>? query;

        if (string.IsNullOrEmpty(request.Name))
        {
            var cachedData = await _redisCache.Db0.GetAsync<IEnumerable<ListCategoriesQueryResponse>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            query = _context.Categories.Find(x => true);
            isCacheable = true;
        }
        else
        {
            query = _context.Categories.Find(x => x.Name != null && x.Name.Contains(request.Name));
        }

        var categories = await query.ToListAsync(cancellationToken);
        var result = _mapper.Map<IEnumerable<ListCategoriesQueryResponse>>(categories);

        if (isCacheable)
            await _redisCache.Db0.AddAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}