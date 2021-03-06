﻿using GameModel.Common.Math;
using WsServer.ClientMessages;
using WsServer.Common;
using WsServer.ServerMessages;

namespace WsServer.ClientMessageHandlers
{
    public class DefaultClientMessageHandler : CommonMessageHandler
    {
        public void OnSetPlayerName(uint clientId, SetPlayerNameClientMessage clientClientMessage)
        {
            GameState.SetPlayerName(clientId, clientClientMessage.Name);
            Messenger.Broadcast(new SetPlayerNameServerMessage(clientId, clientClientMessage.Name));

            Logger.Log($"Player {clientId} set name: {clientClientMessage.Name}");
            GameServer.BroadCastTop();
        }

        public void OnSendChatMessage(uint clientId, ChatClientMessage clientClientMessage)
        {
            Messenger.Broadcast(new ChatServerMessage(clientId, clientClientMessage.Message));
            Logger.Log($"Player {clientId} wrote to chat: {clientClientMessage.Message}");
        }

        public void OnUpdatePlayerState(uint clientId, UpdatePlayerStateClientMessage msg)
        {
            var p = GameState.SetPlayerControls(clientId, new Vector2D(msg.AimX, msg.AimY), msg.ControlsState);
        }

        public void OnPlayerShooting(uint clientId, PlayerShootingClientMessage msg)
        {
            Messenger.Broadcast(new PlayerShootingServerMessage(clientId, msg.Weapon));
        }

        public void OnPlayerTargetUpdate(uint clientId, UpdatePlayerTargetClientMessage msg)
        {
            var p = GameState.SetPlayerTarget(clientId, msg.AimX, msg.AimY);
        }

        public void OnPlayerSlotsUpdate(uint clientId, UpdatePlayerSlotsClientMessage msg)
        {
            var player = GameState.GetPlayer(clientId);

            player.BodyIndex = msg.Body;
            player.WeaponIndex = msg.Gun;
            player.ArmorIndex = msg.Armor;

            Messenger.Broadcast(new UpdatePlayerSlotsServerMessage(player));
        }

        public void OnPlayerHit(uint clientId, PlayerHitClientMessage msg)
        {
            var hitterPlayer = GameState.GetPlayer(msg.PlayeId);
            if (hitterPlayer != null && hitterPlayer.Hp > 0)
            {
                var targetPlayer = GameState.HitPlayer(msg.PlayeId, msg.HitPoints, clientId);

                Messenger.Broadcast(new SetPlayerHpServerMessage(targetPlayer));

                if (targetPlayer.Hp == 0)
                {
                    Logger.Log(GameState.top);
                    GameServer.BroadCastTop();
                }
            }
        }

        public void OnPlayerRespawn(uint clientId, PlayerRespawnClientMessage msg)
        {
            var respawnPlayer = GameState.RespawnPlayer(msg.PlayerId);

            Messenger.Broadcast(new PlayerRespawnServerMessage(respawnPlayer));
        }

    }
}