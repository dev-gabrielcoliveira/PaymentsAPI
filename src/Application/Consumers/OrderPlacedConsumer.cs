using FCG.Application.Events;
using MassTransit;

namespace FCG.PaymentsAPI.Application.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(ILogger<OrderPlacedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderPlacedEvent> context)
        {
            var order = context.Message;

            _logger.LogInformation(
                "[PaymentsAPI] Processando pagamento. Usuário: {UserId}, Jogo: {GameId}, Valor: {Price}",
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

            await context.Publish(paymentProcessedEvent);

            _logger.LogInformation(
                "[NotificationsAPI] Notificação de compra enviada. Status: {Status} Usuário: {UserId}, Jogo: {GameId}",
                paymentProcessedEvent.Status, 
                order.UserId,
                order.GameId);
        }
    }
}