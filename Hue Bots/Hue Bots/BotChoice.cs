using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hue_Bots
{
	class BotChoice : Actor
	{
		public static int Selection = -1;

		private int count;

		public BotChoice(int x, int y, int color, int count) : base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
			this.count = count;
		}

		public override void Update()
		{
			if (curMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
			{
				if (Box.Contains(curMouse.X, curMouse.Y))
					Selection = this.Color;
			}
			else if (curMouse.LeftButton == ButtonState.Released && prevMouse.RightButton == ButtonState.Pressed)
			{
				if (Selection == this.Color && this.count > 0)
				{
					count--;
				}
				Selection = -1;
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_bot, Position, MainGame.COLORS[Color]);
		}
	}
}
