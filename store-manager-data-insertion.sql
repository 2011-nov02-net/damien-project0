SELECT * FROM [Stores].[Store]
SELECT * FROM [Stores].[Customer]
SELECT * FROM [Stores].[Address]
SELECT * FROM [Stores].[Order]
SELECT * FROM [Stores].[OperatingLocation]
SELECT * FROM [Stores].[Product]

INSERT INTO [Stores].[OperatingLocation]
    (OperatingLocationId, StoreId, AddressId)
VALUES
    (0, 0, 0),
    (1, 1, 1),
    (2, 2, 2);
GO

INSERT INTO [Stores].[Address]
    (AddressId, AddressLine1, AddressLine2, City, State, Country, ZipCode)
VALUES
    (0, '123 Address Rd', 'Apt #612', 'Austin', 'Texas', 'United States', 12345),
    (1, '123 Arkhenplatz Blvd', 'Apt #451', 'Eleison', NULL, 'Khalrun', '09876'),
    (2, '136 Alkyrtruz Rd', NULL, 'Eleison', NULL, 'Khalrun', '09876');
GO


INSERT INTO [Stores].[Store]
    (StoreId, Name)
VALUES
    (0, 'Arkh Manufacturing'),
    (1, 'Hyperion Robotics'),
    (2, 'Apollo Transportation');
GO

INSERT INTO [Stores].[Product]
    (ProductId, Name, Price, Discount)
VALUES
    (0, 'Bag of Whole Coffee Beans', 12.00, NULL),
    (1, '4-pack of Monster Energy Drinks', 14.00, NULL),
    (2, 'Large Pack of Granola Bars', 10.00, 0.3),
    (3, 'Box of Dry Erase Markers', 5.00, NULL);
GO
