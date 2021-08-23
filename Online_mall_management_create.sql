create database DBOnlineMall
go
use DBOnlineMall
go

-- tables
-- Table: Admin
CREATE TABLE Admin (
    Id int identity not null,
    UserName varchar(100)  NOT NULL unique,
	DisplayName varchar(100)  NOT NULL,
    Password varchar(max)  NOT NULL,
    CONSTRAINT Admin_pk PRIMARY KEY  (Id)
);

-- Table: AdminRoles
CREATE TABLE AdminRoles (
    Role_id int  NOT NULL,
    Admin_id int  NOT NULL,
    CONSTRAINT AdminRoles_pk PRIMARY KEY  (Role_id,Admin_id)
);

-- Table: Area
CREATE TABLE Area (
    AreaId int identity  NOT NULL,
    AreaName varchar(100)  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Area_pk PRIMARY KEY  (AreaId)
);

-- Table: Banner
CREATE TABLE Banner (
    Id int identity NOT NULL,
    Image varchar(200) NOT NULL,
    link varchar(max),
    description varchar(max),
    OrderBy int NOT NULL,
    Sratus bit  default(1),
    page varchar(100) NOT NULL,
	CreatedDate datetime default getDate(),
	ModifiedDate datetime,
    CONSTRAINT Banner_pk PRIMARY KEY  (Id)
);

-- Table: Blog
CREATE TABLE Blog (
    BlogId int identity NOT NULL,
    Description varchar(max) NOT NULL,
    BlogTime date default getdate(),
    Category_blog_id int  NOT NULL,
    Title varchar(100)  NOT NULL,
    Content text  NOT NULL,
	Status bit default(1),
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
	CreatedDate datetime default getDate(),
	ModifiedDate datetime,
    CONSTRAINT Blog_pk PRIMARY KEY  (BlogId)
);

-- Table: Category_Movie
CREATE TABLE Category_Movie (
    Cate_id int identity NOT NULL,
    Name varchar(50)  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
    CONSTRAINT Category_Movie_pk PRIMARY KEY  (Cate_id)
);

-- Table: Category_blog
CREATE TABLE Category_blog (
    Cate_blog_id int identity  NOT NULL,
    name varchar(255)  NOT NULL,
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Category_blog_pk PRIMARY KEY  (Cate_blog_id)
);

-- Table: Config
CREATE TABLE Config (
    Id int identity NOT NULL,
    title varchar(100),
    value varchar(100),
    Status bit default(1),
    url text,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Config_pk PRIMARY KEY  (Id)
);

-- Table: Customer
CREATE TABLE Customer (
    CustomerId int identity NOT NULL,
    FullName varchar(100)  NOT NULL,
    Phone varchar(20)  NOT NULL,
    Email varchar(100)  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Customer_pk PRIMARY KEY  (CustomerId)
);

-- Table: Events
CREATE TABLE Events (
    Event_id int identity NOT NULL,
    Shop_ShoId int  NOT NULL,
    Date date NOT NULL,
    StartTime time  NOT NULL,
    Price float NOT NULL,
    Title varchar(max)  NOT NULL,
    Descriptions text  NOT NULL,
    EndTime time  NOT NULL,
    Image varchar(max)  NOT NULL,
    Address varchar(100),
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
	Status bit default(1)
    CONSTRAINT Events_pk PRIMARY KEY  (Event_id)
);

-- Table: Feedback
CREATE TABLE Feedback (
    Id int identity NOT NULL,
    Email varchar(100)  NOT NULL,
    Content varchar(200)  NOT NULL,
    FirstName varchar(100)  NOT NULL,
    LastName varchar(100)  NOT NULL,
    Phone varchar(20)  NOT NULL,
    CONSTRAINT Feedback_pk PRIMARY KEY  (Id)
);

-- Table: Movie
CREATE TABLE Movie (
    IdMovie int identity NOT NULL,
    MoviveName varchar(max) NOT NULL,
    Image nvarchar(max)  NOT NULL,
    Duration int  NOT NULL,
    Actors nvarchar(max)  NOT NULL,
    AgeRestriction int  NOT NULL,
    Description varchar(max)  NOT NULL,
    Language varchar(20)  NOT NULL,
    ReleaseDate date  NOT NULL,
    Country varchar(100)  NOT NULL,
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
	Status bit default(1)
    CONSTRAINT Movie_pk PRIMARY KEY  (IdMovie)
);

