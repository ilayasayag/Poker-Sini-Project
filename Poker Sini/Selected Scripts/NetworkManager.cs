using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Random = System.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{

   public LobbyPhoton LP;
    public const int MAX_PLAYERS = 2;
    Random rnd;
    bool port;

    void Awake()
    {
        LP = FindObjectOfType<LobbyPhoton>();
    }
   public void Connect()
    {
        if(PhotonNetwork.IsConnected)
        {
            //EDITED!!!!!
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { "Arena", LP.CurrArena } }, MAX_PLAYERS );
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }
    #region Photon Callback
    public override void OnConnectedToMaster()
    {
        Debug.Log("Onconnecttomaster");
        PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { "Arena", LP.CurrArena } }, MAX_PLAYERS);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // PhotonNetwork.CreateRoom(null,new RoomOptions() { MaxPlayers = 2 });
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            CustomRoomPropertiesForLobby = new string[] { "Arena" },
            MaxPlayers = MAX_PLAYERS,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Arena", LP.CurrArena } },
        });

    }


    public void SetPlayerArena()
    {
        
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "Arena", LP.CurrArena } });
    }



    public override void OnJoinedRoom()
    {
        LP = FindObjectOfType<LobbyPhoton>();
        Debug.Log($"Player {PhotonNetwork.LocalPlayer.ActorNumber} Joined the room");
        if(PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            LP.OnOnePlayerEntered();
            LP.info.text = "Waiting for second player";

        }
        //if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            LP.ShowReadyToStartUI();
            LP.GetComponent<PhotonView>().RPC("UpdateProfile", RpcTarget.Others, LP.PlayerName.text,LP.PlayerPhoto,LP.CurrArena);
            LP.Player1Portrait.GetComponent<Image>().color = Color.white;
            LP.info.text = "Waiting for the Room Master";
           // LP.StartButtonActive();
        }
            
        //LP.ShowReadyToStartUI();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError($"Player {newPlayer.ActorNumber}");
        LP.ShowReadyToStartUI();
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
            LP.StartButtonActive();
        LP.info.text = "Game Ready";

    }
    #endregion
    void Start()
    {
        rnd = new Random();
        port = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            if(LP.PAName.text != PlayerPrefs.GetString("PlayerName"))
            {
                LP.PAName.text = PlayerPrefs.GetString("PlayerName");
                LP.Player1Portrait.GetComponent<Image>().sprite = LP.Portraits[LP.PlayerPhoto].GetComponent<Image>().sprite;
            }
            if(port)
                StartCoroutine(portR());
            //LP.Player2Portrait.SetActive(false);
            LP.PBName.text = "";

            LP.info.text = "Waiting for an opponent";
        }

            
    }


    public IEnumerator portR()
    {
        port = false;
        LP.Player2Portrait.SetActive(true);
        while (LP.PBName.text == "")
        {
            LP.Player1Portrait.GetComponent<Image>().color = Color.white;
            LP.Player2Portrait.GetComponent<Image>().sprite = LP.Portraits[rnd.Next(LP.Portraits.Length)].GetComponent<Image>().sprite;
            yield return new WaitForSeconds(0.3f);
            LP.Player2Portrait.GetComponent<Image>().color = Color.grey;
            LP.StartRoomButton.SetActive(false);
        }
        LP.Player2Portrait.GetComponent<Image>().sprite = LP.Portraits[LP.forU].GetComponent<Image>().sprite;
        LP.Player2Portrait.GetComponent<Image>().color = Color.white;
        LP.StartRoomButton.SetActive(true);


        port = true;

    }
}
