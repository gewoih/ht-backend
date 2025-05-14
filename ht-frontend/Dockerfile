FROM node:alpine AS build
WORKDIR /src
COPY package*.json .
RUN npm ci
COPY . .

ARG VITE_API_BASE_URL
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}
RUN npm run build

FROM nginx:alpine
COPY --from=build /src/dist /usr/share/nginx/html
