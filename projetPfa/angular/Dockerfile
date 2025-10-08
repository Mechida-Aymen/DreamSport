FROM node:18-alpine

WORKDIR /app

COPY angular/package.json angular/package-lock.json ./
RUN npm install

COPY angular/ ./

COPY angular/localhost.crt angular/localhost.key ./

EXPOSE 4200

CMD ["npx", "ng", "serve", "--ssl", "true", "--ssl-cert", "localhost.crt", "--ssl-key", "localhost.key", "--host", "0.0.0.0", "--port", "4200"]
