CREATE TABLE [dbo].[Specification] (
    [Id]    INT          NOT NULL IDENTITY,
    [Name]  VARCHAR (50) NOT NULL,
    [Value] FLOAT (53)   NOT NULL,
    [ItemId] INT NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Specification_ToItem] FOREIGN KEY (ItemId) REFERENCES Item(Id)
);

