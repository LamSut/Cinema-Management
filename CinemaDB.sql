USE master;
GO

-- Close all existing connections to the database
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'CinemaDB')
BEGIN
    ALTER DATABASE CinemaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CinemaDB;
END
GO

-- Create the new database
CREATE DATABASE CinemaDB;
GO
USE CinemaDB;
GO

CREATE TABLE Movie (
    MovieID NVARCHAR(10) PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Genre NVARCHAR(50),
    Duration INT,
    Description NVARCHAR(1000),
    Director NVARCHAR(100),
    ReleaseDate DATE,
    PosterURL NVARCHAR(255), 
    Status NVARCHAR(20) DEFAULT 'Active'
);
GO

CREATE TABLE Theater (
    TheaterID NVARCHAR(10) PRIMARY KEY,
    TheaterName NVARCHAR(50) NOT NULL,
    Capacity INT NOT NULL,
    TheaterType NVARCHAR(20) DEFAULT 'Standard'
);

CREATE TABLE Show (
    ShowID NVARCHAR(10) PRIMARY KEY,
    MovieID NVARCHAR(10) NOT NULL,
    TheaterID NVARCHAR(10) NOT NULL,
    ShowTime DATETIME NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Active',
    FOREIGN KEY (MovieID) REFERENCES Movie(MovieID),
    FOREIGN KEY (TheaterID) REFERENCES Theater(TheaterID)
);

CREATE TABLE SeatCategory (
    CategoryID NVARCHAR(10) PRIMARY KEY,
    CategoryName NVARCHAR(50) NOT NULL,
    PriceMultiplier DECIMAL(3,2) DEFAULT 1.00
);

CREATE TABLE Seat (
    SeatID NVARCHAR(10) PRIMARY KEY,
    TheaterID NVARCHAR(10) NOT NULL,
    SeatRow CHAR(1) NOT NULL,
    SeatNumber INT NOT NULL,
    CategoryID NVARCHAR(10) NOT NULL,
    FOREIGN KEY (TheaterID) REFERENCES Theater(TheaterID),
    FOREIGN KEY (CategoryID) REFERENCES SeatCategory(CategoryID)
);

CREATE TABLE Discount (
    DiscountID NVARCHAR(10) PRIMARY KEY,
    DiscountName NVARCHAR(50) NOT NULL,
    DiscountPercent DECIMAL(5,2) NOT NULL,
    Description NVARCHAR(200)
);

CREATE TABLE PaymentMethod (
    PaymentMethodID NVARCHAR(10) PRIMARY KEY,
    MethodName NVARCHAR(50) NOT NULL
);

CREATE TABLE Customer (
    CustomerID NVARCHAR(10) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    RegistrationDate DATE DEFAULT GETDATE()
);

CREATE TABLE Booking (
    BookingID NVARCHAR(15) PRIMARY KEY,
    ShowID NVARCHAR(10) NOT NULL,
    CustomerID NVARCHAR(10),
    BookingTime DATETIME NOT NULL DEFAULT GETDATE(),
    DiscountID NVARCHAR(10),
    PaymentMethodID NVARCHAR(10),
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Active',
    FOREIGN KEY (ShowID) REFERENCES Show(ShowID),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID),
    FOREIGN KEY (DiscountID) REFERENCES Discount(DiscountID),
    FOREIGN KEY (PaymentMethodID) REFERENCES PaymentMethod(PaymentMethodID)
);

CREATE TABLE SeatBooking (
    BookingID NVARCHAR(15) NOT NULL,
    SeatID NVARCHAR(10) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (BookingID, SeatID),
    FOREIGN KEY (BookingID) REFERENCES Booking(BookingID),
    FOREIGN KEY (SeatID) REFERENCES Seat(SeatID)
);

CREATE TABLE Employee (
    EmployeeID NVARCHAR(50) PRIMARY KEY,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Role BIT NOT NULL -- 0: nhân viên, 1: admin
);

