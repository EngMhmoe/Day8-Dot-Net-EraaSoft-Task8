-- =============================================
-- PHARMACY DATABASE - FIXED & BEST PRACTICES
-- Author: Mohamed Alwany
-- =============================================


-- =============================================
-- SECTION 1: CREATE TABLES
-- =============================================

-- 1. PHARMA_COMPANY
CREATE TABLE PHARMA_COMPANY (
    Company_ID   INT PRIMARY KEY,
    Company_Name VARCHAR(100) UNIQUE,   -- UNIQUE عشان مايتكررش
    Address      VARCHAR(255),
    Phone        VARCHAR(20)
);


-- 2. DOCTOR
CREATE TABLE DOCTOR (
    Doctor_ID          INT PRIMARY KEY,
    Name               VARCHAR(100),
    Email              VARCHAR(100) UNIQUE,
    Phone              VARCHAR(20),
    Specialty          VARCHAR(100),
    Years_Of_Experience INT CHECK (Years_Of_Experience >= 0)
);


-- 3. PATIENT
CREATE TABLE PATIENT (
    UR_Number          VARCHAR(20)  PRIMARY KEY,
    Name               VARCHAR(100),
    Address            VARCHAR(255),
    Age                INT   CHECK (Age > 0),
    Email              VARCHAR(100) UNIQUE,
    Phone              VARCHAR(20) ,
    Medicare_Card_Number VARCHAR(20) UNIQUE,
    Primary_Doctor_ID  INT,
    FOREIGN KEY (Primary_Doctor_ID) REFERENCES DOCTOR(Doctor_ID)
);


-- 4. DRUG
CREATE TABLE DRUG (
    Trade_Name   VARCHAR(100),
    Strength     VARCHAR(50) ,
    Company_ID   INT,
    PRIMARY KEY (Trade_Name, Strength),
    FOREIGN KEY (Company_ID) REFERENCES PHARMA_COMPANY(Company_ID)
        ON DELETE CASCADE
);


-- 5. PRESCRIPTION
CREATE TABLE PRESCRIPTION (
    Prescription_ID   INT IDENTITY(1,1) PRIMARY KEY,  -- PK بسيط وواضح
    UR_Number         VARCHAR(20) ,
    Doctor_ID         INT     ,
    Trade_Name        VARCHAR(100),
    Strength          VARCHAR(50),
    Prescription_Date DATE ,
    Quantity          INT CHECK (Quantity > 0),

    FOREIGN KEY (UR_Number)              REFERENCES PATIENT(UR_Number),
    FOREIGN KEY (Doctor_ID)              REFERENCES DOCTOR(Doctor_ID),
    FOREIGN KEY (Trade_Name, Strength)   REFERENCES DRUG(Trade_Name, Strength)
);




-- 1. SELECT
SELECT * FROM DOCTOR;

-- 2. ORDER BY
SELECT *
FROM PATIENT
ORDER BY Age ASC;

-- 3. OFFSET FETCH - Pagination
SELECT *
FROM PATIENT
ORDER BY UR_Number
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

-- 4. 
SELECT TOP 5 *
FROM DOCTOR;

-- 5
SELECT DISTINCT Address
FROM PATIENT;

-- 6. 
SELECT *
FROM PATIENT
WHERE Age = 25;

-- 7. 
SELECT *
FROM PATIENT
WHERE Email IS NULL;

-- 8. AND
SELECT *
FROM DOCTOR
WHERE Years_Of_Experience > 5
  AND Specialty = 'Cardiology';

-- 9. 
SELECT *
FROM DOCTOR
WHERE Specialty IN ('Cardiology', 'Dermatology');

-- 10. BETWEEN 
SELECT *
FROM PATIENT
WHERE Age BETWEEN 18 AND 30;

-- 11. LIKE 
SELECT *
FROM DOCTOR
WHERE Name LIKE 'Dr.%';

-- 12. Aliases 
SELECT Name  AS DoctorName,
       Email AS DoctorEmail
FROM DOCTOR;

-- 13. JOIN
SELECT P.Name AS PatientName,
       Pr.*
FROM PRESCRIPTION Pr
JOIN PATIENT P ON Pr.UR_Number = P.UR_Number;

-- 14. GROUP BY
SELECT Address    AS City,
       COUNT(*)   AS PatientCount
FROM PATIENT
GROUP BY Address;

-- 15. HAVING
SELECT Address    AS City,
       COUNT(*)   AS PatientCount
FROM PATIENT
GROUP BY Address
HAVING COUNT(*) > 3;

-- 16. EXISTS
SELECT *
FROM PATIENT P
WHERE EXISTS (
    SELECT 1
    FROM PRESCRIPTION Pr
    WHERE Pr.UR_Number = P.UR_Number
);

-- 17. UNION
SELECT Name, 'Doctor'  AS Type FROM DOCTOR
UNION
SELECT Name, 'Patient' AS Type FROM PATIENT;


-- =============================================
-- SECTION 3: INSERT
-- =============================================

-- 18. INSERT Doctor
INSERT INTO DOCTOR (Name, Email, Phone, Specialty, Years_Of_Experience)
VALUES ('Dr. Temp', 'temp@mail.com', '01200000000', 'Neurology', 3)

-- 19. INSERT Patient 
INSERT INTO PATIENT (UR_Number, Name, Address, Age, Email, Phone, Primary_Doctor_ID)
VALUES ('UR002', 'Temp Patient', 'Alexandria', 30, 'temppatient@mail.com', '01000000000', 1);


-- =============================================
-- SECTION 4: UPDATE
-- =============================================

-- 20. UPDATE 
UPDATE DOCTOR
SET Phone = '01099999999'
WHERE Doctor_ID = 1;

-- 21. UPDATE JOIN
UPDATE P
SET P.Address = 'Giza'
FROM PATIENT P
JOIN PRESCRIPTION Pr ON P.UR_Number = Pr.UR_Number;


-- =============================================
-- SECTION 5: DELETE
-- =============================================

-- 22. DELETE
DELETE FROM PATIENT
WHERE UR_Number = 'UR002';


-- =============================================
-- SECTION 6: TRANSACTION
-- =============================================

-- 23. TRANSACTION
BEGIN TRANSACTION;

BEGIN TRY
    INSERT INTO DOCTOR (Name, Email, Phone, Specialty, Years_Of_Experience)
    VALUES ('Dr. New', 'new@mail.com', '01100000000', 'Pediatrics', 5);

    DECLARE @NewDoctorID INT = SCOPE_IDENTITY();

    INSERT INTO PATIENT (UR_Number, Name, Address, Age, Email, Phone, Primary_Doctor_ID)
    VALUES ('UR003', 'New Patient', 'Cairo', 15, 'newpatient@mail.com', '01200000000', @NewDoctorID);

    COMMIT TRANSACTION;
    PRINT 'Transaction completed successfully';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error occurred - Transaction rolled back ❌';
    PRINT ERROR_MESSAGE();
END CATCH;
