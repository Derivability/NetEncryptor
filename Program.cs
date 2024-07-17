using System;
using System.Text;
using System.Linq;

namespace NetEncryptor
{
    class Program
    {
        static void Encrypt(string key, byte[] buf)
        {
            byte[] key_bytes = Encoding.ASCII.GetBytes(key);
            byte[] encrypted = new byte[buf.Length];
            for (int i = 0; i < buf.Length; i++)
            {
                encrypted[i] = (byte)(buf[i] ^ key_bytes[i % key_bytes.Length]);
            }
            StringBuilder hex = new StringBuilder(encrypted.Length * 2);
            foreach (byte b in encrypted)
            {
                hex.AppendFormat("0x{0:x2},", b);
            }
            hex.Remove(hex.Length - 1, 1);
            Console.WriteLine($"Encrypted payload length: {encrypted.Length} bytes");
            Console.WriteLine($"Ecnrypted payload: {hex}");
        }

        static string AskKey()
        {
            Console.Write("Enter XOR key: ");
            return Console.ReadLine();
        }

        public static void Main(string[] args)
        {
            byte[] buf = { };
            string key = "";

            if (args.Length <= 1)
            {
                string line;
                string[] strbuf = new string[] { };

                Console.WriteLine("Enter comma separated hex bytes of payload: ");
                while (!string.IsNullOrEmpty(line = Console.ReadLine()))
                {
                    strbuf = strbuf.Concat(line.Split(',').Select(str => str.Trim()).Where(str => !string.IsNullOrWhiteSpace(str)).ToArray()).ToArray();
                }
                buf = new byte[strbuf.Length];
                for (int i = 0; i < strbuf.Length; i++)
                {
                    //Console.WriteLine($"{strbuf[i]} at pos {i}");
                    buf[i] = (byte)Convert.ToInt32(strbuf[i].Replace('\0', ' ').Trim(), 16);
                };
                key = AskKey();
            }
            else if (args.Length == 1)
            {
                buf = System.IO.File.ReadAllBytes(args[0]);
                key = AskKey();
            }
            else if (args.Length == 2)
            {
                buf = System.IO.File.ReadAllBytes(args[0]);
                key = args[1];
            }
            else
            {
                Console.WriteLine("Wrong number of arguments");
                Environment.Exit(1);
            }

            Console.WriteLine($"Raw payload length: {buf.Length}");
            Encrypt(key, buf);

        }
    }
}
