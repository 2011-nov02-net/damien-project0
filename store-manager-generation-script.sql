IF NOT EXISTS(SELECT * FROM sys.databases WHERE Name = 'StoreManager')
BEGIN
    CREATE DATABASE [StoreManager]
END
GO

CREATE SCHEMA [Stores];
GO

/******************************************************
    Store:
        - StoreId
        - Name
        - OperatingLocations[]
        - ProductCatalog{}
        - Customers[]

    OperatingLocation:
        - OperatingLocationId
        - AddressId

    Address:
        - AddressLine1
        - AddressLine2
        - City
        - State (Nullable)
        - Country
        - ZipCode

    Customer:
        - CustomerId
        - FirstName
        - LastName
        - BirthDate
        - AddressId
        - Email
        - PhoneNumber
        - DefaultOperatingLocation OperatingLocationId
        - Orders[]
        
    Product:
        - ProductId
        - Name
        - Price
        - Discount (Nullable)

    Order:
        - OrderId
        - CustomerId
        - OperatingLocationId
        - ProductsRequested{}

    StoreOperatingLocation:
        - StoreId
        - OperatingLocationId

    CustomerOrder:
        - CustomerId
        - OrderId

    StoreInventory:
        - OperatingLocationId
        - ProductId
        - Count

    OrderProduct:
        - OrderId
        - ProductId
        - Count

******************************************************/

-- DROP TABLE [Stores].[Store]
CREATE TABLE [Stores].[Store] (
    [StoreId]                       INT             NOT NULL PRIMARY KEY IDENTITY,
    [Name]                          NVARCHAR(64)    NOT NULL     
);
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[Address]
CREATE TABLE [Stores].[Address] (
    [AddressId]                     INT             NOT NULL PRIMARY KEY IDENTITY,
    [AddressLine1]                  NVARCHAR(128)   NOT NULL,
    [AddressLine2]                  NVARCHAR(128)   NOT NULL,
    [City]                          NVARCHAR(128)   NOT NULL,
    [State]                         NVARCHAR(128)   NULL,
    [Country]                       NVARCHAR(128)   NOT NULL,
    [ZipCode]                       NVARCHAR(128)   NOT NULL
);
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[OperatingLocation]
CREATE TABLE [Stores].[OperatingLocation] (
    [OperatingLocationId]           INT             NOT NULL PRIMARY KEY IDENTITY,
    [AddressId]                     INT             NOT NULL UNIQUE
);
GO

ALTER TABLE [Stores].[OperatingLocation]
    ADD CONSTRAINT FK_OperatingLocation_AddressId FOREIGN KEY (AddressId)
        REFERENCES [Stores].[Address] (AddressId)
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[Customer]
CREATE TABLE [Stores].[Customer] (
    [CustomerId]                    INT             NOT NULL PRIMARY KEY IDENTITY,
    [FirstName]                     NVARCHAR(128)   NOT NULL,
    [LastName]                      NVARCHAR(128)   NOT NULL,
    [BirthDate]                     DATETIME        NOT NULL,
    [Email]                         NVARCHAR(128)   NULL,
    [PhoneNumber]                   NVARCHAR(128)   NULL,
    [AddressId]                     INT             NOT NULL,
    [OperatingLocationId]           INT             NULL,
    CHECK (Email IS NOT NULL OR PhoneNumber IS NOT NULL)
);
GO

ALTER TABLE [Stores].[Customer]
    ADD CONSTRAINT FK_Customer_CustomerAddressId FOREIGN KEY (AddressId)
        REFERENCES [Stores].[Address] (AddressId)
GO

