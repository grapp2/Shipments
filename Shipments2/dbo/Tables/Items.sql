CREATE TABLE [dbo].[Items] (
    [Id]          INT        IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR(50) NULL,
    [Description] VARCHAR(50) NULL,
    CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED ([Id] ASC)
);

