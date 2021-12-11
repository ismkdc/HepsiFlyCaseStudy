using System.ComponentModel.DataAnnotations;
using HepsiFlyCaseStudy.CQRS.Commands.Response;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Commands.Request;

public class CreateProductCommandRequest : IRequest<CreateProductCommandResponse>
{
    [Required] public string? Name { get; set; }

    public string? Description { get; set; }
    public Guid CategoryId { get; set; }

    [Required] [Range(1, double.MaxValue)] public decimal Price { get; set; }

    [Required] public string? Currency { get; set; }
}