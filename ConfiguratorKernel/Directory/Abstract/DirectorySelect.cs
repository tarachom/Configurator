using System;
using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{
    /// <summary>
    /// Клас ДовідникВибірка. 
    /// </summary>
    public class DirectorySelect
    {
        public DirectorySelect(string tableName, DirectoryLink emptyLink)
        {
            Table = tableName;
            EmptyLink = emptyLink;
        }

        /// <summary>
        /// Назва таблиці
        /// </summary>
        public string Table { get; }

        protected DirectoryLink EmptyLink { get; private set; }

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
        /// Вибрані ссилки
        /// </summary>
        public List<DirectoryLink> Link { get; private set; }

        /// <summary>
        /// Вибірка ссилок
        /// </summary>
        /// <returns></returns>
        public int Select()
        {
            Link = new List<DirectoryLink>();

            Kernel.ChannelData.DirectorySelectLink(Link, EmptyLink, Where, OrderBy, Limit);

            return Link.Count;
        }
    }
}
