using System;
using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{
    /// <summary>
    /// Клас ДовідникВибірка. 
    /// Обримує вибірку ссилок для довідника
    /// </summary>
    public class DirectorySelect
    {
        public DirectorySelect(string tableName)
        {
            Table = tableName;
        }

        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// Відбір по полях
        /// </summary>
        public Dictionary<string, string> Where { get; set; }

        /// <summary>
        /// Сортування
        /// </summary>
        public Dictionary<string, string> OrderBy { get; set; }

        /// <summary>
        /// Кількість елементів
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Вибірка ссилок
        /// </summary>
        /// <returns></returns>
        protected void SelectLink(List<> link)
        {
            //Вибірка з бази даних ...

            link.Add((T)new DirectoryLink(this.Table, "10"));
        }

        /// <summary>
        /// Вибірка з бази даних однієї ссилки
        /// </summary>
        /// <returns></returns>
        protected DirectoryLink SelectLinkOne()
        {
            //Вибірка з бази даних одного...

            return new DirectoryLink(Table);
        }
    }
}
