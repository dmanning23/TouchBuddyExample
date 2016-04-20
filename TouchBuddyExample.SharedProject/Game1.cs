using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TouchScreenBuddy;
using PrimitiveBuddy;
using System.Collections.Generic;
using InputHelper;
using System.Linq;

namespace TouchBuddyExample
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		#region Properties

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		IInputHelper input
		{
			get; set;
		}

		Primitive Prim
		{
			get; set;
		}

		List<HighlightEventArgs> ButtonPresses
		{
			get; set;
		}

		List<ClickEventArgs> Clicks
		{
			get; set;
		}

		DragEventArgs Drag
		{
			get; set;
		}

		List<DragEventArgs> Drags
		{
			get; set;
		}

		List<DropEventArgs> Drops { get; set; }

		#endregion //Properties

		#region Methods

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			ButtonPresses = new List<HighlightEventArgs>();
			Clicks = new List<ClickEventArgs>();
			Drops = new List<DropEventArgs>();
			Drags = new List<DragEventArgs>();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			input = new TouchComponent(this);

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Prim = new Primitive(GraphicsDevice, spriteBatch);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				//Exit();
			}

			Drag = null;

			ButtonPresses.AddRange(input.Highlights);
			Clicks.AddRange(input.Clicks);
			Drags.AddRange(input.Drags);
			Drag = input.Drags.FirstOrDefault();
			Drops.AddRange(input.Drops);

			while (ButtonPresses.Count > 5)
			{
				ButtonPresses.RemoveAt(0);
            }

			while (Clicks.Count > 5)
			{
				Clicks.RemoveAt(0);
			}

			while (Drops.Count > 5)
			{
				Drops.RemoveAt(0);
			}

			while (Drags.Count > 30)
			{
				Drags.RemoveAt(0);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			//darw the mouse cursor
			Prim.Thickness = 1;
			//Prim.Circle(Mouse.MousePos, 5, Color.White);

			//draw the button down events
			Prim.Thickness = 2;

			foreach (var mouseEvent in ButtonPresses)
			{
				Prim.Circle(mouseEvent.Position, 10, Color.Yellow);
			}

			foreach (var mouseEvent in Drags)
			{
				Prim.Line(mouseEvent.Current, mouseEvent.Current - mouseEvent.Delta, Color.LimeGreen);
			}

			if (null != Drag)
			{
				Prim.Circle(Drag.Current, 10, Color.LimeGreen);
				Prim.Line(Drag.Start, Drag.Current, Color.LimeGreen);
			}

			foreach (var mouseEvent in Clicks)
			{
				Prim.Circle(mouseEvent.Position, 10, (mouseEvent.Button == MouseButton.Left) ? Color.Red : Color.DarkRed);
			}

			foreach (var mouseEvent in Drops)
			{
				Prim.Line(mouseEvent.Start, mouseEvent.Drop, Color.Green);
				Prim.Circle(mouseEvent.Drop, 10, Color.Green);
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion //Methods
	}
}
