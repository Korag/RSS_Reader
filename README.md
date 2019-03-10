# RSS Reader Project Documentation

## About the project

RSS Reader is a WEB application consisting of 2 projects, which are responsible for downloading information from RSS feeds, storing and archiving them, as well as providing a user interface thanks to which you can browse the stored information. With RSS Reader you can view your favourite RSS feeds at any time, even if the feed provider is temporarily offline - all RSS feeds displayed in the application are loaded directly from the database and are always available to the user.

## What are the components of a fully functional RSS Reader? 

This application consists of three main components, which must be operational for the correct functioning of the final product. 

+ RSS Downloader

+ RSS Reader

+ MongoDB Database

Additionally, there are 2 dll libraries within the application, which are necessary for proper operation. These libraries contain common elements, which use both RSS Downloader and RSS Reader projects.

+ RSSDbContextLib

+ RSSModelsLib

In addition to the elements providing the main functionalities, there are also modules not required for the operation of the application but providing new amenities for users.

+ Newsletter Service Provider

## General principle of operation

The system created by us works in the following way. 

+ Every specified interval RSS Downloader downloads the current content of RSS feeds and enters this data into a non-relational MongoDB database. 

+ The MongoDB non-relational database is an intermediary layer between RSS Downloader and RSS Reader instances, as well as Email Service Provider. Its role is to provide collections that store the content of RSS feeds, including archived data.

+ RSS Reader provides a presentation layer for the user, which is available at the web address. Thanks to it, the reader has access to news from RSS feeds and can quickly move to the information of interest to him/her.

+ The Newsletter Service Provider provides subscribers with the possibility of subscribing to their favourite RSS feeds, which results in sending regular emails to their mailboxes, which contain the latest updates to the RSS reader.

## A detailed discussion of the individual components.

### RSS Downloader

RSS Downloader is a console application written in C# language, which is placed on the Azure server and executed there periodically every specified time interval using the Jobs functionality provided by the Azure environment. Because it is a console application, it does not have a frontend layer. The user using our RSS reader does not have access to the RSS Downloader project, which is the only and exclusive provider of data used in the project of the reader itself. RSS Downloader communicates with the MongoDB database through the implemented Db Context allowing to connect to a remote database located on a separate server. If the RSS Downloader project fails or the Azure RSS Reader platform fails to work properly, it will remain fully functional but will not be updated with new data, because the update mechanism will be faulty.

### MongoDB

In our project, non-relational MongoDB database serves as a database, where RSS Downloader introduces newly downloaded elements and RSS Reader uses the database to update the status of the WEB client application. To work with the database we use a free cloud solution which is:

https://mlab.com/

This free database allows you to manage collections not exceeding 500MB completely free of charge. Thanks to the possibility of choosing the server on which our documents will be stored, we can adjust the location of the server to our needs. In this project, where the number of references to the database is not very large, such an approach is completely sufficient.

### RSSModelsLib

RSSModelsLib is a project compiled into a dll library, which stores models used in the main projects.

#### Structure of individual collections(from Models in RSSModelsLib)

`RssDocumentSingle` stores information about the RSS channel and contains a list of items where specific messages contained therein are stored.
``` c#
namespace RssModelsLib.Models
{
    public class RssDocumentSingle
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string LastUpdate { get; set; }
        public string Flag { get; set; }
        public DateTime LastFetched { get; set; }
        public List<RssDocumentItem> RssDocumentContent { get; set; }
    }
}
```

`RssDocumentItem`contains information about atomic information contained inside a specific RSS channel.
``` c#
   public class RssDocumentItem
    {
        public string Guid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public object Image { get; set; }
        public string Links { get; set; }
        public string DateOfPublication { get; set; }
        public string Category { get; set; }
        public DateTime LastModified { get; set; }
    }
```

`SubscriberEmail` contains the email address with a list of channel subscriptions assigned to it
``` c#
   public class SubscriberEmail
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string EmailAddress { get; set; }
        public List<string> SubscriberList { get; set; }
    }
```
### RSSDbContextLib

It is a dll library which is a context used for connection to a remote database. Inside the library there are all necessary functions that manipulate directly on the data in the database.

