using System;
using System.Collections.Generic;

namespace FpgFromFolder
{
    public interface IFiles2FpgMaker
    {
        void Make(IEnumerable<string> paths, string outFile);
        event EventHandler<ConversionEventArgs> FileAdded;
        event EventHandler<ConversionEventArgs> FileSkipped;
    }
}