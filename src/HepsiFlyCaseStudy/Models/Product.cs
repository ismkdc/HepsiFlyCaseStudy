namespace HepsiFlyCaseStudy.Models;

public class Product : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public Guid CategoryId { get; set; }
    public decimal Price { get; set; }
    public string? Currency { get; set; }
}