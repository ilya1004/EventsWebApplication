## BUILD STAGE

# FROM node:alpine as build

FROM node:19

WORKDIR /usr/src/app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build

EXPOSE 3000

CMD [ "npm", "start" ]

## UP NGINX STAGE

# FROM nginx:alpine

# COPY --from=build /usr/src/app/dist /usr/share/nginx/html

# COPY --from=build /usr/src/app/nginx.conf /etc/nginx/conf.d/default.conf

# EXPOSE 3000

# CMD ["nginx", "-g", "daemon off;"]