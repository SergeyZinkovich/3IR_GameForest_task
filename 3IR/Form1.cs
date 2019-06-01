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
        List<List<int>> map = new List<List<int>>();
        Graphics g;
        Pair<int, int> selected_item = new Pair<int, int>(-1, -1);

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            map = Generate_map(8, 8);
            Draw_field(map);
            Annihilate(ref map);
            Drop_elements(ref map);
            Draw_field(map);
            Regenerate_removed_items(ref map);
            Draw_field(map);
            int a = 1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < 9; i++)
            {
                e.Graphics.DrawLine(pen, 194, 44 + i*64, 706, 44 + i*64);
            }
            for (int j = 0; j < 9; j++)
            {
                e.Graphics.DrawLine(pen, 194 + j*64, 44, 194 + j*64, 556);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Location.X >= 194) && (e.Location.X <= 642) && (e.Location.Y >= 44) && (e.Location.Y <= 492))
            {
                int j = (e.Location.X - 194) / 64;
                int i = (e.Location.Y - 44) / 64;
                if (((i == selected_item.first) && (Math.Abs(j - selected_item.second) == 1)) || ((j == selected_item.second) && (Math.Abs(i - selected_item.first) == 1)))
                {
                    Turn();
                }
                else
                {
                    selected_item.second = j;
                    selected_item.first = i;
                }
                Draw_field(map);
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

        public void Turn()
        {

        }

        public List<List<int>> Generate_map(int n, int m)
        {
            Random rnd = new Random();
            List<List<int>> arr = new List<List<int>>();
            for (int i = 0; i < n; i++)
            {
                arr.Add(new List<int>());
                for(int j = 0; j < m; j++){
                    arr[i].Add(rnd.Next() % 5);
                }
            }
            return arr;
        }

        public void Regenerate_removed_items(ref List<List<int>> arr)
        {
            Random rnd = new Random();
            for (int i = 0; i < arr.Count; i++)
            {
                for (int j = 0; j < arr[0].Count; j++)
                {
                    if (arr[i][j] == -1)
                    {
                        arr[i][j] = rnd.Next() % 5;
                    }
                }
            }
        }

        public void Annihilate(ref List<List<int>> arr)
        {
            List<Pair<int, int>> delete_marks = new List<Pair<int, int>>();
            bool in_seq = false;
            for (int i = 0; i < arr.Count(); i++)
            {
                for (int j = 2; j < arr[0].Count(); j++)
                {
                    if ((arr[i][j] == arr[i][j - 1]) && (arr[i][j] == arr[i][j - 2]))
                    {
                        if (!in_seq)
                        {
                            in_seq = true;
                            delete_marks.Add(new Pair<int, int>(i, j));
                            delete_marks.Add(new Pair<int, int>(i, j - 1));
                            delete_marks.Add(new Pair<int, int>(i, j - 2));
                        }
                        else
                        {
                            delete_marks.Add(new Pair<int, int>(i, j));
                        }
                    }
                    else
                    {
                        in_seq = false;
                    }
                }
            }

            in_seq = false;
            for (int i = 0; i < arr.Count(); i++)
            {
                for (int j = 2; j < arr[0].Count(); j++)
                {
                    if ((arr[j][i] == arr[j - 1][i]) && (arr[j][i] == arr[j - 2][i]))
                    {
                        if (!in_seq)
                        {
                            in_seq = true;
                            delete_marks.Add(new Pair<int, int>(j, i));
                            delete_marks.Add(new Pair<int, int>(j - 1, i));
                            delete_marks.Add(new Pair<int, int>(j - 2, i));
                        }
                        else
                        {
                            delete_marks.Add(new Pair<int, int>(i, j));
                        }
                    }
                    else
                    {
                        in_seq = false;
                    }
                }
            }

            foreach (Pair<int, int> pair in delete_marks)
            {
                arr[pair.first][pair.second] = -1;
            }
        }

        public void Drop_elements(ref List<List<int>> field)
        {
            for (int i = 0; i < field.Count; i++)
            {
                for (int j = field[0].Count - 1; j >= 0 ; j--)
                {
                    if (field[j][i] != -1) {
                        int k = j;
                        while ((k + 1 < field.Count) && (field[k + 1][i] == -1))
                        {
                            int c = field[k][i];
                            field[k][i] = field[k + 1][i];
                            field[k + 1][i] = c;
                            k++;
                        }
                    }
                }
            }
        }
    }

    public class Pair<T, G>
    {
        public T first;
        public G second;

        public Pair(T first, G second)
        {
            this.first = first;
            this.second = second;
        }

    }
}
