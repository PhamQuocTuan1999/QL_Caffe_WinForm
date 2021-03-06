CREATE DATABASE QuanLyQuanCafe
Go

USE QuanLyQuanCafe
Go

--Food
--Table
--FoodCategory
--Account
--BillInfo

CREATE TABLE TableFood(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100)NOT NULL DEFAULT N'Chưa đặt tên',
	status NVARCHAR(100) NOT NULL DEFAULT N'Trống' --Trong || Co nguoi 
)
GO

CREATE TABLE Account(
	UserName NVARCHAR(100) PRIMARY KEY,
	DisplayName NVARCHAR(100) NOT NULL DEFAULT N'Kter',
	PassWord NVARCHAR(1000) NOT NULL DEFAULT 0,
	Type INT NOT NULL DEFAULT 0 --1:admin|| 0:staff
)
GO

CREATE TABLE FoodCategory(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR NOT NULL DEFAULT N'Chưa đặt tên'
)

ALTER TABLE FoodCategory 
ALTER COLUMN name NVARCHAR(100) NOT NULL;

CREATE TABLE Food(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100) NOT NULL DEFAULT N'Chưa đặt tên',
	idCategory INT NOT NULL,
	price FLOAT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idCategory) REFERENCES dbo.FoodCategory(id)
)
GO

CREATE TABLE Bill(
	id INT IDENTITY PRIMARY KEY,
	DateCheckIn DATE NOT NULL DEFAULT GETDATE(),
	DateCheckOut DATE , 
	idTable INT NOT NULL,
	status INT NOT NULL DEFAULT 0 --1: da thanh toan||0: chua thanh toan
	
	FOREIGN KEY (idTable) REFERENCES dbo.TableFood(id)
)
GO

CREATE TABLE BillInfo(
	id INT IDENTITY PRIMARY KEY,
	idBill INT NOT NULL,
	idFood INT NOT NULL,
	count INT NOT NULL DEFAULT 0
	
	FOREIGN KEY (idBill) REFERENCES dbo.Bill(id),
	FOREIGN KEY (idFood) REFERENCES dbo.Food(id)
	)
GO

INSERT INTO dbo.Account
(
	UserName,
	DisplayName,
	PassWord,
	Type
)
VALUES (
	N'K9',
	N'RongK9',
	N'1',
	1
)

INSERT INTO dbo.Account
(
	UserName,
	DisplayName,
	PassWord,
	Type
)
VALUES (
	N'staff',
	N'staff',
	N'1',
	0
)
GO

CREATE PROCEDURE USP_GetAccountByUserName
@userName nvarchar(100)
AS
BEGIN 
	SELECT * FROM dbo.Account WHERE UserName = @userName
END
GO

EXEC dbo.USP_GetAccountByUserName @userName = N'K9' 

GO

CREATE PROC USP_Login
@userName nvarchar(100), @passWord nvarchar(100)
AS
BEGIN
	SELECT * FROM dbo.Account WHERE UserName = @userName AND PassWord = @passWord
END
GO

--them ban
DECLARE @i INT = 0
WHILE @i <=14
BEGIN
	INSERT INTO dbo.TableFood (name) VALUES (N'Bàn '+ CAST(@i+1 AS nvarchar(100)))
	SET @i = @i+1;
END
GO

CREATE PROC USP_GetTableList
AS
SELECT * FROM dbo.TableFood

GO

EXEC dbo.USP_GetTableList

--them category
INSERT INTO dbo.FoodCategory (name) VALUES (N'Cà phê đá xay')
INSERT INTO dbo.FoodCategory (name) VALUES (N'Nước ép')
INSERT INTO dbo.FoodCategory (name) VALUES (N'Sinh tố')
INSERT INTO dbo.FoodCategory (name) VALUES (N'Milkshake')

--them mon
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Cà phê đá xay', 9, 25000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Bánh Oreo đá xay', 9, 22000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Sôcôla chuối', 9, 25000)

INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Cam', 10, 20000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Dưa hấu', 10, 20000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Táo', 10, 17000)

INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Dừa', 11, 25000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Bơ', 11, 22000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Xoài', 11, 25000)

INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Thơm + Bạc hà', 12, 22000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Sầu riêng', 12, 22000)
INSERT INTO dbo.Food (name, idCategory, price) VALUES (N'Trà xanh Matcha', 12, 22000)

--them bill
INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), null, 2525951, 0)

INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), null, 2525952, 0)

INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)
VALUES (GETDATE(), GETDATE(), 2525953, 1)

--them billinfo
INSERT dbo.BillInfo(idBill, idFood, count)
VALUES (4, 1, 2)

INSERT dbo.BillInfo(idBill, idFood, count)
VALUES (5, 2, 2)

INSERT dbo.BillInfo(idBill, idFood, count)
VALUES (6, 5, 2)
GO

create PROC USP_InsertBill
@idTable INT
AS
BEGIN
	INSERT INTO dbo.Bill
	(
		DateCheckIn,
		DateCheckOut,
		idTable,
		status,
		discount
	)
	VALUES (GETDATE(),
			NULL,
			@idTable,
			0,
			0)
END
GO
--them mon vao bill
ALTER PROC USP_InsertBillInfo
@idBill INT,@idFood INT, @count INT
AS
BEGIN
	DECLARE @isExitsBillInfo INT
	DECLARE @foodCount INT =1
	
	SELECT @isExitsBillInfo = id, @foodCount = b.count 
	FROM dbo.BillInfo AS b
	WHERE idBill = @idBill AND idFood = @idFood
	
	IF(@isExitsBillInfo > 0)
	BEGIN
		DECLARE @newCount INT = @foodCount + @count
		IF(@newCount>0)
			UPDATE dbo.BillInfo SET count =@foodCount + @count WHERE idFood=@idFood
		ELSE DELETE dbo.BillInfo WHERE idBill = @idBill AND idFood = @idFood
	END
	ELSE
	BEGIN
		INSERT dbo.BillInfo(idBill, idFood, count)
	VALUES (@idBill, @idFood, @count)
	END
END

GO


--thay doi status cua ban
alter TRIGGER UTG_UpdateBillInfo
ON dbo.BillInfo FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = idBill FROM Inserted
	DECLARE @idTable INT
	
	
	
	SELECT @idTable = idTable FROM dbo.Bill WHERE id = @idBill AND status = 0
	
	DECLARE @count INT
	select @count = COUNT (*) FROM dbo.BillInfo WHERE idBill = @idBill
	if(@count>0)
	BEGIN
		UPDATE dbo.TableFood SET status = N'Có người' WHERE id = @idTable 

	END
	ELSE
	BEGIN
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable 

	END
	END
GO

