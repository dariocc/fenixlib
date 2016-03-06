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
using System.Linq;
using System.Security.Cryptography;

namespace FenixLib.Core.Tests.IntegrationTests.Comparison
{
    internal class PixelsComparer : SpriteComparer
    {
        public PixelsComparer ( SpriteComparer comparer = null ) : base ( comparer ) { }

        public override int CalculateHashCode ( ISprite x )
        {
            // Simple hashcode based on three elements of the PixelData array
            return x.PixelData[0].GetHashCode () 
                ^ x.PixelData[x.PixelData.Length / 2 - 1].GetHashCode ()
                ^ x.PixelData[x.PixelData.Length - 1].GetHashCode();
        }

        public override bool CompareCore ( ISprite x, ISprite y )
        {
            return x.PixelData.SequenceEqual ( y.PixelData );
        }
    }
}
