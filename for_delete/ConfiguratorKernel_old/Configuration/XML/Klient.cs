using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{

  // -------------------------------------------------- //
  //                      Klient
  // -------------------------------------------------- //

  public class Klient_Link : DirectoryLink
  {
      public Klient_Link() : base("table2") { }
      public Klient_Link(string id) : base("table2", id) { }

      public Klient_Object GetObject()
      {
          return new Klient_Object().GetObjectByLink(this);
      }
  }

  public class Klient_Select : DirectorySelect
  {
      public Klient_Select() : base("table2") { }

      public List<Klient_Link> Link { get; private set; }

      public int Select()
      {
          Link = new List<Klient_Link>();

          List<DirectoryLink> collectionLink = base.SelectLink();

          foreach (DirectoryLink item_link in collectionLink)
              Link.Add(new Klient_Link(item_link.ID));

          return Link.Count;
      }

      public Klient_Link SelectOne()
      {
          DirectoryLink item_link = base.SelectLinkOne();
          return new Klient_Link(item_link.ID);
      }

  }

  public class Klient_Object : DirectoryObject
  {
      public Klient_Object() : base("table2") { }

      public string Code { get; set; }
      public string Name { get; set; }
      public string Desc { get; set; }
      public Klient_Link Tovar { get; set; }

      public Klient_Object GetObjectByLink(Klient_Link link)
      {
           Dictionary<string, string> row = link.GetRow();

          Code = row["Code"];
          Name = row["Name"];
          Desc = row["Desc"];
          Tovar = new Klient_Link(row["Tovar"]);

           return this;
      }
  }

}
