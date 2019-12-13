using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.XPath;

using ConfiguratorKernel;
using ConfiguratorKernel.Directory;

namespace Configurator
{
    class Program
    {
        static void Main(string[] args)
        {
            Kernel.Connect();

            TMC_Object newTMC = new TMC_Object();

            TMC_Select ts = new TMC_Select();
            ts.Select();
            Console.WriteLine(ts.Link.Count);

            TMC_Link tmc_link = (TMC_Link)ts.Link[0];

            Console.WriteLine("Get");
            newTMC = tmc_link.GetObject();
            Console.WriteLine("Ok");

            //for(int j=0; j < 500; j++)
            //{
            //    newTMC.TableA.Records.RemoveAt(j);
            //    newTMC.TableB.Records.RemoveAt(j);
            //}

            //Console.WriteLine("Write");
            //for (int i = 0; i < 1000; i++)
            //{
            //    TMC_TableA_Record rnewA = new TMC_TableA_Record();
            //    rnewA.Code = "44444";
            //    rnewA.Name = "44444";
            //    newTMC.TableA.Records.Add(rnewA);

            //    TMC_TableB_Record rnew = new TMC_TableB_Record();
            //    rnew.Code = "1000";
            //    rnew.Name = "1000";
            //    rnew.Desc = "1000";
            //    rnew.Count = 1000;
            //    newTMC.TableB.Records.Add(rnew);

            //    //Console.WriteLine(i);
            //}
            //Console.WriteLine("Ok");

            Console.WriteLine("Save");
            newTMC.Save();
            Console.WriteLine("Ok");
            

            Console.ReadLine();

            Kernel.Close();
        }

        static void CreateConfig()
        {
            string FolderXML = @"D:\Developer\VS\Configurator\ConfiguratorKernel\Configuration\XML\";

            XPathDocument xpDoc = new XPathDocument(FolderXML + "Configuration.xml");
            XPathNavigator xpDocNavigator = xpDoc.CreateNavigator();

            XPathNodeIterator nodes = xpDocNavigator.Select("/Configuration/Directory/Directory");
            while (nodes.MoveNext())
            {
                string directory_table = nodes.Current.GetAttribute("table", "");
                string directory_name = nodes.Current.GetAttribute("name", "");
                string directory_desc = nodes.Current.GetAttribute("desc", "");

                TextWriter tw = File.CreateText(FolderXML + directory_name + ".cs");

                tw.WriteLine("using System.Collections.Generic;");
                tw.WriteLine();
                tw.WriteLine("namespace ConfiguratorKernel.Directory");
                tw.WriteLine("{");

                // LINK

                tw.WriteLine();
                tw.WriteLine("  // -------------------------------------------------- //");
                tw.WriteLine("  //                      " + directory_name);
                tw.WriteLine("  // -------------------------------------------------- //");
                tw.WriteLine();

                tw.WriteLine("  public class " + directory_name + "_Link : DirectoryLink");
                tw.WriteLine("  {");
                tw.WriteLine("      public " + directory_name + "_Link() : base(\"" + directory_table + "\") { }");
                tw.WriteLine("      public " + directory_name + "_Link(string id) : base(\"" + directory_table + "\", id) { }");
                tw.WriteLine();
                tw.WriteLine("      public " + directory_name + "_Object GetObject()");
                tw.WriteLine("      {");
                tw.WriteLine("          return new " + directory_name + "_Object().GetObjectByLink(this);");
                tw.WriteLine("      }");
                tw.WriteLine("  }");
                tw.WriteLine();

                // SELECT

                tw.WriteLine("  public class " + directory_name + "_Select : DirectorySelect");
                tw.WriteLine("  {");
                tw.WriteLine("      public " + directory_name + "_Select() : base(\"" + directory_table + "\") { }");
                tw.WriteLine();
                tw.WriteLine("      public List<" + directory_name + "_Link> Link { get; private set; }");
                tw.WriteLine();
                tw.WriteLine("      public int Select()");
                tw.WriteLine("      {");
                tw.WriteLine("          Link = new List<" + directory_name + "_Link>();");
                tw.WriteLine();
                tw.WriteLine("          List<DirectoryLink> collectionLink = base.SelectLink();");
                tw.WriteLine();
                tw.WriteLine("          foreach (DirectoryLink item_link in collectionLink)");
                tw.WriteLine("              Link.Add(new " + directory_name + "_Link(item_link.ID));");
                tw.WriteLine();
                tw.WriteLine("          return Link.Count;");
                tw.WriteLine("      }");
                tw.WriteLine();
                tw.WriteLine("      public " + directory_name + "_Link SelectOne()");
                tw.WriteLine("      {");
                tw.WriteLine("          DirectoryLink item_link = base.SelectLinkOne();");
                tw.WriteLine("          return new " + directory_name + "_Link(item_link.ID);");
                tw.WriteLine("      }");
                tw.WriteLine();
                tw.WriteLine("  }");
                tw.WriteLine();

                // OBJECT

                XPathNodeIterator nodesField = nodes.Current.Select("Fields/Field");

                tw.WriteLine("  public class " + directory_name + "_Object : DirectoryObject");
                tw.WriteLine("  {");
                tw.WriteLine("      public " + directory_name + "_Object() : base(\"" + directory_table + "\") { }");
                tw.WriteLine();

                while (nodesField.MoveNext())
                {
                    string field_name = nodesField.Current.GetAttribute("name", "");
                    string field_type = nodesField.Current.GetAttribute("type", "");

                    if (field_type == "link")
                        tw.WriteLine("      public " + directory_name + "_Link " + field_name + " { get; set; }");
                    else
                        tw.WriteLine("      public string " + field_name + " { get; set; }");
                }

                XPathNodeIterator nodesField2 = nodes.Current.Select("Fields/Field");

                tw.WriteLine();
                tw.WriteLine("      public " + directory_name + "_Object GetObjectByLink(" + directory_name + "_Link link)");
                tw.WriteLine("      {");
                tw.WriteLine("           Dictionary<string, string> row = link.GetRow();");
                tw.WriteLine();

                while (nodesField2.MoveNext())
                {
                    string field_name = nodesField2.Current.GetAttribute("name", "");
                    string field_type = nodesField2.Current.GetAttribute("type", "");

                    if (field_type == "link")
                        tw.WriteLine("          " + field_name + " = new " + directory_name + "_Link(row[\"" + field_name + "\"]);");
                    else
                        tw.WriteLine("          " + field_name + " = row[\"" + field_name + "\"];");
                }

                tw.WriteLine();
                tw.WriteLine("           return this;");
                tw.WriteLine("      }");
                tw.WriteLine("  }");
                tw.WriteLine();

                tw.WriteLine("}");

                tw.Flush();
                tw.Close();
            }
        }
    }
}