``` c#
   public RssDocumentsRepository()
        {
            string path;
            string user;
            string password;
            string database;
            string connectionstring;

            path = @"C:\credentials.txt";
            user = "Admin";
            password = File.ReadLines(path).First();
            database = "rss_downloader_web_application";
            connectionstring = $"mongodb://{user}:{password}@ds062818.mlab.com:62818/{database}";

            InitializeContext(connectionstring, database);
        }


        public void InitializeContext(string connectionstring, string database)
        {
            _mongoClient = new MongoClient(connectionstring);
            _server = _mongoClient.GetDatabase(database);
            _rssDocumentCollection = _server.GetCollection<RssDocumentSingle>(collectionRSS);
        }
```

### RSS Reader

RSS Reader is a project made in ASP.NET MVC technology. Of course with implemented MVC pattern. Our controller in this case is WebApi, which provides appropriate methods, including connection with Db Context for the view. The view is not based on RAZOR, but on Vue.js. 

`here some code from Reader, what functionality? adding to mailingList and more..`

### Newsletter Service Provider

Newsletter Service Provider is a module in the RSS Downloader project, whose task is to periodically send emails to the mailing list pairs of email address and list of subscribed feeds.

The Postal framework is used for this purpose. Since this framework is designed for ASP.NET MVC and the RSS Downloader project is a console application, you need to add some RAZOR view support packages on which this technology is based.

`EmailServiceProvider` is a class that stores the methods of the service allowing you to send a newsletter by e-mail.
``` c#
private void SendSubscriberEmail(SubscriberEmail model)
        {
            dynamic email = new Email("Email");

            email.To = model.EmailAddress;
            email.SubscriberList = model.SubscriberList;

            email.From = _emailFrom;

            email.RssList = _context._rssDocumentCollection.Find(new BsonDocument()).ToList();

            email.Subject = "Newsletter RSS Reader RMF24 - " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + ".";

            Task t = _service.SendAsync(email);

            int attemptsCounter = 1;

            try
            {
                t.Wait();
            }
            catch (Exception)
            {
                while (t.Status == TaskStatus.Faulted)
                {
                    if (attemptsCounter < 6)
                    {
                        attemptsCounter++;
                        Task.Delay(900000);
                        try
                        {
                            t = _service.SendAsync(email);
                            break;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
```

`Email` is a view based on the RAZOR page, which defines the structure of data contained in an email message.
``` html
@Model RssModelsLib.Models.EmailTemplateViewModel
<h2>Witamy.</h2>


@if (Model.SubscriberList != null)
{
    <p><b>Nowości w obserwowanych kanałach:</b></p>
    <ul>
        @foreach (var Title in Model.SubscriberList)
        {
            <h2>@Title</h2>

            foreach (var singleDocument in Model.RssList)
            {
                int counter = 0;

                if (singleDocument.Title == Title)
                {
                    foreach (var singleDocumentContent in singleDocument.RssDocumentContent)
                    {
                        DateTime PubDate = Convert.ToDateTime(singleDocumentContent.DateOfPublication);
                        if (DateTime.Now.Subtract(PubDate).Hours < 12)
                        {
                            <li>
                                <a href="@singleDocumentContent.Links">@singleDocumentContent.Title</a>
                            </li>
                            counter++;
                        }
                    }
                    if (counter == 0)
                    {
                        <li>
                            <p>Brak nowości z ostatnich 12 godzin.</p>
                        </li>
                    }

                }

            }

        }
    </ul>
}

else
{
    <p>
        Podczas subskrypcji newslettera nie wybrano żadnych kategorii.<br>
        Proszę powrócić do etapu wybierania subskrypcji newslettera.
    </p>
}
<br />

<h4>Pozdrawiamy</h4>
<h4>Zespół Vertisio</h4>

<img src="https://raw.githubusercontent.com/Korag/RSS_Reader/master/Rss_Downloader_Console/Assets/vertisio_logo_tiny.png" />
<br>

<form action="http://localhost:59554/Reader/CancelNewsletter?EmailAddress=@Model.To" style="text-align:center" method="POST" id="ResignFromNewsletter">
    <p>Nie chcesz otrzymywać newslettera?</p>
    <input type="submit" value="Rezygnuję z newslettera" />
</form>
```

## Contact us if you have any questions

If you want to contact us, you can do so via github or email address vertisio.com@gmail.com.

If you would like to develop our project and add new functionalities, we are open to any suggestions. We will consider every push to the remote repository

## About Vertisio group

We are a group of students currently consisting of 4 members. 

+ Łukasz (`Korag`)
+ Kamil (`kAHA`)
+ Bartosz (`Owner`)
+ Konrad (`HoUdini`)
