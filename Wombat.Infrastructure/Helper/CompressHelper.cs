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
            try
            {
                if (compressedData == null || compressedData.Length < 2 || !IsValidGZip(compressedData))
                    return string.Empty; // 无效数据直接返回空字符串

                using (var memoryStream = new MemoryStream(compressedData))
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    while ((bytesRead = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        resultStream.Write(buffer, 0, bytesRead);
                    }
                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
            catch
            {
                return string.Empty; // 遇到任何异常时返回空字符串
            }
        }

        public static byte[] GZipCompressString(string data)
        {
            if (string.IsNullOrEmpty(data))
                return Array.Empty<byte>(); // 空字符串直接返回空字节数组

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                using (var memoryStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
                    {
                        gzipStream.Write(byteArray, 0, byteArray.Length);
                    }
                    return memoryStream.ToArray();
                }
            }
            catch
            {
                return Array.Empty<byte>(); // 异常时返回空字节数组
            }
        }

        private static bool IsValidGZip(byte[] data)
        {
            // 检查数据是否符合 GZip 的魔数规则
            return data.Length > 2 && data[0] == 0x1F && data[1] == 0x8B;
        }
    }
}
