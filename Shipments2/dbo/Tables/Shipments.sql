CREATE TABLE [dbo].[Shipments] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [Sender]       INT          NULL,
    [Receiver]     INT          NULL,
    [SentDate]     DATETIME     NULL,
    [ReceivedDate] DATETIME     NULL,
    [Description]  VARCHAR (50) NULL,
    [Tracking]     INT          NULL,
    CONSTRAINT [PK_Shipments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Shipment_ToCompany] FOREIGN KEY ([Sender]) REFERENCES [dbo].[Companies] ([Id]),
    CONSTRAINT [FK_Shipment_ToCompany2] FOREIGN KEY ([Receiver]) REFERENCES [dbo].[Companies] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Shipment_ToCompany2]
    ON [dbo].[Shipments]([Receiver] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_Shipment_ToCompany]
    ON [dbo].[Shipments]([Sender] ASC);

