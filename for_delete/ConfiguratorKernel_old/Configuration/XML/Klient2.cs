using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{

  // -------------------------------------------------- //
  //                      Klient2
  // -------------------------------------------------- //

  public class Klient2_Link : DirectoryLink
  {
      public Klient2_Link() : base("table3") { }
      public Klient2_Link(string id) : base("table3", id) { }

      public Klient2_Object GetObject()
      {
          return new Klient2_Object().GetObjectByLink(this);
      }
  }

  public class Klient2_Select : DirectorySelect
  {
      public Klient2_Select() : base("table3") { }

      public List<Klient2_Link> Link { get; private set; }

      public int Select()
      {
          Link = new List<Klient2_Link>();

          List<DirectoryLink> collectionLink = base.SelectLink();

          foreach (DirectoryLink item_link in collectionLink)
              Link.Add(new Klient2_Link(item_link.ID));

          return Link.Count;
      }

      public Klient2_Link SelectOne()
      {
          DirectoryLink item_link = base.SelectLinkOne();
          return new Klient2_Link(item_link.ID);
      }

  }

  public class Klient2_Object : DirectoryObject
  {
      public Klient2_Object() : base("table3") { }

      public string Code { get; set; }
      public string Name { get; set; }
      public string Desc { get; set; }
      public Klient2_Link Tovar { get; set; }
      public Klient2_Link Tovar2 { get; set; }

      public Klient2_Object GetObjectByLink(Klient2_Link link)
      {
           Dictionary<string, string> row = link.GetRow();

          Code = row["Code"];
          Name = row["Name"];
          Desc = row["Desc"];
          Tovar = new Klient2_Link(row["Tovar"]);
          Tovar2 = new Klient2_Link(row["Tovar2"]);

           return this;
      }
  }

}
