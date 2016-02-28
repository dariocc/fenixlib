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
using System.Collections.Generic;
using FenixLib.Core;
using static FenixLib.IO.NativeFormat;

namespace FenixLib.Image
{
    public class BasicConverter : IFormatConverter
    {
        public byte[] Convert ( IGraphic graphic, GraphicFormat format )
        {
            if ( graphic == null )
                throw new ArgumentNullException ( "Parameter 'graphic' cannot be null." );

            if ( format == null )
                throw new ArgumentNullException ( "Parameter 'format' cannot be null." );

            byte[] destBuffer;

            using ( var source = PixelReader.Create ( graphic ) )
            {
                using ( var dest = PixelWriter.Create ( format, graphic.Width, graphic.Height,
                    graphic.Palette ) )
                {
                    while ( source.HasPixels )
                    {
                        source.Read ();
                        dest.Write ( source.alpha, source.r, source.g, source.b );
                    }

                    destBuffer = dest.GetPixels ();
                }
            }

            return destBuffer;
        }
    }

    internal abstract class PixelWriter : IDisposable
    {
        private byte[] DestBuffer { get; set; }
        private Stream BaseStream { get; set; }
        protected BinaryWriter Writer { get; private set; }
        protected int Width { get; private set; }
        protected int Height { get; private set; }

        public abstract void Write ( int alpha, int r, int g, int b );

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose ( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Writer.Dispose ();
                    BaseStream.Dispose ();
                }

