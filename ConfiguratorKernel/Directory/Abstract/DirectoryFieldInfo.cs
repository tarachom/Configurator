using System;

namespace ConfiguratorKernel.Directory
{
    public class DirectoryFieldInfo
    {
        public DirectoryFieldInfo(string fieldName, string fieldType, string fieldTypeLink, object fieldValue =null)
        {
            FieldName = fieldName;
            FieldType = fieldType;
            FieldTypeLink = fieldTypeLink;
            FieldValue = fieldValue;
        }

        public string FieldName { get; private set; }
        public string FieldType { get; private set; }
        public string FieldTypeLink { get; private set; }

        private object mFieldValue;
        public object FieldValue
        {
            get
            {
                return mFieldValue;
            }
            set
            {
                mFieldValue = value;

                if (mFieldValue == null)
                {
                    if (FieldType == "string")
                        mFieldValue = "";
                    else if (FieldType == "integer" || FieldType == "link")
                        mFieldValue = 0;
                }
                else
                {
                    string mFieldValueString = mFieldValue.ToString();

                    if (FieldType == "string")
                        mFieldValue = mFieldValueString;
                    else if (FieldType == "integer" || FieldType == "link")
                        mFieldValue = (mFieldValueString == "" ? 0 : int.Parse(mFieldValueString));
                }
            }
        }
    }
}
