![FenixLib](https://github.com/dariocc/fenixlib/blob/development/fennixlib/logo.png "FenixLib Logo")

FenixLib brings .NET support for opening, creating and operating with
native graphic, graphic collections, bitmap fonts and palette formats of
[PixTudio](https://pixtudio.org), [BennuGD](https://bennugd.org), 
[Div GO](http://www.amakasoft.com/herramientas/div-go.html) and 
[DIV](http://div-arena.co.uk/) game programming languages and frameworks.

The following snippet shows how easy is to open a Fpg file, print the codes
and description of every map, change the description of map with code 10 and
save the changes in a new file.

```csharp
using FenixLib.Core;
using FenixLib.IO;

// Load a Fpg file
var spriteAssortment = NativeFile.LoadFpg ( "foo.fpg" );

// Print out the code and description of every sprite in the Fpg
foreach ( var sprite in spriteAssortment )
{
  System.Console.WriteLine ( sprite.Id.ToString() + " - " + sprite.Description );
}

// Change the description of Sprite with code 10
spriteAssortment[10].Description = "Bar";

spriteAssortment.SaveToFpg ( "foo-changed.fpg" );
```

Another example, see how easy you can create a Fnt font file from scratch:

```csharp
using FenixLib.Core;
using FenixLib.IO;

var font = new BitmapFont ( GraphicFormat.Format32bppArgb, FontEncoding.ISO85591 );

// Create a 10x10 transparent graphic
var glyphGraphic = new Graphic( GraphicFormat.Format32bppArgb, 10, 10, new byte[10 * 10 * 4] );

// Create the glyph and assign it to the letter 'å'
// å is a character that exists in the ISO8559-1 code page.
font['å'] = new Glyph ( glyphGraphic );

// Save the font to a Fnt file (only 'å' will contain a bitmap)
font.SaveToFnt ( "foo.fnt" );
```

For additional examples and documentation visit http://github.com/dacucar/fenixlib/wiki.
