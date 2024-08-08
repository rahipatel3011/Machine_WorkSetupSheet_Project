Our project endeavors to enhance the management of machine setups within machine shops by simplifying the complexities associated with machine configuration for diverse production tasks. Through robust tools for setup data management and optimization, the solution aims to minimize downtime and elevate operational efficiency. Centralizing setup configurations and monitoring metrics empowers machine shop operators with actionable insights, enabling them to make informed decisions that drive productivity gains.

## Project Overview

Our project aims to streamline the management of machine setups within machine shops. It addresses the complexities involved in setting up and configuring machines for various production tasks. The solution provides tools to manage setup data and optimize machine setup processes to minimize downtime and improve operational efficiency. By centralizing setup configurations and monitoring metrics, it empowers machine shop operators to make informed decisions and enhance productivity.

### How to Run

To run this project locally using Docker Compose, follow these steps:

1. Create docker-compose.yml:
```bash
version: '3.4'
services:
  redis:
    image: redis:latest
    container_name: redis_container
    ports:
      - "6379:6379"
    volumes:
      - redisdata:/data

  machine_setup_db:
    image: mcr.microsoft.com/mssql/server:latest
    container_name: sql_server_container
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword
    ports:
      - "1433:1433"
    volumes:
      - sqlserverdata:/var/opt/mssql

  machine_setup:
    container_name: my_app
    image: rahiblogger/machine_setup_app:v1
    ports:
      - "8080:8080"
    depends_on:
      - machine_setup_db
      - redis
    volumes:
      - ./Machine_Setup_worksheet/DataProtection-Keys:/app/DataProtection-Keys
    environment:
      - ConnectionStrings__DefaultConnection=Server=machine_setup_db;Database={DatabaseName};User ID=sa;Password={yourpassword};TrustServerCertificate=true;

volumes:
  dataprotection-keys:
  sqlserverdata:
  redisdata:
```

2. Modify the Docker Compose file (docker-compose.yml) as needed for your environment.

3. Build and start the Docker containers:
```bash
docker-compose up -d
```

4. Access the application:
Machine Setup App: Open your web browser and go to http://localhost:8080

5. To stop and remove containers:
```bash
docker-compose down
```
