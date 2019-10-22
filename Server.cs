using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
   

    private const int MAX_USER = 5;
    private const int PORT = 26000;
    private const int WEBPORT = 26001;
    private const int BYTE_SIZE = 1024; // that size is good enough?

    private byte reliableChannel;
    private int hostId;
    private int webHostId;

    private byte error;

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
        //server only code
        hostId = NetworkTransport.AddHost(topo, PORT, null);
        webHostId = NetworkTransport.AddWebsocketHost(topo, WEBPORT, null);

        Debug.Log(string.Format("Opening conection on port {0} and webport {1}", PORT, WEBPORT));
        isStarted = true;
    }
    private void Update()
    {
        UpdateMessagePump();

    }
    public void UpdateMessagePump()
    {
        if (!isStarted)
            return;
        int recHostId; // Is this from web or Stanlonne?
        int connectionId; //Which user is sendind me this?
        int channelId; // Which lane is sending that message from?
        byte[] recBuffer = new byte[BYTE_SIZE];
        int dataSize;

        NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,BYTE_SIZE, out dataSize, out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Debug.Log(string.Format("User {0} has connected!, trough {1}", connectionId, channelId));
                break;
            case NetworkEventType.DisconnectEvent:
                Debug.Log(string.Format("User {0} has disconnected!", connectionId));
                break;
            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                Net_Msg msg = (Net_Msg)formatter.Deserialize(ms);
                SendClient(msg);
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
    public void SendClient (Net_Msg msg)
    {
        byte[] buffer = new byte[BYTE_SIZE];
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);
        NetworkTransport.Send(hostId, 1, reliableChannel, buffer, BYTE_SIZE, out error);
        NetworkTransport.Send(hostId, 2, reliableChannel, buffer, BYTE_SIZE, out error);
    }



}
