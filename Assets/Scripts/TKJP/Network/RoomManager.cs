using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace TKJP.Network
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        //private string playerPath = "Oculus Like Player";
        //------------------------------
        //public関数
        //------------------------------
        //------------------------------
        //private関数
        //------------------------------
        //------------------------------
        //コールバック関数
        //------------------------------
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            //PhotonNetwork.Instantiate(playerPath, Vector3.zero, Quaternion.identity);

            PhotonNetwork.Instantiate("TestPlayer", Vector3.zero, Quaternion.identity);
        }
    }
}
