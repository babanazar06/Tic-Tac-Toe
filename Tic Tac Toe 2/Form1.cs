using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tic_Tac_Toe_2
{
    public partial class Form1 : Form
    {
        String[,] board = new String[3, 3] { { "", "", "" }, 
                                             { "", "", "" },
                                             { "", "", "" } };
        String player = "X";
        String ai = "O";
        String currentPlayer = "X";
        static int count = 0;
        Graphics gObject;

        struct CompMove
        {
            public int i;
            public int j;
        };
        
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            renderBoard();
        }
        private void renderBoard()
        {
            gObject = panel1.CreateGraphics();
            gObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Brush bg = new SolidBrush(Color.White);
            Pen lines = new Pen(Color.Black, 3);

            gObject.FillRectangle(bg, new Rectangle(0, 0, panel1.Width, panel1.Height));


            gObject.DrawLine(lines, 121, 0, 121, 375);
            gObject.DrawLine(lines, 242, 0, 242, 375);

            gObject.DrawLine(lines, 0, 125, 363, 125);
            gObject.DrawLine(lines, 0, 250, 363, 250);

            bg.Dispose();
            lines.Dispose();
            gObject.Dispose();
        }
        private void playerMove(double locX,double locY)
        {
            gObject = panel1.CreateGraphics();
            gObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen lines = new Pen(Color.Black,3);

            var w = panel1.Width / 3;
            var h = panel1.Height / 3;
            
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    var x = j * w;
                    var y = i * h;
                    if(locX > x && locY > y && locX < (x+w) && locY < (y+h) && board[i,j] == "")
                    {
                        board[i, j] = currentPlayer;

                        gObject.DrawLine(lines, x + 20, y + 20, x + w - 30, y + h - 30);
                        gObject.DrawLine(lines, x + w - 30, y + 20, x + 20, y + h - 30);
                        currentPlayer = ai;

                        count++;
                    }
                }
            }
            lines.Dispose();
            gObject.Dispose();
        }

        private void computerMove()
        {
            gObject = panel1.CreateGraphics();
            Pen lines = new Pen(Color.Black, 3);
            gObject.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var bestScore = int.MinValue;
            var bestMove = new CompMove();
            var alpha = int.MinValue;
            var beta = int.MaxValue;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(board[i,j] == "")
                    {
                        board[i, j] = ai;
                        int score = minimax(0, false, alpha, beta);
                        board[i, j] = "";
                        if(score > bestScore)
                        {
                            bestScore = score;
                            bestMove.i = i;
                            bestMove.j = j;
                        }
                    }
                }
            }
            board[bestMove.i, bestMove.j] = ai;
            currentPlayer = player;
            count++;
            var w = panel1.Width / 3;
            var h = panel1.Height / 3;
            var x = bestMove.j * w;
            var y = bestMove.i * h;
            gObject.DrawEllipse(lines, new Rectangle(x + 20, y + 20, w - 40, h - 40));

            lines.Dispose();
            gObject.Dispose();
        }
        private Boolean isBoardFull()
        {
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                        return false;
                }
            }
            return true;
        }
        private int minimax(int depth, Boolean isMaximizing, int alpha, int beta)
        {
            var res = checkWinner();
            int score;
            int bestScore = 0;
            if (res == "X")
                return -1;
            if (res == "O")
                return 1;
            if (isBoardFull() == true)
                return 0;

            if(isMaximizing)
            {
                bestScore = int.MinValue;
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == "")
                        {
                            board[i, j] = ai;
                            score = minimax(depth + 1, false, alpha, beta);
                            board[i, j] = "";
                            bestScore = Math.Max(score, bestScore);
                            alpha = Math.Max(alpha, bestScore);
                            if (beta <= alpha)
                                break;
                        }
                    }
                }
                return bestScore;
            }
            else
            {
                bestScore = int.MaxValue;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board[i, j] == "")
                        {
                            board[i, j] = player;
                            score = minimax(depth + 1, true, alpha, beta);
                            board[i, j] = "";
                            bestScore = Math.Min(score, bestScore);
                            beta = Math.Min(beta, bestScore);
                            if (beta <= alpha)
                                break;
                        }
                    }
                }
                return bestScore;
            }
        }
        private void newGame()
        {
            panel1.Invalidate();
            count = 0;
            currentPlayer = player;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = "";
                }
            }
        }
        private String checkWinner()
        {
            String res = null;
            // horizontal
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i,2] != "")
                {
                    res = board[i, 2];
                }
            }
            // vertical
            for (int i = 0; i < 3; i++)
            {
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[2, i] != "")
                {
                    res = board[2, i];
                }
            }
            // diagonal
            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[2, 2] != "")
            {
                res = board[2, 2];
            }
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[2, 0] != "")
            {
                res = board[2, 0];
            }
            // if it is draw
            if(count == 9)
            {
                res = "Tie";  
            }
            return res;
        }
        private void PrintIfGameOver(String res)
        {
            string title;
            string message = "Press yes to play new game or no to exit game";
            if (res == player)
            {
                title = res + " wins!";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    newGame();
                }
                else
                {
                    Application.Exit();
                }
            }
            else if(res == ai)
            {
                title = res + " wins!";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    newGame();
                }
                else
                {
                    Application.Exit();
                }
            }
            else if(res == "Tie")
            {
                title = "Tie!";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    newGame();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (currentPlayer == player)
            {
                playerMove(e.X,e.Y);
                var result = checkWinner();
                if(result != null)
                    PrintIfGameOver(result);
            }

            if (currentPlayer == ai)
            {
                computerMove();
                var result = checkWinner();
                if (result != null)
                    PrintIfGameOver(result);
            }
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Application.Exit();
        }
    }
}