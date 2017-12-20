# Author: Bozhi Qian

## An ASP.NET Core MVC and Web Api Demo - Get pets grouped by owner's gender

A json web service has been set up at the url: http://agl-developer-test.azurewebsites.net/people.json

And another people api is imeplemented at http://peopleapi2017.azurewebsites.net/swagger

### An ASP.NET Core 2.0 MVC front end is deployed to Azure App Service as Azure Web App.

http://aspnetcoremvcappdemo.azurewebsites.net/

## ASP.NET Core Middlewares to be used

* **Marvin.HttpCache** https://github.com/KevinDockx/HttpCache
* **AutoMapper** https://github.com/AutoMapper/AutoMapper
* **EntityFrameworkCore** https://github.com/aspnet/EntityFrameworkCore
* **AspNetCoreRateLimit** https://github.com/stefanprodan/AspNetCoreRateLimit
* **Moq** https://github.com/moq/moq4 
* **NLog.Web** https://github.com/NLog/NLog.Web
* **XUnit** https://github.com/xunit/xunit
* **Swashbuckle.AspNetCore** https://github.com/domaindrivendev/Swashbuckle.AspNetCore
 
### people.json

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

### Source code
Please use latest VS2017 to open solution.

### Build Status from VSTS
[<img src="https://xps15.visualstudio.com/_apis/public/build/definitions/858197f8-7447-40b4-af5e-4ffd829dee43/2/badge"/>](https://xps15.visualstudio.com/GithubDemo/_build/index?definitionId={858197f8-7447-40b4-af5e-4ffd829dee43})

### Deployed site on Azure
http://aspnetcoremvcappdemo.azurewebsites.net/
