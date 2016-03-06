using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FenixLib.Core.Tests.IntegrationTests
{
    internal abstract class SpriteAssetElementComparer : IEqualityComparer<SpriteAssetElement>
    {
        private SpriteAssetElementComparer decorated;

        public SpriteAssetElementComparer ( SpriteAssetElementComparer comparer = null )
        {
            decorated = comparer;
        }

        public abstract bool CompareCore ( SpriteAssetElement x, SpriteAssetElement y );

        public abstract int CalculateHashCode ( SpriteAssetElement x );

        public bool Equals ( SpriteAssetElement x, SpriteAssetElement y )
        {
            bool result = true;

            if ( decorated != null )
            {
                result = decorated.CompareCore ( x, y ) ;
            }

            return result & CompareCore ( x, y );
        }

        public int GetHashCode ( SpriteAssetElement obj )
        {
            int hashCode = CalculateHashCode ( obj );

            if ( decorated != null )
            {
                hashCode = decorated.CalculateHashCode ( obj ) ^ CalculateHashCode ( obj );
            }
            else
            {
                hashCode = CalculateHashCode ( obj );
            }

            return hashCode ;
        }
    }
}
