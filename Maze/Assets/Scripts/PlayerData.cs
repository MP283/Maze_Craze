using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable{

	public ulong clientId;
    public int colorId;
    public FixedString64Bytes playerUserName;
    public FixedString64Bytes playerId;

    public PlayerData(ulong clientId, int colorId, string playerUserName,string playerId) {
        this.clientId = clientId;
        this.colorId = colorId;
        this.playerUserName = playerUserName;
        this.playerId = playerId;

        Debug.Log("Player Data colorid " + colorId);
    }

	public bool Equals(PlayerData other) {
        return clientId == other.clientId && colorId == other.colorId && playerUserName == other.playerUserName && playerId == other.playerId;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref clientId);
        serializer.SerializeValue(ref colorId);
        serializer.SerializeValue(ref playerUserName);
        serializer.SerializeValue(ref playerId);
    }

}
/* && 
            colorId == other.colorId &&
            playerName == other.playerName &&
            playerId == other.playerId*/