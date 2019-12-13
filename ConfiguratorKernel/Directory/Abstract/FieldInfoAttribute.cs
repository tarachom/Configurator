using System;

namespace ConfiguratorKernel.Directory
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfoAttribute : Attribute
    {
        public FieldInfoAttribute(string fieldType, string fieldTypeLink)
        {
            FieldType = fieldType;
            FieldTypeLink = fieldTypeLink;
        }

        public string FieldType { get; private set; }
        public string FieldTypeLink { get; private set; }
    }
}
