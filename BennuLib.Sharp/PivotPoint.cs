using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
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
