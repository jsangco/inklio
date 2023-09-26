# Inklio Front-End Web

Look at the [Nuxt 3 documentation](https://nuxt.com/docs/getting-started/introduction) to learn more.

Please use the following [naming conventions](naming.md).

## Prerequisites
 1. Must have [Node.js](https://nodejs.org/en) installed.
 2. Yarn (`npm install -g yarn`)

## Setup

Make sure to install the dependencies:

```bash
yarn install
```

## Local Development

### 1. Run the front end application

```bash
yarn dev --host
```

### 2. Configure the reverse proxy

Once the application is running you will be presented with something like the following information:
``` bash
  > Local:    http://localhost:3000/
  > Network:  http://172.20.16.1:3000/
  > Network:  http://172.30.176.1:3000/
  > Network:  http://192.168.4.28:3000/
```
Take the **Network** IP and PORT (i.e. `http://172.20.16.1:3000/`), and *replace* the existing `proxy_pass` value in the `Web` section of the [nginx.conf](../../ReverseProxy/nginx.conf). If you do not see the `Network` option, double check that `--host` was properly appended to the `yarn dev` command.

It should look something like this:
```
# Web
location / {
  expires $expires;

  proxy_redirect                      off;
  proxy_set_header X-Forwarded-Proto  $scheme;
  proxy_read_timeout          1m;
  proxy_connect_timeout       1m;
  proxy_pass   http://172.20.16.1:3000/; # <-- Set this
  # proxy_pass http://inklio-web/;         <-- Remove this
}
```

### 3. Build the back-end services and run Docker
Run `dotnet build -c Release .\Inklio.sln` from the project directory.

Run `docker compose up --build` from the [docker-compose.yml](./docker-compose.yml) file directory. If the application starts correctly you should be able to navigate to [http://localhost](http://localhost) and begin debugging.

> **NOTE:** This weird configuration is necessary to ensure there are no [CORS](https://developer.mozilla.org/en-US/docs/Web/HTTP/CORS) related problems when running in docker compose.


## Production

Build the application for production:

```bash
yarn build
```

Locally preview production build:

```bash
yarn preview
```

Check out the [deployment documentation](https://nuxt.com/docs/getting-started/deployment) for more information.

> WARNING: The environment variables used at build time are what get stuck in the docker container. Make sure the enviroment variables match the production environment when building. The build pipeline should automatically handle this, so don't push from a dev environment.
