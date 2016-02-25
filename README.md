![FenixLib](http://dacucar.com/fenixlib/fenixlib.png "FenixLib Logo")

FenixLib brings to you.NET support for opening, creating and operating with 
[PixTudio](https://pixtudio.org), [BennuGD](https://bennugd.org) and 
[DIV](http://div-arena.co.uk/) native graphic, graphic collections, 
fonts and palette formats.

The following snippet shows how easy is to open a Fpg file, print the codes
and description of every map, change the description of map with code 10 and
save the changes in a new file.
```csharp
using FenixLib.Core;
using FenixLib.IO;

// Load a Fpg file
SpriteAsset asset = File.LoadFpg ( "myfpg.fpg" );

// Print out the code and description of every 
// sprite in the Fpg
foreach ( Sprite sprite in asset )
{
	System.Console.WriteLine ( sprite.Id.ToString() + " - " + sprite.Description );
}

// Change the description of Sprite with code 10
asset[10].Description = "My graphic";

File.SaveFpg ( "modified.fpg",  );
```

Another cool example, see how easy it is to create a font file:
```csharp
using FenixLib.Core;
using FenixLib.IO;

BitmapFont font = BitmapFont ( DepthMode.Argb32, FontCodePage.ISO85591 );
// A dummy glyph
font['å'] = Glyph.Create ( DepthMode.ArgbInt32, 10, 10, new byte[10 * 10 * 4] );
File.SaveFnt ( 'myfont.fnt' );
```

## Using the library
If you know what [Fpg](https://github.com/dacucar/fenixlib/wiki/FpgFormat),  [Map](https://github.com/dacucar/fenixlib/wiki/MapFormat), [Pal](https://github.com/dacucar/fenixlib/wiki/PalFormat) and [Fnt](https://github.com/dacucar/fenixlib/wiki/FntFormat) files, working with FenixLib should be quite strightforward, just make sure to go through the information in the following topics. 

* [Core Types](https://github.com/dacucar/fenixlib/wiki/CoreTypes)
* [IO Api](https://github.com/dacucar/fenixlib/wiki/IOApi)
* [Image Api](https://github.com/dacucar/fenixlib/wiki/ImageApi)
* [Image Api Backends](https://github.com/dacucar/fenixlib/wiki/ImageBackends)

If you don't, just have a look to the [native formats introduction](https://github.com/dacucar/fenixlib/wiki/NativeFormats) and then you will be ready to the topics above.

## Compiling
FenixLib core assembly runs in any platform with support for .NET 4.5 and C# as it has no other dependencies. I alternate development in [VisualStudio.NET](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx) and
[MonoDevelop / Xamarin](http://www.monodevelop.com/) and I have succesfully built it from Windows, Linux and OSX.

Go ahead and make a clone of this repository:

    git clone https://github.com/dacucar/fenixlib.git
    
Then open the ```FenixLib.Sln``` within the ```./fenixlib/FenixLib``` folder with either Visual Studio, MonoDevelop or Xamarin and you are ready to go... FenixLib builds as any other class library. Note that as long as you have a C# compiler for the .NET 4.5 platform you do not need any IDE, but I guess that if you know that you already know what to do... 

The [Image Api Backends](https://github.com/dacucar/fenixlib/wiki/ImageBackEnds) and auxiliary assemblies might however be platform dependent. This is done on purpose, to take the most of each system. For example, the classes exposed by the FenixLib.Wpf are dependent on the [Wpf](https://msdn.microsoft.com/en-us/library/ms754130.aspx) assemblies.

For more detailed instructions refer to the [Compiling FenixLib](https://github.com/dacucar/fenixlib/wiki/Compiling) section of the Wiki.

## Contributing
There basically three ways in which you can contribute:
  1. Reporting issues or feature requests. 
  2. Joining the development.
  3. Donating.
  4. Letting me know how useful is this to you.

Well, that was 4 actually!

### Reporting issues or feature requests
Use the [issues](https://github.com/dacucar/fenixlib/issues) section. Any feature request shall also be written there.

### Joining the development
If you feel to contribute with actual code, you can. Make sure to read the [FenixLib Architecture](https://github.com/dacucar/fenixlib/wiki/Architecture) notes and the [Coding Conventions](https://github.com/dacucar/fenixlib/wiki/CodingConventions). Then ju

### Donating
If FenixLib is useful to you and you want so, you are welcomed to fund this project. For the price of a cup of coffee at your local cafeteria, I will be more than glad to mantain and extend this project.

<div>
<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="encrypted" value="-----BEGIN PKCS7-----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-----END PKCS7-----
">
<input type="image" src="https://www.paypalobjects.com/en_GB/i/btn/btn_donate_SM.gif" border="0" name="submit" alt="PayPal – The safer, easier way to pay online.">
<img alt="" border="0" src="https://www.paypalobjects.com/es_ES/i/scr/pixel.gif" width="1" height="1">
</form>
</div>

### Letting me know how useful is this to you
If FenixLib is useful to you but donating is not an option for you, don't worry, you can still contribute by simply letting me know where and how do you use FenixLib. I am honour with every end-user and very willing to know about projects using my work.

## License
Copyright 2016 Darío Cutillas Carrillo

FenixLib is distributed under the very permisive 
[Apache License, Version 2.0] (http://www.apache.org/licenses/LICENSE-2.0)

You are free to use FenixLib in both commercial and non commercial and 
open or close source applications as long as you follow the terms of the 
License.