                disposedValue = true;
            }
        }

        public void Dispose ()
        {
            Dispose ( true );
        }
        #endregion

        public static PixelWriter Create ( GraphicFormat format, int width, int height, Palette destPalette = null )
        {
            if ( width <= 0 || height <= 0 )
                throw new ArgumentOutOfRangeException ( "Width and Height need to be greater than 0." );

            PixelWriter pixelWriter;

            if ( format == GraphicFormat.ArgbInt32 )
            {
                pixelWriter = new PixelWriterArgbInt32 ();
            }
            else if ( format == GraphicFormat.RgbInt16 )
            {
                pixelWriter = new PixelWriterRgbInt16 ();
            }
            else if ( format == GraphicFormat.RgbIndexedPalette )
            {
                if ( destPalette == null )
                {
                    throw new ArgumentNullException ( "Parameter 'destPalette' cannot be null for "
                        + "indexed palette graphic format." );
                }

                pixelWriter = new PixelWriterRgbIndexed ( destPalette );
            }
            else if ( format == GraphicFormat.Monochrome )
            {
                pixelWriter = new PixelWriterMonochrome ();
            }
            else
            {
                throw new ArgumentOutOfRangeException ( "Unsupported GraphicFormat." );
            }

            pixelWriter.Width = width;
            pixelWriter.Height = height;

            byte[] destBuffer = new byte[CalculatePixelBufferBytes (
                format.BitsPerPixel, width, height )];

            pixelWriter.DestBuffer = destBuffer;
            pixelWriter.BaseStream = new MemoryStream ( destBuffer );
            pixelWriter.Writer = new BinaryWriter ( pixelWriter.BaseStream );

            return pixelWriter;
        }

        public byte[] GetPixels ()
        {
            BaseStream.Flush ();
            return DestBuffer;
        }

    }

    internal class PixelWriterArgbInt32 : PixelWriter
    {
        public override void Write ( int alpha, int r, int g, int b )
        {
            int value = ( alpha << 24 ) | ( r << 16 ) | ( g << 8 ) | b;
            Writer.Write ( value );
        }
    }

    internal class PixelWriterRgbInt16 : PixelWriter
    {
        public override void Write ( int alpha, int r, int g, int b )
        {
            ushort value;

            if ( alpha < 128 )
            {
                value = 0;
            }
            else
            {
                value = ( ushort ) ( ( ( r >> 3 ) << 11 ) | ( ( g >> 2 ) << 5 ) | ( b >> 3 ) );
            }

            Writer.Write ( value );
        }
    }

    internal class PixelWriterRgbIndexed : PixelWriter
    {
        private HslColor[] _hslPalette;
        private IDictionary<Color, int> ColorCache = new Dictionary<Color, int> ();

        public PixelWriterRgbIndexed ( Palette destPalette )
        {
            _hslPalette = PrepareHslPalette ( destPalette );
        }

        public override void Write ( int alpha, int r, int g, int b )
        {
            int paletteIndex;

            if ( alpha < 128 )
            {
                paletteIndex = 0;
            }
            else
            {
                paletteIndex = FindNearestInPalette ( r, g, b );
            }

            Writer.Write ( ( byte ) paletteIndex );
        }

        private int FindNearestInPalette ( int r, int g, int b )
        {
            Color c = new Color ( r, g, b );

            if ( ColorCache.ContainsKey ( c ) )
            {
                return ColorCache[c];
            }

            HslColor hslColor = HslColor.FromRgb ( r, g, b );

            HslWeightFactor weights;

            if ( hslColor.L > 0.8 | hslColor.L < 0.2 )
            {
                weights = new HslWeightFactor ( 0.1, 0.2, 0.7 );
            }
            else
            {
                weights = new HslWeightFactor ( 0.4, 0.4, 0.2 );
            }

            int index = -1;
            double minDistance = 360 * 360;
            int minDistanceIndex = 0;
            foreach ( HslColor paletteColor in _hslPalette)
            {
                index += 1;
                double distance = SquaredDistance ( hslColor, paletteColor, weights );
                if ( distance < minDistance )
                {
                    minDistance = distance;
                    minDistanceIndex = index;
                }
            }

            ColorCache[c] = minDistanceIndex;

            return minDistanceIndex;
        }

        private double SquaredDistance ( HslColor color1, HslColor color2, HslWeightFactor weights )
        {
            double hdiff = Math.Abs ( color1.H - color2.H ) / 360;
            double sdiff = Math.Abs ( color1.S - color2.S );
            double ldiff = Math.Abs ( color1.L - color1.L );

            return hdiff * hdiff * weights.H
                + ldiff * ldiff * weights.S
                + ldiff * ldiff * weights.L;
        }

        private static HslColor[] PrepareHslPalette ( Palette palette )
        {
            HslColor[] hslColors = new HslColor[palette.Colors.Length];

            for ( int i = 0 ; i < palette.Colors.Length ; i++ )
            {
                Color c = palette[i];

                hslColors[i] = HslColor.FromRgb ( c.r, c.g, c.b );
            }

            return hslColors;
        }

        private struct HslColor
        {
            public int H { get; }
            public double S { get; }
            public double L { get; }

            public HslColor ( int hue, double saturation, double luminosity )
            {
                if ( hue >= 360 || hue <= 0 )
                    throw new ArgumentOutOfRangeException ();

                if ( saturation > 1 )
                    throw new ArgumentOutOfRangeException ();

                if ( luminosity > 1 )
                    throw new ArgumentOutOfRangeException ();

                H = hue;
                S = saturation;
                L = luminosity;
            }

            public static HslColor FromRgb ( double r, double g, double b )
            {
                double max = Math.Max ( Math.Max ( r, g ), b );
                double min = Math.Min ( Math.Min ( r, g ), b );

                int h;
                if ( max == min )
                {
                    h = 0;
                }
                else if ( max == r )
                {
                    h = ( int ) ( ( 60 * ( g - b ) / ( max - min ) + 360 ) ) % 360;
                }
                else if ( max == g )
                {
                    h = ( int ) ( 60 * ( b - r ) / ( max - min ) + 120 );
                }
                else
                {
                    h = ( int ) ( 60 * ( r - g ) / ( max - min ) + 240 );
                }

                double l = 0.5 * ( max - min );

                double s;
                if ( max == min )
                {
                    s = 0;
                }
                else if ( l <= 0.5 )
                {
                    s = ( max - min ) / ( 2 * l );
                }
                else
                {
                    s = ( max - min ) / ( 2 - 2L );
                }

                return new HslColor ( h, s, l );
            }

            public static HslColor FromRgb ( int r, int g, int b )
            {
                return FromRgb ( r / 255.0, g / 255.0, b / 255.0 );
            }
        }

        private struct HslWeightFactor
        {
            public double H { get; }
            public double S { get; }
            public double L { get; }

            public HslWeightFactor ( double wh, double ws, double wl )
            {
                H = wh;
                S = ws;
                L = wl;
            }
        }
    }

    internal class PixelWriterMonochrome : PixelWriter
    {

        public override void Write ( int alpha, int r, int g, int b )
        {
            byte value;

            if ( alpha < 128 )
            {
                value = 0;
            }
            else
            {
                value = FindNearest ( r, g, b );
            }

            Writer.Write ( value );
        }

        private byte FindNearest ( int r, int g, int b )
        {
            double rn = r / 765.0;
            double gn = g / 765.0;
            double bn = b / 765.0;

            double avg = rn + gn + bn;

            if ( avg >= 0.5 )
                return 1;
            else
                return 0;
        }
    }

    internal abstract class PixelReader : IDisposable
    {
        protected Stream BaseStream { get; private set; }
        protected BinaryReader Reader { get; private set; }
        protected IGraphic Graphic { get; private set; }

        public int r { get; protected set; }
        public int g { get; protected set; }
        public int b { get; protected set; }
        public int alpha { get; protected set; }

        public abstract bool HasPixels { get; }

        public abstract void Read ();

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose ( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Reader.Dispose ();
                    BaseStream.Dispose ();
                }

                disposedValue = true;
            }
        }

        public void Dispose ()
        {
            Dispose ( true );
        }
        #endregion

        public static PixelReader Create ( IGraphic graphic )
        {
            PixelReader pixelReader;

            if ( graphic.GraphicFormat == GraphicFormat.ArgbInt32 )
                pixelReader = new PixelReaderArgbInt32 ();
            else if ( graphic.GraphicFormat == GraphicFormat.RgbIndexedPalette )
                pixelReader = new PixelReaderRgbInt16 ();
            else if ( graphic.GraphicFormat == GraphicFormat.RgbIndexedPalette )
                pixelReader = new PixelReaderRgbIndexed ();
            else if ( graphic.GraphicFormat == GraphicFormat.Monochrome )
                pixelReader = new PixelReaderMonochrome ();
            else
                throw new ArgumentOutOfRangeException ( "Unsupported GraphicFormat" );

            pixelReader.BaseStream = new MemoryStream ( graphic.PixelData );
            pixelReader.Reader = new BinaryReader ( pixelReader.BaseStream );
            pixelReader.Graphic = graphic;

            return pixelReader;
        }

    }

    internal class PixelReaderArgbInt32 : PixelReader
    {
        public override bool HasPixels
        {
            get
            {
                return ( BaseStream.Position + 4 < BaseStream.Length );
            }
        }

        public override void Read ()
        {
            int value = Reader.ReadInt32 ();

            alpha = ( value >> 8 ) & 0xFF0000;
            r = value & 0xFF0000;
            g = value & 0xFF00;
            b = value & 0xFF;
        }
    }

    internal class PixelReaderRgbInt16 : PixelReader
    {
        public override bool HasPixels
        {
            get
            {
                return ( BaseStream.Position + 2 < BaseStream.Length );
            }
        }

        public override void Read ()
        {
            int value = Reader.ReadInt16 ();

            r = value & 0xF800;
            g = value & 0x7E0;
            b = value & 0x1F;

            if ( value == 0 )
                alpha = 0;
            else
                alpha = 255;
        }
    }

    internal class PixelReaderRgbIndexed : PixelReader
    {
        public override bool HasPixels
        {
            get
            {
                return ( BaseStream.Position + 1 < BaseStream.Length );
            }
        }

        public override void Read ()
        {
            int value = Reader.ReadByte ();

            r = Graphic.Palette[value].r;
            g = Graphic.Palette[value].g;
            b = Graphic.Palette[value].b;

            if ( value == 0 )
                alpha = 0;
            else
                alpha = 255;
        }
    }

    internal class PixelReaderMonochrome : PixelReader
    {
        public override bool HasPixels
        {
            get
            {
                return ( BaseStream.Position + 4 < BaseStream.Length );
            }
        }

        public override void Read ()
        {
            int value = Reader.ReadInt16 ();

            r = value & 0xF800;
            g = value & 0x7E0;
            b = value & 0x1F;

            if ( value == 0 )
                alpha = 0;
            else
                alpha = 255;
        }
    }
}
