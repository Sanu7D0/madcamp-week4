using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyManager : SingletonP<LobbyManager>
{
    private RoomManager roomManager;
    private LobbyUIManager lobbyUIManager;
    public string selectedCharacterName;

    protected override void Awake()
    {
        base.Awake();

        lobbyUIManager = LobbyUIManager.Instance;
        roomManager = RoomManager.Instance;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        lobbyUIManager.onClickedLeaveButtonListener.AddListener(() => {
            LeaveRoom();
		});
        lobbyUIManager.onCharacterClickedListener.AddListener((name) =>
        {
            selectedCharacterName = name;
            InstantiateCharacter(name);
        });

        if (roomManager.CanStartGame())
        {
            Debug.Log("roomManager.CanStartGame");

            roomManager.Ready();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");

        SceneManager.LoadScene(0);
        Destroy(this.gameObject);
    }

    private void LeaveRoom()
    {
        Debug.Log("LeaveRoom");
        if (!roomManager.CanStartGame())    
        {
            roomManager.Wait();
            Debug.Log("!roomManager.CanStartGame == True");
            return;
        }
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        if (roomManager.CanStartGame())
        {
            roomManager.Ready();
            Debug.Log("roomManager.CanStartGame");
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom");

        if (!roomManager.CanStartGame())
        {
            roomManager.Wait();
            Debug.Log("!roomManager.CanStartGame == True");
            return;
        }

    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        PhotonNetwork.LoadLevel("Room");
    }

    void InstantiateCharacter(string name) {
        Debug.Log("InstanciateLobbyCharacter");

        if (name is null)
        {
            Debug.Log("prefab Name is null");
        }

		PhotonNetwork.Instantiate(name, new Vector3(0, 0), Quaternion.identity, 0);
    }

}
