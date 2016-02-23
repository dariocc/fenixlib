using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bennu
{
    public sealed class FontCodePage
    {
        private readonly int _codePage;
        public int Value { get { return _codePage; } }

        private FontCodePage(int codePage)
        {
            _codePage = codePage;
        }

        internal static FontCodePage FromEncoding(Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 850:
                    return CP850;
                case 28591:
                    return ISO85591;
                default:
                    throw new ArgumentException (); // TODO: Customize
            }
        }

        public static FontCodePage ISO85591 = new FontCodePage(28591);
        public static FontCodePage CP850 = new FontCodePage(850);
    }
}
