using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Engine.Classes
{
    public class Sprite2D
    {
        public Vector Position = null;
        public Vector Scale = null;
        public string Directory = "";
        public string tag = "";
        public Bitmap Sprite = null;

        public Sprite2D(Vector Position, Vector Scale, string Directory, string tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Directory = Directory;
            this.tag = tag;

            Image tmp = Image.FromFile($"Assets/Sprites/{Directory}.png");
            Bitmap sprite = new Bitmap(tmp);
            Sprite = sprite;

            //Log.Info($"[SPRITE2D]({tag}) has been registered at {Position.x},{Position.y}");
            Engine.RegisterSprite(this);
        }

        public void DestroySelf()
        {
            Engine.UnRegisterSprite(this);
        }
    }
}
