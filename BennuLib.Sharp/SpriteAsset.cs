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
		[Obsolete()]
		private bool IsIdValid(int x)
		{
			return x >= MinCode & x <= MaxCode;
		}

		private IDictionary<int, Sprite> _sprites = new SortedDictionary<int, Sprite>();

		private Palette _palette;

		public Sprite this[int code] {
			get { return _sprites[code]; }
		}

		internal Palette Palette {
			get { return _palette; }
		}

		[Obsolete()]

		private short _depth;
		[Obsolete()]
		public short Depth {
			get { return _depth; }
		}

		public int Count {
			get { return _sprites.Count; }
		}

		public ICollection<Sprite> Sprites {
			get { return _sprites.Values; }
		}

		public void Add(int code, ref Sprite sprite)
		{
			// TODO: ensure palette constraint
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
