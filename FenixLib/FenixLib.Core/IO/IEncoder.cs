using System.IO;

namespace FenixLib
{

	public interface IEncoder<T>
	{
		void Encode(T obj, Stream output);
	}
}
