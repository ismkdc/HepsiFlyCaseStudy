using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Commands.Response;
using HepsiFlyCaseStudy.Models;
using MediatR;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace HepsiFlyCaseStudy.CQRS.Handlers.CommandHandlers;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    private readonly MongoDBContext _context;
    private readonly IMapper _mapper;
    private readonly IRedisCacheClient _redisCache;

    public CreateCategoryCommandHandler(MongoDBContext context, IMapper mapper, IRedisCacheClient redisCache)
    {
        _context = context;
        _mapper = mapper;
        _redisCache = redisCache;
    }

    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest request,
        CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request);

        await _context.Categories.InsertOneAsync(category, cancellationToken: cancellationToken);
        await _redisCache.Db0.RemoveAsync("CATEGORIES");

        return new CreateCategoryCommandResponse
        {
            Id = category.Id
        };
    }
}