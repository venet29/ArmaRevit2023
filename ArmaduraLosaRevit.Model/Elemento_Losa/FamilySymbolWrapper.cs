using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace ArmaduraLosaRevit.Model.Elemento_Losa
{
    /// <summary>
    /// A wrapper of family symbol
    /// </summary>
    public class FamilySymbolWrapper
    {
        FamilySymbol m_familySymbol;

        /// <summary>
        /// Family symbol
        /// </summary>
        public FamilySymbol FamilySymbol
        {
            get { return m_familySymbol; }
        }

        /// <summary>
        /// Display name
        /// </summary>
        public string Name
        {
            get { return m_familySymbol.Family.Name + " : " + m_familySymbol.Name; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tagSymbol"></param>
        public FamilySymbolWrapper(FamilySymbol familySymbol)
        {
            m_familySymbol = familySymbol;
        }
    };
}
