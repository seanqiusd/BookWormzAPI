Book Wormz API: Blue Badge Group API Project
=============================================

[![N|Solid](https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcShNr_qvEhshsb5yO_f4jm061arTxC6JtT6mA&usqp=CAU)](https://visualstudio.microsoft.com/vs/community/ "VS Community 2019")
Visual Studio Community 2019 is a free software program provided to you through the good folks at Microsoft. You can download it by clicking the purple logo above.


> [!IMPORTANT]
> How To Download Our Web API and Console App
>-------------------------
> Please feel free to clone and test our BookWormz Web API and Console App.
> 1. Since we are using C# as our programming language, please make sure you have already downloaded Visual Studio Community 2019. Click on the purple icon above if you'd like a download. VS Community 2019 runs on windows and can be bootcamped onto Macs.
> 2. There's a few different ways you can go about downloading our Web API and Console App, but we recommend if you're new to coding, just click on the green "Code" button to the right of the repository page to download the file. It'll be a zip file so after downloading, open your file explorer. Find the file wherever it is you chose to download it, and hit "Extract All" and then also select where you'd like to extract it. 
> 3. Once Extracted, find the file titled "BookWormz". 
> 4. Then double click on the solution file called "BookWormz.sln".
> 5. Our assemblies for the Web API and Console App automatically open in VS Community 2019, assuming you have that downloaded already.

> [!IMPORTANT]
> How To Run Our Web API and/or Console App
>-------------------------------------------
> 1. In order to run the Web API, you'll want to go to the solution explorer to the right of your screen. There you'll find my stack of console applications. Right click on assembly titled "BookWormz.WebApi" and select "Start As Project Startup". 
> 2. If for whatever reason your solution explorer isn't already pinned to the right of your screen, then just go to the top icon under view and select Solution Explorer or just use the shortcut Ctrl Alt L and following all the steps as aforementioned in step 1.
> 3. Then go to the Green Start button at the top of your screen and select start, to launch the web api. 
> 4. Assuming you have Google Chrome Web Browser installed as your default browser, a local host uri should load once you've hit the start button. If not, please switch over to Google Chrome or download google chrome by doing a simple search for Google Chrome Web Browser that is compatible with your system. Here's the link to download Google Chrome: https://www.google.com/chrome/.
> 5. Since the goal of this project was to create the back-end framework for a Web API, when running, you'll not be met with a front-end user interface. In order to try out the features we've implemented, please download the API client called Postman. You can find the link to download Postman here: https://www.postman.com/downloads/.
> 6. Once Postman is downloaded and launched, go to the center of the app called My Workspace and open a new tab; there'll be a small plus sign with a circle around it. This'll initiate a new, untitled request. Now go back to the local host page we launched just a little while ago.
> 7. From the local host page, at the top  of the page please select API. This will lead  you to a list of endpoints that have been established for our Web API. 
> 8. The first feature you should try is creating a user account. If you look at account, click the endpoint that says POST api/Account/Register. This will take you to a page that lists the required body parameters that you'll have to input in the body parameters of Postman. Before we go back to postman, double click on the url bar, right click and copy the url and paste that url in the "enter request url" bar in postman. Then delete the back part of the url up to the local host number. So for example, you'll be deleting the "Help/Api/Post-api-Account-Register", then replacing it with "api/account/register" (i.e. the endpoint we selected earlier is what we put after the "http://localhostnumber/" and this same exact method will be used for any of the other endpoints you'd like to try out going forward. So for example, to create a book (i.e. Post), you'll add "api/Book" after the "http://localhostnumber/". Under the requested url bar, select Body. Then 
> 9. After pasting and completing the url in Postman, to the left of the requested url bar, you'll see a drop down menu. Make sure that Post is selected. Post means to create. Since we are wanting to create a user, "Post" will need to be selected. For future references, "Get" means to read/retrieve, "Put" means to update, and "Delete" means to remove.
> 10. After selecting Post from the drop down menu, below that select Body, and then below that select x-www.form-urlencoded. From the localhost page, we pulled up earlier the endpoint Post api/Account/Register. If you scroll down just a little, you'll see the boy parameters that you'll need to input for the Key in the body parameters of the Postman url request we just set up. Make sure to type each body parameter into the key category as you see it written in the local host page. For each key, fill in your own information in the value column next to the key. Please be sure to pay attention to the Type and the Additional Information next to the body parameters from the local host page to make sure you're inputting information that is consistent with the data return type and required inputs needed for each said parameter. When you're ready to create the user, next to the "requested url" bar, there's a blue send button that you'll want to select. If you've entered all the correct parameters, then you should get a "200 ok" status to the right of the response field near the middle right of your screen in Postman. 
> 11. There's two ways to see if our program created your user. The first is by going back to our assembly in VS Community. If you don't have SQL Server Object Explorer pinned and hidden on the left-hand side of your VS Community screen, go to the Search bar at the top of VS Community and type in "SQL SErver Object Explorer" and select it to launch or use the shortcut Ctrl+S or Ctrl+/. Once that's open, select SQL server. Then select (localdb)\MSSQLLocalDB. From that dropdown, you'll see some more dropdown options. Select the "Databases" folder. From there you'll see more dropdown options with cylinders next to them. These are SQL databases. The one named "BookWormz" is the one you'll want to click on. Then a list of dropdown folders will appear. Select the one named "Tables". More dropdown options will appear. Right click on the one named "dbo.ApplicationUser" and select "View Data". If all went according, a SQL table appears and it'll have the informatin for the user you created. The second way is to retrieve the User in Postman, but before we can do that, we need to obtain a token first. A token is used for authentication purposes and is needed before we can retrieve a user. To get a token, in the "Requested URL" bar, delete everything after the local host number and forward slash. After the forward slash, add "token". Then in the body parameters, deselect the previously entered parameters by unchecking them on the left. Don't erase these because we can use them later. Enter the following 3 new parameters in the body: grant_type, username, and password. The value for grant_type should be password. For username, type in the email you used to register with earlier. For password, type in the password you created earlier. Next to Body, there's an option to view the Headers. Make sure in Headers that Content-Type is selected. Then hit the blue send button. You should get a "200 ok" response and a rather long string of characters, which is your access_token for the bearer. Copy everything inside the quotations. We'll take your access token to retrieve the user. Change "Post" to "Get". In the "Requested URL" bar delete every after the local host number and forward slash. Replace with "api/account/userinfo". In the Headers, add a key named "Authorization". In the value column, put "bear" and add a space and paste in the access token you obtained just a while ago. Click the blue send button. You should get a "200 ok" status and be able to see the email you registered with in the response field. The bool "HasRegistered" should be "true". 
> 12. The next feature you should try is posting a book you own and would like to share with someone else. On the local host page, click on API at the top. Scroll down and you'll see under the heading "Book" an endpoint named Post api/Book. Click it. You'll be given the list of body parameters you'll need to enter and input information for in Postman body. Make sure "Post" is selected and in the "Requested Url" bar, delete everything after the local host number and forward slash and add the endpoint you just clicked on, which is "api/book". In the Header in Postman make sure that your Authorization you created earlier is still selected and in the body after inputting all required data, hit the blue send button. You should get a "200 ok" status and a message with the title of the book you added as having been added.  
> 13. To delete a book, in Postman, make sure "Delete" is selected and then in the "Requested url" after the local host number and forward slash, type "api/Book?ISBN=". After the equal sign, make sure to put the ISBN of the book you wish to remove. In the body parameters make sure the key ISBN is selected and everything else deselected. Then hit the blue send button. You should get a "200 ok" status and get a response message stating that your said ISBN has been deleted. 
> 14. In order to run the Console App, you have to have our Web API's local host running simultaneously, so if you haven't closed out of the local host page don't. If you have, then please relaunch. Then go to wherever the solution file for BookWormz is saved and launch a second BookWormz. This time in the solution explorer, go to the the assembly named "BookWorms.UI", right click, and select "Set as startup project". Then launch the program by hitting the green play button at the top. A console window opens. Just follow the prompts on the screen and enjoy!   

>-----------------------------------------------------------------------------------------------------
>-----------------------------------------------------------------------------------------------------
What To Expect
----------------
> 1. BookWormz was created to allow our fellow book worms to go back to the basics with enjoying a physical book. We recognize that books can be costly, take up space, and kill trees, but there are many amongst us who still love the feel and understand the value of a book--not to mention there�s already so many existing books, we�ve got to find a use for them somehow. In BookWormzAPI, our aim is to create a community of users who will partake in book shares. We want each individual user to have the freedom to connect to other users in exchanging books they have and/or are interested in reading. Together we can help each other out in not only doing something that could help the environment and economy, but we can connect with more people. Through our Web API and Console App, we believe we've done all that we set out to do. Through this Web API and Console App we'll create a community of book sharers who will be able to create accounts, post avaiable books, delete a book that you've posted, see available books and individual details of each book, and exchange books with other users. In order to maintain quality of experiences through the exchanging of books, we also allow users to rate the sender of books they perform exhanges with. It is our hope that through community self-monitoring, issues such as responsiveness in communication during the exchange process and the receiving of books can be held to higher standards, thereby creating a better experience for all users. 


>-------------------------------------------------------------------------------------------------------
>-------------------------------------------------------------------------------------------------------
Goals Of The Code
-------------------
> 1. One goal of this project was to learn how to work together as a team. As a team we learned to better communicate what each of us was working on. We also learned to work together to have a better experience with Github. 
> 2. Another goal for this project was for us to better understand the interactions of foreign keys across multiple databases in SQL.
> 3. We also explored how to build a console app based upon using a local host Web API as a client.  