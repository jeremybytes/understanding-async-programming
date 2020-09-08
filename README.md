Understanding Asynchronous Programming in C# [Virtual Training]
============================
This project contains slides and code samples for the "Understanding Asynchronous Programminc in C#" virtual training with Jeremy Clark.  

Overview and Objectives
-----------------------
*Level: Introductory / Intermediate*  
Asynchronous code is everywhere. In our C# code, we "await" method calls to services and databases; and more and more packages that we use every day have asynchronous methods. But do you really understand what this does?  

Understanding is critical. When done correctly, we can make our applications more responsive, faster, and reliable. When done incorrectly, we can block threads or even hang the application entirely.  

In this 4-hour workshop, we'll start at the beginning to see how "await" relates to "Task”. We'll do this by calling an asynchronous method, getting a result, and handling errors that come up. We will create our own "awaitable" methods to see how Task and return types work together. With our own methods, we'll also better understand why we may (or may not) care about getting back to the original calling thread. We'll also cover some dangers, such as using "async void" or misusing ".Result". Finally, we'll use Task to run multiple operations in parallel to make things faster. With all of these skills, we can write more effective asynchronous code.  

For this workshop, it is assumed that you have experience with C#, but no specific asynchronous programming experience is needed. To run the samples code, you will need .NET Core 3.1 installed. Jeremy will be using Visual Studio 2019, but the code samples will run using Visual Studio Code or the editor of your choice.  

You will learn:  
* How to use "await" and "Task" to run asynchronous methods  
* About handling errors from asynchronous processes  
* About writing your own asynchronous methods
How to avoid pitfalls such as "async void" and ".Result"  
* About running multiple methods in parallel  

Running the Samples
-------------------
The sample code uses .NET Core 3.1. The console and web samples will run on all Window, macOS, and Linux versions that support .NET Core 3.1. The desktop samples are Windows-only.

Samples have been tested with Visual Studio 2019 and Visual Studio Code.

All samples require the "Person.Service" web service be running. To start the service, navigate to the "Person.Service" folder from the command line and type "dotnet run".

Ex:
```
C:\understanding-async\People.Service> dotnet run
```  

The service can be found at [http://localhost:9874/people](http://localhost:9874/people)

Project Layout
--------------
**Shared Projects**  
* *People.Service*  
A web service that supplies data for the sample projects.  
* *TaskAwait.Shared*  
A library with data types that are shared across projects (primarily the "Person" type).  
* *TaskAwait.Library*  
A library with asynchronous methods that access the web service.  
Relevant file: **PersonReader.cs**

**Concurrent Samples**
* *Concurrent.UI.Console*  
A console application that calls asynchronous methods, gets results, handles exceptions, and supports cancellation. (Windows, macOS, Linux)  
Relevant file: **Program.cs**
* *Concurrent.UI.Desktop*  
A WPF desktop application that calls asynchronous methods, gets results, handles exceptions, and supports cancellation. (Windows only)  
Relevant file: **MainWindow.xaml.cs**  
* *Concurrent.UI. Web*  
A web application that calls asynchronous methods, gets results, and handles exceptions. (Windows, macOS, Linux)  
Relevant file: **Controllers/PeopleController.cs**  

**Parallel Samples**
* *Parallel.UI.Console*  
A console application that uses Task to call asynchronous methods in parallel. Also gets results, handles exceptions, and supports cancellation. (Windows, macOS, Linux)  
Relevant file: **Program.cs**
* *Parallel.UI.Desktop*  
A WPF desktop application that that uses Task to call asynchronous methods in parallel. Also gets results, handles exceptions, and supports cancellation. (Windows only)  
Relevant file: **MainWindow.xaml.cs**  
* *Parallel.UI. Web*  
A web application that that uses Task to call asynchronous methods in parallel. Also gets results, and handles exceptions. (Windows, macOS, Linux)  
Relevant file: **Controllers/PeopleController.cs**  

**Progress Reporting**  
* *ProgressReport.UI.Console*  
A console application that reports percentage complete progress from an asynchronous method call. Also gets results, handles exceptions, and supports cancellation. (Windows, macOS, Linux)  
Relevant file: **Program.cs**
* *Parallel.UI.Desktop*  
A WPF desktop application that  reports percentage complete progress from an asynchronous method call. Also gets results, handles exceptions, and supports cancellation. (Windows only)  
Relevant file: **MainWindow.xaml.cs**  
* *TaskAwait.Library*  
This shared library contains a method that supports progress reporting.  
Relevant method:
```c#
public async Task<List<Person>> GetPeopleAsync(IProgress<int> progress,
    CancellationToken cancelToken = new CancellationToken()) {...}
```

Additional Resources
--------------------
**Video Series & Articles (by Jeremy)**  
Each of these has a lot of supporting links:  
* [I'll Get Back to You: Task, Await, and Asynchronous Programming in C#](http://www.jeremybytes.com/Demos.aspx#TaskAndAwait)  
* [Run Faster: Parallel Programming in C#](http://www.jeremybytes.com/Demos.aspx#ParallelProgramming)  
* [Learn to Love Lambdas in C# (and LINQ, ToO!)](http://www.jeremybytes.com/Demos.aspx#LLL)  
* [Get Func-y: Delegates in .NET](http://www.jeremybytes.com/Demos.aspx#GF)

**Other Resources**  
Stephen Cleary has lots of great articles, books, and practical advice.
* [Don't Block on Async Code](https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html) - Stephen Cleary
* [Async/Await - Best Practices in Asynchronous Programming](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming) - Stephen Cleary

For more information, visit [jeremybytes.com](http://www.jeremybytes.com).