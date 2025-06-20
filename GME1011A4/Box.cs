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
    //Declaring the boxes attribute variables.
    internal class Box
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _speed;
        private bool _isBroken;

        //Constructors of the box.

        public Box(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
            _speed = 400f;
            _isBroken = false;
        }

        //Mutator to destroy the box.
        public void IsBroken()
        {
            _isBroken = true;
        }

        //Mutator for collision with the player. Modified later depending on each type of box.
        public virtual void Collides(Player player)
        {

        }

        //Update. Making the box fall using delta time.
        public void Update(GameTime gameTime)
        {
            //Creating float variable of delta time.
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position.Y += _speed * deltaTime;
            if ( _position.Y > 1100)
            {
                this.IsBroken();
            }

            //Creating hitbox for the box.
            Rectangle boxHitbox = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        //Creating accessor to get the box hitbox. Copy and paste the hitbox from above.
        public Rectangle GetHitbox()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        //Creating accessor to check if boxes are broken.
        public bool GetBroken()
        {
            return _isBroken;
        }

        //Draw event of the box.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }

}
