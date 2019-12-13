using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{

  // -------------------------------------------------- //
  //                      Klient4
  // -------------------------------------------------- //

  public class Klient4_Link : DirectoryLink
  {
      public Klient4_Link() : base("table4") { }
      public Klient4_Link(string id) : base("table4", id) { }

      public Klient4_Object GetObject()
      {
          return new Klient4_Object().GetObjectByLink(this);
      }
  }

  public class Klient4_Select : DirectorySelect
  {
      public Klient4_Select() : base("table4") { }

      public List<Klient4_Link> Link { get; private set; }

      public int Select()
      {
          Link = new List<Klient4_Link>();

          List<DirectoryLink> collectionLink = base.SelectLink();

          foreach (DirectoryLink item_link in collectionLink)
              Link.Add(new Klient4_Link(item_link.ID));

          return Link.Count;
      }

      public Klient4_Link SelectOne()
      {
          DirectoryLink item_link = base.SelectLinkOne();
          return new Klient4_Link(item_link.ID);
      }

  }

  public class Klient4_Object : DirectoryObject
  {
      public Klient4_Object() : base("table4") { }

      public string Code { get; set; }
      public string Name { get; set; }
      public string Desc { get; set; }
      public Klient4_Link Tovar { get; set; }
      public Klient4_Link Tovar2 { get; set; }
      public Klient4_Link Tovar5 { get; set; }
      public Klient4_Link Tovar6 { get; set; }

      public Klient4_Object GetObjectByLink(Klient4_Link link)
      {
           Dictionary<string, string> row = link.GetRow();

          Code = row["Code"];
          Name = row["Name"];
          Desc = row["Desc"];
          Tovar = new Klient4_Link(row["Tovar"]);
          Tovar2 = new Klient4_Link(row["Tovar2"]);
          Tovar5 = new Klient4_Link(row["Tovar5"]);
          Tovar6 = new Klient4_Link(row["Tovar6"]);

           return this;
      }
  }

}
