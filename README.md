# FCG.PaymentsAPI

Microsserviço responsável pelo processamento de pagamentos da plataforma FIAP Cloud Games (FCG).

## Sobre o projeto

O FCG.PaymentsAPI faz parte da arquitetura de microsserviços da plataforma FIAP Cloud Games.

Este serviço é responsável por processar as solicitações de pagamento das compras de jogos, simulando a aprovação ou rejeição de uma transação.

A comunicação com os demais microsserviços é realizada de forma assíncrona utilizando eventos através do RabbitMQ.

## Responsabilidades

- Consumir solicitações de compra
- Processar pagamentos simulados
- Gerar resultado da transação
- Publicar eventos de processamento de pagamento

## Tecnologias utilizadas

- .NET 8
- ASP.NET Core Web API
- MassTransit
- RabbitMQ
- Docker
- Kubernetes
- Serilog

## Arquitetura

O projeto possui separação de responsabilidades:

- **API**
  - Controllers
  - Endpoints HTTP

- **Application**
  - Consumers
  - Serviços de aplicação
  - Processamento de pagamento

- **Domain**
  - Entidades
  - Regras de negócio

- **Infrastructure**
  - Configurações externas
  - Integrações

## Mensageria

O PaymentsAPI participa do fluxo de compra utilizando comunicação orientada a eventos.

Fluxo:

```text
CatalogAPI
    |
    | OrderPlacedEvent
    ↓
RabbitMQ
    ↓
PaymentsAPI
    |
    | PaymentProcessedEvent
    ↓
RabbitMQ
    ↓
CatalogAPI
+
NotificationsAPI
```

## OrderPlacedEvent

O serviço consome o evento:

```
OrderPlacedEvent
```

Recebido pelo consumidor:

```
OrderPlacedConsumer
```

O evento contém informações necessárias para processamento:

- UserId
- GameId
- Price

Após o processamento, o pagamento é aprovado ou rejeitado.

## PaymentProcessedEvent

Após concluir a simulação do pagamento, o serviço publica:

```
PaymentProcessedEvent
```

Com informações como:

- Identificação da compra
- Status do pagamento
- Resultado da operação

Status possíveis:

```
Approved
Rejected
```

## Docker

O projeto possui Dockerfile utilizando multi-stage build.

A construção da imagem é dividida em:

1. Compilação utilizando o SDK do .NET.
2. Execução utilizando apenas o runtime necessário.

Benefícios:

- Imagens menores.
- Melhor segurança.
- Ambiente de produção otimizado.

## Kubernetes

Os manifestos Kubernetes estão disponíveis na pasta:

```
/k8s
```

Recursos utilizados:

- Deployment
- Service
- ConfigMap
- Secret

## Execução local

### Docker Compose

```bash
docker compose up
```

### Kubernetes

```bash
kubectl apply -f k8s/
```

Verificar execução:

```bash
kubectl get pods
```

Logs:

```bash
kubectl logs <nome-do-pod>
```

## Observabilidade

A aplicação utiliza Serilog para geração de logs estruturados.

Os logs permitem acompanhar o processamento dos eventos e o funcionamento do consumidor.

## Objetivo do serviço

O FCG.PaymentsAPI representa o microsserviço responsável pelo processamento de pagamentos da plataforma FIAP Cloud Games, mantendo baixo acoplamento através da comunicação orientada a eventos.