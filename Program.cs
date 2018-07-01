using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Net;
using System.Linq;
using System.Text;
using System.Diagnostics;
using log4net;
using System.Configuration;
using System.IO;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace AsyncSocketServer
{    
    public class Program
    {
        public static ILog Logger;
        public static string FileDirectory;
        
        static void Main(string[] args)
        {
            //DateTime currentTime = DateTime.Now;
            //log4net.GlobalContext.Properties["LogDir"] = currentTime.ToString("yyyyMM");
            //log4net.GlobalContext.Properties["LogFileName"] = "_SocketAsyncServer" + currentTime.ToString("yyyyMMdd");
            //Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //FileDirectory = config.AppSettings.Settings["FileDirectory"].Value;
            //if (FileDirectory == "")
            //    FileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            //if (!Directory.Exists(FileDirectory))
            //    Directory.CreateDirectory(FileDirectory);

            // authorize by remote server
            /*
             int userID = -1;
            if (!(int.TryParse(config.AppSettings.Settings["ServerUserID"].Value, out userID)))
                userID = -1;
            String authorizeURL = "http://222.175.75.230:8081/OperationUser/ashx/SoftUserLoginHandler.ashx?userid=";
            authorizeURL += userID;
            string strMsg = "";
            try
            {
                WebRequest request = WebRequest.Create(authorizeURL);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gb2312"));
                strMsg = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                response.Close();
            }
            catch
            { }  
            if (strMsg == null || strMsg!="true")
            {
                System.Environment.Exit(0);
            }
            */


            String serverIP = "127.0.0.1";
            if (config.AppSettings.Settings["serverIP"].Value != null)
                serverIP = config.AppSettings.Settings["serverIP"].Value;

            int port = 0;
            if (!(int.TryParse(config.AppSettings.Settings["Port"].Value, out port)))
                port = 9999;

            int parallelNum = 0;
            if (!(int.TryParse(config.AppSettings.Settings["ParallelNum"].Value, out parallelNum)))
                parallelNum = 80;

            int sleepTime = 0;
            if (!(int.TryParse(config.AppSettings.Settings["sleepTime"].Value, out sleepTime)))
                sleepTime = 10000;

            int socketTimeOutMS = 0;
            if (!(int.TryParse(config.AppSettings.Settings["SocketTimeOutMS"].Value, out socketTimeOutMS)))
                socketTimeOutMS = 5 * 60 * 1000;

            long start = 0;
            if (!(long.TryParse(config.AppSettings.Settings["start"].Value, out start)))
                start = 0;

              long FilterNumber = 0;
              if (!(long.TryParse(config.AppSettings.Settings["FilterNumber"].Value, out FilterNumber)))
                FilterNumber = 0;
            int count = 1;
            List<DaemonThread> list_DaemonThread = new List<DaemonThread>();
            List<string> list_dtu_id = new List<string>();
            //using (irrigatteEntities entities = new irrigatteEntities())
            //{
            //    try
            //    {
            //        var query = from it in entities.DTU where it.GP_ID == 26 && (it.DTU_ID.StartsWith("811") || it.DTU_ID.StartsWith("900")) select it;
            //        if (query != null && query.FirstOrDefault() != null)
            //        {
            //             count = query.Count();
            //            foreach(var it in query)
            //            {
            //                list_dtu_id.Add(it.DTU_ID);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.ToString());
            //    }
            //}

            for (long i = start; i <start+ parallelNum; i++)
            {
                if (i % 100 == 0  && i > start)
                {
                    Thread.Sleep(sleepTime);
                }
                //DaemonThread DaemonThread = new DaemonThread(serverIP, port, list_dtu_id.ElementAt(i % count));
                DaemonThread DaemonThread = new DaemonThread(serverIP, port, i + "", FilterNumber);
                list_DaemonThread.Add(DaemonThread);
                
            }




            Console.WriteLine(parallelNum + "个线程加载完成。Press any key to terminate the server process....");
            Console.ReadKey();
        }


    }

}