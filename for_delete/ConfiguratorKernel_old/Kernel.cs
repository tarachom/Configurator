using System;
using System.Collections.Generic;
using System.Text;

namespace ConfiguratorKernel
{
    /// <summary>
    /// Ядро
    /// </summary>
    public class Kernel
    {
        /// <summary>
        /// Поточний інтерфейс бази даних
        /// </summary>
        private IData mIData { get; set; }

        /// <summary>
        /// Конструктор.
        /// Підключення до бази даних
        /// </summary>
        public Kernel()
        {
            mIData = new MySqlData();
            mIData.ConnectString = "Database=image;Data Source=localhost;User Id=root;Password=525491;";

            if (!mIData.Connect())
                throw new Exception("Невдалось підключитись до бази");
        }
    }
}
