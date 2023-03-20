﻿using System;
using System.Linq;
using System.Text;

namespace Wombat.Infrastructure
{
    /// <summary>
    /// 拓展类
    /// </summary>
    public static partial class Extention
    {
        /// <summary>
        /// byte[]转string
        /// 注：默认使用UTF8编码
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// byte[]转string
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <param name="encoding">指定编码</param>
        /// <returns></returns>
        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将byte[]转为Base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 转为二进制字符串
        /// </summary>
        /// <param name="aByte">字节</param>
        /// <returns></returns>
        public static string ToBinString(this byte aByte)
        {
            return new byte[] { aByte }.ToBinString();
        }

        /// <summary>
        /// 转为二进制字符串
        /// 注:一个字节转为8位二进制
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ToBinString(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var aByte in bytes)
            {
                builder.Append(Convert.ToString(aByte, 2).PadLeft(8, '0'));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Byte数组转为对应的16进制字符串
        /// </summary>
        /// <param name="bytes">Byte数组</param>
        /// <returns></returns>
        public static string To0XString(this byte[] bytes)
        {
            StringBuilder resStr = new StringBuilder();
            bytes.ToList().ForEach(aByte =>
            {
                resStr.Append(aByte.ToString("x2"));
            });

            return resStr.ToString();
        }

        /// <summary>
        /// Byte数组转为对应的16进制字符串
        /// </summary>
        /// <param name="aByte">一个Byte</param>
        /// <returns></returns>
        public static string To0XString(this byte aByte)
        {
            return new byte[] { aByte }.To0XString();
        }

        /// <summary>
        /// 转为ASCII字符串（一个字节对应一个字符）
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string ToASCIIString(this byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bytes.ToList().ForEach(aByte =>
            {
                stringBuilder.Append((char)aByte);
            });

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 转为ASCII字符串（一个字节对应一个字符）
        /// </summary>
        /// <param name="aByte">字节数组</param>
        /// <returns></returns>
        public static string ToASCIIString(this byte aByte)
        {
            return new byte[] { aByte }.ToASCIIString();
        }

        /// <summary>
        /// 获取异或值
        /// 注：每个字节异或
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static byte GetXOR(this byte[] bytes)
        {
            int value = bytes[0];
            for (int i = 1; i < bytes.Length; i++)
            {
                value = value ^ bytes[i];
            }

            return (byte)value;
        }

        /// <summary>
        /// 将字节数组转为Int类型
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static int ToInt(this byte[] bytes)
        {
            int num = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                num += bytes[i] * ((int)Math.Pow(256, bytes.Length - i - 1));
            }

            return num;
        }

        #region Get Value From Bytes

        /// <summary>
        /// 从缓存中提取出bool结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">位的索引</param>
        /// <returns>bool对象</returns>
        public static bool TransBool(this byte[] buffer, int index = 0)
        {
            return ((buffer[index] & 0x01) == 0x01);
        }


        /// <summary>
        /// 从缓存中提取出bool数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">位的索引</param>
        /// <param name="length">bool长度</param>
        /// <returns>bool数组</returns>
        public static bool[] TransBool(this byte[] buffer, int index, int length, bool reverse = false)
        {
            int realLength = (int)Math.Ceiling((length) * 1.0 / 8);
            byte[] temp = new byte[realLength];
            int bufferLength = buffer.Length - index;
            if (bufferLength <= realLength)
            {
                realLength = bufferLength;
            }
            Array.Copy(buffer, index, temp, 0, realLength);
            if (reverse) { temp = temp.Reverse().ToArray(); }
            return ByteToBoolArray(temp, length);
        }

        /// <summary>
        /// 从缓存中提取byte结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>byte对象</returns>
        public static byte TransByte(this byte[] buffer, int index)
        {
            return buffer[index];
        }

        /// <summary>
        /// 从缓存中提取byte数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>byte数组对象</returns>
        public static byte[] TransByte(this byte[] buffer, int index, int length)
        {
            byte[] tmp = new byte[length];
            Array.Copy(buffer, index, tmp, 0, length);
            return tmp;
        }


        /// <summary>
        /// 从缓存中提取short结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>short对象</returns>
        public static short TransInt16(this byte[] buffer, int index=0, bool reverse = false)
        {
            return TransInt16(buffer, index,1, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取short数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>short数组对象</returns>
        public static short[] TransInt16(this byte[] buffer, int index, int length, bool reverse = false)
        {
            short[] result = new short[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[2];
                if (reverse)
                {
                    temp[0] = buffer[1 + index + 2 * i];
                    temp[1] = buffer[0 + index + 2 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 2 * i, temp, 0, 2);
                }              
                result[i] = BitConverter.ToInt16(temp, 0);
            }
            return result;
        }


        /// <summary>
        /// 从缓存中提取ushort结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ushort对象</returns>
        public static ushort TransUInt16(this byte[] buffer, int index=0, bool reverse = false)
        {
            return TransUInt16(buffer, index, 1, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取ushort数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>ushort数组对象</returns>
        public static ushort[] TransUInt16(this byte[] buffer, int index, int length, bool reverse = false)
        {
            ushort[] result = new ushort[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[2];
                if (reverse)
                {
                    temp[0] = buffer[1 + index + 2 * i];
                    temp[1] = buffer[0 + index + 2 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 2 * i, temp, 0, 2);
                }
                result[i] = BitConverter.ToUInt16(temp, 0);
            }
            return result;
        }



        /// <summary>
        /// 从缓存中提取int结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>int对象</returns>
        public static int TransInt32(this byte[] buffer, int index=0, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransInt32(buffer, index, 1, format, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取int数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>int数组对象</returns>
        public static int[] TransInt32(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[4];

                if (reverse)
                {
                    temp[0] = buffer[3 + index + 4 * i];
                    temp[1] = buffer[2 + index + 4 * i];
                    temp[2] = buffer[1 + index + 4 * i];
                    temp[3] = buffer[0 + index + 4 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 4 * i, temp, 0, 4);
                }
                result[i] = BitConverter.ToInt32(ByteTransDataFormat4(temp, 0, format), 0);
            }
            return result;
        }



        /// <summary>
        /// 从缓存中提取uint结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>uint对象</returns>
        public static uint TransUInt32(this byte[] buffer, int index = 0, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransUInt32(buffer, index, 1, format, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取uint数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>uint数组对象</returns>
        public static uint[] TransUInt32(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            uint[] result = new uint[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[4];

                if (reverse)
                {
                    temp[0] = buffer[3 + index + 4 * i];
                    temp[1] = buffer[2 + index + 4 * i];
                    temp[2] = buffer[1 + index + 4 * i];
                    temp[3] = buffer[0 + index + 4 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 4 * i, temp, 0, 4);
                }
                result[i] = BitConverter.ToUInt32(ByteTransDataFormat4(temp, 0, format), 0);
            }
            return result;
        }

        /// <summary>
        /// 从缓存中提取long结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>long对象</returns>
        public static long TransInt64(this byte[] buffer, int index, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransInt64(buffer, index, 1, format, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取long数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>long数组对象</returns>
        public static long[] TransInt64(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            long[] result = new long[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[8];
                if (reverse)
                {
                    temp[0] = buffer[7 + index + 8 * i];
                    temp[1] = buffer[6 + index + 8 * i];
                    temp[2] = buffer[5 + index + 8 * i];
                    temp[3] = buffer[4 + index + 8 * i];
                    temp[4] = buffer[3 + index + 8 * i];
                    temp[5] = buffer[2 + index + 8 * i];
                    temp[6] = buffer[1 + index + 8 * i];
                    temp[7] = buffer[0 + index + 8 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 8 * i, temp, 0, 8);
                }
                result[i] = BitConverter.ToInt64(ByteTransDataFormat8(temp, 0, format), 0);
            }
            return result;
        }


        /// <summary>
        /// 从缓存中提取ulong结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <returns>ulong对象</returns>
        public static ulong TransUInt64(this byte[] buffer, int index, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransUInt64(buffer, index, 1, format, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取ulong数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>ulong数组对象</returns>
        public static ulong[] TransUInt64(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            ulong[] result = new ulong[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[8];
                if (reverse)
                {
                    temp[0] = buffer[7 + index + 8 * i];
                    temp[1] = buffer[6 + index + 8 * i];
                    temp[2] = buffer[5 + index + 8 * i];
                    temp[3] = buffer[4 + index + 8 * i];
                    temp[4] = buffer[3 + index + 8 * i];
                    temp[5] = buffer[2 + index + 8 * i];
                    temp[6] = buffer[1 + index + 8 * i];
                    temp[7] = buffer[0 + index + 8 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 8 * i, temp, 0, 8);
                }
                result[i] = BitConverter.ToUInt64(ByteTransDataFormat8(temp, 0, format), 0);
            }
            return result;
        }

        /// <summary>
        /// 从缓存中提取float结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>float对象</returns>
        public static float TransFloat(this byte[] buffer, int index=0, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransFloat(buffer, index, 1, format, reverse)[0];
        }

        /// <summary>
        /// 从缓存中提取float数组结果
        /// </summary>
        /// <param name="buffer">缓存数据</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>float数组对象</returns>
        public static float[] TransFloat(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {

            float[] reuslt = new float[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[4];
                if (reverse)
                {
                    temp[0] = buffer[3 + index + 4 * i];
                    temp[1] = buffer[2 + index + 4 * i];
                    temp[2] = buffer[1 + index + 4 * i];
                    temp[3] = buffer[0 + index + 4 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 4 * i, temp, 0, 4);

                }
                reuslt[i] = BitConverter.ToSingle(ByteTransDataFormat4(temp, 0, format), 0);

            }
            return reuslt;
        }


        /// <summary>
        /// 从缓存中提取double结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <returns>double对象</returns>
        public static double TransDouble(this byte[] buffer, int index=0, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
           return TransDouble(buffer, index, 1, format, reverse)[0];
        }
        /// <summary>
        /// 从缓存中提取double数组结果
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">读取的数组长度</param>
        /// <returns>double数组对象</returns>
        public static double[] TransDouble(this byte[] buffer, int index, int length, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            double[] result = new double[length];
            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[8];
                if (reverse)
                {
                    temp[0] = buffer[7 + index + 8 * i];
                    temp[1] = buffer[6 + index + 8 * i];
                    temp[2] = buffer[5 + index + 8 * i];
                    temp[3] = buffer[4 + index + 8 * i];
                    temp[4] = buffer[3 + index + 8 * i];
                    temp[5] = buffer[2 + index + 8 * i];
                    temp[6] = buffer[1 + index + 8 * i];
                    temp[7] = buffer[0 + index + 8 * i];
                }
                else
                {
                    Array.Copy(buffer, index + 8 * i, temp, 0, 8);
                }
                result[i]=  BitConverter.ToDouble(ByteTransDataFormat8(temp, 0, format), 0);
            }
            return result;
        }


        /// <summary>
        /// 从缓存中提取string结果，使用指定的编码
        /// </summary>
        /// <param name="buffer">缓存对象</param>
        /// <param name="index">索引位置</param>
        /// <param name="length">byte数组长度</param>
        /// <param name="encoding">字符串的编码</param>
        /// <returns>string对象</returns>
        public static string TransString(this byte[] buffer, int index , int length, Encoding encoding)
        {
            byte[] tmp = TransByte(buffer, index, length);
            return encoding.GetString(tmp);
        }


        #endregion

        #region Get Bytes From Value


        /// <summary>
        /// bool变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this bool value)
        {
            return TransByte(new bool[] { value });
        }

        /// <summary>
        /// bool数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this bool[] values)
        {
            if (values == null) return null;

            return BoolArrayToByte(values);
        }


        /// <summary>
        /// byte变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this byte value)
        {
            return new byte[] { value };
        }


        /// <summary>
        /// short变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this short value, bool reverse = false)
        {
            return TransByte(new short[] { value },reverse);
        }


        /// <summary>
        /// short数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this short[] values, bool reverse = false)
        {
            if (values == null) return null;
            byte[] buffer = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                if(reverse)
                {
                    byte[] temp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(temp);
                    temp.CopyTo(buffer, 2 * i);

                }
                else
                {
                    BitConverter.GetBytes(values[i]).CopyTo(buffer, 2 * i);
                }
            }
            return buffer;
        }


        /// <summary>
        /// ushort变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this ushort value, bool reverse = false)
        {
            return TransByte(new ushort[] { value },reverse);
        }


        /// <summary>
        /// ushort数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this ushort[] values, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 2];
            for (int i = 0; i < values.Length; i++)
            {
                if (reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    tmp.CopyTo(buffer, 2 * i);

                }
                else
                {
                    BitConverter.GetBytes(values[i]).CopyTo(buffer, 2 * i);
                }
            }

            return buffer;
        }


        /// <summary>
        /// int变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this int value, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransByte(new int[] { value },format,reverse);
        }


        /// <summary>
        /// int数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this int[] values, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                if (reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    ByteTransDataFormat4(tmp, format: format).CopyTo(buffer, 4 * i);

                }
                else
                {
                    ByteTransDataFormat4(BitConverter.GetBytes(values[i]), format: format).CopyTo(buffer, 4 * i);
                }
            }

            return buffer;
        }

        /// <summary>
        /// uint变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this uint value, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransByte(new uint[] { value },format,reverse);
        }


        /// <summary>
        /// uint数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this uint[] values, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                if(reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    ByteTransDataFormat4(tmp, format: format).CopyTo(buffer, 4 * i);

                }
                else
                {
                    ByteTransDataFormat4(BitConverter.GetBytes(values[i]), format: format).CopyTo(buffer, 4 * i);

                }
            }

            return buffer;
        }


        /// <summary>
        /// long变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this long value, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransByte(new long[] { value },format,reverse);
        }

        /// <summary>
        /// long数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this long[] values, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                if(reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    ByteTransDataFormat8(tmp,format:format).CopyTo(buffer, 8 * i);

                }
                else
                {
                    ByteTransDataFormat8(BitConverter.GetBytes(values[i]), format: format).CopyTo(buffer, 8 * i);

                }
            }

