using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hue_Bots
{
	public class Static
	{
		public int Color { get; set; }

		private Vector2 _pos;
		public Vector2 Position
		{
			get { return _pos; }
			set
			{
				_pos = value;
				_box.X = (int)_pos.X;
				_box.Y = (int)_pos.Y;
			}
		}
		
		public int X 
		{ 
			get { return (int)_pos.X; } 
			set 
			{ 
				_pos.X = value;
				_box.X = value;
			} 
		}		
		public int Y 
		{
			get { return (int)_pos.Y; } 
			set 
			{ 
				_pos.Y = value;
				_box.Y = value;
			}
		}

		private Rectangle _box;
		public Rectangle Box { get { return _box; } }

		public Static()
		{
			_box = new Rectangle();
			_box.Width = Actor.WIDTH;
			_box.Height = Actor.WIDTH;
		}

		public virtual void Draw(SpriteBatch sb) { }
	}
}
