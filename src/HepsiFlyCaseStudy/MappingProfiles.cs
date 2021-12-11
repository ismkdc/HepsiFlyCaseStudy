using AutoMapper;
using HepsiFlyCaseStudy.CQRS.Commands.Request;
using HepsiFlyCaseStudy.CQRS.Queries.Response;
using HepsiFlyCaseStudy.Models;

namespace HepsiFlyCaseStudy;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateProductCommandRequest, Product>();
        CreateMap<Product, ListProductsQueryResponse>();
        CreateMap<Product, GetProductQueryResponse>();

        CreateMap<CreateCategoryCommandRequest, Category>();
        CreateMap<Category, ListCategoriesQueryResponse>();
        CreateMap<Category, GetCategoryQueryResponse>();
    }
}