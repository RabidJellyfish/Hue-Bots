using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hue_Bots
{
	class Wall : Actor
	{
		public Wall(int x, int y, int color) : base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_wall, Position, null, MainGame.COLORS[Color], 0, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
		}
	}
}
