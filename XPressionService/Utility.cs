using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XPressionService
{
    public static class Utility
    {
        public struct ColorRGB
        {
            public byte R;
            public byte G;
            public byte B;
            public ColorRGB(Color value)
            {
                this.R = value.R;
                this.G = value.G;
                this.B = value.B;
            }
            public static implicit operator Color(ColorRGB rgb)
            {
                Color c = Color.FromArgb(rgb.R, rgb.G, rgb.B);
                return c;
            }
            public static explicit operator ColorRGB(Color c)
            {
                return new ColorRGB(c);
            }
        }
        // Given H,S,L in range of 0-1
        // Returns a Color (RGB struct) in range of 0-255
        public static ColorRGB HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;

                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            ColorRGB rgb;
            rgb.R = Convert.ToByte(r * 255.0f);
            rgb.G = Convert.ToByte(g * 255.0f);
            rgb.B = Convert.ToByte(b * 255.0f);
            return rgb;
        }
        public static void RGB2HSL(ColorRGB rgb, out double h, out double s, out double l)
        {
            double r = rgb.R / 255.0;
            double g = rgb.G / 255.0;
            double b = rgb.B / 255.0;
            double v;
            double m;
            double vm;
            double r2, g2, b2;

            h = 0; // default to black
            s = 0;
            l = 0;
            v = Math.Max(r, g);
            v = Math.Max(v, b);
            m = Math.Min(r, g);
            m = Math.Min(m, b);
            l = (m + v) / 2.0;
            if (l <= 0.0)
            {
                return;
            }
            vm = v - m;
            s = vm;
            if (s > 0.0)
            {
                s /= (l <= 0.5) ? (v + m) : (2.0 - v - m);
            }
            else
            {
                return;
            }
            r2 = (v - r) / vm;
            g2 = (v - g) / vm;
            b2 = (v - b) / vm;
            if (r == v)
            {
                h = (g == m ? 5.0 + b2 : 1.0 - g2);
            }
            else if (g == v)
            {
                h = (b == m ? 1.0 + r2 : 3.0 - b2);
            }
            else
            {
                h = (r == m ? 3.0 + g2 : 5.0 - r2);
            }
            h /= 6.0;
        }
        public static string Ordinal(int num)
        {
            var suff = "";
            var one = num % 10;
            var ten = Math.Floor((decimal)(num / 10)) % 10;
            if (ten == 1)
            {
                suff = "th";
            }
            else
            {
                switch (one)
                {
                    case 1: suff = "st"; break;
                    case 2: suff = "nd"; break;
                    case 3: suff = "rd"; break;
                    default: suff = "th"; break;
                }
            }
            return num + suff;
        }
        public delegate void WriteFileinfo(FileInfo file, string root);
        public static void RecurseDirectory(string current, string root, WriteFileinfo fileAction)
        {
            foreach (string file in Directory.GetFiles(current))
            {
                try
                {
                    fileAction(new FileInfo(file), root);
                }
                catch
                {
                    // swallow
                }
            }
            foreach (string subDir in Directory.GetDirectories(current))
            {
                try
                {
                    RecurseDirectory(subDir, root, fileAction);
                }
                catch
                {
                    // swallow
                }
            }
        }
        public static List<FileInfo> RecurseDirectory(string current, string root, List<FileInfo> files)
        {
            foreach (string file in Directory.GetFiles(current, "*.*"))
            {
                try
                {
                    files.Add(new FileInfo(file));
                }
                catch
                {
                    // swallow
                }
            }
            foreach (string subDir in Directory.GetDirectories(current))
            {
                try
                {
                    RecurseDirectory(subDir, root, files);
                }
                catch
                {
                    // swallow
                }
            }
            return files;
        }
        public static List<DirectoryInfo> GetDirectoryNames(string source)
        {
            List<DirectoryInfo> directoryInfos = new List<DirectoryInfo>();
            RecurseDirectoryNames(source, directoryInfos);
            return directoryInfos;
        }

        private static void RecurseDirectoryNames(string current, List<DirectoryInfo> directoryList)
        {
            foreach (string directory in Directory.GetDirectories(current))
            {
                try
                {
                    directoryList.Add(new DirectoryInfo(directory));
                    RecurseDirectoryNames(directory, directoryList);
                }
                catch
                {
                    Console.WriteLine("Can't Access Directory.");
                }

            }
        }

        public static string CalculateMD5Hash(string input)

        {

            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();

        }
        public static void SaveToXml(string location, object ingest)
        {
            XmlSerializer writer = new XmlSerializer(ingest.GetType());
            StreamWriter file = new StreamWriter(location);
            writer.Serialize(file, ingest);
            file.Close();
        }
        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
        public static object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);
            return obj;
        }

        public static object ReadXML(string location, Type objecttype)
        {
            XmlSerializer serializer = new XmlSerializer(objecttype);
            using (StreamReader file = new StreamReader(location))
            {
                return serializer.Deserialize(file);
            }

        }

        public delegate void ReportByte(long total, long copy);
        internal static bool CopySource(string sourcefile, string copydirectory, ReportByte report, int buffersize = 16384)
        {
            if (!File.Exists(sourcefile))
            {
                return false;
            }
            try
            {
                string copy = Path.Combine(copydirectory, new FileInfo(sourcefile).Name);

                long total = new FileInfo(sourcefile).Length;
               // long copyl = 0;
                FileStream fs1 = new FileStream(sourcefile, FileMode.Open, FileAccess.Read, FileShare.Read, buffersize);

                fs1.Seek(0, SeekOrigin.Begin);

                CopySection(fs1, copy, (int)total, buffersize, report);

                // Close the files.
                fs1.Close();


                return new FileInfo(sourcefile).Length == new FileInfo(copy).Length;
            }
            catch
            {
                return false;
            }

        }
        private static void CopySection(Stream input, string targetFile, int length, int buffersize, ReportByte report)
        {
            byte[] buffer = new byte[buffersize];
            int total = length;

            using (Stream output = File.OpenWrite(targetFile))
            {
                int bytesRead = 1;
                int totalBytes = 0;
                // This will finish silently if we couldn't read "length" bytes.
                // An alternative would be to throw an exception
                while (length > 0 && bytesRead > 0)
                {
                    int fil = Math.Min(length, buffer.Length);
                    bytesRead = input.Read(buffer, 0, fil);
                    output.Write(buffer, 0, bytesRead);
                    length -= bytesRead;

                    totalBytes += fil;
                    report(total, totalBytes);
                }
            }
        }
    }
}
