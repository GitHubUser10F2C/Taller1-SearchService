## Database

The MongoDB database used in this project is designed to be **denormalized**, meaning it contains data redundancy. This structure allows for greater efficiency in queries and faster response times for requests, especially in applications that require frequent access to related data.

### Recommendations

- **Data Synchronization**: It is recommended to synchronize the data from the main application to ensure consistency and data integrity between services. This will help minimize potential inconsistencies that may arise due to the inherent redundancy of the denormalized structure.
- **Monitoring**: Implement adequate monitoring to detect possible synchronization issues and ensure that the data accurately reflects the state of the main application.

## Contributors
1. Nelson Eduardo Soto Sánchez / 19.962.608-6 / nelson.soto@alumnos.ucn.cl


## Installation

Before getting started, ensure that you have .NET SDK 8 installed.

To verify if you have the SDK 8 installed, run the following command in your terminal:

```
dotnet --version
```

## Quick Start

1. Clone this repository to your local machine:

```
git clone https://github.com/GitHubUser10F2C/Taller1-SearchService.git
```

2. Navigate to the project folder:

```
cd .\Taller1-SearchService\
```

3. Restore the project dependencies:

```
dotnet restore
```

4. Compose Mongo Database

```
docker-compose up -d
```

5. Run the project

```
dotnet run
```