# TransactionAudit

TransactionAudit is a tool for keeping financial transaction info for the purpose of using them for aggregation queries.

## Prerequisites
Docker and docker-compose.

## Run

```bash
docker-compose up
```

## Uninstall

```bash
docker-compose down
```

## Debugging
### Probanx.TransactionAudit.Web
Start the needed containers.
```
    docker-compose up rabbitMQ elasticSearch consumer
```

Set the environment vars.
```
    "ELASTIC_HOST_URL": "http://localhost:9200",
    "RABBIT_MQ_HOST_NAME": "localhost"
```

And debug as usual.

### Probanx.TransactionAudit.Consumer
Start the needed containers.
```
    docker-compose up rabbitMQ elasticSearch web
```

Set the environment vars.
```
    "ELASTIC_HOST_URL": "http://localhost:9200",
    "RABBIT_MQ_HOST_NAME": "localhost"
```

And debug as usual.
