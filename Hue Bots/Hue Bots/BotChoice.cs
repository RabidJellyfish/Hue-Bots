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

		public int Count { get; set; }

		public BotChoice(int x, int y, int color, int count) : base()
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
				if (Selection == this.Color && this.Count > 0)
				{
					var spawners = from Actor s in MainGame.actors
								   where s is Spawner
								   select s as Spawner;
					foreach (Spawner s in spawners)
					{
						if (s.Box.Contains((int)(curMouse.X / MainGame.SCREEN_SCALE), (int)(curMouse.Y / MainGame.SCREEN_SCALE)))
						{
							MainGame.actors.Add(new Bot(s.X, s.Y, Selection, true));
							Count--;
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
			sb.Draw(MainGame.tex_bots[Color], Position, this.Count > 0 ? Microsoft.Xna.Framework.Color.White : new Microsoft.Xna.Framework.Color(255, 255, 255, 50));
			sb.DrawString(MainGame.fnt_font, Count.ToString(), Position + Vector2.UnitX * 75, Microsoft.Xna.Framework.Color.Black);
		}
	}
}
