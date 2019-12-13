using System;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace ConfiguratorKernel.Directory
{
    /// <summary>
    /// Клас ДовідникОбєкт
    /// </summary>
    abstract public class DirectoryObject
    {
        public DirectoryObject(string tableName)
        {
            Table = tableName;
        }

        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// ІД Обєкта
        /// </summary>
        public string ID { get; protected set; }

        /// <summary>
        /// Це новий?
        /// </summary>
        public bool IsNew { get; private set; }

        public void New()
        {
            IsNew = true;
            ID = "";
        }

        public void Save()
        {

        }

        protected void GetObjectByLinkBase(DirectoryLink link)
        {
            Dictionary<string, string> row = new Dictionary<string, string>(); //link.GetRow();

            row.Add("Code", "1");
            row.Add("Name", "Name Name Name");
            row.Add("Desc", "Description");
            row.Add("klient", "110");
            row.Add("Count", "100");

            ID = link.ID;

            foreach (PropertyInfo propertyInfoItem in GetType().GetRuntimeProperties())
            {
                Attribute attribute = propertyInfoItem.GetCustomAttribute(typeof(FieldInfoAttribute));
                if (attribute != null)
                {
                    FieldInfoAttribute attributeFieldInfo = (FieldInfoAttribute)attribute;

                    if (attributeFieldInfo.TypeName == "string")
                    {
                        propertyInfoItem.SetValue(this, row[propertyInfoItem.Name]);
                    }
                    else if (attributeFieldInfo.TypeName == "integer")
                    {
                        propertyInfoItem.SetValue(this, int.Parse(row[propertyInfoItem.Name]));
                    }
                    else if (attributeFieldInfo.TypeName == "link")
                    {
                        Type tt = Type.GetType("ConfiguratorKernel.Directory." + attributeFieldInfo.TypeLink);
                        object o = Activator.CreateInstance(tt, new object[] { row[propertyInfoItem.Name] });

                        propertyInfoItem.SetValue(this, o);
                    }
                }

            }
        }

        /*
        
        public void Test()
        {
            Type t = this.GetType();

            IEnumerable<PropertyInfo> pif = t.GetRuntimeProperties();

            foreach (PropertyInfo ipif in pif)
            {
                Debug.WriteLine(ipif.Name);

                Attribute a = ipif.GetCustomAttribute(typeof(FieldInfoAttribute));
                if (a != null)
                {
                    FieldInfoAttribute fa = (FieldInfoAttribute)a;
                    Debug.WriteLine(" a -->" + fa._Type + "; --> 2 " + fa._Type2);

                    if (fa._Type == "link")
                    {
                        Type tt = Type.GetType("ConfiguratorKernel.Directory." + fa._Type2);
                        object o = Activator.CreateInstance(tt, new object[] { "10" });

                        ipif.SetValue(this, o);
                    }
                        
                }
                   
            }

        }

        */

        /// <summary>
        /// Ссилка на обєкт
        /// </summary>
        /// <returns></returns>
        public DirectoryLink Link()
        {
            if (!IsNew)
                return new DirectoryLink(Table, ID);
            else
                return null;
        }
    }
}
