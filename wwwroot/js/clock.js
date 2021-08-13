"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/clock").build();
connection.on("ShowTime", function (message) {
    console.log(message);
});
connection.start().then(function () {
    console.log('/hubs/clock STARTED .....');
}).catch(function (err) {
    return console.error(err.toString());
});