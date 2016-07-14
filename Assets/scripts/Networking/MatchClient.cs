﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VGDC_RPG.Networking
{
    public static class MatchClient
    {
        private static NetClient client;

        public static void Init()
        {
            client = new NetClient();
            client.Init();
            client.DataRecieved += Client_DataRecieved;
        }

        public static void Connect(string ip, int port)
        {
            client.Connect(ip, port);
        }

        private static void Client_DataRecieved(NetConnection connection, NetCodes code, DataReader r)
        {
            switch (code)
            {
                case NetCodes.Clone:
                    NetCloner.HandleClone(r);
                    break;
                case NetCodes.Event:
                    NetEvents.HandleEvent(r);
                    break;
                default:
                    throw new Exception("Invalid Net Code: " + code.ToString());
            }
        }

        public static void Disconnect()
        {
            client.Disconnect();
        }

        public static void Update()
        {
            client.Update();
        }
    }
}
