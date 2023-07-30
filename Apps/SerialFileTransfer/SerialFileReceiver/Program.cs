using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialFileCommon;

namespace SerialFileReceiver
{
    class Program
    {
        static void Main(string[] p_args)
        {
            uint portNum = uint.Parse(p_args[0]);
            SerialFileTransferPort receiver = new SerialFileTransferPort(portNum);
            MemoryStream ms = new MemoryStream();

            receiver.RegisterReceivedData((p_bytes, p_nReads, p_totalBytes) =>
            {
                Console.WriteLine("Bytes read: {0} {1} / {2}", p_nReads, p_bytes.Length, p_totalBytes);
                ms.Write(p_bytes, 0, p_bytes.Length);
                if (p_bytes.Length >= 2)
                {
                    if (p_bytes[p_bytes.Length - 1] == 0xAA && p_bytes[p_bytes.Length - 2] == 0x55)
                    {
                        MemoryStream msFile = new MemoryStream(ms.ToArray(), 0, (int)(ms.Length - 2));
                        File.WriteAllBytes(p_args[1] , msFile.ToArray());
                    }
                }
            });

            Console.WriteLine("Waiting");
            Console.ReadKey();
        }
    }
}
