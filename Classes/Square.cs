using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Classes
{
    public class Square
    {
        public Vector ContextPosition = null;
        public Vector BoardPosition = null;
        public string Piece = null;     //should be the directory for the desired piece
        public Sprite2D Sprite = null;
        public Shape2D highlight = null;

        public Square(int x, string Piece)
        { 
            ContextPosition = new Vector((x / 8) * 80, (x % 8) * 80);
            BoardPosition = new Vector(x / 8, x % 8);
            this.Piece = Piece;

            if (this.Piece != null)
            {
                Sprite = new Sprite2D(ContextPosition, new Vector(80, 80), Piece, $"{Piece}");
            }
        }
        public Square(int x)
        {
            ContextPosition = new Vector((x / 8) * 80, (x % 8) * 80);
            BoardPosition = new Vector(x / 8, x % 8);
            Piece = "Empty";
        }

        public void ChangePiece(string Piece)
        {
            if (Sprite != null)
            {
                Sprite.DestroySelf();
            }
            Sprite = new Sprite2D(ContextPosition, new Vector(80, 80), Piece, $"{Piece}");
            this.Piece = Piece;
        }

        public void SimChangePiece(string Piece)
        {
            this.Piece = Piece;
        }
        public void ChangePiece()
        {
            Sprite.DestroySelf();
            Sprite = null;
            Piece = "Empty";
        }
        public void SimChangePiece()
        {
            Piece = "Empty";
        }

        public void HighlightOn()
        {
            if (highlight == null)
            {
                highlight = new Shape2D(ContextPosition, new Vector(80, 80), "highlight", new SolidBrush(Color.FromArgb(190, 255, 255, 0)));
            }
        }
        public void HighlightOff()
        {
            if (highlight != null)
            {
                highlight.DestroySelf();
                highlight = null;
            }
        }
        public void HighlightToggle()
        {
            if (highlight == null)
            {
                highlight = new Shape2D(ContextPosition, new Vector(80, 80), "highlight", new SolidBrush(Color.FromArgb(190, 255, 255, 0)));
            }
            else
            {
                highlight.DestroySelf();
                highlight = null;
            }
        }
    }
}
