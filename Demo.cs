using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Engine.Classes;
using System.Security.Cryptography.Xml;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Data;
using System.Diagnostics;

namespace Engine
{
    internal class Demo : Classes.Engine
    {
        public Demo() : base(new Classes.Vector(656,679),"Demo") { }

        bool white = false;
        Point boardMouse;
        bool click = false;
        bool clickR = false;
        string selectedPiece = null;
        Vector selectedSquare = null;
        Shape2D highlight = null;
        string[] whitePieces = { "W_Pawn", "W_Rook", "W_Knight", "W_Bishop", "W_Queen", "W_King" };
        string[] blackPieces = { "B_Pawn", "B_Rook", "B_Knight", "B_Bishop", "B_Queen", "B_King" };
        public List<int> whiteSpaces = new List<int>();
        public List<int> blackSpaces = new List<int>();
        bool[] firstMoves_W = { true, true, true, true, true, true, true, true };     //keeps track if each pawn should be able to move two spaces for their first turn
        static int[] whitePawnStarts = { 6, 14, 22, 30, 38, 46, 54, 62};
        static int[] blackPawnStarts = { 1, 9, 17, 25, 33, 41, 49, 57 };
        bool[] firstMoves_B = { true, true, true, true, true, true, true, true };
        bool[] castleRights_W = { true, true };
        bool[] castleRights_B = { true, true };

        int[] pawnHeatmap = { 3, 3, 3, 3, 3, 3, 9, 3,
                              3, 3, 2, 3, 3, 3, 9, 3,
                              3, 3, 1, 3, 3, 4, 9, 3,
                              3, 0, 5, 5, 5, 5, 9, 3,
                              3, 0, 5, 5, 5, 5, 9, 3,
                              3, 3, 1, 3, 3, 4, 9, 3,
                              3, 3, 2, 3, 3, 3, 9, 3,
                              3, 3, 3, 3, 3, 3, 9, 3};
        int[] bishopHeatmap = { 3, 1, 1, 1, 1, 1, 1, 0,
                                1, 5, 4, 3, 3, 3, 3, 1,
                                1, 3, 5, 5, 3, 3, 3, 1,
                                1, 3, 4, 4, 4, 4, 3, 1,
                                1, 3, 4, 4, 4, 4, 3, 1,
                                1, 3, 5, 5, 3, 3, 3, 1,
                                1, 5, 4, 3, 3, 3, 3, 1,
                                3, 1, 1, 1, 1, 1, 1, 0};
        int[] knightHeatmap = { 1, 2, 3, 3, 3, 3, 2, 1,
                                2, 4, 5, 5, 5, 5, 4, 2,
                                3, 4, 5, 5, 5, 5, 4, 3,
                                3, 4, 5, 5, 5, 5, 4, 3,
                                3, 4, 5, 5, 5, 5, 4, 3,
                                3, 4, 5, 5, 5, 5, 4, 3,
                                2, 4, 5, 5, 5, 5, 4, 2,
                                1, 2, 3, 3, 3, 3, 2, 1};
        int[] rookHeatmap = { 2, 1, 1, 1, 1, 1, 4, 3,
                              2, 2, 2, 2, 2, 2, 5, 3,
                              3, 2, 2, 2, 2, 2, 5, 3,
                              4, 2, 2, 2, 2, 2, 5, 3,
                              4, 2, 2, 2, 2, 2, 5, 3,
                              3, 2, 2, 2, 2, 2, 5, 3,
                              2, 2, 2, 2, 2, 2, 5, 3,
                              2, 1, 1, 1, 1, 1, 4, 3};
        int[] queenHeatmap = { 0, 1, 1, 5, 2, 1, 1, 0,
                               1, 2, 2, 2, 2, 2, 2, 1,
                               1, 2, 3, 3, 3, 3, 2, 1,
                               2, 3, 3, 3, 3, 3, 2, 2,
                               2, 3, 3, 3, 3, 3, 2, 2,
                               1, 2, 3, 3, 3, 3, 2, 1,
                               1, 2, 2, 2, 2, 2, 2, 1,
                               0, 1, 1, 2, 2, 1, 1, 0};
        int[] kingHeatmap = { 5, 4, 3, 3, 3, 3, 1, 1,
                              5, 4, 2, 2, 1, 1, 1, 1,
                              3, 3, 2, 2, 1, 1, 0, 0,
                              3, 3, 2, 1, 0, 0, 0, 0,
                              3, 3, 2, 1, 0, 0, 0, 0,
                              3, 3, 2, 2, 1, 1, 0, 0,
                              5, 4, 2, 2, 1, 1, 1, 1,
                              5, 4, 3, 3, 3, 3, 1, 1};
        int[] endGameHeatmap = { 20, 20, 20, 20, 20, 20, 20, 20,
                                 20, 10, 10, 10, 10, 10, 10, 20,
                                 20, 10, 5 , 5 , 5 , 5 , 10, 20,
                                 20, 10, 5 , 0 , 0 , 5 , 10, 20,
                                 20, 10, 5 , 0 , 0 , 5 , 10, 20,
                                 20, 10, 5 , 5 , 5 , 5 , 10, 20,
                                 20, 10, 10, 10, 10, 10, 10, 20,
                                 20, 20, 20, 20, 20, 20, 20, 20};

        bool whiteTurn = true;
        bool toggleTurn = true;
        bool requestMoves = true;

