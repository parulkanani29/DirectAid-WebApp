# DirectAid Web Application

This web application is used to interact with the Direct Aid smart contract. Using this application an applicant can submit an application to get relief from the government during COVID-19 pandemic.

## Get Started

The project uses ASP.NET Core 2.2 and MSSQL server. Therefore, install and configure the dependencies before proceeding.

1. Visual Studio 2017 or later with .NET Core SDK 2.2 
2. SQL Server 2014 or later 
3. Cirrus Core Wallet (Hackathon Edition)

#### 1. Database Configuration.

Open up the DB script from the root (`DBScript.sql`) and run it on your SQL server. It will create a new database `SignatureDB`, tables and required stored procedures.

#### 2. Web Project Configuration.

Open up the `appsettings.json` file and update the `ConnectionString` and `ContractAddress`, then run the project.

### Modules

#### Login

A user can log in with a wallet name and password to the system. Hence, to use this system, a user needs to have the core wallet account.

In case, if the user is not registered with the system, it will be redirected to the register screen.

The system will check the user-role; if the role is admin, it will redirect the user to the admin dashboard, otherwise, to the applicant dashboard.

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/1.Login.png" width="90%"></img>