INSERT INTO Movie (MovieID, Title, Genre, Duration, Description, Director, ReleaseDate, PosterURL, Status)
VALUES 
('M001', N'Inception', N'Science Fiction, Action', 148, N'A thief who steals corporate secrets through dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.', N'Christopher Nolan', '2010-07-16', 'https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg', 'Active'),

('M002', N'Avatar: The Way of Water', N'Adventure, Sci-Fi', 192, N'Jake Sully lives with his newfound family formed on the extrasolar moon Pandora. Once a familiar threat returns to finish what was previously started, Jake must work with Neytiri and the army of the Na''vi race to protect their home.', N'James Cameron', '2022-12-16', 'https://image.tmdb.org/t/p/w500/t6HIqrRAclMCA60NsSmeqe9RmNV.jpg', 'Active'),

('M003', N'The Batman', N'Action, Crime, Drama', 176, N'When the Riddler, a sadistic serial killer, begins murdering key political figures in Gotham, Batman is forced to investigate the city''s hidden corruption and question his family''s involvement.', N'Matt Reeves', '2022-03-04', 'https://image.tmdb.org/t/p/w500/74xTEgt7R36Fpooo50r9T25onhq.jpg', 'Active'),

('M004', N'Interstellar', N'Adventure, Drama, Sci-Fi', 169, N'A team of explorers travel through a wormhole in space in an attempt to ensure humanity''s survival.', N'Christopher Nolan', '2014-11-07', 'https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg', 'Active'),

('M005', N'John Wick: Chapter 4', N'Action, Crime, Thriller', 169, N'John Wick uncovers a path to defeating The High Table. But before he can earn his freedom, he must face off against a new enemy with powerful alliances across the globe and forces that turn old friends into foes.', N'Chad Stahelski', '2023-03-24', 'https://image.tmdb.org/t/p/w500/vZloFAK7NmvMGKE7VkF5UHaz0I.jpg', 'Active'),

('M006', N'Everything Everywhere All at Once', N'Adventure, Comedy, Sci-Fi', 139, N'An aging Chinese immigrant is swept up in an insane adventure, where she alone can save the world by exploring other universes connecting with the lives she could have led.', N'Daniel Kwan, Daniel Scheinert', '2022-03-25', 'https://image.tmdb.org/t/p/w500/w3LxiVYdWWRvEVdn5RYq6jIqkb1.jpg', 'Active'),

('M007', N'Parasite', N'Drama, Thriller', 132, N'Greed and class discrimination threaten the newly formed symbiotic relationship between the wealthy Park family and the destitute Kim clan.', N'Bong Joon Ho', '2019-05-30', 'https://image.tmdb.org/t/p/w500/7IiTTgloJzvGI1TAYymCfbfl3vT.jpg', 'Active'),

('M008', N'Top Gun: Maverick', N'Action, Drama', 130, N'After more than thirty years of service as one of the Navy''s top aviators, Pete Mitchell is where he belongs, pushing the envelope as a courageous test pilot and dodging the advancement in rank that would ground him.', N'Joseph Kosinski', '2022-05-27', 'https://image.tmdb.org/t/p/w500/62HCnUTziyWcpDaBO2i1DX17ljH.jpg', 'Active'),

('M009', N'Spider-Man: No Way Home', N'Action, Adventure, Fantasy', 148, N'With Spider-Man''s identity now revealed, Peter asks Doctor Strange for help. When a spell goes wrong, dangerous foes from other worlds start to appear, forcing Peter to discover what it truly means to be Spider-Man.', N'Jon Watts', '2021-12-17', 'https://image.tmdb.org/t/p/w500/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg', 'Active'),

('M010', N'The Shawshank Redemption', N'Drama', 142, N'Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.', N'Frank Darabont', '1994-10-14', 'https://image.tmdb.org/t/p/w500/q6y0Go1tsGEsmtFryDOJo3dEmqu.jpg', 'Active');


