﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace _3IR
{
    public partial class Form1 : Form
    {
        private int game_time_rest;
        private Graphics g;
        private Pair<int, int> selected_item;
        private Engine engine;
        private Button menuButton;

        public Form1()
        {
            InitializeComponent();
            Show_menu();
        }

        public void Show_menu()
        {
            menuButton = new Button();
            menuButton.Location = new Point(395, 290);
            menuButton.Text = "Play";
            menuButton.Width = 150;
            menuButton.Height = 80;
            menuButton.Click += Play_button_Click;
            //this.Controls.Clear();
            this.Controls.Add(menuButton);
        }

        public void Play_button_Click(object sender, EventArgs e)
        {
            Start_game();
        }

        public void Start_game()
        {
            menuButton.Hide();
            menuButton.Enabled = false;
            pictureBox1.Enabled = true;
            label1.Show();
            pictureBox1.Show();
            timer1.Enabled = true;
            timer2.Enabled = true;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            engine = new Engine(8, 8);
            game_time_rest = 5;
            selected_item = new Pair<int, int>(-1, -1);
        }

        public void End_game()
        {
            pictureBox1.Enabled = false;
            pictureBox1.Hide();
            label1.Hide();
            timer1.Enabled = false;
            timer2.Enabled = false;

            menuButton = new Button();
            menuButton.Location = new Point(395, 290);
            menuButton.Text = "Ok";
            menuButton.Width = 150;
            menuButton.Height = 80;
            menuButton.Click += End_button_Click;
            this.Controls.Add(menuButton);
        }

        public void End_button_Click(object sender, EventArgs e)
        {
            Close_endgame();
            Show_menu();
        }

        public void Close_endgame()
        {
            Animation_helper.Stop_animation();
            menuButton.Enabled = false;
            menuButton.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Animation_helper.Iteration_inc();
            Draw_field(engine.Get_map());
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
            if (Animation_helper.is_Animation_set())
            {
                return;
            }
            if ((e.Location.X >= 194) && (e.Location.X <= 706) && (e.Location.Y >= 44) && (e.Location.Y <= 556))
            {
                int j = (e.Location.X - 194) / 64;
                int i = (e.Location.Y - 44) / 64;
                if (((i == selected_item.first) && (Math.Abs(j - selected_item.second) == 1)) || ((j == selected_item.second) && (Math.Abs(i - selected_item.first) == 1)))
                {
                    Animation_helper.Init_swap_animation(selected_item.first, selected_item.second, i, j);
                    engine.Turn(selected_item.first, selected_item.second, i, j);
                    selected_item.second = -1;
                    selected_item.first = -1;
                }
                else
                {
                    selected_item.second = j;
                    selected_item.first = i;
                }
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
                        g.DrawImage(image, 194 + j * 64 + Animation_helper.Get_swap_coordinates(i, j).second,
                            44 + i * 64 + Animation_helper.Get_swap_coordinates(i, j).first);
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

        private void timer2_Tick(object sender, EventArgs e)
        {
            game_time_rest--;
            if (game_time_rest == 0)
            {
                End_game();
            }
            label1.Text = game_time_rest.ToString();
        }
    }
}
