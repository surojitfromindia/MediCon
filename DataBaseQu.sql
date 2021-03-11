create database MyDb;
use MyDb;
create table userTable
(
    username  varchar(22) primary key not null,
    password  varchar(10)             not null,
    password2 varchar(20)
);

create table manufactureDetailsTable
(
    manfName    varchar(50)  not null,
    manfAdd     varchar(100) not null,
    manfContact varchar(20)  not null,
    manfEmail   varchar(30)  not null,
    manfWebSite varchar(40),
    primary key (manfName)
);

create table medNameTable
(
    medName        varchar(50) not null,
    manfName       varchar(50) not null,
    username       varchar(50) not null,
    expiryDuration int         not null,
    price          int         not null,
    primary key (medName),
    constraint key_3 foreign key (username) references usertable (username) on UPDATE cascade,
    constraint key_4 foreign key (manfName) references manufactureDetailsTable (manfName) on UPDATE cascade
);

create table tagTable
(
    tagid int         not null primary key auto_increment,
    tag   varchar(30) not null
);


create table medicineTable
(
    medName     varchar(50)           not null,
    entryDate   date        default (current_date),
    expiryDate  date        default 0 not null,
    batchNumber int         default 0 not null,
    numOf       int         default 0 not null,
    status      varchar(16) default 'OK',
    price       int                   not null,
    constraint key_1 foreign key (medName) references medNameTable (medName) on delete cascade
);

/*This will Be Schedule table
  User can schedule medicine order here.
  this schedule result will be shown at medicine entry time
  if schedule medicine is add to inventory then this table will also be
  updated. (optional) if the User don't update this table for that value
  */
create table scheduleTable
(
    medicineName varchar(50) not null,
    scheduleDate date default (current_date),
    noOfSchedule int         not null,
    primary key (medicinename),
    constraint key_2 foreign key (medicineName) references medNameTable (medName) on delete cascade on UPDATE cascade
);

/*A insert procedure
  create a new batch every time you entry same medicine.
  */
delimiter |
create procedure medicineBatchInsert(IN med_name varchar(50), IN no_Of_Item_To_Add int, In med_price int)
begin
    select expiryDuration from medNameTable where medNameTable.medName = med_name into @expiryDuration;
    select max(batchNumber) from medicinetable where medName = med_name into @nextBatch;
    set @expiryDate = date_add(current_date(), interval @expiryDuration MONTH);
    if (@nextBatch is null) then
        set @nextBatch = 0;
    end if;
    insert into medicinetable (medName, batchNumber, expiryDate, numOf, price)
    values (med_name, @nextBatch + 1, @expiryDate, no_Of_Item_To_Add, med_price);
end |
delimiter ;


/*Manual Batch merging Procedure
  Will call itself if the entry date of same medicine is not older than 30 days from last entry
  eg:
  you entry a medicine name 'vioc' on 17-8-2020'
  and next day you want to entry the same medicine which is 18-8-2020, which is less than 30 days
  so. by calling this procedure you will not create a new batch for thar medicine
  only update number of medicine (numOf) for that batch (which is the last, you can't update the previous batch)
  */
delimiter |
create procedure medicineBatchMargeUpdateProcedure(IN med_name varchar(50), IN no_of_Item_to_Add int)
begin
    select max(entryDate) from medicinetable where medName = med_name into @currentDate;
    select max(batchNumber) from medicinetable where medName = med_name into @updateBatch;
    if datediff(@currentDate, current_date) < 30 then
        update medicinetable
        set numOf = numOf + no_of_Item_to_Add
        where medName = med_name
          and batchNumber = @updateBatch;
    end if;
end |
delimiter ;

/*Schedule Insert and Update Procedure*/
delimiter |
create procedure MakeASchedule(in Medicine_Name varchar(50), in Medicine_Quan int)
begin
    set @cAld = (select noOfSchedule from scheduletable where medicineName = Medicine_Name);
    /*if the schedule already exist just update the value */
    if @cAld > 0 and Medicine_Quan > 0 then
        update scheduletable set noOfSchedule =Medicine_Quan where medicineName = Medicine_Name;
    else
        insert into scheduletable(medicineName, noOfSchedule) values (Medicine_Name, Medicine_Quan);
    end if;
