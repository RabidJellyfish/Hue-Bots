using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hue_Bots
{
	class Arrow : Actor
	{
		public Vector2 direction;

		public Arrow(int x, int y, int color, Vector2 direction)
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
			this.direction = direction;
		}

		public override void Draw(SpriteBatch sb)
		{
			float rotation = direction.X == -1 ? MathHelper.Pi : (direction.X == 1 ? 0 : (direction.Y == -1 ? 3 * MathHelper.PiOver2 : MathHelper.PiOver2));
			sb.Draw(MainGame.tex_arrow, Position + Vector2.One * 32, null, MainGame.COLORS[Color], rotation, new Vector2(32, 32), 1f, SpriteEffects.None, 1f);
		}
	}
}
