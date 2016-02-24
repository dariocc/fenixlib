# fenixlib
.NET support for opening, creating and operating with [PixTudio](https://pixtudio.org), 
BennuGD(https://bennugd.org) and [DIV](http://div-arena.co.uk/) native graphic, 
graphic collections, fonts and palette formats.

A code is worth more than a thousand words... Here there is how to load a Fpg file:
    using FenixLib.IO;
    
    SpriteAsset asset = File.LoadFpg ( path );
    foreach ( Sprite sprite in asset )
    {
        System.Console.WriteLine ( sprite.Description );
    }

## Compiling
Wip
## Contribute
Wip
## License
Copyright 2016 Darío Cutillas Carrillo

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

