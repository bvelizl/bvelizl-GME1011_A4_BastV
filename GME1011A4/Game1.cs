using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GME1011A4
/* This is my last assignment for this course.
 * The main idea of this game is to collect 5 boxes
 * while dodging the metal boxes.
 * Bastian Veliz_GME-1003.
 */
{
    public class Game1 : Game
    {
        //Declaring all my variables, including the ones for every sprite of my character & "enemy".
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _plIdle, _plLeft, _plRight, _plW, _plL, _box, _boxD, _background, _glitter;
        private SpriteFont _font;
        private float _plX, _plY, _plSpeed, _boxX, _boxY, _boxSpeed, _boxDX, _boxDY, _glitterRotation;
        private Color _plColor;
        private Random _RNG;
        private int _plLives, _plScore, count, _numGlitter;
        private List<int> _glitterX;
        private List<int> _glitterY;
        private bool _plDead, _plWin;

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
            //Assigning values for my variables.
            _plX = 540;
            _plY = 1080;
            _plSpeed = 600;
            _plColor = Color.White;
            _plLives = 3;
            _plScore = 0;
            _boxX = 600;
            _boxY = -100;
            _boxSpeed = 400;
            _boxDX = 0;
            _boxDY = -100;

            _plWin = false;
            _plDead = false;
            _RNG = new Random();

            //List for glitter.
            _numGlitter = _RNG.Next(4, 20);
            _glitterRotation = _RNG.Next(0, 100) / 50f;
            _glitterX = new List<int>();
            _glitterY = new List<int>();

            //List of both coordinates.
            for (int i = 0; i < _numGlitter; i++)
            {
                _glitterX.Add(_RNG.Next(0, 1030));
                _glitterY.Add(_RNG.Next(0, 600));
            }


            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Loading the sprites for character, boxes, background, and font.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _plIdle = Content.Load<Texture2D>("Player_Idle");
            _plLeft = Content.Load<Texture2D>("Player_Left");
            _plRight = Content.Load<Texture2D>("Player_Right");
            _plW = Content.Load<Texture2D>("Player_Win");
            _plL = Content.Load<Texture2D>("Player_Lose");
            _box = Content.Load<Texture2D>("Box");
            _boxD = Content.Load<Texture2D>("Death_Box");
            _background = Content.Load<Texture2D>("Background");
            _glitter = Content.Load<Texture2D>("Glitter");
            _font = Content.Load<SpriteFont>("Font");


        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //If statement to know if it is game over, or the player has won.
            if (_plLives <= 0)
                _plDead = true;

            if (_plScore >= 5)
                _plWin = true;

            if (!_plDead)
            {
                KeyboardState keyboardState = Keyboard.GetState();
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Assigning Keys for player's movement with delta time.
                if (keyboardState.IsKeyDown(Keys.A))
                    _plX -= _plSpeed * deltaTime;
                if (keyboardState.IsKeyDown(Keys.D))
                    _plX += _plSpeed * deltaTime;

                //Measuring my 3 sprites of player (Idle, Left, and Right sprite).
                _plX = MathHelper.Clamp(_plX, 0, _graphics.PreferredBackBufferWidth - _plIdle.Width);
                _plY = MathHelper.Clamp(_plY, 0, _graphics.PreferredBackBufferHeight - _plIdle.Height);
                _plX = MathHelper.Clamp(_plX, 0, _graphics.PreferredBackBufferWidth - _plLeft.Width);
                _plY = MathHelper.Clamp(_plY, 0, _graphics.PreferredBackBufferHeight - _plLeft.Height);
                _plX = MathHelper.Clamp(_plX, 0, _graphics.PreferredBackBufferWidth - _plRight.Width);
                _plY = MathHelper.Clamp(_plY, 0, _graphics.PreferredBackBufferHeight - _plRight.Height);

                //Creating hitbox rectangle for player, box, and boxD.
                Rectangle _plHitbox = new Rectangle((int)_plX, (int)_plY, _plIdle.Width, _plIdle.Height);
                Rectangle _boxHitbox = new Rectangle((int)_boxX, (int)_boxY, _box.Width, _box.Height);
                Rectangle _boxDHitbox = new Rectangle((int)_boxDX, (int)_boxDY, _boxD.Width, _boxD.Height);

                //Assigning speed with delta time for both boxes.
                _boxY += _boxSpeed * deltaTime;
                _boxDY += _boxSpeed * deltaTime;

                //2 if statements that returns the boxes to the top, outside of the screen.
                if (_boxY > 1100)
                    _boxY = -100;


                if (_boxDY > 1100)
                    _boxDY = -100;

                //If statement that increase player's score if it collides with a box. Returning box to the top.
                if (_plHitbox.Intersects(_boxHitbox))
                {
                    _plScore++;
                    _boxY = -200;
                }

                //If statement that decrease player's lves if it collides with a boxD. Returning boxD to the top. Changing player's colour randomly.
                if (_plHitbox.Intersects(_boxDHitbox))
                {
                    _plLives--;
                    _boxDY = -200;
                    _plColor = new Color(128 + _RNG.Next(1, 128), 128 + _RNG.Next(1, 128), 128 + _RNG.Next(1, 128));
                }

                //If statement that prevents two different boxes overlapping.
                if (_boxHitbox.Intersects(_boxDHitbox))
                    _boxY = -100;

                //Variable created to prevent boxes changing X position by every frame. Settled to 2 seconds to make boxes position changes while on-screen.
                //RNG applied to box X and boxD X position to change their position on-screen randomly.
                count++;
                if (count == 60*2)
                {
                    _boxX = _RNG.Next(0, 980);
                    _boxDX = _RNG.Next(0, 980);
                    count = 0;
                }
                _glitterRotation += 0.009f;



            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            //If statement that only runs if the player is not dead, and it has not won yet.
            //Draws the background, player(idle), boxes, Lives and Score.
            //2 if statements that change the sprite of the player, dependind on the Key that the user inputs.
            if(!_plDead && !_plWin)
            {
                _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
                _spriteBatch.Draw(_plIdle, new Vector2(_plX, _plY), _plColor);
                _spriteBatch.Draw(_box, new Vector2(_boxX, _boxY), Color.White);
                _spriteBatch.Draw(_boxD, new Vector2(_boxDX, _boxDY), Color.White);
                _spriteBatch.DrawString(_font, "Lives: " + _plLives, new Vector2(80, 50), Color.White);
                _spriteBatch.DrawString(_font, "Score: " + _plScore, new Vector2(900, 50), Color.White);

                if(Keyboard.GetState().IsKeyDown(Keys.A))
                    _spriteBatch.Draw(_plLeft, new Vector2(_plX, _plY), _plColor);

                if(Keyboard.GetState().IsKeyDown(Keys.D))
                    _spriteBatch.Draw(_plRight, new Vector2(_plX, _plY), _plColor);

                for(int i = 0; i < _numGlitter; i++)
                {
                    _spriteBatch.Draw(_glitter, new Vector2(_glitterX[i], _glitterY[i]), null, Color.White, _glitterRotation, 
                        new Vector2(_glitter.Width / 2, _glitter.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0f);
                }

            }

            //If the player is dead, it changes the background, and it prints a string in the middle of the screen.
            if(_plDead == true)
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
            if (_plWin == true)
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