-- Thêm dữ liệu mẫu cho Theater
INSERT INTO Theater (TheaterID, TheaterName, Capacity, TheaterType)
VALUES 
('T001', N'Theater 1', 120, 'Standard'),
('T002', N'Theater 2', 100, 'Standard'),
('T003', N'Theater 3', 80, 'VIP'),
('T004', N'Theater 4', 150, 'IMAX'),
('T005', N'Theater 5', 100, 'Standard'),
('T006', N'Theater 6', 80, '4DX'),
('T007', N'Theater 7', 60, 'Premium'),
('T008', N'Theater 8', 100, 'Standard'),
('T009', N'Theater 9', 80, 'Standard'),
('T010', N'Theater 10', 200, 'IMAX');

-- Thêm dữ liệu mẫu cho SeatCategory
INSERT INTO SeatCategory (CategoryID, CategoryName, PriceMultiplier)
VALUES 
('SC001', N'Standard', 1.00),
('SC002', N'Premium', 1.20),
('SC003', N'VIP', 1.50),
('SC004', N'Couple', 2.00),
('SC005', N'Recliner', 1.75),
('SC006', N'Accessible', 1.00),
('SC007', N'Gold Class', 2.50),
('SC008', N'Platinum', 3.00),
('SC009', N'Elite', 2.25),
('SC010', N'Economy', 0.90);


-- Thêm dữ liệu mẫu cho Discount
INSERT INTO Discount (DiscountID, DiscountName, DiscountPercent, Description)
VALUES 
('D001', N'No Discount', 0.00, N'No discount applied'),
('D002', N'Student', 10.00, N'Discount for students with valid ID'),
('D003', N'Senior', 15.00, N'Discount for senior citizens (age 60+)'),
('D004', N'Member', 20.00, N'Discount for cinema club members'),
('D005', N'Holiday Special', 5.00, N'Special holiday season discount'),
('D006', N'Birthday', 25.00, N'Birthday discount (valid during birthday month)'),
('D007', N'Morning Show', 10.00, N'Discount for morning shows before 12 PM'),
('D008', N'Group Booking', 15.00, N'Discount for booking 10+ tickets'),
('D009', N'Weekday Special', 10.00, N'Discount for Monday-Thursday shows'),
('D010', N'First Show', 10.00, N'Discount for first show of new releases');

-- Thêm dữ liệu mẫu cho PaymentMethod
INSERT INTO PaymentMethod (PaymentMethodID, MethodName)
VALUES 
('PM001', N'Credit Card'),
('PM002', N'Debit Card'),
('PM003', N'Cash'),
('PM004', N'Mobile Payment'),
('PM005', N'PayPal'),
('PM006', N'Cinema Points');


INSERT INTO Employee (EmployeeID, Password, FullName, Role)
VALUES
('E001', 'password123', 'Nguyen Van A', 0),
('E002', 'password456', 'Tran Thi B', 1),
('E003', 'password789', 'Le Minh C', 0),
('E004', 'admin123', 'Pham Thanh D', 1),
('E005', 'password000', 'Hoang Thi E', 0);


-- Thêm dữ liệu mẫu cho Customer
INSERT INTO Customer (CustomerID, Name, Phone, Email, RegistrationDate)
VALUES 
('C001', N'Nguyen Van A', '0901234567', 'nguyenvana@email.com', '2022-01-15'),
('C002', N'Tran Thi B', '0912345678', 'tranthib@email.com', '2022-02-20'),
('C003', N'Le Van C', '0923456789', 'levanc@email.com', '2022-03-10'),
('C004', N'Pham Thi D', '0934567890', 'phamthid@email.com', '2022-04-05'),
('C005', N'Hoang Van E', '0945678901', 'hoangvane@email.com', '2022-05-18'),
('C006', N'Nguyen Thi F', '0956789012', 'nguyenthif@email.com', '2022-06-22'),
('C007', N'Truc Lam', '0967890123', 'truclam@email.com', '2022-07-30'),
('C008', N'Le Anh Quan', '0978901234', 'leaquan@email.com', '2022-08-12'),
('C009', N'Le Xuan Thanh', '0989012345', 'thanh@email.com', '2022-09-05'),
('C010', N'Tran Trung Nguyen', '0990123456', 'nguyen@email.com', '2022-10-25');


