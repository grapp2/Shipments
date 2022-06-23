CREATE TABLE [dbo].[Shipment] (
    [Id]           INT          NOT NULL IDENTITY,
    [Sender]       INT          NULL,
    [Receiver]     INT          NULL,
    [SentDate]     DATETIME     NULL,
    [ReceivedDate] DATETIME     NULL,
    [Description]  VARCHAR (50) NULL,
    [Tracking]     INT          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Shipment_ToCompany] FOREIGN KEY ([Sender]) REFERENCES [dbo].[Company] ([Id]),
    CONSTRAINT [FK_Shipment_ToCompany2] FOREIGN KEY ([Receiver]) REFERENCES [dbo].[Company] ([Id])
);

