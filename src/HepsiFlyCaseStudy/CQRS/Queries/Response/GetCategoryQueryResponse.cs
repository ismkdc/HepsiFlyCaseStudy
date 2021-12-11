namespace HepsiFlyCaseStudy.CQRS.Queries.Response;

public class GetCategoryQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}