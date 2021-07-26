## FMsgStat
Easily create charts for your Messenger data
### How to use?
0. Download and install [.NET 5 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0)
1. `git clone https://github.com/v0idzz/FMsgStat`
2. `cd FMsgStat && dotnet build`
3. `mkdir messages`
4. Extract the contents of `messages/inbox` directory of your Facebook data archive to `messages` directory created in previous step
5. `dotnet run -p FMsgStat` - charts will be generated in `messages/{conversation_name}/charts` directory

### Disclaimer
This is my first F# project, so the code may not be perfect