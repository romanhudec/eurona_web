/*

USE [master]
GO
ALTER DATABASE [eurona] SET SINGLE_USER with ROLLBACK IMMEDIATE
GO
DROP DATABASE [eurona]
GO

*/

USE [master]
GO
CREATE DATABASE [eurona] ON  PRIMARY 
(	NAME = N'eurona', 
	FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\eurona.mdf' , 
	SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB
)
LOG ON 
(	NAME = N'eurona_log', 
	FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER\MSSQL\DATA\eurona_log.ldf' , 
	SIZE = 1024KB , MAXSIZE = 102400KB , FILEGROWTH = 10%
)
COLLATE SLOVAK_CI_AS
GO

EXEC dbo.sp_dbcmptlevel @dbname=N'eurona', @new_cmptlevel=90
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eurona].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE [eurona] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [eurona] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [eurona] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [eurona] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [eurona] SET ARITHABORT OFF 
GO
ALTER DATABASE [eurona] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [eurona] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [eurona] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [eurona] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [eurona] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [eurona] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [eurona] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [eurona] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [eurona] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [eurona] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [eurona] SET  ENABLE_BROKER 
GO
ALTER DATABASE [eurona] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [eurona] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [eurona] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [eurona] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [eurona] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [eurona] SET  READ_WRITE 
GO
ALTER DATABASE [eurona] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [eurona] SET  MULTI_USER 
GO
ALTER DATABASE [eurona] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [eurona] SET DB_CHAINING OFF
GO

USE eurona;
GO

EXEC sp_grantlogin 'NT AUTHORITY\NETWORK SERVICE'
GO

EXEC sp_addrolemember 'db_owner', 'NT AUTHORITY\NETWORK SERVICE'
GO
