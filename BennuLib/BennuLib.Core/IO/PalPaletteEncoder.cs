namespace Bennu.IO
{
	public class PalPaletteEncoder : NativeEncoder<Palette>
	{

		protected override byte LastHeaderByte { get; }

		protected override void WriteNativeFormatBody(Palette palette, NativeFormatWriter writer)
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
