using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Network = Photon.Pun.PhotonNetwork;
public class DemoNetwork : MonoBehaviourPunCallbacks
{
    private bool IsHost { get
        {
            return Network.IsMasterClient;
        }
    }
    private ClientTransform masterTransform = new ClientTransform(Vector3.back * 5, Quaternion.identity);
    private ClientTransform subTransform = new ClientTransform(Vector3.forward * 5, Quaternion.Euler(Vector3.up * 180));
    private GameObject Body;

    public void Update()
    {
        if (Input.touchCount > 0 || Input.GetKey(KeyCode.Space))
        {
            if (Network.IsConnected) return;
            Connect();
        }
    }

    public void Connect()
    {
        Network.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Network.JoinOrCreateRoom("room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Body = Network.Instantiate("GamePlayer",Vector3.zero,Quaternion.identity);
        SetClientTransform(IsHost ? masterTransform : subTransform);

        StartCoroutine(GyroRotate());
    }
    
    public void SetClientTransform(ClientTransform transformState)
    {
        transform.SetPositionAndRotation(transformState.Pos,transformState.Rot);
        Body.transform.SetPositionAndRotation(transformState.Pos,transformState.Rot);
    }

    IEnumerator GyroRotate()
    {
#if UNITY_EDITOR
        Body.GetComponent<MeshRenderer>().material.color = Color.red;
        yield break;
#endif

        Input.gyro.enabled = true;
        
        while (Network.IsConnected)
        {
            var rot = Quaternion.Euler(0, 0, -180) * Quaternion.Euler(-90, 0, 0) * Input.gyro.attitude * Quaternion.Euler(0, 0, 180);
            transform.rotation = rot;
            Body.transform.rotation = rot;
            yield return null;
        }
        Input.gyro.enabled = false;
    }
}

public struct ClientTransform
{
    public Vector3 Pos;
    public Quaternion Rot;
    public ClientTransform(Vector3 pos,Quaternion rot)
    {
        Pos = pos;
        Rot = rot;
    }
}
