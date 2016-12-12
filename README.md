# Library

Installation guide.

Used technologies: ASP.NET MVC 5, SQL Server 2012.

To install the application one should specify the connection string in the web.config to the database which is currently bind to
 a local machine. The sql server 2012 database backup .BAK file is provided. Some data is being added to the database.
Important to note that RoldeData table in the database should not be modified or cleared to maintain normal work of the app.

Smtp configurations to send emails to the users, who takes books, currently contains credentials to the newly created gmail.com 
account. Those configurations should also be changed in case one wants to send emails from a different account or host.

Standard ASP.NET MVC 5 authorization was used in this project. As the result some authorization data is also stored in th local 
db.

User quide.

When application loads the user may see a list of books in the library. User can look up books' details, edit the number of the
books, take books. Editing of the total number of the specific book in the library is validated so that the total number of the
books can't be made negative or less then number of available books. In order to take the book user should register.
After registration the user is redirected to the list of books where one can take a book. Email notification is sent when the user clicks "Take" link. 

User can sort books by clicking on the corresponding table headers. Also one can click "View Available" link in order to
sort out all unavailable books.

User can view one's profile. To do that user should navigate to the "Readers" menu and select own details link. In the details 
link personal user information is displayed (all but password) along with user history with the dates when specific book was taken and when or if the book was returned. If the book wasn't returned the corresponding "Return" link is diplayed as well. By 
clicking on the link user returns the book to the library and the profile updates showing returning date. (Currently any user can
return book of any other user which is a flaw). 

The user also can see a book history. To do so one should navigate to "Books" menu tab and select "History" link of the specific book one is interested in. The book details are displayed along with the history of when and who took and returned (if returned) 
the book.

User also can create the book. In order to do so one should navigate to "Books" or "Library" link and click "Create" link. There 
one can add title, total quantity and authors of the new book. If the user can't find the proper author, he\she can add author after navigating to "Authors" menu tab and clicking "Create" link. Note that only non negative number of books can be specified 
and books' names should be unique.   
