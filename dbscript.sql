USE [master]
GO
/****** Object:  Database [FilmManager]    Script Date: 6/8/2020 9:16:03 PM ******/
CREATE DATABASE [FilmManager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'FilmManager', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS2014\MSSQL\DATA\FilmManager.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FilmManager_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS2014\MSSQL\DATA\FilmManager_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FilmManager] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FilmManager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FilmManager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [FilmManager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [FilmManager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [FilmManager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [FilmManager] SET ARITHABORT OFF 
GO
ALTER DATABASE [FilmManager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [FilmManager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [FilmManager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [FilmManager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [FilmManager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [FilmManager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [FilmManager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [FilmManager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [FilmManager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [FilmManager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [FilmManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FilmManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FilmManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FilmManager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [FilmManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FilmManager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [FilmManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FilmManager] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [FilmManager] SET  MULTI_USER 
GO
ALTER DATABASE [FilmManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FilmManager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [FilmManager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [FilmManager] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [FilmManager] SET DELAYED_DURABILITY = DISABLED 
GO
USE [FilmManager]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 6/8/2020 9:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NULL,
	[Password] [varchar](256) NULL,
	[Fullname] [nvarchar](50) NULL,
	[Image] [varchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Phone] [varchar](15) NULL,
	[Email] [varchar](100) NULL,
	[Role] [int] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 6/8/2020 9:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Quantity] [int] NULL,
	[Status] [bit] NULL,
	[CreateByID] [int] NULL,
	[CreateTime] [datetime] NULL,
	[LastModifiedByID] [int] NULL,
	[LastModified] [datetime] NULL,
 CONSTRAINT [PK_Equipment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EquipmentImage]    Script Date: 6/8/2020 9:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EquipmentImage](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Url] [varchar](50) NULL,
	[EquipmentID] [bigint] NULL,
 CONSTRAINT [PK_EquipmentImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Scenario]    Script Date: 6/8/2020 9:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Scenario](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[Location] [nvarchar](50) NULL,
	[TimeStart] [datetime] NULL,
	[TimeEnd] [datetime] NULL,
	[RecordQuantity] [int] NULL,
	[Script] [varchar](50) NULL,
	[Status] [int] NULL,
	[CreateTime] [datetime] NULL,
	[CreateByID] [int] NULL,
	[LastModified] [datetime] NULL,
	[LastModifiedByID] [int] NULL,
 CONSTRAINT [PK_Scenario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ScenarioAccountDetail]    Script Date: 6/8/2020 9:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScenarioAccountDetail](
	[ScenarioID] [bigint] NOT NULL,
	[AccountID] [int] NOT NULL,
	[CreateTime] [datetime] NULL,
	[CreateByID] [int] NULL,
	[LastModified] [datetime] NULL,
	[LastModifiedByID] [int] NULL,
	[StartTime] [datetime] NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_ScenarioAccountDetail] PRIMARY KEY CLUSTERED 
(
	[ScenarioID] ASC,
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ScenarioEquipmentDetail]    Script Date: 6/8/2020 9:16:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScenarioEquipmentDetail](
	[ScenarioID] [bigint] NOT NULL,
	[EquipmentID] [bigint] NOT NULL,
	[Quantity] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[CreatedTime] [datetime] NULL,
	[CreatedByID] [int] NULL,
	[LastModified] [datetime] NULL,
	[LastModifiedByID] [int] NULL,
 CONSTRAINT [PK_ScenarioEquipmentDetail] PRIMARY KEY CLUSTERED 
(
	[ScenarioID] ASC,
	[EquipmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [Role]) VALUES (1, N'admin', N'admin', N'Admin', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [Role]) VALUES (2, N'thienphuc01', N'thienphuc01', NULL, NULL, NULL, NULL, NULL, 2)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [Role]) VALUES (3, N'thienphuc02', N'thienphuc02', NULL, NULL, NULL, NULL, NULL, 2)
SET IDENTITY_INSERT [dbo].[Account] OFF
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_Account] FOREIGN KEY([CreateByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [FK_Equipment_Account]
GO
ALTER TABLE [dbo].[Equipment]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_Account1] FOREIGN KEY([LastModifiedByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Equipment] CHECK CONSTRAINT [FK_Equipment_Account1]
GO
ALTER TABLE [dbo].[EquipmentImage]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentImage_Equipment] FOREIGN KEY([EquipmentID])
REFERENCES [dbo].[Equipment] ([ID])
GO
ALTER TABLE [dbo].[EquipmentImage] CHECK CONSTRAINT [FK_EquipmentImage_Equipment]
GO
ALTER TABLE [dbo].[Scenario]  WITH CHECK ADD  CONSTRAINT [FK_Scenario_Account] FOREIGN KEY([CreateByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Scenario] CHECK CONSTRAINT [FK_Scenario_Account]
GO
ALTER TABLE [dbo].[Scenario]  WITH CHECK ADD  CONSTRAINT [FK_Scenario_Account1] FOREIGN KEY([LastModifiedByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Scenario] CHECK CONSTRAINT [FK_Scenario_Account1]
GO
ALTER TABLE [dbo].[ScenarioAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioAccountDetail_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ScenarioAccountDetail] CHECK CONSTRAINT [FK_ScenarioAccountDetail_Account]
GO
ALTER TABLE [dbo].[ScenarioAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioAccountDetail_Account1] FOREIGN KEY([LastModifiedByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ScenarioAccountDetail] CHECK CONSTRAINT [FK_ScenarioAccountDetail_Account1]
GO
ALTER TABLE [dbo].[ScenarioAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioAccountDetail_Account2] FOREIGN KEY([LastModifiedByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ScenarioAccountDetail] CHECK CONSTRAINT [FK_ScenarioAccountDetail_Account2]
GO
ALTER TABLE [dbo].[ScenarioAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioAccountDetail_Scenario] FOREIGN KEY([ScenarioID])
REFERENCES [dbo].[Scenario] ([ID])
GO
ALTER TABLE [dbo].[ScenarioAccountDetail] CHECK CONSTRAINT [FK_ScenarioAccountDetail_Scenario]
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioEquipmentDetail_Account] FOREIGN KEY([CreatedByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail] CHECK CONSTRAINT [FK_ScenarioEquipmentDetail_Account]
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioEquipmentDetail_Account1] FOREIGN KEY([LastModifiedByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail] CHECK CONSTRAINT [FK_ScenarioEquipmentDetail_Account1]
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioEquipmentDetail_Equipment] FOREIGN KEY([EquipmentID])
REFERENCES [dbo].[Equipment] ([ID])
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail] CHECK CONSTRAINT [FK_ScenarioEquipmentDetail_Equipment]
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioEquipmentDetail_Scenario] FOREIGN KEY([ScenarioID])
REFERENCES [dbo].[Scenario] ([ID])
GO
ALTER TABLE [dbo].[ScenarioEquipmentDetail] CHECK CONSTRAINT [FK_ScenarioEquipmentDetail_Scenario]
GO
USE [master]
GO
ALTER DATABASE [FilmManager] SET  READ_WRITE 
GO
