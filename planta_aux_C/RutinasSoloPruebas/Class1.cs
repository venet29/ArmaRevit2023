using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.Runtime;

using Autodesk.AutoCAD.DatabaseServices;

using Autodesk.AutoCAD.EditorInput;
using System;
using Newtonsoft.Json;

namespace EntitySelection
{

    public class Commands
    {

        [CommandMethod("SEWP")]

        static public void SelectEntitiesWithProperties()
        {

            Document doc =

              Application.DocumentManager.MdiActiveDocument;

            Editor ed = doc.Editor;

            // Build a conditional filter list so that only

            // entities with the specified properties are

            // selected

            TypedValue[] tvs = new TypedValue[] {

          new TypedValue((int)DxfCode.Operator,"<or"),
          new TypedValue((int)DxfCode.Operator,"<and"),
          new TypedValue((int)DxfCode.LayerName,"0"),
          new TypedValue((int)DxfCode.Start,"LINE"),
          new TypedValue((int)DxfCode.Operator,"and>"),
          new TypedValue((int)DxfCode.Operator,"<and"),
          new TypedValue((int)DxfCode.Start,"CIRCLE"),
          new TypedValue((int)DxfCode.Operator,">="),
          new TypedValue((int)DxfCode.Real, 10 ),
          new TypedValue((int)DxfCode.Operator,"and>"),
          new TypedValue((int)DxfCode.Operator,"or>"

          )

        };

            SelectionFilter sf = new SelectionFilter(tvs);
            PromptSelectionResult psr = ed.SelectAll(sf);
            ed.WriteMessage("\nFound {0} entit{1}.", psr.Value.Count, (psr.Value.Count == 1 ? "y" : "ies"));

        }

        //        public static void ShowSerializeDeserialize()
        //        {
        //            Console.Clear();
        //            string xavierJson = @"{
        //                                    'name': 'Xavier Morera',
        //                                    'courses': [
        //                                        'Solr',
        //                                        'dotTrace'
        //                                        ],
        //                                    'since': '2014-01-14T00:00:00',
        //                                    'happy': true,
        //                                    'issues': null,
        //                                    'car': {
        //                                        'model': 'Land Rover Series III',
        //                                        'year': 1976
        //                                        },
        //                                    'authorRelationship': 1
        //                                }";

        //            Console.WriteLine("Step 1: Output JSON");
        //            Console.WriteLine(xavierJson);

        //            Console.WriteLine(Environment.NewLine + "Step 2: Output property Author.Name from deserialized class");
        //            Author xavierAuthor = JsonConvert.DeserializeObject<Author>(xavierJson);
        //            Console.WriteLine(xavierAuthor.name);

        //            Console.WriteLine(Environment.NewLine + "Step 3: Output serialized Author class");
        //            string xavierJsonSerialized = JsonConvert.SerializeObject(xavierAuthor);
        //            Console.WriteLine(xavierJsonSerialized);

        //            Console.WriteLine(Environment.NewLine + "Step 4: Output serialized Author class with indentation");
        //            string xavierJsonSerializedIndented = JsonConvert.SerializeObject(xavierAuthor, Formatting.Indented);
        //            Console.WriteLine(xavierJsonSerializedIndented);
        //        }

        //        //**********************************************************************
        //        public class Author
        //        {
        //            public string country;
        //            public int age;
        //            public string name { get; set; }
        //            public string[] courses { get; set; }
        //            public DateTime since { get; set; }
        //            public bool happy { get; set; }
        //            public object issues { get; set; }
        //            public Car car { get; set; }
        //            public List<Author> favoriteAuthors { get; set; }
        //            public Relationship authorRelationship { get; set; }
        //        }

        //        public class Car
        //        {
        //            public string model { get; set; }
        //            public int year { get; set; }
        //        }

        //        public enum Relationship
        //        {
        //            EmployeeAuthor,
        //            IndependentAuthor
        //        }


    }

}