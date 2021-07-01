![FenixLib](Media/fenixlib.png "FenixLib Logo")

![.NET](https://github.com/dariocc/fenixlib/actions/workflows/dotnet.yml/badge.svg)
[![License: Apache-2.0](https://img.shields.io/badge/License-Apache-2.0-yellow.svg)](https://opensource.org/licenses/Apache-2.0)

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

If you know what [Fpg](https://github.com/dacucar/fenixlib/wiki/Native-Format#Fpg),
[Map](https://github.com/dacucar/fenixlib/wiki/Native-Format#Map), 
[Pal](https://github.com/dacucar/fenixlib/wiki/Native-Format#Pal) and
[Fnt](https://github.com/dacucar/fenixlib/wiki/Native-Format#Pal) files, working with FenixLib should 
be quite straightforward, just make sure to go through the information in the following topics. 

* [Core Types](https://github.com/dacucar/fenixlib/wiki/Core-Types)
* [IO Api](https://github.com/dacucar/fenixlib/wiki/IO-Api)

If you don't, just have a look to the [native formats introduction](https://github.com/dacucar/fenixlib/wiki/Native-Formats) 
and then you will be ready to the topics above.

You may also check the [example projects](https://github.com/dacucar/fenixlib/wiki/Examples) for some inspiration.

## Building

Fenixlib core assembly (FenixLib.dll) was originally written to target .NET framework 4.5 but it has 
been migrated to .NET Standard 2.0 and therefore it should run on any .NET impelementation that
supports it, including .NET Framework > 4.5, Mono, .NET Core and .NET 5.0.

Go ahead and make a clone of this repository:

    git clone https://github.com/dacucar/fenixlib.git

Then build the fenixlib classlibrary. If using .NET 5.0 SDK, run:

    dotnet build Src/FenixLib

There exists some additional projects in the `Src` folder:

* `FenixLib.Tests`: Test for fenix core assembly.
* [`FenixLib.Gdk`](https://github.com/dacucar/fenixlib/wiki/FenixLibCairoAssembly). Basic functionality
  for mapping fenixlib images to `Gdk` pixbuffs and be therefore able to visualize images in
  Gdk applications.
* `FenixLib.Gdip`: Provides types for mapping fenixlib image data to Gdi+ images and to create
  fenixlib images from Gdi+ bitmaps.
* `FenixLib.Gdip.Tests`: Tests for `FenixLib.Gdip`.

And also some Examples under `Src/Examples` with a basic Winforms and Gdk applications. The winforms application
will not work on Linux environments, but who cares nowadays? :)

### Running unit and integration tests

[NUnit](http://www.nunit.org/) is the test framework used. At the time the test were being written I was still
learning about NUnit and .NET testing in general.

[Rhino Mocks](https://www.hibernatingrhinos.com/oss/rhino-mocks) was the mocking framework used at the time,
but because it doesn't work on .NET 5.0 I did a small migration effort to [Moq](https://github.com/Moq/moq).
I didn't spend lot of time making _the best possible migration_, just did the minimum to get them to 
build and pass the tests.

For each FenixLib assembly there will be, if any, only one Test project. All test projects have a similar structure:

* A ``Unit`` folder containing all unit tests. I put a lot of effor in doing code refractoring to minimize the overlapping 
  between units and in most situation I try to only test public API. 
  However, I do make use of the InternalsVisible and test ``internal`` functionality when I decide it is to early to expose 
  a certain class outside the assembly. All unit tests are grouped under the ''unit'' `TestCategory`.

* A ``Integration`` folder containing tests of groups of functionality. These tests help me test real-case situations 
  such as decoding real files, etc. The tests are still written They are grouped under the ''integration'' TestCategory.

> I'd normally prefer two separate assemblies for unit and integration tests, but this is an olde project where
> my testing habits were still in development.

Use whatever test runner your .NET implementation provides you. If using .NET 5.0:

    dotnet test Src/FenixLib.Tests

## Contributing

This project is not actively maintained but if for whatever reasons you find it useful don't hesitate
to contact me, open an issue or send your PRs and I'll look at them.

## License

Copyright 2016-2021 Darío Cutillas Carrillo

FenixLib is distributed under the very permisive 
 [Apache License, Version 2.0](http://www.apache.org/licenses/LICENSE-2.0)

You are free to use FenixLib in both commercial and non commercial and 
open or close source applications as long as you follow the terms of the 
License.
