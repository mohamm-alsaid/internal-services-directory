CREATE TABLE [Service] (
  [serviceID] int PRIMARY KEY IDENTITY(1, 1),
  [programID] int,
  [departmentID] int,
  [divisionID] int,
  [serviceName] nvarchar(255),
  [serviceDescription] nvarchar(255),
  [executiveSummary] nvarchar(255),
  [serviceArea] nvarchar(255),
  [contactID] int,
  [employeeConnectMethod] nvarchar(255),
  [customerConnectMethod] nvarchar(255),
  [expirationDate] datetime,
  [active] bool NOT NULL DEFAULT (true)
)
GO

CREATE TABLE [Program] (
  [programID] int PRIMARY KEY IDENTITY(1, 1),
  [sponsorName] nvarchar(255),
  [programName] nvarchar(255),
  [offerType] nvarchar(255)
)
GO

CREATE TABLE [Department] (
  [departmentID] int PRIMARY KEY IDENTITY(1, 1),
  [departmentCode] int UNIQUE NOT NULL,
  [departmentName] nvarchar(255)
)
GO

CREATE TABLE [Division] (
  [divisionID] int PRIMARY KEY IDENTITY(1, 1),
  [divisionCode] int UNIQUE NOT NULL,
  [divisionName] nvarchar(255)
)
GO

CREATE TABLE [Contact] (
  [contactID] int PRIMARY KEY IDENTITY(1, 1),
  [contactName] nvarchar(255),
  [phoneNumber] nvarchar(255),
  [emailAddress] nvarchar(255)
)
GO

CREATE TABLE [Community] (
  [communityID] int PRIMARY KEY IDENTITY(1, 1),
  [communityName] nvarchar(255) UNIQUE,
  [communityDescription] nvarchar(255)
)
GO

CREATE TABLE [ProgramCommunityAssociation] (
  [programCommunityAssociationID] int PRIMARY KEY IDENTITY(1, 1),
  [serviceID] int NOT NULL,
  [communityID] int NOT NULL
)
GO

CREATE TABLE [Language] (
  [languageID] int PRIMARY KEY IDENTITY(1, 1),
  [languageName] nvarchar(255) UNIQUE
)
GO

CREATE TABLE [ServiceLanguageAssociation] (
  [serviceLanguageAssociationID] int PRIMARY KEY IDENTITY(1, 1),
  [serviceID] int,
  [languageID] int
)
GO

CREATE TABLE [Location] (
  [locationID] int PRIMARY KEY IDENTITY(1, 1),
  [locationTypeID] int NOT NULL,
  [locationName] nvarchar(255) UNIQUE,
  [buildingID] nvarchar(255),
  [locationAddress] nvarchar(255),
  [roomNumber] nvarchar(255),
  [floorNumber] nvarchar(255)
)
GO

CREATE TABLE [LocationType] (
  [locationTypeID] int PRIMARY KEY IDENTITY(1, 1),
  [locationTypeName] nvarchar(255) UNIQUE NOT NULL
)
GO

CREATE TABLE [ServiceLocationAssociation] (
  [serviceLocationAssociationID] int PRIMARY KEY IDENTITY(1, 1),
  [serviceID] int,
  [locationID] int
)
GO

ALTER TABLE [Service] ADD FOREIGN KEY ([programID]) REFERENCES [Program] ([programID])
GO

ALTER TABLE [Service] ADD FOREIGN KEY ([contactID]) REFERENCES [Contact] ([contactID])
GO

ALTER TABLE [Service] ADD FOREIGN KEY ([departmentID]) REFERENCES [Department] ([departmentID])
GO

ALTER TABLE [Service] ADD FOREIGN KEY ([divisionID]) REFERENCES [Division] ([divisionID])
GO

ALTER TABLE [ProgramCommunityAssociation] ADD FOREIGN KEY ([serviceID]) REFERENCES [Service] ([serviceID])
GO

ALTER TABLE [ProgramCommunityAssociation] ADD FOREIGN KEY ([communityID]) REFERENCES [Community] ([communityID])
GO

ALTER TABLE [ServiceLanguageAssociation] ADD FOREIGN KEY ([languageID]) REFERENCES [Language] ([languageID])
GO

ALTER TABLE [ServiceLocationAssociation] ADD FOREIGN KEY ([serviceID]) REFERENCES [Service] ([serviceID])
GO

ALTER TABLE [Location] ADD FOREIGN KEY ([locationTypeID]) REFERENCES [LocationType] ([locationTypeID])
GO

ALTER TABLE [ServiceLocationAssociation] ADD FOREIGN KEY ([locationID]) REFERENCES [Location] ([locationID])
GO

ALTER TABLE [ServiceLanguageAssociation] ADD FOREIGN KEY ([serviceID]) REFERENCES [Service] ([serviceID])
GO
