@echo off
echo ========================================
echo Dog Walking Manager - Database Reset
echo ========================================
echo.

echo Stopping LocalDB instance...
sqllocaldb stop MSSQLLocalDB 2>NUL

echo Deleting any orphaned database files...
del "%USERPROFILE%\DogWalkingDb.mdf" 2>NUL >NUL
del "%USERPROFILE%\DogWalkingDb_log.ldf" 2>NUL >NUL

echo Deleting LocalDB instance...
sqllocaldb delete MSSQLLocalDB 2>NUL

echo Creating new LocalDB instance...
sqllocaldb create MSSQLLocalDB

echo Starting LocalDB instance...
sqllocaldb start MSSQLLocalDB

echo.
echo Applying EF Core migrations...
cd DogWalkingApp.Data
dotnet ef database update
if %ERRORLEVEL% neq 0 (
    echo Migration failed! Please check for errors.
    pause
    exit /b 1
)

echo.
echo Populating database with sample data...
sqlcmd -S "(localdb)\mssqllocaldb" -d "DogWalkingDb" -i "../SQL/SampleData.sql"
if %ERRORLEVEL% neq 0 (
    echo Sample data insertion failed! Please check for errors.
    pause
    exit /b 1
)

echo.
echo ========================================
echo Database reset complete!
echo ========================================
echo Default login: admin / admin123
echo Database now contains 50 sample walk records
echo - 15 clients with various phone numbers
echo - 20 dogs of different breeds and ages
echo - 50 walk records from Dec 2024 to Dec 2024
echo.
pause