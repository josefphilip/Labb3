using System;
using System.Globalization;
using System.IO;

namespace Labb3
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {

                try
                {
                    var fs = new FileStream(args[0], FileMode.Open);
                    var fileSize = (int)fs.Length;
                    var data = new byte[fileSize];
                    fs.Read(data, 0, fileSize);
                    fs.Close();

                    if (PNG(data))
                    {
                        Console.Write($"This is a .png image. ");
                        PNGRes(data);
                        return;
                    }

                    if (BMP(data))
                    {
                        Console.Write("This is a .bmp image. ");
                        BMPRes(data);
                        return;
                    }

                    {
                        Console.WriteLine("This is not a valid .bmp or .png file!");
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found!");
                }
            }
        }

        private static bool PNG(byte[] data)
        {
            var png = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0A, 0x1A, 0x0A };
            for (var pngi = 0; pngi < 7; pngi++)
            {
                if (data[pngi] == png[pngi])
                    return true;
            }
            return false;
        }

        private static bool BMP(byte[] data)
        {
            var bmp = new byte[] { 0x42, 0x4D };
            for (var bmpi = 0; bmpi < 2; bmpi++)
            {
                if (data[bmpi] == bmp[bmpi])
                    return true;
            }
            return false;
        }

        private static void PNGRes(byte[] data)
        {
            string widthHex = string.Empty;
            for (int widthi = 16; widthi < 20; widthi++)
            {
                widthHex += toHex(data, widthi);
            }
            int width = int.Parse(widthHex, NumberStyles.HexNumber);

            string heightHex = string.Empty;
            for (int heighti = 20; heighti < 24; heighti++)
            {
                heightHex += toHex(data, heighti);
            }
            int height = int.Parse(heightHex, NumberStyles.HexNumber);
            Console.WriteLine($"Resolution: {width}x{height} pixels.");
        }

        private static void BMPRes(byte[] data)
        {
            string widthHex = string.Empty;
            for (int widthi = 21; widthi > 17; widthi--)
            {
                widthHex += toHex(data, widthi);
            }
            int width = int.Parse(widthHex, NumberStyles.HexNumber);

            string heightHex = string.Empty;
            for (int heighti = 25; heighti > 21; heighti--)
            {
                heightHex += toHex(data, heighti);
            }
            int height = int.Parse(heightHex, NumberStyles.HexNumber);
            Console.WriteLine($"Resolution: {width}x{height} pixels.");
        }

        private static string toHex(byte[] data, int i) => data[i].ToString("X");
    }
}