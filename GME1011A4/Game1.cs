using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GME1011A4
/* This is my last assignment for this course. For this, I will reuse A5 from GME1003.
 * 
 * The main idea of this game is to collect 5 boxes
 * while dodging the metal boxes. You just have 3 lives, so be careful.
 * 
 * Move to left using "A", or right using "D".
 * 
 * GME1011_A4_BastV
 */
{
    public class Game1 : Game
    {
        //Declaring my variables.
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _plW, _plL, _background, _glitter;
        private SpriteFont _font;
        private float _glitterRotation;
        private Random _RNG;
        private int _numGlitter, _numBox, _boxRNG, _counter;
        private List<int> _glitterX;
        private List<int> _glitterY;

        //Declaring my new objects.
        private Player _player;
        private List<Box> _boxes;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            //Changing the size of the window to be the same as the background that I drew.
            _graphics.PreferredBackBufferWidth = 1080;
            _graphics.PreferredBackBufferHeight = 1080;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Initializing a random number for glitter and number of boxes.
            _RNG = new Random();

            //List for glitter.
            _numGlitter = _RNG.Next(4, 20);
            _glitterRotation = _RNG.Next(0, 100) / 50f;
            _glitterX = new List<int>();
            _glitterY = new List<int>();
            

            //List for both coordinates of glitter.
            for (int i = 0; i < _numGlitter; i++)
            {
                _glitterX.Add(_RNG.Next(0, 1030));
                _glitterY.Add(_RNG.Next(0, 600));
            }

            //List of boxes.
            _boxes = new List<Box>();
            _numBox = _RNG.Next(3,6);

            for (int b = 0; b < _numBox; b++)
            {
                _boxRNG = _RNG.Next(0, 3);

                if (_boxRNG == 2)
                    _boxes.Add(new woodenBox(Content.Load<Texture2D>("Box"), new Vector2(_RNG.Next(0, 981), -100)));
                else
                    _boxes.Add(new metalBox(Content.Load<Texture2D>("Death_Box"), new Vector2(_RNG.Next(0, 981), -100)));
            }

            //counter to change the amount of boxes later.
            _counter = 0;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loading the sprites for background, win or lose, glitter, and font.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _plW = Content.Load<Texture2D>("Player_Win");
            _plL = Content.Load<Texture2D>("Player_Lose");
            _background = Content.Load<Texture2D>("Background");
            _glitter = Content.Load<Texture2D>("Glitter");
            _font = Content.Load<SpriteFont>("Font");

            //Loading my player with its three sprites.
            _player = new Player();
            _player.LoadContent(Content);

        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            //Rotation for glitter.
            _glitterRotation += 0.009f;

            //creating my keyboard state for update ONLY if you are alive and not winning yet. Required for updating my player.
            KeyboardState keyboardState = Keyboard.GetState();

            //Check for hitbox only if you are still alive and playing.
            if (_player._playerWin == false && _player._playerDead == false)
            {
                _player.Update(gameTime, keyboardState);

                foreach (Box box in _boxes)
                {
                    box.Update(gameTime);

                    if (box.GetHitbox().Intersects(_player.GetHitbox()))
                        box.Collides(_player);
                }
                //Code to remove broken boxes.
                for (int i = 0; i < _boxes.Count; i++)
                {
                    if (_boxes[i].GetBroken())
                    {
                        _boxes.RemoveAt(i);
                    }
                }
                
                //Counter to change the amount of boxes on the list.
                _counter++;
                if (_counter == 180)
                {
                    _numBox = _RNG.Next(3, 6);
                    _boxRNG = _RNG.Next(0, 3);

                    //List added again to modify the amount and type of boxes falling later.
                    for (int b = 0; b < _numBox; b++)
                    {
                        _boxRNG = _RNG.Next(0, 3);

                        if (_boxRNG == 2)
                            _boxes.Add(new woodenBox(Content.Load<Texture2D>("Box"), new Vector2(_RNG.Next(0, 981), -100)));
                        else
                            _boxes.Add(new metalBox(Content.Load<Texture2D>("Death_Box"), new Vector2(_RNG.Next(0, 981), -100)));
                    }

                    _counter = 0;
                }
                
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            //If statement that only runs if the player is not dead, and it has not won yet.
            //Draws the background, player, boxes, glitter, Lives and Score.
            if(!_player._playerDead && !_player._playerWin)
            {
                _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
                _player.Draw(_spriteBatch, Keyboard.GetState());
                _spriteBatch.DrawString(_font, "Lives: " + _player._Lives, new Vector2(80, 50), Color.White);
                _spriteBatch.DrawString(_font, "Score: " + _player._Score, new Vector2(900, 50), Color.White);

                for(int i = 0; i < _numGlitter; i++)
                {
                    _spriteBatch.Draw(_glitter, new Vector2(_glitterX[i], _glitterY[i]), null, Color.White, _glitterRotation, 
                        new Vector2(_glitter.Width / 2, _glitter.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0f);
                }

                foreach (Box box in _boxes)
                {
                    box.Draw(_spriteBatch);
                }

            }

            //If the player is dead, it changes the background, and it prints a string in the middle of the screen.
            if(_player._playerDead == true)
            {
                _spriteBatch.Draw(_plL, new Vector2(0, 0), Color.Gray);
                string gameOver = "Game over. Press ESC to exit.";
                int screenCenterLX = _graphics.PreferredBackBufferWidth / 2;
                int screenCenterLY = _graphics.PreferredBackBufferHeight / 2;
                int textHalfWidthL = (int)_font.MeasureString(gameOver).X / 2;
                int textHalfHeightL = (int)_font.MeasureString(gameOver).Y / 2;
                _spriteBatch.DrawString(_font, gameOver, new Vector2(screenCenterLX - textHalfWidthL, screenCenterLY - textHalfHeightL), Color.White);
            }

            //If the player has won, it changes the background, and it prints a string in the middle of the screen.
            //The integers have a W to differentiate themselves with the game over version.
            if (_player._playerWin == true)
            {
                _spriteBatch.Draw(_plW, new Vector2(0, 0), Color.PapayaWhip);
                string win = "You win! Press ESC to exit.";
                int screenCenterWX = _graphics.PreferredBackBufferWidth / 2;
                int screenCenterWY = _graphics.PreferredBackBufferHeight / 2;
                int textHalfWidthW = (int)_font.MeasureString(win).X / 2;
                int textHalfHeightW = (int)_font.MeasureString(win).Y / 2;
                _spriteBatch.DrawString(_font, win, new Vector2(screenCenterWX - textHalfWidthW, screenCenterWY - textHalfHeightW), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
