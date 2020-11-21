Use on your own risk

Spam Cli
The program is used to send mass of emails to one or more receivers from one or more senders.
It stores its data in several .txt files and the user must prepare them before running the program.
Running from command line :
SpamCli.exe <Root directory: String> <NumberOfMails : Integer>
Example : spamcli.exe c:\temp 100  - will send 100 email (will be explain) with data located in c:\temp.
Uses google mail as senders – must create app token for each sender.

The data 
Recivers.txt :  holds the addresses for the receivers, each address in separate line. Be aware not to end with empty line in the EOF – will crash!!
Senders.txt : holds the address and the tokens for the senders. Each address in separate line and bellow its token :
 
Be aware not to end with empty line in the EOF – will crash!!
Headers.txt : holds subjects to mail message, each subject in separate line.  Be aware not to end with empty line in the EOF – will crash!!
Body.txt : holds the message body.

The sending
For each receiver in the file, it runs NumberOfMails times and send message where:
It randomize the subjects and randomize the senders in order not to send the same message all the time.
At the end, log.txt file is created with the data of the process.
Written in C#.Net By Itay Cohen, 11/2020.
Use on your own risk
