#region Header
//
// (C) Copyright 2003-2010 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
#endregion // Header

#region Namespaces
using System.Collections.Generic;
using System.Diagnostics; // for the Stopwatch class
using System.Linq;
using System.Xml.Linq; // to use XML LINQ
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace FilterExamples
{
  #region Static helpers utility class
  public class Util
  {
    public static string RealString( double a )
    {
      return a.ToString( "0.##" );
    }

    public static string PointString( XYZ p )
    {
      return string.Format( "({0},{1},{2})",
        RealString( p.X ),
        RealString( p.Y ),
        RealString( p.Z ) );
    }
    
    public static void ShowTaskDialog( string title, string info, string content )
    {
      Debug.Print( title + ": " + info + ": " + content );

      TaskDialog dialog = new TaskDialog( title );
      dialog.MainInstruction = info;
      dialog.MainContent = content;
      dialog.Show();
    }

    public static void ShowElapsedTime( Stopwatch stopwatch, string info )
    {
      Debug.Print( stopwatch.ElapsedMilliseconds.ToString() + " milliseconds" );

      TaskDialog dialog = new TaskDialog( "Time" );
      dialog.MainInstruction = "milliseconds: " + stopwatch.ElapsedMilliseconds.ToString();
      dialog.MainContent = info;
      dialog.Show();
    }

    public static Material FindMaterial( Document doc, string name )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( Material ) );

      // Now use LINQ to see if one exists with provided name.
      Material materialReturn = null;
      try
      {
        Element e = collector.First( material => material.Name.Equals( name ) );
        materialReturn = e as Material;
        if( e != null )
          return materialReturn;
      }
      catch( System.InvalidOperationException )
      {
      }
      catch( System.ArgumentNullException )
      {
      }
      return materialReturn;
    }

    public static ElementId FindLevelId( Document doc, string name )
    {
      // Simple example of finding all levels.
      FilteredElementCollector collectorLevels = new FilteredElementCollector( doc );
      collectorLevels.OfClass( typeof( Level ) );

      // Now use LINQ to see if one exists with provided name.
      ElementId idLevel = ElementId.InvalidElementId;
      try
      {
        Element levelMatched = collectorLevels.First( level => level.Name.Equals( name ) );
        if( levelMatched != null )
          idLevel = levelMatched.Id;
      }
      catch( System.InvalidOperationException )
      {
      }
      catch( System.ArgumentNullException )
      {
      }
      return idLevel;
    }

    public static Level FindLevel( Document doc, string name )
    {
      // Simple example of finding all levels.
      FilteredElementCollector collectorLevels = new FilteredElementCollector( doc );
      collectorLevels.OfClass( typeof( Level ) );

      // Now use LINQ to see if one exists with provided name.
      Level levelMatched = null;
      try
      {
        levelMatched = collectorLevels.First( level => level.Name.Equals( name ) ) as Level;
      }
      catch( System.InvalidOperationException )
      {
      }
      catch( System.ArgumentNullException )
      {
      }
      return levelMatched;
    }

    public static ElementId FindAnalysisDisplayStyleId( Document doc, string name )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( AnalysisDisplayStyle ) );

      // Now use LINQ to see if one exists with provided name.
      ElementId idAds = ElementId.InvalidElementId;
      try
      {
        Element adsMatched = collector.First( ads => ads.Name.Equals( name ) );
        if( adsMatched != null )
          idAds = adsMatched.Id;
      }
      catch( System.InvalidOperationException )
      {
      }
      catch( System.ArgumentNullException )
      {
      }
      return idAds;
    }

    public static Solid FindSolid( GeometryObjectArray geomObjs )
    {
      foreach( GeometryObject obj in geomObjs )
        if( obj is Solid )
          return obj as Solid;

      return null;
    }
  }
  #endregion

  #region Performance Example 1 Emulate old
  /// <summary>
  /// This example shows a technique similar to the "old" way of finding information 
  /// and is not very efficient.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the StructuralUsage.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class PerformanceEx1_old : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Stopwatch sw = new Stopwatch();
      sw.Reset();
      sw.Start();

      // Old get_Elements() doesn't exist so in 2011 we get the FamilyInstances 
      // the new way using the OfClass shortcut. Even this is faster than 2010, 
      // so not entirely acturate performance demo, but shows how the direct 
      // filter can find things fast because the native code is searching 
      // instead of the managed code like here.
      FilteredElementCollector collector = new FilteredElementCollector( activeDoc );
      collector.OfClass( typeof( FamilyInstance ) );
      FilteredElementIterator iter = collector.GetElementIterator();

      // Process in the typical old way looping through to find the ones we want
      // FamilyInstance with Structural Material == Steel
      // and any kind of Structural Instance USage that is not listed.
      int nCount = 0;
      string report = string.Empty;
      ElementId categoryId = new ElementId( BuiltInCategory.OST_StructuralColumns );
      while( !iter.IsDone() )
      {
        FamilyInstance instance = iter.GetCurrent() as FamilyInstance;
        if( ( instance != null ) && ( instance.Category.Id == categoryId ) )
        {
          report += "\nName = " + instance.Name;
          nCount++;
        }
        iter.MoveNext();
      }

      report += "\nFound :" + nCount.ToString();

      sw.Stop();
      Util.ShowElapsedTime( sw, report );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Performance Example 2 new
  /// <summary>
  /// This example shows a new quick filter example for category.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the StructuralUsage.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class PerformanceEx2_new : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Stopwatch sw = new Stopwatch();
      sw.Start();

      FilteredElementCollector collectorFamilyInstance = new FilteredElementCollector( activeDoc );

      // Show how the direct filter can find things fast because the native code is searching 
      // instead of the managed code like above.
      ICollection<Element> instances = collectorFamilyInstance.OfClass( typeof( FamilyInstance ) ).OfCategory( BuiltInCategory.OST_StructuralColumns ).ToElements();

      // generate a report to display in the dialog.
      int nCount = 0;
      string report = string.Empty;
      foreach( FamilyInstance instance in instances )
      {
        // Category match was included in filter above, so no need to check it in managed code.
        report += "\nName = " + instance.Name;
        nCount++;
      }

      report += "\nNew way Found " + nCount.ToString();

      sw.Stop();
      Util.ShowElapsedTime( sw, report );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Filter Example 1
  /// <summary>
  /// Filter example 01.
  /// This example uses the StructuralMaterialTypeFilter, to find all elements that have StructuralMaterialType.Steel.
  /// It then creates four inverted StructuralInstanceUsageFilter to eliminate instance usage of Wall, Undefined, Other, and Automatic,
  /// to effectively find those that match Brace, Column, Girder, Horizontal Bracing, Joist, Kicker Bracing, Purlin, or Truss Chord.
  /// These filters are then passed to the LogocalAndFilter to find FamilyInstances that matches this criteria.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the StructuralUsage.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class FilterEx1 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      // Filter for everything that uses structural steel for material type.
      StructuralMaterialTypeFilter structMatFilter = new StructuralMaterialTypeFilter( StructuralMaterialType.Steel );

      // Now lets setup for finding truely structural instances (Like brace, column, girder, etc.)
      // because there are more of the types we want, why not just exclude the types we do not want with inverted filter.

      // Find everything but Walls, Undefined, Other, and Automatic.
      StructuralInstanceUsageFilter structInstanceFilterNotWall = new StructuralInstanceUsageFilter( StructuralInstanceUsage.Wall, true );
      StructuralInstanceUsageFilter structInstanceFilterNotUndefined = new StructuralInstanceUsageFilter( StructuralInstanceUsage.Undefined, true );
      StructuralInstanceUsageFilter structInstanceFilterNotOther = new StructuralInstanceUsageFilter( StructuralInstanceUsage.Other, true );
      StructuralInstanceUsageFilter structInstanceFilterNotAuto = new StructuralInstanceUsageFilter( StructuralInstanceUsage.Automatic, true );

      // Now add them to a ElementFilter list so we can pass it to the the logical And filter.
      IList<ElementFilter> filters = new List<ElementFilter>();
      filters.Add( structMatFilter );

      filters.Add( structInstanceFilterNotWall );
      filters.Add( structInstanceFilterNotUndefined );
      filters.Add( structInstanceFilterNotOther );
      filters.Add( structInstanceFilterNotAuto );

      FilteredElementCollector collectorFamilyInstance = new FilteredElementCollector( activeDoc );
      // note that this collector has 7 seperate filtering criteria.
      ICollection<Element> instances = collectorFamilyInstance.OfClass( typeof( FamilyInstance ) ).WherePasses( new LogicalAndFilter( filters ) ).ToElements();

      // generate a report to display.
      int nCount = 0;
      string report = string.Empty;
      foreach( FamilyInstance instance in instances )
      {
        string name = instance.Name;
        string matType = instance.StructuralMaterialType.ToString();
        string instanceUsage = instance.StructuralUsage.ToString();
        report += "\nName = " + name + "   Material Type = " + matType + "   Structural Usage = " + instanceUsage + "   Element Id: " + instance.Id.ToString();
        nCount++;
      }

      report += "\nFound " + nCount.ToString();

      Util.ShowTaskDialog( "Structural Information", string.Empty, report );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Filter Example 2
  /// <summary>
  /// Filter example 02.
  /// This example uses the BoundingBoxIntersectsFilter
  /// to find all elements that intersect a selected element.
  /// It also uses the collector viewId constructor to narrow 
  /// down to only viewable elements, and finally an 
  /// exclusion filter to eliminate known elements to exclude.
  /// </summary>
  /// <remarks>
  /// This is meant to run in the StructuralUsage.rvt model.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class FilterEx2 : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document 
      // from external command data.

      UIApplication uiApp = commandData.Application;
      UIDocument uiDoc = uiApp.ActiveUIDocument;
      Application app = uiApp.Application;
      Document activeDoc = uiDoc.Document;

      // Select something to use as base bounding box.

      Reference reference = uiDoc.Selection.PickObject(
        Autodesk.Revit.UI.Selection.ObjectType.Element );

      // Find the bounding box from the selected 
      // object and convert to outline.

      BoundingBoxXYZ bb = reference.Element.get_BoundingBox(activeDoc.ActiveView );

      Outline outline = new Outline( bb.Min, bb.Max );

      // Create a BoundingBoxIntersectsFilter to 
      // find everything intersecting the bounding 
      // box of the selected element.

      BoundingBoxIntersectsFilter bbfilter 
        = new BoundingBoxIntersectsFilter( outline );

      // Use a view to construct the filter so we 
      // get only visible elements. For example, 
      // the analytical model will be found otherwise.

      FilteredElementCollector collector 
        = new FilteredElementCollector( 
          activeDoc, activeDoc.ActiveView.Id );

      // Lets also exclude the view itself (which 
      // often will have an intersecting bounding box), 
      // and also the element selected.

      ICollection<ElementId> idsExclude 
        = new List<ElementId>();

      idsExclude.Add( reference.Element.Id );
      idsExclude.Add( activeDoc.ActiveView.Id );

      // Get the set of elements that pass the 
      // criteria. Note both filters are quick, 
      // so order is not so important.

      ICollection<Element> elems 
        = collector.Excluding( idsExclude )
          .WherePasses( bbfilter )
          .ToElements();

      // Generate a report to display in the dialog.

      int nCount = 0;
      string report = string.Empty;
      foreach( Element e in elems )
      {
        string name = e.Name;

        report += "\nName = " + name 
          + " Element Id: " + e.Id.ToString();

        nCount++;
      }

      report += "\nFound " + nCount.ToString();

      Util.ShowTaskDialog( 
        "Elements whose Bounding Box intersects selected", 
        string.Empty, report );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Linq Example 1
  /// <summary>
  /// LINQ example 01.
  /// This example uses the ElementClassFilter, and LINQ to find elements with a matching material.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the ArchSample.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class LinqEx1 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      // Find all FamilyInstances
      FilteredElementCollector collectorFamInst = new FilteredElementCollector( activeDoc );
      collectorFamInst.OfClass( typeof( FamilyInstance ) );

      // Further filter down to only those with specified category.
      BuiltInCategory idCategoryFurniture = BuiltInCategory.OST_Furniture;
      collectorFamInst.OfCategory( idCategoryFurniture );

      // Find the desired material
      string nameMaterial = "Textile - Black";
      Material material = Util.FindMaterial( activeDoc, nameMaterial );

      // Now we need to work with FamilyInstance (materials) 
      IEnumerable<FamilyInstance> listFamInst = collectorFamInst.OfType<FamilyInstance>();

      // Use LINQ to further filter down those that match the desired material.
      IEnumerable<FamilyInstance> furnitureWithMaterialLinq =
                              from element in listFamInst
                              where element.Materials.Contains( material )
                              select element;

      // Now that we have only what we want, further foreach process them.
      string names = string.Empty;
      foreach( FamilyInstance e in furnitureWithMaterialLinq )
        names += "\n" + e.Name + " id: " + e.Id.ToString();


      Util.ShowTaskDialog( "Material Example", "Furniture Names and ids with specified material: " + nameMaterial, names );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Linq Example 2
  /// <summary>
  /// LINQ example 02.
  /// This example uses the ElementClassFilter, and LINQ to find Mullions.
  /// Because Mullions are not a native revit class, we cannot filter directly for it.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the ArchSample.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class LinqEx2 : IExternalCommand
  {

    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Stopwatch sw = new Stopwatch();

      // Find all Mullions; Mullion are not an internal type, so cannot filter on it directly.
      // The class filter will throw an exception because internally we cannot match it in native code.
      // See ElementClassFilter for docs listing all examples of this situation.
      FilteredElementCollector collectorMullion = new FilteredElementCollector( activeDoc );
      collectorMullion.OfClass( typeof( FamilyInstance ) );

      // Now we need to work with Mullion specifically to access "MullionType"
      // so we will transform our element list into a mullion list.
      sw.Reset();
      sw.Start();
      IEnumerable<Mullion> listMullion = collectorMullion.OfType<Mullion>();
      sw.Stop();
      Util.ShowElapsedTime( sw, "Mullions found: " + listMullion.Count().ToString() );

      // Note above filters ONLY mullions into new list. If we converted to FamilyInstance,
      // you can see there were many more family instances found. This just to show how the transform
      // helps you to narrow down based on the API types, too.
      IEnumerable<FamilyInstance> listFamilyInstance = collectorMullion.OfType<FamilyInstance>();
      Util.ShowTaskDialog( "Mullions Example", "FamilyInstances found: " + listFamilyInstance.Count().ToString(), string.Empty );

      // just for example sake, show the conversion not using IEnumerable.OfType.
      sw.Reset();
      sw.Start();
      List<Mullion> alternatelistMullion = new List<Mullion>();
      IList<Element> alternatelistFamilyInstances = collectorMullion.ToElements();
      foreach( FamilyInstance fi in alternatelistFamilyInstances )
      {
        Mullion m = fi as Mullion;
        if( m != null )
          alternatelistMullion.Add( m );
      }
      sw.Stop();
      Util.ShowElapsedTime( sw, "Alternate way - Mullions found: " + alternatelistMullion.Count.ToString() );

      // Use LINQ to further filter down those that match the desired material.
      IEnumerable<Mullion> listMullionWithType =
                              from mullion in listMullion
                              where mullion.MullionType.Name.Equals( "thin horizontal" )
                              select mullion;


      // Now that we have only what we want, further foreach process them.

      string names = string.Empty;
      foreach( Mullion mullion in listMullionWithType )
        names += "\n" + mullion.MullionType.Name + " id: " + mullion.Id.ToString();

      Util.ShowTaskDialog( "Mullions Example", "Mullions with specified type: " + listMullionWithType.Count().ToString(), names );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Linq Example 3
  /// <summary>
  /// LINQ example 03.
  /// This example uses the ElementClassFilter, FamilySymbolFilter, and FamilyInstanceFilter
  /// to gather infomration about each Family, then find each symbol within the family, and finally
  /// find all the instances of each symbol. 
  /// This will produce an abundance of information in a typical project, so we will also use 
  /// the LINQ to XML functional approach (aka "DOM free") to produce a Family Inventory of the 
  /// model in a nice XML format.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the ArchSample.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class LinqEx3 : IExternalCommand
  {
    /// <summary>
    /// Return the family instance location.
    /// This consists of the endpoints of the given 
    /// family instance location curve, if it has one, 
    /// else its location point, if it has one, 
    /// else "<none>".
    /// </summary>
    static string Position( FamilyInstance fi )
    {
      LocationPoint pt = null;
      LocationCurve curve = null;
      return (null == ( pt = fi.Location as LocationPoint )
        ? ( ( null == (curve = fi.Location as LocationCurve)
          ? "<none>" 
          : Util.PointString( curve.Curve.get_EndPoint( 0 ) ) 
            + " to " + Util.PointString( curve.Curve.get_EndPoint( 1 ) ) ) )
        : Util.PointString( pt.Point ) );
    }

    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Stopwatch sw = Stopwatch.StartNew();

      XElement xmlFamilyInstance = new XElement( "Family_Inventory" );

      // Start by getting all families. 
      // We use the ElementClassFilter shortcut and filter all "Family" elements.
      FilteredElementCollector collectorFamily = new FilteredElementCollector( activeDoc );

      ICollection<Element> families = collectorFamily.OfClass( typeof( Family ) ).ToElements();

      foreach( Family family in families )
      {
        // XML: Start by adding the Family element
        XElement temp = new XElement( "FamilyName", family.Name );

        // Now use the FamilySymbolFilter for each Family.
        FamilySymbolFilter filterFamSym = new FamilySymbolFilter( family.Id );
        FilteredElementCollector collectorFamSym = new FilteredElementCollector( activeDoc );

        ICollection<Element> famSymbols = collectorFamSym.WherePasses( filterFamSym ).ToElements();
        foreach( FamilySymbol famSymbol in famSymbols )
        {
          FamilyInstanceFilter filterFamilyInst = new FamilyInstanceFilter( activeDoc, famSymbol.Id );
          FilteredElementCollector collectorFamInstances = new FilteredElementCollector( activeDoc, activeDoc.ActiveView.Id );
          IEnumerable<FamilyInstance> famInstances = collectorFamInstances.WherePasses( filterFamilyInst ).OfType<FamilyInstance>();
          int nInstanceCount = famInstances.Count<FamilyInstance>();
          temp.Add( new XElement( 
            "SymbolName", famSymbol.Name,
            from fi in famInstances
            select new XElement( 
              "Instance", 
              fi.Id.ToString(),
              new XElement( "Type", fi.GetType().ToString() ),
              new XElement( "Position", Position( fi ) ) ) ) );
        }
        xmlFamilyInstance.Add( temp );
      }

      // Create the document
      XDocument xmldoc =
        new XDocument(
            new XDeclaration( "1.0", "utf-8", "yes" ),
            new XComment( "Current Family Inventory of revit project: " + activeDoc.PathName ),
            xmlFamilyInstance
            );

      string fileName = "C:/FamilyInventory.xml";
      xmldoc.Save( fileName );

      Util.ShowElapsedTime( sw, string.Empty );
      
      // We can use Internet Explorer or whatever your favorite XML viewer is...
      Process.Start("C:/Program Files/Internet Explorer/iexplore.exe", fileName);
      
      // Here is one that is free and is a little more robust than Internet Explorer.
      // If interested, download from here: http://download.cnet.com/XML-Marker/3000-7241_4-10202365.html
      //Process.Start( @"C:/Program Files (x86)/XML Marker/xmlmarker.exe", fileName );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Parameter Filter Example 1
  /// <summary>
  /// ElementParameterFilter Filter example 1.
  /// This example shows different ways of using the 
  /// parameter filters with different data types and rules.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the AecDevCamp2010SampleCode_project.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class FilterParameterEx1 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      // Use numeric evaluator and integer rule to test ElementId parameter
      // Filter analysis display style whose id is matches the one we want.
      ElementId idAds = Util.FindAnalysisDisplayStyleId( activeDoc, "fred" );
      BuiltInParameter testParam = BuiltInParameter.VIEW_ANALYSIS_DISPLAY_STYLE;
      ParameterValueProvider provider = new ParameterValueProvider( new ElementId( testParam ) );
      FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();
      FilterRule rule = new FilterElementIdRule( provider, evaluator, idAds );
      ElementParameterFilter filter = new ElementParameterFilter( rule );
      FilteredElementCollector collector = new FilteredElementCollector( activeDoc );
      collector.OfClass( typeof( ViewPlan ) ).WherePasses( filter ); // only deal with ViewPlan

      ICollection<Element> set1 = collector.ToElements();

      // Use numeric evaluator and integer rule to test bool parameter
      // Filter levels whose crop view is false. Parameter is stored as 0 or 1.
      int ruleValInt = 0;
      testParam = BuiltInParameter.VIEWER_CROP_REGION;
      provider = new ParameterValueProvider( new ElementId( testParam ) );
      evaluator = new FilterNumericEquals();
      rule = new FilterIntegerRule( provider, evaluator, ruleValInt );
      filter = new ElementParameterFilter( rule );
      collector = new FilteredElementCollector( activeDoc );
      collector.OfClass( typeof( ViewPlan ) ).WherePasses( filter ); // only deal with ViewPlan

      ICollection<Element> set2 = collector.ToElements();

      // Use numeric evaluator and double rule to test double parameter
      // Filter levels whose top offset is greater than specified value
      double ruleValDb = 10.0;
      testParam = BuiltInParameter.VIEWER_BOUND_OFFSET_TOP;
      provider = new ParameterValueProvider( new ElementId( testParam ) );
      evaluator = new FilterNumericGreater();
      rule = new FilterDoubleRule( provider, evaluator, ruleValDb, double.Epsilon );
      filter = new ElementParameterFilter( rule );
      collector = new FilteredElementCollector( activeDoc );
      collector.OfClass( typeof( ViewPlan ) ).WherePasses( filter ); // only deal with ViewPlan

      ICollection<Element> set3 = collector.ToElements();

      // use string evaluator and string rule to test string parameter
      // Filter all elements whose view name contains "Level"
      string ruleValStr = "Level";
      testParam = BuiltInParameter.VIEW_NAME;
      provider = new ParameterValueProvider( new ElementId( testParam ) );
      FilterStringRuleEvaluator evaluatorStr = new FilterStringContains();
      rule = new FilterStringRule( provider, evaluatorStr, ruleValStr, false );
      filter = new ElementParameterFilter( rule );
      collector = new FilteredElementCollector( activeDoc );
      collector.OfClass( typeof( ViewPlan ) ).WherePasses( filter ); // only deal with ViewPlan

      ICollection<Element> set4 = collector.ToElements();

      return Result.Succeeded;
    }
  }
  #endregion

  #region Parameter Filter Example 2
  /// <summary>
  /// This example shows how the parameter filter 
  /// can be faster than other techniques at finding certain criteria,
  /// in this example all elements on a specific level. 
  /// We compare the ElementLevelFilter versus a parameter filter
  /// versus a LINQ query. In all the tests attempted, the parameter 
  /// filter was the fastest, and the LINQ query the slowest.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the ArchSample.rvt model file.
  /// </remarks>
  [Transaction( TransactionMode.Manual)]
  [Regeneration( RegenerationOption.Manual )]
  public class FilterParameterEx2 : IExternalCommand
  {
    static string _level_name = "Level 2"; // "02 - Floor"

    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      // Level 2 example criteria
      ElementId levelId = Util.FindLevelId( activeDoc, _level_name );
      Stopwatch sw = new Stopwatch();
      string names = string.Empty;

      // Use the Level filter to find all FamilyInstances with desired Level.
      // Note this is a slow filter... notice it is applied after the class (quick) filter
      sw.Reset();
      sw.Start();

      ElementLevelFilter filterElementsOnLevel = new ElementLevelFilter( levelId );
      FilteredElementCollector collector1 = new FilteredElementCollector( activeDoc );
      collector1.OfClass( typeof( FamilyInstance ) ).WherePasses( filterElementsOnLevel );
      ICollection<Element> listLevel = collector1.ToElements();

      // Now that we have only what we want, foreach process them into a report.
      names = string.Empty;
      foreach( FamilyInstance instance in listLevel )
      {
        names += "\nLevel Name = " + instance.Level.Name 
          + "   Instance name = " + instance.Name 
          + "   id: " + instance.Id.ToString();
      }

      sw.Stop();
      
      Util.ShowElapsedTime( sw, 
        "Using ElementLevelFilter to find "
        + listLevel.Count().ToString() 
        + " family instances on a given level: " 
        + names );

      // Use ElementParameterFilter to find all FamilyInstances 
      // with desired Level by finding the data in the FAMILY_LEVEL_PARAM parameter.

      sw.Reset();
      sw.Start();

      BuiltInParameter bip = BuiltInParameter.FAMILY_LEVEL_PARAM;
      ParameterValueProvider provider = new ParameterValueProvider( new ElementId( bip ) );
      FilterNumericRuleEvaluator evaluator = new FilterNumericEquals();
      FilterRule rule = new FilterElementIdRule( provider, evaluator, levelId );
      ElementParameterFilter filter = new ElementParameterFilter( rule );
      FilteredElementCollector collector2 = new FilteredElementCollector( activeDoc );
      collector2.OfClass( typeof( FamilyInstance ) ).WherePasses( filter );
      ICollection<Element> listLevelParam = collector2.ToElements();

      // Now that we have only what we want, foreach process them into a report.
      names = string.Empty;
      foreach( FamilyInstance instance in listLevelParam )
      {
        names += "\nLevel Name = " + instance.Level.Name
          + "   Instance name = " + instance.Name
          + "   id: " + instance.Id.ToString();
      }

      sw.Stop();
      
      Util.ShowElapsedTime( sw, 
        "Using Element Parameter Filter: " 
        + listLevelParam.Count().ToString() + names );

      // Use LINQ to find all FamilyInstances with desired Level.
      sw.Reset();
      sw.Start();
      FilteredElementCollector collector3 = new FilteredElementCollector( activeDoc );
      collector3.OfClass( typeof( FamilyInstance ) );

      IEnumerable<FamilyInstance> listLevelLINQ = collector3.OfType<FamilyInstance>();

      // Use LINQ to filter down those that match the desired level name.
      Level level = null;
      IEnumerable<FamilyInstance> listFiOnLevelLINQ 
        = from fi in listLevelLINQ
          where ( ( level = fi.Level ) != null ) 
            && ( level.Id.Equals( levelId ) )
          select fi;

      // Now that we have only what we want, foreach process them into a report.
      names = string.Empty;
      foreach( FamilyInstance instance in listFiOnLevelLINQ )
      {
        names += "\nLevel Name = " + instance.Level.Name
          + "   Instance name = " + instance.Name
          + "   id: " + instance.Id.ToString();
      }

      sw.Stop();
      
      Util.ShowElapsedTime( sw, 
        "Using LINQ to find " 
        + listFiOnLevelLINQ.Count<FamilyInstance>().ToString() 
        + " family instances on a specific level: " 
        + names );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Regeneration Example 1
  /// <summary>
  /// Regeneration example 01.
  /// This sample illustrates a defect in 2011 RTM. It will be fixed for the update user release.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the EmptyProject.rvt model file. 
  /// Mainly this empty model has an extra Level (3) as base data 
  /// over the standard template.
  /// </remarks>
  [Transaction( TransactionMode.Manual)]
  [Regeneration( RegenerationOption.Manual )]
  public class RegenerateEx1 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      // Find some level information
      Level level1 = Util.FindLevel( activeDoc, "Level 1" );
      ElementId level2Id = Util.FindLevelId( activeDoc, "Level 2" );

      if( ( level1 == null ) || ( level2Id == ElementId.InvalidElementId ) )
        return Result.Failed;

      double width = 50;
      double depth = 40;

      double dx = width / 2.0;
      double dy = depth / 2.0;

      IList<XYZ> listPoints = new List<XYZ>();
      listPoints.Add( new XYZ( -dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, dy, 0.0 ) );
      listPoints.Add( new XYZ( -dx, dy, 0.0 ) );
      listPoints.Add( listPoints[0] );

      Parameter heightParam;

      // Create the walls and set the WALL_HEIGHT_TYPE parameter
      for( int i = 0; i < 4; i++ )
      {
        Line baseCurve = app.Create.NewLineBound( listPoints[i], listPoints[i + 1] );
        Wall wall = activeDoc.Create.NewWall( baseCurve, level1, true );
        heightParam = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE );
        heightParam.Set( level2Id );
      }
      // Defect in Revit 2011 RTM Automatic where auto-join fails.
      return Result.Succeeded;
    }
  }
  #endregion

  #region Regeneration Example 2
  /// <summary>
  /// Regeneration example 01.
  /// This sample illustrates using automatic transaction, 
  /// and regenerate/auto-join after all elements are created.
  /// /// </summary>
  /// <remarks>
  /// This is meant to run with the EmptyProject.rvt model file. 
  /// Mainly this empty model has an extra Level (3) as base data 
  /// over the standard template.
  /// </remarks>
  [Transaction( TransactionMode.Manual)]
  [Regeneration( RegenerationOption.Manual )]
  public class RegenerateEx2 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Level level1 = Util.FindLevel( activeDoc, "Level 1" );
      ElementId level2Id = Util.FindLevelId( activeDoc, "Level 2" );

      if( ( level1 == null ) || ( level2Id == ElementId.InvalidElementId ) )
        return Result.Failed;

      double width = 50;
      double depth = 40;

      double dx = width / 2.0;
      double dy = depth / 2.0;

      IList<XYZ> listPoints = new List<XYZ>();
      listPoints.Add( new XYZ( -dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, dy, 0.0 ) );
      listPoints.Add( new XYZ( -dx, dy, 0.0 ) );
      listPoints.Add( listPoints[0] );

      Parameter heightParam;

      for( int i = 0; i < 4; i++ )
      {
        Line baseCurve = app.Create.NewLineBound( listPoints[i], listPoints[i + 1] );
        Wall wall = activeDoc.Create.NewWall( baseCurve, level1, true );
        heightParam = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE );
        heightParam.Set( level2Id );
      }
      activeDoc.Regenerate();
      activeDoc.AutoJoinElements();

      return Result.Succeeded;
    }
  }
  #endregion

  #region Regeneration Example 3
  /// <summary>
  /// Regeneration example 03.
  /// This sample compares regenerate and auto-join performance 
  /// for each element creation vs. after all creations at once.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the EmptyProject.rvt model file. 
  /// Mainly this empty model has an extra Level (3) as base data 
  /// over the standard template.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class RegenerateEx3 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Level level1 = Util.FindLevel( activeDoc, "Level 1" );
      ElementId level2Id = Util.FindLevelId( activeDoc, "Level 2" );

      if( ( level1 == null ) || ( level2Id == ElementId.InvalidElementId ) )
        return Result.Failed;

      double width = 50;
      double depth = 40;

      double dx = width / 2.0;
      double dy = depth / 2.0;

      IList<XYZ> listPoints = new List<XYZ>();
      listPoints.Add( new XYZ( -dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, dy, 0.0 ) );
      listPoints.Add( new XYZ( -dx, dy, 0.0 ) );
      listPoints.Add( listPoints[0] );

      Parameter heightParam;
      Stopwatch sw = new Stopwatch();

      // Demonstrate performance when regenerating and auto-joining after each
      // parameter change.
      sw.Start();
      Transaction trans = new Transaction( activeDoc );
      trans.Start( "Regen After Every Add" );
      for( int i = 0; i < 4; i++ )
      {
        Line baseCurve = app.Create.NewLineBound( listPoints[i], listPoints[i + 1] );
        Wall wall = activeDoc.Create.NewWall( baseCurve, level1, true );
        heightParam = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE );
        heightParam.Set( level2Id );

        activeDoc.Regenerate();
        activeDoc.AutoJoinElements();
      }
      trans.Commit();
      sw.Stop();
      Util.ShowElapsedTime( sw, "Regen after every add" );

      dx *= 1.5;
      dy *= 1.5;
      listPoints.Clear();
      listPoints.Add( new XYZ( -dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, -dy, 0.0 ) );
      listPoints.Add( new XYZ( dx, dy, 0.0 ) );
      listPoints.Add( new XYZ( -dx, dy, 0.0 ) );
      listPoints.Add( listPoints[0] );

      // Demonstrate performance when regenerating and auto-joining after all
      // parameter changes are finished.
      sw.Reset();
      sw.Start();
      trans.Start( "Regen after add finished" );
      for( int i = 0; i < 4; i++ )
      {
        Line baseCurve = app.Create.NewLineBound( listPoints[i], listPoints[i + 1] );
        Wall wall = activeDoc.Create.NewWall( baseCurve, level1, true );
        heightParam = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE );
        heightParam.Set( level2Id );

      }
      activeDoc.Regenerate();
      activeDoc.AutoJoinElements();
      trans.Commit();
      sw.Stop();
      Util.ShowElapsedTime( sw, "Regen after add finished" );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Regeneration Example 4
  /// <summary>
  /// Regeneration example 04.
  /// This samples illustrates that when trying to get data that may have changed before 
  /// regeneration occurs will result in stale data. It also compares regenerate performance 
  /// for each parameter change vs. regenerate once after all parameter changes and 
  /// then iterate again to effectively get the updated data.
  /// </summary>
  /// <remarks>
  /// This is meant to run with the EmptyProject.rvt model file
  /// after running Regeneration Example 3.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class RegenerateEx4 : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      // Get the application and document from external command data.
      Application app = commandData.Application.Application;
      Document activeDoc = commandData.Application.ActiveUIDocument.Document;

      Level level1 = Util.FindLevel( activeDoc, "Level 1" );
      ElementId level2Id = Util.FindLevelId( activeDoc, "Level 2" );
      ElementId level3Id = Util.FindLevelId( activeDoc, "Level 3" );

      if( ( level1 == null )
        || ( level2Id == ElementId.InvalidElementId )
        || ( level3Id == ElementId.InvalidElementId ) )
      {
        return Result.Failed;
      }

      FilteredElementCollector collector = new FilteredElementCollector( activeDoc );
      collector.OfClass( typeof( Wall ) );

      FilteredElementIterator iter = collector.GetElementIterator();

      string report = string.Empty;
      Wall wall;
      Parameter heightParam;
      GeometryElement geom;
      GeometryObjectArray geomObjs;
      Solid wallSolid;

      Autodesk.Revit.DB.Options opts = app.Create.NewGeometryOptions();
      opts.DetailLevel = Autodesk.Revit.DB.DetailLevels.Fine;

      Stopwatch sw = Stopwatch.StartNew();
      report = "Update Parameter and Regen at each change";
      Transaction trans = new Transaction( activeDoc );
      trans.Start( report );

      while( !iter.IsDone() )
      {
        wall = iter.GetCurrent() as Wall;
        geom = wall.get_Geometry( opts );
        geomObjs = geom.Objects;
        wallSolid = Util.FindSolid( geomObjs );
        if( wallSolid != null )
          report += "\nOriginal Wall Volume = " + wallSolid.Volume.ToString() + "  id: " + wall.Id.ToString();

        heightParam = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE );
        heightParam.Set( level3Id );

        geom = wall.get_Geometry( opts );
        geomObjs = geom.Objects;
        wallSolid = Util.FindSolid( geomObjs );
        if( wallSolid != null )
          report += "\nAfter Param change, Wall Volume = " + wallSolid.Volume.ToString() + "  id: " + wall.Id.ToString();

        activeDoc.Regenerate();

        geom = wall.get_Geometry( opts );
        geomObjs = geom.Objects;
        wallSolid = Util.FindSolid( geomObjs );
        if( wallSolid != null )
          report += "\nAfter regenerate with each Param change, Wall Volume = " + wallSolid.Volume.ToString() + "  id: " + wall.Id.ToString() + "\n";

        iter.MoveNext();
      }
      trans.Commit();
      sw.Stop();
      Util.ShowElapsedTime( sw, report );

      iter.Reset();
      sw.Reset();

      sw.Start();
      report = "Update Parameter and regen once.";
      trans = new Transaction( activeDoc );
      trans.Start( report );

      while( !iter.IsDone() )
      {
        wall = iter.GetCurrent() as Wall;
        geom = wall.get_Geometry( opts );
        geomObjs = geom.Objects;
        wallSolid = Util.FindSolid( geomObjs );
        if( wallSolid != null )
          report += "\nOriginal Wall Volume = " + wallSolid.Volume.ToString() + "  id: " + wall.Id.ToString();

        heightParam = wall.get_Parameter( BuiltInParameter.WALL_HEIGHT_TYPE );
        heightParam.Set( level2Id );

        geom = wall.get_Geometry( opts );
        geomObjs = geom.Objects;
        wallSolid = Util.FindSolid( geomObjs );
        if( wallSolid != null )
          report += "\nAfter Param change, Wall Volume = " + wallSolid.Volume.ToString() + "  id: " + wall.Id.ToString() + "\n";

        iter.MoveNext();
      }

      activeDoc.Regenerate();
      iter.Reset();

      while( !iter.IsDone() )
      {
        wall = iter.GetCurrent() as Wall;
        geom = wall.get_Geometry( opts );
        geomObjs = geom.Objects;
        wallSolid = Util.FindSolid( geomObjs );
        if( wallSolid != null )
          report += "\nAfter regenerate with ALL Param changes, Wall Volume = " + wallSolid.Volume.ToString() + "  id: " + wall.Id.ToString();

        iter.MoveNext();
      }
      trans.Commit();

      sw.Stop();
      Util.ShowElapsedTime( sw, report );

      return Result.Succeeded;
    }
  }
  #endregion

  #region Run all commands
  /// <summary>
  /// Test run all the commands in non-interactive mode.
  /// </summary>
  /// <remarks>
  /// This command runs all the other commands without displaying the 
  /// task dialogues so they can be roughly tested without user interaction.
  /// </remarks>
  [Transaction( TransactionMode.Manual )]
  [Regeneration( RegenerationOption.Manual )]
  public class RunAllCommands : IExternalCommand
  {
    bool IsOk( Result r )
    {
      return Result.Succeeded == r;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      return IsOk( new PerformanceEx1_old().Execute( commandData, ref message, elements ) )
        && IsOk( new PerformanceEx2_new().Execute( commandData, ref message, elements ) )
        && IsOk( new FilterEx1().Execute( commandData, ref message, elements ) )
        && IsOk( new FilterEx2().Execute( commandData, ref message, elements ) )
        && IsOk( new LinqEx1().Execute( commandData, ref message, elements ) )
        && IsOk( new LinqEx2().Execute( commandData, ref message, elements ) )
        && IsOk( new LinqEx3().Execute( commandData, ref message, elements ) )
        && IsOk( new FilterParameterEx1().Execute( commandData, ref message, elements ) )
        && IsOk( new FilterParameterEx2().Execute( commandData, ref message, elements ) )
        //&& IsOk( new RegenerateEx1().Execute( commandData, ref message, elements ) )
        //&& IsOk( new RegenerateEx2().Execute( commandData, ref message, elements ) )
        && IsOk( new RegenerateEx3().Execute( commandData, ref message, elements ) )
        && IsOk( new RegenerateEx4().Execute( commandData, ref message, elements ) )
        ? Result.Succeeded
        : Result.Failed;
    }
  }
  #endregion
}
