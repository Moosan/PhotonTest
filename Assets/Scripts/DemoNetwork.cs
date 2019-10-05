using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Network = Photon.Pun.PhotonNetwork;
public class DemoNetwork : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public void Connect()
    {
        Network.ConnectUsingSettings();
        Debug.Log("a");
    }

    public override void OnConnectedToMaster()
    {
        Network.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        var v = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        Network.Instantiate("GamePlayer",v,Quaternion.identity);
    }
    
    public void Update()
    {
        if(Input.touchCount > 0)
        {
            if (Network.IsConnected) return;
            Connect();
        }
    }
}

