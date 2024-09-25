using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Classes
{
    public class Vector
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vector()
        {
            x = 0;
            y = 0;
        }
        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }
}