# Author: Bozhi Qian

## An ASP.NET Core MVC and Web Api Demo - Get pets grouped by owner's gender



### An ASP.NET Core 2.0 MVC front-end deployed to Azure App Service.

1. Azure DevOps bring this github repository to create a continuous  integration (CI) and continuous delivery (CD) pipeline from VSTS project "pets". Each change to the GitHub repo initiates a build in VSTS, and a VSTS Release Management definition executes a deployment to Azure. http://pets-people.azurewebsites.net/
2. This one was first time deployed from publishing directly from VS2017 to Azure App Service. http://aspnetcoremvcappdemo.azurewebsites.net/
 
### Restful Services
1. A json web service has been set up at the url: http://agl-developer-test.azurewebsites.net/people.json
2. And another people api is imeplemented at http://peopleapi2017.azurewebsites.net/swagger

### ASP.NET Core Middlewares to be used

* **Marvin.HttpCache** https://github.com/KevinDockx/HttpCache
* **AutoMapper** https://github.com/AutoMapper/AutoMapper
* **EntityFrameworkCore** https://github.com/aspnet/EntityFrameworkCore
* **AspNetCoreRateLimit** https://github.com/stefanprodan/AspNetCoreRateLimit
* **Moq** https://github.com/moq/moq4 
* **NLog.Web** https://github.com/NLog/NLog.Web
* **XUnit** https://github.com/xunit/xunit
* **Swashbuckle.AspNetCore** https://github.com/domaindrivendev/Swashbuckle.AspNetCore
 



### Source code
Please use latest VS2017 to open solution.

### Build Status from VSTS
[<img src="https://xps15.visualstudio.com/_apis/public/build/definitions/a051525a-3100-4942-9cb7-c141c7be301c/3/badge"/>](https://xps15.visualstudio.com/Pets/_build/index?definitionId=a051525a-3100-4942-9cb7-c141c7be301c) Commit changes to GitHub and automatically deploy to Azure

### Build configuration on VSTS
![Screenshot of the build definition from VSTS for the "Pet" project. ](file:///C:\Users\bozhi\Pictures\pets-vsts-build.png)

### Azure DevOps project
![Screenshot of the Azure DevOps project](file:///C:\Users\bozhi\Pictures\azure-devops-project.png)

### Reference: 
people.json

```json
[
  {
    "name": "Bob",
    "gender": "Male",
    "age": 23,
    "pets": [
      {
        "name": "Garfield",
        "type": "Cat"
      },
      {
        "name": "Fido",
        "type": "Dog"
      }
    ]
  },
  {
    "name": "Jennifer",
    "gender": "Female",
    "age": 18,
    "pets": [
      {
        "name": "Garfield",
        "type": "Cat"
      }
    ]
  },
  {
    "name": "Steve",
    "gender": "Male",
    "age": 45,
    "pets": null
  },
  {
    "name": "Fred",
    "gender": "Male",
    "age": 40,
    "pets": [
      {
        "name": "Tom",
        "type": "Cat"
      },
      {
        "name": "Max",
        "type": "Cat"
      },
      {
        "name": "Sam",
        "type": "Dog"
      },
      {
        "name": "Jim",
        "type": "Cat"
      }
    ]
  },
  {
    "name": "Samantha",
    "gender": "Female",
    "age": 40,
    "pets": [
      {
        "name": "Tabby",
        "type": "Cat"
      }
    ]
  },
  {
    "name": "Alice",
    "gender": "Female",
    "age": 64,
    "pets": [
      {
        "name": "Simba",
        "type": "Cat"
      },
      {
        "name": "Nemo",
        "type": "Fish"
      }
    ]
  }
]
```