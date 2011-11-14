
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/13/2011 17:09:10
-- Generated from EDMX file: C:\projects\TimeTracking\TimeTracking\Models\TimeTrackingModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TimeTracking];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_EmployeeHourLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HourLogs] DROP CONSTRAINT [FK_EmployeeHourLog];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[HourLogs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HourLogs];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'HourLogs'
CREATE TABLE [dbo].[HourLogs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeId] int  NOT NULL,
    [UpdatedAt] datetime  NOT NULL,
    [Date] datetime  NOT NULL,
    [StartHour] decimal(17,1)  NOT NULL,
    [EndHour] decimal(17,1)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HourLogs'
ALTER TABLE [dbo].[HourLogs]
ADD CONSTRAINT [PK_HourLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [EmployeeId] in table 'HourLogs'
ALTER TABLE [dbo].[HourLogs]
ADD CONSTRAINT [FK_EmployeeHourLog]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeHourLog'
CREATE INDEX [IX_FK_EmployeeHourLog]
ON [dbo].[HourLogs]
    ([EmployeeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------