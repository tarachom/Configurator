using System;
using System.Collections.Generic;
using System.Text;

using MySql.Data.MySqlClient;

namespace ConfiguratorKernel
{
    /// <summary>
    /// Клас для роботи з MySQL
    /// </summary>
    public class MySqlData : IData
    {
        #region CONNECT CLOSE

        /// <summary>
        /// Поточне підключення
        /// </summary>
        private MySqlConnection mConnect { get; set; }

        /// <summary>
        /// Строка підключення
        /// </summary>
        public string ConnectString { get; set; }

        /// <summary>
        /// Підключення до бази даних
        /// </summary>
        /// <returns>true якщо ок</returns>
        public bool Connect()
        {
            mConnect = new MySqlConnection(ConnectString);

            try
            {
                mConnect.Open();
                return true;
            }
            catch (MySqlException e)
            {
                mConnect = null;
                return false;
            }
        }

        /// <summary>
        /// Закриття підключення
        /// </summary>
        /// <returns>true якщо ок</returns>
        public bool Close()
        {
            try
            {
                mConnect.Close();
                return true;
            }
            catch (MySqlException e)
            {
                mConnect = null;
                return false;
            }
        }

        #endregion
    }
}