end |
delimiter ;


/*Procedure of this tables*/
/*case 1 : The User delete a schedule manually*/
delimiter |
create procedure RemoveSchedule(in MedName varchar(50))
begin
    delete from scheduletable where medicineName = MedName ;
end |
delimiter ;

/*case 2: Auto clear zero noOfSchedule Records (make this a event that will run in every 1hr)*/
delimiter |
create procedure RemoveScheduleAuto()
begin
    delete from scheduletable where noOfSchedule < 1;
end |
delimiter ;


/*A User Define Function*/
/*1 - means true*/
/*0 -means false*/
delimiter |
create function ValidDateMedicineName(med_name varchar(70)) RETURNS BOOLEAN
    deterministic
begin
    declare ANS BOOLEAN default FALSE;
    set @counts = (select count(*) from medNameTable where medName = med_name);
    if @counts = 1 then
        set ANS = TRUE;
    end if;
    RETURN (ANS);
end |
delimiter ;


/*Status Update Event*/
delimiter |
create event statusUpdateEvent
    on schedule
        every 12 hour
    do
    begin
        update medicinetable set status = 'Expired' where current_date() > expiryDate;
    end |
delimiter ;

drop event statusUpdateEvent;


/*Views*/

create or replace VIEW MedicineInventoryView as
select medNameTable.medName, ifnull(sum(medicineTable.numOf), 0) as In_Inventory
from medicinetable
         right join medNameTable on medicineTable.medName = medNameTable.medName
group by medName;



create or replace view MedicineStockStatusView2 as
select (select sum(numOf) from medicinetable)                                            as 'Total_Medicine_In_Stock',
       (select count(*) from medicinetable where status = 'Expired')                     as 'Batch_Expired',
       ifnull((select sum(numOf) from medicinetable where status = 'Expired'), 0)        as 'Medicine_Expired',
       (select sum(numOf) from medicinetable where status = 'OK')                        as 'Medicine_Valid',
       (select count(batchNumber)
        from medicinetable
        where datediff(expiryDate, current_date) < (select day(last_day(current_date)))) as 'Will_Expired_In_One_Month',
       (select count(batchNumber) from medicineTable)                                    as 'Total_Batch'
;


create or replace view FullMedicineStatusView as
select medname,
       sum(numOf)                                                    as 'stocked',
       (select count(*) from medicinetable where status = 'Expired') as 'stockedExpB',
       count(batchNumber)                                            as 'Batch'
from medicinetable
group by medname
;



create or replace view LatestPriceView AS
select a.medName, a.price, a.batchNumber
from medicinetable a
         inner join (
    select medName, max(batchNumber) batchNumber
    from medicinetable
    group by medName
) b on a.medName = b.medName and a.batchNumber = b.batchNumber;



create or replace view FullMedicineStatusViewAdvance as
select a.medname                 as "medname",
       stocked                   as "stocked",
       a.stockedExpB             as "stockedExpB",
       a.Batch                   as "Batch",
       b.price                   as "price",
       ifnull(c.noOfSchedule, 0) as "scheduled"
from FullMedicineStatusView a
         left join (LatestPriceView b) on a.medname = b.medName
         left join (scheduleTable c) on a.medname = c.medicineName
order by a.medname;



create or replace view ScheduleInfoView as
select m1.medName,
       ifnull(m2.numOf, 0) as Total_Stock,
       ifnull(m3.noOfSchedule, 0
           )               as For_Schedule
from mednametable m1
         left join medicineTable m2 on m1.medName = m2.medName
         left join scheduleTable m3 on m3.medicineName = m1.medName
group by m1.medName;
select *
from ScheduleInfoView;



/* step by step*/
/*Insert this manually*/
insert into userTable
values ('surojit', '2cehelper', '7835');
insert into userTable
values ('soumya', 'softking', '8535');
insert into userTable
values ('Apricot', '782', '7825');
/*Insert this manually*/
insert into manufactureDetailsTable
values ('Jaxan Life', 'London', '12345678', 'iokonAbc@yahoo.com', 'www.MXYIndustryIndia.com');
/*Demo Purpose*/
insert into medNameTable
values ('Trapex 2', 'Jaxan Life', 'Apricot', 24, 60);
/*Insert new record using this procedure*/
call medicineBatchInsert('Trapex 2', 10, 80);
/*update and merge*/
call medicineBatchMargeUpdateProcedure('Trapex 2', 81);
/*Make a schedule for order of this medicine*/
call MakeASchedule('Trapex 2', 10);

