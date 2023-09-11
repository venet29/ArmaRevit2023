#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections;
#endregion // Namespaces



namespace ArmaduraLosaRevit.Model
{
    /// <summary>
    /// Vector4 class use to store vector
    /// and contain method to handle the vector
    /// </summary>
    public class Vector4
    {
        #region Class member variables and properties
        private float m_x;
        private float m_y;
        private float m_z;
        private float m_w = 1.0f;

        /// <summary>
        /// X property to get/set x value of Vector4
        /// </summary>
        public float X
        {
            get
            {
                return m_x;
            }
            set
            {
                m_x = value;
            }
        }

        /// <summary>
        /// Y property to get/set y value of Vector4
        /// </summary>
        public float Y
        {
            get
            {
                return m_y;
            }
            set
            {
                m_y = value;
            }
        }

        /// <summary>
        /// Z property to get/set z value of Vector4
        /// </summary>
        public float Z
        {
            get
            {
                return m_z;
            }
            set
            {
                m_z = value;
            }
        }

        /// <summary>
        /// W property to get/set fourth value of Vector4
        /// </summary>
        public float W
        {
            get
            {
                return m_w;
            }
            set
            {
                m_w = value;
            }
        }
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public Vector4(float x, float y, float z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }

        /// <summary>
        /// constructor, transfer Autodesk.Revit.DB.XYZ to vector
        /// </summary>
        /// <param name="v">Autodesk.Revit.DB.XYZ structure which need to be transferred</param>
        public Vector4(Autodesk.Revit.DB.XYZ v)
        {
            this.X = (float)v.X; this.Y = (float)v.Y; this.Z = (float)v.Z;
        }

        /// <summary>
        /// adds two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        public static Vector4 operator +(Vector4 va, Vector4 vb)
        {
            return new Vector4(va.X + vb.X, va.Y + vb.Y, va.Z + vb.Z);
        }

        /// <summary>
        /// subtracts two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        /// <returns>subtraction of two vector</returns>
        public static Vector4 operator -(Vector4 va, Vector4 vb)
        {
            return new Vector4(va.X - vb.X, va.Y - vb.Y, va.Z - vb.Z);
        }

        /// <summary>
        /// multiplies a vector by a floating type value
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="factor">multiplier of floating type</param>
        /// <returns> the result vector </returns>
        public static Vector4 operator *(Vector4 v, float factor)
        {
            return new Vector4(v.X * factor, v.Y * factor, v.Z * factor);
        }

        /// <summary>
        /// divides vector by an floating type value
        /// </summary>
        /// <param name="v">vector</param>
        /// <param name="factor">floating type value</param>
        /// <returns> vector divided by a floating type value </returns>
        public static Vector4 operator /(Vector4 v, float factor)
        {
            return new Vector4(v.X / factor, v.Y / factor, v.Z / factor);
        }

        /// <summary>
        /// dot multiply vector
        /// </summary>
        /// <param name="v"> the result vector </param>
        public float DotProduct(Vector4 v)
        {
            return (this.X * v.X + this.Y * v.Y + this.Z * v.Z);
        }

        /// <summary>
        /// get normal vector of two vectors
        /// </summary>
        /// <param name="v">second vector</param>
        /// <returns> normal vector of two vectors</returns>
        public Vector4 CrossProduct(Vector4 v)
        {
            return new Vector4(this.Y * v.Z - this.Z * v.Y, this.Z * v.X
                - this.X * v.Z, this.X * v.Y - this.Y * v.X);
        }

        /// <summary>
        /// dot multiply two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        public static float DotProduct(Vector4 va, Vector4 vb)
        {
            return (va.X * vb.X + va.Y * vb.Y + va.Z * vb.Z);
        }

        /// <summary>
        /// get normal vector of two vectors
        /// </summary>
        /// <param name="va">first vector</param>
        /// <param name="vb">second vector</param>
        /// <returns> normal vector of two vectors </returns>
        public static Vector4 CrossProduct(Vector4 va, Vector4 vb)
        {
            return new Vector4(va.Y * vb.Z - va.Z * vb.Y, va.Z * vb.X
                - va.X * vb.Z, va.X * vb.Y - va.Y * vb.X);
        }

        /// <summary>
        /// get unit vector
        /// </summary>
        public void Normalize()
        {
            float length = Length();
            if (length == 0)
            {
                length = 1;
            }
            this.X /= length;
            this.Y /= length;
            this.Z /= length;
        }

        /// <summary>
        /// calculate the length of vector
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }
    };

}
