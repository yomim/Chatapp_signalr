


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
connection.on("ReceiveMessage", function (user, message) {

    var msg = user + ":" + message;
    var li = document.createElement("li");
    li.textContent = msg;
    $("#list").prepend(li);


});
connection.start();
$("#btnsend").on("click", function () {
    var user = $("#txtuser").val();
    var message = $("#txtmessage").val();
    connection.invoke("SendMessage", user, message);

});