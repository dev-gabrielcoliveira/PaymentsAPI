# FCG.PaymentsAPI

Microsserviço responsável pelo processamento de pagamentos da plataforma FIAP Cloud Games (FCG).

## Sobre o projeto

O FCG.PaymentsAPI faz parte da arquitetura de microsserviços da plataforma FIAP Cloud Games.

Este serviço é responsável por consumir solicitações de compra de jogos, realizar o processamento simulado do pagamento e publicar o resultado da transação para os demais microsserviços.

A comunicação é realizada de forma assíncrona através de eventos utilizando RabbitMQ e MassTransit.

## Responsabilidades

- Consumir eventos de compra
- Processar pagamentos simulados
- Definir resultado da transação
- Publicar eventos de pagamento processado

## Tecnologias utilizadas

- .NET 8
- MassTransit
- RabbitMQ
- Docker
- Kubernetes
- Serilog

## Arquitetura

O projeto possui separação de responsabilidades:

- **Application**
  - Consumers de eventos
  - Serviços de aplicação
  - Processamento de pagamento
  - Publicação de eventos

- **Domain**
  - Eventos de domínio
  - Regras de negócio

- **Infrastructure**
  - Configurações externas
  - Integrações necessárias

- **API**
  - Inicialização da aplicação
  - Configuração do pipeline da aplicação

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

através do consumidor:

```
OrderPlacedConsumer
```

O evento contém informações da compra:

- UserId
- GameId
- Price

Após o recebimento, o serviço realiza a simulação do pagamento.

## PaymentProcessedEvent

Após o processamento, o PaymentsAPI publica:

```
PaymentProcessedEvent
```

Contendo o resultado da operação.

Status possíveis:

```
Approved
Rejected
```

O evento é consumido pelo:

- CatalogAPI
- NotificationsAPI

## Docker

O projeto possui Dockerfile utilizando multi-stage build.

O processo é dividido em:

1. Compilação utilizando o SDK do .NET.
2. Execução utilizando somente o runtime necessário.

Benefícios:

- Imagem final menor.
- Melhor segurança.
- Ambiente otimizado para produção.

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

## Execução

### Docker Compose

```bash
docker compose up
```

### Kubernetes

Aplicar os manifestos:

```bash
kubectl apply -f k8s/
```

Verificar Pods:

```bash
kubectl get pods
```

Visualizar logs:

```bash
kubectl logs <nome-do-pod>
```

## Observabilidade

A aplicação utiliza Serilog para geração de logs estruturados.

Os logs permitem acompanhar:

- Consumo dos eventos.
- Processamento das transações.
- Publicação dos eventos de pagamento.

## Objetivo do serviço

O FCG.PaymentsAPI representa o microsserviço responsável pelo processamento de pagamentos dentro da plataforma FIAP Cloud Games, mantendo baixo acoplamento através de arquitetura orientada a eventos.