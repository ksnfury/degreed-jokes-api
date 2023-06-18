# Joke API

A simple API for retrieving and managing jokes.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)


## Prerequisites

To run the Joke API application, ensure you have the following prerequisites installed on your machine:

- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [MySQL Server](https://dev.mysql.com/downloads/)

## Getting Started

Follow the steps below to get the Joke API up and running on your local machine:

1. Clone this repository to your local machine.

2. Open the `appsettings.json` file located in the `JokeApi` project folder.

3. Update the database connection string in the `"ConnectionStrings"` section to match your MySQL server configuration.

4. Open a terminal or command prompt and navigate to the `JokeApi` project folder.

5. Run the following command to build and run the application:
   dotnet build
   dotnet run

6. The Joke API will now be running locally at `http://localhost:5275`.

## API Documentation

The API documentation provides details about the available endpoints and how to use them.
You can access the documentation by navigating to `http://localhost:5275/swagger` in your web browser while the application is running.

<img width="1488" alt="image" src="https://github.com/ksnfury/degreed-jokes-api/assets/8427974/30fedfc4-8ca8-4581-8ffb-40966bfc57b6">

First go to the login API. Enter the username/password=dummy/password and get the bearer token
Next, click authorize in the top right corner and enter the token:

<img width="1520" alt="image" src="https://github.com/ksnfury/degreed-jokes-api/assets/8427974/528f9e31-b77a-4c20-8adc-51da87e19b1e">

1. Try the random joke API. This doesn't require authentication
2. Try the search API with a search term for a Joke like 'Dad'

# UI Documentation

Try the UI at JokeApi/HTML/jokeClient.html. This will provide the same functionality as above. 
<img width="1012" alt="image" src="https://github.com/ksnfury/degreed-jokes-api/assets/8427974/206dd04d-3397-406e-ad29-22d2fff93338">
Try a random Joke:
<img width="1105" alt="image" src="https://github.com/ksnfury/degreed-jokes-api/assets/8427974/53dc36b0-dfdf-46f6-ac91-2b2b39362e78">
Try a search term:
<img width="933" alt="image" src="https://github.com/ksnfury/degreed-jokes-api/assets/8427974/237c7cae-c129-4656-90e6-efedd6b1c869">

# Some design priniciples considered :

1. Separation of Concerns: The application follows the principle of separation of concerns by dividing functionality into distinct components such as controllers, services, and middleware. This promotes modular and maintainable code.

2. API Versioning: We have implemented API versioning to support future changes and updates to the API without breaking existing client applications. This allows for backward compatibility and smooth transitions between API versions.

3. Swagger Documentation: We have integrated Swagger UI to provide interactive API documentation. Swagger helps in visualizing and testing the API endpoints, making it easier for developers to understand and consume the API.

4. Exception Handling: The application includes centralized exception handling to handle and log any exceptions that occur during the execution of API requests. This ensures proper error responses are returned to clients and facilitates troubleshooting and debugging.

5. Authentication and Authorization: We have added token-based authentication using JWT (JSON Web Tokens) to secure the API endpoints. The application validates the incoming JWT token and authorizes access based on user roles and permissions.

6. Dependency Injection: The application uses dependency injection to manage and resolve dependencies between components. This promotes loose coupling and facilitates unit testing and extensibility.

Others which I considered but could not complete are:

1. Rate limiting - This would be really nice to have to prevent abuse of resources
2. Unit/Integration Tests - these would ensure that the App goes into production with confidence


