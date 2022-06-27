CREATE TABLE [dbo].[Specifications] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    [Value]  FLOAT (53)   NOT NULL,
    [ItemId] INT          NOT NULL,
    CONSTRAINT [PK_Specifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Specification_ToItem] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Specification_ToItem]
    ON [dbo].[Specifications]([ItemId] ASC);

