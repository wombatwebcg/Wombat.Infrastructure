using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wombat.Infrastructure
{
   public class CompressHelper
    {
        public static string GZipDecompressString(byte[] compressedData)
        {
            using (var memoryStream = new MemoryStream(compressedData))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(resultStream);
                        byte[] decompressedData = resultStream.ToArray();
                        return Encoding.UTF8.GetString(decompressedData);
                    }
                }
            }
        }

        public static byte[] GZipCompressString(string data)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(data);

            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(byteArray, 0, byteArray.Length);
                }
                return memoryStream.ToArray();
            }

        }

    }
}
