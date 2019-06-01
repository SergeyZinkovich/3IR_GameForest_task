using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3IR
{
    public partial class Form1 : Form
    {
        Graphics g;
        Pair<int, int> selected_item = new Pair<int, int>(-1, -1);
        Engine engine;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            engine = new Engine(8, 8);
            Draw_field(engine.Get_map());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < 9; i++)
            {
                e.Graphics.DrawLine(pen, 194, 44 + i * 64, 706, 44 + i * 64);
            }
            for (int j = 0; j < 9; j++)
            {
                e.Graphics.DrawLine(pen, 194 + j * 64, 44, 194 + j * 64, 556);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Location.X >= 194) && (e.Location.X <= 706) && (e.Location.Y >= 44) && (e.Location.Y <= 556))
            {
                int j = (e.Location.X - 194) / 64;
                int i = (e.Location.Y - 44) / 64;
                if (((i == selected_item.first) && (Math.Abs(j - selected_item.second) == 1)) || ((j == selected_item.second) && (Math.Abs(i - selected_item.first) == 1)))
                {
                    engine.Turn(selected_item.first, selected_item.second, i, j);
                    selected_item.second = -1;
                    selected_item.first = -1;
                }
                else
                {
                    selected_item.second = j;
                    selected_item.first = i;
                }
                Draw_field(engine.Get_map());
            }
        }

        public void Draw_field(List<List<int>> field)
        {
            g.Clear(Color.White);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (field[i][j] != -1)
                    {
                        String name = "diamond_" + field[i][j].ToString();
                        Image image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(name, Properties.Resources.Culture));
                        g.DrawImage(image, 194 + j * 64, 44 + i * 64);
                        if ((i == selected_item.first) && (j == selected_item.second))
                        {
                            Pen pen = new Pen(Color.Red, 3);
                            g.DrawRectangle(pen, 194 + j * 64, 44 + i * 64, 64, 64);
                        }
                    }
                }
            }
            pictureBox1.Invalidate();
        }
    }
}
