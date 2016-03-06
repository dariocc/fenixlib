using System.Collections.Generic;

namespace FenixLib.Core
{
    public interface IBitmapFont : IEnumerable<FontGlyph>
    {
        IGlyph this[int index] { get; set;  }
        IGlyph this[char character] { get; set;  }

        FontEncoding CodePage { get; }
        IEnumerable<FontGlyph> Glyphs { get; }
        GraphicFormat GraphicFormat { get; }
        Palette Palette { get; }
    }
}