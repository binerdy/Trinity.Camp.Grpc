using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Trinity.Camp.GrpcClient;
using Trinity.Camp.Proto;

namespace Trinity.Camp.GrpcClient
{
    internal class Program
    {
        static AsyncServerStreamingCall<StreamResponse> _call;

        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7121");
            var client = new StreamService.StreamServiceClient(channel);

            // You can set a deadline for your call by deadLine parameter.
            _call = client.FetchResponse(new StreamRequest
                {
                    Id = 1
                },
                deadline: DateTime.UtcNow.AddSeconds(10));

            _ = Task.Run(async () =>
            {
                while (await _call.ResponseStream.MoveNext())
                {
                    Console.WriteLine(_call.ResponseStream.Current);
                }
            });
        }
    }
}
