using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HepsiFlyCaseStudy.CQRS.Common;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Commands.Request;

public class UpdateProductCommandRequest : IRequest<EmptyResponse>
{
    [JsonIgnore] public Guid Id { get; set; }

    [Required] public string? Name { get; set; }

    public string? Description { get; set; }
    public Guid CategoryId { get; set; }

    [Required] [Range(1, double.MaxValue)] public decimal Price { get; set; }

    [Required] public string? Currency { get; set; }
}