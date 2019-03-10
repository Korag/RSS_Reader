# RSS Reader Project Documentation

## About the project

RSS Reader is a WEB application consisting of 2 projects, which are responsible for downloading information from RSS feeds, storing and archiving them, as well as providing a user interface thanks to which you can browse the stored information. With RSS Reader you can view your favourite RSS feeds at any time, even if the feed provider is temporarily offline - all RSS feeds displayed in the application are loaded directly from the database and are always available to the user.

## What are the components of a fully functional RSS Reader? 

This application consists of three main components, which must be operational for the correct functioning of the final product. 

+ RSS Downloader

+ RSS Reader

+ MongoDB Database

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

### RSS Reader

### Newsletter Service Provider

## Contact us if you have any questions

## About Vertisio group

