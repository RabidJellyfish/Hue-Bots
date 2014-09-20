using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hue_Bots
{
	public class Finish : Actor
	{
		public int finishAnimation = -1;

		public Finish(int x, int y) : base()
		{
			this.X = x;
			this.Y = y;
		}

		public override void Update()
		{
			if (finishAnimation >= 0)
			{
				finishAnimation--;
				if (finishAnimation == 0)
				{
					MainGame.currentLevel = "level " + (int.Parse(MainGame.currentLevel.Split(' ')[1]) + 1);
					MainGame.changeLevel = true;
				}
			}
		}

		public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_finish, Position, Microsoft.Xna.Framework.Color.White);
		}
	}
}
