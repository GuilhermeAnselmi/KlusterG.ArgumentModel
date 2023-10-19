# KlusterG.ArgumentModel
* This project converts command line arguments into a preloaded template.

# Requirements
* .NET Core v7.0

# How To Import?
**NuGet**
* Access the NuGet package manager in your project
* Click Search
* Search for KlusterG.ArgumentModel
* Install the latest version of the library

**.NET CLI**
* Type the command dotnet add package KlusterG.ArgumentModel --version 1.0.0

# How It Works?
**Description**
* Create a data model that represents the arguments and add the attributes to the respective properties

**Attribute Arguments**
* isRequired: Boolean type. Used to define as mandatory or not.
* name: String type. Mandatory use. Used to define argument name.
* abbreviation: String type. Used to define the argument name abbreviation.

**Example:**
* Argument Model in code
```
using KlusterG.ArgumentModel.Attributes;

namespace KlusterG.ArgumentModel.Sample.Models
{
    public class Person
    {
        [Argument(isRequired: true, name: "--name", abbreviation: "-n")]
        public string Name { get; set; }
        [Argument(isRequired: true, name: "--age", abbreviation: "-a")]
        public int Age { get; set; }
        [Argument(isRequired: false, name: "--alive", abbreviation: "-l")]
        public bool Alive { get; set; }
    }
}
```

* Code in Program.Main
```
using KlusterG.ArgumentModel.Sample.Models;

namespace KlusterG.ArgumentModel.Sample;
public class Program
{
    public static void Main(string[] args)
    {
        var model = ConvertArgument.DefineModel<Person>(args);
    }
}
```

* Call the Command Line with arguments
~~~
MyProgram.exe --name "Willian Smith" -a 34 --aline
~~~
