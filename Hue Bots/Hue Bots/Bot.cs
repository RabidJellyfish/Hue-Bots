using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hue_Bots
{
	class Bot : Actor
	{
		private bool selected, canMove;
		private MouseState prevMouse, curMouse;
		private KeyboardState prevKey, curKey;

		public Bot(int x, int y, int color) : base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
			selected = false;
			canMove = true;
		}

		public override void Update()
		{
			curMouse = Mouse.GetState();
			curKey = Keyboard.GetState();

			if (curMouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
			{
				if (Box.Contains(curMouse.X, curMouse.Y))
					this.selected = true;
				else
					this.selected = false;
			}

			if (selected && canMove)
			{
				if (curKey.IsKeyDown(Keys.W) || curKey.IsKeyDown(Keys.Up))
				{
					Velocity = Vector2.Zero;
					Velocity.Y = -8;
					canMove = false;
				}
				else if (curKey.IsKeyDown(Keys.S) || curKey.IsKeyDown(Keys.Down))
				{
					Velocity = Vector2.Zero;
					Velocity.Y = 8;
					canMove = false;
				}
				else if (curKey.IsKeyDown(Keys.A) || curKey.IsKeyDown(Keys.Left))
				{
					Velocity = Vector2.Zero;
					Velocity.X = -8;
					canMove = false;
				}
				else if (curKey.IsKeyDown(Keys.D) || curKey.IsKeyDown(Keys.Right))
				{
					Velocity = Vector2.Zero;
					Velocity.X = 8;
					canMove = false;
				}
			}

			Position += Velocity;

			foreach (Static s in MainGame.statics)
			{
				if (this.Box.Intersects(s.Box) && ColorMatches(s.Color))
				{
					if (s is Wall)
					{
						Velocity = Vector2.Zero;
						Snap();
						canMove = true;
					}
				}
			}

			prevMouse = Mouse.GetState();
			prevKey = Keyboard.GetState();
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_bot, Position, MainGame.COLORS[Color]);
			if (selected)
				sb.Draw(MainGame.tex_selected, Position, Microsoft.Xna.Framework.Color.White);
		}
	}
}
