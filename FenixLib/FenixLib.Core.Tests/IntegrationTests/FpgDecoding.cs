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
using System.Reflection;
using System.IO;

using FenixLib.Core;


namespace FenixLib.Tests.IntegrationTests
{
    [TestFixture, Category ( "Integration" )]
    public class FpgDecoding
    {
        [TestCase( "8bpp-uncompressed.fpg", 8)]
        [TestCase ( "8bpp-compressed.fpg", 8 )]
        [TestCase ( "16bpp-uncompressed.fpg", 8 )]
        [TestCase ( "16bpp-compressed.fpg", 16 )]
        [TestCase ( "32bpp-uncompressed.fpg", 32 )]
        [TestCase ( "32bpp-compressed.fpg", 32 )]
        public void FpgFileCanBeDecoded ( string resourceName, int expectedDepth )
        {
            var assembly = Assembly.GetExecutingAssembly ();
            string folder = Path.GetDirectoryName ( assembly.Location );
            string path = Path.Combine ( folder, "TestFiles", "Fpg", resourceName );

            SpriteAsset fpg = IO.File.LoadFpg ( path );

            // Assert.AreEqual ( depth, (int) fpg.GraphicFormat );
            Assert.AreEqual ( 3, fpg.Sprites.Count );
            Assert.IsNotNull ( fpg.Sprites );

            // Validate the dimensions
            Assert.AreEqual ( 304, fpg[1].Width );
            Assert.AreEqual ( 256, fpg[100].Width );
            Assert.AreEqual ( 256, fpg[500].Width );
            Assert.AreEqual ( 315, fpg[1].Height );
            Assert.AreEqual ( 256, fpg[100].Height );
            Assert.AreEqual ( 256, fpg[500].Height );

            // Validate description
            Assert.AreEqual ( "hippo", fpg[1].Description );
            Assert.AreEqual ( "parrot", fpg[100].Description );
            Assert.AreEqual ( "penguin", fpg[500].Description );

            // Validate control points
            Assert.AreEqual ( 2, fpg[500].PivotPoints.Count );
        }
        /*
        [Test]
        public void DecodeFpg_1bppCompressed ()
        {
            SpriteAsset fpg = File.LoadFpg ( "./Fpg/1bpp-compressed.fpg" );
            Assert.AreEqual ( fpg[1].Width, 10 );
            Assert.AreEqual ( fpg[1].Height, 10 );
            Assert.AreEqual ( fpg.GraphicFormat, 1);
            Assert.AreEqual ( fpg.Sprites.Count, 1 );
        }*/
    }
}
