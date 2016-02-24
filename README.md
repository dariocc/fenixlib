![FenixLib](http://dacucar.com/fenixlib/fenixlib.png "FenixLib Logo")

FenixLib brings to you.NET support for opening, creating and operating with 
[PixTudio](https://pixtudio.org), [BennuGD](https://bennugd.org) and 
[DIV](http://div-arena.co.uk/) native graphic, graphic collections, 
fonts and palette formats.

The following example snippet shows how easy is to manipulate Fpg files:
```csharp
using FenixLib.Core;
using FenixLib.IO;

// Load a Fpg file
SpriteAsset asset = File.LoadFpg ( "myfpg.fpg" );

// Print out the code and description of every 
// sprite in the Fpg
foreach ( Sprite sprite in asset )
{
	System.Console.WriteLine ( sprite.Id + " - " + sprite.Description );
}

// Change the description of Sprite with code 10
asset[10].Description = "My graphic";

File.SaveFpg ( "modified.fpg",  );
```
Not harder than it is to create new fonts:
```csharp
using FenixLib.Core;
using FenixLib.IO;

BitmapFont font = BitmapFont(DepthMode.Argb32, FontCodePage.ISO85591);
font['å'] = Glyph.Create( DepthMode.ArgbInt32, 10, 10, new byte[10 * 10 * 8] );
File.SaveFnt ( 'myfont.fnt' );
```

## Using the library
Wip
## Compiling
Wip

Detailed instructions can be found on [Building FenixLib sources] section in the Wiki.
FenixLib shall run in any platform with support for .NET 4.5 and C#. This includes
Windows, Linux and MacOS.

You can use Visual Studio.NET or Mono Develop.
## Contributing
Wip
## License
Copyright 2016 Darío Cutillas Carrillo

FenixLib is distributed under the very permisive 
[Apache License, Version 2.0] (http://www.apache.org/licenses/LICENSE-2.0)

You are free to use FenixLib in both commercial and non commercial and 
open or close source applications as long as you follow the terms of the 
License.
