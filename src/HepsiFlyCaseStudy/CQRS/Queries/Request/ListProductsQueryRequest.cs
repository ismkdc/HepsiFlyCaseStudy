using HepsiFlyCaseStudy.CQRS.Queries.Response;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Queries.Request;

public class ListProductsQueryRequest : IRequest<IEnumerable<ListProductsQueryResponse>>
{
    public string? Name { get; set; }
}