--BULK INSERT

TRUNCATE TABLE dbo.Customer;
GO
 
-- import the file
BULK INSERT dbo.Customer
FROM "C:\Users\Fady Shafeek\Downloads\Import\Customer.csv"
WITH
(
        FORMAT='CSV',
        FIRSTROW=1
)
GO
TRUNCATE TABLE dbo.Service;
GO
 
-- import the file
BULK INSERT dbo.Service
FROM "C:\Users\Fady Shafeek\Downloads\Import\Service.csv"
WITH
(
        FORMAT='CSV',
        FIRSTROW=1
)
GO

TRUNCATE TABLE dbo.Contract;
GO
 
-- import the file
BULK INSERT dbo.Contract
FROM "C:\Users\Fady Shafeek\Downloads\Import\Contract.csv"
WITH
(
FIELDTERMINATOR = ',',
        FORMAT='CSV',
        FIRSTROW=1
)
GO