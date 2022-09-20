using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trinity.Camp.GrpcClient.Client;

var host = Host
    .CreateDefaultBuilder()
    .ConfigureAppConfiguration(app => { app.AddJsonFile("appsettings.json"); })
    .ConfigureServices(services => { services.AddSingleton<ClientWorker>(); })
    .Build();

var clientWorker = host.Services.GetRequiredService<ClientWorker>();
await clientWorker.RunAsync();
