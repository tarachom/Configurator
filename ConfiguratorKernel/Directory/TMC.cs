using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ConfiguratorKernel.Directory
{
    public class TMC_Link : DirectoryLink
    {
        public TMC_Link() : base("TMC") { }
        public TMC_Link(string id) : base("TMC", id) { }

        public TMC_Object GetObject()
        {
            return (TMC_Object)new TMC_Object().GetObjectByLink(this);
        }
    }
    
    public class TMC_Object: DirectoryObject
    {
        public TMC_Object() : base("TMC", new TMC_Link())
        {
            TableA = new TMC_TableA(this);
            TableB = new TMC_TableB(this);
        }

        [FieldInfo("string", "")]
        public string Code { get; set; }

        [FieldInfo("string", "")]
        public string Name { get; set; }

        [FieldInfo("string", "")]
        public string Desc { get; set; }

        [FieldInfo("integer", "")]
        public int Count { get; set; }

        [TabularPart("TableA")]
        public TMC_TableA TableA { get; private set; }

        [TabularPart("TableB")]
        public TMC_TableB TableB { get; private set; }
    }

    public class TMC_Select : DirectorySelect
    {
        public TMC_Select() : base("TMC", new TMC_Link()) { }
    }




    public class TMC_TableA : DirectoryTabularPart
    {
        public TMC_TableA(TMC_Object owner) : base("TMC_TableA", owner, new TMC_TableA_Record()) { }
    }

    public class TMC_TableA_Record : DirectoryTabularPartRecord
    {
        [FieldInfo("string", "")]
        public string Code { get; set; }

        [FieldInfo("string", "")]
        public string Name { get; set; }
    }

    public class TMC_TableB : DirectoryTabularPart
    {
        public TMC_TableB(TMC_Object owner) : base("TMC_TableB", owner, new TMC_TableB_Record()) { }
    }

    public class TMC_TableB_Record : DirectoryTabularPartRecord
    {
        [FieldInfo("string", "")]
        public string Code { get; set; }

        [FieldInfo("string", "")]
        public string Name { get; set; }

        [FieldInfo("string", "")]
        public string Desc { get; set; }

        [FieldInfo("integer", "")]
        public int Count { get; set; }
    }
}
