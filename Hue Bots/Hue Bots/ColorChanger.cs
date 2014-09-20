using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hue_Bots
{
	public class ColorChanger : Actor
	{
		public ColorChanger(int x, int y, int color)
			: base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_changer, Position, MainGame.COLORS[Color]);
		}
	}
}