-- Table: Movie_type
CREATE TABLE Movie_type (
    cate_movie_id int NOT NULL,
    movie_id int  NOT NULL,
    CONSTRAINT Movie_type_pk PRIMARY KEY  (cate_movie_id,movie_id)
);

-- Table: Order
CREATE TABLE "Order" (
    OrderId int identity NOT NULL,
    CustomerId int  NOT NULL,
    Discount int  NOT NULL,
    Total float NOT NULL,
    Screening_Id int  NOT NULL,
    OrderDate datetime default getdate(),
    NumberOfSeats int  NOT NULL,
	Status bit default (0),
    CONSTRAINT Order_pk PRIMARY KEY  (OrderId)
);

-- Table: Roles
CREATE TABLE Roles (
    Role_id int identity NOT NULL,
    Role_name varchar(255)  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Roles_pk PRIMARY KEY  (Role_id)
);

-- Table: Room
CREATE TABLE Room (
    IdRoom int identity NOT NULL,
    RoomName varchar(100)  NOT NULL,
    TotalSeat int  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Room_pk PRIMARY KEY  (IdRoom)
);

-- Table: Sales
CREATE TABLE Sales (
    Sale_id int identity NOT NULL,
    Shop_ShoId int  NOT NULL,
    StartDate datetime  NOT NULL,
    EndDate datetime  NOT NULL,
    Sale int  NOT NULL,
	Status bit default(1),
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Sales_pk PRIMARY KEY  (Sale_id)
);

-- Table: Screening
CREATE TABLE Screening (
    Id int identity NOT NULL,
    Room_IdRoom int  NOT NULL,
    Movie_Id int  NOT NULL,
    ScreeningDate date  NOT NULL,
    StartTime time  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Screening_pk PRIMARY KEY  (Id)
);

-- Table: Screening_seat
CREATE TABLE Screening_seat (
    ShowSeat_id int  NOT NULL,
    Seat_IdSeat int  NOT NULL,
    Screening_Id int  NOT NULL,
    Order_OrderId int  NOT NULL,
    Status bit default(0),
    Price float  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Screening_seat_pk PRIMARY KEY  (ShowSeat_id)
);

-- Table: Seat
CREATE TABLE Seat (
    IdSeat int identity NOT NULL,
    SeatName varchar(100)  NOT NULL,
    IdRoom int  NOT NULL,
    type int  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Seat_pk PRIMARY KEY  (IdSeat)
);

-- Table: Service
CREATE TABLE Service (
    ServiceId int identity NOT NULL,
    ServiceName varchar(100)  NOT NULL,
    Service_area int  NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Service_pk PRIMARY KEY  (ServiceId)
);

-- Table: Shop
CREATE TABLE Shop (
    ShoId int identity NOT NULL,
    ShopName varchar(225)  NOT NULL,
    Photo varchar(200),
    ServiceId int  NOT NULL,
    Description varchar(500),
    Address varchar(100),
    Phone varchar(20),
    Status bit default(1),
    Email varchar(200),
    Url_web varchar(100),
    Logo varchar(200),
	slug varchar(100),
	Meta_title varchar(max),
	Meta_keyword varchar(max),
	Meta_description varchar(max),
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
    CONSTRAINT Shop_pk PRIMARY KEY  (ShoId)
);

-- Table: Shop_Product
CREATE TABLE Shop_Product (
    Pro_Id int identity NOT NULL,
    Pro_name nvarchar(255) NOT NULL,
    Images text NOT NULL,
    Shop_ShoId int NOT NULL,
	CreatedDate datetime default getdate(),
	ModifiedDate datetime,
	Status bit default(1)
    CONSTRAINT Shop_Product_pk PRIMARY KEY  (Pro_Id)
);

