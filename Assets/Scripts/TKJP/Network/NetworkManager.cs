using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace TKJP.Network {
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager instance { get; private set; }

        public ConnectionState state { get; private set; }
        public bool autoJoinLobby = true;

        //------------------------------
        //public関数
        //------------------------------
        public void JoinPairRoom(string roomName)
        {
            var option = new RoomOptions();
            option.MaxPlayers = 2;

            PhotonNetwork.JoinOrCreateRoom(roomName, option, TypedLobby.Default);
        }
        public void LeaveRoom()
        {
            if (!PhotonNetwork.InRoom) return;

            PhotonNetwork.LeaveRoom();
        }
        public string GetRoomName()
        {
            return PhotonNetwork.InRoom?PhotonNetwork.CurrentRoom.Name:"";
        }
        //------------------------------
        //private関数
        //------------------------------
        //trialCount回 waitTime秒毎に接続を試みる
        private IEnumerator TryConnect(int trialCount, float waitTime)
        {
            state = ConnectionState.Wait;
            var wait = new WaitForSeconds(waitTime);

            for (int i = 0; i < trialCount; i++)
            {
                PhotonNetwork.ConnectUsingSettings();

                if (PhotonNetwork.IsConnected)
                {
                    state = ConnectionState.Connect;
                    break;
                }
                yield return wait;
            }

            if (!PhotonNetwork.IsConnected) state = ConnectionState.Failed;
        }

        //------------------------------
        //コールバック関数
        //------------------------------
        private void Awake()
        {
            if (instance)
            {
                Destroy(this.gameObject);
                DestroyImmediate(this);
            }

            instance = this;
            DontDestroyOnLoad(this);
        }
        private void Start()
        {
            StartCoroutine(TryConnect(5, 0.5f));
        }
        private void OnDestroy()
        {
            if (ReferenceEquals(instance, this))
            {
                if (PhotonNetwork.InRoom) PhotonNetwork.LeaveRoom();
                if (PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
                if (PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
                //onDisconnect等は同一フレーム内で処理する訳では無いので注意

                instance = null;
            }
        }
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("photonサーバーと繋がったよ");
            if (autoJoinLobby) PhotonNetwork.JoinLobby();
        }
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("ロビーに入ったよ");
        }
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log(PhotonNetwork.CurrentRoom+"に入ったよ");
        }
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            Debug.Log("部屋から出たよ");
        }
        public override void OnLeftLobby()
        {
            base.OnLeftLobby();
            Debug.Log("ロビーから出たよ");
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            switch (cause)
            {
                case DisconnectCause.ExceptionOnConnect:
                    //公式 said that The server is not available or the address is wrong.
                    //俺 said that wi-fiがoffで失敗したときとかもここだよ.
                    Debug.LogError("photonサーバーにアクセスできなかったよ");
                    break;
                case DisconnectCause.Exception:
                    Debug.LogError("photonサーバとの接続が切れたよ");
                    break;
                case DisconnectCause.DisconnectByClientLogic:
                    Debug.Log("photonサーバーとの接続を切ったよ");
                    break;
                case DisconnectCause.DisconnectByServerLogic:
                    Debug.Log("photonサーバーとの接続を切られたよ");
                    break;
                default:
                    Debug.Log(cause);
                    break;
            }
        }
    }

    public enum ConnectionState
    {
        None,
        Wait,
        Connect,
        Failed,
    }
    public struct Room
    {
        public string name;

        public Room(string name)
        {
            this.name = name;
        }
    }
}
