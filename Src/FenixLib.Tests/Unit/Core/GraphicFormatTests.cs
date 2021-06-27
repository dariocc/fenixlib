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
using System.Collections;
using FenixLib.Core;

namespace FenixLib.Tests.Unit.Core
{
    [TestFixture (Category = "Unit")]
    public class GraphicFormatTests
    {
        [TestCaseSource ( "PixelsBytesForSizeTestCases" )]
        public int PixelsBytesForSize_FormatAndSizeTestCases_ReturnsAsTestCaseExpects (
            GraphicFormat format, int width, int height )
        {
            return format.PixelsBytesForSize ( width, height );
        }

        public static IEnumerable PixelsBytesForSizeTestCases
        {
            get
            {
                // Format, Width, Height
                yield return new TestCaseData ( new object[] { GraphicFormat.Format32bppArgb, 10, 10 } ).Returns ( 400 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format16bppRgb565, 10, 10 } ).Returns ( 200 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format8bppIndexed, 10, 10 } ).Returns ( 100 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format1bppMonochrome, 10, 10 } ).Returns ( 20 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format1bppMonochrome, 8, 1 } ).Returns ( 1 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format1bppMonochrome, 9, 1 } ).Returns ( 2 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format1bppMonochrome, 81, 2 } ).Returns ( 22 );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format1bppMonochrome, 80, 10 } ).Returns ( 100 );
            }
        }

        [TestCaseSource ( "FromIntOperatorTestCases" )]
        public GraphicFormat FromIntOperator_FromIntTestCases_ReturnsAsTestCaseExpects ( int value )
        {
            return ( GraphicFormat ) value;
        }

        public static IEnumerable FromIntOperatorTestCases
        {
            get
            {
                // Format, Width, Height
                yield return new TestCaseData ( new object[] { 32 } ).Returns ( GraphicFormat.Format32bppArgb );
                yield return new TestCaseData ( new object[] { 16 } ).Returns ( GraphicFormat.Format16bppRgb565 );
                yield return new TestCaseData ( new object[] { 8 } ).Returns ( GraphicFormat.Format8bppIndexed );
                yield return new TestCaseData ( new object[] { 1 } ).Returns ( GraphicFormat.Format1bppMonochrome );
            }
        }

        [TestCaseSource ( "ToIntOperatorTestCases" )]
        public void ToIntOperator_ToIntOperatorCases_ReturnsBitsperPixels ( GraphicFormat format )
        {
            Assert.AreEqual ( format.BitsPerPixel, ( int ) format );
        }

        public static IEnumerable ToIntOperatorTestCases
        {
            get
            {
                // Format, Width, Height
                yield return new TestCaseData ( new object[] { GraphicFormat.Format32bppArgb } );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format16bppRgb565 } );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format8bppIndexed } );
                yield return new TestCaseData ( new object[] { GraphicFormat.Format1bppMonochrome } );
            }
        }
    }
}