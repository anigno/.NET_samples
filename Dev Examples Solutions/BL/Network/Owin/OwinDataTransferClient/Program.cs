#region

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OwinDataTransferHost;

#endregion

namespace OwinDataTransferClient
{
    internal class Program
    {
        #region Private Methods

        private static void Main(string[] args)
        {
            DataTransferClient dataTransferClient = new DataTransferClient("http://localhost:8080");
            byte[] buffer = Enumerable.Range(1, 1024*1024*2).Select(p => (byte)p).ToArray();

            dataTransferClient.SendDataData(new DataItem() { Descriptor = "MyData",Label = "12345678901234567890", Data = buffer });
            Console.WriteLine("Data sent");
            Console.ReadLine();
        }

        #endregion
    }
}