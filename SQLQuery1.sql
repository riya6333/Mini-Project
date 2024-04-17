create database RailwayReservation_Project
use RailwayReservation_Project

----------------------------------------------------table for Trains------------------------------------------------------
create table Trains (Train_No int primary key,
					 Train_Name varchar(100) not null,
					 Source_Station varchar(50)not null,
					 Destination_Station varchar(50)not null,
					 Status varchar(20) check(Status IN ('Active', 'Inactive')) default 'Active')

----------------------------------------------------table for Classes------------------------------------------------------
create table Class_Detail(Class_ID int  primary key identity,
						  Train_No int,
						  Class_Name Varchar(50),
						  Total_Seats int NOT NULL,
						  Available_Seats int NOT NULL,
						  Fare decimal(10,2)
						  foreign key (Train_No) references Trains(Train_No))
select*from Class_Detail

----------------------------------------------------table for Booking ------------------------------------------------------
create table Bookings (Booking_ID int primary key identity,
					   Passenger_Name varchar(50),
					   Train_No int NOT NULl,
					   Class_Name varchar(50) NOT NULL,
					   Travel_Date date NOT NULL,
					   Tickets_Count int NOT NULL,
					   Total_Amount decimal(10, 2) ,
					   Status varchar(20) check (Status IN ('Active', 'Inactive')) default 'Active'
					   foreign key (Train_No) references Trains(Train_No))
select* from Bookings

----------------------------------------------------table for Cancellation ------------------------------------------------------
create table Cancellation (Cancellation_ID  int primary key identity,
						   Booking_ID int,
						   Passenger_Name varchar(30),
						   Train_No int,
						   Class_Name varchar(30) ,
						   Tickets_Count int,
						   DateOf_Cancel date default GETDATE(),
						   foreign key (Booking_ID) references bookings(Booking_ID),
						   foreign key (Train_No) references Trains(Train_No))
select* from Cancellation





----------------------------------Creating  Procedure-------------------------------------------------------------

-------------------------addTrain procedure--------------------------------------------------
create or alter procedure AddTrain
    @Train_No int,
    @Train_Name varchar(100),
    @Source_Station varchar(100),
	@Destination_Station varchar(100)
as
begin
insert into Trains(Train_No, Train_Name, Source_Station, Destination_Station)
values (@Train_No, @Train_Name, @Source_Station, @Destination_Station )
end

Exec AddTrain @Train_No=18701, @Train_Name=Superfast, @Source_Station=Delhi, @Destination_Station=Lucknow
select* from Trains


-------------------------procedure for class------------------------------------------
create or alter procedure AddClass_Detail 
@Train_No int,
@Class_Name Varchar(50),
@Total_Seats int ,
@Available_Seats int,
@Fare decimal(10,2)  
as
begin
insert into Class_Detail(Train_No,Class_Name, Total_Seats,Available_Seats,Fare)
values (@Train_No,@Class_Name, @Total_Seats,@Available_Seats,@Fare);
end


-------------------------------------------procedure for deplaying trains-----------------------------------------
create or alter proc Display_Train
as
begin
select*from Trains
end 
exec Display_Train


--------------------------------------------procedure for booking tickets---------------------------------
create or alter procedure BookTrain_Ticket
    @Train_No INT,
    @Passenger_Name VARCHAR(50),
    @Class_Name VARCHAR(50),
    @Travel_Date DATETIME,
    @Tickets_Count INT
as
begin
------------ Calculate booking amount-------------------------
declare @Ticket_Fare decimal(10, 2)
declare @Total_Amount decimal(10, 2)

select @Ticket_Fare = Fare from Class_Detail
where Train_No = @Train_No AND Class_Name = @Class_Name;
set @Total_Amount = @Ticket_Fare * @Tickets_Count;

------ Check if the train and class are available----------------------
declare @Available_Seats int;
select @Available_Seats = Available_Seats from Class_Detail
where Train_No = @Train_No AND Class_Name = @Class_Name;

if @Available_Seats >= @Tickets_Count
begin
------------- Update available seats--------------------------------------------
update Class_Detail
set Available_Seats = Available_Seats - @Tickets_Count
where Train_No = @Train_No AND Class_Name = @Class_Name;
------------ Insert booking details-----------------------------------------------
insert into Bookings(Train_No, Passenger_Name, Class_Name,Travel_Date ,Tickets_Count, Total_Amount, Status)
values ( @Train_No, @Passenger_Name, @Class_Name, @Travel_Date, @Tickets_Count, @Total_Amount, 'Active');
end
end

Select*from Bookings
EXEC BookTrain_Ticket 
    @Passenger_Name = 'Diya',
    @Train_No = 12456,
    @Class_Name = '1AC',
    @Travel_Date = '2024-09-29',
    @Tickets_Count = 2



------------------procedure for Cancellation-------------------------------------------
create or alter procedure Cancel_Ticket
@Booking_ID int,
@Passenger_Name varchar(30),
@Train_No int,
@Class_Name varchar(30),
@Tickets_Count int
as
begin
-- Update available seats in CLASS_DETAILS----------------------------------
update Class_Detail
set AVAILABLE_SEATS = AVAILABLE_SEATS + @Tickets_Count
where TRAIN_NO = @Train_No;
---- Insert cancellation details into Cancellation table------------------
insert into Cancellation (Booking_ID, Passenger_Name, Train_No, Class_Name, Tickets_Count)
values (@Booking_ID, @Passenger_Name, @Train_No, @Class_Name, @Tickets_Count)
end

EXEC Cancel_Ticket 
    @Booking_ID = 1,
    @Passenger_Name =Siya,
    @Train_No = 43212,
    @Class_Name = Sleeper,
    @Tickets_Count = 2;



------------------------------procedure for updating train status-------------------------------------------------
create or alter procedure TrainStatus
    @Train_No INT
as
begin

update Trains
set Status = 'Active'
where Train_No = @Train_No AND Status = 'Inactive';

update Trains
set Status = 'Inactive'
where Train_No = @Train_No AND Status = 'Active';

-- Select and return the updated status of the train
select Status from Trains
where Train_No = @Train_No;
end
