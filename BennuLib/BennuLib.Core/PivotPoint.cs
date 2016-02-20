using System;

namespace BennuLib
{
	[Serializable()]
	public struct PivotPoint
	{
		public readonly int Id;
		public readonly int X;
		public readonly int Y;

		internal PivotPoint(int id, int x, int y)
		{
			Id = id;
			X = x;
			Y = y;
		}

        public bool Equals(PivotPoint point)
        {
            if (ReferenceEquals(point, null))
            {
                return false;
            }

            return point.X == X && point.Y == Y && point.Id == Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if ( !(obj is PivotPoint) )
            {
                return false;
            }

            return Equals((PivotPoint) obj);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Id.GetHashCode();
        }

        public static bool operator == (PivotPoint pointA, PivotPoint pointB)
        {
            return pointA.Equals(pointB);
        }

        public static bool operator != (PivotPoint pointA, PivotPoint pointB)
        {
            return !pointA.Equals(pointB);
        }
    }
}
