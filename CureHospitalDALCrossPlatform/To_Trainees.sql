USE [master]
GO
IF (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE ('[' + name + ']' = N'CureHospitalDB'OR name = N'CureHospitalDB')))
DROP DATABASE CureHospitalDB
GO

CREATE DATABASE CureHospitalDB
GO

USE CureHospitalDB
GO

--DROP Scripts
IF OBJECT_ID('Surgery') IS NOT NULL
	DROP TABLE Surgery
GO

IF OBJECT_ID('DoctorSpecialization') IS NOT NULL
	DROP TABLE DoctorSpecialization
GO

IF OBJECT_ID('Doctor') IS NOT NULL
	DROP TABLE Doctor
GO

IF OBJECT_ID('Specialization') IS NOT NULL
	DROP TABLE Specialization
GO

IF OBJECT_ID('usp_AddSurgeryDetails') IS NOT NULL
	DROP PROCEDURE usp_AddSurgeryDetails
GO

IF OBJECT_ID('usp_AddDoctorDetails') IS NOT NULL
	DROP PROCEDURE usp_AddDoctorDetails
GO

IF OBJECT_ID('ufn_FetchSurgeryDetails') IS NOT NULL
	DROP FUNCTION ufn_FetchSurgeryDetails
GO

IF OBJECT_ID('ufn_FetchSurgeryCount') IS NOT NULL
	DROP FUNCTION ufn_FetchSurgeryCount
GO


-- Scripts for creation of tables and insertion of sample data
CREATE TABLE Specialization
(
	SpecializationCode CHAR(3) CONSTRAINT pk_SpecializationCode PRIMARY KEY,
	SpecializationName VARCHAR(20) NOT NULL 
)
GO

INSERT INTO Specialization VALUES('GYN','Gynecologist')
INSERT INTO Specialization VALUES('CAR','Cardiologist')
INSERT INTO Specialization VALUES('ANE','Anesthesiologist')
GO


CREATE TABLE Doctor
(
	DoctorID  INT IDENTITY(1001,1) CONSTRAINT pk_DoctorID PRIMARY KEY,
	DoctorName VARCHAR(25) NOT NULL
)
GO

INSERT INTO Doctor VALUES('Albert')
INSERT INTO Doctor VALUES('Olivia')
INSERT INTO Doctor VALUES('Susan')
GO


CREATE TABLE DoctorSpecialization
(
	DoctorID  INT CONSTRAINT fk_DoctorID REFERENCES Doctor(DoctorId),
	SpecializationCode  CHAR(3) CONSTRAINT fk_SpecializatioinCode REFERENCES Specialization(SpecializationCode),
	SpecializationDate DATE NOT NULL,
	CONSTRAINT pk_DoctorIDSpecializatioinCode PRIMARY KEY (DoctorID,SpecializationCode)
)
GO

INSERT INTO DoctorSpecialization VALUES(1001,'ANE','2010-01-01')
INSERT INTO DoctorSpecialization VALUES(1002,'CAR','2010-01-01')
INSERT INTO DoctorSpecialization VALUES(1003,'CAR','2010-01-01')
GO


CREATE TABLE Surgery
(
	SurgeryID INT IDENTITY(5000,1) CONSTRAINT pk_SurgeryID PRIMARY KEY,
	DoctorID INT CONSTRAINT fk_DoctorID_Surgery REFERENCES Doctor(DoctorID),
	SurgeryDate DATE NOT NULL,
	StartTime DECIMAL(4,2) NOT NULL,
	EndTime DECIMAL(4,2) NOT NULL,
	SurgeryCategory CHAR(3) CONSTRAINT fk_SpecializatioinCode_Surgery REFERENCES Specialization(SpecializationCode)
)
GO
		
INSERT INTO Surgery VALUES(1001,'2011-01-01',9.00,14.00,'ANE')
INSERT INTO Surgery VALUES(1002,'2015-01-01',10.00,16.00,'CAR')
GO


--Stored Procedure : usp_InsertSurgeryDetails used to insert the surgery details 
CREATE PROCEDURE usp_AddSurgeryDetails
(
	@DoctorID INT ,
	@SurgeryDate DATE,
	@StartTime DECIMAL(4,2),
	@EndTime DECIMAL(4,2) ,
	@SurgeryCategory  CHAR(3),
	@SurgeryID INT OUT
)
AS
BEGIN
	SET @SurgeryID=-1
	 BEGIN TRY
		IF NOT EXISTS(SELECT * FROM Doctor WHERE  DoctorID=@DoctorID)
			RETURN -1
		IF NOT EXISTS(SELECT * FROM DoctorSpecialization WHERE  DoctorID=@DoctorID AND SpecializationCode=@SurgeryCategory)
			RETURN -2
		IF EXISTS(SELECT * FROM Surgery WHERE  DoctorID=@DoctorID AND SurgeryDate=@SurgeryDate AND (@StartTime BETWEEN StartTime AND EndTime OR @EndTime BETWEEN StartTime AND EndTime OR (@StartTime<StartTime AND @EndTime>EndTime)))
			RETURN -3			 
		INSERT INTO Surgery VALUES(@DoctorID,@SurgeryDate,@StartTime,@EndTime,@SurgeryCategory)
		SET @SurgeryID= @@IDENTITY
		RETURN 1  
	 END TRY
	 BEGIN CATCH
		RETURN -99
	 END CATCH
END
GO


--Stored Procedure : usp_AddDoctorDetails used to insert the doctor details 
CREATE PROCEDURE usp_AddDoctorDetails
(
	@DoctorName VARCHAR(25),
	@SpecializationCode  CHAR(3),
	@SpecializationName VARCHAR(20)
)
AS
BEGIN
	
	 BEGIN TRY
		IF NOT EXISTS(SELECT * FROM Specialization WHERE SpecializationCode=@SpecializationCode)
			INSERT INTO Specialization VALUES(@SpecializationCode,@SpecializationName)		
		INSERT INTO Doctor VALUES(@DoctorName)
		DECLARE @DoctorID INT
		SET @DoctorID = @@IDENTITY
		INSERT INTO DoctorSpecialization VALUES(@DoctorID,@SpecializationCode,GETDATE())
		RETURN 1  
	 END TRY
	 BEGIN CATCH
		RETURN -99
	 END CATCH
END
GO


--Function -- ufn_FetchSurgeryDetails to fetch surgery details
CREATE FUNCTION ufn_FetchSurgeryDetails( @SurgeryDate date)
RETURNS TABLE
AS
RETURN 
			(SELECT	SurgeryID,
					DoctorID,
					SurgeryDate,
					StartTime,
					EndTime,
					SurgeryCategory
					FROM  Surgery
					WHERE SurgeryDate =@SurgeryDate)
	
GO


--Function -- ufn_FetchSurgeryCount to fetch the number of surgeries
CREATE FUNCTION ufn_FetchSurgeryCount 
(
	@SpecializationName VARCHAR(20),
	@SurgeryDate DATE
)
RETURNS INT
AS
BEGIN
	DECLARE @Count INT
	DECLARE @SpecializationCode VARCHAR(20)

	SELECT @SpecializationCode = SpecializationCode FROM Specialization WHERE SpecializationName = @SpecializationName

	SELECT @Count = COUNT(SurgeryID) FROM Surgery WHERE SurgeryCategory = @SpecializationCode AND SurgeryDate = @SurgeryDate

	RETURN @Count
END
GO
