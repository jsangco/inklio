# Inklio Front-End Web

Look at the [Nuxt 3 documentation](https://nuxt.com/docs/getting-started/introduction) to learn more.

Please use the following [naming conventions](naming.md).

## Prerequisites
 1. Must have [Node.js](https://nodejs.org/en) installed.
 2. Yarn (`npm install -g yarn`)

## Local Development

### 1. Build inklio solution

From the root directory, build [Inklio.sln](../../../Inklio.sln) file in `Release`.

`dotnet build .\Inklio.sln -c Release`

### 2. Run docker compose

`docker compose up --build`

| **NOTE:** The `--build` is only needed the first time `docker compose up` is run.

### 3. Host the development inklio web front-end

From the `./Client/Web/inklio` directory run:

```bash
yarn dev
```

### 4. Verify development changes are reflected on the site.

* Open `http://localhost` in a web browser and verify the page loads.
* Modify the [FrontEnd.vue](./components/FrontPage.vue) file and verify the site reflects the changes

> **NOTE:** When using the reverse-proxy for debugging, using HTTP, and *not* HTTP**S** is required.

### 5. (Optional) Fill the site with test content

A freshly initialized site has no content, but you can generate test content by running the [Inklio.Console.Test](../../../Services/Inklio.Console.Test/) app.

```bash
cd ./Services/Inklio.Console.Test
dotnet run
```

## Building the Docker container

The build pipeline will automatically build the docker container, but it can also be built locally.

1. Run `yarn build` from the inklio web front-end directory
2. Run `docker build . -t inklio-web:latest`

> **WARNING:** The environment variables used at build time are what get stuck in the docker container. If building a production container on a local dev machine, **make sure the enviroment variables match the production environment when building**. The build pipeline should automatically handle this, so there should be no need to push from a local dev machine
