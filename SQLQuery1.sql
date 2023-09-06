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
BookId int primary key,
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


alter table Books add Picture varbinary(max)
