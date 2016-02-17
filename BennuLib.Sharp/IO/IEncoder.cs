using System.IO;

namespace BennuLib
{

	public interface IEncoder<T>
	{
		void Encode(T obj, Stream output);
	}
}
