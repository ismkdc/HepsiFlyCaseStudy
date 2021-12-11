using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Queries.Request;
using HepsiFlyCaseStudy.CQRS.Queries.Response;
using HepsiFlyCaseStudy.Models;
using MediatR;
using MongoDB.Driver;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.QueryHandlers;

public class
    ListProductsQueryHandler : IRequestHandler<ListProductsQueryRequest, IEnumerable<ListProductsQueryResponse>>
{
    private readonly MongoDBContext _context;
    private readonly IMapper _mapper;
    private readonly IRedisCacheClient _redisCache;

    public ListProductsQueryHandler(MongoDBContext context, IMapper mapper, IRedisCacheClient redisCache)
    {
        _context = context;
        _mapper = mapper;
        _redisCache = redisCache;
    }

    public async Task<IEnumerable<ListProductsQueryResponse>> Handle(ListProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        var isCacheable = false;
        const string cacheKey = "PRODUCTS";
        IFindFluent<Product, Product>? query;

        if (string.IsNullOrEmpty(request.Name))
        {
            var cachedData = await _redisCache.Db0.GetAsync<IEnumerable<ListProductsQueryResponse>>(cacheKey);
            if (cachedData != null)
                return cachedData;

            query = _context.Products.Find(x => true);
            isCacheable = true;
        }
        else
        {
            query = _context.Products.Find(x => x.Name != null && x.Name.Contains(request.Name));
        }

        var products = await query.ToListAsync(cancellationToken);
        var result = _mapper.Map<IEnumerable<ListProductsQueryResponse>>(products);

        if (isCacheable)
            await _redisCache.Db0.AddAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return result;
    }
}