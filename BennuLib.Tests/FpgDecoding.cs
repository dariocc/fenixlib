using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BennuLib;
using BennuLib.IO;

namespace BennuLib.Tests
{
    [TestClass]
    public class FpgDecoding
    {
        private SpriteAsset DecodeFpg(string path, int depth)
        {
            SpriteAsset fpg = File.LoadFpg(path);

            Assert.AreEqual(depth, fpg.Depth);
            Assert.AreEqual(3, fpg.Sprites.Count);
            Assert.IsNotNull(fpg.Sprites);

            // Validate the dimensions
            Assert.AreEqual(304, fpg[1].Width);
            Assert.AreEqual(256, fpg[100].Width);
            Assert.AreEqual(256, fpg[500].Width);
            Assert.AreEqual(315, fpg[1].Height);
            Assert.AreEqual(256, fpg[100].Height);
            Assert.AreEqual(256, fpg[500].Height);

            // Validate description
            Assert.AreEqual("hippo", fpg[1].Description);
            Assert.AreEqual("parrot", fpg[100].Description);
            Assert.AreEqual("penguin", fpg[500].Description);

            // Validate control points
            Assert.AreEqual(2, fpg[500].PivotPoints.Count);

            return fpg;
        }

        [TestMethod]
        public void DecodeFpg_8bppUncompressed()
        {
           var fpg = DecodeFpg("./Fpg/8bpp-uncompressed.fpg", 8);
            Assert.IsNotNull(fpg.Palette);
        }

        [TestMethod]
        public void DecodeFpg_16bppUncompressed()
        {
            var fpg = DecodeFpg("./Fpg/16bpp-uncompressed.fpg", 16);
        }

        [TestMethod]
        public void DecodeFpg_32bppUncompressed()
        {
            var fpg = DecodeFpg("./Fpg/32bpp-uncompressed.fpg", 32);
        }
    }
}
