# Events Web Application

# Installation guide

1. Clone the repository

```bash
    git clone https://github.com/ilya1004/EventsWebApplication.git
```

2. Go to the project folder and run it by entering the following command to cmd

```bash
    docker-compose up
```

3. Wait untill it runs...

4. When the project runs successfully you can navigate to the folowing URL to see the client side of the application

```bash
    http://localhost:3000/login
```

## API of the application

1. You can navigate to the following URL to see the main application endpoints

```bash
    http://localhost:7012/swagger/index.html
```

2. You also can navigate to the following URL to see endpoints of the identity server which is the part of project 

```bash
    http://localhost:7013/swagger/index.html
```

3. You can navigate to the following URL to see the email notification which are sended by the Papercut service

```bash
    http://localhost:8080
```

## Test users to sign in into the application

1. Admin

```bash
Email: admin@gmail.com
Password: Admin_123
```

2. Test users:

- User 1
```bash
Email: ilya@gmail.com
Password: Ilya_123
```

- User 2
```bash
Email: anna@gmail.com
Password: Anna_123
```

- User 3
```bash
Email: dmitry@gmail.com
Password: Dmitry_123
```