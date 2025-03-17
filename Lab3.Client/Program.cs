using Lab3.Client;

var clientCreated = SocketClient.TryCreate("127.0.0.1", 8080, out var client);
if (clientCreated)
{
    client.Send("This is a client.");
}