# Multco API

## Authorization

Developers may turn authorization on/off by performing the following steps in Visual Studio:

-   Go to  **project**  >  **properties**
-   Select desired configuration (e.g. Debug/Release)
-   In the "Conditional compilation symbols" field, define a variable  `AUTH`
-   Make sure to save the changes
-   Build project

## What is it?

This API allows Multnomah County access to all of their services in a easy to use SQL database, allows parsing of their service offers in PDFs, and easy querying and updating through a multitude of REST endpoints

## How to use

### Required software
- Visual Studio
- SQL Server Management Studio
- Microsoft SQL Server

### Installing Microsoft SQL Server
1. Choose Basic installation
2. Click I agree and accept the basic installation path
3. When it's done installing, take note of the connection settings as we will use these for connecting to the database (default is localhost)
4. Click install SSMS

### Installing SQL Server Management Studio
1. Download SQL Server Management Studio from the web page the previous step took you
2. Click install

### Installing Visual Studio
1. Download Visual Studio
2. Accept all the defaults and install

### Connecting to the database
1. Launch SQL Server Management Studio
2. Connect to the database with the server name (default is localhost)
3. Expand SQL Server
4. Click on databases
5. Click on File > Open > File
6. Choose “Setup_InternalServicesDirectoryV1.sql” from within the git repository you downloaded
7. Click on Execute, this will create the database!

### Populating the database
1. Run run_on_startup.exe from within Parser > run_on_startup.exe in the git repository you downloaded
2. Enter your URL links to the PDF files you want to parse and populate the database with
3. Hit q or Q when done entering links
4. Wait for parser to finish
5. Database should now be populated!

### Viewing the database
1. From within SQL Server Management Studio, expand InternalServicesDirectoryV1 > Tables
2. Right click on a table and click "Select top 1000 rows" to view the data in the table