CREATE TABLE [dbo].[Item] (
    [Id]          INT        NOT NULL IDENTITY,
    [Name]        NCHAR (10) NULL,
    [Description] NCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