-- Hàng A (Premium)
INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('A01', 'T001', 'A', 1, 'SC002'),
('A02', 'T001', 'A', 2, 'SC002'),
('A03', 'T001', 'A', 3, 'SC002'),
('A04', 'T001', 'A', 4, 'SC002'),
('A05', 'T001', 'A', 5, 'SC002'),
('A06', 'T001', 'A', 6, 'SC002'),
('A07', 'T001', 'A', 7, 'SC002'),
('A08', 'T001', 'A', 8, 'SC002'),
('A09', 'T001', 'A', 9, 'SC002'),
('A10', 'T001', 'A', 10, 'SC002'),
('A11', 'T001', 'A', 11, 'SC002'),
('A12', 'T001', 'A', 12, 'SC002'),
('A13', 'T001', 'A', 13, 'SC002'),
('A14', 'T001', 'A', 14, 'SC002'),
('A15', 'T001', 'A', 15, 'SC002'),
('A16', 'T001', 'A', 16, 'SC002');

-- Hàng B (Premium)
INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('B01', 'T001', 'B', 1, 'SC002'),
('B02', 'T001', 'B', 2, 'SC002'),
('B03', 'T001', 'B', 3, 'SC002'),
('B04', 'T001', 'B', 4, 'SC002'),
('B05', 'T001', 'B', 5, 'SC002'),
('B06', 'T001', 'B', 6, 'SC002'),
('B07', 'T001', 'B', 7, 'SC002'),
('B08', 'T001', 'B', 8, 'SC002'),
('B09', 'T001', 'B', 9, 'SC002'),
('B10', 'T001', 'B', 10, 'SC002'),
('B11', 'T001', 'B', 11, 'SC002'),
('B12', 'T001', 'B', 12, 'SC002'),
('B13', 'T001', 'B', 13, 'SC002'),
('B14', 'T001', 'B', 14, 'SC002'),
('B15', 'T001', 'B', 15, 'SC002'),
('B16', 'T001', 'B', 16, 'SC002');

INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('C01', 'T001', 'C', 1, 'SC002'),
('C02', 'T001', 'C', 2, 'SC002'),
('C03', 'T001', 'C', 3, 'SC002'),
('C04', 'T001', 'C', 4, 'SC002'),
('C05', 'T001', 'C', 5, 'SC002'),
('C06', 'T001', 'C', 6, 'SC002'),
('C07', 'T001', 'C', 7, 'SC002'),
('C08', 'T001', 'C', 8, 'SC002'),
('C09', 'T001', 'C', 9, 'SC002'),
('C10', 'T001', 'C', 10, 'SC002'),
('C11', 'T001', 'C', 11, 'SC002'),
('C12', 'T001', 'C', 12, 'SC002'),
('C13', 'T001', 'C', 13, 'SC002'),
('C14', 'T001', 'C', 14, 'SC002'),
('C15', 'T001', 'C', 15, 'SC002'),
('C16', 'T001', 'C', 16, 'SC002');

INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('D01', 'T001', 'D', 1, 'SC001'),
('D02', 'T001', 'D', 2, 'SC001'),
('D03', 'T001', 'D', 3, 'SC001'),
('D04', 'T001', 'D', 4, 'SC001'),
('D05', 'T001', 'D', 5, 'SC001'),
('D06', 'T001', 'D', 6, 'SC001'),
('D07', 'T001', 'D', 7, 'SC001'),
('D08', 'T001', 'D', 8, 'SC001'),
('D09', 'T001', 'D', 9, 'SC001'),
('D10', 'T001', 'D', 10, 'SC001'),
('D11', 'T001', 'D', 11, 'SC001'),
('D12', 'T001', 'D', 12, 'SC001'),
('D13', 'T001', 'D', 13, 'SC001'),
('D14', 'T001', 'D', 14, 'SC001'),
('D15', 'T001', 'D', 15, 'SC001'),
('D16', 'T001', 'D', 16, 'SC001');

INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('E01', 'T001', 'E', 1, 'SC001'),
('E02', 'T001', 'E', 2, 'SC001'),
('E03', 'T001', 'E', 3, 'SC001'),
('E04', 'T001', 'E', 4, 'SC001'),
('E05', 'T001', 'E', 5, 'SC001'),
('E06', 'T001', 'E', 6, 'SC001'),
('E07', 'T001', 'E', 7, 'SC001'),
('E08', 'T001', 'E', 8, 'SC001'),
('E09', 'T001', 'E', 9, 'SC001'),
('E10', 'T001', 'E', 10, 'SC001'),
('E11', 'T001', 'E', 11, 'SC001'),
('E12', 'T001', 'E', 12, 'SC001'),
('E13', 'T001', 'E', 13, 'SC001'),
('E14', 'T001', 'E', 14, 'SC001'),
('E15', 'T001', 'E', 15, 'SC001'),
('E16', 'T001', 'E', 16, 'SC001');

INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('F01', 'T001', 'F', 1, 'SC001'),
('F02', 'T001', 'F', 2, 'SC001'),
('F03', 'T001', 'F', 3, 'SC001'),
('F04', 'T001', 'F', 4, 'SC001'),
('F05', 'T001', 'F', 5, 'SC001'),
('F06', 'T001', 'F', 6, 'SC001'),
('F07', 'T001', 'F', 7, 'SC001'),
('F08', 'T001', 'F', 8, 'SC001'),
('F09', 'T001', 'F', 9, 'SC001'),
('F10', 'T001', 'F', 10, 'SC001'),
('F11', 'T001', 'F', 11, 'SC001'),
('F12', 'T001', 'F', 12, 'SC001'),
('F13', 'T001', 'F', 13, 'SC001'),
('F14', 'T001', 'F', 14, 'SC001'),
('F15', 'T001', 'F', 15, 'SC001'),
('F16', 'T001', 'F', 16, 'SC001');

INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('G01', 'T001', 'G', 1, 'SC001'),
('G02', 'T001', 'G', 2, 'SC001'),
('G03', 'T001', 'G', 3, 'SC001'),
('G04', 'T001', 'G', 4, 'SC001'),
('G05', 'T001', 'G', 5, 'SC001'),
('G06', 'T001', 'G', 6, 'SC001'),
('G07', 'T001', 'G', 7, 'SC001'),
('G08', 'T001', 'G', 8, 'SC001'),
('G09', 'T001', 'G', 9, 'SC001'),
('G10', 'T001', 'G', 10, 'SC001'),
('G11', 'T001', 'G', 11, 'SC001'),
('G12', 'T001', 'G', 12, 'SC001'),
('G13', 'T001', 'G', 13, 'SC001'),
('G14', 'T001', 'G', 14, 'SC001'),
('G15', 'T001', 'G', 15, 'SC001'),
('G16', 'T001', 'G', 16, 'SC001');

INSERT INTO Seat (SeatID, TheaterID, SeatRow, SeatNumber, CategoryID)
VALUES 
('H01', 'T001', 'H', 1, 'SC001'),
('H02', 'T001', 'H', 2, 'SC001'),
('H03', 'T001', 'H', 3, 'SC001'),
('H04', 'T001', 'H', 4, 'SC001'),
('H05', 'T001', 'H', 5, 'SC001'),
('H06', 'T001', 'H', 6, 'SC001'),
('H07', 'T001', 'H', 7, 'SC001'),
('H08', 'T001', 'H', 8, 'SC001'),
('H09', 'T001', 'H', 9, 'SC001'),
('H10', 'T001', 'H', 10, 'SC001'),
('H11', 'T001', 'H', 11, 'SC001'),
('H12', 'T001', 'H', 12, 'SC001'),
('H13', 'T001', 'H', 13, 'SC001'),
('H14', 'T001', 'H', 14, 'SC001'),
('H15', 'T001', 'H', 15, 'SC001'),
('H16', 'T001', 'H', 16, 'SC001');