/*Tables*/
select *
from usertable;
select *
from manufacturedetailstable;
select *
from mednametable;
select *
from tagTable;
select *
from medicinetable;
select *
from scheduletable;

/*Views*/
select *
from MedicineStockStatusView2;
select *
from MedicineInventoryView;
select*
from FullMedicineStatusView;
select *
from LatestPriceView;
select *
from FullMedicineStatusViewAdvance;
select *
from InventoryPriceView;

/*ADD HERE AFTER THIS WILL SUPPLY AS DIFFERENT SCRIPT FILE*/


CREATE OR REPLACE VIEW overviewview AS
select (select medname
        from FullMedicineStatusViewAdvance
        where price = (select MAX(price) from FullMedicineStatusViewAdvance)
        limit 1)                                                as 'MAX_PRICE_MED',
       (select MAX(price) from FullMedicineStatusViewAdvance)   as 'MAX_PRICE',
       (select medname
        from FullMedicineStatusViewAdvance
        where price = (select MIN(price) from FullMedicineStatusViewAdvance)
        limit 1)                                                as 'MIN_PRICE_MED',
       (select MIN(price) from FullMedicineStatusViewAdvance)   as 'MIN_PRICE',

       (select medname
        from FullMedicineStatusViewAdvance
        where stocked = (select MAX(stocked) from FullMedicineStatusViewAdvance)
        limit 1)                                                as 'MAX_STOCKED_MED',
       (select MAX(stocked) from FullMedicineStatusViewAdvance) as 'MAX_STOCKED',
       (select medname
        from FullMedicineStatusViewAdvance
        where stocked = (select MIN(stocked) from FullMedicineStatusViewAdvance)
        limit 1)                                                as 'MIN_STOCKED_MED',
       (select MIN(stocked) from FullMedicineStatusViewAdvance) as 'MIN_STOCKED',

       (select medname
        from FullMedicineStatusViewAdvance
        where Batch = (select MAX(Batch) from FullMedicineStatusViewAdvance)
        limit 1)                                                as 'MAX_BATCH_MED',
       (select MAX(Batch) from FullMedicineStatusViewAdvance)   as 'MAX_BATCH',
       (select medname
        from FullMedicineStatusViewAdvance
        where Batch = (select MIN(Batch) from FullMedicineStatusViewAdvance)
        limit 1)                                                as 'MIN_BATCH_MED',
       (select MIN(Batch) from FullMedicineStatusViewAdvance)   as 'MIN_BATCH';


/*Use this to get which medicine become empty , eg. if 'tprice' is zero that means it is empty*/
create or replace view InventoryPriceView as
select a.medName, ifnull(sum(b.price), 0) * ifnull((b.numOf), 0) as 'tprice'
from mednametable a
         left join medicineTable b on a.medName = b.medName
group by a.medName;

/*Create a Contact table*/
create table onlyContactTable
(
    manufac varchar(50),
    contact varchar(12),
    constraint contact_key foreign key (manufac) REFERENCES manufactureDetailsTable (manfName) on UPDATE cascade on delete cascade
);

/*Create a email table*/
create table onlyEmailTable
(
    manufac varchar(50),
    email varchar(50),
    constraint email_key foreign key (manufac) REFERENCES manufactureDetailsTable (manfName) on UPDATE cascade on delete cascade
);



set global local_infile = true;
load data local INFILE 'D:/2.C#/ProjectIFPossible/ProjectIFPossible/bin/Debug/settings/DataOnTxtFile/MedicineInfotxt.txt' INTO TABLE manufactureDetailsTable fields enclosed by '\'' lines TERMINATED BY '\r\n';
load data local INFILE 'D:/2.C#/ProjectIFPossible/ProjectIFPossible/bin/Debug/settings/DataOnTxtFile/MedicineInfotxt.txt' INTO TABLE mednametable fields enclosed by '\'' lines TERMINATED BY '\r\n';