/*  Copyright 2016 Darío Cutillas Carrillo
*
*   Licensed under the Apache License, Version 2.0 (the "License");
*   you may not use this file except in compliance with the License.
*   You may obtain a copy of the License at
*
*       http://www.apache.org/licenses/LICENSE-2.0
*
*   Unless required by applicable law or agreed to in writing, software
*   distributed under the License is distributed on an "AS IS" BASIS,
*   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*   See the License for the specific language governing permissions and
*   limitations under the License.
*/
using System;
using System.IO;
using System.Collections.Generic;
using FenixLib.Core;
using NUnit.Framework;
using Moq;

namespace FenixLib.Imaging
{
    [TestFixture]
    public class PixelReaderTests
    {

        [Test]
        public void Read_WorksAsExpectedForMonochromeGraphics()
        {
            var graphic = Create1bppGraphicStub();
            var pixelReader = PixelReader.Create(graphic);

            var pixels = new List<(int R, int G, int B)>();
            while (pixelReader.HasPixels)
            {
                pixelReader.Read();
                var color = (R: pixelReader.R, G: pixelReader.G, B: pixelReader.B);
                pixels.Add(color);
            }

            var white = (255, 255, 255);
            var black = (0, 0, 0);
            var expectedPixels = new List<(int R, int G, int B)>()
            {
                white, white, white, white, white, white, white, white, white, white,
                white, black, black, black, black, black, black, black, black, white,
                white, white, white, white, white, white, white, white, white, white,
            };

            Assert.That(pixels, Has.Count.EqualTo(30));
            Assert.That(pixels, Is.EquivalentTo(expectedPixels));
        }

        private static IGraphic Create1bppGraphicStub()
        {
            // 10x3 1bpp data
            // 11111111 11xxxxx
            // 10000000 01xxxxx
            // 11111111 11xxxxx
            byte[] pixelData1bpp = new byte[6];
            pixelData1bpp[0] = 0xFF; pixelData1bpp[1] = 0x3 << 6;
            pixelData1bpp[2] = 0x1 << 7; pixelData1bpp[3] = 0x1 << 6;
            pixelData1bpp[4] = 0xFF; pixelData1bpp[5] = 0x3 << 6;

            var graphic = new Mock<IGraphic>();
            graphic.Setup(x => x.PixelData).Returns(pixelData1bpp);
            graphic.Setup(x => x.GraphicFormat).Returns(GraphicFormat.Format1bppMonochrome);
            graphic.Setup(x => x.Width).Returns(10);
            graphic.Setup(x => x.Height).Returns(3);

            return graphic.Object;
        }

        private static IGraphic Create8bbpGraphicStub()
        {
            // 10x3 8bpp data
            // 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF
            // 0xFF 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0xFF
            // 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF
            byte[] pixelData8bpp = new byte[30];
            for (int i = 0; i < 10; i++)
            {
                pixelData8bpp[i] = 0xFF;
                pixelData8bpp[29 - i] = 0xFF;
            }
            pixelData8bpp[10] = 0xFF;
            for (int i = 11; i < 19; i++)
            {
                pixelData8bpp[i] = 0x00;
            }
            pixelData8bpp[19] = 0xFF;
            // Palette colors
            PaletteColor[] colors = new PaletteColor[256];
            colors[0] = new PaletteColor(0, 0, 0);
            colors[1] = new PaletteColor(255, 255, 255);
            colors[2] = new PaletteColor(255, 0, 0);
            colors[3] = new PaletteColor(0, 255, 0);
            colors[4] = new PaletteColor(0, 0, 255);
            var graphic = new Mock<IGraphic>();
            graphic.Setup(x => x.PixelData).Returns(pixelData8bpp);
            graphic.Setup(x => x.Palette).Returns(new Palette(colors));
            graphic.Setup(x => x.GraphicFormat).Returns(GraphicFormat.Format8bppIndexed);
            graphic.Setup(x => x.Width).Returns(10);
            graphic.Setup(x => x.Height).Returns(3);
            return graphic.Object;
        }
    }

}