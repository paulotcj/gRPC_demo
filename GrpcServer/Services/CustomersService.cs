using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcServer.Services
//namespace GrpcServer
{
    public class CustomersService : Customer.CustomerBase
    {
        private readonly ILogger<CustomersService> _logger;
        public CustomersService(ILogger<CustomersService> logger)
        {
            _logger = logger;

        }

        public override Task<CustomerModel> GetCustomerInfo(CustomerLookupModel request, ServerCallContext context)
        {
            CustomerModel output = new CustomerModel();
            if (request.UserId == 1) {
                output.FirstName = "Elsa";
                output.LastName = "Bradshaw";
            }
            else if (request.UserId == 2) {
                output.FirstName = "Jermaine";
                output.LastName = "Myers";
            }
            else {
                output.FirstName = "Aaron";
                output.LastName = "Valencia";
            }

            return Task.FromResult(output);

        }

        public override async Task GetNewCustomer(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel {
                    FirstName = "Tyreese",
                    LastName = "Mcmanus",
                    EmailAddress = "tyreese@mcmanus.com",
                    Age = 45,
                    IsAlive = true
                },
                new CustomerModel {
                    FirstName = "Anoushka",
                    LastName = "Cohen",
                    EmailAddress = "anoushka@cohen.net",
                    Age = 39,
                    IsAlive = true
                },
                new CustomerModel {
                    FirstName = "Calum",
                    LastName = "Chung",
                    EmailAddress = "calum@chung.net",
                    Age = 24,
                    IsAlive = true
                },
            };

            foreach (var cust in customers)
            {
                await Task.Delay(1800);
                await responseStream.WriteAsync(cust);
            }
        }
    }
}
