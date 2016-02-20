namespace Bennu
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
