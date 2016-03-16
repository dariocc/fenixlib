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
var asset = File.LoadFpg ( "myfpg.fpg" );

// Print out the code and description of every sprite in the Fpg
foreach ( var sprite in asset )
{
	System.Console.WriteLine ( sprite.Id.ToString() + " - " + sprite.Description );
}

// Change the description of Sprite with code 10
asset[10].Description = "My graphic";

File.SaveFpg ( "modified.fpg" );
```

Another example, see how easy you can create a Fnt font file from scratch:
```csharp
using FenixLib.Core;
using FenixLib.IO;

var font = new BitmapFont ( GraphicFormat.Format32bppArgb, FontEncoding.ISO85591 );

// Create a 10x10 transparent graphic
var glyphGraphic = new StaticGraphic( GraphicFormat.Format32bppArgb, 10, 10, new byte[10 * 10 * 4] );

// Create the glyph and assign it to the letter 'å'
font['å'] = new Glyph ( glyphGraphic );

// Save the font to a Fnt file (only 'å' will contain a bitmap)
File.SaveFnt ( 'myfont.fnt' );
```

## Using the library
If you know what [Fpg](https://github.com/dacucar/fenixlib/wiki/FpgFormat),  [Map](https://github.com/dacucar/fenixlib/wiki/MapFormat), [Pal](https://github.com/dacucar/fenixlib/wiki/PalFormat) and [Fnt](https://github.com/dacucar/fenixlib/wiki/FntFormat) files, working with FenixLib should be quite strightforward, just make sure to go through the information in the following topics. 

* [Core Types](https://github.com/dacucar/fenixlib/wiki/CoreTypes)
* [IO Api](https://github.com/dacucar/fenixlib/wiki/IOApi)

If you don't, just have a look to the [native formats introduction](https://github.com/dacucar/fenixlib/wiki/NativeFormats) and then you will be ready to the topics above.

## Compiling
FenixLib core assembly (FenixLib.dll) runs in any platform with support for .NET framework 4.5 and C# as it has no other dependencies. I alternate development in [VisualStudio.NET](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx) and
[MonoDevelop / Xamarin](http://www.monodevelop.com/) and I have succesfully built it from Windows, Linux and OSX.

Go ahead and make a clone of this repository:

    git clone https://github.com/dacucar/fenixlib.git
    
Then open the ```FenixLib.Sln``` within the ```./fenixlib/FenixLib``` folder with either Visual Studio, MonoDevelop or Xamarin and you are ready to go... FenixLib builds as any other class library. Note that as long as you have a C# compiler for the .NET 4.5 platform you do not need any IDE, but I guess that if you know that you already know what to do... 

Other assemblies might however be dependent on additional libraries, for example [FenixLib.Cairo](https://github.com/dacucar/fenixlib/wiki/FenixLibCairoAssembly) offers utility classes to interacting with Mono.Cairo, which requires, of course, Cairo(http://cairographics.org/).

## Contributing
There basically three ways in which you can contribute:
  1. Reporting issues or feature requests. 
  2. Sending your code contributions.
  3. Donating.
  4. Letting me know how useful is this to you.
  5. Hire me!

Well, that was 5 actually! Let's have a look on each of them:

### Reporting issues or feature requests
Use the [issues](https://github.com/dacucar/fenixlib/issues) section. Any feature request shall also be written there.

### Sending your code contributions
If you modify FenixLib or extended and you think your modifications are useful for everyone, you are welcomed to send your pull requests. Be sure to have a look at existing code so as your code is formatted consistently with FenixLib standards and so as you follow same type of naming convention.

### Donating
If FenixLib is useful to you and you want so, you are welcomed to fund this project. For the price of a cup of coffee at your local cafeteria, I will keep my spirit fully loaded to mantain and extend this project.

<a href='https://pledgie.com/campaigns/31179'><img alt='Click here to lend your support to: Support FenixLib open source project and make a donation at pledgie.com !' src='https://pledgie.com/campaigns/31179.png?skin_name=chrome' border='0' ></a>

If you are donating to get some particular feature done, you are very welcomed to specified it in your donation message!

### Letting me know how useful is this to you
If FenixLib is useful to you but donating is not an option for you, don't worry, you can still contribute by simply letting me know where and how do you use FenixLib. I am honour with every end-user and very willing to know about projects using my work.

### Hire me! :)
I develop FenixLib for free and for the fun of it. If you believe that something you see here might suit your projects needs don't be shy feel free to contact me.

## License
Copyright 2016 Darío Cutillas Carrillo

FenixLib is distributed under the very permisive 
[Apache License, Version 2.0] (http://www.apache.org/licenses/LICENSE-2.0)

You are free to use FenixLib in both commercial and non commercial and 
open or close source applications as long as you follow the terms of the 
License.
