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