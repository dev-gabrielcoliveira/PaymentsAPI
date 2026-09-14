using Azure.Storage.Queues;
using FCG.Application.Events;
using MassTransit;
using System.Text.Json;

namespace FCG.PaymentsAPI.Application.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedConsumer> _logger;
        private readonly QueueClient _queueClient;

        public OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger, QueueClient queueClient)
        {
            _logger = logger;
            _queueClient = queueClient;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var order = context.Message;

            _logger.LogInformation("[PaymentsAPI] Processando pagamento. Usuário: {UserId}, Jogo: {GameId}, Valor: {Price}",
                order.UserId,
                order.GameId,
                order.Price);

            var aprovado = true;

            var paymentProcessedEvent = new PaymentProcessedEvent
            {
                UserId = order.UserId,
                GameId = order.GameId,
                Price = order.Price,
                Status = aprovado ? "Approved" : "Rejected"
            };

            // 1. Mantém a comunicação interna no RabbitMQ para outros microsserviços do Core
            await context.Publish(paymentProcessedEvent);

            // 2. Monta o DTO e envia para a fila do Azure que dispara a Azure Function
            string mensagem = $"Usuário (ID: {order.UserId}) | Atualização do seu Pedido | O pagamento do seu jogo (ID: {order.GameId}) foi {(aprovado ? "Aprovado" : "Recusado")}";

            var queueClient = new QueueClient(
                "UseDevelopmentStorage=true",
                "notifications-v3",
                new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64 }
            );

            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(mensagem);

            _logger.LogInformation("[PaymentsAPI] Pagamento concluído. Evento publicado no RabbitMQ e notificação enfileirada no Azure Queue.");

        }
    }
}