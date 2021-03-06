USE [master]
GO
/****** Object:  Database [suux-db]    Script Date: 31/07/2019 12:32:55 ******/
CREATE DATABASE [suux-db]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'suux-db', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\suux-db.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'suux-db_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\suux-db_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [suux-db] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [suux-db].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [suux-db] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [suux-db] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [suux-db] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [suux-db] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [suux-db] SET ARITHABORT OFF 
GO
ALTER DATABASE [suux-db] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [suux-db] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [suux-db] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [suux-db] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [suux-db] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [suux-db] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [suux-db] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [suux-db] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [suux-db] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [suux-db] SET  DISABLE_BROKER 
GO
ALTER DATABASE [suux-db] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [suux-db] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [suux-db] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [suux-db] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [suux-db] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [suux-db] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [suux-db] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [suux-db] SET RECOVERY FULL 
GO
ALTER DATABASE [suux-db] SET  MULTI_USER 
GO
ALTER DATABASE [suux-db] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [suux-db] SET DB_CHAINING OFF 
GO
ALTER DATABASE [suux-db] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [suux-db] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [suux-db] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'suux-db', N'ON'
GO
ALTER DATABASE [suux-db] SET QUERY_STORE = OFF
GO
USE [suux-db]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [suux-db]
GO
/****** Object:  Table [dbo].[Banks]    Script Date: 31/07/2019 12:32:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banks](
	[bank_id] [int] IDENTITY(1,1) NOT NULL,
	[bank] [nvarchar](20) NOT NULL,
	[street] [nvarchar](20) NULL,
	[street_n] [int] NULL,
	[floor] [int] NULL,
	[departament] [varchar](2) NULL,
	[province_id] [int] NULL,
	[city] [nvarchar](20) NULL,
	[postal_code] [int] NULL,
	[phone_n] [bigint] NULL,
 CONSTRAINT [PK_Banks] PRIMARY KEY CLUSTERED 
(
	[bank_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Calendars]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calendars](
	[calendar_id] [int] NOT NULL,
	[file_n] [int] NOT NULL,
	[month] [date] NOT NULL,
 CONSTRAINT [PK_Calendars] PRIMARY KEY CLUSTERED 
(
	[calendar_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categorys]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categorys](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category] [nvarchar](20) NOT NULL,
	[description] [nvarchar](125) NULL,
	[basic_salary] [decimal](9, 2) NOT NULL,
	[amount_extra_1] [decimal](9, 2) NULL,
	[amount_extra_2] [decimal](9, 2) NULL,
	[amount_extra_3] [decimal](9, 2) NULL,
	[amount_extra_4] [decimal](9, 2) NULL,
 CONSTRAINT [PK_Categorys_1] PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companys]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companys](
	[company_id] [int] IDENTITY(1,1) NOT NULL,
	[company] [nvarchar](30) NOT NULL,
	[street] [nvarchar](20) NOT NULL,
	[street_n] [int] NOT NULL,
	[province_id] [int] NOT NULL,
	[city] [nvarchar](20) NOT NULL,
	[postal_code] [int] NOT NULL,
	[cuit_n] [bigint] NOT NULL,
	[phone_n] [bigint] NOT NULL,
 CONSTRAINT [PK_Companys] PRIMARY KEY CLUSTERED 
(
	[company_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Concept_types]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Concept_types](
	[concept_type_id] [int] NOT NULL,
	[concept_type] [nvarchar](30) NOT NULL,
	[concept_type_initials] [varchar](3) NOT NULL,
 CONSTRAINT [PK_Concept_types] PRIMARY KEY CLUSTERED 
(
	[concept_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Concepts]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Concepts](
	[concept_id] [int] IDENTITY(1,1) NOT NULL,
	[sorted_concept_id] [int] NOT NULL,
	[concept] [nvarchar](20) NOT NULL,
	[concept_type_id] [int] NOT NULL,
	[percentage] [decimal](5, 2) NULL,
	[amount] [decimal](9, 2) NULL,
	[formula_id] [int] NOT NULL,
	[common] [bit] NOT NULL,
	[quantity_editable] [bit] NOT NULL,
 CONSTRAINT [PK_Concepts] PRIMARY KEY CLUSTERED 
(
	[concept_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[file_n] [int] IDENTITY(1001,1) NOT NULL,
	[name] [nvarchar](30) NOT NULL,
	[last_name] [nvarchar](20) NOT NULL,
	[birthdate] [date] NOT NULL,
	[sex_id] [int] NULL,
	[document_n] [bigint] NOT NULL,
	[cuil_n] [bigint] NOT NULL,
	[email] [nvarchar](35) NULL,
	[phone_n] [bigint] NULL,
	[province_id] [int] NULL,
	[city] [nvarchar](20) NULL,
	[postal_code] [int] NULL,
	[street] [nvarchar](20) NULL,
	[street_n] [int] NULL,
	[floor] [int] NULL,
	[departament] [varchar](2) NULL,
	[prof_calification] [nvarchar](30) NULL,
	[basic_salary] [decimal](9, 2) NULL,
	[entry_date] [date] NOT NULL,
	[category_id] [int] NOT NULL,
	[social_work_id] [int] NOT NULL,
	[labor_union_id] [int] NOT NULL,
	[low_date] [date] NULL,
	[state] [bit] NOT NULL,
	[fixed_schedule_id] [int] NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[file_n] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees_liquidated]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees_liquidated](
	[employee_liquidated_id] [int] IDENTITY(1,1) NOT NULL,
	[liquidation_fixed_data_id] [int] NOT NULL,
	[file_n] [int] NOT NULL,
	[rem_total] [decimal](9, 2) NOT NULL,
	[no_rem_total] [decimal](9, 2) NOT NULL,
	[ded_total] [decimal](9, 2) NOT NULL,
	[basic_salary] [decimal](9, 2) NOT NULL,
	[gross_salary] [decimal](9, 2) NOT NULL,
	[net_salary] [decimal](9, 2) NOT NULL,
	[issue] [bit] NOT NULL,
 CONSTRAINT [PK_Employees_liquidated] PRIMARY KEY CLUSTERED 
(
	[employee_liquidated_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees_liquidated_concepts]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees_liquidated_concepts](
	[employee_liquidated_id] [int] NOT NULL,
	[sorted_concept_id] [int] NOT NULL,
	[concept] [nvarchar](40) NOT NULL,
	[quantity] [nvarchar](15) NULL,
	[rem] [decimal](9, 2) NULL,
	[no_rem] [decimal](9, 2) NULL,
	[ded] [decimal](9, 2) NULL,
 CONSTRAINT [PK_Employees_liquidated_concepts] PRIMARY KEY CLUSTERED 
(
	[employee_liquidated_id] ASC,
	[sorted_concept_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees-concepts]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees-concepts](
	[file_n] [int] NOT NULL,
	[concept_id] [int] NOT NULL,
 CONSTRAINT [PK_B_employees_concepts] PRIMARY KEY CLUSTERED 
(
	[file_n] ASC,
	[concept_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fixed_schedules]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fixed_schedules](
	[fixed_schedule_id] [int] IDENTITY(1,1) NOT NULL,
	[lu_m_d] [time](0) NULL,
	[lu_m_h] [time](0) NULL,
	[lu_l_d] [time](0) NULL,
	[lu_l_h] [time](0) NULL,
	[ma_m_d] [time](0) NULL,
	[ma_m_h] [time](0) NULL,
	[ma_l_d] [time](0) NULL,
	[ma_l_h] [time](0) NULL,
	[mi_m_d] [time](0) NULL,
	[mi_m_h] [time](0) NULL,
	[mi_l_d] [time](0) NULL,
	[mi_l_h] [time](0) NULL,
	[ju_m_d] [time](0) NULL,
	[ju_m_h] [time](0) NULL,
	[ju_l_d] [time](0) NULL,
	[ju_l_h] [time](0) NULL,
	[vi_m_d] [time](0) NULL,
	[vi_m_h] [time](0) NULL,
	[vi_l_d] [time](0) NULL,
	[vi_l_h] [time](0) NULL,
	[sa_m_d] [time](0) NULL,
	[sa_m_h] [time](0) NULL,
	[sa_l_d] [time](0) NULL,
	[sa_l_h] [time](0) NULL,
	[do_m_d] [time](0) NULL,
	[do_m_h] [time](0) NULL,
	[do_l_d] [time](0) NULL,
	[do_l_h] [time](0) NULL,
 CONSTRAINT [PK_Fixed_schedules] PRIMARY KEY CLUSTERED 
(
	[fixed_schedule_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Formulas]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Formulas](
	[formula_id] [int] IDENTITY(1,1) NOT NULL,
	[formula] [nvarchar](150) NOT NULL,
	[quantity] [nvarchar](100) NULL,
	[quantity_leyend] [nvarchar](5) NULL,
 CONSTRAINT [PK_Formulas] PRIMARY KEY CLUSTERED 
(
	[formula_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Holidays]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holidays](
	[month] [date] NOT NULL,
	[day] [int] NOT NULL,
	[description] [nvarchar](15) NULL,
 CONSTRAINT [PK_Holidays] PRIMARY KEY CLUSTERED 
(
	[month] ASC,
	[day] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Labor_unions]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Labor_unions](
	[labor_union_id] [int] IDENTITY(1,1) NOT NULL,
	[labor_union] [nvarchar](20) NOT NULL,
	[street] [nvarchar](20) NULL,
	[street_n] [int] NULL,
	[floor] [int] NULL,
	[departament] [varchar](2) NULL,
	[province_id] [int] NULL,
	[city] [nvarchar](20) NULL,
	[postal_code] [int] NULL,
	[phone_n] [bigint] NULL,
	[percentage_retention] [decimal](5, 2) NULL,
	[amount_retention] [decimal](9, 2) NULL,
 CONSTRAINT [PK_Labors_unions] PRIMARY KEY CLUSTERED 
(
	[labor_union_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Liquidation_fixed_datas]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Liquidation_fixed_datas](
	[liquidation_fixed_data_id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL,
	[period] [date] NOT NULL,
	[liquidation_type_id] [int] NOT NULL,
	[description] [nvarchar](20) NOT NULL,
	[deposited_date] [date] NULL,
	[bank_id] [int] NULL,
	[deposited_period] [date] NULL,
 CONSTRAINT [PK_Liquidation_fixed_datas] PRIMARY KEY CLUSTERED 
(
	[liquidation_fixed_data_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Liquidation_types]    Script Date: 31/07/2019 12:32:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Liquidation_types](
	[liquidation_type_id] [int] NOT NULL,
	[liquidation_type] [nvarchar](30) NULL,
	[liquidation_type_initials] [varchar](3) NULL,
 CONSTRAINT [PK_Settlement_types] PRIMARY KEY CLUSTERED 
(
	[liquidation_type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Provinces]    Script Date: 31/07/2019 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Provinces](
	[province_id] [int] NOT NULL,
	[province] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Provinces] PRIMARY KEY CLUSTERED 
(
	[province_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 31/07/2019 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[calendar_id] [int] NOT NULL,
	[day] [int] NOT NULL,
	[turn_id] [int] NULL,
	[fixed_schedule_id] [int] NULL,
	[nwork_day] [varchar](1) NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[calendar_id] ASC,
	[day] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sexs]    Script Date: 31/07/2019 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sexs](
	[sex_id] [int] IDENTITY(1,1) NOT NULL,
	[sex] [varchar](10) NOT NULL,
 CONSTRAINT [PK_Sexs] PRIMARY KEY CLUSTERED 
(
	[sex_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Social_works]    Script Date: 31/07/2019 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Social_works](
	[social_work_id] [int] IDENTITY(1,1) NOT NULL,
	[social_work] [nvarchar](20) NOT NULL,
	[street] [nvarchar](20) NULL,
	[street_n] [int] NULL,
	[floor] [int] NULL,
	[departament] [varchar](2) NULL,
	[province_id] [int] NULL,
	[city] [nvarchar](20) NULL,
	[postal_code] [int] NULL,
	[phone_n] [bigint] NULL,
	[percentage_retention] [decimal](5, 2) NULL,
	[amount_retention] [decimal](9, 2) NULL,
 CONSTRAINT [PK_Social_works] PRIMARY KEY CLUSTERED 
(
	[social_work_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Turns]    Script Date: 31/07/2019 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Turns](
	[turn_id] [int] IDENTITY(1,1) NOT NULL,
	[morning_d] [time](0) NULL,
	[morning_h] [time](0) NULL,
	[late_d] [time](0) NULL,
	[late_h] [time](0) NULL,
	[color] [nvarchar](9) NULL,
 CONSTRAINT [PK_Turns] PRIMARY KEY CLUSTERED 
(
	[turn_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 31/07/2019 12:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[user_name] [nvarchar](20) NOT NULL,
	[user_password] [nvarchar](20) NOT NULL,
	[name] [nvarchar](20) NOT NULL,
	[last_name] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[user_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Companys] ON 

INSERT [dbo].[Companys] ([company_id], [company], [street], [street_n], [province_id], [city], [postal_code], [cuit_n], [phone_n]) VALUES (1, N'EMPRESA', N'CALLE', 1234, 1, N'CIUDAD', 1234, 12345678911, 12345678)
SET IDENTITY_INSERT [dbo].[Companys] OFF
INSERT [dbo].[Concept_types] ([concept_type_id], [concept_type], [concept_type_initials]) VALUES (1, N'REMUNERATIVO', N'REM')
INSERT [dbo].[Concept_types] ([concept_type_id], [concept_type], [concept_type_initials]) VALUES (2, N'NO REMUNERATIVO', N'NRE')
INSERT [dbo].[Concept_types] ([concept_type_id], [concept_type], [concept_type_initials]) VALUES (3, N'RETENCIÓN', N'RET')
INSERT [dbo].[Liquidation_types] ([liquidation_type_id], [liquidation_type], [liquidation_type_initials]) VALUES (1, N'MENSUAL', N'MEN')
INSERT [dbo].[Liquidation_types] ([liquidation_type_id], [liquidation_type], [liquidation_type_initials]) VALUES (2, N'AGUINALDO', N'AGU')
INSERT [dbo].[Liquidation_types] ([liquidation_type_id], [liquidation_type], [liquidation_type_initials]) VALUES (3, N'VACACIONES', N'VAC')
INSERT [dbo].[Liquidation_types] ([liquidation_type_id], [liquidation_type], [liquidation_type_initials]) VALUES (4, N'EXTRAORDINARIA REMUNERATIVA', N'ERE')
INSERT [dbo].[Liquidation_types] ([liquidation_type_id], [liquidation_type], [liquidation_type_initials]) VALUES (5, N'EXTRAORDINARIA NO REMUNERATIVA', N'ENR')
INSERT [dbo].[Liquidation_types] ([liquidation_type_id], [liquidation_type], [liquidation_type_initials]) VALUES (6, N'BAJA', N'BAJ')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (1, N'Buenos Aires')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (2, N'Catamarca')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (3, N'Chaco')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (4, N'Chubut')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (5, N'Córdoba')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (6, N'Corrientes')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (7, N'Entre Ríos')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (8, N'Formosa')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (9, N'Jujuy')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (10, N'La Pampa')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (11, N'La Rioja')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (12, N'Mendoza')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (13, N'Misiones')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (14, N'Neuquén')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (15, N'Río Negro')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (16, N'Salta')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (17, N'San Juan')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (18, N'San Luis')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (19, N'Santa Cruz')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (20, N'Santa Fe')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (21, N'Santiago del Estero')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (22, N'Tierra del Fuego')
INSERT [dbo].[Provinces] ([province_id], [province]) VALUES (23, N'Tucumán')
SET IDENTITY_INSERT [dbo].[Sexs] ON 

INSERT [dbo].[Sexs] ([sex_id], [sex]) VALUES (1, N'Masculino')
INSERT [dbo].[Sexs] ([sex_id], [sex]) VALUES (2, N'Femenino')
SET IDENTITY_INSERT [dbo].[Sexs] OFF
INSERT [dbo].[Users] ([user_name], [user_password], [name], [last_name]) VALUES (N'MAURI', N'weimer', N'Mauricio Alejandro', N'Weimer')
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users]    Script Date: 31/07/2019 12:32:57 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED 
(
	[user_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employees_liquidated] ADD  CONSTRAINT [DF_Employees_liquidated_issue]  DEFAULT ((0)) FOR [issue]
GO
ALTER TABLE [dbo].[Banks]  WITH CHECK ADD  CONSTRAINT [FK_Banks_Provinces] FOREIGN KEY([province_id])
REFERENCES [dbo].[Provinces] ([province_id])
GO
ALTER TABLE [dbo].[Banks] CHECK CONSTRAINT [FK_Banks_Provinces]
GO
ALTER TABLE [dbo].[Calendars]  WITH CHECK ADD  CONSTRAINT [FK_Calendars_Employees] FOREIGN KEY([file_n])
REFERENCES [dbo].[Employees] ([file_n])
GO
ALTER TABLE [dbo].[Calendars] CHECK CONSTRAINT [FK_Calendars_Employees]
GO
ALTER TABLE [dbo].[Companys]  WITH CHECK ADD  CONSTRAINT [FK_Companys_Provinces] FOREIGN KEY([province_id])
REFERENCES [dbo].[Provinces] ([province_id])
GO
ALTER TABLE [dbo].[Companys] CHECK CONSTRAINT [FK_Companys_Provinces]
GO
ALTER TABLE [dbo].[Concepts]  WITH CHECK ADD  CONSTRAINT [FK_Concepts_Concept_types] FOREIGN KEY([concept_type_id])
REFERENCES [dbo].[Concept_types] ([concept_type_id])
GO
ALTER TABLE [dbo].[Concepts] CHECK CONSTRAINT [FK_Concepts_Concept_types]
GO
ALTER TABLE [dbo].[Concepts]  WITH CHECK ADD  CONSTRAINT [FK_Concepts_Formulas] FOREIGN KEY([formula_id])
REFERENCES [dbo].[Formulas] ([formula_id])
GO
ALTER TABLE [dbo].[Concepts] CHECK CONSTRAINT [FK_Concepts_Formulas]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Categorys] FOREIGN KEY([category_id])
REFERENCES [dbo].[Categorys] ([category_id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Categorys]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Fixed_schedules] FOREIGN KEY([fixed_schedule_id])
REFERENCES [dbo].[Fixed_schedules] ([fixed_schedule_id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Fixed_schedules]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Labor_unions] FOREIGN KEY([labor_union_id])
REFERENCES [dbo].[Labor_unions] ([labor_union_id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Labor_unions]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Provinces] FOREIGN KEY([province_id])
REFERENCES [dbo].[Provinces] ([province_id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Provinces]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Sexs] FOREIGN KEY([sex_id])
REFERENCES [dbo].[Sexs] ([sex_id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Sexs]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK_Employees_Social_works] FOREIGN KEY([social_work_id])
REFERENCES [dbo].[Social_works] ([social_work_id])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK_Employees_Social_works]
GO
ALTER TABLE [dbo].[Employees_liquidated]  WITH CHECK ADD  CONSTRAINT [FK_Employees_liquidated_Employees] FOREIGN KEY([file_n])
REFERENCES [dbo].[Employees] ([file_n])
GO
ALTER TABLE [dbo].[Employees_liquidated] CHECK CONSTRAINT [FK_Employees_liquidated_Employees]
GO
ALTER TABLE [dbo].[Employees_liquidated]  WITH CHECK ADD  CONSTRAINT [FK_Employees_liquidated_Liquidation_fixed_datas] FOREIGN KEY([liquidation_fixed_data_id])
REFERENCES [dbo].[Liquidation_fixed_datas] ([liquidation_fixed_data_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees_liquidated] CHECK CONSTRAINT [FK_Employees_liquidated_Liquidation_fixed_datas]
GO
ALTER TABLE [dbo].[Employees_liquidated_concepts]  WITH CHECK ADD  CONSTRAINT [FK_Employees_liquidated_concepts_Employees_liquidated] FOREIGN KEY([employee_liquidated_id])
REFERENCES [dbo].[Employees_liquidated] ([employee_liquidated_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees_liquidated_concepts] CHECK CONSTRAINT [FK_Employees_liquidated_concepts_Employees_liquidated]
GO
ALTER TABLE [dbo].[Employees-concepts]  WITH CHECK ADD  CONSTRAINT [FK_b_employees_concepts_Concepts] FOREIGN KEY([concept_id])
REFERENCES [dbo].[Concepts] ([concept_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employees-concepts] CHECK CONSTRAINT [FK_b_employees_concepts_Concepts]
GO
ALTER TABLE [dbo].[Employees-concepts]  WITH CHECK ADD  CONSTRAINT [FK_b_employees_concepts_Employees] FOREIGN KEY([file_n])
REFERENCES [dbo].[Employees] ([file_n])
GO
ALTER TABLE [dbo].[Employees-concepts] CHECK CONSTRAINT [FK_b_employees_concepts_Employees]
GO
ALTER TABLE [dbo].[Labor_unions]  WITH CHECK ADD  CONSTRAINT [FK_Labor_unions_Provinces] FOREIGN KEY([province_id])
REFERENCES [dbo].[Provinces] ([province_id])
GO
ALTER TABLE [dbo].[Labor_unions] CHECK CONSTRAINT [FK_Labor_unions_Provinces]
GO
ALTER TABLE [dbo].[Liquidation_fixed_datas]  WITH CHECK ADD  CONSTRAINT [FK_Liquidation_fixed_datas_Banks] FOREIGN KEY([bank_id])
REFERENCES [dbo].[Banks] ([bank_id])
GO
ALTER TABLE [dbo].[Liquidation_fixed_datas] CHECK CONSTRAINT [FK_Liquidation_fixed_datas_Banks]
GO
ALTER TABLE [dbo].[Liquidation_fixed_datas]  WITH CHECK ADD  CONSTRAINT [FK_Liquidation_fixed_datas_Liquidation_types] FOREIGN KEY([liquidation_type_id])
REFERENCES [dbo].[Liquidation_types] ([liquidation_type_id])
GO
ALTER TABLE [dbo].[Liquidation_fixed_datas] CHECK CONSTRAINT [FK_Liquidation_fixed_datas_Liquidation_types]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Calendars] FOREIGN KEY([calendar_id])
REFERENCES [dbo].[Calendars] ([calendar_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Calendars]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Fixed_schedules] FOREIGN KEY([fixed_schedule_id])
REFERENCES [dbo].[Fixed_schedules] ([fixed_schedule_id])
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Fixed_schedules]
GO
ALTER TABLE [dbo].[Schedules]  WITH CHECK ADD  CONSTRAINT [FK_Schedules_Turns] FOREIGN KEY([turn_id])
REFERENCES [dbo].[Turns] ([turn_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Schedules] CHECK CONSTRAINT [FK_Schedules_Turns]
GO
ALTER TABLE [dbo].[Social_works]  WITH CHECK ADD  CONSTRAINT [FK_Social_works_Provinces] FOREIGN KEY([province_id])
REFERENCES [dbo].[Provinces] ([province_id])
GO
ALTER TABLE [dbo].[Social_works] CHECK CONSTRAINT [FK_Social_works_Provinces]
GO
USE [master]
GO
ALTER DATABASE [suux-db] SET  READ_WRITE 
GO
