using System;
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
        private const int game_duration = 60;
        private const int height = 8, width = 8;
        private int game_time_rest;
        private Graphics g;
        private Pair<int, int> selected_item, swap_item;
        private bool turn_in_progress;
        private Engine engine;
        private Button menuButton;
        private Animation_helper animator;

        public Form1()
        {
            InitializeComponent();
            Show_menu();
        }

        public void Show_menu()
        {
            menuButton = new Button();  //TODO: убрать пересоздание
            menuButton.Location = new Point(395, 290);
            menuButton.Text = "Play";
            menuButton.Width = 150;
            menuButton.Height = 80;
            menuButton.Click += Play_button_Click;
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
            label1.Enabled = true;
            label2.Enabled = true;
            label1.Text = game_duration.ToString();
            label1.Show();
            label2.Location = new Point(755, 90);
            label2.Text = "Score: 0";
            label2.Show();
            pictureBox1.Show();
            timer1.Enabled = true;
            timer2.Enabled = true;

            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            engine = new Engine(8, 8);
            animator = new Animation_helper(width, height);
            game_time_rest = game_duration;
            selected_item = new Pair<int, int>(-1, -1);
            swap_item = new Pair<int, int>(-1, -1);
            turn_in_progress = false;
        }

        public void End_game()
        {
            pictureBox1.Enabled = false;
            pictureBox1.Hide();
            label1.Enabled = false;
            label2.Location = new Point(395, 230);
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
            animator.Stop_animation();
            menuButton.Enabled = false;
            menuButton.Hide();
            label2.Enabled = false;
            label2.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            animator.Iteration_inc();
            Draw_field(engine.Get_map());
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (animator.is_Animation_set())
            {
                return;
            }
            if ((e.Location.X >= 194) && (e.Location.X <= 706) && (e.Location.Y >= 44) && (e.Location.Y <= 556))
            {
                int j = (e.Location.X - 194) / 64;
                int i = (e.Location.Y - 44) / 64;
                if (((i == selected_item.first) && (Math.Abs(j - selected_item.second) == 1)) || ((j == selected_item.second) && (Math.Abs(i - selected_item.first) == 1)))
                {
                    turn_in_progress = true;
                    swap_item.first = i;
                    swap_item.second = j;
                    animator.Init_swap_animation(selected_item.first, selected_item.second, i, j);
                    animator.Start_animation();
                }
                else
                {
                    selected_item.second = j;
                    selected_item.first = i;
                }
            }
        }

        public void Turn()
        {
            engine.Swap_items(selected_item.first, selected_item.second, swap_item.first, swap_item.second);
            if (!Annihilate())
            {
                engine.Swap_items(selected_item.first, selected_item.second, swap_item.first, swap_item.second);
                animator.Init_revers_swap_animation(selected_item.first, selected_item.second,
                    swap_item.first, swap_item.second);
                animator.Start_animation();
            }
            selected_item.second = -1;
            selected_item.first = -1;
            turn_in_progress = false;
        }

        public bool Annihilate()
        {
            if(engine.Annihilate()){
                animator.Init_gems_fall_animation(engine.Drop_elements());
                animator.Add_missing_items_to_fall_animation(engine.Get_map());
                engine.Regenerate_annihilated_items();
                animator.Start_animation();
                return true;
            }
            return false;
        }

        public void Draw_field(List<List<int>> field)
        {
            if (turn_in_progress && !animator.is_Animation_set())
            {
                Turn();
            }
            else if (!animator.is_Animation_set()) {
                Annihilate();
            }
            g.DrawImage((Image)Properties.Resources.background, 0, 0, 900, 600);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (field[i][j] != -1)
                    {
                        String name = "diamond_" + field[i][j].ToString();
                        Image image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(name, Properties.Resources.Culture));
                        g.DrawImage(image, 194 + animator.Get_coordinates(i, j).second,
                            44 + animator.Get_coordinates(i, j).first);
                        if ((i == selected_item.first) && (j == selected_item.second) && !animator.is_Animation_set())
                        {
                            Pen pen = new Pen(Color.Red, 3);
                            g.DrawRectangle(pen, 194 + j * 64, 44 + i * 64, 64, 64);
                        }
                    }
                }
            }
            label2.Text = "Score: " + engine.Get_score().ToString();
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