        public List<Square> board = new List<Square>(64);
        public List<int> validMoves = new List<int>();
        public override void OnLoad()
        {
            BackgroundColor = Color.Brown;
            for (int x = 0; x < 64; x++)
            {
                if (x % 8 == 0) { if (white == true) { white = false; } else { white = true; } }
                if (white == false) { white = true; } else
                {
                    new Shape2D(new Vector((x/8) * 80, (x % 8) * 80), new Vector(80, 80), "whiteSquare", new SolidBrush(Color.White));
                    white = false;
                }
                switch (x)
                {
                    case 0: case 56:
                        board.Add(new Square(x, "B_Rook"));
                        blackSpaces.Add(x);
                        break;
                    case 8: case 48:
                        board.Add(new Square(x, "B_Knight"));
                        blackSpaces.Add(x);
                        break;
                    case 16: case 40:
                        board.Add(new Square(x, "B_Bishop"));
                        blackSpaces.Add(x);
                        break;
                    case 24:
                        board.Add(new Square(x, "B_Queen"));
                        blackSpaces.Add(x);
                        break;
                    case 32:
                        board.Add(new Square(x, "B_King"));
                        blackSpaces.Add(x);
                        break;
                    case 1: case 9: case 17: case 25: case 33: case 41: case 49: case 57:
                        board.Add(new Square(x, "B_Pawn"));
                        blackSpaces.Add(x);
                        break;

                    case 7: case 63:
                        board.Add(new Square(x, "W_Rook"));
                        whiteSpaces.Add(x);
                        break;
                    case 15: case 55:
                        board.Add(new Square(x, "W_Knight"));
                        whiteSpaces.Add(x);
                        break;
                    case 23: case 47:
                        board.Add(new Square(x, "W_Bishop"));
                        whiteSpaces.Add(x);
                        break;
                    case 31:
                        board.Add(new Square(x, "W_Queen"));
                        whiteSpaces.Add(x);
                        break;
                    case 39:
                        board.Add(new Square(x, "W_King"));
                        whiteSpaces.Add(x);
                        break;
                    case 6: case 14: case 22: case 30: case 38: case 46: case 54: case 62:
                        board.Add(new Square(x, "W_Pawn"));
                        whiteSpaces.Add(x);
                        break;
                    default:
                        board.Add(new Square(x));
                        break;
                }
            }

            Log.Warn("Done Initializing");
            for (int i = 0; i < 64; i++)
            {
                Log.Info($"{board[i].Piece}");
            }
        }
        public override void OnDraw()
        {
            
        }
        public override void OnUpdate()
        {
            boardMouse.X = (mouseLocation.X / 8) / 10;
            boardMouse.Y = (mouseLocation.Y / 80) % 8;

            if (whiteTurn)      //players turn
            {
                if (requestMoves)
                {
                    requestMoves = false;
                    Log.Warn("Getting moves...");
                    validMoves = getMoves(board, whiteSpaces, blackSpaces, true);
                    /*for (int i = 0; i < validMoves.Count; i+=2)
                    {
                        Log.Info($"from:{validMoves[i]}, to:{validMoves[i+1]}");
                    }*/
                }
                if (click)
                {
                    Log.Info($"Selected Piece:{board[(boardMouse.X * 8) + boardMouse.Y].Piece}, Index:{(boardMouse.X * 8) + boardMouse.Y}, Coordinates:{boardMouse.X}, {boardMouse.Y}");
                    if (blackSpaces.Contains((boardMouse.X * 8) + boardMouse.Y)) { Log.Info("Black spaces contains this piece"); }
                    if (whiteSpaces.Contains((boardMouse.X * 8) + boardMouse.Y)) { Log.Info("White spaces contains this piece"); }
                    if (selectedPiece != null && board[(boardMouse.X * 8) + boardMouse.Y].BoardPosition != selectedSquare && board[(boardMouse.X * 8) + boardMouse.Y].highlight != null)   //if you click on a new square
                    {
                        board[(boardMouse.X * 8) + boardMouse.Y].ChangePiece(selectedPiece);
                        blackSpaces.Remove((boardMouse.X * 8) + boardMouse.Y);
                        board[((int)selectedSquare.x * 8) + (int)selectedSquare.y].ChangePiece();
                        whiteSpaces.Remove(((int)selectedSquare.x * 8) + (int)selectedSquare.y);
                        whiteSpaces.Add((boardMouse.X * 8) + boardMouse.Y);
                        board[((int)selectedSquare.x * 8) + (int)selectedSquare.y].HighlightOff();
                        foreach(Square space in board)
                        {
                            if (space.highlight != null) { space.HighlightOff(); }
                        }
                        if (selectedPiece == "W_Pawn" && selectedSquare.y == 6) { firstMoves_W[(((int)selectedSquare.x * 8) + (int)selectedSquare.y) / 8] = false; }
                        Log.Warn($"{selectedPiece}, {selectedSquare.y}");
                        if (selectedPiece == "W_Pawn" && boardMouse.Y == 0)
                        {
                            switch ((boardMouse.X * 8) + boardMouse.Y)
                            {
                                case 0:
                                    board[0].ChangePiece("W_Queen");
                                    break;
                                case 8:
                                    board[8].ChangePiece("W_Queen");
                                    break;
                                case 16:
                                    board[16].ChangePiece("W_Queen");
                                    break;
                                case 24:
                                    board[24].ChangePiece("W_Queen");
                                    break;
                                case 32:
                                    board[32].ChangePiece("W_Queen");
                                    break;
                                case 40:
                                    board[40].ChangePiece("W_Queen");
                                    break;
                                case 48:
                                    board[48].ChangePiece("W_Queen");
                                    break;
                                case 56:
                                    board[56].ChangePiece("W_Queen");
                                    break;

                            }
                        }
                        if (selectedPiece == "W_King" && castleRights_W[0] && ((boardMouse.X * 8) + boardMouse.Y == 23)) { board[31].ChangePiece("W_Rook"); board[7].ChangePiece(); whiteSpaces.Add(31); }
                        if (selectedPiece == "W_King" && castleRights_W[1] && ((boardMouse.X * 8) + boardMouse.Y == 55)) { board[47].ChangePiece("W_Rook"); board[63].ChangePiece(); whiteSpaces.Add(47); }
                        if (selectedPiece == "W_King") { castleRights_W[0] = false; castleRights_W[1] = false; }
                        if (selectedPiece == "W_Rook" && selectedSquare.x == 0) { castleRights_W[0] = false; }
                        if (selectedPiece == "W_Rook" && selectedSquare.x == 7) { castleRights_W[1] = false; }
                        selectedPiece = null;
                        selectedSquare = null;

                        whiteTurn = false;
                        requestMoves = true;
                    }
                    else if (selectedPiece == null && board[(boardMouse.X * 8) + boardMouse.Y].Piece != "Empty")
                    {
                        selectedPiece = board[(boardMouse.X * 8) + boardMouse.Y].Piece;
                        selectedSquare = board[(boardMouse.X * 8) + boardMouse.Y].BoardPosition;
                        int selectedIndex = (int)(selectedSquare.x * 8) + (int)selectedSquare.y;
                        board[selectedIndex].HighlightOn();
                        for (int i = 0; i <= validMoves.Count; i += 2)
                        {
                            if (validMoves[i] == selectedIndex) { board[validMoves[i + 1]].HighlightOn(); }
                        }
                    }
                    click = false;
                }
                if (clickR)
                {
                    if (selectedPiece != null)
                    {
                        board[((int)selectedSquare.x * 8) + (int)selectedSquare.y].HighlightOff();
                    }
                    foreach (Square space in board)
                    {
                        if (space.highlight != null) { space.HighlightOff(); }
                    }
                    selectedPiece = null;
                    selectedSquare = null;
                    clickR = false;
                }
            } else //ai turn
            {
                if (toggleTurn)
                {
                    toggleTurn = false;
                    Log.Error("Black Turn");
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    validMoves = getMoves(board, whiteSpaces, blackSpaces, false);

                    Log.Warn("Calculating best move...");
                    //int bestMove = negaMax(3, board, whiteSpaces, blackSpaces, false, true);
                    int bestMoveAB = alphaBeta(3, board, validMoves, whiteSpaces, blackSpaces, false, true, -int.MaxValue, int.MaxValue);

                    //int from = validMoves[bestMove];
                    //int to = validMoves[bestMove + 1];
                    int fromAB = validMoves[bestMoveAB];
                    int toAB = validMoves[bestMoveAB + 1];

                    //Log.Warn($"Minimax selected Index:{from}, Target Index:{to}");
                    Log.Warn($"Alpha Beta selected Index:{fromAB}, Target Index:{toAB}");

                    int specialCode = checkSpecial(board, fromAB, toAB, board[fromAB].Piece);
                    Log.Error($"Special code is {specialCode}");
                    switch (specialCode)
                    {
                        case 9:
                            blackSpaces.Add(24);
                            blackSpaces.Remove(0);
                            break;
                        case 10:
                            blackSpaces.Add(40);
                            blackSpaces.Remove(56);
                            break;
                    }

                    if (specialCode == 0 || specialCode == 9 || specialCode == 10) { board[toAB].ChangePiece(board[fromAB].Piece); }
                    whiteSpaces.Remove(toAB);
                    board[fromAB].ChangePiece();
                    blackSpaces.Remove(fromAB);
                    blackSpaces.Add(toAB);

                    watch.Stop();
                    TimeSpan t = watch.Elapsed;
                    Log.Info($"Move calculated in {t.TotalMilliseconds} milliseconds");
                    Log.Info($"Board evaluation: {evaluate(board, false)}");

                    toggleTurn = true;
                    requestMoves = true;
                    whiteTurn = true;
                }
            }
        }

