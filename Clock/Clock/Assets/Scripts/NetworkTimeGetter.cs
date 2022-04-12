using UnityEngine;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
//      "ntp1.stratum2.ru",
//      "ntp4.stratum2.ru"
public class NetworkTimeGetter
{
    private readonly string[] _serverList;
    public NetworkTimeGetter(string[] serverList) => _serverList = serverList;

    public DateTime Get(){
        DateTime time = new DateTime();
        bool connectionSuccess = false;

        foreach (var server in _serverList){
            if (GetNetworkTime(server, out time)){
                connectionSuccess = true;
                break;
            }
        }
       
        Debug.Log("Connection to server success " + connectionSuccess);

        return time.ToLocalTime();
    }

    private bool GetNetworkTime(string ntpServer, out DateTime time){
        try{
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B;

            IPAddress[] addresses = Dns.GetHostEntry(ntpServer).AddressList;
            IPEndPoint ipEndPoint = new IPEndPoint(addresses[0], 123);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            ulong intPart = (ulong) ntpData[40] << 24 | (ulong) ntpData[41] << 16 | (ulong) ntpData[42] << 8 | ntpData[43];
            ulong fractPart = (ulong) ntpData[44] << 24 | (ulong) ntpData[45] << 16 | (ulong) ntpData[46] << 8 | ntpData[47];

            ulong milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            DateTime networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long) milliseconds);

            time = networkDateTime;
            return true;
        }
        catch (Exception){
            time = DateTime.Now;
            return false;
        }
    }
}
