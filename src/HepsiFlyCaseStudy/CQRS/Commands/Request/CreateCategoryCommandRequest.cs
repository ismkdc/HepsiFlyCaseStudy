using System.ComponentModel.DataAnnotations;
using HepsiFlyCaseStudy.CQRS.Commands.Response;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Commands.Request;

public class CreateCategoryCommandRequest : IRequest<CreateCategoryCommandResponse>
{
    [Required] public string? Name { get; set; }

    public string? Description { get; set; }
}