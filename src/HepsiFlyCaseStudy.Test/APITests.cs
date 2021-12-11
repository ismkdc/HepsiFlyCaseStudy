using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Commands.Response;
using Newtonsoft.Json;
using Xunit;

namespace HepsiFlyCaseStudy.Test;

public class APITests
{
    [Fact]
    public async Task<Guid> CreateCategoryTest()
    {
        var client = new TestClientProvider().Client;
        var result = await client.PostAsJsonAsync("/api/categories",
            new CreateCategoryCommandRequest
            {
                Name = "test category",
                Description = "test description"
            });

        result.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);

        var str = await result.Content.ReadAsStringAsync();
        var obj = JsonConvert.DeserializeObject<CreateCategoryCommandResponse>(str);

        return obj.Id;
    }
    
    [Fact]
    public async Task CreateProduct()
    {
        var categoryId = await CreateCategoryTest();
        var client = new TestClientProvider().Client;
        var result = await client.PostAsJsonAsync("/api/products",
            new CreateProductCommandRequest()
            {
                Name = "test category",
                Description = "test description",
                CategoryId = categoryId,
                Currency = "tl",
                Price = 1
            });
        
        result.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }
    
    [Fact]
    public async Task GetCategories()
    {
        var client = new TestClientProvider().Client;

        var result = await client.GetAsync("/api/categories");
        result.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
    
    [Fact]
    public async Task GetProducts()
    {
        var client = new TestClientProvider().Client;

        var result = await client.GetAsync("/api/products");
        result.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}