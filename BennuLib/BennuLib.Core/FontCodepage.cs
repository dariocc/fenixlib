using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennuLib
{
    public sealed class FontCodePage
    {
        private readonly int _codePage;
        public int Value { get { return _codePage; } }

        private FontCodePage(int codePage)
        {
            _codePage = codePage;
        }

        public static FontCodePage ISO85591 = new FontCodePage(28591);
        public static FontCodePage CP850 = new FontCodePage(850);
    }
}
