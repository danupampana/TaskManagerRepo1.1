TaskManager.ClientApi is the web api project built to accomplish the task. It has been integarted with swagger to run the unit tests.

I have used Clean Architecture, Mediator Pattern and CQRS design pattern to allow the application to be domain driven, test driven. 
Query and Commmand handlers can be found in TaskManager.Application.Handlers

Please modify the database connection string in TaskManager.ClientApi appsettings.json to connect the database in your network. 
Also run the TaskDbScript.sql attached to create the required database and tables.

MaxHighPriorityTasksCount in appsettings allows you to run the add/update high priority tasks test case for your desigired count. 
Testing for 100 records is tedious, we can leverage this option to set the max count to single digit and run our unit test.

I thought of creating the docker image to pusblish in any of the container, but my personal laptop hardware not supporting virtulization to run docker desktop application.
So I just created a publish profile to deploy the web api project in Azure App Service.

Application deployed in Azure App Serive. You can browse the apis at https://taskmgrappservice.azurewebsites.net/index.html

Also created NUnit project to give an idea of how we can test the command and query handlers through mediator. Sorry,  I didn't get enough time to write all the tests.


