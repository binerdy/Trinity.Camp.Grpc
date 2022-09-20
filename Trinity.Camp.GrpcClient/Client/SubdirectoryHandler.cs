namespace Trinity.Camp.GrpcClient.Client;

internal class SubdirectoryHandler : DelegatingHandler
{
    private readonly string _subdirectory;
    public SubdirectoryHandler(HttpMessageHandler innerHandler, string subdirectory)
        : base(innerHandler)
    {
        _subdirectory = subdirectory;
    }
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var old = request.RequestUri ?? new Uri(string.Empty);
        var url = $"{old.Scheme}://{old.Host}:{old.Port}";
        url += $"{_subdirectory}{request.RequestUri?.AbsolutePath}";
        request.RequestUri = new Uri(url, UriKind.Absolute);
        return base.SendAsync(request, cancellationToken);
    }
}