using MongoDB.Bson.Serialization.Attributes;

namespace HepsiFlyCaseStudy.Models;

public abstract class BaseEntity
{
    [BsonId] public Guid Id { get; set; }
}