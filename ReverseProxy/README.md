# Introduction
An nginx reverse proxy used to route requests to the correct microservices. The reverse proxy allows all Inklio applications to be hosted on the same web domain which helps simplify some things like, authentication, CORS, certs, etc...


## Configuration Details

The table below provides details on the mapping used for Inklio applications. The Nginx Reverse Proxy listens on web port 80 and routes
to the other applications using their **docker compose URL and port**. The mapping on the Docker compose URL can be determined by looking at the application name in the [docker compose file](../docker-compose.yml) and adding `http://`.  Please see the [nginx.conf](./nginx.conf) file for more details on the configuration.

| Route        | Application  | Docker Compose URL | Port         | FQDN URL                              |
| -----------  | -----------  | -----------        | -----------  | -----------                           |
| `/`          | FE Web       | http://web         | 3000         | `inklio.azurewebsite.net`             |
| `/api`       | Inklio.Api   | http://api         | 5000         | `inklio.azurewebsite.net/api/v1/asks` |
| `/auth `     | Inklio.Auth  | http://auth        | 2000         | `inklio.azurewebsite.net/auth/login`  |

> NOTE: Directly referencing the docker compose url in the `proxy_pass` of the [nginx.conf](./nginx.conf) doesn't always work, and an `upstream` must be used for the routing to work. For example `http://api` doesn't work, but creating an upstream of `upstream inklio-api { server api:5000; }` and then setting the `proxy_pass` to `http://inklio-api` will work. Please see the [nginx.conf](./nginx.conf) for an example of the proper configuration.

## Building Docker

1. `cd /ReverseProxy/`
2. `docker build . -t inklio.azurecr.io/docker/reverseproxy:latest`