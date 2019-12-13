using System;

namespace ConfiguratorKernel
{
    public static class Kernel
    {
        /// <summary>
        /// Поточний інтерфейс бази даних
        /// </summary>
        public static IData ChannelData { get; private set; }

        /// <summary>
        /// Підключення до бази даних
        /// </summary>
        public static void Connect()
        {
            ChannelData = new MySqlData();
            ChannelData.ConnectString = "Database=configurator;Data Source=localhost;User Id=root;Password=1;";

            if (!ChannelData.Connect())
                throw new Exception("Невдалось підключитись до бази");
        }

        public static void Close()
        {
            ChannelData.Close();
        }
    }
}
