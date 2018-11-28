# Author: Bozhi Qian

## An ASP.NET Core 2.1 MVC Demo - Get pets grouped by owner's gender

### Screenshots
![Screenshot](screenshot.jpg)

### Programming challenge
A json web service has been set up at the url: http://agl-developer-test.azurewebsites.net/people.json

You need to write some code to consume the json and output a list of all the cats in alphabetical order under a heading of the gender of their owner.

You can write it in any language you like. You can use any libraries/frameworks/SDKs you choose.

Example:
Male
Angel
Molly
Tigger
Female
Gizmo
Jasper
Notes
Submissions will only be accepted via github or bitbucket
Use industry best practices
Use the code to showcase your skill.

### Restful Services
1. A json web service has been set up at the url: http://agl-developer-test.azurewebsites.net/people.json


### Source code
Please use latest VS2017 to open solution.


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