FROM node:20-alpine AS build
WORKDIR /src
COPY package*.json .
RUN npm ci
COPY . .
RUN npm run build
