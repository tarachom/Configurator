using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConfiguratorKernel.Directory
{
    /// <summary>
    /// Клас ДовідникТабличнаЧастинаЗапис
    /// </summary>
    public abstract class DirectoryTabularPartRecord
    {
        public DirectoryTabularPartRecord()
        {
            ID = "0";
        }

        public string ID { get; set; }
    }
}
