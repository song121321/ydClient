using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using YDIOTSvr.YDIOTSvrUtil;


namespace AsyncSocketServer
{
    class DaemonThread : Object
    {
        private Thread m_thread;
        Socket clientSocket;
        string serverIP;
        int port;
        string buffer;
        List<Byte[]> commandList = new List<byte[]>();
        int ii = 0;
        long filter = 0;
        int cout = 0;

        public DaemonThread()
        {


        }



        public DaemonThread(string serverIP, int port, string buffer, long filterNumber)
        {
            this.serverIP = serverIP;
            this.port = port;
            this.buffer = buffer;
            this.filter = filterNumber;
            m_thread = new Thread(DaemonThreadStart);
            commandList.Add(new byte[] { 2, 3, 0, 3, 0, 12, 181, 252 });
            m_thread.Start();

        }
        public bool connect()
        {
            if (m_thread.GetHashCode() % filter == 0)
            {
                Console.WriteLine("线程哈希码==" + m_thread.GetHashCode());
            }
            byte[] RelayCmd = new byte[14];
            RelayCmd[0] = 170;
            RelayCmd[1] = 1;
            RelayCmd[2] = 250;
            RelayCmd[3] = 250;
            RelayCmd[4] = 250;
            RelayCmd[5] = 250;
            RelayCmd[6] = 250;
            RelayCmd[7] = 250;
            RelayCmd[8] = 250;
            RelayCmd[9] = 250;
            RelayCmd[10] = 250;
            RelayCmd[11] = 250;
            RelayCmd[12] = 250;
            RelayCmd[13] = 13;

            for (int i = 0; i < buffer.Length; i++)
            {
                RelayCmd[2 + i] = (byte)buffer.ElementAt(i);
            }

            int sum = 0;
            for (int i = 1; i <= 11; i++)
            {
                sum += RelayCmd[i];
            }
            sum &= 255;
            RelayCmd[12] = (byte)sum;

            try
            {
                IPAddress ip = IPAddress.Parse(serverIP);
                IPEndPoint ipe = new IPEndPoint(ip, port);

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                clientSocket.Connect(ipe);


                clientSocket.Send(RelayCmd);

                //receive message
                string recStr = "";
                byte[] recBytes = new byte[4096];

                int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                recStr += "" + recBytes[0] + recBytes[1] + recBytes[2];
                if (m_thread.GetHashCode() % filter == 0)
                {
                    Console.WriteLine(recStr);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("exception:" + ex.ToString());
                return false;
            }
            return true;
        }

        public void DaemonThreadStart()
        {
            if (this.clientSocket == null)
                connect();
            while (m_thread.IsAlive && this.clientSocket != null)
            {
                if (clientSocket.Connected)
                {
                    try
                    {
                        byte[] recBytes = new byte[8];
                        if (clientSocket != null)
                        {
                            int bytes = 0;
                            try
                            {
                                bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                            }
                            catch (SocketException ex)
                            {
                                Console.WriteLine("exception:2" + ex.ToString());
                            }

                            if (bytes > 0 && m_thread.GetHashCode() % filter == 0)
                            {

                                Console.WriteLine(DateTime.Now.ToString() + "  线程" + m_thread.GetHashCode() + "—" + recBytes[0] + " " + recBytes[1] + " " + recBytes[2] + "—-----" + (++cout));

                            }

                            if (recBytes[1] == 3)
                            {
                                clientSocket.Send(createRecieveData(recBytes));
                            }
                            else {
                                clientSocket.Send(recBytes);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("exception:3" + ex.ToString());
                    }
                }
                else
                {
                    connect();
                }
            }

        }

        private byte[] createRecieveData(byte[] gets)
        {
            byte[] rawGets = new byte[gets.Length - 2];
            for (int j = 0; j < rawGets.Length; j++)
            {
                rawGets[j] = gets[j];
            }

            byte[] sends = new byte[29];
            sends[0] = gets[0];
            sends[1] = gets[1];
            sends[2] = 24;
            for (int i = 0; i < 24; i++)
            {
                sends[3 + i] = 10;
            }
            CRCData crcData = CRCDataCaculate.CRCCaculate(rawGets);
            sends[27] = crcData.CRCLow;
            sends[28] = crcData.CRCHigh;
            return sends;
        }

        public void Close()
        {
            m_thread.Abort();
            m_thread.Join();
        }
    }
}
