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

		public int Count { get; set; }

		public WallChoice(int x, int y, int color, int count)
			: base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
			this.Count = count;
		}

		public override void Update()
		{
			curMouse = Mouse.GetState();

			if (curMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
			{
				if (Box.Contains((int)(curMouse.X / MainGame.SCREEN_SCALE), (int)(curMouse.Y / MainGame.SCREEN_SCALE)) && this.Count > 0)
					Selection = this.Color;
			}
			else if (curMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed)
			{
				bool onSpawner = false;
				foreach (Actor a in MainGame.actors)
				{
					if (a.Box.Contains((int)(curMouse.X / MainGame.SCREEN_SCALE), (int)(curMouse.Y / MainGame.SCREEN_SCALE)))
					{
						onSpawner = true;
						Selection = -1;
						break;
					}
				}
				if (!onSpawner && Selection == this.Color)
				{
					if (this.Count > 0 && (int)(curMouse.X / MainGame.SCREEN_SCALE) < MainGame.SCREEN_WIDTH - 320)
					{
						MainGame.actors.Add(new Wall((int)(curMouse.X / MainGame.SCREEN_SCALE) - 32, (int)(curMouse.Y / MainGame.SCREEN_SCALE) - 32, Selection));
						MainGame.actors.Last().Snap();
						Count--;
					}
					Selection = -1;
				}
			}

			prevMouse = curMouse;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_wall, Position, this.Count > 0 ? MainGame.COLORS[Color] : new Color(MainGame.COLORS[Color].R, MainGame.COLORS[Color].G, MainGame.COLORS[Color].B, 50));
			sb.DrawString(MainGame.fnt_font, Count.ToString(), Position + Vector2.UnitX * 75, Microsoft.Xna.Framework.Color.Black);
		}
	}
}
