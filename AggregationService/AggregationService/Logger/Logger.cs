using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AggregationService.Logger
{
    public static class Logger
    {
        private static string path = "Logs.txt";

        public static async Task LogQuery(string request, string response, byte[] ResponseMessage)
        {
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);

            StreamWriter sw;
            
            if (!File.Exists(path))
            {
                // Create a file to write to.
                sw = File.CreateText(path);
            }
            else
            {
                sw = File.AppendText(path);
            }

            await Task.Run(() =>
            {
                sw.WriteLine(string.Format("{0} - Request: {1}\r\n", corrId, request));
                sw.WriteLine(string.Format("{0} - Response: {1}\r\n{2}", corrId, response, Encoding.UTF8.GetString(ResponseMessage)));
                sw.WriteLine("\r\n\r\n");
            });

            //Close the file
            sw.Close();
        }

        public static async Task LogQuery(string request, string RequestMessage, string response, byte[] ResponseMessage)
        {
            var corrId = string.Format("{0}{1}", DateTime.Now.Ticks, Thread.CurrentThread.ManagedThreadId);

            StreamWriter sw;

            if (!File.Exists(path))
            {
                // Create a file to write to.
                sw = File.CreateText(path);
            }
            else
            {
                sw = File.AppendText(path);
            }

            await Task.Run(() =>
            {
                sw.WriteLine(string.Format("{0} - Request: {1}{2}\r\n", corrId, request, RequestMessage));
                sw.WriteLine(string.Format("{0} - Response: {1}\r\n{2}", corrId, response, Encoding.UTF8.GetString(ResponseMessage)));
                sw.WriteLine("\r\n\r\n");
            });

            //Close the file
            sw.Close();
        }
    }
}
