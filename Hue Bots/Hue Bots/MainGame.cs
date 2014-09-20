using System;
using System.Collections.Generic;
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
		public static Color[] COLORS = new Color[] { Color.DarkGray, Color.Blue, Color.Yellow, Color.Lime, Color.Red, Color.Purple, Color.Orange, Color.White };

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static Texture2D tex_blank, tex_bot, tex_wall, tex_selected;

		public static List<Actor> actors;
		public static List<Actor> removeActors;

		public MainGame()
		{
			actors = new List<Actor>();
			removeActors = new List<Actor>();

			graphics = new GraphicsDeviceManager(this);
			graphics.PreferredBackBufferHeight = 896;
			graphics.PreferredBackBufferWidth = 1408;
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
			tex_bot = Content.Load<Texture2D>("bot");
			tex_wall = Content.Load<Texture2D>("wall");
			tex_selected = Content.Load<Texture2D>("selected");

			actors.Add(new Bot(64, 64, 1));
			for (int i = 0; i < 1088; i += 64)
			{
				actors.Add(new Wall(i, 0, 0));
				actors.Add(new Wall(i, graphics.PreferredBackBufferHeight - 64, 0));
			}
			for (int i = 0; i < 896; i += 64)
			{
				actors.Add(new Wall(0, i, 0));
				actors.Add(new Wall(graphics.PreferredBackBufferWidth - 384, i, 0));
			}
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
			spriteBatch.Draw(tex_blank, new Rectangle(1088, 0, 320, 896), Color.LightGray);
			foreach (Actor a in actors)
				a.Draw(spriteBatch);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
