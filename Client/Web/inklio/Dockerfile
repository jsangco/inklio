FROM node:18-alpine

# RUN mkdir -p /usr/src/nuxt-app
# WORKDIR /usr/src/nuxt-app
# COPY . .
# RUN yarn install
# RUN yarn run build
COPY [".output", "."]

ENV NUXT_HOST=0.0.0.0
ENV NUXT_PORT=3000

EXPOSE 3000 

ENTRYPOINT ["node", "server/index.mjs"]