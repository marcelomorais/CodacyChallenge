# CodacyChallenge
Hello Codacy Team! I hope you all are well.
This is my solution for your challenge, I hope you enjoy.

## Before we start

So, I tried my best to not over engineering and still show you my capabilities. On this project you will found some comments explaining the reason why I did that or even what I should be done (I didn't want to extend the challenge time too much).

OBS: I build as a plus a Console Application that provide in a easy way the commit list from a specific GitHub url. I will explain bellow how to run it. 

## Getting Started

#Console Application

If you are interested to run the Console Application, after clone the project you can open the "PublishedChallenge" folder and open your CMD inside it, now you can run
 "dotnet CodacyChallenge.ConsoleApplication.dll YOURURL".

 e.g (dotnet CodacyChallenge.ConsoleApplication.dll https://github.com/marcelomorais/CodacyChallenge)

 #WebApi

 To run the WebApi project just need to set the CodacyChallenge.API as StartUp project and press F5 to run.

 Request Example:

 Here is a example of request that you can run: "http://localhost:62932/api/git/repo/commits?url=https://github.com/marcelomorais/CodacyChallenge&requesttype=2"

 Request Object:
 The request object has 5 properties, they are;

 - RequestType { CLI = 1 , API = 2 } (It's an enumerator that will decide which type of engine you will run (If you will call GitHub by API or by CLI))
 - Url (string)
 - pageSize (int)
 - pageNumber (int)
