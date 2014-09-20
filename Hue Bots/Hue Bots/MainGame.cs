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

		public static Color[] COLORS = new Color[] { Color.DarkGray, Color.Blue, Color.Yellow, Color.Lime, Color.Red, Color.Purple, Color.Orange, Color.White };
		public static Texture2D[] tex_bots = new Texture2D[8];

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static Texture2D tex_blank, tex_wall, tex_selected, tex_door, tex_spawner;
		public static SpriteFont fnt_font;

		public static List<Actor> actors;
		public static List<Actor> removeActors;

		private static List<BotChoice> botChoices;
		private static List<WallChoice> wallChoices;

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

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			tex_blank = Content.Load<Texture2D>("blank");

			tex_wall = Content.Load<Texture2D>("wall");
			tex_selected = Content.Load<Texture2D>("selected");
            tex_door = Content.Load<Texture2D>("door");
			tex_spawner = Content.Load<Texture2D>("spawner");

			tex_bots[0] = Content.Load<Texture2D>("bots/black bot");
			tex_bots[1] = Content.Load<Texture2D>("bots/blue bot");
			tex_bots[2] = Content.Load<Texture2D>("bots/yellow bot");
			tex_bots[3] = Content.Load<Texture2D>("bots/green bot");
			tex_bots[4] = Content.Load<Texture2D>("bots/red bot");
			tex_bots[5] = Content.Load<Texture2D>("bots/purple bot");
			tex_bots[6] = Content.Load<Texture2D>("bots/orange bot");
			tex_bots[7] = Content.Load<Texture2D>("bots/white bot");

			fnt_font = Content.Load<SpriteFont>("font");

			LoadLevel("test level");

			//// Testing ////
			// for (int i = 0; i < 1088; i += 64)
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
						;// Color changer
					else if (line[x][0] == 'm')
						;// Color mixer
					else if (line[x][0] == 'f')
						;// Finish
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
		}

		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				this.Exit();

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

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();
			foreach (Actor a in actors)
				a.Draw(spriteBatch);
			spriteBatch.Draw(tex_blank, new Rectangle(1088, 0, 320, 896), Color.LightGray);
			foreach (Actor a in botChoices)
				a.Draw(spriteBatch);
			foreach (Actor a in wallChoices)
				a.Draw(spriteBatch);
			if (BotChoice.Selection != -1)
				spriteBatch.Draw(tex_bots[BotChoice.Selection], new Vector2(Mouse.GetState().X - 32, Mouse.GetState().Y - 32), Color.White);
			if (WallChoice.Selection != -1)
				spriteBatch.Draw(tex_wall, new Vector2((float)Math.Round((float)(Mouse.GetState().X - 32) / 64) * 64, (float)Math.Round((float)(Mouse.GetState().Y - 32) / 64) * 64), COLORS[WallChoice.Selection]);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
