using Google.Protobuf;
using Grpc.Core;
using Trinity.Camp.Proto;

namespace Trinity.Camp.GrpcService.Services;

public class StreamService : Proto.StreamService.StreamServiceBase
{
    public override async Task FetchResponse(StreamRequest request, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
    {
        Random random = new Random();
        
        while (!context.CancellationToken.IsCancellationRequested)
        {
            await responseStream.WriteAsync(new StreamResponse
            {
                Result = $"Cardano price\t{random.NextDouble()}$"
            });

            Thread.Sleep(200);
        }
    }
}