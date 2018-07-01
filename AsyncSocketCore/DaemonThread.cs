using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;


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
        //byte[] sendBytes1 = new byte[8];
        //byte[] sendBytes2 = new byte[6];
        //byte[] sendBytes3 = new byte[6];
        int cout = 0;

        public DaemonThread()
        {
            

        }



        public DaemonThread(string serverIP, int port, string buffer,long filterNumber)
        {
            this.serverIP = serverIP;
            this.port = port;
            this.buffer = buffer;
            this.filter = filterNumber;
            m_thread = new Thread(DaemonThreadStart);
            //int sleepTime = 500;
            //Thread.Sleep(sleepTime*m_thread.GetHashCode());
            //connect();

            commandList.Add(new byte[] { 1,1, 0, 0, 0, 4, 61, 201 });
            commandList.Add(new byte[] { 1, 1, 0, 0, 0, 4, 61, 201 });
            commandList.Add(new byte[] { 1, 2  , 0, 0, 0, 4, 121, 201 });
            commandList.Add(new byte[] { 1, 2, 0, 0, 0, 4, 121, 201 });
            commandList.Add(new byte[] { 1, 3, 0, 0, 0, 4, 68, 09 });
            commandList.Add(new byte[] { 1, 3, 0, 0, 0, 4, 68, 09 });
            commandList.Add(new byte[] { 1, 4, 0, 0, 0, 4, 241, 201 });
            commandList.Add(new byte[] { 1, 4, 0, 0, 0, 4, 241, 201 });
            commandList.Add(new byte[] { 1, 5, 0, 0, 0, 4, 121, 201 });
            commandList.Add(new byte[] { 1, 5, 0, 0, 0, 4, 121, 201 });
            commandList.Add(new byte[] { 2, 1, 0, 0, 0, 4, 61, 250 });
            commandList.Add(new byte[] { 2, 1, 0, 0, 0, 4, 61, 250 });
            commandList.Add(new byte[] { 2, 2, 0, 0, 0, 4, 121, 250 });
            commandList.Add(new byte[] { 2, 2, 0, 0, 0, 4, 121, 250 });
            commandList.Add(new byte[] { 2, 3, 0, 0, 0, 4, 68, 58 });
            commandList.Add(new byte[] { 2, 3, 0, 0, 0, 4, 68, 58 });
            commandList.Add(new byte[] { 2, 4, 0, 0, 0, 4, 241, 250 });
            commandList.Add(new byte[] { 2, 4, 0, 0, 0, 4, 241, 250 });
            commandList.Add(new byte[] { 1, 5, 0, 0, 0, 4, 121, 201 });
            commandList.Add(new byte[] { 1, 5, 1, 15, 225, 140 });
            commandList.Add(new byte[] { 2, 1, 0, 3, 0, 12, 181, 252 });
            commandList.Add(new byte[] { 2, 1, 24, 0, 0, 27, 77, 29, 243, 25, 135, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 8 });
            commandList.Add(new byte[] { 2, 2, 0, 3, 0, 12, 181, 252 });
            commandList.Add(new byte[] { 2, 2, 24, 0, 0, 27, 77, 29, 243, 25, 135, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 8 });
            commandList.Add(new byte[] { 2, 3, 0, 3, 0, 12, 181, 252 });
            commandList.Add(new byte[] { 2, 3, 24, 0, 0, 27, 77, 29, 243, 25, 135, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 8 });
            commandList.Add(new byte[] { 2, 4, 0, 3, 0, 12, 181, 252 });
            commandList.Add(new byte[] { 2, 4, 24, 0, 0, 27, 77, 29, 243, 25, 135, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 200, 8 });
            commandList.Add(new byte[] { 3, 1, 0, 0, 0, 10, 196, 47 });
            commandList.Add(new byte[] { 3, 1, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19, 174, 68, 191, 9, 241, 0, 0, 124, 61 });
            commandList.Add(new byte[] { 3, 2, 0, 124, 0, 4, 132, 51 });
            commandList.Add(new byte[] { 3, 2, 8, 0, 0, 0, 0, 236, 184, 68, 17, 217, 214 });
            commandList.Add(new byte[] { 3, 3, 0, 0, 0, 9, 133, 85 });
            commandList.Add(new byte[] { 3, 3, 18, 0, 0, 0, 0, 38, 115, 0, 4, 0, 4, 0, 5, 1, 98, 0, 0, 0, 133, 189, 68 });

            //sendBytes1[0] = 1;
            //sendBytes1[1] = 5;
            //sendBytes1[2] = 0;
            //sendBytes1[3] = 0;
            //sendBytes1[4] = 0;
            //sendBytes1[5] = 0;
            //sendBytes1[6] = 205;
            //sendBytes1[7] = 202;


            //sendBytes2[0] = 1;
            //sendBytes2[1] = 1;
            //sendBytes2[2] = 1;
            //sendBytes2[3] = 0;
            //sendBytes2[4] = 81;
            //sendBytes2[5] = 136;


            //sendBytes3[0] = 1;
            //sendBytes3[1] = 2;
            //sendBytes3[2] = 1;
            //sendBytes3[3] = 14;
            //sendBytes3[4] = 32;
            //sendBytes3[5] = 76;

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
                recStr +=""+ recBytes[0] + recBytes[1]+ recBytes[2];
                if (m_thread.GetHashCode() % filter == 0)
                {
                    Console.WriteLine(recStr);
                }
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("exception:"+ex.ToString());
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
                if(clientSocket.Connected)
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
                               
                                    Console.WriteLine(DateTime.Now.ToString()+ "  线程" + m_thread.GetHashCode() + "—" + recBytes[0] + " " + recBytes[1] + " " + recBytes[2] + "—-----" + (++cout));
                                
                            }

                            //Console.WriteLine(DateTime.Now.ToString() + "--线程" + m_thread.GetHashCode() + "—" + recBytes[0] + " " + recBytes[1] + " " + recBytes[2] + "          " + (ii++));
                            clientSocket.Send(recBytes);
                            //for (int i = 0; i < commandList.Count(); i++,i++)
                            //{
                            //    bool bl = true;
                            //    for(int j = 0; j < commandList[i].Length && j < recBytes.Length; j++)
                            //    {
                            //        if(commandList[i][j] != recBytes[j])
                            //        {
                            //            bl = false;
                            //            break;
                            //        }
                            //    }
                            //    if(bl)
                            //    {
                            //        if(i + 1 < commandList.Count())
                            //        {
                            //            Thread.Sleep(300);
                            //            clientSocket.Send(commandList[i+1]);
                            //        }
                            //        break;
                            //    }
                            //}

                            //if (bytes > 3 && recBytes[0] == 1 && recBytes[1] == 5)
                            //{

                            //    clientSocket.Send(sendBytes1);
                            //}

                            //if (bytes > 3 && recBytes[0] == 1 && recBytes[1] == 1)
                            //{

                            //    clientSocket.Send(sendBytes2);
                            //}

                            //if (bytes > 3 && recBytes[0] == 1 && recBytes[1] == 2)
                            //{

                            //    clientSocket.Send(sendBytes3);
                            //}
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

        public void Close()
        {
            m_thread.Abort();
            m_thread.Join();
        }
    }
}
