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

Project Layout
--------------
Details coming soon

Additional Resources
--------------------
For more information, visit [jeremybytes.com](http://www.jeremybytes.com).