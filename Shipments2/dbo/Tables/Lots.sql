CREATE TABLE [dbo].[Lots] (
    [Id]         INT        IDENTITY (1, 1) NOT NULL,
    [Quantity]   INT NULL,
    [ItemId]     INT        NULL,
    [ShipmentId] INT        NULL,
    CONSTRAINT [PK_Lots] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Lot_ToItem] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Lot_ToShipment] FOREIGN KEY ([ShipmentId]) REFERENCES [dbo].[Shipments] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Lot_ToShipment]
    ON [dbo].[Lots]([ShipmentId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Lot_ToItem]
    ON [dbo].[Lots]([ItemId] ASC);

