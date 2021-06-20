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
using System.Runtime.Serialization;

namespace FenixLib.Core
{

    [Serializable]
    public class UnsuportedFileFormatException : IOException
    {
        #region CommonConstructors
        public UnsuportedFileFormatException ()
            : base ()
        {
        }

        public UnsuportedFileFormatException ( string message )
            : base ( message )
        {
        }

        public UnsuportedFileFormatException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected UnsuportedFileFormatException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
        #endregion
    }

    [Serializable]
    public abstract class GraphicCollectionException : Exception
    {
        #region CommonConstructors
        public GraphicCollectionException ()
            : base ()
        {
        }

        public GraphicCollectionException ( string message )
            : base ( message )
        {
        }

        public GraphicCollectionException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected GraphicCollectionException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
        #endregion
    }

    [Serializable]
    public class FormatMismatchException : GraphicCollectionException
    {
        public GraphicFormat Expected { get; }
        public GraphicFormat Was { get; }

        public FormatMismatchException ( GraphicFormat expected = null,
            GraphicFormat was = null )
        {
            Expected = expected;
            Was = was;
        }

        #region CommonConstructors
        public FormatMismatchException ()
            : base ()
        {
        }

        public FormatMismatchException ( string message )
            : base ( message )
        {
        }

        public FormatMismatchException ( string message, Exception inner )
            : base ( message, inner )
        {
        }

        protected FormatMismatchException ( SerializationInfo info, StreamingContext context )
            : base ( info, context )
        {
        }
        #endregion

        public override string Message
        {
            get
            {
                string msg;

                if ( Expected != null && Was != null )
                {
                    msg = $", invalid graphic format (was:'{Was}', expected: '{Expected}')";
                }
                else if ( Was != null )
                {
                    msg = $", invalid graphic format (was:'{Was}')";
                }
                else if ( Expected != null )
                {
                    msg = $", invalid graphic format (expected: '{Expected}')";
                }
                else
                {
                    msg = $", invalid graphic format";
                }

                return base.Message + msg;
            }
        }
    }
}

