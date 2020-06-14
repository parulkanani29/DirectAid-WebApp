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

#### Register

Users can register with the system by providing basic detail like first name, last name, wallet name, and can choose a single wallet address from the available addresses.

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/2.Register.png" width="90%"></img>

#### Admin Dashboard

From the admin dashboard, the admin user can perform various functions as below.

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/3.Admin%20Area.png" width="90%"></img>

#### Add Contract Fund

The admin user will transfer the amount that needs to distribute among people. The smart contract will then hold that amount.

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/4.Add%20contract%20fund.png" width="90%"></img>

#### Set Category Amount

The admin user could define different relief packages for different people. Here are the examples

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/5.Set%20category%20amount.png" width="90%"></img>

#### Approve/Reject Application

The admin user can approve or reject the application by reviewing them on the admin dashboard.

### Applicant Dashboard

An applicant can view the current address balance from the dashboard, can submit a new application, and view the submitted application status.

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/6.%20Applicant%20dashboard.png" width="90%"></img>

### Submit Application

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/7.Submit%20Application.png" width="90%"></img>

#### Approved Logs

Registered users can see the approved application logs from the system, which makes the relief process transparent. People would know about the actual fund distributed among needy people

<img src="https://raw.githubusercontent.com/parulkanani29/DirectAid-WebApp/master/Images/8.%20Approved%20Logs.png" width="90%"></img>

#### <ins> **Note :** </ins>

This is POC level code and the purpose of this application is to demonstrate a *happy flow* with smart contract usage. The code may break without proper setup and configurations.


