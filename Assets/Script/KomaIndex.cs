using System;
using UnityEngine;

namespace Koma
{
    /// <summary>
    /// ストーン配列に対応するインデックス
    /// </summary>
    public class KomaIndex : IEquatable<KomaIndex>
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public KomaIndex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public float GetLength()
        {
            return Mathf.Sqrt(X * X + Y * Y);
        }

        public static KomaIndex operator +(KomaIndex a, KomaIndex b)
        {
            return new KomaIndex(a.X + b.X, a.Y + b.Y);
        }

        public static KomaIndex operator -(KomaIndex a, KomaIndex b)
        {
            return new KomaIndex(a.X - b.X, a.Y - b.Y);
        }

        public bool Equals(KomaIndex other)
        {
            if (other == null)
            {
                return false;
            }
            return X == other.X && Y == other.Y;
        }
    }
}
