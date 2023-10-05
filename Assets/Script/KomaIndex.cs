using System;
using UnityEditor;
using UnityEngine;

namespace Koma
{
  
    public class KomaIndex 
    {
        public int X { get; private set; }
        public int Y { get; private set; }

       public bool Build{ get; private set; }
        public KomaIndex(int x, int y , bool Builds)
        {
            X = x;
            Y = y;
           Build = Builds;
           
        }

      
    }
}
