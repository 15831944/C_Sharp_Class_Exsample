using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<TBook> list = new List<TBook>();
            //操作数据
            for (int i = 0; i < 5; i++)
            {
                list.Add(
                    new TBook(i.ToString(),
                    new Tep() { age = i * 4 },
                    3.14 * i)
                    );
            }

            //序列化
            SerializeData(list);

            //反序列化
            List<TBook> var = DeserializeData();
        }
        static void SerializeData(List<TBook> list)
        {
            //所有需要处理的数据压入 Hashtable
            Hashtable data = new Hashtable();
            data.Add("data", list);

            //创建文件流
            string file = @"D:\temp\asd.bin";
            if (File.Exists(file)) File.Delete(file);
            FileStream ms = new FileStream(file, FileMode.CreateNew, FileAccess.ReadWrite);

            //序列化
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, data);
            ms.Close();
        }
        static List<TBook> DeserializeData()
        {
            using (FileStream fs = new FileStream(@"D:\temp\asd.bin", FileMode.Open, FileAccess.Read))
            {
                //读出来
                byte[] buf = new byte[fs.Length];
                fs.Read(buf, 0, (int)fs.Length);

                //转成数据流,反序列化
                MemoryStream ms = new MemoryStream(buf);
                BinaryFormatter bf = new BinaryFormatter();
                object var = bf.Deserialize(ms);

                //转出
                Hashtable hash = var as Hashtable;
                List<TBook> te = hash["data"] as List<TBook>;
                return te;
            }
        }
    }
}

