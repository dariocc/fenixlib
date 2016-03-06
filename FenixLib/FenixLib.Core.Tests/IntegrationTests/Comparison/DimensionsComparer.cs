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

namespace FenixLib.Core.Tests.IntegrationTests.Comparison
{
    internal class DimensionsComparer : SpriteComparer
    {
        public DimensionsComparer ( SpriteComparer comparer = null )
            : base ( comparer )
        { }

        public override int CalculateHashCode ( ISprite x )
        {
            return x.Width.GetHashCode () ^ x.Height.GetHashCode ();
        }

        public override bool CompareCore ( ISprite x, ISprite y )
        {
            return x.Width == y.Width && x.Height == y.Height;
        }
    }
}
