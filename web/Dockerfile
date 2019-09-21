FROM node:12.2.0-alpine

WORKDIR /app

# NOTE: Copying here shouldn't be necessary. Should be checked in the future.
COPY . /app

RUN npm install && npm run build

CMD ["npx", "serve", "-l", "tcp://0.0.0.0:5000", "-s", "build"]
