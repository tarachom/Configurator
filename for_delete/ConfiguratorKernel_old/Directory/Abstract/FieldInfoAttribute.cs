using System;

namespace ConfiguratorKernel.Directory
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfoAttribute : System.Attribute
    {
        public FieldInfoAttribute(string typeName, string typeLink)
        {
            TypeName = typeName;
            TypeLink = typeLink;
        }

        public string TypeName { get; private set; }
        public string TypeLink { get; private set; }
    }


}
