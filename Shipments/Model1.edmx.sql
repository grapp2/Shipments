
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/27/2022 10:28:46
-- Generated from EDMX file: C:\Users\rapps\source\repos\Shipments\Shipments\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Shipments];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Lot_ToItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lots] DROP CONSTRAINT [FK_Lot_ToItem];
GO
IF OBJECT_ID(N'[dbo].[FK_Lot_ToShipment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Lots] DROP CONSTRAINT [FK_Lot_ToShipment];
GO
IF OBJECT_ID(N'[dbo].[FK_Shipment_ToCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [FK_Shipment_ToCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_Shipment_ToCompany2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Shipments] DROP CONSTRAINT [FK_Shipment_ToCompany2];
GO
IF OBJECT_ID(N'[dbo].[FK_Specification_ToItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Specifications] DROP CONSTRAINT [FK_Specification_ToItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Items]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Items];
GO
IF OBJECT_ID(N'[dbo].[Lots]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lots];
GO
IF OBJECT_ID(N'[dbo].[Shipments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Shipments];
GO
IF OBJECT_ID(N'[dbo].[Specifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Specifications];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Address1] varchar(50)  NULL,
    [Address2] varchar(50)  NULL,
    [Country] varchar(50)  NULL,
    [State] varchar(50)  NULL,
    [Zip] varchar(50)  NULL,
    [City] varchar(50)  NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [dbo].[Items] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NULL,
    [Description] varchar(50)  NULL
);
GO

-- Creating table 'Lots'
CREATE TABLE [dbo].[Lots] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] int  NULL,
    [ItemId] int  NULL,
    [ShipmentId] int  NULL
);
GO

-- Creating table 'Shipments'
CREATE TABLE [dbo].[Shipments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sender] int  NULL,
    [Receiver] int  NULL,
    [SentDate] datetime  NULL,
    [ReceivedDate] datetime  NULL,
    [Description] varchar(50)  NULL,
    [Tracking] int  NULL
);
GO

-- Creating table 'Specifications'
CREATE TABLE [dbo].[Specifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(50)  NOT NULL,
    [Value] float  NOT NULL,
    [ItemId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [dbo].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [PK_Lots]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [PK_Shipments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Specifications'
ALTER TABLE [dbo].[Specifications]
ADD CONSTRAINT [PK_Specifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Sender] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_Shipment_ToCompany]
    FOREIGN KEY ([Sender])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Shipment_ToCompany'
CREATE INDEX [IX_FK_Shipment_ToCompany]
ON [dbo].[Shipments]
    ([Sender]);
GO

-- Creating foreign key on [Receiver] in table 'Shipments'
ALTER TABLE [dbo].[Shipments]
ADD CONSTRAINT [FK_Shipment_ToCompany2]
    FOREIGN KEY ([Receiver])
    REFERENCES [dbo].[Companies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Shipment_ToCompany2'
CREATE INDEX [IX_FK_Shipment_ToCompany2]
ON [dbo].[Shipments]
    ([Receiver]);
GO

-- Creating foreign key on [ItemId] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [FK_Lot_ToItem]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Lot_ToItem'
CREATE INDEX [IX_FK_Lot_ToItem]
ON [dbo].[Lots]
    ([ItemId]);
GO

-- Creating foreign key on [ItemId] in table 'Specifications'
ALTER TABLE [dbo].[Specifications]
ADD CONSTRAINT [FK_Specification_ToItem]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Specification_ToItem'
CREATE INDEX [IX_FK_Specification_ToItem]
ON [dbo].[Specifications]
    ([ItemId]);
GO

-- Creating foreign key on [ShipmentId] in table 'Lots'
ALTER TABLE [dbo].[Lots]
ADD CONSTRAINT [FK_Lot_ToShipment]
    FOREIGN KEY ([ShipmentId])
    REFERENCES [dbo].[Shipments]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Lot_ToShipment'
CREATE INDEX [IX_FK_Lot_ToShipment]
ON [dbo].[Lots]
    ([ShipmentId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------