# PaymentsAPI

> Microsserviço responsável pelo processamento de pagamentos, validação financeira e integração assíncrona com o ecossistema FIAP Cloud Games (FCG).

---

## 💡 Sobre o projeto

O **PaymentsAPI** é o microsserviço encarregado de intermediar e processar a cobrança de jogos na plataforma. 

Ele atua de forma totalmente assíncrona, reagindo aos pedidos de compra iniciados no catálogo, efetuando o processamento financeiro e disparando atualizações tanto para a liberação dos jogos quanto para a notificação do cliente via arquitetura serverless.

---

## 🎯 Responsabilidades

- **Processamento Financeiro:** Recebimento e processamento de transações de pagamento.
- **Consumo de Eventos:** Consumo de pedidos de compra via RabbitMQ.
- **Publicação de Resultados:** Emissão de confirmações de pagamento via RabbitMQ para atualização de biblioteca.
- **Integração Serverless:** Enfileiramento de mensagens no Azure Storage Queue para disparo de notificações.

---

## 🛠️ Tecnologias Utilizadas

- **.NET 8** (ASP.NET Core Web API)
- **Entity Framework Core** & **SQL Server**
- **MassTransit** & **RabbitMQ** (Eventos de domínio)
- **Azure Storage Queues** & **Azure Functions** (Processamento de notificações)
- **Docker** & **Kubernetes**
- **Serilog**, **Prometheus** & **Grafana** (Observabilidade)

---

## 🏗️ Arquitetura Interna

O projeto adota Clean Architecture com separação clara de responsabilidades:

- **API:** Controllers, endpoints de auditoria de pagamentos e middlewares.
- **Application:** Casos de uso de cobrança, handlers de eventos e DTOs.
- **Domain:** Regras de negócio de transações financeiras e modelos de pagamento.
- **Infrastructure:** Persistência no SQL Server, comunicação com RabbitMQ e integração com Azure Storage Queues.

---

## 🔄 Mensageria e Eventos de Domínio

O **PaymentsAPI** atua como ponto central no fluxo de mensagens, conectando o barramento RabbitMQ ao modelo Serverless do Azure Storage Queue.

```text
[CatalogAPI] --(RabbitMQ: OrderPlacedEvent)--> [PaymentsAPI]
                                                      |
    +-------------------------------------------------+---------------------------------------+
    | (RabbitMQ: PaymentProcessedEvent)                                                       | (Azure Queue: notifications-v3)
                                                                                              ↓
                                                                       [NotificationsAPI.Serverless] (Envia notificação)
