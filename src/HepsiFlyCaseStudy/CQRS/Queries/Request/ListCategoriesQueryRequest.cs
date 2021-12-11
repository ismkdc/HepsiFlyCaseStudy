using HepsiFlyCaseStudy.CQRS.Queries.Response;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Queries.Request;

public class ListCategoriesQueryRequest : IRequest<IEnumerable<ListCategoriesQueryResponse>>
{
    public string? Name { get; set; }
}