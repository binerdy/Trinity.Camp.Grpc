using Grpc.Core;
using Trinity.Camp.Proto;

namespace Trinity.Camp.GrpcService.Services;

public class StreamService : Proto.StreamService.StreamServiceBase
{
    public override async Task FetchResponse(StreamRequest streamRequest, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
    {
        var client = new HttpClient();

        while (!context.CancellationToken.IsCancellationRequested)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://www.binance.com/api/v3/ticker/price?symbol={streamRequest.Symbol}")
            };

            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<StreamResponse>();

            await responseStream.WriteAsync(new StreamResponse
            {
                Symbol = content?.Symbol ?? "Unknown",
                Price = content?.Price ?? "Unknown"
            });

            Thread.Sleep(1000);
        }
    }
}