using HepsiFlyCaseStudy.CQRS.Common;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Commands.Request;

public class DeleteCategoryCommandRequest : IRequest<EmptyResponse>
{
    public Guid Id { get; set; }
}