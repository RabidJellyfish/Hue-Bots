using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hue_Bots
{
	class WallChoice : Actor
	{
		public static int Selection = -1;

		private int count;

		public WallChoice(int x, int y, int color, int count)
			: base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
			this.count = count;
		}

		public override void Update()
		{
			curMouse = Mouse.GetState();

			if (curMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
			{
				if (Box.Contains(curMouse.X, curMouse.Y))
					Selection = this.Color;
			}
			else if (curMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed)
			{
				bool onSpawner = false;
				foreach (Actor a in MainGame.actors)
				{
					if (a.Box.Contains(curMouse.X, curMouse.Y))
					{
						onSpawner = true;
						Selection = -1;
						break;
					}
				}
				if (!onSpawner && Selection == this.Color)
				{
					if (this.count > 0 && curMouse.X < MainGame.SCREEN_WIDTH - 320)
					{
						MainGame.actors.Add(new Wall(curMouse.X - 32, curMouse.Y - 32, Selection));
						MainGame.actors.Last().Snap();
						count--;
					}
					Selection = -1;
				}
			}

			prevMouse = curMouse;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_wall, Position, MainGame.COLORS[Color]);
			sb.DrawString(MainGame.fnt_font, count.ToString(), Position + Vector2.UnitX * 75, Microsoft.Xna.Framework.Color.Black);
		}
	}
}
