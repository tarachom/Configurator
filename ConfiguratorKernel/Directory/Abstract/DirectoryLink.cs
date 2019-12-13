using System;
using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{
    /// <summary>
    /// Клас ДовідникСсилка
    /// </summary>
    public class DirectoryLink
    {
        /// <summary>
        /// Пуста ссилка
        /// </summary>
        /// <param name="tableName"></param>
        public DirectoryLink(string tableName)
        {
            Table = tableName;
            ID = "";
        }

        /// <summary>
        /// Реальна ссилка
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        public DirectoryLink(string tableName, string id)
        {
            Table = tableName;
            ID = id;
        }

        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// ІД ссилки
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Чи це пуста ссилка?
        /// </summary>
        /// <returns></returns>
        public bool IsEmptyLink()
        {
            return (ID == "");
        }

        public override string ToString()
        {
            return Table + ":" + ID;
        }
    }
}
