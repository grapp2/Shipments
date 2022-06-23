CREATE TABLE [dbo].[Lot]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Quantity] NCHAR(10) NULL, 
    [ItemId] INT NULL, 
    [ShipmentId] INT NULL, 
    CONSTRAINT [FK_Lot_ToItem] FOREIGN KEY (ItemId) REFERENCES Item(Id), 
    CONSTRAINT [FK_Lot_ToShipment] FOREIGN KEY (ShipmentId) REFERENCES Shipment(Id)
)
