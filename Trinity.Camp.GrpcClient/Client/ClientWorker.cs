using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Trinity.Camp.Proto;
using static Trinity.Camp.Proto.StreamService;
using Grpc.Core;

namespace Trinity.Camp.GrpcClient.Client;

internal class ClientWorker
{
    private readonly IConfiguration _configuration;

    public ClientWorker(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Run()
    {
        var proxyUrl = _configuration.GetSection("ProxyUrl").Value;
        var gatewayPrefix = _configuration.GetSection("GatewayPrefix").Value;

        var handler = new SubdirectoryHandler(new HttpClientHandler(), gatewayPrefix);
        var channel = GrpcChannel.ForAddress(proxyUrl, new GrpcChannelOptions
        {
            HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, handler),
        });

        var client = new StreamServiceClient(channel);
        var asyncServerCall = client.FetchResponse(new StreamRequest { Id = 21 });

        while (await asyncServerCall.ResponseStream.MoveNext())
        {
            var response = asyncServerCall.ResponseStream.Current;
            Console.WriteLine(response.Result);
        }
    }
}