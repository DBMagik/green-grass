# GreenGrass Ideal Rewrite

The rewrite fixes the button not working for adding a new client. The pages just had to be made interactable. 

A simple ASP.NET Core Blazor application for customer management.

## Overview

GreenGrass is a web application built with ASP.NET Core and Blazor that provides customer management functionality. The application allows you to manage customer information including contact details, addresses, and payment methods.

## Technologies Used

- .NET 9.0
- ASP.NET Core
- Blazor
- C# 13.0
- SQL Server

## Prerequisites

- .NET 9.0 SDK
- SQL Server (or SQL Server Express)

## Database Setup

This application requires a SQL Server database. You'll need to:

1. Create a database on your SQL Server instance
2. Update the connection string in `appsettings.json` with your database details
3. Run the following SQL script to create the required tables:

```sql
-- Create Customer table
CREATE TABLE Customer (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) NULL,
    
    -- Address fields (assuming Address is embedded)
    Street NVARCHAR(255) NULL,
    City NVARCHAR(100) NULL,
    State NVARCHAR(50) NULL,
    ZipCode NVARCHAR(10) NULL,
    Country NVARCHAR(50) NULL,
    
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    
    -- Payment information
    PaymentMethodToken NVARCHAR(255) NULL,
    LastFourDigits NVARCHAR(4) NULL,
    
    -- Indexes
    INDEX IX_Customer_Email (Email),
    INDEX IX_Customer_LastName (LastName),
    INDEX IX_Customer_IsActive (IsActive),
    INDEX IX_Customer_CreatedDate (CreatedDate)
);

-- Add unique constraint on email
ALTER TABLE Customer
ADD CONSTRAINT UQ_Customer_Email UNIQUE (Email);

-- Add check constraint for LastFourDigits
ALTER TABLE Customer
ADD CONSTRAINT CK_Customer_LastFourDigits 
CHECK (LastFourDigits IS NULL OR LEN(LastFourDigits) = 4);

-- Insert test customers
INSERT INTO Customer (
    FirstName, 
    LastName, 
    Email, 
    PhoneNumber, 
    Street, 
    City, 
    State, 
    ZipCode, 
    Country,
    CreatedDate,
    IsActive,
    PaymentMethodToken,
    LastFourDigits
) VALUES 
(
    'John', 
    'McTest', 
    'john.mctest@example.com', 
    '(555) 123-4567',
    '123 Test Street',
    'Test City',
    'TX',
    '12345',
    'USA',
    GETUTCDATE(),
    1,
    'tok_test_john_1234',
    '1234'
),
(
    'Jane', 
    'McTest', 
    'jane.mctest@example.com', 
    '(555) 987-6543',
    '456 Demo Avenue',
    'Demo Town',
    'CA',
    '67890',
    'USA',
    GETUTCDATE(),
    1,
    'tok_test_jane_5678',
    '5678'
);
```
