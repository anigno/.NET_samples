using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialFileCommon;

namespace SerialFileSender
{
    class Program
    {
        static void Main(string[] p_args)
        {
            uint portNum = uint.Parse(p_args[0]);
            SerialFileTransferPort sender = new SerialFileTransferPort(portNum);
            byte[] fileReadBytes = File.ReadAllBytes(p_args[1]);
            sender.SendData(fileReadBytes);
            sender.SendData(new byte[] { 0x55, 0xAA });
            Console.WriteLine("COM{0} {1} Done",p_args[0],p_args[1]);
            Console.ReadKey();
        }
    }
}
