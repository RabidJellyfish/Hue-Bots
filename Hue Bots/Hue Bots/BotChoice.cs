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
		public static bool FoundSpawner = false;

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
			curMouse = Mouse.GetState();

			if (curMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
			{
				if (Box.Contains(curMouse.X, curMouse.Y))
					Selection = this.Color;
			}
			else if (curMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed)
			{
				if (Selection == this.Color && this.count > 0)
				{
					var spawners = from Actor s in MainGame.actors
								   where s is Spawner
								   select s as Spawner;
					foreach (Spawner s in spawners)
					{
						if (s.Box.Contains(curMouse.X, curMouse.Y))
						{
							MainGame.actors.Add(new Bot(s.X, s.Y, Selection, true));
							count--;
							Selection = -1;
							FoundSpawner = true;
							break;
						}
					}
				}
			}

			prevMouse = curMouse;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_bots[Color], Position, Microsoft.Xna.Framework.Color.White);
			sb.DrawString(MainGame.fnt_font, count.ToString(), Position + Vector2.UnitX * 75, Microsoft.Xna.Framework.Color.Black);
		}
	}
}
