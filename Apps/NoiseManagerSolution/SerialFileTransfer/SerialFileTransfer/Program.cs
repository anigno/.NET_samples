using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialFileCommon;

namespace SerialFileTransfer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileNameRar = "Debug.rar";
            SerialFileTransferPort sender = new SerialFileTransferPort(2);
            SerialFileTransferPort receiver = new SerialFileTransferPort(4);
            MemoryStream ms=new MemoryStream();

            receiver.RegisterReceivedData((p_bytes, p_nReads, p_totalBytes) =>
            {
                Console.WriteLine("Bytes read: {0} {1} / {2}", p_nReads, p_bytes.Length, p_totalBytes);
                ms.Write(p_bytes, 0, p_bytes.Length);
                if (p_bytes.Length >= 2)
                {
                    if (p_bytes[p_bytes.Length - 1] == 0xAA && p_bytes[p_bytes.Length - 2] == 0x55)
                    {
                        MemoryStream msFile=new MemoryStream(ms.ToArray(),0,(int) (ms.Length-2));
                        File.WriteAllBytes(fileNameRar+".new.RAR", msFile.ToArray());
                    }
                }
            });

            //byte[] bytes = new byte[1 * 1000 * 1000];
            //for (int a = 0; a < bytes.Length; a++)
            //{
            //    bytes[a] = 0x7;
            //}
            //File.WriteAllBytes(fileName, bytes);
            byte[] fileReadBytes = File.ReadAllBytes(fileNameRar);

            sender.SendData(fileReadBytes);
            sender.SendData(new byte[]{0x55,0xAA});



            Console.ReadKey();
            return;
        }
    }
}
