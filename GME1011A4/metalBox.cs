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
    internal class metalBox : Box
    {
        //Attribute variable of rng. Created to change the player's colour after collision from here.
        private Random rng;

        //Constructor of the metal box.
        //Using :base to call the super class constructor.

        public metalBox(Texture2D texture, Vector2 position) : base(texture, position)
        {
            rng = new Random();
        }

        //Mutator to add score to the player if collide with it.
        public void Collides()
        {
            player.Lives--;
            player.Color = new Color(128 + rng.Next(128), 128 + rng.Next(128), 128 + rng.Next(128));
            base.Respawn();
        }
    }
}
