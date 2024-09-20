using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
public class UI_NetManager : NetworkBehaviour
{
    [SerializeField] private Button _serverBtn, _clientBtn, _hostBtn, _startBtn;

    [SerializeField] private GameObject _connectionBtnGroup;
    [SerializeField] private SpawnController _mySpawnController;

    
    // Start is called before the first frame update
    void Start()
    {
        _startBtn.gameObject.SetActive(false);
       if (_serverBtn != null) _serverBtn.onClick.AddListener(ServerClick);
       if (_clientBtn != null) _clientBtn.onClick.AddListener(ClientClick);
       if (_hostBtn   != null) _hostBtn.onClick.AddListener(HostClick);
       if (_startBtn  != null) _startBtn.onClick.AddListener(StartClick);

    }

    private void ServerClick()
    {
        NetworkManager.Singleton.StartServer();
        _connectionBtnGroup.gameObject.SetActive(false);
        _startBtn.gameObject.SetActive(true);
    }

    private void ClientClick()
    {
        NetworkManager.Singleton.StartClient();
        _connectionBtnGroup.gameObject.SetActive(false);
        
    }

    private void HostClick()
    {
        NetworkManager.Singleton.StartHost();
        _connectionBtnGroup.gameObject.SetActive(false);
        _startBtn.gameObject.SetActive(true);
    }

    private void StartClick()
    {
        if (IsServer)
        {
            _mySpawnController.SpawnAllPlayers();
            _startBtn.gameObject.SetActive(false);
        }
    }
}
