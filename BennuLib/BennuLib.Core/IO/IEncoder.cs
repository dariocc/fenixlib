using System.IO;

namespace Bennu
{

	public interface IEncoder<T>
	{
		void Encode(T obj, Stream output);
	}
}
