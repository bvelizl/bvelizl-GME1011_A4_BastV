using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace GME1011A4
{
    internal class woodenBox : Box
    {
        //Constructor of the wooden box.
        //Using :base to call the super class constructor.

        public woodenBox(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        //Mutator to add score to the player if collide with it.
        public void Collides()
        {
            player.Score++;
            base.Respawn();
        }
    }
}
