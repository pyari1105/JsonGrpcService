namespace JsonGrpcService.Models
{
    public class EmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; } = 0;
        public string Address { get; set; } = string.Empty;
    }
}
