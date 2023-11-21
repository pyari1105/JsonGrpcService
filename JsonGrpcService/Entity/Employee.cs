using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace JsonGrpcService.Entity
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public string Address { get; set; } = string.Empty;
    }
}