-- Thêm dữ liệu mẫu cho Show
INSERT INTO Show (ShowID, MovieID, TheaterID, ShowTime, Price, Status)
VALUES 
('S001', 'M001', 'T001', '2025-03-10 10:00:00', 10.00, 'Active'),
('S002', 'M001', 'T001', '2025-03-10 14:30:00', 12.50, 'Active'),
('S003', 'M001', 'T001', '2025-03-10 19:00:00', 15.00, 'Active'),
('S004', 'M002', 'T002', '2025-03-10 11:00:00', 10.00, 'Active'),
('S005', 'M002', 'T002', '2025-03-10 15:30:00', 12.50, 'Active'),
('S006', 'M002', 'T002', '2025-03-10 20:00:00', 15.00, 'Active'),
('S007', 'M003', 'T003', '2025-03-10 12:00:00', 15.00, 'Active'),
('S008', 'M003', 'T003', '2025-03-10 16:30:00', 17.50, 'Active'),
('S009', 'M003', 'T003', '2025-03-10 21:00:00', 20.00, 'Active'),
('S010', 'M004', 'T004', '2025-03-10 13:00:00', 18.00, 'Active'),
('S011', 'M004', 'T004', '2025-03-10 17:30:00', 20.00, 'Active'),
('S012', 'M004', 'T004', '2025-03-10 22:00:00', 22.00, 'Active'),
('S013', 'M005', 'T005', '2025-03-11 09:30:00', 10.00, 'Active'),
('S014', 'M005', 'T005', '2025-03-11 14:00:00', 12.50, 'Active'),
('S015', 'M005', 'T005', '2025-03-11 18:30:00', 15.00, 'Active'),
('S016', 'M006', 'T006', '2025-03-11 10:30:00', 15.00, 'Active'),
('S017', 'M006', 'T006', '2025-03-11 15:00:00', 17.50, 'Active'),
('S018', 'M006', 'T006', '2025-03-11 19:30:00', 20.00, 'Active'),
('S019', 'M007', 'T007', '2025-03-11 11:30:00', 15.00, 'Active'),
('S020', 'M007', 'T007', '2025-03-11 16:00:00', 17.50, 'Active');


INSERT INTO Booking (BookingID, ShowID, CustomerID, BookingTime, DiscountID, PaymentMethodID, TotalAmount, Status)
VALUES 
('B20250310001', 'S001', 'C001', '2025-03-09 14:25:00', 'D001', 'PM001', 20.00, 'Active'),
('B20250310002', 'S002', 'C002', '2025-03-09 16:10:00', 'D002', 'PM003', 22.50, 'Active'),
('B20250310003', 'S003', 'C003', '2025-03-09 17:45:00', 'D001', 'PM004', 30.00, 'Active'),
('B20250310004', 'S004', 'C004', '2025-03-09 18:30:00', 'D003', 'PM001', 17.00, 'Active'),
('B20250310005', 'S005', 'C005', '2025-03-09 19:15:00', 'D004', 'PM001', 30.00, 'Active'),
('B20250310006', 'S006', 'C006', '2025-03-09 20:05:00', 'D001', 'PM003', 15.00, 'Active'),
('B20250310007', 'S007', 'C007', '2025-03-09 21:00:00', 'D002', 'PM004', 27.00, 'Active'),
('B20250310008', 'S008', 'C008', '2025-03-09 21:45:00', 'D001', 'PM001', 35.00, 'Active'),
('B20250310009', 'S009', 'C009', '2025-03-09 22:30:00', 'D003', 'PM001', 51.00, 'Active'),
('B20250310010', 'S010', 'C010', '2025-03-09 23:15:00', 'D004', 'PM001', 43.20, 'Active');
GO

