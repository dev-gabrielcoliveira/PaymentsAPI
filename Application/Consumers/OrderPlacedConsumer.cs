using FCG.Application.Events;
using MassTransit;

namespace FCG.PaymentsAPI.Application.Consumers
{
    public class OrderPlacedConsumer : IConsumer<OrderPlacedEvent>
    {
        private readonly ILogger<OrderPlacedConsumer> _logger;

        public OrderPlacedConsumer(
            ILogger<OrderPlacedConsumer> logger)
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


            // Simulação de pagamento aprovado
            var paymentProcessedEvent = new PaymentProcessedEvent
            {
                UserId = order.UserId,
                GameId = order.GameId,
                Price = order.Price,
                Status = "Approved"
            };


            await context.Publish(paymentProcessedEvent);
        }
    }
}