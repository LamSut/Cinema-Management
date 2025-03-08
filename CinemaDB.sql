-- USE master;
-- DROP DATABASE IF EXISTS CinemaDB;
-- DROP PROCEDURE IF EXISTS Add_Show;
-- DROP PROCEDURE IF EXISTS Book_Seat;

CREATE DATABASE CinemaDB;
GO

USE CinemaDB;
GO

SELECT * FROM Employee;
SELECT * FROM Booking;
SELECT * FROM Movie;
SELECT * FROM Show;
SELECT * FROM SeatBooking;

-- NHÂN VIÊN
CREATE TABLE Employee (
    EmployeeID NVARCHAR(50) PRIMARY KEY,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Role BIT NOT NULL -- 0: nhân viên, 1: admin
);

-- PHIM
CREATE TABLE Movie (
    MovieID NVARCHAR(50) PRIMARY KEY,
    Title NVARCHAR(255) NOT NULL,
    Duration INT NOT NULL,  -- phút
    Genre NVARCHAR(100)
);

-- SUẤT CHIẾU
CREATE TABLE Show (
    ShowID NVARCHAR(50) PRIMARY KEY,
    MovieID NVARCHAR(50) NOT NULL,
    ShowTime DATETIME NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (MovieID) REFERENCES Movie(MovieID) ON DELETE CASCADE
);

-- ĐẶT VÉ
CREATE TABLE Booking (
    BookingID NVARCHAR(50) PRIMARY KEY,
    ShowID NVARCHAR(50) NOT NULL,
    BookingTime DATETIME NOT NULL,
    FOREIGN KEY (ShowID) REFERENCES Show(ShowID) ON DELETE CASCADE
);

-- ĐẶT CHỖ NGỒI CỤ THỂ
CREATE TABLE SeatBooking (
    BookingID NVARCHAR(50) NOT NULL,
    SeatID NVARCHAR(10) NOT NULL,
    PRIMARY KEY (BookingID, SeatID),
    FOREIGN KEY (BookingID) REFERENCES Booking(BookingID) ON DELETE CASCADE
);
GO

-- THÊM SUẤT CHIẾU (KIỂM TRA TRÙNG GIỜ TRƯỚC KHI THÊM)
CREATE PROCEDURE Add_Show
    @NewShowID NVARCHAR(50),
    @NewMovieID NVARCHAR(50),
    @NewShowTime DATETIME,
    @NewDuration INT,
    @NewPrice DECIMAL(10,2)
AS
BEGIN
    DECLARE @NewEndTime DATETIME;
    SET @NewEndTime = DATEADD(MINUTE, @NewDuration, @NewShowTime);

    IF EXISTS (
        SELECT 1 
        FROM Show s
        JOIN Movie m ON s.MovieID = m.MovieID
        WHERE s.ShowTime < @NewEndTime
          AND DATEADD(MINUTE, m.Duration, s.ShowTime) > @NewShowTime
    )
    BEGIN
        PRINT N'Lỗi: Suất chiếu này bị trùng với một phim khác!';
        RETURN -1;
    END

    INSERT INTO Show (ShowID, MovieID, ShowTime, Price)
    VALUES (@NewShowID, @NewMovieID, @NewShowTime, @NewPrice);
END;
GO

-- ĐẶT CHỖ NGỒI (KIỂM TRA GHẾ TRƯỚC KHI THÊM)
CREATE PROCEDURE Book_Seat
    @BookingID NVARCHAR(50),
    @SeatID NVARCHAR(10)
AS
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM SeatBooking sb
        JOIN Booking b ON sb.BookingID = b.BookingID
        WHERE b.ShowID = (SELECT ShowID FROM Booking WHERE BookingID = @BookingID)
          AND sb.SeatID = @SeatID
    )
    BEGIN
        PRINT N'Lỗi: Ghế này đã được đặt!';
        RETURN -1;
    END

    INSERT INTO SeatBooking (BookingID, SeatID)
    VALUES (@BookingID, @SeatID);
END;
GO

-- DỮ LIỆU MẪU
INSERT INTO Employee (EmployeeID, FullName, Password, Role) VALUES
    ('E001', 'Trịnh Trần Phương Tuấn', '001', 1),
    ('E002', 'J97', '002', 0),
    ('E003', 'Jack', '003', 0);

INSERT INTO Movie (MovieID, Title, Duration, Genre) VALUES
    ('M001', 'Kẻ Đánh Cắp Giấc Mơ', 148, 'Khoa Học Viễn Tưởng'),
    ('M002', 'Thế Thân: Dòng Chảy Của Nước', 192, 'Phiêu Lưu'),
    ('M003', 'Người Dơi', 176, 'Hành Động'),
    ('M004', 'Hố Đen Vũ Trụ', 169, 'Khoa Học Viễn Tưởng'),
    ('M005', 'Sát Thủ John Wick 4', 169, 'Hành Động');
GO

EXEC Add_Show 'S001', 'M001', '2025-02-25 19:00:00', 148, 120000;
EXEC Add_Show 'S002', 'M001', '2025-02-25 21:30:00', 148, 100000;
EXEC Add_Show 'S003', 'M002', '2025-02-26 18:45:00', 192, 150000;
-- test lỗi
-- EXEC Add_Show 'S004', 'M001', '2025-02-25 20:00:00', 148, 120000;
GO

INSERT INTO Booking (BookingID, ShowID, BookingTime) VALUES
    ('B001', 'S001', '2025-02-20 14:00:00'),
    ('B002', 'S002', '2025-02-21 10:30:00'),
    ('B003', 'S001', '2025-02-20 14:00:00');
GO

EXEC Book_Seat 'B001', 'A1';
EXEC Book_Seat 'B002', 'A1';
EXEC Book_Seat 'B002', 'A2';
-- test lỗi
-- EXEC Book_Seat 'B003', 'A1';
