namespace Bennu.IO
{
	public class PalPaletteEncoder : NativeEncoder<Palette>
	{
        private const int version = 0x00;

		protected override byte GetLastHeaderByte(Palette palette) => version;

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
