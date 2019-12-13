using System;
using System.Collections.Generic;
using System.Text;

using ConfiguratorKernel.Directory;

namespace ConfiguratorKernel
{
    /// <summary>
    /// Інтерфейс для роботи з базою даних
    /// </summary>
    public interface IData
    {
        #region CONNECT

        /// <summary>
        /// Строка підключення до бази даних
        /// </summary>
        string ConnectString { get; set; }

        /// <summary>
        /// Підключення
        /// </summary>
        /// <returns>true якщо все ок</returns>
        bool Connect();

        /// <summary>
        /// Закриття підключення
        /// </summary>
        /// <returns>true якщо все ок</returns>
        bool Close();

        #endregion

        #region DIRECTORY

        /// <summary>
        /// Вибірка ссилок довідника
        /// </summary>
        /// <param name="link">Список для вибраних ссилок</param>
        /// <param name="emptyLink">Пуста ссилка конкретного типу</param>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="limit"></param>
        void DirectorySelectLink(List<DirectoryLink> link, DirectoryLink emptyLink, Dictionary<string, string> where = null, Dictionary<string, string> orderBy = null, int limit = 0);

        /// <summary>
        /// Вибірка полів по ссилці. 
        /// Словник linkFields уже повинен містити поля які потрібно заповнити.
        /// </summary>
        /// <param name="link"></param>
        /// <param name="linkFields"></param>
        void DirectorySelectFieldsByLink(DirectoryLink link, List<DirectoryFieldInfo> fields);

        /// <summary>
        /// Збереження елементу довідника
        /// </summary>
        /// <param name="link"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        DirectoryLink DirectoryObjectSave(DirectoryLink link, List<DirectoryFieldInfo> fields);

        void DirectoryObjectTabularPartSave(List<DirectoryTabularPartRecord> records, string ownerID, string tabularPartTable, List<DirectoryFieldInfo> fields);

        void DirectoryTabularPartSelect(List<DirectoryTabularPartRecord> records, DirectoryTabularPartRecord record, string ownerID, string tabularPartTable, List<DirectoryFieldInfo> fields);


        #endregion

    }
}
