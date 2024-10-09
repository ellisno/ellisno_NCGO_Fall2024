using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Unity.Collections;
using JetBrains.Annotations;
public struct PlayerInfoData : INetworkSerializable, IEquatable<PlayerInfoData>
{

    public ulong _clientID;
    public FixedString64Bytes _name;
    public bool _isPlayerReady;
    public Color _colorID;


    public PlayerInfoData(ulong id)
    {
        _clientID = id;
        _name = (FixedString64Bytes) "";
        _isPlayerReady = false;
        _colorID = Color.magenta; ;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            var reader = serializer.GetFastBufferReader();

            reader.ReadValueSafe(out _clientID);
            reader.ReadValueSafe(out _name);
            reader.ReadValueSafe(out _isPlayerReady);
            reader.ReadValueSafe(out _colorID);
        }
        else
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(_clientID);
            writer.WriteValueSafe(_name);
            writer.WriteValueSafe(_isPlayerReady);
            writer.WriteValueSafe(_colorID);
        }
    }

    public bool Equals([CanBeNull] PlayerInfoData other)
    {
        return _clientID == other._clientID && _name.Equals(other._name) && _isPlayerReady == other._isPlayerReady && _colorID.Equals(other._colorID);
    }

    public override bool Equals([CanBeNull] object obj)
    {
        return obj is PlayerInfoData other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(_clientID, _name, _isPlayerReady, _colorID);
    }

    public override string ToString() => _name.Value.ToString();

    public static implicit operator string(PlayerInfoData name) => name.ToString();

    public static implicit operator PlayerInfoData(string s) => new PlayerInfoData { _name = new FixedString64Bytes(s) };
}
