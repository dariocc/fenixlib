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

namespace FenixLib.IO
{
    public abstract class NativeFormatReader : IDisposable
    {
        public NativeFormatReader ( Stream input )
        {
            if ( input == null )
            { 
                throw new ArgumentNullException ( nameof ( input ) );
            }

            BaseStream = input;
        }

        public abstract string ReadAsciiZ ( int length );
        public abstract GlyphInfo ReadExtendedFntGlyphInfo ();
        public abstract NativeFormat.Header ReadHeader ();
        public abstract GlyphInfo ReadLegacyFntGlyphInfo ();
        public abstract Palette ReadPalette ();
        public abstract byte[] ReadPaletteGammas ();
        public abstract PivotPoint[] ReadPivotPoints ( int number );
        public abstract int ReadPivotPointsMaxIdInt32 ();
        public abstract int ReadPivotPointsMaxIdUint16 ();
        public abstract byte[] ReadPixels ( GraphicFormat format, int width, int height );
        public abstract int ReadInt32();
        public abstract short ReadInt16();
        public abstract byte ReadByte();
        public abstract byte[] ReadBytes ( int number );
        public abstract int ReadUInt16 ();

        public Stream BaseStream { get; }
        public abstract void Dispose ();
    }
}