using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hue_Bots
{
	public class Spawner : Actor
	{
		public Spawner(int x, int y) : base()
		{
			this.X = x;
			this.Y = y;
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_spawner, Position, Microsoft.Xna.Framework.Color.White);
		}
	}
}
