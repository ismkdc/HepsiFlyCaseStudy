using HepsiFlyCaseStudy.CQRS.Common;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Commands.Request;

public class DeleteProductCommandRequest : IRequest<EmptyResponse>
{
    public Guid Id { get; set; }
}