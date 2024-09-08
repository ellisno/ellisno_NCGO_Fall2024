using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
public class UI_NetManager : NetworkBehaviour
{
    [SerializeField] private Button _serverBtn, _clientBtn, _hostBtn;
    // Start is called before the first frame update
    void Start()
    {
        _serverBtn.onClick.AddListener(ServerClick);
        _clientBtn.onClick.AddListener(ClientClick);
        _hostBtn.onClick.AddListener(HostClick);

    }

    private void ServerClick()
    {
        NetworkManager.Singleton.StartServer();
        this.gameObject.SetActive(false);
    }

    private void ClientClick()
    {
        NetworkManager.Singleton.StartClient();
        this.gameObject.SetActive(false);
    }

    private void HostClick()
    {
        NetworkManager.Singleton.StartHost();
        this.gameObject.SetActive(false);
    }
}
