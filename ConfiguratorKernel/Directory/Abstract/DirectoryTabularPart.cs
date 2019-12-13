using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConfiguratorKernel.Directory
{
    /// <summary>
    /// Клас ДовідникТабличнаЧастина
    /// </summary>
    public abstract class DirectoryTabularPart
    {
        /// <summary>
        /// Функція знаходить поля табличної частини і добавляє їх у словник
        /// </summary>
        /// <param name="linkFields"></param>
        private void FillTabularPartFields()
        {
            Fields = new List<DirectoryFieldInfo>();

            foreach (PropertyInfo propertyInfoItem in EmptyRecord.GetType().GetRuntimeProperties())
            {
                Attribute attribute = propertyInfoItem.GetCustomAttribute(typeof(FieldInfoAttribute));
                if (attribute != null)
                {
                    FieldInfoAttribute attributeFieldInfo = (FieldInfoAttribute)attribute;
                    Fields.Add(new DirectoryFieldInfo(propertyInfoItem.Name, attributeFieldInfo.FieldType, attributeFieldInfo.FieldTypeLink));
                }
            }
        }

        /// <summary>
        /// Констуктор
        /// </summary>
        /// <param name="owner">Власник табличної частини</param>
        /// <param name="record">Шаблон запису</param>
        /// <param name="tableName">Таблиця в базі даних</param>
        public DirectoryTabularPart(string tableName, DirectoryObject owner, DirectoryTabularPartRecord emptyRecord)
        {
            Table = tableName;
            Owner = owner;

            EmptyRecord = emptyRecord;
            Records = new List<DirectoryTabularPartRecord>();

            FillTabularPartFields();
        }

        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// Поля табличної частини
        /// </summary>
        public List<DirectoryFieldInfo> Fields { get; private set; }

        /// <summary>
        /// Власник табличної частини
        /// </summary>
        protected DirectoryObject Owner { get; private set; }

        /// <summary>
        /// Запис табличної частини. Це пустий шаблон запису з набором полів.
        /// </summary>
        protected DirectoryTabularPartRecord EmptyRecord { get; private set; }

        /// <summary>
        /// Список записів табличної частини
        /// </summary>
        public List<DirectoryTabularPartRecord> Records { get; private set; }

        /// <summary>
        /// Загрузка даних табличної частини
        /// </summary>
        public void Load()
        {
            Kernel.ChannelData.DirectoryTabularPartSelect(Records, EmptyRecord, Owner.ID, Table, Fields);
        }
    }
}
