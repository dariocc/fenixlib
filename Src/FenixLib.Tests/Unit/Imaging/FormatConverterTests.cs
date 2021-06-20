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
using NUnit.Framework;
using FenixLib.Core;
using Moq;

namespace FenixLib.Tests.Imaging
{
    [TestFixture]
    class FormatConverterTests
    {
        private Mock<IGraphic> stubGraphic1bpp;
        private Mock<IGraphic> stubGraphic8bpp;
        private Mock<IGraphic> stubGraphic16bpp;
        private Mock<IGraphic> stubGraphic32bpp;

        private IGraphic StubGraphic1bpp => stubGraphic1bpp.Object;
        private IGraphic StubGraphic8bpp => stubGraphic8bpp.Object; 
        private IGraphic StubGraphic16bpp => stubGraphic16bpp.Object; 
        private IGraphic StubGraphic32bpp => stubGraphic32bpp.Object;

        [SetUp]
        public void SetUp ()
        {
            SetUpMonochromeSetup ();
            SetUpIndexedSetup ();
        }

        private void SetUpMonochromeSetup()
        {
            // 10x3 1bpp data
            // 11111111 11xxxxx
            // 10000000 01xxxxx
            // 11111111 11xxxxx
            byte[] pixelData1bpp = new byte[6];
            pixelData1bpp[0] = 0xFF; pixelData1bpp[1] = 0x3 << 6;
            pixelData1bpp[2] = 0x1 << 7; pixelData1bpp[3] = 0x1 << 6;
            pixelData1bpp[4] = 0xFF; pixelData1bpp[5] = 0x3 << 6;

            stubGraphic1bpp = new Mock<IGraphic> ();
            stubGraphic1bpp.Setup ( x => x.PixelData ).Returns ( pixelData1bpp );
            stubGraphic1bpp.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format1bppMonochrome );
        }

        private void SetUpIndexedSetup()
        {
            // 10x3 8bpp data
            // 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF
            // 0xFF 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00 0xFF
            // 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF 0xFF
            byte[] pixelData8bpp = new byte[30];
            for ( int i = 0 ; i < 10 ; i++ )
            {
                pixelData8bpp[i] = 0xFF;
                pixelData8bpp[29 - i] = 0xFF;
            }
            pixelData8bpp[10] = 0xFF;
            for ( int i = 11 ; i < 19 ; i++ )
            {
                pixelData8bpp[i] = 0x00;
            }
            pixelData8bpp[19] = 0xFF;
            // Palette colors
            PaletteColor[] colors = new PaletteColor[256];
            colors[0] = new PaletteColor ( 0, 0, 0 );
            colors[1] = new PaletteColor ( 255, 255, 255 );
            colors[2] = new PaletteColor ( 255, 0, 0 );
            colors[3] = new PaletteColor ( 0, 255, 0 );
            colors[4] = new PaletteColor ( 0, 0, 255 );
            stubGraphic8bpp = new Mock<IGraphic> ();
            stubGraphic8bpp.Setup ( x => x.PixelData ).Returns ( pixelData8bpp );
            stubGraphic8bpp.Setup ( x => x.Palette ).Returns ( new Palette ( colors ) );
            stubGraphic8bpp.Setup ( x => x.GraphicFormat ).Returns ( GraphicFormat.Format8bppIndexed );
        }

        private void SetUp16bppSetup()
        {
            ushort[,] pixelData = new ushort[10, 3];
            for (int i = 0 ; i < pixelData.GetLength(0) ; i++ )
            {
                for (int j = 0 ; j < pixelData.GetLength(1) ; i ++ )
                {
                    if ( i == 0 || i == 3 || j == 0 || j == 9 )
                        pixelData[i, j] = 0xFFFF;
                    else
                        pixelData[i, j] = 0x00;
                }
            }
        }

        [Test]
        public void Convert_MonochromeToIndexed_CheckFormat ()
        {

        }
        [Test]
        public void Convert_MonochromeTo16bpp_CheckFormat ()
        {

        }
        [Test]
        public void Convert_MonochromeTo32bpp_CheckFormat ()
        {

        }
        [Test]
        public void Convert_IndexedTo16bpp_CheckFormat ()
        {

        }
        [Test]
        public void Convert_IndexedTo32bpp_CheckFormat ()
        {

        }
        [Test]
        public void Convert_16bppTo32bpp_CheckFormat ()
        {

        }
    }
}
