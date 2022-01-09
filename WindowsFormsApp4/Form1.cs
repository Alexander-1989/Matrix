using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            oldLocation = Location;
            normalSize = Size;
            fullSize = Screen.PrimaryScreen.Bounds.Size;
            KeyDown += Form1_KeyDown;
            pictureBox1. MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseDoubleClick += PictureBox1_MouseDoubleClick;
            Font = new Font(Font.FontFamily, charSize);
            ChangeSize();
            timer1.Start();
        }

        Size normalSize, fullSize;
        Point oldLocation, position;
        Graphics graphics = null;
        Random rnd = new Random();
        List<MChar[]> allLines = new List<MChar[]>(400);
        bool isFullSize = true;
        int charSize = 12;
        int delay = 4;
        char[] katakana =
        {
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '゠', 'ァ', 'ア', 'ィ', 'イ', 'ゥ', 'ウ', 'ェ', 'エ',
            'ォ', 'オ', 'カ', 'ガ', 'キ', 'ギ', 'ク', 'グ', 'ケ',
            'ゲ', 'コ', 'ゴ', 'サ', 'ザ', 'シ', 'ジ', 'ス', 'ズ',
            'セ', 'ゼ', 'ソ', 'ゾ', 'タ', 'ダ', 'チ', 'ヂ', 'ッ',
            'ツ', 'ヅ', 'テ', 'デ', 'ト', 'ド', 'ナ', 'ニ', 'ヌ',
            'ネ', 'ノ', 'ハ', 'バ', 'パ', 'ヒ', 'ビ', 'ピ', 'フ',
            'ブ', 'プ', 'ヘ', 'ベ', 'ペ', 'ホ', 'ボ', 'ポ', 'マ',
            'ミ', 'ム', 'メ', 'モ', 'ャ', 'ヤ', 'ュ', 'ユ', 'ョ',
            'ヨ', 'ラ', 'リ', 'ル', 'レ', 'ロ', 'ヮ', 'ワ', 'ヰ',
            'ヱ', 'ヲ',  'ン', 'ヴ', 'ヵ', 'ヶ', 'ヷ', 'ヸ', 'ヹ',
            'ヺ', '・', 'ー', 'ヽ', 'ヾ', 'ヿ'
        };

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!isFullSize && e.Button == MouseButtons.Left)
            {
                position = e.Location;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && !isFullSize)
            {
                int dx = e.Location.X - position.X;
                int dy = e.Location.Y - position.Y;
                Location = new Point(Location.X + dx, Location.Y + dy);
            }
        }

        private void PictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ChangeSize();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.F) || (e.Alt && e.KeyCode == Keys.Enter))
            {
                ChangeSize();
            }
            if (e.KeyData == Keys.Escape)
            {
                Application.Exit();
            }
        }

        private void ChangeSize()
        {
            if (!isFullSize)
            {
                isFullSize = true;
                oldLocation = Location;
                Location = new Point(0, 0);
                Size = fullSize;
                NativeMethods.ShowCursor(false);
            }
            else
            {
                isFullSize = false;
                Location = oldLocation;
                Size = normalSize;
                NativeMethods.ShowCursor(true);
            }

            pictureBox1.Image?.Dispose();
            graphics?.Dispose();

            pictureBox1.Image = new Bitmap(Width, Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
        }

        private void Draw(MChar ch)
        {
            int alpha = ch.GetAlpha();
            if (alpha > 0)
            {
                Color color = Color.SpringGreen;
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(alpha, color)))
                {
                    graphics.DrawString($"{ch.Ch}", Font, brush, ch.X, ch.Y);
                }
            }
        }

        private MChar[] InitLine()
        {
            MChar[] cline = new MChar[Height / charSize];
            int dSize = 2 * charSize;
            int x = dSize * rnd.Next(Width / dSize);
            int y = rnd.Next(-Height, 0);

            for (int i = 0; i < cline.Length; i++)
            {
                char ch = katakana[rnd.Next(katakana.Length)];
                cline[i] = new MChar(ch, x, y + i * (charSize + 4), i * delay);
            }

            return cline;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            graphics.Clear(Color.Black);

            for (int x = 0; x < allLines.Count; x++)
            {
                for (int y = 0; y < allLines[x].Length; y++)
                {
                    Draw(allLines[x][y]);
                    allLines[x][y].Tick();
                }

                if (allLines[x].Last().IsUsed())
                {
                    allLines[x] = InitLine();
                }

                pictureBox1.Invalidate();
            }

            if (allLines.Count < 300)
            {
                allLines.Add(InitLine());
            }
        }
    }
}