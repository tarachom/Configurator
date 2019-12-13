using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ConfiguratorKernel.Directory
{
    public class TMC_Link : DirectoryLink
    {
        public TMC_Link() : base("Table1") { }
        public TMC_Link(string id) : base("Table1", id) { }

        public TMC_Object GetObject()
        {
            return new TMC_Object().GetObjectByLink(this);
        }
    }
    
    public class TMC_Object: DirectoryObject
    {
        public TMC_Object() : base("Table1") { }

        [FieldInfo("string", "")]
        public string Code { get; set; }

        [FieldInfo("string", "")]
        public string Name { get; set; }

        [FieldInfo("string", "")]
        public string Desc { get; set; }

        [FieldInfo("link", "TMC_Link")]
        public TMC_Link klient { get; set; }

        [FieldInfo("integer", "")]
        public int Count { get; set; }

        public TMC_Object GetObjectByLink(TMC_Link link)
        {
            GetObjectByLinkBase(link);

            return this;
        }
    }

    public class TMC_Select : DirectorySelect
    {
        public TMC_Select() : base("Table1") { }

        public List<TMC_Link> Link { get; private set; }

        public int Select()
        {
            //Link = new List<TMC_Link>();

            //base.SelectLink(Link);

            //Копіювання
            //foreach (DirectoryLink item_link in collectionLink)
            //    Link.Add(new TMC_Link(item_link.ID));

            return Link.Count;
        }

        public TMC_Link SelectOne()
        {
            DirectoryLink item_link = base.SelectLinkOne();
            return new TMC_Link(item_link.ID);
        }
    }

    public class TMC_Test
    {
        public void test()
        {
            Debug.WriteLine("---------------------------------");

            TMC_Link tl = new TMC_Link("10");
            TMC_Object toa = tl.GetObject();

            TMC_Object to = new TMC_Object();
            to.GetObjectByLink(tl);

            TMC_Select ts = new TMC_Select();
            ts.Select();
            
        }
    }

}
