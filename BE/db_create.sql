use master;
create database PSYCare;
go

use PSYCare
CREATE TABLE Psychologist
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    password VARCHAR(128) NOT NULL,
    salt VARCHAR(64) NOT NULL,
);

CREATE TABLE Patient
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    password VARCHAR(128) NOT NULL,
    salt VARCHAR(64) NOT NULL,
);

create table Planificator
(
	Psychologist int foreign key references Psychologist(id),
	Patient int foreign key references Patient(id),
	appointment_date datetime not null,
	fee decimal(6,2) not null default 0.00
	PRIMARY KEY (Psychologist, Patient, appointment_date)
);

create table Mood
(
	Patient int foreign key references Patient(id),
	completion_date datetime default getdate(),
	score tinyint default 1,
	primary key(Patient, completion_date)
);
go

USE PSYCare;
GO

CREATE TABLE VaultIdentifier
(
    token UNIQUEIDENTIFIER PRIMARY KEY,           -- the token stored in main tables
    encrypted_value VARBINARY(MAX) NOT NULL,     -- encrypted PNC / doctor code
    data_type VARCHAR(30) NOT NULL,              -- 'PNC' or 'DOCTOR_CODE'
    retention_until DATE NOT NULL,               -- automatic deletion enforcement
    created_at DATETIME2 DEFAULT SYSUTCDATETIME()-- timestamp for audit
);
GO

use PSYCare
-- Convert column type
ALTER TABLE Patient
    ALTER COLUMN IdentifierToken UNIQUEIDENTIFIER NULL;

ALTER TABLE Psychologist
    ALTER COLUMN IdentifierToken UNIQUEIDENTIFIER NULL;

-- If existing rows have string GUIDs, convert them
UPDATE Patient
SET IdentifierToken = CAST(IdentifierToken AS UNIQUEIDENTIFIER)
WHERE IdentifierToken IS NOT NULL;

UPDATE Psychologist
SET IdentifierToken = CAST(IdentifierToken AS UNIQUEIDENTIFIER)
WHERE IdentifierToken IS NOT NULL;
go