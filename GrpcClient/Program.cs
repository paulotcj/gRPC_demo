using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var input = new HelloRequest { Name = "Tim" };
            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(input);

            //Console.WriteLine(reply.Message);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var customerClient = new Customer.CustomerClient(channel);

            var clientRequested = new CustomerLookupModel { UserId = 2 };

            Console.WriteLine("____________________________________");
            Console.WriteLine("Requesting Customer where UserId = 2 and no delay:");
            var customer = await customerClient.GetCustomerInfoAsync(clientRequested);
            Console.WriteLine($"    {customer.FirstName} {customer.LastName}");
            Console.WriteLine("\n" + "____________________________________");
            Console.WriteLine("Customer list stream - 1800ms delay per record:");

            using (var call = customerClient.GetNewCustomer(new NewCustomerRequest())) //NewCustomerRequest is a empty request
            {
                while (await call.ResponseStream.MoveNext()) {
                    var currentCustomer = call.ResponseStream.Current;

                    Console.WriteLine($"    {currentCustomer.FirstName} {currentCustomer.LastName} : {currentCustomer.EmailAddress}");
                }
            }

            Console.WriteLine("\n"+"____________________________________" + "\nDone");
            Console.ReadLine();
        }
    }
}
