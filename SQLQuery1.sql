create database Library_Management

use Library_Management

create table Roles
(
RoleId int primary key,
RoleName varchar(15)
)

insert into Roles values (1,'Librarian'),(2,'User')

create table Users
(
UserId int primary key identity(1,1),
Username varchar(25),
Email varchar(35),
Password varchar(20),
LastLoginDate datetime,
RoleId int references Roles(RoleId) default 2
)

Insert into Users values('Pradeep Kumar','pradeepkumar@gmail.com','pradeepK@1998','2022-09-04',1),
('Sarah Naren','saraa12@gmail.com','saraN@12','2022-09-04',1)

select * from Roles
select * from Users

create table Book_Genre
(
GenreId int primary key identity(1,1),
Genre varchar(30)
)

insert into Book_Genre values('Fiction'),('Novel'),('Science Fiction'),('Mystery'),('Non-Fiction'),('Thriller'),('Literary Fiction'),
('Fantasy'),('Short Story'),('Biography'),('Autobiography'),('Poetry'),('Crime'),('Romance'),('Essay'),('Humor'),('Horror'),
('Adventure'),('Contemporary Literature'),('History'),('Cookbook'),('Fairy tale'),('Social Science'),('Spirtuality')

select*from Book_Genre

create table Books
(
BookId int primary key identity(1,1),
BookName varchar(40),
Author varchar(35),
GenreId int references Book_Genre(GenreId)
)

create table Book_Taken
(
TakeId int primary key identity(1,1),
UserId int references Users(UserId),
Username varchar(25),
BookId int references Books(BookId),
BookName varchar(40),
TakenDate datetime,
ReturnDate datetime
)

create table Fine
(
FineId int primary key identity(1,1),
TakeId int references Book_Taken(TakeId),
UserId int references Users(UserId),
Username varchar(25),
BookId int references Books(BookId),
BookName varchar(40),
ExceededDays int,
FineAmount int
)

select*from Books
select* from Book_Taken
Select * from Fine

alter Procedure [dbo].[Validate_User]
@Email nvarchar(25),
@Password nvarchar(20)
as begin
set nocount on
declare @UserId int,@LastLoginDate datetime,@RoleId int
select @UserId = UserId,@LastLoginDate = LastLoginDate,@RoleId = RoleId from Users where Email = @Email and [Password] = @Password
if @UserId is not null
begin
update Users set LastLoginDate = GETDATE() where UserId = @UserId
select @UserId[UserId],(select RoleName from Roles where RoleId = @RoleId)[Roles]
end
else
begin
select -1[UserId],''[Roles]
end
end

alter table Book_Taken add Picture varbinary(max)

alter table Book_Genre add Image varbinary(max)

update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\fiction.jpg',Single_Blob) as img where GenreId = 1
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\novel.jpg',Single_Blob) as img where GenreId = 2
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\science fiction.jpg',Single_Blob) as img where GenreId = 3
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\mystery.png',Single_Blob) as img where GenreId = 4
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\Non-Fiction.png',Single_Blob) as img where GenreId = 5
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\thriller.png',Single_Blob) as img where GenreId = 6
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\literary.png',Single_Blob) as img where GenreId = 7
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\fantasy.png',Single_Blob) as img where GenreId = 8
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\short.png',Single_Blob) as img where GenreId = 9
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\biography.png',Single_Blob) as img where GenreId = 10
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\autobio.png',Single_Blob) as img where GenreId = 11
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\poetry.png',Single_Blob) as img where GenreId = 12
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\crime.png',Single_Blob) as img where GenreId = 13
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\romance.png',Single_Blob) as img where GenreId = 14
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\essay.png',Single_Blob) as img where GenreId = 15
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\humor.png',Single_Blob) as img where GenreId = 16
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\horror.png',Single_Blob) as img where GenreId = 17
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\adventure.png',Single_Blob) as img where GenreId = 18
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\contem.png',Single_Blob) as img where GenreId = 19
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\history.png',Single_Blob) as img where GenreId = 20
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\cook.png',Single_Blob) as img where GenreId = 21
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\fairy.png',Single_Blob) as img where GenreId = 22
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\social.jpg',Single_Blob) as img where GenreId = 23
update Book_Genre set Image = BulkColumn from OpenRowSet(Bulk 'E:\genres\spirtual.png',Single_Blob) as img where GenreId = 24

alter table Fine drop column Status
alter table Fine add IsPaid bit default 0;


select*from Books
select* from Book_Taken
Select * from Fine

alter PROCEDURE [dbo].[GenerateFine] 
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Fine
    SET ExceededDays = DATEDIFF(DAY, bt.ReturnDate, GETDATE()),
        FineAmount = DATEDIFF(DAY, bt.ReturnDate, GETDATE()) * 5
    FROM Fine AS f
    JOIN Book_Taken AS bt ON f.TakeId = bt.TakeId
    WHERE DATEDIFF(DAY, bt.ReturnDate, GETDATE()) > 0;

    INSERT INTO Fine (TakeId, UserId, Username, BookId, BookName, Email, ExceededDays, FineAmount)
    SELECT bt.TakeId, bt.UserId, bt.Username, bt.BookId, bt.BookName, bt.Email,
           DATEDIFF(DAY, bt.ReturnDate, GETDATE()) AS ExceededDays,
           DATEDIFF(DAY, bt.ReturnDate, GETDATE()) * 5 AS FineAmount
    FROM Book_Taken AS bt
    WHERE DATEDIFF(DAY, bt.ReturnDate, GETDATE()) > 0 AND NOT EXISTS (
        SELECT 1
        FROM Fine AS f
        WHERE f.UserId = bt.UserId
          AND f.BookId = bt.BookId
      );
END;


alter table Book_Taken add IsReturned bit default 0;

alter table Books add Available int