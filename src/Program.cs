using Azure.Storage.Queues;
using FCG.PaymentsAPI.Application.Consumers;
using MassTransit;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    builder.Services.AddSingleton(sp => new QueueClient(
        "UseDevelopmentStorage=true",
        "notifications-v3",
        new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 }
    ));

    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitHost = builder.Configuration["RabbitMq:Host"] ?? "localhost";

        cfg.Host(rabbitHost, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.UseRawJsonSerializer();
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
// Captura todas requisições que entram na API
app.UseHttpMetrics();
// Mapeamento do endpoint que o prometheus vai espiar (ex: /metrics)
app.MapMetrics();

app.MapGet("/", () => "Payments API Rodando");

app.Run();