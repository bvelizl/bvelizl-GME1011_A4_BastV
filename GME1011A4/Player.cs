using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;


namespace GME1011A4
{
    internal class Player
        //Declaring my player attribute variables.
    {
        private Vector2 _position;
        private float _speed;
        public int _Lives, _Score;
        public Color _Color;
        private Texture2D _Idle, _Left, _Right;
        public bool _playerDead, _playerWin;

        //Constructor of the player.

        public Player()
        {
            _position = new Vector2(540, 1080);
            _speed = 600f;
            _Lives = 3;
            _Score = 0;
            _Color = Color.White;
            _playerDead = false;
            _playerWin = false;
        }

        //Load content. The idea is to make the player class responsible to modify
        //its sprite according to the direction.
        public void LoadContent(ContentManager content)
        {
            _Idle = content.Load<Texture2D>("Player_Idle");
            _Left = content.Load<Texture2D>("Player_Left");
            _Right = content.Load<Texture2D>("Player_Right");
        }

        //Update. Checking if key A and S is pressed to change the player direction.
        //using delta time.
        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.A))
                _position.X -= _speed * deltaTime;
            if (keyboardState.IsKeyDown(Keys.D))
                _position.X += _speed * deltaTime;

            //Check if the player is dead, or if the player wins.
            if (_Lives == 0)
                _playerDead = true;

            if (_Score == 3)
                _playerWin = true;

            //Creating hitbox for the player.
            Rectangle playerHitbox = new Rectangle((int)_position.X, (int)_position.Y, _Idle.Width, _Idle.Height);
        }

        //Draw. Here the sprite will change according to the player's direction.
        public void Draw(SpriteBatch spriteBatch, KeyboardState keyboardState)
        {
            spriteBatch.Draw(_Idle, _position, Color.White);

            if (keyboardState.IsKeyDown(Keys.A))
                spriteBatch.Draw(_Left, _position, Color.White);

            if (keyboardState.IsKeyDown(Keys.D))
                spriteBatch.Draw(_Right, _position, Color.White);
        }
    }

    
}
