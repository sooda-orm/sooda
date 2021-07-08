create database SoodaConsoleApp;
go

use SoodaConsoleApp;
go

create user soodatest for login soodatest;
go


create table KeyGen
(
	key_name varchar(64) primary key,
	key_value int not null
)
go

--- Category
create table Categories
(
    id int not null,
    name nvarchar(64) not null,
    description nvarchar(max) null
);
go

alter table Categories add constraint PK_Categories primary key (id);

go

--- Customer
create table Customers
(
    id int not null,
    company_name nvarchar(64) not null,
    contact_name nvarchar(64) null,
    contact_title nvarchar(64) null,
    address nvarchar(256) null,
    city nvarchar(128) null,
    region nvarchar(128) null,
    postal_code nvarchar(16) null,
    country nvarchar(64) null,
    phone nvarchar(24) null
);
go

alter table Customers add constraint PK_Customers primary key (id);

go

--- Employee
create table Employees
(
    id int not null,
    last_name nvarchar(64) not null,
    first_name nvarchar(32) not null,
    title nvarchar(64) null,
    birth_date DateTime null,
    address nvarchar(256) null,
    city nvarchar(128) null,
    region nvarchar(128) null,
    postal_code nvarchar(16) null,
    country nvarchar(64) null,
    phone nvarchar(24) null,
    reports_to int null
);
go

alter table Employees add constraint PK_Employees primary key (id);

alter table Employees add constraint FK_Employees_reports_to foreign key (reports_to) references Employees(id);

create index IDX_Employees_reports_to on Employees(reports_to);

go

--- Order
create table Orders
(
    id int not null,
    customer_id int null,
    employee_id int null,
    order_date DateTime null,
    required_date DateTime null,
    shipped_date DateTime null,
    ship_address nvarchar(256) null,
    ship_city nvarchar(128) null,
    ship_region nvarchar(64) null,
    ship_postal_code nvarchar(10) null,
    ship_country nvarchar(128) null
);
go

alter table Orders add constraint PK_Orders primary key (id);

alter table Orders add constraint FK_Orders_customer_id foreign key (customer_id) references Customers(id);
alter table Orders add constraint FK_Orders_employee_id foreign key (employee_id) references Employees(id);

create index IDX_Orders_customer_id on Orders(customer_id);
create index IDX_Orders_employee_id on Orders(employee_id);

go

--- Product
create table Products
(
    id int not null,
    name nvarchar(128) not null,
    category_id int null,
    unit_price decimal null,
    units_in_stock int null,
    discontinued int not null
);
go

alter table Products add constraint PK_Products primary key (id);

alter table Products add constraint FK_Products_category_id foreign key (category_id) references Categories(id);

create index IDX_Products_category_id on Products(category_id);

go


--- OrderItem
create table OrderItems
(
    order_id int not null,
    product_id int not null,
    unit_price decimal not null,
    quantity int not null,
    discount decimal not null
);
go

alter table OrderItems add constraint PK_OrderItems primary key (order_id, product_id);

alter table OrderItems add constraint FK_OrderItems_order_id foreign key (order_id) references Orders(id);
alter table OrderItems add constraint FK_OrderItems_product_id foreign key (product_id) references Products(id);

create index IDX_OrderItems_order_id on OrderItems(order_id);
create index IDX_OrderItems_product_id on OrderItems(product_id);

go

alter role db_owner add member soodatest;
go

print 'Granting table permissions...'

grant select,insert,update on KeyGen to soodatest
grant select,insert,update,delete on Categories to soodatest
grant select,insert,update,delete on Customers to soodatest
grant select,insert,update,delete on Employees to soodatest
grant select,insert,update,delete on Orders to soodatest
grant select,insert,update,delete on OrderItems to soodatest
grant select,insert,update,delete on Products to soodatest

go

insert into Categories values(1, 'Ksi¹¿ka', null);
insert into Categories values(2, 'Film', null);
insert into Categories values(3, 'Gra video', null);


















