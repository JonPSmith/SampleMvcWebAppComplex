SampleMvcWebApp - Complex
=========================

SampleMvcWebAppComplex is a ASP.NET MVC5 web site which was built as a 'stress test' of how well 
[GenericServices library](https://github.com/JonPSmith/GenericServices) and Microsoft's 
[Entity Framework V6](http://msdn.microsoft.com/en-us/data/aa937723) could cope with an existing SQL
database [AdventureWorksLT2012](http://msftdbprodsamples.codeplex.com/releases/view/55330).
This database is a step up on complexity over the simple database used in the example available at 
[SampleMvcWebApp](http://samplemvcwebapp.net/).

You can read more about this exercise in two articles I wrote for the 
[Simple-Talk web site](https://www.simple-talk.com/). The two articles are:

1. [Using Entity Framework with an Existing Database: Data Access](https://www.simple-talk.com/dotnet/.net-framework/using-entity-framework-with-an-existing-database-data-access/)
2. [Using Entity Framework with an Existing Database: User Interface](https://www.simple-talk.com/dotnet/asp.net/using-entity-framework-with-an-existing-database--user-interface/)

#### Go to the [example web site](http://complex.samplemvcwebapp.net/) for live demostrations of SampleMvcWebApp - Complex 

NOTE: if you are new to GenericServices I suggest you start with [SampleMvcWebApp](http://samplemvcwebapp.net/)
as it explains the basics. However if you want more read on.

### Important information

1. This application is *NOT* open-source because it contains a proprietary, paid-for library 
[Kendo UI MVC](http://docs.telerik.com/kendo-ui/aspnet-mvc/introduction) which I have a developer's licence for.
2. If you clone this application is will NOT run. That is because:
  * I have not included the Kendo UI MVC code because I am not allowed to.
  * I have not included the database, but you can pick that up from [here](http://msftdbprodsamples.codeplex.com/releases/view/55330).
Look for the 'AdventureWorksLT2012_Data' entry.

### So why have made the source code available?

While this application is not open-source it does contain a great deal of useful reference material for
anyone wanting to use [GenericServices](https://github.com/JonPSmith/GenericServices) in a real application.
It also makes writing the documentation much easier as I have examples to link to.

There are a number of usages of GenericServices not seen in the original 
[SampleMvcWebApp(basic) code](https://github.com/JonPSmith/SampleMvcWebApp) 
which are useful to see. 
Examples of the GenericServices features found only in this example are:

- Use of [Calculated Properties](https://github.com/JonPSmith/GenericServices/wiki/Calculated-properties)
using [DelegateDecompiler](https://github.com/hazzik/DelegateDecompiler).
- Use of [AutoMapper calculated properties](https://github.com/JonPSmith/GenericServices/wiki/DTO-data-copying#using-automapper-for-calculated-properties).
for the (few) cases that DelegateDecompiler cannot handle.
- Various places where the developer needs to overide some of the DTO methods like 
`FindItemTrackedForUpdate`, `CreateDataFromDto` etc.
- Using `DeleteAssociatedAddress` to handle the more complex deletion situations.
- Handling nested DTO copying.
- Handling updates that include relational data.
- Useful to see the MVC Controllers and Views needed for this sort of application. 

