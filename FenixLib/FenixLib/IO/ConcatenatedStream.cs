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

namespace FenixLib.IO
{
    /// <summary>
    /// A read-only stream that reads bytes from two streams as if they were concatenated.
    ///
    /// Invoking <see cref="Seek(long, SeekOrigin)"/>, <see cref="Write(byte[], int, int)"/>
    /// and <see cref="SetLength(long)"/> will originate a <see cref="NotSupportedException"/>.
    /// </summary>
    internal class ConcatenatedStream : Stream
    {
        private Stream stream1;
        private Stream stream2;
        private int _position;
        private int _stream1Length;

        public override bool CanRead { get; } = true;

        public override bool CanSeek { get; } = false;

        public override bool CanWrite { get; } = false;

        /// <summary>
        ///
        /// </summary>
        /// <param name="stream1">Stream1 needs to support the Length property and it
        /// cannot be longer than sizeof(int). Stream 2.</param>
        /// <param name="stream2"></param>
        public ConcatenatedStream ( Stream stream1, Stream stream2 )
        {
            _position = 0;

            if ( stream1.Length > sizeof ( int ) )
                throw new ArgumentException (); // TODO:Customize

            _stream1Length = ( int ) stream1.Length;

            this.stream1 = stream1;
            this.stream2 = stream2;
        }

        public override long Length
        {
            get
            {
                throw new NotSupportedException ();
            }
        }

        public override long Position
        {
            get
            {
                return _position;
            }

            set
            {
                throw new NotSupportedException ();
            }
        }

        public override void Flush ()
        {
            throw new NotSupportedException ();
        }

        public override int Read ( byte[] buffer, int offset, int count )
        {
            byte[] bytes = new byte[count];

            int bytesToReadFromStream1 = 0;
            int bytesReadFromStream1 = 0;
            int bytesReadFromStream2 = 0;

            if ( Position < _stream1Length )
            {
                if ( stream1.Length != _stream1Length )
                {
                    throw new InvalidOperationException (
                        "The length of the stream 1 has changed since the object was created."
                        );
                }

                int bytesLeftFromStream1 = _stream1Length - ( int ) stream1.Position;
                bytesToReadFromStream1 = Math.Min ( bytesLeftFromStream1, count );
                bytesReadFromStream1 = stream1.Read ( bytes, 0, bytesToReadFromStream1 );
            }

            if ( bytesReadFromStream1 < count && bytesReadFromStream1 == bytesToReadFromStream1 )
            {
                bytesReadFromStream2 = stream2.Read ( bytes, bytesReadFromStream1,
                    count - bytesReadFromStream1 );
            }

            bytes.CopyTo ( buffer, offset );

            _position = bytesReadFromStream1 + bytesReadFromStream2;
            return _position;
        }

        public override long Seek ( long offset, SeekOrigin origin )
        {
            throw new NotSupportedException ();
        }

        public override void SetLength ( long value )
        {
            throw new NotSupportedException ();
        }

        public override void Write ( byte[] buffer, int offset, int count )
        {
            throw new NotSupportedException ();
        }
    }
}
