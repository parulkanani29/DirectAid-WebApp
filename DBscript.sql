CREATE DATABASE DirectAidDB;
GO
USE [DirectAidDB];
GO

/****** Object:  Table [dbo].[Applications]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[Applications]
([Id]                   [UNIQUEIDENTIFIER] NOT NULL, 
 [Category]             [INT] NOT NULL, 
 [Applicant]            [UNIQUEIDENTIFIER] NOT NULL, 
 [AverageIncome]        [NUMERIC](18, 0) NULL, 
 [IdentificationNumber] [NVARCHAR](50) NULL, 
 [Status]               [INT] NULL, 
 [IsDraft]              [BIT] NULL, 
 [IsDeleted]            [BIT] NULL, 
 [Comments]             [VARCHAR](MAX) NULL, 
 [CreatedOn]            [DATETIME] NULL, 
 [UpdatedOn]            [DATETIME] NULL, 
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED([Id] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

/****** Object:  Table [dbo].[Users]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE TABLE [dbo].[Users]
([UserId]        [UNIQUEIDENTIFIER] NOT NULL, 
 [WalletName]    [VARCHAR](50) NULL, 
 [WalletAddress] [VARCHAR](50) NULL, 
 [FirstName]     [VARCHAR](50) NULL, 
 [LastName]      [VARCHAR](50) NULL, 
 [Email]         [VARCHAR](50) NULL, 
 [Role]          [INT] NULL, 
 [BirthDate]     [DATE] NULL, 
 [CreatedOn]     [DATETIME] NULL, 
 [UpdatedOn]     [DATETIME] NULL, 
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED([UserId] ASC)
 WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
)
ON [PRIMARY];
GO
ALTER TABLE [dbo].[Applications]
WITH CHECK
ADD CONSTRAINT [FK_Applications_Users] FOREIGN KEY([Applicant]) REFERENCES [dbo].[Users]([UserId]);
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_Users];
GO

/****** Object:  StoredProcedure [dbo].[CreateApplication]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[CreateApplication] @Category             INT, 
                                           @Applicant            UNIQUEIDENTIFIER, 
                                           @IdentificationNumber NVARCHAR(50), 
                                           @AverageIncome        VARCHAR(50), 
                                           @Status               INT
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;
        DECLARE @NewApplicationId UNIQUEIDENTIFIER= NEWID(), @RowCount INT;
        INSERT INTO [Applications]
        (Id, 
         Category, 
         Applicant, 
         IdentificationNumber, 
         AverageIncome, 
         [status], 
         CreatedOn, 
         UpdatedOn, 
         IsDeleted
        )
        VALUES
        (@NewApplicationId, 
         @Category, 
         @Applicant, 
         @IdentificationNumber, 
         @AverageIncome, 
         @Status, 
         GETUTCDATE(), 
         GETUTCDATE(), 
         0
        );
        SET @RowCount = @@ROWCOUNT;
        IF(@RowCount > 0)
            BEGIN
                SELECT @NewApplicationId;
            END;
    END;
GO

/****** Object:  StoredProcedure [dbo].[GetAllApplications]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[GetAllApplications]
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;
        SELECT [Id], 
               [Category], 
               [Applicant], 
               u.FirstName + ' ' + u.LastName AS UserFullName, 
               u.WalletAddress, 
               [AverageIncome], 
               [Status], 
               [IsDraft], 
               [IsDeleted], 
               [Comments]
        FROM [Applications] app
             INNER JOIN [Users] u ON app.Applicant = u.UserId
        WHERE IsDeleted = 0;
    END;
GO

/****** Object:  StoredProcedure [dbo].[GetApplicationsByUser]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[GetApplicationsByUser] @UserId UNIQUEIDENTIFIER
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;
        SELECT [Id], 
               [Category], 
               [Applicant], 
               u.FirstName + ' ' + u.LastName AS UserFullName, 
               [AverageIncome], 
               [IdentificationNumber], 
               [Status], 
               [IsDraft], 
               [IsDeleted], 
               [Comments]
        FROM [Applications] app
             INNER JOIN [Users] u ON app.Applicant = u.UserId
        WHERE app.Applicant = @UserId;
    END;
GO

/****** Object:  StoredProcedure [dbo].[GetUserByWallet]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[GetUserByWallet] @WalletName VARCHAR(50)
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;
        SELECT [UserId], 
               [WalletName], 
               [WalletAddress], 
               [FirstName], 
               [LastName], 
               [Email], 
               [BirthDate], 
               [Role]
        FROM [Users]
        WHERE WalletName = @WalletName;
    END;
GO

/****** Object:  StoredProcedure [dbo].[RegisterUser]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[RegisterUser] @WalletName    VARCHAR(50), 
                                      @WalletAddress VARCHAR(50), 
                                      @UserRole      INT, 
                                      @FirstName     VARCHAR(50), 
                                      @LastName      VARCHAR(50), 
                                      @Email         VARCHAR(50), 
                                      @BirthDate     DATE
AS
    BEGIN
        SET NOCOUNT OFF;
        DECLARE @NewUserId UNIQUEIDENTIFIER= NEWID(), @RowCount INT;
        INSERT INTO [Users]
        (UserId, 
         WalletName, 
         WalletAddress, 
         [Role], 
         FirstName, 
         LastName, 
         Email, 
         BirthDate, 
         CreatedOn, 
         UpdatedOn
        )
        VALUES
        (@NewUserId, 
         @WalletName, 
         @WalletAddress, 
         @UserRole, 
         @FirstName, 
         @LastName, 
         @Email, 
         @BirthDate, 
         GETUTCDATE(), 
         GETUTCDATE()
        );
        SET @RowCount = @@ROWCOUNT;
        IF(@RowCount > 0)
            BEGIN
                SELECT @NewUserId;
            END;
    END;
GO

/****** Object:  StoredProcedure [dbo].[UpdateApplicationStatus]    Script Date: 14-Jun-20 8:45:47 PM ******/

SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
CREATE PROCEDURE [dbo].[UpdateApplicationStatus] @ApplicationId UNIQUEIDENTIFIER, 
                                                 @Status        INT
AS
    BEGIN
        -- SET NOCOUNT ON added to prevent extra result sets from
        -- interfering with SELECT statements.
        SET NOCOUNT ON;

        -- Insert statements for procedure here
        UPDATE Applications
          SET 
              [Status] = @Status, 
              UpdatedOn = GETUTCDATE()
        WHERE Id = @ApplicationId;
        SELECT @@ROWCOUNT;
    END;
GO

-- insert admin user

INSERT INTO Users
(UserId, 
 WalletName, 
 WalletAddress, 
 FirstName, 
 LastName, 
 Email, 
 [Role], 
 [birthdate], 
 [createdOn], 
 UpdatedOn
)
VALUES
(NEWID(), 
 'Hackathon_1', 
 'CUtNvY1Jxpn4V4RD1tgphsUKpQdo4q5i54', 
 'Admin', 
 'User', 
 'a@a.com', 
 2, 
 '2004-06-13', 
 GETUTCDATE(), 
 GETUTCDATE()
);