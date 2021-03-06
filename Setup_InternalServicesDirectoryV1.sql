USE [master]
GO
/****** Object:  Database [InternalServicesDirectoryV1]    Script Date: 6/4/2020 5:39:56 PM ******/
CREATE DATABASE [InternalServicesDirectoryV1]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'InternalServicesDirectoryV1', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InternalServicesDirectoryV1.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'InternalServicesDirectoryV1_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\InternalServicesDirectoryV1_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [InternalServicesDirectoryV1].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET ARITHABORT OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET  DISABLE_BROKER 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET RECOVERY FULL 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET  MULTI_USER 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET DB_CHAINING OFF 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'InternalServicesDirectoryV1', N'ON'
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET QUERY_STORE = OFF
GO
USE [InternalServicesDirectoryV1]
GO
/****** Object:  Table [dbo].[Community]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Community](
	[communityID] [int] IDENTITY(1,1) NOT NULL,
	[communityName] [nvarchar](255) NOT NULL,
	[communityDescription] [nvarchar](255) NULL,
 CONSTRAINT [PK__Communit__938137AD6720E966] PRIMARY KEY CLUSTERED 
(
	[communityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Communit__C43D6630E08699EF] UNIQUE NONCLUSTERED 
(
	[communityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[contactID] [int] IDENTITY(1,1) NOT NULL,
	[contactName] [nvarchar](255) NULL,
	[phoneNumber] [nvarchar](255) NULL,
	[emailAddress] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[contactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[departmentID] [int] IDENTITY(1,1) NOT NULL,
	[departmentCode] [int] NOT NULL,
	[departmentName] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[departmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[departmentCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Division]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Division](
	[divisionID] [int] IDENTITY(1,1) NOT NULL,
	[divisionCode] [int] NOT NULL,
	[divisionName] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[divisionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[divisionCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Language]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Language](
	[languageID] [int] IDENTITY(1,1) NOT NULL,
	[languageName] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Language__12696A42B61029D1] PRIMARY KEY CLUSTERED 
(
	[languageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Language__6E492863F6D7EE1D] UNIQUE NONCLUSTERED 
(
	[languageName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Location]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[locationID] [int] IDENTITY(1,1) NOT NULL,
	[locationTypeID] [int] NOT NULL,
	[locationName] [nvarchar](255) NULL,
	[buildingID] [nvarchar](255) NULL,
	[locationAddress] [nvarchar](255) NULL,
	[roomNumber] [nvarchar](255) NULL,
	[floorNumber] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[locationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[locationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocationType]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocationType](
	[locationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[locationTypeName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[locationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[locationTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Program]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Program](
	[programID] [int] IDENTITY(1,1) NOT NULL,
	[sponsorName] [nvarchar](255) NULL,
	[offerType] [nvarchar](255) NULL,
	[programName] [nvarchar](255) NULL,
	[programOfferNumber] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[programID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Unique_ProgramOfferNumber] UNIQUE NONCLUSTERED 
(
	[programOfferNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[serviceID] [int] IDENTITY(1,1) NOT NULL,
	[programID] [int] NULL,
	[departmentID] [int] NULL,
	[divisionID] [int] NULL,
	[serviceName] [nvarchar](255) NULL,
	[serviceDescription] [varchar](6000) NULL,
	[executiveSummary] [varchar](6000) NULL,
	[serviceArea] [nvarchar](255) NULL,
	[contactID] [int] NULL,
	[employeeConnectMethod] [nvarchar](255) NULL,
	[customerConnectMethod] [nvarchar](255) NULL,
	[expirationDate] [datetime] NULL,
	[active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[serviceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceCommunityAssociation]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceCommunityAssociation](
	[serviceCommunityAssociationID] [int] IDENTITY(1,1) NOT NULL,
	[serviceID] [int] NOT NULL,
	[communityID] [int] NOT NULL,
 CONSTRAINT [PK__ServiceC__3576AE4967FB1C02] PRIMARY KEY CLUSTERED 
(
	[serviceCommunityAssociationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uk_ServiceCommunityAssociation] UNIQUE NONCLUSTERED 
(
	[serviceID] ASC,
	[communityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceLanguageAssociation]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceLanguageAssociation](
	[serviceLanguageAssociation] [int] IDENTITY(1,1) NOT NULL,
	[serviceID] [int] NOT NULL,
	[languageID] [int] NOT NULL,
 CONSTRAINT [PK__ServiceL__01B8A411AA2BB883] PRIMARY KEY CLUSTERED 
(
	[serviceLanguageAssociation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uk_ServiceLanguageAssociation] UNIQUE NONCLUSTERED 
(
	[serviceID] ASC,
	[languageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceLocationAssociation]    Script Date: 6/4/2020 5:39:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceLocationAssociation](
	[serviceLocationAssociation] [int] IDENTITY(1,1) NOT NULL,
	[serviceID] [int] NOT NULL,
	[locationID] [int] NOT NULL,
 CONSTRAINT [PK__ServiceL__9D5CE72616DC9152] PRIMARY KEY CLUSTERED 
(
	[serviceLocationAssociation] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uk_ServiceLocationAssociation] UNIQUE NONCLUSTERED 
(
	[serviceID] ASC,
	[locationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD FOREIGN KEY([locationTypeID])
REFERENCES [dbo].[LocationType] ([locationTypeID])
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__contact__403A8C7D] FOREIGN KEY([contactID])
REFERENCES [dbo].[Contact] ([contactID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__contact__403A8C7D]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__departm__412EB0B6] FOREIGN KEY([departmentID])
REFERENCES [dbo].[Department] ([departmentID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__departm__412EB0B6]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__divisio__4222D4EF] FOREIGN KEY([divisionID])
REFERENCES [dbo].[Division] ([divisionID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__divisio__4222D4EF]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK__Service__program__4316F928] FOREIGN KEY([programID])
REFERENCES [dbo].[Program] ([programID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK__Service__program__4316F928]
GO
ALTER TABLE [dbo].[ServiceCommunityAssociation]  WITH CHECK ADD  CONSTRAINT [FK__ServiceCo__commu__440B1D61] FOREIGN KEY([communityID])
REFERENCES [dbo].[Community] ([communityID])
GO
ALTER TABLE [dbo].[ServiceCommunityAssociation] CHECK CONSTRAINT [FK__ServiceCo__commu__440B1D61]
GO
ALTER TABLE [dbo].[ServiceCommunityAssociation]  WITH CHECK ADD  CONSTRAINT [FK__ServiceCo__servi__44FF419A] FOREIGN KEY([serviceID])
REFERENCES [dbo].[Service] ([serviceID])
GO
ALTER TABLE [dbo].[ServiceCommunityAssociation] CHECK CONSTRAINT [FK__ServiceCo__servi__44FF419A]
GO
ALTER TABLE [dbo].[ServiceLanguageAssociation]  WITH CHECK ADD  CONSTRAINT [FK__ServiceLa__langu__45F365D3] FOREIGN KEY([languageID])
REFERENCES [dbo].[Language] ([languageID])
GO
ALTER TABLE [dbo].[ServiceLanguageAssociation] CHECK CONSTRAINT [FK__ServiceLa__langu__45F365D3]
GO
ALTER TABLE [dbo].[ServiceLanguageAssociation]  WITH CHECK ADD  CONSTRAINT [FK__ServiceLa__servi__46E78A0C] FOREIGN KEY([serviceID])
REFERENCES [dbo].[Service] ([serviceID])
GO
ALTER TABLE [dbo].[ServiceLanguageAssociation] CHECK CONSTRAINT [FK__ServiceLa__servi__46E78A0C]
GO
ALTER TABLE [dbo].[ServiceLocationAssociation]  WITH CHECK ADD  CONSTRAINT [FK__ServiceLo__locat__47DBAE45] FOREIGN KEY([locationID])
REFERENCES [dbo].[Location] ([locationID])
GO
ALTER TABLE [dbo].[ServiceLocationAssociation] CHECK CONSTRAINT [FK__ServiceLo__locat__47DBAE45]
GO
ALTER TABLE [dbo].[ServiceLocationAssociation]  WITH CHECK ADD  CONSTRAINT [FK__ServiceLo__servi__48CFD27E] FOREIGN KEY([serviceID])
REFERENCES [dbo].[Service] ([serviceID])
GO
ALTER TABLE [dbo].[ServiceLocationAssociation] CHECK CONSTRAINT [FK__ServiceLo__servi__48CFD27E]
GO
USE [master]
GO
ALTER DATABASE [InternalServicesDirectoryV1] SET  READ_WRITE 
GO
