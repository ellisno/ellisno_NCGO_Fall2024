using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using System;

public class SpawnController : NetworkBehaviour
{
    [SerializeField]
    private NetworkObject _playerPrefab;

    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private NetworkVariable<int> _playerCount = new NetworkVariable<int>(value:0,NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server );

    [SerializeField]
    private TMP_Text _countTxt;



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            NetworkManager.Singleton.OnConnectionEvent += OnConnectionEvent;
        }
        _playerCount.OnValueChanged += playerCountChanged;

    }



    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        NetworkManager.Singleton.OnConnectionEvent -= OnConnectionEvent;

        _playerCount.OnValueChanged -= playerCountChanged;

    }



    private void playerCountChanged(int previousValue, int newValue)
    {
        UpdateCountTextClientRPc(newValue);
    }



    [Rpc(target:SendTo.Everyone)]
    private void UpdateCountTextClientRPc(int newValue)
    {
        Debug.Log(message: "Message From Client RPC");
        UpdateCountText(newValue);
    }



    private void UpdateCountText(int newValue)
    {
        _countTxt.text = $"Players: {newValue}";
    }



    private void OnConnectionEvent(NetworkManager manager, ConnectionEventData eventData)
    {
        if (eventData.EventType == ConnectionEvent.ClientConnected)
        {
            _playerCount.Value++;
        }
    }


    public void SpawnAllPlayers()
    {
        if (!IsServer) return;

        int spawnNum = 0;

        
        
        foreach (ulong clientId in NetworkManager.ConnectedClientsIds)
        {
            NetworkObject spawnedPlayerNO = NetworkManager.Instantiate(_playerPrefab, _spawnPoints[spawnNum].position, _spawnPoints[spawnNum].rotation);

            spawnedPlayerNO.SpawnAsPlayerObject(clientId);
            spawnNum++;

        }
        
        
    }

}
