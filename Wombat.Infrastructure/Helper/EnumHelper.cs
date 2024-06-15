using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Wombat.Infrastructure
{
    public static class EnumHelper
    {
        /// <summary>
        /// 将枚举类型转为选项列表
        /// 注：value为值,text为显示内容
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static List<SelectOption> ToOptionList(Type enumType)
        {
            var values = Enum.GetValues(enumType);
            List<SelectOption> list = new List<SelectOption>();
            foreach (var aValue in values)
            {
                list.Add(new SelectOption
                {
                    value = ((int)aValue).ToString(),
                    text = aValue.ToString()
                });
            }

            return list;
        }

        /// <summary>
        /// 多选枚举转为对应文本,逗号隔开
        /// </summary>
        /// <param name="values">多个值</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static string ToMultipleText(List<int> values, Type enumType)
        {
            if (values == null)
                return string.Empty;

            List<string> textList = new List<string>();

            var allValues = Enum.GetValues(enumType);
            foreach (var aValue in allValues)
            {
                if (values.Contains((int)aValue))
                    textList.Add(aValue.ToString());
            }

            return string.Join(",", textList);
        }



        public static List<T> ToList<T>() where T:Enum
        {
            List<T> dataCalibrationCoreTypes = new List<T>();
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                dataCalibrationCoreTypes.Add(item);
            }
            return dataCalibrationCoreTypes;
        }


        public static List<string> ToDescriptionList<T>() where T : Enum
        {
            Type enumType = typeof(T);

            return Enum.GetValues(enumType)
                       .Cast<Enum>()
                       .Select(value =>
                       {
                           string name = Enum.GetName(enumType, value);
                           FieldInfo field = enumType.GetField(name);

                           if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                           {
                               return attribute.Description;
                           }
                           else
                           {
                               return name;
                           }
                       }).ToList();
        }


        /// <summary>
        /// 通过枚举描述获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetEnumValueFromDescription<T>(string description) where T : struct,Enum
        {
            foreach (FieldInfo field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return (T)field.GetValue(null);
                    }
                }
            }

            throw new ArgumentException($"Enum value with description '{description}' not found.", nameof(description));
        }

        /// <summary>
        /// 通过枚举名称获取枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetEnumValueFromName<T>(string name) where T : struct,  Enum ,IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Type {typeof(T)} is not an enum");
            }

            if (Enum.TryParse(name, out T result) )
            {
                return result;
            }

            throw new ArgumentException($"Enum value with name '{name}' not found.", nameof(name));
        }

    }
}
