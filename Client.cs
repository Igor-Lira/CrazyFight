
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Client : MonoBehaviour
{
    private GameObject player;
    private Vector3 pos;

    private const int BYTE_SIZE = 1024;
    private const int MAX_USER = 2;
    private const int PORT = 26000;
    private const int WEBPORT = 26001;
    private const string SERVER_IP = "127.0.0.1"; // PRoduction Envieriment, we need to change this someday
    private byte error;
    private byte reliableChannel;
    private int hostId;
    private int connectionId;
    private bool isStarted;

    #region Monobehaviour 
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Init();

    }
    #endregion
    public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topo = new HostTopology(cc, MAX_USER);
        //Client only code
        hostId = NetworkTransport.AddHost(topo, 0);


#if UNITY_WEBGL && !UNITY_EDITOR
        //Web Client 
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, WEBPORT, 0, out error);
         Debug.Log("Conecting in Web and my number is " + connectionId);

#else
        //Standalone Client 
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log("Conecting from standlone and my number is " + connectionId);
        

#endif
        Debug.Log(string.Format("Conecting {0} ....", SERVER_IP));
        isStarted = true;
        if (connectionId == 1)
        {
            player = GameObject.Find("Player1");
        }
        else
        {
            player = GameObject.Find("Player2");
        }
    }

    private void Update()
    {
        UpdateMessagePump();
        SendPoisition(); // We transmit the postion of one client 

    }
    public void UpdateMessagePump()
    {
        if (!isStarted)
            return;
        int recHostId; // Is this from web or Stanlonne?
        int recConnectionId; //Which user is sendind me this?
        int channelId; // Which lane is sending that message from?
        byte[] recBuffer = new byte[BYTE_SIZE];
        int dataSize;

        
        NetworkEventType type = NetworkTransport.Receive(out recHostId, out recConnectionId, out channelId, recBuffer, BYTE_SIZE, out dataSize, out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log("we have connected to the server");
                //We create enable the player and control him 
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log("We have been disconnected");
                break;
            // This is what the client recives from the Socket/ Host:
            case NetworkEventType.DataEvent:
                MemoryStream ms = new MemoryStream(recBuffer);
                BinaryFormatter formatter = new BinaryFormatter();
                Net_Msg msg = (Net_Msg)formatter.Deserialize(ms);
                //We need to find the gameObject from this other connectionID
                UpdateScene(connectionId, recConnectionId, msg);
                break;
            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }
    }
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }
    #region Send
    public void SendServer(NetMessage msg)
    {
        // This is where we fold our data 
        byte[] buffer = new byte[BYTE_SIZE];
        //This is where we would crush data into a byte
        BinaryFormatter formatter = new BinaryFormatter();
        //We get some space in the memory 
        MemoryStream ms = new MemoryStream(buffer);
        //We transform the mensager in bytes because this is already serialized
        formatter.Serialize(ms, msg);

        //The client send the binary messenger to the server
        NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
    }
    #endregion
    public void SendPoisition()
    {
        pos = player.transform.position;
        Net_Msg msg = new Net_Msg();
        msg.positionX = pos.x;
        msg.positionY = pos.y;
        msg.positionZ = pos.z;
        SendServer(msg);
    }
    public void UpdateScene(int connID, int recConnID, Net_Msg msg)
    {
        GameObject playerUpdate;

        print("Jogador" + connID + " Recebendo dados do jogador " + recConnID);
        if (recConnID == 1)
        {
            playerUpdate = GameObject.Find("Player2");
        }
        else
        {
            playerUpdate = GameObject.Find("Player1");
        }

    }

}

