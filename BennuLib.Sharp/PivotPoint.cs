using System;

namespace BennuLib
{
	[Serializable()]
	public struct PivotPoint
	{
		public readonly int Id;
		public readonly int X;

		public readonly int Y;
		public PivotPoint(int id, int x, int y)
		{
			this.Id = id;
			this.X = x;
			this.Y = y;
		}
	}
}
