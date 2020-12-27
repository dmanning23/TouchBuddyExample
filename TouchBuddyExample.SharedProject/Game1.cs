using FontBuddyLib;
using GameTimer;
using InputHelper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Linq;
using TouchScreenBuddy;

namespace TouchBuddyExample
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		#region Properties

		GraphicsDeviceManager _graphics;
		SpriteBatch _spriteBatch;

		IFontBuddy _font;
		IInputHelper _input;
		GameClock _time;
		float lineSpace;

		#endregion //Properties

		#region Methods

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			var touch = new TouchComponent(this, null);
			touch.SupportedGestures = GestureType.Tap | GestureType.Pinch | GestureType.PinchComplete | GestureType.DoubleTap | GestureType.Flick;
			_input = touch;

			var debug = new DebugInputComponent(this, null);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			_spriteBatch = new SpriteBatch(_graphics.GraphicsDevice);

			_font = new FontBuddy();
			_font.LoadContent(Content, "ArialBlack24");
			lineSpace = _font.MeasureString("Yy").Y;

			_time = new GameClock();
			_time.Start();
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

			_time.Update(gameTime);

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			_spriteBatch.Begin();

			//write the stuff
			var pos = Vector2.Zero;
			_font.Write(string.Format("Highlights: {0}", _input.Highlights.Count), pos, Justify.Left, 1f, Color.White, _spriteBatch, _time);
			pos.Y += lineSpace;

			_font.Write(string.Format("Clicks: {0}", _input.Clicks.Count), pos, Justify.Left, 1f, Color.White, _spriteBatch, _time);
			pos.Y += lineSpace;

			_font.Write(string.Format("Drags: {0}", _input.Drags.Count), pos, Justify.Left, 1f, Color.White, _spriteBatch, _time);
			pos.Y += lineSpace;

			_font.Write(string.Format("Drops: {0}", _input.Drops.Count), pos, Justify.Left, 1f, Color.White, _spriteBatch, _time);
			pos.Y += lineSpace;

			if (_input.Pinches.Count > 0)
			{
				var pinch = _input.Pinches.First();
				_font.Write(string.Format("Pinch: {0}", pinch.Delta.ToString()), pos, Justify.Left, 1f, Color.White, _spriteBatch, _time);
				pos.Y += lineSpace;

				_font.Write(pinch.Delta < 0f ? "Zoom out" : "Zoom in", pos, Justify.Left, 1f, Color.White, _spriteBatch, _time);
				pos.Y += lineSpace;
			}

			_spriteBatch.End();

			base.Draw(gameTime);
		}

		#endregion //Methods
	}
}
