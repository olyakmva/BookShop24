CREATE LOGIN opy WITH PASSWORD = 'pRl24kb';
CREATE USER opy FOR LOGIN opy;
EXEC sp_addrolemember 'db_owner', 'opy'