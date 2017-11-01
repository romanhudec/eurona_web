/*

USE [master]
GO
ALTER DATABASE [cms] SET SINGLE_USER with ROLLBACK IMMEDIATE
GO
DROP DATABASE [cms]
GO

*/

USE [master]
GO
CREATE DATABASE [cms] ON  PRIMARY 
(	NAME = N'cms', 
	FILENAME = N'D:\Databases\cms.mdf' , 
	SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB
)
LOG ON 
(	NAME = N'cms_log', 
	FILENAME = N'D:\Databases\cms_log.ldf' , 
	SIZE = 1024KB , MAXSIZE = 102400KB , FILEGROWTH = 10%
)
COLLATE SLOVAK_CI_AS
GO

EXEC dbo.sp_dbcmptlevel @dbname=N'cms', @new_cmptlevel=90
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [cms].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE [cms] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [cms] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [cms] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [cms] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [cms] SET ARITHABORT OFF 
GO
ALTER DATABASE [cms] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [cms] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [cms] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [cms] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [cms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [cms] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [cms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [cms] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [cms] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [cms] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [cms] SET  ENABLE_BROKER 
GO
ALTER DATABASE [cms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [cms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [cms] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [cms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [cms] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [cms] SET  READ_WRITE 
GO
ALTER DATABASE [cms] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [cms] SET  MULTI_USER 
GO
ALTER DATABASE [cms] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [cms] SET DB_CHAINING OFF
GO

USE cms;
GO

EXEC sp_grantlogin 'NT AUTHORITY\NETWORK SERVICE'
GO

EXEC sp_addrolemember 'db_owner', 'NT AUTHORITY\NETWORK SERVICE'
GO
