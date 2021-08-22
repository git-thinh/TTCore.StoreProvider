Microsoft.AspNetCore.Mvc
Microsoft.AspNetCore.Mvc.RazorPages
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.InMemory

Microsoft.AspNetCore.SignalR
Microsoft.AspNetCore.SignalR.Protocols.MessagePack

NLog
NLog.Web.AspNetCore

StackExchange.Redis
https://redis.io/topics/notifications
$ redis-cli      > config set notify-keyspace-events KEA
$ redis-cli--csv > psubscribe '__key*__:*'

Swashbuckle.AspNetCore.SwaggerGen
Swashbuckle.AspNetCore.SwaggerUI

Microsoft.Toolkit.Parsers.Rss

System.IdentityModel.Tokens.Jwt
Microsoft.AspNetCore.Authentication.JwtBearer

Google.Protobuf
Grpc.Core
Grpc.Tools

Microsoft.ClearScript.V8
Microsoft.ClearScript.V8.Native.win-x64
---------------------------------------------

* If you get the error ERR_SPDY_INADEQUATE_TRANSPORT_SECURITY in Chrome, 
run these commands to update your development certificate:

dotnet dev-certs https --clean
dotnet dev-certs https --trust