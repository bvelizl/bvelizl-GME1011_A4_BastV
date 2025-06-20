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
    internal class woodenBox : Box
    {
        //Constructor of the wooden box.
        //Using :base to call the super class constructor.

        public woodenBox(Texture2D texture, Vector2 position) : base(texture, position)
        {

        }

        //Mutator to add score to the player if collide with it.
        public override void Collides(Player player)
        {
            player._Score++;
            base.Respawn();
        }
    }
}
