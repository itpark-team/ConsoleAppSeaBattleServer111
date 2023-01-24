using ConsoleAppSeaBattleServer.Net;

Lobby lobby = new Lobby("127.0.0.1", 34536);
lobby.StartLobby();
lobby.AwaitTwoClients();