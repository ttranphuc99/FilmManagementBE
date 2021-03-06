USE [master]
GO
/****** Object:  Database [FilmManager]    Script Date: 7/4/2020 4:25:58 PM ******/
CREATE DATABASE [FilmManager]
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
ALTER DATABASE [FilmManager] SET  ENABLE_BROKER 
GO
ALTER DATABASE [FilmManager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [FilmManager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [FilmManager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [FilmManager] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [FilmManager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [FilmManager] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [FilmManager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [FilmManager] SET RECOVERY FULL 
GO
ALTER DATABASE [FilmManager] SET  MULTI_USER 
GO
ALTER DATABASE [FilmManager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [FilmManager] SET DB_CHAINING OFF 
GO
USE [FilmManager]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 7/4/2020 4:25:58 PM ******/
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
	[Image] [varchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Phone] [varchar](15) NULL,
	[Email] [varchar](100) NULL,
	[DeviceToken] [varchar](max) NULL,
	[Role] [int] NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Equipment]    Script Date: 7/4/2020 4:25:58 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[EquipmentImage]    Script Date: 7/4/2020 4:25:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EquipmentImage](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Url] [varchar](max) NULL,
	[EquipmentID] [bigint] NULL,
 CONSTRAINT [PK_EquipmentImage] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Scenario]    Script Date: 7/4/2020 4:25:58 PM ******/
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
	[Script] [varchar](max) NULL,
	[Status] [int] NULL,
	[CreateTime] [datetime] NULL,
	[CreateByID] [int] NULL,
	[LastModified] [datetime] NULL,
	[LastModifiedByID] [int] NULL,
 CONSTRAINT [PK_Scenario] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ScenarioAccountDetail]    Script Date: 7/4/2020 4:25:58 PM ******/
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
	[Characters] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ScenarioAccountDetail] PRIMARY KEY CLUSTERED 
(
	[ScenarioID] ASC,
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[ScenarioEquipmentDetail]    Script Date: 7/4/2020 4:25:58 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (1, N'admin', N'admin', N'fullname changed', NULL, N'Test', N'090000', NULL, NULL, 1, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (2, N'thienphuc01', N'thienphuc01', N'Trần Thiên Phúc', N'https://pbs.twimg.com/media/Dc7ZGXfWkAACvjV.jpg', N'Test test test', NULL, N'phuc@gmail.com', NULL, 2, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (3, N'thienphuc02', N'thienphuc02', N'Nguyễn Thiên Minh', N'https://data.whicdn.com/images/315285129/original.jpg', N'No on on', NULL, N'thienm@gmail.com', N'e02bWqL51ac:APA91bGOmydPueGR1GWAZFp0mKJ-F4g_-U2JOlmMD3oDFH5pTHdarBBPf-kyOZLcW74aFyuX-6McrCaHoaEHmBYHAkXtXqq-CrCRPaF9rYjd0CDEUxyzCVZT5ezzyGqINQ1Y1Gop4n9N', 2, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (4, N'acc01', N'1', N'name', N'', NULL, N'09001200', NULL, NULL, 2, 0)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (5, N'hkhk', N'hkhk', N'Hong Kong 1', NULL, NULL, N'', N'', NULL, 2, 0)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (6, N'hkhk2', N'hkhk2', N'hk', NULL, NULL, N'09098765', N'ahi@hi', NULL, 2, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (7, N'acc02', N'acc02', N'Test actor', NULL, NULL, N'19234', N'test@test', NULL, 2, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (8, N'fukufuku', N'fukufuku', N'Fuku San Desu', NULL, NULL, N'09786452', N'fuku@jp.co', NULL, 2, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (9, N'taido', N'taido', N'đỗ lương tài', NULL, NULL, N'03972448222', N'cebro@gmail', NULL, 2, 1)
INSERT [dbo].[Account] ([ID], [Username], [Password], [Fullname], [Image], [Description], [Phone], [Email], [DeviceToken], [Role], [Status]) VALUES (10, N'maniha', N'maniha', N'Manhida Taraka', NULL, NULL, N'025874', N'taraka@jp.co', NULL, 2, 1)
SET IDENTITY_INSERT [dbo].[Account] OFF
SET IDENTITY_INSERT [dbo].[Equipment] ON 

INSERT [dbo].[Equipment] ([ID], [Name], [Description], [Quantity], [Status], [CreateByID], [CreateTime], [LastModifiedByID], [LastModified]) VALUES (1, N'eq 1111111', N'for something i dont know', 90, 1, 1, CAST(N'2020-06-30 20:06:48.000' AS DateTime), 1, CAST(N'2020-07-01 10:46:04.270' AS DateTime))
INSERT [dbo].[Equipment] ([ID], [Name], [Description], [Quantity], [Status], [CreateByID], [CreateTime], [LastModifiedByID], [LastModified]) VALUES (3, N'eq 3', N'for something i dont know', 68, 1, 1, CAST(N'2020-06-30 20:06:48.823' AS DateTime), 1, CAST(N'2020-07-01 10:33:56.500' AS DateTime))
INSERT [dbo].[Equipment] ([ID], [Name], [Description], [Quantity], [Status], [CreateByID], [CreateTime], [LastModifiedByID], [LastModified]) VALUES (5, N'houyli', N'fiary tail', 120, 1, 1, CAST(N'2020-07-01 02:36:08.820' AS DateTime), 1, CAST(N'2020-07-02 18:34:23.283' AS DateTime))
INSERT [dbo].[Equipment] ([ID], [Name], [Description], [Quantity], [Status], [CreateByID], [CreateTime], [LastModifiedByID], [LastModified]) VALUES (9, N'babir', N'papi', 25, 1, 1, CAST(N'2020-07-01 10:46:28.927' AS DateTime), 1, CAST(N'2020-07-01 10:46:43.303' AS DateTime))
SET IDENTITY_INSERT [dbo].[Equipment] OFF
SET IDENTITY_INSERT [dbo].[EquipmentImage] ON 

INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (8, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker4641205567676287185.jpg?alt=media&token=00789337-d5d4-4023-93ac-8cdfbc2063c1', 5)
INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (9, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker8413644524056086510.jpg?alt=media&token=26ab25fb-3cba-4608-80b1-c30ee158dbad', 1)
INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (10, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker4016090796740203055.jpg?alt=media&token=31708ef3-f551-4015-bbf9-bef4236f5b56', 3)
INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (11, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker686591890593948918.jpg?alt=media&token=4f8c18d2-5e84-4611-9d40-3326b5fa54e2', 3)
INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (12, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker3807877164313611699.jpg?alt=media&token=059796db-5e61-4b8e-8ca2-dfcdcf7ac028', 3)
INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (13, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker7688002248898043131.jpg?alt=media&token=6b0bfe22-6780-4617-9dbc-95b03559eb07', 9)
INSERT [dbo].[EquipmentImage] ([ID], [Url], [EquipmentID]) VALUES (14, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2Fimage_picker5352271760746395705.jpg?alt=media&token=79968f32-abab-40e0-9d50-dd7d388c2ecc', 5)
SET IDENTITY_INSERT [dbo].[EquipmentImage] OFF
SET IDENTITY_INSERT [dbo].[Scenario] ON 

INSERT [dbo].[Scenario] ([ID], [Name], [Description], [Location], [TimeStart], [TimeEnd], [RecordQuantity], [Script], [Status], [CreateTime], [CreateByID], [LastModified], [LastModifiedByID]) VALUES (1, N'Name is changed', N'changed description', N'Ho Con Rua changed', CAST(N'2020-05-06 12:00:00.000' AS DateTime), CAST(N'2020-05-06 22:00:00.000' AS DateTime), 8, NULL, -1, CAST(N'2020-06-24 11:28:34.190' AS DateTime), 1, CAST(N'2020-06-30 08:07:39.213' AS DateTime), 1)
INSERT [dbo].[Scenario] ([ID], [Name], [Description], [Location], [TimeStart], [TimeEnd], [RecordQuantity], [Script], [Status], [CreateTime], [CreateByID], [LastModified], [LastModifiedByID]) VALUES (3, N'Test cảnh quay 2', NULL, N'Hồ Con Rùa', CAST(N'2020-01-01 05:00:00.000' AS DateTime), CAST(N'2020-01-05 18:00:00.000' AS DateTime), NULL, NULL, 0, CAST(N'2020-06-24 13:41:55.990' AS DateTime), 1, NULL, NULL)
INSERT [dbo].[Scenario] ([ID], [Name], [Description], [Location], [TimeStart], [TimeEnd], [RecordQuantity], [Script], [Status], [CreateTime], [CreateByID], [LastModified], [LastModifiedByID]) VALUES (5, N'test 2d', N'', N'yu', CAST(N'2020-06-01 06:53:00.000' AS DateTime), CAST(N'2020-06-26 10:53:00.000' AS DateTime), 4, NULL, 1, CAST(N'2020-06-26 03:55:37.827' AS DateTime), 1, CAST(N'2020-06-30 10:02:29.667' AS DateTime), 1)
INSERT [dbo].[Scenario] ([ID], [Name], [Description], [Location], [TimeStart], [TimeEnd], [RecordQuantity], [Script], [Status], [CreateTime], [CreateByID], [LastModified], [LastModifiedByID]) VALUES (7, N'test', N'edit sc', N'HCI', CAST(N'2020-06-30 14:47:00.000' AS DateTime), CAST(N'2020-06-30 16:47:00.000' AS DateTime), 5, N'https://firebasestorage.googleapis.com/v0/b/filmmanager-785d6.appspot.com/o/script%2FHDSD%2BVISA%2BSLIVER%26GOLD_2018.pdf-2020-07-01%2017%3A47%3A34.059916.pdf?alt=media&token=9867ca91-4a2c-4d73-b828-a751c56fbac1', 0, CAST(N'2020-06-30 07:47:27.967' AS DateTime), 1, CAST(N'2020-07-01 10:47:37.427' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Scenario] OFF
INSERT [dbo].[ScenarioAccountDetail] ([ScenarioID], [AccountID], [CreateTime], [CreateByID], [LastModified], [LastModifiedByID], [Characters]) VALUES (1, 2, CAST(N'2020-06-24 13:30:51.083' AS DateTime), 1, NULL, NULL, N'Quần chúng, Nam chính')
INSERT [dbo].[ScenarioAccountDetail] ([ScenarioID], [AccountID], [CreateTime], [CreateByID], [LastModified], [LastModifiedByID], [Characters]) VALUES (3, 3, CAST(N'2020-06-24 13:42:42.930' AS DateTime), 1, CAST(N'2020-06-24 13:43:28.413' AS DateTime), 1, N'bạn trai nam chính')
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Account] FOREIGN KEY([ID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Account]
GO
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
ALTER TABLE [dbo].[ScenarioAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_ScenarioAccountDetail_Account_3] FOREIGN KEY([CreateByID])
REFERENCES [dbo].[Account] ([ID])
GO
ALTER TABLE [dbo].[ScenarioAccountDetail] CHECK CONSTRAINT [FK_ScenarioAccountDetail_Account_3]
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
