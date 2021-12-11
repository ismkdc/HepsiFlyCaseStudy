using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HepsiFlyCaseStudy.CQRS.Common;
using MediatR;

namespace HepsiFlyCaseStudy.CQRS.Commands.Request;

public class UpdateCategoryCommandRequest : IRequest<EmptyResponse>
{
    [JsonIgnore] public Guid Id { get; set; }

    [Required] public string? Name { get; set; }

    public string? Description { get; set; }
}