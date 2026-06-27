
-- 1. PHARMA_COMPANY
CREATE TABLE PHARMA_COMPANY (
    Company_Name VARCHAR(100) PRIMARY KEY,
    Address VARCHAR(255) NOT NULL,
    Phone VARCHAR(20) NOT NULL
);


-- 2.  DOCTOR
CREATE TABLE DOCTOR (
    Doctor_ID VARCHAR(20) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Specialty VARCHAR(100) NOT NULL,
    Years_Of_Experience INT NOT NULL CHECK (Years_Of_Experience >= 0)
);


-- 3.  PATIENT
CREATE TABLE PATIENT (
    UR_Number VARCHAR(20) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Address VARCHAR(255) NOT NULL,
    Age INT NOT NULL CHECK (Age > 0),
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Medicare_Card_Number VARCHAR(20) UNIQUE NULL, -- اختياري زي التاسك
    Primary_Doctor_ID VARCHAR(20) NOT NULL,
    FOREIGN KEY (Primary_Doctor_ID) REFERENCES DOCTOR(Doctor_ID)
);


-- 4.  DRUG - PK  + Cascade Delete
CREATE TABLE DRUG (
    Trade_Name VARCHAR(100) NOT NULL,
    Strength VARCHAR(50) NOT NULL,
    Company_Name VARCHAR(100) NOT NULL,
    PRIMARY KEY (Trade_Name, Strength), 
    FOREIGN KEY (Company_Name) REFERENCES PHARMA_COMPANY(Company_Name)
        ON DELETE CASCADE -- 
);


-- 5.   PRESCRIPTION  M:N:M
CREATE TABLE PRESCRIPTION (
    UR_Number VARCHAR(20) NOT NULL,
    Doctor_ID VARCHAR(20) NOT NULL,
    Trade_Name VARCHAR(100) NOT NULL,
    Strength VARCHAR(50) NOT NULL,
    Prescription_Date DATE NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    PRIMARY KEY (UR_Number, Doctor_ID, Trade_Name, Strength, Prescription_Date), -- PK 
    
    FOREIGN KEY (UR_Number) REFERENCES PATIENT(UR_Number),
    FOREIGN KEY (Doctor_ID) REFERENCES DOCTOR(Doctor_ID),
    FOREIGN KEY (Trade_Name, Strength) REFERENCES DRUG(Trade_Name, Strength) -- FK 
);
------------------------------------------------------
    ph1--m drug
    doctor1---m pathent
    pathint1--m perscripthion
    doctor1---m perscripthion
    drug1--m perscriphion
-------------------------------------------------------
    

----. 1-SELECT 
SELECT * FROM Doctor;

---. 2-ORDER BY 
SELECT * 
FROM Patient ORDER BY Age ASC;

----. 3-OFFSET FETCH 
SELECT * 
FROM Patient ORDER BY UR_Number OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

-----. 4-SELECT TOP 
SELECT TOP 5
* FROM Doctor;

----. 5-SELECT DISTINCT 
SELECT DISTINCT
Address FROM Patient;

----. 6-WHERE 
SELECT *
FROM Patient WHERE Age = 25;

----. 7-NULL 
SELECT *
FROM Patient WHERE Email IS NULL;

----. 8-AND 
SELECT *
FROM Doctor WHERE YearsOfExperience > 5 AND Specialty = 'Cardiology';

-- 9-IN 
SELECT *
FROM Doctor WHERE Specialty IN ('Cardiology', 'Dermatology');

---. 10-BETWen
SELECT *
FROM Patient WHERE Age BETWEEN 18 AND 30;

--. 11-LIKE 
SELECT *
FROM Doctor WHERE Name LIKE 'Dr.%';

--. 12-Aliases 
SELECT Name
AS DoctorName, Email AS DoctorEmail FROM Doctor;

-- . 13-JOIN 
SELECT P.Name AS PatientName, 
Pr.* FROM Prescription Pr JOIN Patient P ON Pr.UR_Number = P.UR_Number;

--. 14-GROUP BY 
SELECT Address AS City
, COUNT(*) AS PatientCount FROM Patient GROUP BY address;

--. 15-HAVING 
SELECT Address AS City
, COUNT(*) AS PatientCount FROM Patient GROUP BY address HAVING COUNT(*) > 3;

--. 16-EXISTS 
SELECT * FROM Patient 
P WHERE EXISTS (SELECT 1 FROM Prescription Pr WHERE Pr.UR_Number = P.UR_Number);

-- . 17-UNION 
SELECT Name
, 'Doctor' AS Type FROM Doctor UNION SELECT Name, 'Patient' AS Type FROM Patient;

--. 18-INSERT Doctor 
INSERT INTO Doctor
(DoctorID, Name, Email, Phone, Specialty, YearsOfExperience) VALUES (2, 'Dr. Temp', 'temp@mail.com', '012', 'Neurology', 3);

--. 19-INSERT Patient 
INSERT INTO Patient 
(UR_Number, Name, Age, Address, Email, Phone) VALUES ('UR2', 'Temp Patient', 30, 'Alex', 'temp@mail.com', '012');

--. 20-UPDATE 
UPDATE Doctor
SET Phone = '099';

-- . 21-UPDATE JOIN 
UPDATE P SET P.Address
= 'Giza' FROM Patient P JOIN Prescription Pr ON P.UR_Number = Pr.UR_Number;

-- . 22-DELETE 
DELETE FROM
Patient;

-- 23-TRANSACTION 
BEGIN TRANSACTION;

INSERT INTO Doctor (DoctorID, Name, Email, Specialty) VALUES (3, 'Dr. New', 'new@mail.com', 'Pediatrics');

INSERT INTO Patient (UR_Number, Name, Age, PrimaryDoctorID) VALUES ('UR3', 'New Patient', 15, 3);

COMMIT TRANSACTION;
