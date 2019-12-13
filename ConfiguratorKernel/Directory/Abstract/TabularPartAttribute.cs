using System;

namespace ConfiguratorKernel.Directory
{
    [AttributeUsage(AttributeTargets.Property)]
    public class TabularPartAttribute : Attribute
    {
        public TabularPartAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }


}
