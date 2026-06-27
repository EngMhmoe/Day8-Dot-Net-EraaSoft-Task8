merge امر
بيخليك تعمل دمج وتعمل في استعلام واحدinsert,delete,updat
وتستخدمه لما يكون عندك جدولين وعايز جدولين وعايزتحدث البيانات وتضيفها
المثال
MERGE Employees_Old AS Target
USING Employees_New AS Source
ON Target.EmployeeID = Source.EmployeeID

WHEN MATCHED THEN
    UPDATE SET Target.Salary = Source.Salary, Target.Name = Source.Name

WHEN NOT MATCHED BY TARGET THEN
    INSERT (EmployeeID, Name, Salary)
    VALUES (Source.EmployeeID, Source.Name, Source.Salary)

WHEN NOT MATCHED BY SOURCE THEN
    DELETE;
--------------------------------------------------------------------------------------------------------------------------------------------------
    الفرق بين delet , truncat
1    delete form users 
  
      بيمسح صف ومينفعش مع where
    ينفع ترجع لبيانات
    rollback

----------------
2 truncate table
بيمسح الجدول كله مينفعش لرجع للبيانات بسهوله

    
