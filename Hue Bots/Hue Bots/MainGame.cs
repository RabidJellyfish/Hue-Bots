using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Hue_Bots
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class MainGame : Microsoft.Xna.Framework.Game
	{
		public const int SCREEN_WIDTH = 1408;
		public const int SCREEN_HEIGHT = 896;

		public const bool EDIT_MODE = false;
		private char currentChar = 'w';
		private int currentColor = 0;
		private static int levelID = 123;

		public static Color[] COLORS = new Color[] { Color.DarkGray, Color.Blue, Color.Yellow, Color.Lime, Color.Red, Color.Purple, Color.Orange, Color.White };
		public static Texture2D[] tex_bots = new Texture2D[8];
		public static Texture2D[] tex_doors = new Texture2D[8];

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static Texture2D tex_helpArrow, tex_blank, tex_wall, tex_selected, tex_spawner, tex_finish, tex_changer, tex_arrow, tex_bg;
		public static SpriteFont fnt_font;
		public static Texture2D[] tex_winBots = new Texture2D[34];
		public static AnimatedTexture anim_winBot;

		public static List<Actor> actors;
		public static List<Actor> removeActors;

		private static List<BotChoice> botChoices;
		private static List<WallChoice> wallChoices;

		public static string currentLevel;
		public static bool changeLevel = false;

		KeyboardState curKeys, prevKeys;

		public static Song music;

		public MainGame()
		{
			actors = new List<Actor>();
			removeActors = new List<Actor>();
			botChoices = new List<BotChoice>();
			wallChoices = new List<WallChoice>();

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
			graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
			graphics.IsFullScreen = false;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			IsMouseVisible = true;
			curKeys = Keyboard.GetState();
			prevKeys = curKeys;
			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			tex_blank = Content.Load<Texture2D>("blank");
			tex_helpArrow = Content.Load<Texture2D>("help arrow");

			tex_wall = Content.Load<Texture2D>("wall");
			tex_selected = Content.Load<Texture2D>("selected");
			tex_spawner = Content.Load<Texture2D>("spawner");
			tex_finish = Content.Load<Texture2D>("finish");
			tex_changer = Content.Load<Texture2D>("changer 2");
			tex_arrow = Content.Load<Texture2D>("arrow");

			tex_bots[0] = Content.Load<Texture2D>("bots/black bot");
			tex_bots[1] = Content.Load<Texture2D>("bots/blue bot");
			tex_bots[2] = Content.Load<Texture2D>("bots/yellow bot");
			tex_bots[3] = Content.Load<Texture2D>("bots/green bot");
			tex_bots[4] = Content.Load<Texture2D>("bots/red bot");
			tex_bots[5] = Content.Load<Texture2D>("bots/purple bot");
			tex_bots[6] = Content.Load<Texture2D>("bots/orange bot");
			tex_bots[7] = Content.Load<Texture2D>("bots/white bot");

			tex_doors[0] = Content.Load<Texture2D>("doors/black door");
			tex_doors[1] = Content.Load<Texture2D>("doors/blue door");
			tex_doors[2] = Content.Load<Texture2D>("doors/yellow door");
			tex_doors[3] = Content.Load<Texture2D>("doors/green door");
			tex_doors[4] = Content.Load<Texture2D>("doors/red door");
			tex_doors[5] = Content.Load<Texture2D>("doors/purple door");
			tex_doors[6] = Content.Load<Texture2D>("doors/orange door");
			tex_doors[7] = Content.Load<Texture2D>("doors/white door");

			for (int i = 0; i < tex_winBots.Length; i++)
				tex_winBots[i] = Content.Load<Texture2D>("win bot/white bot animation" + (i < 10 ? "0" : "") + i.ToString());
			anim_winBot = new AnimatedTexture(tex_winBots, 2, false, true);

			tex_bg = Content.Load<Texture2D>("bg");
			music = Content.Load<Song>("hue bots music");

			fnt_font = Content.Load<SpriteFont>("font");

			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(music);

			currentLevel = "level 7";
			LoadLevel(currentLevel);

			//// Testing ////
			// for (int i = 0; i < 1088; i += 64
			// {
			// 	actors.Add(new Wall(i, 0, 0));
			// 	actors.Add(new Wall(i, graphics.PreferredBackBufferHeight - 64, 0));
			// }
			// for (int i = 0; i < 896; i += 64)
			// {
			// 	actors.Add(new Wall(0, i, 0));
			// 	actors.Add(new Wall(graphics.PreferredBackBufferWidth - 384, i, 0));
			// }
			// actors.Add(new Door(320, 64, 4));
			// actors.Add(new Spawner(640, 640));
			////////
		}

		public static void LoadLevel(string name)
		{
			anim_winBot.Frame = 0;
			anim_winBot.IsPlaying = false;
			actors.Clear();
			removeActors.Clear();
			botChoices.Clear();
			wallChoices.Clear();
			if (File.Exists("Content/levels/" + name + ".txt"))
			{
				StreamReader reader = new StreamReader("Content/levels/" + name + ".txt");
				for (int y = 0; y < 14; y++)
				{
					string[] line = reader.ReadLine().Split(' ');
					for (int x = 0; x < 17; x++)
					{
						if (line[x][0] == 'w')
							actors.Add(new Wall(x * Actor.WIDTH, y * Actor.WIDTH, (int)char.GetNumericValue(line[x][1])));
						else if (line[x][0] == 's')
							actors.Add(new Spawner(x * Actor.WIDTH, y * Actor.WIDTH));
						else if (line[x][0] == 'd')
							actors.Add(new Door(x * Actor.WIDTH, y * Actor.WIDTH, (int)char.GetNumericValue(line[x][1])));
						else if (line[x][0] == 'c')
							actors.Add(new ColorChanger(x * Actor.WIDTH, y * Actor.WIDTH, (int)char.GetNumericValue(line[x][1])));
						else if (line[x][0] == 'm')
							;// Color mixer
						else if (line[x][0] == 'f')
							actors.Add(new Finish(x * Actor.WIDTH, y * Actor.WIDTH));
						else if (line[x][0] == '<' || line[x][0] == '>' || line[x][0] == '^' || line[x][0] == 'v')
						{
							int dirX = (line[x][0] == '<' ? -1 : (line[x][0] == '>' ? 1 : 0));
							int dirY = (line[x][0] == '^' ? -1 : (line[x][0] == 'v' ? 1 : 0));
							actors.Add(new Arrow(x * Actor.WIDTH, y * Actor.WIDTH, (int)char.GetNumericValue(line[x][1]),
											new Vector2(dirX, dirY)));
						}
					}
				}
				reader.ReadLine();
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 96, 4, int.Parse(reader.ReadLine().Split('=')[1])));
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 196, 6, int.Parse(reader.ReadLine().Split('=')[1])));
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 296, 2, int.Parse(reader.ReadLine().Split('=')[1])));
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 396, 3, int.Parse(reader.ReadLine().Split('=')[1])));
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 496, 1, int.Parse(reader.ReadLine().Split('=')[1])));
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 596, 5, int.Parse(reader.ReadLine().Split('=')[1])));
				botChoices.Add(new BotChoice(SCREEN_WIDTH - 300, 696, 7, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 96, 4, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 196, 6, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 296, 2, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 396, 3, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 496, 1, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 596, 5, int.Parse(reader.ReadLine().Split('=')[1])));
				wallChoices.Add(new WallChoice(SCREEN_WIDTH - 150, 696, 0, int.Parse(reader.ReadLine().Split('=')[1])));
				reader.Close();
				reader.Dispose();
			}
		}

		public void SaveLevel()
		{
			StreamWriter writer = new StreamWriter(levelID + ".txt");
			string[,] matrix = new string[14, 17];
			foreach (Actor a in actors)
			{
				int y = (int)a.Position.Y / 64;
				int x = (int)a.Position.X / 64;
				if (x >= 0 && x < 17 && y >= 0 && y < 14)
				{
					if (a is Wall)
						matrix[y, x] = "w" + a.Color;
					if (a is Spawner)
						matrix[y, x] = "s0";
					if (a is Door)
						matrix[y, x] = "d" + a.Color;
					if (a is ColorChanger)
						matrix[y, x] = "c" + a.Color;
					if (a is Finish)
						matrix[y, x] = "f0";
					if (a is Arrow && ((Arrow)a).direction.X == 1)
						matrix[y, x] = ">" + a.Color;
					if (a is Arrow && ((Arrow)a).direction.X == -1)
						matrix[y, x] = "<" + a.Color;
					if (a is Arrow && ((Arrow)a).direction.Y == -1)
						matrix[y, x] = "^" + a.Color;
					if (a is Arrow && ((Arrow)a).direction.Y == 1)
						matrix[y, x] = "v" + a.Color;
				}
			}
			for (int y = 0; y < 14; y++)
			{
				for (int x = 0; x < 17; x++)
					writer.Write((matrix[y, x] ?? ".0") + " ");
				writer.WriteLine();
			}
			writer.WriteLine();
			writer.WriteLine("redbots=0");
			writer.WriteLine("orangebots=0");
			writer.WriteLine("yellowbots=0");
			writer.WriteLine("greenbots=0");
			writer.WriteLine("bluebots=0");
			writer.WriteLine("purplebots=0");
			writer.WriteLine("whitebots=0");
			writer.WriteLine("redwalls=0");
			writer.WriteLine("orangewalls=0");
			writer.WriteLine("yellowwalls=0");
			writer.WriteLine("greenwalls=0");
			writer.WriteLine("bluewalls=0");
			writer.WriteLine("purplewalls=0");
			writer.WriteLine("graywalls=0");
			writer.Flush();
			writer.Close();
			writer.Dispose();
		}

		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		private bool rPressed = true;
		protected override void Update(GameTime gameTime)
		{
			curKeys = Keyboard.GetState();

			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

			if (EDIT_MODE)
			{
				#region Edit mode

				if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				{
					Vector2 pos = new Vector2((int)Math.Round((float)(Mouse.GetState().X - 32) / 64) * 64, (int)Math.Round((float)(Mouse.GetState().Y - 32) / 64) * 64);
					int x = (int)pos.X;
					int y = (int)pos.Y;
					foreach (Actor a in actors)
						if (a.Position == pos)
							removeActors.Add(a);
					switch (currentChar)
					{
						case 'w':
							actors.Add(new Wall(x, y, currentColor));
							break;
						case 's':
							actors.Add(new Spawner(x, y));
							break;
						case 'd':
							actors.Add(new Door(x, y, currentColor));
							break;
						case 'c':
							actors.Add(new ColorChanger(x, y, currentColor));
							break;
						case 'f':
							actors.Add(new Finish(x, y));
							break;
						case '^':
							actors.Add(new Arrow(x, y, currentColor, Vector2.UnitY * -1));
							break;
						case 'v':
							actors.Add(new Arrow(x, y, currentColor, Vector2.UnitY));
							break;
						case '<':
							actors.Add(new Arrow(x, y, currentColor, Vector2.UnitX * -1));
							break;
						case '>':
							actors.Add(new Arrow(x, y, currentColor, Vector2.UnitX));
							break;
					}
				}

				if (Mouse.GetState().RightButton == ButtonState.Pressed)
				{
					foreach (Actor a in actors)
					{
						if (a.Box.Contains(Mouse.GetState().X, Mouse.GetState().Y) && !removeActors.Contains(a))
							removeActors.Add(a);
					}
				}

				if (curKeys.IsKeyDown(Keys.W) && prevKeys.IsKeyUp(Keys.W))
				{
					if (currentChar == 'w')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = 'w';
				}
				if (curKeys.IsKeyDown(Keys.S) && prevKeys.IsKeyUp(Keys.S) && curKeys.IsKeyUp(Keys.LeftControl))
				{
					currentChar = 's';
				}
				if (curKeys.IsKeyDown(Keys.D) && prevKeys.IsKeyUp(Keys.D))
				{
					if (currentChar == 'd')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = 'd';
				}
				if (curKeys.IsKeyDown(Keys.C) && prevKeys.IsKeyUp(Keys.C))
				{
					if (currentChar == 'c')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = 'c';
				}
				if (curKeys.IsKeyDown(Keys.F) && prevKeys.IsKeyUp(Keys.F))
				{
					currentChar = 'f';
				}
				if (curKeys.IsKeyDown(Keys.Left) && prevKeys.IsKeyUp(Keys.Left))
				{
					if (currentChar == '<')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = '<';
				}
				if (curKeys.IsKeyDown(Keys.Right) && prevKeys.IsKeyUp(Keys.Right))
				{
					if (currentChar == '>')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = '>';
				}
				if (curKeys.IsKeyDown(Keys.Up) && prevKeys.IsKeyUp(Keys.Up))
				{
					if (currentChar == '^')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = '^';
				}
				if (curKeys.IsKeyDown(Keys.Down) && prevKeys.IsKeyUp(Keys.Down))
				{
					if (currentChar == 'v')
						currentColor = currentColor == 7 ? 0 : currentColor + 1;
					currentChar = 'v';
				}

				if (curKeys.IsKeyDown(Keys.LeftControl))
				{
					if (curKeys.IsKeyDown(Keys.S) && curKeys.IsKeyUp(Keys.Up))
						SaveLevel();
				}

				#endregion
			}
			else
			{
				if (Keyboard.GetState().IsKeyDown(Keys.R))
				{
					if (!rPressed)
					{
						rPressed = true;
						LoadLevel(currentLevel);
					}
				}
				else
					rPressed = false;
			}

			anim_winBot.Update();
			BotChoice.FoundSpawner = false;
			foreach (Actor a in botChoices)
				a.Update();
			if (Mouse.GetState().LeftButton == ButtonState.Released && !BotChoice.FoundSpawner && BotChoice.Selection != -1)
				BotChoice.Selection = -1;
			foreach (Actor a in wallChoices)
				a.Update();

			foreach (Actor a in actors)
				a.Update();

			foreach (Actor a in removeActors)
				actors.Remove(a);
			removeActors.Clear();

			if (changeLevel)
			{
				LoadLevel(currentLevel);
				changeLevel = false;
			}

			prevKeys = curKeys;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			spriteBatch.Draw(tex_bg, Vector2.Zero, Color.White);

			foreach (Actor a in actors)
				if (!(a is Bot))
					a.Draw(spriteBatch);
			var bots = from Actor a in actors
					   where a is Bot
					   select a as Bot;
			foreach (Bot b in bots)
				b.Draw(spriteBatch);

			spriteBatch.Draw(tex_blank, new Rectangle(1088, 0, 320, 896), Color.LightGray);
			foreach (Actor a in botChoices)
				a.Draw(spriteBatch);
			foreach (Actor a in wallChoices)
				a.Draw(spriteBatch);

			if (BotChoice.Selection != -1)
				spriteBatch.Draw(tex_bots[BotChoice.Selection], new Vector2(Mouse.GetState().X - 32, Mouse.GetState().Y - 32), Color.White);
			if (WallChoice.Selection != -1)
				spriteBatch.Draw(tex_wall, new Vector2((float)Math.Round((float)(Mouse.GetState().X - 32) / 64) * 64, (float)Math.Round((float)(Mouse.GetState().Y - 32) / 64) * 64), COLORS[WallChoice.Selection]);

			if (EDIT_MODE)
			{
				Vector2 pos = new Vector2(Mouse.GetState().X - 32, Mouse.GetState().Y - 32);
				
				switch (currentChar)
				{
					case 'w':
						spriteBatch.Draw(tex_wall, pos, COLORS[currentColor]);
						break;
					case 's':
						spriteBatch.Draw(tex_spawner, pos, Color.White);
						break;
					case 'd':
						spriteBatch.Draw(tex_doors[currentColor], pos, Color.White);
						break;
					case 'c':
						spriteBatch.Draw(tex_changer, pos, COLORS[currentColor]);
						break;
					case 'f':
						spriteBatch.Draw(tex_finish, pos, Color.White);
						break;
					case '^':
						spriteBatch.Draw(tex_arrow, pos + Vector2.One * 32, null, COLORS[currentColor], MathHelper.PiOver2 * 3, Vector2.One * 32, 1f, SpriteEffects.None, 1f);
						break;
					case 'v':
						spriteBatch.Draw(tex_arrow, pos + Vector2.One * 32, null, COLORS[currentColor], MathHelper.PiOver2, Vector2.One * 32, 1f, SpriteEffects.None, 1f);
						break;
					case '<':
						spriteBatch.Draw(tex_arrow, pos + Vector2.One * 32, null, COLORS[currentColor], MathHelper.Pi, Vector2.One * 32, 1f, SpriteEffects.None, 1f);
						break;
					case '>':
						spriteBatch.Draw(tex_arrow, pos + Vector2.One * 32, null, COLORS[currentColor], 0, Vector2.One * 32, 1f, SpriteEffects.None, 1f);
						break;
				}
			}

			#region draw help

			if (currentLevel == "level 0")
			{
				spriteBatch.DrawString(fnt_font, "Click/drag robots to robot builder to make a robot\n" +
												 "Click built robot to select it, use WASD to move", new Vector2(100, 600), Color.White);
				if (botChoices[6].Count > 0)
				{
					spriteBatch.Draw(tex_helpArrow, new Vector2(280, 220), null, Color.White, MathHelper.PiOver2, new Vector2(96, 32), 1f, SpriteEffects.None, 0f);
					spriteBatch.Draw(tex_helpArrow, new Vector2(1000, 730), null, Color.White, 0f, new Vector2(96, 32), 1f, SpriteEffects.None, 0f);
				}
				else
				{
					spriteBatch.Draw(tex_helpArrow, new Vector2(740, 220), null, Color.White, MathHelper.PiOver2, new Vector2(96, 32), 1f, SpriteEffects.None, 0f);
				}
			}
			else if (currentLevel == "level 2")
			{
				spriteBatch.DrawString(fnt_font, "Click/drag walls\n"+
												 "to create them.\n\n" +
												 "Press <R> to restart\n" +
												 "level", new Vector2(50, 60), Color.White);
			}

			#endregion

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
