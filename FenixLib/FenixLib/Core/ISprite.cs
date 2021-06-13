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
using System.Collections.Generic;

namespace FenixLib.Core
{
    public interface ISprite : IGraphic
    {
        string Description { get; set; }

        PivotPoint Center { get; }

        ICollection<PivotPoint> PivotPoints { get; }

        void ClearPivotPoints ();

        void SetCenter (int x, int y);

        void DefinePivotPoint (int id, int x, int y);

        void DeletePivotPoint (int id);

        PivotPoint GetPivotPoint (int id);

        int? FindFreePivotPointId (int start = 0, 
                                    Sprite.SearchDirection direction = Sprite.SearchDirection.Fordward);

        bool IsPivotPointDefined (int id);
    }
}