using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PUNManager : MonoBehaviourPunCallbacks
{
    //[SerializeField]
    //private Text myArmorUI;
    //[SerializeField]
    //private Text enemyArmorUI;
    public Text statusText;
    public string carP1;
    public string carP2;
    private TypedLobby lobbyInfo;
    private int roomNo;
    // Start is called before the first frame update
    void Start()
    {
        //GameVersionを指定
        PhotonNetwork.GameVersion = "0.1";
        //1秒間に送信するパケットの数
        PhotonNetwork.SendRate = 10;
        PhotonNetwork.SerializationRate = 10;
        PhotonNetwork.NetworkingClient.AppId =
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームルーム内にいるかを判定
        if (!PhotonNetwork.InRoom)
        {
            //ルーム内でなければ、接続の情報を表示
            statusText.text = PhotonNetwork.NetworkClientState.ToString();
        }
        else
        {
            //そうでなければルームへの接続情報を表示
            statusText.text = "JoinedRoom:" + PhotonNetwork.CurrentRoom.Name + "\n" +
                "JoinedPlayerNum:" + PhotonNetwork.CurrentRoom.PlayerCount + "\n" +
                "IsMaster:" + PhotonNetwork.IsMasterClient
                ;
        }

        //リージョンの表示
        if (PhotonNetwork.CloudRegion != null)
        { statusText.text += "\n" + PhotonNetwork.CloudRegion; }

        //Lobbyに接続している状態で、Spaceキーが押されたらRoomに接続する。
        if(PhotonNetwork.NetworkClientState==ClientState.JoinedLobby&&
            Input.GetKeyDown(KeyCode.Space))
        {
            JoinOrCreateRoom();
        }
    }

    public void SetMyAmorText(string text)
    {
        //myArmorUI.text = text;
    }

    public void SetEnemyArmorText(string text)
    {
        //enemyArmorUI.text = text;
    }


    //PhotonNetworkに接続出来たら
    public override void OnConnectedToMaster()
    {
        //接続出来たらロビーに接続開始
        Debug.Log("OnConnectedToMaster");
        PhotonNetwork.JoinLobby(lobbyInfo);
    }

    //lobbyに接続出来たら
    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby\nJoinRoom: Push Space Key");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JoinOrCreateRoom();
        }
    }

    //Roomへの接続が失敗したら
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("JoinRoomFailed. Code:" + returnCode + ".Message:" + message);
        roomNo++;
        JoinOrCreateRoom();
    }

    //Roomに接続出来たら
    public override void OnJoinedRoom()
    {
        Debug.Log("JoinedRoom@" + PhotonNetwork.CurrentRoom.Name);
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                transform.position = new Vector3(5, 0, 0);
                GameObject car1 = PhotonNetwork.Instantiate(carP1, transform.position, Quaternion.identity);
            }
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                transform.position = new Vector3(-5, 0, 0);
                GameObject car2 = PhotonNetwork.Instantiate(carP2, transform.position, Quaternion.identity);
            }
    }

    //Roomに接続、もしくは作成する
    private void JoinOrCreateRoom()
    {
        Debug.Log("CreateRoom RoomNo:" + roomNo);
        //部屋のオプション
        RoomOptions roomOptions = new RoomOptions();

        //最大人数は２人
        roomOptions.MaxPlayers = 2;

        //指定した部屋名に接続する。もしくは上のオプションを使って部屋を作成する
        PhotonNetwork.JoinOrCreateRoom("Room" + roomNo, roomOptions, TypedLobby.Default);
    }
}