ALTER TRIGGER UTP_UpdateBill
ON dbo.Bill FOR UPDATE 
AS
BEGIN
	DECLARE @idBill INT
	SELECT @idBill = id FROM Inserted
	DECLARE @idTable INT
	SELECT @idTable= idTable FROM dbo.Bill WHERE id = @idBill
	DECLARE @count int =0
	SELECT @count = COUNT (*) FROM dbo.Bill WHERE idTable = @idTable AND status=0
	IF(@count = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable
END
GO

ALTER Table dbo.Bill
Add discount INT

UPDATE dbo.Bill Set discount = 0
GO

alTER PROC USP_SwitchTable
@idTable1 INT, @idTable2 INT
AS BEGIN
	DECLARE @idFirstBill INT
	DECLARE @idSecondBill INT
	DECLARE @idFirstBillEmpty INT = 1
	DECLARE @idSecondBillEmpty INT = 1
	
	SELECT @idSecondBill = id FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
	SELECT @idFirstBill = id FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
	
	IF(@idFirstBill is null)
	BEGIN
		INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)
		VALUES (GETDATE(), null, @idTable1, 0)
		SELECT @idFirstBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable1 AND status = 0
		
	END
	select @idFirstBillEmpty = COUNT (*) FROM dbo.BillInfo where idBill = @idFirstBill
	IF(@idSecondBill IS null)
	BEGIN
		INSERT dbo.Bill (DateCheckIn, DateCheckOut, idTable, status)
		VALUES (GETDATE(), null, @idTable2, 0)
		SELECT @idSecondBill = MAX(id) FROM dbo.Bill WHERE idTable = @idTable2 AND status = 0
		
	END
	select @idSecondBillEmpty = COUNT (*) FROM dbo.BillInfo where idBill = @idSecondBill
	
	SELECT id INTO IDBillnfoTable FROM dbo.BillInfo WHERE idBill = @idSecondBill
	UPDATE dbo.BillInfo SET idBill = @idSecondBill WHERE idBill = @idFirstBill
	UPDATE dbo.BillInfo SET idBill = @idFirstBill WHERE id IN (SELECT * FROM IDBillnfoTable)
	DROP TABLE IDBillnfoTable
	if(@idFirstBillEmpty = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable2
	
	if(@idSecondBillEmpty = 0)
		UPDATE dbo.TableFood SET status = N'Trống' WHERE id = @idTable1
	
	END
	GO
	
ALTER TABLE dbo.Bill ADD totalPrice FLOAT 
GO

ALTER PROC USP_GetListBillByDate
@checkIn date, @checkOut date
AS
BEGIN
	SELECT t.name as [Tên bàn], b.totalPrice as [Tổng tiền], DateCheckIn as [Ngày vào], DateCheckOut as [Ngày ra], discount as [Giảm giá]
	FROM dbo.Bill AS b, dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable

END
GO

CREATE PROC USP_UpdateAcount
@userName NVARCHAR(100), @displayName NVARCHAR(100), @password NVARCHAR(100), @newPassword NVARCHAR(100)
AS
BEGIN
	DECLARE @isRightPass INT = 0
	SELECT @isRightPass = COUNT(*) FROM dbo.Account WHERE UserName = @userName AND PassWord = @password
	
	if(@isRightPass = 1)
	BEGIN
		IF(@newPassword = NULL OR @newPassword = '')
		BEGIN
			UPDATE dbo.Account SET DisplayName = @displayName WHERE UserName = @userName
		END
		ELSE
			update dbo.Account set DisplayName = @displayName, PassWord = @newPassword where UserName = @userName
	END
END
GO

create trigger UTG_DeleteBillInfo
ON dbo.BillInfo FOR DELETE
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id, @idBill = deleted.idBill from deleted
	
	declare @idTable int
	select @idTable = idTable from dbo.Bill where id = @idBill
	
	declare @count int = 0
	select @count = COUNT (*)from dbo.BillInfo as bi, dbo.Bill as b where b.id = bi.idBill and b.id = @idBill and b.status = 0
	if(@count = 0)
	update dbo.TableFood set status = N'Trống' where id = @idTable
end
go

alter PROC USP_GetListBillByDateAndPage
@checkIn date, @checkOut date, @page int
AS
BEGIN
	declare @pageRows INT = 10
	declare @selectRows int = @pageRows
	declare @exceptRows int = (@page -1) * @pageRows

	
	 ;WITH BillShow as(  SELECT b.ID, t.name as [Tên bàn], b.totalPrice as [Tổng tiền], DateCheckIn as [Ngày vào], DateCheckOut as [Ngày ra], discount as [Giảm giá]
	FROM dbo.Bill AS b, dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable)
	
	select top (@selectRows) * from BillShow where id not in (select top (@exceptRows) id from BillShow)
	

END
GO

Create PROC USP_GetNumBillByDate
@checkIn date, @checkOut date
AS
BEGIN
	SELECT COUNT(*) FROM dbo.Bill AS b, dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable

END
GO

CREATE PROC USP_GetListBillByDateForRoport
@checkIn date, @checkOut date
AS
BEGIN
	SELECT t.name , b.totalPrice , DateCheckIn, DateCheckOut, discount
	FROM dbo.Bill AS b, dbo.TableFood AS t
	WHERE DateCheckIn >= @checkIn AND DateCheckOut <= @checkOut AND b.status = 1 AND t.id = b.idTable

END
GO

CREATE TABLE Kho(
	id INT IDENTITY PRIMARY KEY,
	name NVARCHAR(100)NOT NULL DEFAULT N'Chưa đặt tên',
	soluong float NOT NULL DEFAULT 0 --Trong || Co nguoi 
)
GO
select * from Kho