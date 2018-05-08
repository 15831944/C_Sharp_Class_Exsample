using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace demo
{
    [Serializable]
    class TBook : Book, ISerializable
    {
        //Field 字段
        public Tep sTep;

        //Property 属性
        public double size { set; get; }

        //4.0增加内容
        public float width { set; get; }

        public TBook(string name, Tep tep, double sz)
        {
            Name = name;
            sTep = tep;
            size = sz;
        }
        
        /// <summary>
        /// 构造函数 ，反序列化时调用
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected TBook(SerializationInfo info, StreamingContext context)
        {
            //使用扩展函数，使多种结构可以共用此函数
            this.Serializable_setData(info, getDic(info.MemberCount));
        }

        /// <summary>
        /// 接口方法，序列化时调用
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.Serializable_getData(getDic(-1));
        }
        
        /// <summary>
        /// 实现版本管理的方法
        /// </summary>
        /// <param name="MemberCount"></param>
        /// <returns></returns>
        Dictionary<string, object> getDic(int MemberCount)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (MemberCount == -1) //序列化时调用，当前版本
            {
                dic.Add("sTep", sTep); //Key 必须与成员名一致，否则无法反序列化
                dic.Add("Name", Name);
                dic.Add("size", size);
            }

            else if (MemberCount == 3) //反序列化时调用，3.0
            {
                dic.Add("sTep", sTep);
                dic.Add("Name", Name);
                dic.Add("size", size);
            }
            else if (MemberCount == 4) //反序列化时调用，4.0
            {
                dic.Add("sTep", sTep);
                dic.Add("Name", Name);
                dic.Add("size", size);
                dic.Add("width", width);
            }
            return dic;
        }
    }

    //组成结构的要素也要标记
    [Serializable]
    class Book
    {
        public string Name;
    }
    [Serializable]
    class Tep
    {
        public int age;
    }


    static class Serializable_Tools
    {
        public static void Serializable_getData(this SerializationInfo info, Dictionary<string, object> dic)
        {
            foreach (var item in dic)
            {
                info.AddValue(item.Key, item.Value);
            }
        }
        public static void Serializable_setData(this object tag_obj, SerializationInfo info, Dictionary<string, object> dic)
        {
            Type obj_type = tag_obj.GetType();
            foreach (var item in dic)
            {
                //根据key找到成员
                MemberInfo[] mi = obj_type.GetMember(item.Key);

                if (mi.Length > 0)
                {
                    //判断成员类型，属性和字段不可能同名，只用第一个
                    MemberTypes mtp = mi[0].MemberType;

                    if (mtp == MemberTypes.Field)
                    {
                        //通过key获取字段对象
                        FieldInfo fi = obj_type.GetField(item.Key);

                        //获取字段值的类型
                        Type tp = fi.FieldType;

                        //按字段类型，从info中取出值
                        object obj_var = info.GetValue(item.Key, tp);

                        //为字段赋值
                        fi.SetValue(tag_obj, obj_var);
                    }
                    else if (mtp == MemberTypes.Property)
                    {
                        PropertyInfo pi = obj_type.GetProperty(item.Key);
                        Type tp = pi.PropertyType;
                        object obj_var = info.GetValue(item.Key, tp);
                        pi.SetValue(tag_obj, obj_var);
                    }
                }
            }
        }
    }

}
