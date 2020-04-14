FROM node:13.12-alpine AS builder
WORKDIR /app
COPY . .
RUN npm install react-scripts -g --silent
RUN npm install
RUN npm run build

FROM node:13.12-alpine
RUN npm install serve -g
WORKDIR /app
EXPOSE 80
COPY --from=builder /app/build .
CMD ["serve", "-l", "80", "-s", "."]doc