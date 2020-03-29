using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class DBSyncController : MonoBehaviour
{
    private const int portnum = 42420;

    // Start is called before the first frame update
    void Start()
    {
     try{
         TcpClient a =new TcpClient(IPAddress.Broadcast.ToString(), portnum);
         a.Connect(IPAddress.Any, portnum);
         NetworkStream stream = a.GetStream();
         stream.Write(Encoding.ASCII.GetBytes("test"), 0 , Encoding.ASCII.GetBytes("test").Length);
         stream.Flush();
     }   catch (Exception e){
         //do something
     }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
