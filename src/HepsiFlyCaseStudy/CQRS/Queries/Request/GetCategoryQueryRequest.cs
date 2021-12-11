using HepsiFlyCaseStudy.CQRS.Queries.Response;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Queries.Request;

public class GetCategoryQueryRequest : IRequest<GetCategoryQueryResponse>
{
    public Guid Id { get; set; }
}