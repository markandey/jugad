using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace StructureToString
{
    class Program
    {
        struct Payload
        {
            public int a;
            public int b;
        }
        struct Packet
        {
            public char a;
            public int b;
            public float c;
            public Payload s;
        }
        static public string SerializeBuffer(object obj, ref int size)
        {
            size = Marshal.SizeOf(obj.GetType());
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, true);
            byte[] byteArray = new byte[size];
            string encode_qp = "";
            for (int i = 0; i < size; ++i)
            {
                byte b = (byte)Marshal.ReadByte(ptr, i);
                encode_qp += "=" + b.ToString("X2");
            }
            Marshal.FreeHGlobal(ptr);
            return encode_qp;
        }
        static public void GetDeSerializedBuffer(ref object Obj, ref string QPData)
        {
            int size = Marshal.SizeOf(Obj.GetType());
            IntPtr ptr = Marshal.AllocHGlobal(size);
            for (int i = 0; i < size; i++)
            {
                Marshal.WriteByte(ptr, i, ReadByteFromQPString(ref QPData));
            }
            Obj = Marshal.PtrToStructure(ptr, Obj.GetType());
            Marshal.FreeHGlobal(ptr);
        }
        static byte GetByteValueFromChar(char c)
        {
            if (char.IsDigit(c))
            {
                return (byte)(c - '0');
            }
            else if (char.IsLetter(c))
            {
                char cc = char.ToUpper(c);
                return (byte)(10 + cc - 'A');
            }
            else
            {
                return (byte)0;
            }
        }
        private static byte GetByte(string bytestr)
        {
            if (bytestr.Length == 0)
            {
                return (byte)0;
            }
            if (bytestr.Length == 1)
            {
                return GetByteValueFromChar(bytestr[0]);
            }
            if (bytestr.Length >= 2)
            {
                return (byte)(16 * GetByteValueFromChar(bytestr[0]) + GetByteValueFromChar(bytestr[1]));
            }
            return 0;
        }
        static private byte ReadByteFromQPString(ref string qp_string)
        {
            string byte_str = "";
            if (qp_string.Length == 0)
            {
                throw new Exception("byte can not be read");
            }
            if (qp_string[0] == '=')
            {
                while (true)
                {
                    if (qp_string.Length > 1 && qp_string[1] != '=')
                    {
                        byte_str += qp_string[1];
                    }
                    else
                    {
                        break;
                    }
                    if (qp_string.Length > 2 && qp_string[2] != '=')
                    {
                        byte_str += qp_string[2];

                    }
                    break;
                }
                qp_string = qp_string.Substring(byte_str.Length + 1);
                return GetByte(byte_str);
            }
            else
            {
                byte byte_val = (byte)qp_string[0];
                qp_string = qp_string.Substring(1);
                return byte_val;
            }
        }
        public static void WriteToFile(string data)
        {
            StreamWriter wr = new StreamWriter("test.txt");
            wr.Write(data);
            wr.Close();
        }
        public static string ReadFromFile()
        {
            StreamReader rd = new StreamReader("test.txt");
            string data=rd.ReadToEnd();
            rd.Close();
            return data;
        }
        static void Main(string[] args)
        {
            Packet bData1;
            bData1.a = 'c';
            bData1.b = 20;
            bData1.c = 30f;
            bData1.s.a = 10;
            bData1.s.b = 20;

            Packet bData2;
            bData2.a = '\0';
            bData2.b = 0;
            bData2.c = 0f;
            bData2.s.a = 0;
            bData2.s.b = 0;


            int size = 0;
            string SerializedBuffer=SerializeBuffer(bData1, ref size);
            Console.Out.WriteLine("size=" + size+"\n");

            WriteToFile(SerializedBuffer);

            string DataReadFromFile = ReadFromFile();
            object obj = (object)bData2;
            GetDeSerializedBuffer(ref obj, ref DataReadFromFile);
            bData2 = (Packet)obj;
            
            Console.Out.WriteLine("bData2.a=" + bData2.a + "\n");
            Console.Out.WriteLine(" bData2.b=" + bData2.b + "\n");
            Console.Out.WriteLine("bData2.c=" + bData2.c + "\n");
            Console.Out.WriteLine("bData2.s.a=" + bData2.s.a + "\n");
            Console.Out.WriteLine("bData2.s.b=" + bData2.s.b + "\n");
        }
    }
}
