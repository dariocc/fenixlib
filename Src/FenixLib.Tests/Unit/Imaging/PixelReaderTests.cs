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
using FenixLib.Core;
using NUnit.Framework;
using Moq;

namespace FenixLib.Imaging
{
    [TestFixture]
    public class PixelReaderTests
    {

        [Test]
        public void test()
        {
            var graphic = Create8bbpGraphicStub();
            var pixelReader = PixelReader.Create(graphic);
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