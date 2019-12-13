using System.Collections.Generic;

namespace ConfiguratorKernel.Directory
{

  // -------------------------------------------------- //
  //                      Tovar
  // -------------------------------------------------- //

  public class Tovar_Link : DirectoryLink
  {
      public Tovar_Link() : base("table1") { }
      public Tovar_Link(string id) : base("table1", id) { }

      public Tovar_Object GetObject()
      {
          return new Tovar_Object().GetObjectByLink(this);
      }
  }

  public class Tovar_Select : DirectorySelect
  {
      public Tovar_Select() : base("table1") { }

      public List<Tovar_Link> Link { get; private set; }

      public int Select()
      {
          Link = new List<Tovar_Link>();

          List<DirectoryLink> collectionLink = base.SelectLink();

          foreach (DirectoryLink item_link in collectionLink)
              Link.Add(new Tovar_Link(item_link.ID));

          return Link.Count;
      }

      public Tovar_Link SelectOne()
      {
          DirectoryLink item_link = base.SelectLinkOne();
          return new Tovar_Link(item_link.ID);
      }

  }

  public class Tovar_Object : DirectoryObject
  {
      public Tovar_Object() : base("table1") { }

      public string Code { get; set; }
      public string Name { get; set; }
      public string Description { get; set; }

      public Tovar_Object GetObjectByLink(Tovar_Link link)
      {
           Dictionary<string, string> row = link.GetRow();

          Code = row["Code"];
          Name = row["Name"];
          Description = row["Description"];

           return this;
      }
  }

}