-- foreign keys
-- Reference: AdminRoles_Admin (table: AdminRoles)
ALTER TABLE AdminRoles ADD CONSTRAINT AdminRoles_Admin
    FOREIGN KEY (Admin_id)
    REFERENCES Admin (Id);

-- Reference: AdminRoles_Roles (table: AdminRoles)
ALTER TABLE AdminRoles ADD CONSTRAINT AdminRoles_Roles
    FOREIGN KEY (Role_id)
    REFERENCES Roles (Role_id);

-- Reference: Blog_Category_blog (table: Blog)
ALTER TABLE Blog ADD CONSTRAINT Blog_Category_blog
    FOREIGN KEY (Category_blog_id)
    REFERENCES Category_blog (Cate_blog_id);

-- Reference: Events_Shop (table: Events)
ALTER TABLE Events ADD CONSTRAINT Events_Shop
    FOREIGN KEY (Shop_ShoId)
    REFERENCES Shop (ShoId);

-- Reference: Movie_type_Category_Movie (table: Movie_type)
ALTER TABLE Movie_type ADD CONSTRAINT Movie_type_Category_Movie
    FOREIGN KEY (cate_movie_id)
    REFERENCES Category_Movie (Cate_id);

-- Reference: Movie_type_Movie (table: Movie_type)
ALTER TABLE Movie_type ADD CONSTRAINT Movie_type_Movie
    FOREIGN KEY (movie_id)
    REFERENCES Movie (IdMovie);

-- Reference: Order_Customer (table: Order)
ALTER TABLE "Order" ADD CONSTRAINT Order_Customer
    FOREIGN KEY (CustomerId)
    REFERENCES Customer (CustomerId);

-- Reference: Order_Screening (table: Order)
ALTER TABLE "Order" ADD CONSTRAINT Order_Screening
    FOREIGN KEY (Screening_Id)
    REFERENCES Screening (Id);

-- Reference: Sales_Shop (table: Sales)
ALTER TABLE Sales ADD CONSTRAINT Sales_Shop
    FOREIGN KEY (Shop_ShoId)
    REFERENCES Shop (ShoId);

-- Reference: Screening_Movie (table: Screening)
ALTER TABLE Screening ADD CONSTRAINT Screening_Movie
    FOREIGN KEY (Movie_Id)
    REFERENCES Movie (IdMovie);

-- Reference: Screening_Room (table: Screening)
ALTER TABLE Screening ADD CONSTRAINT Screening_Room
    FOREIGN KEY (Room_IdRoom)
    REFERENCES Room (IdRoom);

-- Reference: Screening_seat_Order (table: Screening_seat)
ALTER TABLE Screening_seat ADD CONSTRAINT Screening_seat_Order
    FOREIGN KEY (Order_OrderId)
    REFERENCES "Order" (OrderId);

-- Reference: Screening_seat_Screening (table: Screening_seat)
ALTER TABLE Screening_seat ADD CONSTRAINT Screening_seat_Screening
    FOREIGN KEY (Screening_Id)
    REFERENCES Screening (Id);

-- Reference: Screening_seat_Seat (table: Screening_seat)
ALTER TABLE Screening_seat ADD CONSTRAINT Screening_seat_Seat
    FOREIGN KEY (Seat_IdSeat)
    REFERENCES Seat (IdSeat);

-- Reference: Seat_Room (table: Seat)
ALTER TABLE Seat ADD CONSTRAINT Seat_Room
    FOREIGN KEY (IdRoom)
    REFERENCES Room (IdRoom);

-- Reference: Service_Area (table: Service)
ALTER TABLE Service ADD CONSTRAINT Service_Area
    FOREIGN KEY (Service_area)
    REFERENCES Area (AreaId);

-- Reference: Shop_Product_Shop (table: Shop_Product)
ALTER TABLE Shop_Product ADD CONSTRAINT Shop_Product_Shop
    FOREIGN KEY (Shop_ShoId)
    REFERENCES Shop (ShoId);

-- Reference: Shop_Service (table: Shop)
ALTER TABLE Shop ADD CONSTRAINT Shop_Service
    FOREIGN KEY (ServiceId)
    REFERENCES Service (ServiceId);

-- End of file.

