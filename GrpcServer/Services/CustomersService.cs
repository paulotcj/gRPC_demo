﻿using Grpc.Core;
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
                output.FirstName = "Jamie";
                output.LastName = "Smith";
            }
            else if (request.UserId == 2) {
                output.FirstName = "Jane";
                output.LastName = "Doe";
            }
            else {
                output.FirstName = "Greg";
                output.LastName = "Thomas";
            }

            return Task.FromResult(output);

        }

        public override async Task GetNewCustomer(NewCustomerRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            List<CustomerModel> customers = new List<CustomerModel>
            {
                new CustomerModel {
                    FirstName = "Tim",
                    LastName = "Corey",
                    EmailAddress = "tim@iamtimcorey.com",
                    Age = 41,
                    IsAlive = true
                },
                new CustomerModel {
                    FirstName = "Sue",
                    LastName = "Storm",
                    EmailAddress = "sue@stormy.net",
                    Age = 28,
                    IsAlive = true
                },
                new CustomerModel {
                    FirstName = "Bilbo",
                    LastName = "Baggins",
                    EmailAddress = "bilbo@middleearth.net",
                    Age = 117,
                    IsAlive = true
                },
            };

            foreach (var cust in customers)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(cust);
            }
        }
    }
}
