using Trinity.Camp.Gateway;

var builder = WebApplication.CreateBuilder(args);
var proxyBuilder = builder.Services.AddReverseProxy();
proxyBuilder.LoadFromConfig(builder.Configuration.GetSection(ConfigSection.ReverseProxy));

var app = builder.Build();
app.MapReverseProxy();
app.Run();
