CREATE TABLE [dbo].[Company] (
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NULL,
    [Address1] VARCHAR (50) NULL,
    [Address2] VARCHAR (50) NULL,
    [Country]  VARCHAR (50) NULL,
    [State]    VARCHAR (50) NULL,
    [Zip]      VARCHAR (50) NULL,
    [City] VARCHAR(50) NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

