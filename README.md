# PackageAPI

## Summary
This .Net API could be seen as a showcase for the package delivery industry. It has three endpoints; GetPackages, CreatePackage and GetPackageDetails.

## Prerequisites
Before building and running this project please install Docker Desktop, .Net 7.0 and Postman.

## Build
Continue by cloning the project with Git and run this bash command from the project's root catalogue: docker build -t package-api .

## Run
Run this bash command from the project's root catalogue: `run -p <port of choice>:80 package-api`

## How to use the endpoints in Postman
**Create package:** _URL:_ `http://localhost:<port of choice>/package` _example body:_ {"weight": 14000,"length": 30,"height": 30,"width": 60}
**Get package details:** _URL:_ `http://localhost:<port of choice>/package/<kollId>
**Get all packages:** _URL:_ `http://localhost:<port of choice>/package`

## Run tests
Run this bash command from the project's root catalogue: `dotnet test`