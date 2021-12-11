using HepsiFlyCaseStudy.CQRS.Queries.Response;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Queries.Request;

public class GetProductQueryRequest : IRequest<GetProductQueryResponse>
{
    public Guid Id { get; set; }
}