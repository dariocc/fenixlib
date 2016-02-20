namespace Bennu.IO
{
	public class PalPaletteEncoder : NativeEncoder<Palette>
	{

		protected override byte Version { get; }

		protected override void WriteNativeFormat(Palette palette, NativeFormatWriter writer)
		{
			writer.Write(palette);
			writer.WriteReservedPaletteGammaSection();
		}

		protected override string GetFileId(Palette obj)
		{
			return "pal";
		}
	}
}
