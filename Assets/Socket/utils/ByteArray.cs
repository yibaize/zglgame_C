using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
namespace org.zgl
{
    public class ByteArray
    {
        private List<byte> bytes = new List<byte>();
        public int Length
        {
            get
            {
                return this.bytes.Count;
            }
        }
        public int Postion
        {
            get;
            set;
        }
        public byte[] Buffer
        {
            get
            {
                return this.bytes.ToArray();
            }
        }
        public ByteArray()
        {
        }
        public void WriteInt(int value)
        {
            byte[] bs = BitConverter.GetBytes(value);
            Array.Reverse(bs);
            this.bytes.AddRange(bs);
        }
        public void WriteBytes(byte[] value)
        {
            this.bytes.AddRange(value);
        }
        public int ReadInt()
        {
            byte[] array = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 4;
            Array.Reverse(array);
            return BitConverter.ToInt32(array, 0);
        }
        public void WriteBytes(byte[] value, int offest, int length)
        {
            for (int i = 0; i < length; i++)
            {
                try
                {
                    this.bytes.Add(value[i + offest]);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        public byte[] ReadBytes()
        {
            byte[] array = new byte[this.Length - this.Postion];
            for (int i = 0; i < this.Length - this.Postion; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            this.Postion = this.Length;
            return array;
        }
        public void WriteDouble(double dou)
        {
            byte[] bs = BitConverter.GetBytes(dou);
            Array.Reverse(bs);
            this.bytes.AddRange(bs);
        }
        public double ReadDouble()
        {
            byte[] array = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 8;
            Array.Reverse(array);
            return BitConverter.ToDouble(array, 0);
        }
        public void WriteFloat(float flao)
        {
            byte[] bs = BitConverter.GetBytes(flao);
            Array.Reverse(bs);
            this.bytes.AddRange(bs);
        }
        public float ReadFloat()
        {
            byte[] array = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 4;
            Array.Reverse(array);
            return BitConverter.ToSingle(array, 0);
        }
        public void WriteLong(long lon)
        {
            byte[] bs = BitConverter.GetBytes(lon);
            Array.Reverse(bs);
            this.bytes.AddRange(bs);
        }
        public long ReadLong()
        {
            byte[] array = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 8;
            Array.Reverse(array);
            return BitConverter.ToInt64(array, 0);
        }
        public void WriteShort(short sho)
        {
            byte[] bs = BitConverter.GetBytes(sho);
            Array.Reverse(bs);
            this.bytes.AddRange(bs);
        }
        public short ReadShort()
        {
            byte[] array = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 2;
            Array.Reverse(array);
            short t = BitConverter.ToInt16(array, 0);
            return t;
        }
        public void WriteChar(char cha)
        {
            byte[] bs = BitConverter.GetBytes(cha);
            Array.Reverse(bs);
            this.bytes.AddRange(bs);
        }
        public char ReadChar()
        {
            byte[] array = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 2;
            Array.Reverse(array);
            return BitConverter.ToChar(array, 0);
        }
        public void WriteString(string str)
        {
            byte[] bs = Encoding.UTF8.GetBytes(str);
            //Array.Reverse(bs);
            this.WriteInt(bs.Length);
            this.bytes.AddRange(bs);
        }
        public string ReadString()
        {
            int num = this.ReadInt();
            if (num <= 0)
            {
                return string.Empty;
            }
            byte[] array = new byte[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            this.Postion += num;
            //Array.Reverse(array);
            return Encoding.UTF8.GetString(array);
        }
        public void WriteBoolean(bool value)
        {
            if (value == true)
            {
                byte[] bs = BitConverter.GetBytes(1);
                Array.Reverse(bs);
                this.bytes.AddRange(bs);
            }
            else
            {
                byte[] bs = BitConverter.GetBytes(0);
                Array.Reverse(bs);
                this.bytes.AddRange(bs);
            }
        }
        public bool ReadBoolean()
        {
            byte[] array = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                array[i] = this.bytes[i + this.Postion];
            }
            Postion += 4;
            Array.Reverse(array);
            int boo = BitConverter.ToInt32(array, 0);
            return boo != 0;
        }
    }
}
