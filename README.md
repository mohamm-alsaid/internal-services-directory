## Versions
Visual Studio Community 2019 v16.5.1

Microsoft SQL Server 2019 (Developer)

SQL Server Management Studio (SSMS) release v. 18.4 


## Setting Up From Scratch

1. Open up the SSMS client, and then open a new query. Copy and paste the contents of `Setup_InternalServicesDirectory.sql` into the window, and then execute the script. This defines the schema for the database.

2. Create a new ASP.NET Web Application (.NET Framework) project in Visual Studio Community 2019, and call it `API-ISD`. Be certain that this is the C# version of the project.

3. In the same solution, add a new Class Library (.NET Framework) project to the solution. Name it `DataAccessModel`. Delete the `Class1.cs` class that gets automatically generated. You don't need it.

4. In the `DataAccessModel` project, add a new item, an `ADO.NET Entity Data Model`. Call it `DataModel`. A wizard will pop up. Select the `EF Designer From Database` option, it should be the one selected by default. Hit next. 

5. Click `New Connection` on the next page of the wizard. Make sure the Data Source is Microsoft SQL Server. In the next page, type in `localhost` to the Server Name dropdown menu, then under `Connect to a database`, select `InternalServicesDirectoryV1` from the dropdown menu. Press okay.

6. Hit next when you get back to the `Choose Your Data Connection` page. The default settings are fine. On the next page, toggle the `Tables` checkmark to be checked, then press Finish. Press `Okay` on any following pop-ups until you're greeted by what resembles a database diagram. You've now generated your models.

7. In the `DataModelAccess` project, open `App.Config` and copy the XML with the tags `<connectionStrings>`, and paste it into the `Web.config` file in `API-ISD` anywhere where it's a new tag section (basically don't put it in the middle of any other tags).

8. Right click on `References` in `API-ISD`, and then `Add References`, then click the checkbox for `DataModelAccess`. Build the project. **YOU MUST BUILD THE PROJECT BEFORE THE NEXT STEP!**

9. For every model you want an endpoint for, right click on the `Controllers` folder in `API-ISD`, then Add->Controller. In the resulting window, select `Web API 2 Controller with actions, using Entity Framework`. Another window will pop up, either type in the model you want to create a controller for, or select it from the dropdown menu. The data context class should be `InternalServicesDirectoryV1Entities (DataModelAccess)`. Press Add.

10. Repeat step 9 until you have all the controllers you want.

11. Edit `WebApiConfig.cs` in the folder `App_Start` in `API-Config` to contain the following code inside of the `Register` method:
```cs
config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling 
    = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling 
    = Newtonsoft.Json.PreserveReferencesHandling.Objects;
```
(Note: We haven't yet decided if this is the best way to handle circular references.)

12. The project should now be able to be ran, but no data currently exists in the database. A sample SQL script, `Dummy_Data_Script.sql` has been provided to populate the database such that every table has at least one entry.
