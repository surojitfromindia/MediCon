create database MyDb2;
use MyDb2;
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
    expiryDuration int         not null null,
    primary key (medName),
    foreign key (username) references userTable (username),
    foreign key (manfName) references manufactureDetailsTable (manfName)
);



create table medicineTable
(
    medName     varchar(50)           not null,
    entryDate   date        default (current_date),
    expiryDate  date        default 0 not null,
    batchNumber int         default 0 not null,
    numOf       int         default 0 not null,
    status      varchar(16) default 'OK',
    constraint key_1 foreign key (medName) references medNameTable (medName) on delete cascade
);

/*A insert procedure
  create a new batch every time you entry same medicine.
  */
delimiter | 
create procedure medicineBatchInsert(IN med_name varchar(50), IN no_Of_Item_To_Add int)
begin
    select expiryDuration from medNameTable where medNameTable.medName = med_name into @expiryDuration;
    select max(batchNumber) from medicinetable where medName = med_name into @nextBatch;
    set @expiryDate = date_add(current_date(), interval @expiryDuration MONTH);
    if (@nextBatch is null) then
        set @nextBatch = 0;
    end if;
    insert into medicinetable (medName, batchNumber, expiryDate, numOf)
    values (med_name, @nextBatch + 1, @expiryDate, no_Of_Item_To_Add);
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
    if datediff(@currentDate, current_date) < 30  then
        update medicinetable
        set numOf = numOf + no_of_Item_to_Add
        where medName = med_name and batchNumber = @updateBatch;
    end if;
end |
delimiter ;


/*A User Define Function*/
/*1 - means true*/
/*0 -means false*/
delimiter |
create function ValidDateMedicineName(med_name varchar(20)) RETURNS BOOLEAN
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
        every 1 DAY
    do
    begin
        update medicinetable set status = 'Expired' where current_date() > expiryDate;
    end |
delimiter ;







/* step by step*/
insert into userTable
values ('soumya', '1234', '12345');
insert into manufactureDetailsTable
values ('Abc', 'Kolkata', '12345678', 'iokonAbc@yahoo.com', 'www.IkonIndustryIndia.com');
insert into medNameTable
values ('Med1', 'Abc', 'soumya', 24);


/*Insert new record using this procedure*/
call medicineBatchInsert('Med1', 250);
/*update and merge*/
call medicineBatchMargeUpdateProcedure('Med1', 81);

/*Views*/

create or replace VIEW MedicineInventoryView as
select medNameTable.medName, ifnull(sum(medicineTable.numOf), 0) as In_Inventory
from medicinetable
         right join medNameTable on medicineTable.medName = medNameTable.medName
group by medName;



create or replace view MedicineStockStatusView2 as
select (select sum(numOf) from medicinetable)                                            as 'Total_Medicine_In_Stock',
       (select count(*) from medicinetable where status = 'Expired')                     as 'Batch_Expired',
       (select sum(numOf) from medicinetable where status = 'Expired')                   as 'Medicine_Expired',
       (select sum(numOf) from medicinetable where status = 'OK')                        as 'Medicine_Valid',
       (select count(batchNumber)
        from medicinetable
        where datediff(expiryDate, current_date) < (select day(last_day(current_date)))) as 'Will_Expired_In_One_Month'
;


/*Insert this manually*/
select * from usertable;
/*Insert this manually*/
select * from manufacturedetailstable;

select * from mednametable;
select * from medicinetable;

select * from MedicineStockStatusView2;
select * from MedicineInventoryView;