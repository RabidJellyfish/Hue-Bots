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

		public Bot(int x, int y, int color, bool selected) : base()
		{
			this.X = x;
			this.Y = y;
			this.Color = color;
			this.selected = selected;
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
				{
					if (curMouse.X < MainGame.SCREEN_WIDTH - 180)
						this.selected = false;
				}
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

			foreach (Actor s in MainGame.actors)
			{
				if (this.Box.Intersects(s.Box))
				{
					if ((s is Wall) && ColorMatches(s.Color) || (s is Door))
					{
						Stop();
						if (s is Door && ColorMatches(s.Color))
						{
							MainGame.removeActors.Add(s);
							MainGame.removeActors.Add(this);
						}
					}
					else if (s is Bot)
					{
						if (((Bot)s).Velocity.Length() > 0)
						{
							((Bot)s).Velocity *= -1;
							this.Velocity *= -1;
						}
						else
							Stop();
					}
					else if (s is ColorChanger)
					{
						this.Color = s.Color;
					}
					else if ((s is Arrow) && ColorMatches(s.Color))
					{
						if ((this.Position - s.Position).Length() < 8)
						{
							Snap();
							this.Velocity = ((Arrow)s).direction * 8;
						}
					}
					else if ((s is Finish) && this.Color == 7)
					{
						if ((this.Position - s.Position).Length() < 16)
						{
							Snap();
							this.Velocity = Vector2.Zero;
							if (((Finish)s).finishAnimation < 0)
								((Finish)s).finishAnimation = 120;
						}
					}
				}
			}

			prevMouse = Mouse.GetState();
			prevKey = Keyboard.GetState();
		}

		private void Stop()
		{
			Velocity = Vector2.Zero;
			Snap();
			canMove = true;
		}

		public override void Draw(SpriteBatch sb)
		{
			sb.Draw(MainGame.tex_bots[Color], Position, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
			if (selected)
				sb.Draw(MainGame.tex_selected, Position, null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
		}
	}
}
