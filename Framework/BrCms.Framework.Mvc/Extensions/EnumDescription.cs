﻿using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace BrCms.Framework.Mvc.Extensions
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum)]
    public class EnumDescription : Attribute
    {
        /// <summary>
        /// cachedEnum
        /// </summary>
        private static Hashtable cachedEnum = new Hashtable();

        /// <summary>
        /// enumDisplayText
        /// </summary>
        private string enumDisplayText;

        /// <summary>
        /// enumRank
        /// </summary>
        private int enumRank;

        /// <summary>
        /// fieldIno
        /// </summary>
        private FieldInfo fieldIno;

        /// <summary>
        /// 描述枚举值
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        /// <param name="enumRank">排列顺序</param>
        public EnumDescription(string enumDisplayText, int enumRank)
        {
            this.enumDisplayText = enumDisplayText;
            this.enumRank = enumRank;
        }

        /// <summary>
        /// 描述枚举值,默认排序为5
        /// </summary>
        /// <param name="enumDisplayText">描述内容</param>
        public EnumDescription(string enumDisplayText)
            : this(enumDisplayText, 5)
        {
        }

        /// <summary>
        /// 排序类型
        /// </summary>
        public enum SortType
        {
            /// <summary>
            /// 按枚举顺序默认排序
            /// </summary>
            Default,

            /// <summary>
            /// 按描述值排序
            /// </summary>
            DisplayText,

            /// <summary>
            /// 按排序熵
            /// </summary>
            Rank
        }

        /// <summary>
        /// 枚举文本
        /// </summary>
        public string EnumDisplayText
        {
            get { return this.enumDisplayText; }
        }

        /// <summary>
        /// 枚举等级
        /// </summary>
        public int EnumRank
        {
            get { return this.enumRank; }
        }

        /// <summary>
        /// 枚举值
        /// </summary>
        public int EnumValue
        {
            get { return (int)this.fieldIno.GetValue(null); }
        }

        /// <summary>
        /// FieldName
        /// </summary>
        public string FieldName
        {
            get { return this.fieldIno.Name; }
        }

        #region  =========================================对枚举描述属性的解释相关函数

        /// <summary>
        /// 得到对枚举的描述文本
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>返回描述文本</returns>
        public static string GetEnumText(Type enumType)
        {
            EnumDescription[] eds = (EnumDescription[])enumType.GetCustomAttributes(typeof(EnumDescription), false);

            if (eds.Length != 1)
            {
                return string.Empty;
            }

            return eds[0].EnumDisplayText;
        }

        /// <summary>
        /// 枚举是否存在
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>bool</returns>
        public static bool IsAttrbute(object enumValue)
        {
            var fieldIno = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldIno.GetCustomAttributes(typeof(EnumDescription), true).Any())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获得指定枚举类型中,指定值的描述文本。
        /// </summary>
        /// <param name="enumValue">枚举值,不要作任何类型转换</param>
        /// <returns>描述字符串</returns>
        public static string GetFieldText(object enumValue)
        {
            var fieldIno = enumValue.GetType().GetField(enumValue.ToString());
            var attrbute = (EnumDescription)fieldIno.GetCustomAttributes(typeof(EnumDescription), true).SingleOrDefault();
            return attrbute != null ? attrbute.EnumDisplayText : string.Empty;
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本,按定义的顺序返回
        /// </summary>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        /// <param name="enumType">枚举类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType)
        {
            return GetFieldTexts(enumType, SortType.Default);
        }

        /// <summary>
        /// 得到枚举类型定义的所有文本
        /// </summary>
        /// <exception cref="NotSupportedException">NotSupportedException</exception>
        /// <param name="enumType">枚举类型</param>
        /// <param name="sortType">指定排序类型</param>
        /// <returns>所有定义的文本</returns>
        public static EnumDescription[] GetFieldTexts(Type enumType, SortType sortType)
        {
            EnumDescription[] descriptions = null;

            // 缓存中没有找到,通过反射获得字段的描述信息
            if (cachedEnum.Contains(enumType.FullName) == false)
            {
                FieldInfo[] fields = enumType.GetFields();
                ArrayList edAL = new ArrayList();

                foreach (FieldInfo fi in fields)
                {
                    object[] eds = fi.GetCustomAttributes(typeof(EnumDescription), false);

                    if (eds.Length != 1)
                    {
                        continue;
                    }

                    ((EnumDescription)eds[0]).fieldIno = fi;
                    edAL.Add(eds[0]);
                }

                cachedEnum.Add(enumType.FullName, (EnumDescription[])edAL.ToArray(typeof(EnumDescription)));
            }

            descriptions = (EnumDescription[])cachedEnum[enumType.FullName];

            if (descriptions.Length <= 0)
            {
                throw new NotSupportedException("枚举类型[" + enumType.Name + "]未定义属性EnumValueDescription");
            }

            // 按指定的属性冒泡排序
            for (int m = 0; m < descriptions.Length; m++)
            {
                // 默认就不排序了
                if (sortType == SortType.Default)
                {
                    break;
                }

                for (int n = m; n < descriptions.Length; n++)
                {
                    EnumDescription temp;
                    bool swap = false;

                    switch (sortType)
                    {
                        case SortType.Default:
                            break;
                        case SortType.DisplayText:
                            if (String.CompareOrdinal(descriptions[m].EnumDisplayText, descriptions[n].EnumDisplayText) > 0)
                            {
                                swap = true;
                            }

                            break;
                        case SortType.Rank:
                            if (descriptions[m].EnumRank > descriptions[n].EnumRank)
                            {
                                swap = true;
                            }

                            break;
                    }

                    if (swap)
                    {
                        temp = descriptions[m];
                        descriptions[m] = descriptions[n];
                        descriptions[n] = temp;
                    }
                }
            }

            return descriptions;
        }

        #endregion
    }
}
