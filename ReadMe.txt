Microsoft.AspNetCore.Mvc
Microsoft.AspNetCore.Mvc.RazorPages
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.InMemory

Microsoft.AspNetCore.SignalR
Microsoft.AspNetCore.SignalR.Redis
Microsoft.AspNetCore.SignalR.StackExchangeRedis
Microsoft.AspNetCore.SignalR.Protocols.MessagePack

NLog
NLog.Web.AspNetCore

StackExchange.Redis

Swashbuckle.AspNetCore.SwaggerGen
Swashbuckle.AspNetCore.SwaggerUI
Microsoft.Docs.Samples.RouteInfo

LigerShark.WebOptimizer.Core
LigerShark.WebOptimizer.Sass
---------------------------------------------

* If you get the error ERR_SPDY_INADEQUATE_TRANSPORT_SECURITY in Chrome, 
run these commands to update your development certificate:

dotnet dev-certs https --clean
dotnet dev-certs https --trust