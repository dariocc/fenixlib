![FenixLib](Media/fenixlib.png "FenixLib Logo")

FenixLib brings .Net support for opening, creating and operating with
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
var spriteAssortment = NativeFile.LoadFpg ( "myfpg.fpg" );

// Print out the code and description of every sprite in the Fpg
foreach ( var sprite in spriteAssortment )
{
	System.Console.WriteLine ( sprite.Id.ToString() + " - " + sprite.Description );
}

// Change the description of Sprite with code 10
spriteAssortment[10].Description = "My graphic";

spriteAssortment.SaveToFpg ( "modified.fpg" );
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
font.SaveToFnt ( "myfont.fnt" );
```

A battery of [examples](https://github.com/dacucar/fenixlib/wiki/Examples) is provided that coverts most common use cases.

## Using the library
Add a reference to your .NET project by using the [nuget package](https://www.nuget.org/packages/FenixLib/)
or building it from the sources.

If you know what [Fpg](https://github.com/dacucar/fenixlib/wiki/Native-Format#Fpg),  [Map](https://github.com/dacucar/fenixlib/wiki/Native-Format#Map), [Pal](https://github.com/dacucar/fenixlib/wiki/Native-Format#Pal) and [Fnt](https://github.com/dacucar/fenixlib/wiki/Native-Format#Pal) files, working with FenixLib should be quite strightforward, just make sure to go through the information in the following topics. 

* [Core Types](https://github.com/dacucar/fenixlib/wiki/Core-Types)
* [IO Api](https://github.com/dacucar/fenixlib/wiki/IO-Api)

If you don't, just have a look to the [native formats introduction](https://github.com/dacucar/fenixlib/wiki/Native-Formats) and then you will be ready to the topics above.

Or, if you do not like to read, checkout the [example projects](https://github.com/dacucar/fenixlib/wiki/Examples).

## Building
FenixLib core assembly (FenixLib.dll) runs in any platform with support for .NET framework 4.5 and C# as it has no other dependencies. I alternate development in [VisualStudio.NET](https://www.visualstudio.com/en-us/products/vs-2015-product-editions.aspx) and
[MonoDevelop / Xamarin](http://www.monodevelop.com/) and I have succesfully built it from Windows, Linux and OSX.

Go ahead and make a clone of this repository:

    git clone https://github.com/dacucar/fenixlib.git

Then open the ``FenixLib.Sln`` within the ``./fenixlib/FenixLib`` folder with either Visual Studio, MonoDevelop or Xamarin and you are ready to go... FenixLib builds as any other class library.

Additional assemblies might however be dependent on additional libraries, for example [FenixLib.Gdk](https://github.com/dacucar/fenixlib/wiki/FenixLibCairoAssembly) offers utility classes to interacting with [Mono.Gdk](http://docs.go-mono.com/index.aspx?link=N:Gdk), which requires Mono and Gtk to be installed.

### Running unit and integration tests
Along with ``FenixLib`` C# project there is a ``FenixLib.Tests`` project that contains unit and integration tests to validate the functionality of the library. Complementary projects may be accompained by a Test project following the pattern  ``<ProjectName>.Tests``.

It is my intention to cover as much work units as possible via unit tests but I do not always strictly follow a test-first pattern which makes that some part of the library might 
not be fully covered. Once the base functionality that the library is intending to offer is completed and covered with unit-tests I intend to adopt a test-first approach for future
modifications.

I use [NUnit](http://www.nunit.org/) as testing framework and [Rhino Mocks](https://www.hibernatingrhinos.com/oss/rhino-mocks) as mocking framework. Both are available as NuGet packages
and work seamlessly with both Visual Studio and MonoDevelop / Xamarin.

For each FenixLib assembly there will be, if any, only one Test project. All test projects have a similar structure:
* An ``Unit`` folder containing all unit tests. I put a lot of effor in doing code refractoring to minimize the overlapping between units and in most situation I try to only test public API. 
  However, I do make use of the InternalsVisible and test ``internal`` functionality when I decide it is to early to expose a certain class outside the assembly. All unit tests are grouped under the
  ''unit'' TestCategory.

* An ``Integration`` folder containing tests of groups of functionality. These tests help me test real-case situations such as decoding real files, etc. The tests are still written 
  as NUnit TestFixtures, but their motivation is different. They are grouped under the ''integration'' TestCategory.
  
When cloning the repository, make sure to run the tests to be sure that the library functionality is not broken.

## Contributing
There basically three ways in which you can contribute:
  1. Reporting issues or feature requests. 
  2. Sending your code contributions.
  3. Letting me know how useful is this to you.
  4. Hire me!

Well, that was 5 actually! Let's have a look on each of them:

### Reporting issues or feature requests
Use the [issues](https://github.com/dacucar/fenixlib/issues) section. Any feature request shall also be written there.

### Sending your code contributions
If you modify FenixLib or extended and you think your modifications are useful for everyone, you are welcomed to send your pull requests. Be sure to have a look at existing code so as your code is formatted consistently with FenixLib standards and so as you follow same type of naming convention.

### Letting me know how useful is this to you
If FenixLib is useful to you but donating is not an option for you, don't worry, you can still contribute by simply letting me know where and how do you use FenixLib. I am honour with every end-user and very willing to know about projects using my work.

### Hire me! :)
I develop FenixLib for free and for the fun of it. If you believe that something you see here might suit your projects needs don't be shy feel free to contact me.

## License
Copyright 2016-2017 Darío Cutillas Carrillo

FenixLib is distributed under the very permisive 
 [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0)

You are free to use FenixLib in both commercial and non commercial and 
open or close source applications as long as you follow the terms of the 
License.
