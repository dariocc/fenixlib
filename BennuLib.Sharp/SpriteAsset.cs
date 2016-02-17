using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
namespace BennuLib
{


	[Serializable()]
	public class SpriteAsset : IEnumerable<Sprite>
	{


		private const int MinCode = 1;
		private const int MaxCode = 999;

        public static SpriteAsset Create(DepthMode depthMode)
        {
            switch (depthMode)
            {
                case DepthMode.ArgbInt32:
                    return new SpriteAsset(32, null);
                case DepthMode.RgbInt16:
                    return new SpriteAsset(16, null);
                case DepthMode.Monochrome:
                    return new SpriteAsset(1, null);
                default:
                    throw new ArgumentException(); // TODO: Customize
            }
        }

        public static SpriteAsset Create(Palette palette)
        {
            if (palette.Colors.Length != 256)
                throw new ArgumentException();

            return new SpriteAsset(8, palette);
        }

        private SpriteAsset(int depth, Palette palette) {
            Depth = depth;
            Palette = palette;
        }


		[Obsolete()]
		private bool IsIdValid(int x)
		{
			return x >= MinCode & x <= MaxCode;
		}

		private IDictionary<int, Sprite> _sprites = new SortedDictionary<int, Sprite>();

		public Sprite this[int code] {
			get { return _sprites[code]; }
		}

		public Palette Palette { get; private set; }

        public int Depth { get; private set; }

		public ICollection<Sprite> Sprites {
			get { return _sprites.Values; }
		}

		public void Add(int code, ref Sprite sprite)
		{
            if (sprite.Depth != Depth)
                throw new InvalidOperationException();

			_sprites.Add(code, sprite);
			sprite.ParentAsset = this;
		}

		public void Update(int code, Sprite map)
		{
			if (_sprites.ContainsKey(code)) {
				_sprites.Remove(code);
			}

			_sprites.Add(code, map);
		}

		internal int IdOf(Sprite sprite)
		{
			foreach (KeyValuePair<int, Sprite> kvp in _sprites) {
				if (object.ReferenceEquals(kvp.Value, sprite)) {
					return kvp.Key;
				}
			}

			throw new ArgumentException();
			// TODO customize
		}

		public int FindFreeId(int startId = MinCode)
		{

			if (IsIdValid(startId))
				throw new ArgumentException();
			// TODO: Customize

			var found = false;
			var code = startId - 1;
			do {
				code += 1;
				if (!_sprites.ContainsKey(code)) {
					found = true;
				}
			} while (!(code == MaxCode | found));

			if (!found)
				throw new InvalidOperationException();
			// TODO: Customize 

			return code;
		}

		public int PreviousFreeCode(int startId = MaxCode)
		{
			if (IsIdValid(startId))
				throw new ArgumentException();
			// TODO: Customize

			var found = false;
			var code = startId + 1;
			do {
				code -= 1;
				if (!_sprites.ContainsKey(code)) {
					found = true;
				}
			} while (!(code == MinCode | found));

			if (!found)
				throw new InvalidOperationException();
			// TODO: Customize 

			return code;
		}

		public IEnumerator<Sprite> GetEnumerator()
		{
			return _sprites.Values.GetEnumerator();
		}

		private IEnumerator IEnumerable_GetEnumerator()
		{
			return _sprites.Values.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return IEnumerable_GetEnumerator();
		}
	}
}
