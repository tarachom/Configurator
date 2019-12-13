using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