-- Khởi tạo dữ liệu mẫu cho bảng SeatBooking
INSERT INTO SeatBooking (BookingID, SeatID, Price)
VALUES 
('B20250310001', 'A03', 10.00),
('B20250310001', 'A04', 10.00),
('B20250310002', 'B05', 12.50),
('B20250310002', 'B06', 12.50),
('B20250310003', 'C07', 15.00),
('B20250310003', 'C08', 15.00),
('B20250310004', 'D09', 10.00),
('B20250310004', 'D10', 10.00),
('B20250310005', 'E03', 12.50),
('B20250310005', 'E04', 12.50),
('B20250310005', 'E05', 12.50),
('B20250310006', 'F11', 15.00),
('B20250310007', 'A12', 15.00),
('B20250310007', 'A13', 15.00),
('B20250310008', 'B14', 17.50),
('B20250310008', 'B15', 17.50),
('B20250310009', 'C04', 20.00),
('B20250310009', 'C05', 20.00),
('B20250310009', 'C06', 20.00),
('B20250310010', 'D03', 18.00),
('B20250310010', 'D04', 18.00),
('B20250310010', 'D05', 18.00);
GO

-- Thêm một vài booking cho các suất chiếu khác
INSERT INTO Booking (BookingID, ShowID, CustomerID, BookingTime, DiscountID, PaymentMethodID, TotalAmount, Status)
VALUES 
('B20250311001', 'S012', 'C001', '2025-03-10 10:20:00', 'D002', 'PM001', 39.60, 'Active'),
('B20250311002', 'S014', 'C003', '2025-03-10 12:35:00', 'D001', 'PM003', 25.00, 'Active'),
('B20250311003', 'S015', 'C005', '2025-03-10 14:50:00', 'D004', 'PM001', 24.00, 'Active'),
('B20250311004', 'S016', 'C007', '2025-03-10 16:15:00', 'D003', 'PM004', 25.50, 'Active'),
('B20250311005', 'S018', 'C009', '2025-03-10 18:40:00', 'D001', 'PM001', 40.00, 'Active');
GO

-- Thêm ghế cho các booking mới
INSERT INTO SeatBooking (BookingID, SeatID, Price)
VALUES 
('B20250311001', 'G01', 22.00),
('B20250311001', 'G02', 22.00),
('B20250311002', 'F15', 12.50),
('B20250311002', 'F16', 12.50),
('B20250311003', 'B01', 15.00),
('B20250311003', 'B02', 15.00),
('B20250311004', 'C09', 15.00),
('B20250311004', 'C10', 15.00),
('B20250311005', 'D11', 20.00),
('B20250311005', 'D12', 20.00);
GO

-- Cập nhật các giá trị tổng
UPDATE Booking
SET TotalAmount = (
   SELECT SUM(Price)
   FROM SeatBooking
   WHERE SeatBooking.BookingID = Booking.BookingID
)
WHERE BookingID IN (
   SELECT DISTINCT BookingID 
   FROM SeatBooking
);
GO

-- Thêm một số đơn đặt vé đã hủy/hoàn thành
INSERT INTO Booking (BookingID, ShowID, CustomerID, BookingTime, DiscountID, PaymentMethodID, TotalAmount, Status)
VALUES 
('B20250312001', 'S001', 'C002', '2025-03-08 09:15:00', 'D001', 'PM003', 10.00, 'Cancelled'),
('B20250312002', 'S003', 'C004', '2025-03-08 11:30:00', 'D003', 'PM001', 25.50, 'Completed'),
('B20250312003', 'S005', 'C006', '2025-03-08 13:45:00', 'D002', 'PM004', 33.75, 'Completed');
GO

-- Thêm ghế cho các đơn đặt vé đã hủy/hoàn thành
INSERT INTO SeatBooking (BookingID, SeatID, Price)
VALUES 
('B20250312001', 'A01', 10.00),
('B20250312002', 'E13', 15.00),
('B20250312002', 'E14', 15.00),
('B20250312003', 'B08', 12.50),
('B20250312003', 'B09', 12.50),
('B20250312003', 'B10', 12.50);
GO

-- Cập nhật lại tổng số tiền cho các đơn đã thêm cuối cùng
UPDATE Booking
SET TotalAmount = (
   SELECT SUM(Price)
   FROM SeatBooking
   WHERE SeatBooking.BookingID = Booking.BookingID
)
WHERE BookingID IN ('B20250312001', 'B20250312002', 'B20250312003');
GO


SELECT 'Total Bookings: ' + CAST(COUNT(*) AS NVARCHAR) AS BookingCount FROM Booking;

