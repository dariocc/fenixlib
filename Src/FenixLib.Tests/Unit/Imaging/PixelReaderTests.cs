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
            var pixels = ReadAllPixels(Create1bppGraphicStub());
            var white = (255, 255, 255, 255);
            var black = (0, 0, 0, 0);
            var expectedPixels = new List<(int R, int G, int B, int A)>()
            {
                white, white, white, white, white, white, white, white, white, white,
                white, black, black, black, black, black, black, black, black, white,
                white, white, white, white, white, white, white, white, white, white,
            };

            Assert.That(pixels, Is.EquivalentTo(expectedPixels));
        }

        [Test]
        public void Read_WorksAsExpectedForIndexedGraphics()
        {
            var pixels = ReadAllPixels(Create8bbpGraphicStub());
            var color0 = (0, 0, 0, 0);
            var color1 = (128, 0, 255, 255);
            var color2 = (0, 0, 0, 255);
            var expectedPixels = new List<(int R, int G, int B, int A)>()
            {
                color0, color1, color2,
                color2, color1, color0
            };
        }

        private static List<(int R, int G, int B, int A)> ReadAllPixels(IGraphic graphic)
        {
            var pixelReader = PixelReader.Create(graphic);

            var pixels = new List<(int R, int G, int B, int A)>();
            while (pixelReader.HasPixels)
            {
                pixelReader.Read();
                var color = (R: pixelReader.R, G: pixelReader.G, B: pixelReader.B, A: pixelReader.Alpha);
                pixels.Add(color);
            }

            return pixels;
        }

        private static IGraphic Create1bppGraphicStub()
        {
            // 10x3 1bpp data
            // 11111111 11xxxxx
            // 10000000 01xxxxx
            // 11111111 11xxxxx
            byte[] pixelData = new byte[6];
            pixelData[0] = 0xFF; pixelData[1] = 0x3 << 6;
            pixelData[2] = 0x1 << 7; pixelData[3] = 0x1 << 6;
            pixelData[4] = 0xFF; pixelData[5] = 0x3 << 6;

            var graphic = new Mock<IGraphic>();
            graphic.Setup(x => x.PixelData).Returns(pixelData);
            graphic.Setup(x => x.GraphicFormat).Returns(GraphicFormat.Format1bppMonochrome);
            graphic.Setup(x => x.Width).Returns(10);
            graphic.Setup(x => x.Height).Returns(3);

            return graphic.Object;
        }

        private static IGraphic Create8bbpGraphicStub()
        {
            // Palette colors
            PaletteColor[] colors = new PaletteColor[256];
            colors[0] = new PaletteColor(0, 0, 0);
            colors[1] = new PaletteColor(128, 0, 255);
            colors[2] = new PaletteColor(0, 0, 0);
            // 3x2 8bpp data
            // 0 1 2
            // 2 1 0 
            byte[] pixelData = new byte[] {
                0, 1, 2,
                2, 1, 0
            };
            var graphic = new Mock<IGraphic>();
            graphic.Setup(x => x.PixelData).Returns(pixelData);
            graphic.Setup(x => x.Palette).Returns(new Palette(colors));
            graphic.Setup(x => x.GraphicFormat).Returns(GraphicFormat.Format8bppIndexed);
            graphic.Setup(x => x.Width).Returns(4);
            graphic.Setup(x => x.Height).Returns(3);
            return graphic.Object;
        }
    }

}