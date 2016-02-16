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
	public interface IPixel
	{
		int Argb { get; }
		int Red { get; }
		int Green { get; }
		int Blue { get; }
		int Alpha { get; }
		int Value { get; }
		IPixel GetTransparentCopy();
		IPixel GetOpaqueCopy();
		bool IsTransparent { get; }
	}
}
