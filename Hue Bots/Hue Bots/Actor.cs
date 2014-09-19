using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hue_Bots
{
	public class Actor
	{
		public const int WIDTH = 64;

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

		public Vector2 Velocity;
		
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

		public Actor()
		{
			_box = new Rectangle();
			_box.Width = WIDTH;
			_box.Height = WIDTH;
		}

		public void Snap()
		{
			X = (int)Math.Round(Position.X / WIDTH) * WIDTH;
			Y = (int)Math.Round(Position.Y / WIDTH) * WIDTH;
		}

		public bool ColorMatches(int otherColor)
		{
			return this.Color == 0 || otherColor == 0 || this.Color == 7 || otherColor == 7 || this.Color == otherColor;
		}

		public virtual void Update() { }

		public virtual void Draw(SpriteBatch sb) { }
	}
}
