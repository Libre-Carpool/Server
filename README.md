# Libre Carpool
Free, Open Source, and secure carpooling platform.
Free as in freedom and Open Source - given to you under the MIT license.
Free as in free beer - you don't pay for the software. You don't pay the driver.

## Libre Carpool Server
The back end of the [Libre Carpool](https://github.com/Libre-Carpool) platform - an ASP.Net website written in [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)).

You can't register to the service - an existing user with the right priviliges must add you. This is part of the *security* - making sure the other person is trustworthy.

---
### Installation
* Clone / download this repository
* Upload all of the files to your web server
* Create your database
* Create your [Google ReCaptcha 2 API key](http://www.google.com/recaptcha/admin)
* Update it in [`ReCaptcha.cs`](https://github.com/Libre-Carpool/Server/blob/master/App_Code/ReCaptcha.cs#L7) and [`login.aspx`](https://github.com/Libre-Carpool/Server/blob/master/login.aspx#L46).

### Common problems & solutions

1. You get the following error:
    >SqlException was unhandled by user code
An exception of type `'System.Data.SqlClient.SqlException'`
An attempt to attach an auto-named database for file `...\App_Data\Database.mdf` failed. A database with the same name exists, or specified file cannot be opened, or it is located on UNC share.

    This means you didn't create your SQL database (correctly).
    
    ---
    This is how you do it in Visual Studio:
    1. Right click on the project in the _Solution Explorer_,
    2. Select `Add` -> `Add New Item...` -> `SQL Server Database`.
    3. You'll get a message box. Click on `Yes`, put it in the 'App_Data' folder.
    4. In the _Server Explorer_, under `Data Connections`, right click on `Database.mdf`, and select `New Query`.
    5. Copy everything from [`Database Creation.sql`](https://github.com/Libre-Carpool/Server/blob/master/Database%20Creation.sql) and paste it in the new query. Execute it `(Ctrl+Shift+E)`
    6. Right click on `Database.mdf` in the _Server Explorer_, and select `Properties`.
    7. Copy the entire `Connection String`.
    8. Open [`DatabaseAccessor.cs`](https://github.com/Libre-Carpool/Server/blob/master/App_Code/DatabaseAccessor.cs) (under 'App_Code'), and paste it to the `CONNECTION_STRING`.
    9. Copy everything from [`Debug Database Reset.sql`](https://github.com/Libre-Carpool/Server/blob/master/Debug%20Database%20Reset.sql) and paste it in to a new query. Execute it `(Ctrl+Shift+E)`.
    
    ---
    TLDR:
	
    1. Create the `Database.mdf` file.
    2. Create the tables using the query in [`Database Creation.sql`](https://github.com/Libre-Carpool/Server/blob/master/Database%20Creation.sql).
    3. Populate the database using the query in [`Debug Database Reset.sql`](https://github.com/Libre-Carpool/Server/blob/master/Debug%20Database%20Reset.sql).