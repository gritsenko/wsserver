namespace WsServer.Common
{

    public enum ClientMessageType
    {
        SetPlayerName = 100,
        UpdatePlayerState = 101,
        UpdatePlayerSlots = 102,
        PlayerShooting = 103,
        HitPlayer = 104,
        RespawnPlayer = 105,
        UpdatePlayerTarget = 106,
        ChatMessage = 200,

        GetTiles = 50,
        GetMapObjects = 51,
    }
}