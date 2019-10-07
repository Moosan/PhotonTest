using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TKJP.Test
{
    public class PhotonTest : MonoBehaviour
    {
        public GameObject lobbyCanvas;
        public InputField roomNameInput;
        public GameObject roomCanvas;
        public Text roomNameText;

        Network.NetworkManager manager;

        // Start is called before the first frame update
        void Start()
        {
            manager = Network.NetworkManager.instance;
        }

        public void JoinRoom()
        {
            if (string.IsNullOrWhiteSpace(roomNameInput.text)) return;

            manager.JoinPairRoom(roomNameInput.text);

            roomCanvas.SetActive(true);
            roomNameText.text = "Room:"+roomNameInput.text;
            lobbyCanvas.SetActive(false);
        }
        public void LeaveRoom()
        {
            manager.LeaveRoom();
            lobbyCanvas.SetActive(true);
            roomNameInput.text = "";
            roomCanvas.SetActive(false);
        }
    }
}
