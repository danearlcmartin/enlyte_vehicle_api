USE [master]
GO
/****** Object:  Database [POC]    Script Date: 7/29/2022 6:43:47 PM ******/
CREATE DATABASE [POC]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'poc', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\poc.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'poc_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\poc_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [POC] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [POC].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [POC] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [POC] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [POC] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [POC] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [POC] SET ARITHABORT OFF 
GO
ALTER DATABASE [POC] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [POC] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [POC] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [POC] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [POC] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [POC] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [POC] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [POC] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [POC] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [POC] SET  DISABLE_BROKER 
GO
ALTER DATABASE [POC] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [POC] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [POC] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [POC] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [POC] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [POC] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [POC] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [POC] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [POC] SET  MULTI_USER 
GO
ALTER DATABASE [POC] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [POC] SET DB_CHAINING OFF 
GO
ALTER DATABASE [POC] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [POC] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [POC] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [POC] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [POC] SET QUERY_STORE = OFF
GO
USE [POC]
GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 7/29/2022 6:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [int] NULL,
	[Make] [nvarchar](100) NULL,
	[Model] [nvarchar](100) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Vehicle] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Vehicle] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  StoredProcedure [dbo].[AddUpdateVehicle]    Script Date: 7/29/2022 6:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dan Martin
-- Create date: 07-29-2022
-- Description:	Add/Update Vehicle 
-- =============================================
CREATE PROCEDURE [dbo].[AddUpdateVehicle]
	-- Add the parameters for the stored procedure here
	@p_Id int=0,
	@p_Year int,
	@p_Make nvarchar(100),
	@p_Model nvarchar(100)
AS
BEGIN
    -- Insert statements for procedure here
	BEGIN TRY 
		BEGIN TRANSACTION
			IF NOT EXISTS(SELECT TOP 1 1 FROM Vehicle WHERE Id = @p_Id)
			BEGIN 
				INSERT INTO Vehicle([Year],Make,Model) VALUES(@p_Year,@p_Make,@p_Model);

				SELECT 'succeed' as [Status],'New vehicle added.' as [Message]  
			END
			ELSE
			BEGIN 
				UPDATE Vehicle 
				SET [Year]=@p_Year,Make=@p_Make,@p_Model=@p_Model
				WHERE Id = @p_Id;

				SELECT 'succeed' as [Status],'The vehicle updated.' as [Message]
			END
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
			SELECT 'failed' as [Status],'error: ' + error_message() as [Message]  
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteVehicle]    Script Date: 7/29/2022 6:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Dan Martin
-- Create date: 07-29-2022
-- Description:	Delete Vehicle 
-- =============================================
CREATE PROCEDURE [dbo].[DeleteVehicle]
	-- Add the parameters for the stored procedure here
	@p_Id int=0
AS
BEGIN
	BEGIN TRY 
		BEGIN TRANSACTION
			UPDATE Vehicle SET IsDeleted = 1 WHERE Id = @p_Id;

			SELECT 'succeed' as [Status],'Vehicle deleted successfully.' as [Message]

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION
			SELECT 'failed' as [Status],'error: ' + error_message() as [Message]  
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[GetVehicle]    Script Date: 7/29/2022 6:43:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Dan Martin
-- Create date: 07-29-2022
-- Description:	Get Vehicle 
-- =============================================
CREATE PROCEDURE [dbo].[GetVehicle]
	-- Add the parameters for the stored procedure here
	@p_Id int=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@p_Id IS NULL)
		SELECT Id,[Year],[Make],[Model] FROM Vehicle WHERE IsDeleted=0;
	ELSE
		SELECT Id,[Year],[Make],[Model] FROM Vehicle WHERE IsDeleted=0 and Id = @p_Id;
END
GO
USE [master]
GO
ALTER DATABASE [POC] SET  READ_WRITE 
GO