ALTER TABLE [Stores].[Customer]
    ADD CONSTRAINT FK_Customer_OperatingLocationId FOREIGN KEY (OperatingLocationId)
        REFERENCES [Stores].[OperatingLocation] (OperatingLocationId)
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[Product]
CREATE TABLE [Stores].[Product] (
    [ProductId]                     INT             NOT NULL PRIMARY KEY IDENTITY,
    [Name]                          NVARCHAR(64)    NOT NULL,
    [Price]                         MONEY           NOT NULL,
    [Discount]                      MONEY           NULL
);
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[Order]
CREATE TABLE [Stores].[Order] (
    [OrderId]                       INT             NOT NULL PRIMARY KEY IDENTITY,
    [CustomerId]                    INT             NOT NULL,
    [OperatingLocationId]           INT             NOT NULL
);
GO

ALTER TABLE [Stores].[Order]
    ADD CONSTRAINT FK_Order_CustomerId FOREIGN KEY (CustomerId)
        REFERENCES [Stores].[Customer] (CustomerId)
GO

ALTER TABLE [Stores].[Order]
    ADD CONSTRAINT FK_Order_OperatingLocationId FOREIGN KEY (OperatingLocationId)
        REFERENCES [Stores].[OperatingLocation] (OperatingLocationId)
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[StoreOperatingLocation]
CREATE TABLE [Stores].[StoreOperatingLocation] (
    [StoreId]                       INT             NOT NULL PRIMARY KEY IDENTITY,
    [OperatingLocationId]           INT             NOT NULL,
);
GO

ALTER TABLE [Stores].[StoreOperatingLocation]
    ADD CONSTRAINT FK_StoreOperatingLocation_StoreId FOREIGN KEY (StoreId)
        REFERENCES [Stores].[Store] (StoreId)
GO

ALTER TABLE [Stores].[StoreOperatingLocation]
    ADD CONSTRAINT FK_StoreOperatingLocation_OperatingLocationId FOREIGN KEY (OperatingLocationId)
        REFERENCES [Stores].[OperatingLocation] (OperatingLocationId)
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[CustomerOrder]
CREATE TABLE [Stores].[CustomerOrder] (
    [CustomerId]                    INT             NOT NULL,
    [OrderId]                       INT             NOT NULL
);
GO

ALTER TABLE [Stores].[CustomerOrder]
    ADD CONSTRAINT FK_CustomerOrder_CustomerId FOREIGN KEY (CustomerId)
        REFERENCES [Stores].[Customer] (CustomerId)
GO

ALTER TABLE [Stores].[CustomerOrder]
    ADD CONSTRAINT FK_CustomerOrder_OrderId FOREIGN KEY (OrderId)
        REFERENCES [Stores].[Order] (OrderId)
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[StoreInventory]
CREATE TABLE [Stores].[StoreInventory] (
    [StoreId]                       INT             NOT NULL,
    [ProductId]                     INT             NOT NULL,
    [Count]                         INT             NOT NULL
);
GO

ALTER TABLE [Stores].[StoreInventory]
    ADD CONSTRAINT FK_StoreInventory_StoreId FOREIGN KEY (StoreId)
        REFERENCES [Stores].[Store] (StoreId)
GO

ALTER TABLE [Stores].[StoreInventory]
    ADD CONSTRAINT FK_StoreInventory_ProductId FOREIGN KEY (ProductId)
        REFERENCES [Stores].[Product] (ProductId)
GO

-------------------------------------------------------------------------------------------------------------------------------------------

-- DROP TABLE [Stores].[OrderProduct]
CREATE TABLE [Stores].[OrderProduct] (
    [OrderId]                       INT             NOT NULL,
    [ProductId]                     INT             NOT NULL,
    [Count]                         INT             NOT NULL
);
GO

ALTER TABLE [Stores].[OrderProduct]
    ADD CONSTRAINT FK_OrderProduct_OrderId FOREIGN KEY (OrderId)
        REFERENCES [Stores].[Order] (OrderId)
GO

ALTER TABLE [Stores].[OrderProduct]
    ADD CONSTRAINT FK_OrderProduct_ProductId FOREIGN KEY (ProductId)
        REFERENCES [Stores].[Product] (ProductId)
GO