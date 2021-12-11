namespace HepsiFlyCaseStudy.CQRS.Queries.Response;

public class ListCategoriesQueryResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}