using JsonGrpcService.Entity;
using JsonGrpcService.Interface;
using Grpc.Core;

namespace JsonGrpcService.Services
{
    public class EmployeeDetailService : EmployeeDetail.EmployeeDetailBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeDetailService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        public override async Task<CreateEmployeeResponse> CreateEmployee(CreateEmployeeRequest request, ServerCallContext context)
        {
            Employee emp = new Employee()
            {
                Name = request.Name,
                Age = request.Age,
                Address = request.Address
            };
            var response = await _employeeService.CreateAsync(emp);

            return await Task.FromResult(new CreateEmployeeResponse()
            {
                Id = response.Id
            });
        }
        public override async Task<GetResponse> GetEmployee(GetRequest request, ServerCallContext context)
        {
            var response = await _employeeService.GetByIdAsync(request.Id);

            return await Task.FromResult(new GetResponse()
            {
                Id = response.Id,
                Name = response.Name,
                Age = response.Age,
                Address = response.Address
            });
        }
        public override async Task<GetAllResponse> GetAllEmployees(GetAllRequest request, ServerCallContext context)
        {
            var response = new GetAllResponse();
            var employees = await _employeeService.GetAllAsync();
            foreach(var emp in employees)
            {
                response.Employees.Add(new GetResponse
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Age = emp.Age,
                    Address = emp.Address
                });
            }

            return await Task.FromResult(response);
        }
        public override async Task<UpdateResponse> UpdateEmployee(UpdateRequest request, ServerCallContext context)
        {
            Employee emp = new Employee()
            {
                Id = request.Id,
                Name = request.Name,
                Age = request.Age,
                Address = request.Address
            };
            await _employeeService.UpdateAsync(request.Id, emp);

            return await Task.FromResult(new UpdateResponse()
            {
                Id = request.Id
            });
        }
        public override async Task<DeleteResponse> DeleteEmployee(DeleteRequest request, ServerCallContext context)
        {
           await _employeeService.DeleteAsync(request.Id);
            
            return await Task.FromResult(new DeleteResponse
            {
                Id= request.Id
            });
        }
    }
    
}

