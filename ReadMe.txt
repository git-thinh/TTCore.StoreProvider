Microsoft.AspNetCore.Mvc
Microsoft.AspNetCore.Mvc.RazorPages
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.InMemory

Microsoft.AspNetCore.SignalR
Microsoft.AspNetCore.SignalR.Protocols.MessagePack

NLog
NLog.Web.AspNetCore

StackExchange.Redis

Swashbuckle.AspNetCore.SwaggerGen
Swashbuckle.AspNetCore.SwaggerUI
Microsoft.Docs.Samples.RouteInfo

Microsoft.Toolkit.Parsers.Rss

System.IdentityModel.Tokens.Jwt
Microsoft.AspNetCore.Authentication.JwtBearer

Google.Protobuf
Grpc.Core
Grpc.Tools

---------------------------------------------

* If you get the error ERR_SPDY_INADEQUATE_TRANSPORT_SECURITY in Chrome, 
run these commands to update your development certificate:

dotnet dev-certs https --clean
dotnet dev-certs https --trust