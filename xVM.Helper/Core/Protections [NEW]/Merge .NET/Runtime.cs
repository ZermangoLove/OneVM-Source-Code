using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;

namespace xVM.Helper.Core.Protections
{
    internal static class MergeNET_Runtime
	{
        static void Initialize()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                foreach (var resName in ReadDLList())
                {
                    if (!string.IsNullOrEmpty(resName))
                        using (var stream = typeof(MergeNET_Runtime).Assembly.GetManifestResourceStream(resName))
                        {
                            byte[] Data = new byte[stream.Length];
                            stream.Read(Data, 0, Data.Length);

                            return Assembly.Load(Decompress(Data));
                        }
                }
            }
            catch { return null; }
            return null;
        }

        private static string[] ReadDLList()
        {
            string[] list = null;
            using (var stream = typeof(MergeNET_Runtime).Assembly.GetManifestResourceStream("__xMerge__List.resources"))
            {
                byte[] Data = new byte[stream.Length];
                stream.Read(Data, 0, Data.Length);

                list = Encoding.Default.GetString(Decompress(Data)).Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            }
            return list;
        }

        private static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (GZipStream dstream = new GZipStream(input, CompressionMode.Decompress))
            {
                CopyTo(dstream, output);
            }
            return output.ToArray();
        }

        private static void CopyTo(Stream source, Stream destination)
        {
            byte[] array = new byte[81920];
            int count;
            while ((count = source.Read(array, 0, array.Length)) != 0)
            {
                destination.Write(array, 0, count);
            }
        }
    }
}
