CREATE DATABASE AmigoWalletDB
GO
USE AmigoWalletDB
GO
IF OBJECT_ID('Customer')  IS NOT NULL
DROP TABLE Customer
GO
IF OBJECT_ID('usp_RegisterCustomer')  IS NOT NULL
DROP PROC usp_RegisterCustomer
GO
IF OBJECT_ID('usp_UpdatePassword')  IS NOT NULL
DROP PROC usp_UpdatePassword
GO
IF OBJECT_ID('ufn_GetUserDetails') IS NOT NULL
DROP FUNCTION ufn_GetUserDetails
GO


CREATE TABLE Customer
(
	[Name] varchar(40)  NOT NULL,
	[EmailId] VARCHAR(50) CONSTRAINT pk_EmailId PRIMARY KEY,
	[Password] VARCHAR(20) NOT NULL,
	[ContactNo] VARCHAR(11) CONSTRAINT chk_ContactNo CHECK(ContactNo Like '[1-9]%') NOT NULL
)
GO


CREATE PROCEDURE usp_RegisterCustomer
(
	@EmailId VARCHAR(50),
	@Password VARCHAR(20),
	@ContactNo VARCHAR(11),
	@Name VARCHAR(40)
)
AS
BEGIN
	BEGIN TRY
		IF NOT EXISTS (select * from Customer where EmailId = @EmailId)
		BEGIN 
			Return -1
		END
		IF (LEN(@EmailId)<4 OR LEN(@EmailId)>50 OR (@EmailId IS NULL))
			RETURN -2
		IF (LEN(@Password)<8 OR LEN(@Password)>16 OR (@Password IS NULL))
			RETURN -3
		IF(LEN(@Name)>40  OR (@Name IS NULL) )
			RETURN -4
		IF(LEN(@ContactNo)>11 OR (@ContactNo IS NULL))
			RETURN -5

		INSERT INTO Customer VALUES 
		(@Name,@EmailId,@Password, @ContactNo)
		RETURN 1
	END TRY
	BEGIN CATCH
		select ERROR_MESSAGE() as Msg
		RETURN -99
	END CATCH
END
GO


CREATE PROCEDURE usp_UpdatePassword
(
	@EmailId VARCHAR(50),
	@UserPassword VARCHAR(20)
)
AS
BEGIN
	BEGIN TRY
		IF (LEN(@UserPassword)<8 OR LEN(@UserPassword)>16 OR (@UserPassword IS NULL))
		BEGIN 
			Return -1
		END

		UPDATE Customer SET 
		Customer.Password= @UserPassword where Customer.EmailId=@EmailId
		RETURN 1
	END TRY
	BEGIN CATCH
		SELECT ERROR_MESSAGE() as msg
		RETURN -99
	END CATCH
END
GO

CREATE FUNCTION ufn_GetUserDetails(@EmailId VARCHAR(50))
RETURNS TABLE 
AS
RETURN (SELECT Name,ContactNo,Password
		FROM Customer 
		WHERE EmailId=@EmailId)
GO




insert into Customer values('vamsi krishna','vamsikrishnapapana@gmail.com','Vamsi@5d2','9848950299')

declare @result int
EXEC usp_RegisterCustomer 'vkpapana@gmail.com','vamsikp5d2','9848950299','vamsipapana'

exec usp_UpdatePassword 'vkpapana@gmail.com','vamsikp@5d2'

select * from ufn_GetUserDetails('vkpapana@gmail.com')