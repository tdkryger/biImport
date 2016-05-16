use cluster_b;

DROP TABLE IF EXISTS bi_TotalSales;

DROP TABLE IF EXISTS bi_ProductCategories;
DROP TABLE IF EXISTS bi_Suppliers;
DROP TABLE IF EXISTS bi_Customers;
DROP TABLE IF EXISTS bi_Products;
DROP TABLE IF EXISTS bi_ZipCodes;

drop procedure if exists bi_updateFact_TotalSales;

Create table bi_ProductCategories
(
	id INT UNSIGNED NOT NULL,
    productCategory VARCHAR(50) NOT NULL,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(50) NOT NULL,
    
    PRIMARY KEY(id)
);

create table bi_Suppliers
(
	id INT UNSIGNED NOT NULL ,
    name VARCHAR(50) not null,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(50) NOT NULL,
    
    primary key(id)
);

create table bi_Customers
(
	id INT UNSIGNED NOT NULL ,
    name VARCHAR(50) not null,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(50) NOT NULL,
    
    primary key(id)
);


create table bi_Products
(
	id INT UNSIGNED NOT NULL ,
    name VARCHAR(50) not null,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(50) NOT NULL,
    
    primary key(id)
);

create table bi_ZipCodes
(
	id INT UNSIGNED NOT NULL ,
    zipCode varchar(10) not null,
    city varchar(75) not null,
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(50) NOT NULL,
    
    primary key(id)
);

create table bi_TotalSales
(
	orderId INT unsigned not null,
	zipId INT unsigned not null,
    categoryId INT unsigned not null,
    customerId INT unsigned not null,
    productId INT unsigned not null,
	supplierId INT unsigned not null,
    
    sales decimal(10,2) not null,
    units INT not null,
    
    createDate DATETIME NOT NULL,
    createdBy VARCHAR(50) NOT NULL,
    
    foreign key (zipId) references bi_ZipCodes(id),
    foreign key (categoryId) references bi_ProductCategories(id),
    foreign key (customerId) references bi_Customers(id),
    foreign key (productId) references bi_Products(id),
    foreign key (supplierId) references bi_Suppliers(id),
    
    index (zipId),
    index (categoryId),
    index (customerId),
    index (productId),
    index (supplierId),
    
    primary key(orderId, zipId, categoryId, customerId, productId, supplierId)
);