        public override void GetKeyDown(KeyEventArgs e) { }

        public override void GetKeyUp(KeyEventArgs e) { }

        public override void GetMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { click = true; }
            if (e.Button == MouseButtons.Right) { clickR = true; }
        }


        public List<int> getMoves(List<Square> Board, List<int> whites, List<int> blacks, bool whiteTurn)
        {
            List<int> moves = new List<int>();   //this will be in the form that alternates between a selected square and a target square (selected, target, selected, target)
            string selectedPiece;

            if (whiteTurn)
            {
                foreach(int space in whites)   //the spaces arrays contain ints which are the indeces of the squares the white pieces are located at
                {
                    selectedPiece = Board[space].Piece;
                    if (selectedPiece == "W_King")
                    {
                        if ((space - 1) >= 0 && !whitePieces.Contains(Board[space - 1].Piece) && Board[space - 1].BoardPosition.x == Board[space].BoardPosition.x) { moves.Add(space); moves.Add(space - 1); }
                        if ((space + 1) < 64 && !whitePieces.Contains(Board[space + 1].Piece) && Board[space + 1].BoardPosition.x == Board[space].BoardPosition.x) { moves.Add(space); moves.Add(space + 1); }
                        if ((space - 8) >= 0 && !whitePieces.Contains(Board[space - 8].Piece)) { moves.Add(space); moves.Add(space - 8); }
                        if ((space - 7) >= 0 && !whitePieces.Contains(Board[space - 7].Piece) && (Board[space - 7].BoardPosition.x - Board[space].BoardPosition.x) == -1) { moves.Add(space); moves.Add(space - 7); }
                        if ((space - 9) >= 0 && !whitePieces.Contains(Board[space - 9].Piece) && (Board[space - 9].BoardPosition.x - Board[space].BoardPosition.x) == -1) { moves.Add(space); moves.Add(space - 9); }
                        if ((space + 8) < 64 && !whitePieces.Contains(Board[space + 8].Piece)) { moves.Add(space); moves.Add(space + 8); }
                        if ((space + 7) < 64 && !whitePieces.Contains(Board[space + 7].Piece) && (Board[space + 7].BoardPosition.x - Board[space].BoardPosition.x) == 1) { moves.Add(space); moves.Add(space + 7); }
                        if ((space + 9) < 64 && !whitePieces.Contains(Board[space + 9].Piece) && (Board[space + 9].BoardPosition.x - Board[space].BoardPosition.x) == 1) { moves.Add(space); moves.Add(space + 9); }

                        if (castleRights_W[0] && Board[15].Piece == "Empty" && Board[23].Piece == "Empty" && Board[31].Piece == "Empty") { moves.Add(space); moves.Add(23); }
                        if (castleRights_W[1] && Board[47].Piece == "Empty" && Board[55].Piece == "Empty") { moves.Add(space); moves.Add(55); }
                    }
                    if (selectedPiece == "W_Rook")
                    {
                        int i = 1;
                        while (space - i >= 0 && Board[space - i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!whitePieces.Contains(Board[space - i].Piece)) { moves.Add(space); moves.Add(space - i); if (blackPieces.Contains(Board[space - i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i < 64 && Board[space + i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!whitePieces.Contains(Board[space + i].Piece)) { moves.Add(space); moves.Add(space + i); if (blackPieces.Contains(Board[space + i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 8 >= 0)
                        {
                            if (!whitePieces.Contains(Board[space - i * 8].Piece)) { moves.Add(space); moves.Add(space - i * 8); if (blackPieces.Contains(Board[space - i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 8 < 64)
                        {
                            if (!whitePieces.Contains(Board[space + i * 8].Piece)) { moves.Add(space); moves.Add(space + i * 8); if (blackPieces.Contains(Board[space + i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                    }
                    if (selectedPiece == "W_Knight")
                    {
                        if ((space - 6) >= 0 && !whitePieces.Contains(Board[space - 6].Piece) && Board[space - 6].BoardPosition.x == (Board[space].BoardPosition.x - 1)) { moves.Add(space); moves.Add(space - 6); }
                        if ((space - 10) >= 0 && !whitePieces.Contains(Board[space - 10].Piece) && Board[space - 10].BoardPosition.x == (Board[space].BoardPosition.x - 1)) { moves.Add(space); moves.Add(space - 10); }
                        if ((space - 15) >= 0 && !whitePieces.Contains(Board[space - 15].Piece) && Board[space - 15].BoardPosition.x == (Board[space].BoardPosition.x - 2)) { moves.Add(space); moves.Add(space - 15); }
                        if ((space - 17) >= 0 && !whitePieces.Contains(Board[space - 17].Piece) && Board[space - 17].BoardPosition.x == (Board[space].BoardPosition.x - 2)) { moves.Add(space); moves.Add(space - 17); }
                        if ((space + 6) < 64 && !whitePieces.Contains(Board[space + 6].Piece) && Board[space + 6].BoardPosition.x == (Board[space].BoardPosition.x + 1)) { moves.Add(space); moves.Add(space + 6); }
                        if ((space + 10) < 64 && !whitePieces.Contains(Board[space + 10].Piece) && Board[space + 10].BoardPosition.x == (Board[space].BoardPosition.x + 1)) { moves.Add(space); moves.Add(space + 10); }
                        if ((space + 15) < 64 && !whitePieces.Contains(Board[space + 15].Piece) && Board[space + 15].BoardPosition.x == (Board[space].BoardPosition.x + 2)) { moves.Add(space); moves.Add(space + 15); }
                        if ((space + 17) < 64 && !whitePieces.Contains(Board[space + 17].Piece) && Board[space + 17].BoardPosition.x == (Board[space].BoardPosition.x + 2)) { moves.Add(space); moves.Add(space + 17); }
                    }
                    if (selectedPiece == "W_Bishop")
                    {
                        int i = 1;
                        while (space - i * 7 >= 0 && Board[space - i * 7].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!whitePieces.Contains(Board[space - i * 7].Piece)) { moves.Add(space); moves.Add(space - i * 7); if (blackPieces.Contains(Board[space - i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 7 < 64 && Board[space + i * 7].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!whitePieces.Contains(Board[space + i * 7].Piece)) { moves.Add(space); moves.Add(space + i * 7); if (blackPieces.Contains(Board[space + i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 9 >= 0 && Board[space - i * 9].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!whitePieces.Contains(Board[space - i * 9].Piece)) { moves.Add(space); moves.Add(space - i * 9); if (blackPieces.Contains(Board[space - i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 9 < 64 && Board[space + i * 9].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!whitePieces.Contains(Board[space + i * 9].Piece)) { moves.Add(space); moves.Add(space + i * 9); if (blackPieces.Contains(Board[space + i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }

                    }
                    if (selectedPiece == "W_Queen")
                    {
                        int i = 1;
                        while (space - i * 7 >= 0 && Board[space - i * 7].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!whitePieces.Contains(Board[space - i * 7].Piece)) { moves.Add(space); moves.Add(space - i * 7); if (blackPieces.Contains(Board[space - i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 7 < 64 && Board[space + i * 7].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!whitePieces.Contains(Board[space + i * 7].Piece)) { moves.Add(space); moves.Add(space + i * 7); if (blackPieces.Contains(Board[space + i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 9 >= 0 && Board[space - i * 9].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!whitePieces.Contains(Board[space - i * 9].Piece)) { moves.Add(space); moves.Add(space - i * 9); if (blackPieces.Contains(Board[space - i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 9 < 64 && Board[space + i * 9].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!whitePieces.Contains(Board[space + i * 9].Piece)) { moves.Add(space); moves.Add(space + i * 9); if (blackPieces.Contains(Board[space + i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i >= 0 && Board[space - i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!whitePieces.Contains(Board[space - i].Piece)) { moves.Add(space); moves.Add(space - i); if (blackPieces.Contains(Board[space - i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i < 64 && Board[space + i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!whitePieces.Contains(Board[space + i].Piece)) { moves.Add(space); moves.Add(space + i); if (blackPieces.Contains(Board[space + i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 8 >= 0)
                        {
                            if (!whitePieces.Contains(Board[space - i * 8].Piece)) { moves.Add(space); moves.Add(space - i * 8); if (blackPieces.Contains(Board[space - i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 8 < 64)
                        {
                            if (!whitePieces.Contains(Board[space + i * 8].Piece)) { moves.Add(space); moves.Add(space + i * 8); if (blackPieces.Contains(Board[space + i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                    }
                    if (selectedPiece == "W_Pawn")
                    {
                        if (firstMoves_W[space / 8] && Board[space - 1].Piece == "Empty" && Board[space - 2].Piece == "Empty" && whitePawnStarts.Contains(space)) { moves.Add(space); moves.Add(space - 2); }
                        if (Board[space - 1].Piece == "Empty") { moves.Add(space); moves.Add(space - 1); }
                        if (space - 9 >= 0 && blackPieces.Contains(Board[space - 9].Piece)) { moves.Add(space); moves.Add(space - 9); }
                        if (space + 7 < 64 && blackPieces.Contains(Board[space + 7].Piece)) { moves.Add(space); moves.Add(space + 7); }
                    }
                }    
            } else
            {
                foreach (int space in blacks)
                {
                    selectedPiece = Board[space].Piece;
                    if (selectedPiece == "B_King")
                    {
                        if ((space - 1) >= 0 && !blackPieces.Contains(Board[space - 1].Piece) && Board[space - 1].BoardPosition.x == Board[space].BoardPosition.x) { moves.Add(space); moves.Add(space - 1); }
                        if ((space + 1) < 64 && !blackPieces.Contains(Board[space + 1].Piece) && Board[space + 1].BoardPosition.x == Board[space].BoardPosition.x) { moves.Add(space); moves.Add(space + 1); }
                        if ((space - 8) >= 0 && !blackPieces.Contains(Board[space - 8].Piece)) { moves.Add(space); moves.Add(space - 8); }
                        if ((space - 7) >= 0 && !blackPieces.Contains(Board[space - 7].Piece) && (Board[space - 7].BoardPosition.x - Board[space].BoardPosition.x) == -1) { moves.Add(space); moves.Add(space - 7); }
                        if ((space - 9) >= 0 && !blackPieces.Contains(Board[space - 9].Piece) && (Board[space - 9].BoardPosition.x - Board[space].BoardPosition.x) == -1) { moves.Add(space); moves.Add(space - 9); }
                        if ((space + 8) < 64 && !blackPieces.Contains(Board[space + 8].Piece)) { moves.Add(space); moves.Add(space + 8); }
                        if ((space + 7) < 64 && !blackPieces.Contains(Board[space + 7].Piece) && (Board[space + 7].BoardPosition.x - Board[space].BoardPosition.x) == 1) { moves.Add(space); moves.Add(space + 7); }
                        if ((space + 9) < 64 && !blackPieces.Contains(Board[space + 9].Piece) && (Board[space + 9].BoardPosition.x - Board[space].BoardPosition.x) == 1) { moves.Add(space); moves.Add(space + 9); }

                        if (castleRights_B[0] && Board[0].Piece == "B_Rook" && Board[8].Piece == "Empty" && Board[16].Piece == "Empty" && Board[24].Piece == "Empty") { moves.Add(space); moves.Add(16); }
                        if (castleRights_B[1] && Board[40].Piece == "Empty" && Board[48].Piece == "Empty" && Board[56].Piece == "B_Rook") { moves.Add(space); moves.Add(48); }
                    }
                    if (selectedPiece == "B_Rook")
                    {
                        int i = 1;
                        while (space - i >= 0 && Board[space - i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!blackPieces.Contains(Board[space - i].Piece)) { moves.Add(space); moves.Add(space - i); if (whitePieces.Contains(Board[space - i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i < 64 && Board[space + i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!blackPieces.Contains(Board[space + i].Piece)) { moves.Add(space); moves.Add(space + i); if (whitePieces.Contains(Board[space + i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 8 >= 0)
                        {
                            if (!blackPieces.Contains(Board[space - i * 8].Piece)) { moves.Add(space); moves.Add(space - i * 8); if (whitePieces.Contains(Board[space - i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 8 < 64)
                        {
                            if (!blackPieces.Contains(Board[space + i * 8].Piece)) { moves.Add(space); moves.Add(space + i * 8); if (whitePieces.Contains(Board[space + i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                    }
                    if (selectedPiece == "B_Knight")
                    {
                        if ((space - 6) >= 0 && !blackPieces.Contains(Board[space - 6].Piece) && Board[space - 6].BoardPosition.x == (Board[space].BoardPosition.x - 1)) { moves.Add(space); moves.Add(space - 6); }
                        if ((space - 10) >= 0 && !blackPieces.Contains(Board[space - 10].Piece) && Board[space - 10].BoardPosition.x == (Board[space].BoardPosition.x - 1)) { moves.Add(space); moves.Add(space - 10); }
                        if ((space - 15) >= 0 && !blackPieces.Contains(Board[space - 15].Piece) && Board[space - 15].BoardPosition.x == (Board[space].BoardPosition.x - 2)) { moves.Add(space); moves.Add(space - 15); }
                        if ((space - 17) >= 0 && !blackPieces.Contains(Board[space - 17].Piece) && Board[space - 17].BoardPosition.x == (Board[space].BoardPosition.x - 2)) { moves.Add(space); moves.Add(space - 17); }
                        if ((space + 6) < 64 && !blackPieces.Contains(Board[space + 6].Piece) && Board[space + 6].BoardPosition.x == (Board[space].BoardPosition.x + 1)) { moves.Add(space); moves.Add(space + 6); }
                        if ((space + 10) < 64 && !blackPieces.Contains(Board[space + 10].Piece) && Board[space + 10].BoardPosition.x == (Board[space].BoardPosition.x + 1)) { moves.Add(space); moves.Add(space + 10); }
                        if ((space + 15) < 64 && !blackPieces.Contains(Board[space + 15].Piece) && Board[space + 15].BoardPosition.x == (Board[space].BoardPosition.x + 2)) { moves.Add(space); moves.Add(space + 15); }
                        if ((space + 17) < 64 && !blackPieces.Contains(Board[space + 17].Piece) && Board[space + 17].BoardPosition.x == (Board[space].BoardPosition.x + 2)) { moves.Add(space); moves.Add(space + 17); }
                    }
                    if (selectedPiece == "B_Bishop")
                    {
                        int i = 1;
                        while (space - i * 7 >= 0 && Board[space - i * 7].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!blackPieces.Contains(Board[space - i * 7].Piece)) { moves.Add(space); moves.Add(space - i * 7); if (whitePieces.Contains(Board[space - i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 7 < 64 && Board[space + i * 7].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!blackPieces.Contains(Board[space + i * 7].Piece)) { moves.Add(space); moves.Add(space + i * 7); if (whitePieces.Contains(Board[space + i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 9 >= 0 && Board[space - i * 9].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!blackPieces.Contains(Board[space - i * 9].Piece)) { moves.Add(space); moves.Add(space - i * 9); if (whitePieces.Contains(Board[space - i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 9 < 64 && Board[space + i * 9].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!blackPieces.Contains(Board[space + i * 9].Piece)) { moves.Add(space); moves.Add(space + i * 9); if (whitePieces.Contains(Board[space + i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }

                    }
                    if (selectedPiece == "B_Queen")
                    {
                        int i = 1;
                        while (space - i * 7 >= 0 && Board[space - i * 7].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!blackPieces.Contains(Board[space - i * 7].Piece)) { moves.Add(space); moves.Add(space - i * 7); if (whitePieces.Contains(Board[space - i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 7 < 64 && Board[space + i * 7].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!blackPieces.Contains(Board[space + i * 7].Piece)) { moves.Add(space); moves.Add(space + i * 7); if (whitePieces.Contains(Board[space + i * 7].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 9 >= 0 && Board[space - i * 9].BoardPosition.x == Board[space].BoardPosition.x - i)
                        {
                            if (!blackPieces.Contains(Board[space - i * 9].Piece)) { moves.Add(space); moves.Add(space - i * 9); if (whitePieces.Contains(Board[space - i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 9 < 64 && Board[space + i * 9].BoardPosition.x == Board[space].BoardPosition.x + i)
                        {
                            if (!blackPieces.Contains(Board[space + i * 9].Piece)) { moves.Add(space); moves.Add(space + i * 9); if (whitePieces.Contains(Board[space + i * 9].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i >= 0 && Board[space - i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!blackPieces.Contains(Board[space - i].Piece)) { moves.Add(space); moves.Add(space - i); if (whitePieces.Contains(Board[space - i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i < 64 && Board[space + i].BoardPosition.x == Board[space].BoardPosition.x)
                        {
                            if (!blackPieces.Contains(Board[space + i].Piece)) { moves.Add(space); moves.Add(space + i); if (whitePieces.Contains(Board[space + i].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space - i * 8 >= 0)
                        {
                            if (!blackPieces.Contains(Board[space - i * 8].Piece)) { moves.Add(space); moves.Add(space - i * 8); if (whitePieces.Contains(Board[space - i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                        i = 1;
                        while (space + i * 8 < 64)
                        {
                            if (!blackPieces.Contains(Board[space + i * 8].Piece)) { moves.Add(space); moves.Add(space + i * 8); if (whitePieces.Contains(Board[space + i * 8].Piece)) { break; } }
                            else { break; }
                            i++;
                        }
                    }
                    if (selectedPiece == "B_Pawn")
                    {
                        if (firstMoves_B[space / 8] && Board[space + 1].Piece == "Empty" && Board[space + 2].Piece == "Empty" && blackPawnStarts.Contains(space)) { moves.Add(space); moves.Add(space + 2); }
                        if (Board[space + 1].Piece == "Empty") { moves.Add(space); moves.Add(space + 1); }
                        if (space + 9 < 64 && whitePieces.Contains(Board[space + 9].Piece)) { moves.Add(space); moves.Add(space + 9); }
                        if (space - 7 >= 0 && whitePieces.Contains(Board[space - 7].Piece)) { moves.Add(space); moves.Add(space - 7); }
                    }
                }
            }
            return moves;
        }

        public int negaMax(int depth, List<Square> Board, List<int> whites, List<int> blacks, bool turn, bool root)
        {
            if (depth == 0) { return evaluate(Board, false); }
            int bestScore = int.MinValue;
            int bestIndex = 0;
            string toPieceCopy;
            List<int> moves = getMoves(Board, whites, blacks, turn);

            for (int i = 0; i < moves.Count; i += 2)
            {
                //make move
                if (turn)
                {
                    toPieceCopy = Board[moves[i + 1]].Piece;
                    Board[moves[i + 1]].SimChangePiece(Board[moves[i]].Piece);
                    Board[moves[i]].SimChangePiece();
                    blacks.Remove(moves[i + 1]);
                    whites.Remove(moves[i]);
                    whites.Add(moves[i + 1]);
                }
                else
                {
                    toPieceCopy = Board[moves[i + 1]].Piece;
                    Board[moves[i + 1]].SimChangePiece(Board[moves[i]].Piece);
                    Board[moves[i]].SimChangePiece();
                    whites.Remove(moves[i + 1]);
                    blacks.Remove(moves[i]);
                    blacks.Add(moves[i + 1]);
                }

                //evaluate move
                int score = -negaMax(depth - 1, Board, whites, blacks, !turn, false);
                //if (depth == 1) { Log.Info($"testing i {i} out of {moves.Count} in depth {depth}. Current move is {moves[i]} to {moves[i + 1]} which scored {score}"); }
                //if (depth == 2) { Log.Warn($"testing i {i} out of {moves.Count} in depth {depth}. Current move is {moves[i]} to {moves[i + 1]} which scored {score}"); }
                //if (depth == 3) { Log.Error($"testing i {i} out of {moves.Count} in depth {depth}. Current move is {moves[i]} to {moves[i + 1]} which scored {score}"); }
                if (score > bestScore) { bestScore = score; bestIndex = i; }

                //unmake move
                if (turn)
                {
                    Board[moves[i]].SimChangePiece(Board[moves[i + 1]].Piece);
                    Board[moves[i + 1]].SimChangePiece(toPieceCopy);
                    whites.Remove(moves[i + 1]);
                    whites.Add(moves[i]);
                    if (toPieceCopy != "Empty") { blacks.Add(moves[i + 1]); }
                }
                else
                {
                    Board[moves[i]].SimChangePiece(Board[moves[i + 1]].Piece);
                    Board[moves[i + 1]].SimChangePiece(toPieceCopy);
                    blacks.Remove(moves[i + 1]);
                    blacks.Add(moves[i]);
                    if (toPieceCopy != "Empty") { whites.Add(moves[i + 1]); }
                }
            }

            if (root == true) { return bestIndex; }
            else { return bestScore; }
        }

        public int alphaBeta(int depth, List<Square> Board, List<int> moves, List<int> whites, List<int> blacks, bool turn, bool root, int alpha, int beta)
        {
            if (depth == 0) { return evaluate(Board, false); }
            int bestScore = int.MinValue;
            int bestIndex = 0;
            string toPieceCopy;
            int specialCode = 0;

            if (!root)
            {
                moves = getMoves(Board, whites, blacks, turn);
            }
            
            for (int i = 0; i < moves.Count; i += 2)
            {
                //make move
                if (turn)
                {
                    toPieceCopy = Board[moves[i + 1]].Piece;
                    Board[moves[i + 1]].SimChangePiece(Board[moves[i]].Piece);
                    Board[moves[i]].SimChangePiece();
                    blacks.Remove(moves[i + 1]);
                    whites.Remove(moves[i]);
                    whites.Add(moves[i + 1]);
                }
                else
                {
                    toPieceCopy = Board[moves[i + 1]].Piece;
                    Board[moves[i + 1]].SimChangePiece(Board[moves[i]].Piece);
                    Board[moves[i]].SimChangePiece();
                    whites.Remove(moves[i + 1]);
                    blacks.Remove(moves[i]);
                    blacks.Add(moves[i + 1]);
                    //check for special moves
                    specialCode = checkSpecial(Board, moves[i], moves[i + 1], Board[moves[i]].Piece);
                    switch (specialCode)
                    {
                        case 9:
                            blacks.Add(16);
                            blacks.Add(24);
                            break;
                        case 10:
                            blacks.Add(40);
                            blacks.Add(48);
                            break;
                    }
                }

                //evaluate move
                int score = -alphaBeta(depth - 1, Board, moves, whites, blacks, !turn, false, -beta, -alpha);

                //unmake move
                if (turn)
                {
                    Board[moves[i]].SimChangePiece(Board[moves[i + 1]].Piece);
                    Board[moves[i + 1]].SimChangePiece(toPieceCopy);
                    whites.Remove(moves[i + 1]);
                    whites.Add(moves[i]);
                    if (toPieceCopy != "Empty") { blacks.Add(moves[i + 1]); }
                }
                else
                {
                    Board[moves[i]].SimChangePiece(Board[moves[i + 1]].Piece);
                    Board[moves[i + 1]].SimChangePiece(toPieceCopy);
                    blacks.Remove(moves[i + 1]);
                    blacks.Add(moves[i]);
                    if (toPieceCopy != "Empty") { whites.Add(moves[i + 1]); }
                    //if (specialCode == 9) { board[24].ChangePiece(); board[0].ChangePiece("B_Rook"); blacks.Remove(24); blacks.Add(0); }
                    switch (specialCode)
                    {
                        case 9:
                            board[24].ChangePiece();
                            board[0].ChangePiece("B_Rook");
                            blacks.Remove(24);
                            blacks.Remove(16);
                            blacks.Add(0);
                            break;
                        case 10:
                            board[48].ChangePiece();
                            board[56].ChangePiece("B_Rook");
                            blacks.Remove(48);
                            blacks.Remove(40);
                            blacks.Add(56);
                            break;
                    }
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestIndex = i;
                    if (score > alpha) { alpha = score; }
                }
                if (score >= beta) { return bestScore; }
            }

            if (root == true) { return bestIndex; }
            return bestScore;
        }

        public int checkSpecial(List<Square> board, int from, int to, string piece)
        {
            // 0 = no special, 1-8 = pawn promoted to queen, 9 = left side castle, 10 = right side castle
            // right side castle puts a rook on 40, left side castle puts a rook on 24
            int specialCode = 0;

            switch (from)
            {
                case 32:
                    //king cases
                    if (to == 16 && piece == "B_King") { specialCode = 9; board[24].ChangePiece("B_Rook"); board[0].ChangePiece(); break; }
                    if (to == 48 && piece == "B_King") { specialCode = 10; board[40].ChangePiece("B_Rook"); board[56].ChangePiece(); break; }
                    break;
                case 6:
                    if (piece == "B_Pawn")
                    {
                        if (to == 7 || to == 15)
                        {
                            specialCode = 1;
                            if (to == 7) { board[7].ChangePiece("B_Queen"); }
                            if (to == 15) { board[15].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 14:
                    if (piece == "B_Pawn")
                    {
                        if (to == 15 || to == 7 || to == 23)
                        {
                            specialCode = 2;
                            if (to == 7) { board[7].ChangePiece("B_Queen"); }
                            if (to == 15) { board[15].ChangePiece("B_Queen"); }
                            if (to == 23) { board[23].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 22:
                    if (piece == "B_Pawn")
                    {
                        if (to == 23 || to == 15 || to == 31)
                        {
                            specialCode = 3;
                            if (to == 15) { board[15].ChangePiece("B_Queen"); }
                            if (to == 23) { board[23].ChangePiece("B_Queen"); }
                            if (to == 31) { board[31].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 30:
                    if (piece == "B_Pawn")
                    {
                        if (to == 31 || to == 23 || to == 39)
                        {
                            specialCode = 4;
                            if (to == 23) { board[23].ChangePiece("B_Queen"); }
                            if (to == 31) { board[31].ChangePiece("B_Queen"); }
                            if (to == 39) { board[39].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 38:
                    if (piece == "B_Pawn")
                    {
                        if (to == 39 || to == 31 || to == 47)
                        {
                            specialCode = 5;
                            if (to == 31) { board[31].ChangePiece("B_Queen"); }
                            if (to == 39) { board[39].ChangePiece("B_Queen"); }
                            if (to == 47) { board[47].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 46:
                    if (piece == "B_Pawn")
                    {
                        if (to == 47 || to == 39 || to == 55)
                        {
                            specialCode = 6;
                            if (to == 39) { board[39].ChangePiece("B_Queen"); }
                            if (to == 47) { board[47].ChangePiece("B_Queen"); }
                            if (to == 55) { board[55].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 54:
                    if (piece == "B_Pawn")
                    {
                        if (to == 55 || to == 47 || to == 63)
                        {
                            specialCode = 7;
                            if (to == 47) { board[47].ChangePiece("B_Queen"); }
                            if (to == 55) { board[55].ChangePiece("B_Queen"); }
                            if (to == 63) { board[63].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
                case 62:
                    if (piece == "B_Pawn")
                    {
                        if (to == 63 || to == 55)
                        {
                            specialCode = 8;
                            if (to == 55) { board[55].ChangePiece("B_Queen"); }
                            if (to == 63) { board[63].ChangePiece("B_Queen"); }
                            break;
                        }
                    }
                    break;
            }

            return specialCode;
        }

        public int evaluate(List<Square> Board, bool WTurn)
        {
            int WScore = 0;
            int BScore = 0;
            int i = 0;
            int pieceCount = 0;
            int wKingSquare = -1;
            int bKingSquare = -1;
            for (int j = 0; j < Board.Count; j++)
            {
                if (board[j].Piece != "Empty") { pieceCount++; }
                if (board[j].Piece == "W_King") { wKingSquare = j; }
                if (board[j].Piece == "B_King") { bKingSquare = j; }
            }
            if (bKingSquare == -1 && !WTurn) { return int.MaxValue; }
            
            foreach (Square square in Board)
            {
                if (square.Piece == "W_Pawn") { WScore += 10; continue; }
                if (square.Piece == "W_Knight") { WScore += 30; continue; }
                if (square.Piece == "W_Bishop") { WScore += 30; continue; }
                if (square.Piece == "W_Rook") { WScore += 50; continue; }
                if (square.Piece == "W_Queen") { WScore += 90; continue; }
                if (square.Piece == "W_King") { WScore += 9999; continue; }

                if (square.Piece == "B_Pawn") { if (pieceCount > 8) { BScore += 10; BScore += pawnHeatmap[i]; } continue; }
                if (square.Piece == "B_Knight") { if (pieceCount > 8) { BScore += 30; BScore += knightHeatmap[i]; } continue; }
                if (square.Piece == "B_Bishop") { if (pieceCount > 8) { BScore += 30; BScore += bishopHeatmap[i]; } continue; }
                if (square.Piece == "B_Rook") { if (pieceCount > 8) { BScore += 50; BScore += rookHeatmap[i]; } continue; }
                if (square.Piece == "B_Queen") { if (pieceCount > 8) { BScore += 90; BScore += queenHeatmap[i]; } continue; }
                if (square.Piece == "B_King") { if (pieceCount > 8) { BScore += 9999; BScore += kingHeatmap[i]; } continue; }
                i++;
            }
            if (board[16].Piece == "B_King") { BScore += 1000000000; }
            //if (board[35].Piece == "B_Pawn") { BScore += 100000; }
            if (pieceCount <= 5) { BScore += endGameHeatmap[wKingSquare]; WScore += endGameHeatmap[bKingSquare]; }
            if (WTurn == false) { return WScore - BScore; }
            else { return BScore - WScore; }
        }
    }
}