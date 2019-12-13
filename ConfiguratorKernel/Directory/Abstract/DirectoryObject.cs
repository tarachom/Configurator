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
        /// <summary>
        /// Функція знаходить поля довідника і добавляє їх у словник
        /// </summary>
        /// <param name="linkFields"></param>
        private void FillFields()
        {
            Fields = new List<DirectoryFieldInfo>();

            foreach (PropertyInfo propertyInfoItem in GetType().GetRuntimeProperties())
            {
                Attribute attribute = propertyInfoItem.GetCustomAttribute(typeof(FieldInfoAttribute));
                if (attribute != null)
                {
                    FieldInfoAttribute attributeFieldInfo = (FieldInfoAttribute)attribute;

                    object propertyInfoItemValue = propertyInfoItem.GetValue(this);

                    //Default
                    if (propertyInfoItemValue == null)
                    {
                        if (attributeFieldInfo.FieldType == "string")
                        {
                            propertyInfoItemValue = "";
                        }
                        else if (attributeFieldInfo.FieldType == "integer")
                        {
                            propertyInfoItemValue = 0;
                        }
                        else if (attributeFieldInfo.FieldType == "link")
                        {
                            propertyInfoItemValue = 0;
                        }
                    }

                    Fields.Add(new DirectoryFieldInfo(propertyInfoItem.Name, attributeFieldInfo.FieldType, attributeFieldInfo.FieldTypeLink, propertyInfoItemValue.ToString()));
                }
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="tableName"></param>
        public DirectoryObject(string tableName, DirectoryLink emptyLink)
        {
            Table = tableName;
            EmptyLink = emptyLink;

            FillFields();
        }

        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// Поля довідника
        /// </summary>
        private List<DirectoryFieldInfo> Fields;

        protected DirectoryLink EmptyLink { get; private set; }

        /// <summary>
        /// ІД Обєкта
        /// </summary>
        public string ID { get; protected set; }

        /// <summary>
        /// Це новий?
        /// </summary>
        public bool IsNew { get; private set; }

        /// <summary>
        /// Новий
        /// </summary>
        public void New()
        {
            IsNew = true;
            ID = "";
        }

        /// <summary>
        /// Зберегти
        /// </summary>
        public void Save()
        {
            //Перезаповнити дані полів
            FillFields();

            //Запис
            DirectoryLink newlink = Kernel.ChannelData.DirectoryObjectSave(GetLink(), Fields);

            //Запис табличних частин
            foreach (PropertyInfo propertyInfoItem in GetType().GetRuntimeProperties())
            {
                //Признак табличної частини
                Attribute attributeTabularPart = propertyInfoItem.GetCustomAttribute(typeof(TabularPartAttribute));
                if (attributeTabularPart != null)
                {
                    //TabularPartAttribute attributeTabularPartInfo = (TabularPartAttribute)attributeTabularPart;

                    //Таблична частина обєкт
                    DirectoryTabularPart TabularPart = (DirectoryTabularPart)propertyInfoItem.GetValue(this);
                    
                    Kernel.ChannelData.DirectoryObjectTabularPartSave(TabularPart.Records, ID, TabularPart.Table, TabularPart.Fields);
                }
            }

            if (IsNew)
            {
                IsNew = false;
                ID = newlink.ID;
            }
        }

        /// <summary>
        /// Отримати обєкт із ссилки
        /// </summary>
        /// <param name="link"></param>
        public DirectoryObject GetObjectByLink(DirectoryLink link)
        {
            Kernel.ChannelData.DirectorySelectFieldsByLink(link, Fields);

            ID = link.ID;

            IsNew = false;

           Type thisType = GetType();

            //Заповнення полів
            foreach (DirectoryFieldInfo field in Fields)
            {
                PropertyInfo propertyFieldInfo = thisType.GetProperty(field.FieldName);
                propertyFieldInfo.SetValue(this, field.FieldValue);
            }

            foreach (PropertyInfo propertyInfoItem in GetType().GetRuntimeProperties())
            {
                //Признак табличної частини
                Attribute attributeTabularPart = propertyInfoItem.GetCustomAttribute(typeof(TabularPartAttribute));
                if (attributeTabularPart != null)
                {
                    //TabularPartAttribute attributeTabularPartInfo = (TabularPartAttribute)attributeTabularPart;

                    //Виклик методу Load() для табличної частини
                    MethodInfo MethodInfoLoad = propertyInfoItem.PropertyType.GetMethod("Load");
                    MethodInfoLoad.Invoke(propertyInfoItem.GetValue(this), null);
                }
            }

            return this;
        }

        /// <summary>
        /// Отримати ссилку на обєкт
        /// </summary>
        /// <returns></returns>
        public DirectoryLink GetLink()
        {
            return (DirectoryLink)Activator.CreateInstance(EmptyLink.GetType(), new object[] { ID });
        }
    }
}