SELECT 'Total Seats Booked: ' + CAST(COUNT(*) AS NVARCHAR) AS SeatCount FROM SeatBooking;

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'Book_Seat')
    DROP PROCEDURE Book_Seat
GO

-- Create stored procedure for booking seats
CREATE PROCEDURE Book_Seat
    @BookingID NVARCHAR(15),
    @SeatID NVARCHAR(10),
    @Price DECIMAL(10,2)
AS
BEGIN
    DECLARE @ShowID NVARCHAR(10)
    SELECT @ShowID = ShowID FROM Booking WHERE BookingID = @BookingID
    
    IF EXISTS (
        SELECT 1 FROM SeatBooking sb
        JOIN Booking b ON sb.BookingID = b.BookingID
        WHERE b.ShowID = @ShowID AND sb.SeatID = @SeatID AND b.Status = 'Active'
    )
    BEGIN
        RETURN -1
    END
    ELSE
    BEGIN
        INSERT INTO SeatBooking (BookingID, SeatID, Price)
        VALUES (@BookingID, @SeatID, @Price)
        RETURN 0
    END
END
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_ShowDetails')
    DROP VIEW vw_ShowDetails
GO

-- Create View to display show details with movie and theater information
CREATE VIEW vw_ShowDetails
AS
SELECT 
    s.ShowID,
    m.Title AS MovieTitle,
    m.Genre,
    m.Duration,
    m.PosterURL,
    t.TheaterName,
    t.TheaterType,
    s.ShowTime,
    s.Price,
    s.Status
FROM Show s
JOIN Movie m ON s.MovieID = m.MovieID
JOIN Theater t ON s.TheaterID = t.TheaterID;
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_BookingDetails')
    DROP VIEW vw_BookingDetails
GO

-- Create View to display booking details
CREATE VIEW vw_BookingDetails
AS
SELECT 
    b.BookingID,
    b.BookingTime,
    c.Name AS CustomerName,
    c.Phone AS CustomerPhone,
    c.Email AS CustomerEmail,
    s.ShowID,
    m.Title AS MovieTitle,
    t.TheaterName,
    s.ShowTime,
    d.DiscountName,
    d.DiscountPercent,
    p.MethodName AS PaymentMethod,
    b.TotalAmount,
    b.Status AS BookingStatus,
    COUNT(sb.SeatID) AS NumberOfSeats
FROM Booking b
JOIN Show s ON b.ShowID = s.ShowID
JOIN Movie m ON s.MovieID = m.MovieID
JOIN Theater t ON s.TheaterID = t.TheaterID
LEFT JOIN Customer c ON b.CustomerID = c.CustomerID
LEFT JOIN Discount d ON b.DiscountID = d.DiscountID
LEFT JOIN PaymentMethod p ON b.PaymentMethodID = p.PaymentMethodID
JOIN SeatBooking sb ON b.BookingID = sb.BookingID
GROUP BY 
    b.BookingID, b.BookingTime, c.Name, c.Phone, c.Email,
    s.ShowID, m.Title, t.TheaterName, s.ShowTime,
    d.DiscountName, d.DiscountPercent, p.MethodName,
    b.TotalAmount, b.Status;
GO

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_BookedSeats')
    DROP VIEW vw_BookedSeats
GO

-- Create View to display detailed information for booked seats
CREATE VIEW vw_BookedSeats
AS
SELECT 
    b.BookingID,
    sb.SeatID,
    s.SeatRow,
    s.SeatNumber,
    sc.CategoryName,
    sb.Price,
    sh.ShowID,
    sh.ShowTime,
    m.Title AS MovieTitle,
    t.TheaterName
FROM SeatBooking sb
JOIN Booking b ON sb.BookingID = b.BookingID
JOIN Seat s ON sb.SeatID = s.SeatID
JOIN SeatCategory sc ON s.CategoryID = sc.CategoryID
JOIN Show sh ON b.ShowID = sh.ShowID
JOIN Movie m ON sh.MovieID = m.MovieID
JOIN Theater t ON sh.TheaterID = t.TheaterID;
GO