using JsonGrpcService.Entity;
using JsonGrpcService.Interface;
using JsonGrpcService.Models;
using MongoDB.Driver;

namespace JsonGrpcService.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IMongoCollection<Employee> _employee;
        public EmployeeService(IDemoDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _employee = database.GetCollection<Employee>(settings.EmployeesCollectionName);
        }
        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employee.Find(s => true).ToListAsync();
        }
        public async Task<Employee> GetByIdAsync(string id)
        {
            return await _employee.Find<Employee>(c => c.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Employee> CreateAsync(Employee employee)
        {
            await _employee.InsertOneAsync(employee);
            return employee;
        }
        public async Task UpdateAsync(string id, Employee employee)
        {
            await _employee.ReplaceOneAsync(c => c.Id == id, employee);
        }
        public async Task DeleteAsync(string id)
        {
            await _employee.DeleteOneAsync(c => c.Id == id);
        }

    }
}
