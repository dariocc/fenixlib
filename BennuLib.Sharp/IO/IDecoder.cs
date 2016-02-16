using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.IO;
namespace BennuLib
{


	public interface IDecoder<T>
	{
		IEnumerable<string> SupportedExtensions { get; }
		bool TryDecode(Stream input, ref T decoded);
		T Decode(Stream input);
	}
}