            return buffer;
        }

        /// <summary>
        /// ulong变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this ulong value, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransByte(new ulong[] { value },format,reverse);
        }

        /// <summary>
        /// ulong数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this ulong[] values, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                if (reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    ByteTransDataFormat8(tmp, format: format).CopyTo(buffer, 8 * i);

                }
                else
                {
                    ByteTransDataFormat8(BitConverter.GetBytes(values[i]), format: format).CopyTo(buffer, 8 * i);

                }
            }

            return buffer;
        }

        /// <summary>
        /// float变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this float value, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransByte(new float[] { value },format,reverse);
        }

        /// <summary>
        /// float数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this float[] values, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 4];
            for (int i = 0; i < values.Length; i++)
            {
                if (reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    ByteTransDataFormat4(tmp, format: format).CopyTo(buffer, 4 * i);

                }
                else
                {
                    ByteTransDataFormat4(BitConverter.GetBytes(values[i]), format: format).CopyTo(buffer, 4 * i);
                }
            }

            return buffer;
        }

        /// <summary>
        /// double变量转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this double value, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            return TransByte(new double[] { value },format,reverse);
        }

        /// <summary>
        /// double数组变量转化缓存数据
        /// </summary>
        /// <param name="values">等待转化的数组</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this double[] values, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            if (values == null) return null;

            byte[] buffer = new byte[values.Length * 8];
            for (int i = 0; i < values.Length; i++)
            {
                if(reverse)
                {
                    byte[] tmp = BitConverter.GetBytes(values[i]);
                    Array.Reverse(tmp);
                    ByteTransDataFormat8(tmp,format:format).CopyTo(buffer, 8 * i);

                }
                else
                {
                    ByteTransDataFormat8(BitConverter.GetBytes(values[i]), format: format).CopyTo(buffer, 8 * i);

                }
            }

            return buffer;
        }

        /// <summary>
        /// 使用指定的编码字符串转化缓存数据
        /// </summary>
        /// <param name="value">等待转化的数据</param>
        /// <param name="encoding">字符串的编码方式</param>
        /// <returns>buffer数据</returns>
        public static byte[] TransByte(this string values, Encoding encoding, EndianFormat format = EndianFormat.ABCD, bool reverse = false)
        {
            //if (values == null) return null;
            //byte[] buffer = new byte[values.Length * 4];
            //for (int i = 0; i < values.Length; i++)
            //{
            //    if (reverse)
            //    {
            //        byte[] tmp = encoding.GetBytes(values.Substring(i, 4));
            //        Array.Reverse(tmp);
            //        ByteTransDataFormat4(tmp, format: format).CopyTo(buffer, 4 * i);

            //    }
            //    else
            //    {
            //        ByteTransDataFormat4(encoding.GetBytes(values.Substring(i, 4)), format: format).CopyTo(buffer, 4 * i);

            //    }
            //}
            //return buffer;
            return encoding.GetBytes(values);
        }


        #endregion

        #region EndianFormat Support


        private static byte[] BoolArrayToByte(bool[] array)
        {
            if (array == null) return null;

            int length = array.Length % 8 == 0 ? array.Length / 8 : array.Length / 8 + 1;
            byte[] buffer = new byte[length];
            for (int i = 0; i < array.Length; i++)
            {
                int index = i / 8;
                int offect = i % 8;
                byte temp = 0;
                switch (offect)
                {
                    case 0: temp = 0x01; break;
                    case 1: temp = 0x02; break;
                    case 2: temp = 0x04; break;
                    case 3: temp = 0x08; break;
                    case 4: temp = 0x10; break;
                    case 5: temp = 0x20; break;
                    case 6: temp = 0x40; break;
                    case 7: temp = 0x80; break;
                    default: break;
                }
                if (array[i]) buffer[index] += temp;
            }

            return buffer;
        }


        /// <summary>
        /// 反转多字节的数据信息
        /// </summary>
        /// <param name="value">数据字节</param>
        /// <param name="index">起始索引，默认值为0</param>
        /// <returns>实际字节信息</returns>
         private static byte[] ByteTransDataFormat4( byte[] value, int index = 0, EndianFormat format = EndianFormat.ABCD)
        {
            byte[] buffer = new byte[4];
            switch (format)
            {
                case EndianFormat.ABCD:
                    {
                        buffer[0] = value[index + 3];
                        buffer[1] = value[index + 2];
                        buffer[2] = value[index + 1];
                        buffer[3] = value[index + 0];
                        break;
                    }
                case EndianFormat.BADC:
                    {
                        buffer[0] = value[index + 2];
                        buffer[1] = value[index + 3];
                        buffer[2] = value[index + 0];
                        buffer[3] = value[index + 1];
                        break;
                    }

                case EndianFormat.CDAB:
                    {
                        buffer[0] = value[index + 1];
                        buffer[1] = value[index + 0];
                        buffer[2] = value[index + 3];
                        buffer[3] = value[index + 2];
                        break;
                    }
                case EndianFormat.DCBA:
                    {
                        buffer[0] = value[index + 0];
                        buffer[1] = value[index + 1];
                        buffer[2] = value[index + 2];
                        buffer[3] = value[index + 3];
                        break;
                    }
            }
            return buffer;
        }


        /// <summary>
        /// 反转多字节的数据信息
        /// </summary>
        /// <param name="value">数据字节</param>
        /// <param name="index">起始索引，默认值为0</param>
        /// <returns>实际字节信息</returns>
        private static byte[] ByteTransDataFormat8(byte[] value, int index = 0, EndianFormat format = EndianFormat.ABCD)
        {
            byte[] buffer = new byte[8];
            if (value.Length >= 8)
                switch (format)
                {
                    case EndianFormat.ABCD:
                        {
                            buffer[0] = value[index + 7];
                            buffer[1] = value[index + 6];
                            buffer[2] = value[index + 5];
                            buffer[3] = value[index + 4];
                            buffer[4] = value[index + 3];
                            buffer[5] = value[index + 2];
                            buffer[6] = value[index + 1];
                            buffer[7] = value[index + 0];
                            break;
                        }
                    case EndianFormat.BADC:
                        {
                            buffer[0] = value[index + 6];
                            buffer[1] = value[index + 7];
                            buffer[2] = value[index + 4];
                            buffer[3] = value[index + 5];
                            buffer[4] = value[index + 2];
                            buffer[5] = value[index + 3];
                            buffer[6] = value[index + 0];
                            buffer[7] = value[index + 1];
                            break;
                        }

                    case EndianFormat.CDAB:
                        {
                            buffer[0] = value[index + 1];
                            buffer[1] = value[index + 0];
                            buffer[2] = value[index + 3];
                            buffer[3] = value[index + 2];
                            buffer[4] = value[index + 5];
                            buffer[5] = value[index + 4];
                            buffer[6] = value[index + 7];
                            buffer[7] = value[index + 6];
                            break;
                        }
                    case EndianFormat.DCBA:
                        {
                            buffer[0] = value[index + 0];
                            buffer[1] = value[index + 1];
                            buffer[2] = value[index + 2];
                            buffer[3] = value[index + 3];
                            buffer[4] = value[index + 4];
                            buffer[5] = value[index + 5];
                            buffer[6] = value[index + 6];
                            buffer[7] = value[index + 7];
                            break;
                        }
                }
            return buffer;
        }


        /// <summary>
        /// 从Byte数组中提取位数组，length代表位数 ->
        /// Extracts a bit array from a byte array, length represents the number of digits
        /// </summary>
        /// <param name="InBytes">原先的字节数组</param>
        /// <param name="length">想要转换的长度，如果超出自动会缩小到数组最大长度</param>
        /// <returns>转换后的bool数组</returns>
        /// <example>
        /// <code lang="cs" source="HslCommunication_Net45.Test\Documentation\Samples\BasicFramework\SoftBasicExample.cs" region="ByteToBoolArray" title="ByteToBoolArray示例" />
        /// </example> 
        private static bool[] ByteToBoolArray( byte[] InBytes, int length=1)
        {
            if (InBytes == null) return null;

            if (length > InBytes.Length * 8) length = InBytes.Length * 8;
            bool[] buffer = new bool[length];

            for (int i = 0; i < length; i++)
            {
                int index = i / 8;
                int offect = i % 8;

                byte temp = 0;
                switch (offect)
                {
                    case 0: temp = 0x01; break;
                    case 1: temp = 0x02; break;
                    case 2: temp = 0x04; break;
                    case 3: temp = 0x08; break;
                    case 4: temp = 0x10; break;
                    case 5: temp = 0x20; break;
                    case 6: temp = 0x40; break;
                    case 7: temp = 0x80; break;
                    default: break;
                }

                if ((InBytes[index] & temp) == temp)
                {
                    buffer[i] = true;
                }
            }

            return buffer;
        }


        #endregion




    }
}
