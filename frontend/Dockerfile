FROM node:lts-alpine AS build-stage

WORKDIR /app
RUN chmod -R 755 /app

COPY package*.json quasar.config.ts ./
COPY ./ ./

RUN npm install
RUN npm install -g @vue/cli
RUN npm install -g @quasar/cli@latest

ENV VITE_CHAT_API=$VITE_CHAT_API
ENV VITE_BASE_URL=$VITE_BASE_URL

RUN CI=true npx quasar build -m spa && \ 
    npm cache clean --force && \ 
    rm -rf /root/.npm && \ 
    rm -rf node_modules

FROM nginx:stable-alpine as prod-stage

COPY --from=build-stage /app/dist/spa /usr/share/nginx/html
COPY /nginx/default.conf /etc/nginx/conf.d/default.conf
COPY /nginx/nginx.conf /etc/nginx/nginx.conf


EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]