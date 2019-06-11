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
    public partial class Form1 : System.Windows.Forms.Form
    {
        private const int gameDuration = 60;
        private const int height = 8, width = 8;
        private const int gemesCount = 5;
        private int gameTimeRest;
        private Graphics g;
        private Pair<int, int> selectedItem, swapItem;
        private bool turnInProgress;
        private Engine engine;
        private Button menuButton;
        private AnimationHelper animator;

        public Form1()
        {
            InitializeComponent();
            menuButton = new Button();
            menuButton.Location = new Point(395, 290);
            menuButton.Enabled = false;
            menuButton.Hide();
            this.Controls.Add(menuButton);
            ShowMenu();
        }

        public void ShowMenu()
        {
            menuButton.Enabled = true;
            menuButton.Show();
            menuButton.Text = "Play";
            menuButton.Width = 150;
            menuButton.Height = 80;
            menuButton.Click += PlayButtonClick;
        }

        public void PlayButtonClick(object sender, EventArgs e)
        {
            StartGame();
        }

        public void StartGame()
        {
            menuButton.Click -= PlayButtonClick;
            menuButton.Hide();
            menuButton.Enabled = false;

            pictureBox.Enabled = true;
            timeLabel.Enabled = true;
            scoreLabel.Enabled = true;
            timeLabel.Text = gameDuration.ToString();
            timeLabel.Show();
            scoreLabel.Location = new Point(755, 90);
            scoreLabel.Text = "Score: 0";
            scoreLabel.Show();
            pictureBox.Show();
            drawTimer.Enabled = true;
            gameDurationTimer.Enabled = true;

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            g = Graphics.FromImage(pictureBox.Image);
            engine = new Engine(width, height, gemesCount);
            animator = new AnimationHelper(width, height);
            gameTimeRest = gameDuration;
            selectedItem = new Pair<int, int>(-1, -1);
            swapItem = new Pair<int, int>(-1, -1);
            turnInProgress = false;
        }

        public void EndGame()
        {
            pictureBox.Enabled = false;
            pictureBox.Hide();
            timeLabel.Enabled = false;
            scoreLabel.Location = new Point(395, 230);
            timeLabel.Hide();
            drawTimer.Enabled = false;
            gameDurationTimer.Enabled = false;

            menuButton.Enabled = true;
            menuButton.Show();
            menuButton.Text = "Ok";
            menuButton.Width = 150;
            menuButton.Height = 80;
            menuButton.Click += EndButtonClick;
        }

        public void EndButtonClick(object sender, EventArgs e)
        {
            CloseEndgame();
            ShowMenu();
        }

        public void CloseEndgame()
        {
            animator.StopAnimation();
            menuButton.Enabled = false;
            menuButton.Hide();
            scoreLabel.Enabled = false;
            scoreLabel.Hide();
        }

        private void DrawTimerTick(object sender, EventArgs e)
        {
            animator.IterationInc();
            DrawField(engine.GetMap());
        }

        private void PictureBoxMouseClick(object sender, MouseEventArgs e)
        {
            if (animator.IsAnimationSet())
            {
                return;
            }
            if ((e.Location.X >= 194) && (e.Location.X <= 706) && (e.Location.Y >= 44) && (e.Location.Y <= 556))
            {
                int j = (e.Location.X - 194) / 64;
                int i = (e.Location.Y - 44) / 64;
                if (((i == selectedItem.first) && (Math.Abs(j - selectedItem.second) == 1)) || ((j == selectedItem.second) && (Math.Abs(i - selectedItem.first) == 1)))
                {
                    turnInProgress = true;
                    swapItem.first = i;
                    swapItem.second = j;
                    animator.InitSwapAnimation(selectedItem.first, selectedItem.second, i, j);
                    animator.StartAnimation();
                }
                else
                {
                    selectedItem.second = j;
                    selectedItem.first = i;
                }
            }
        }

        public void Turn()
        {
            engine.SwapItems(selectedItem.first, selectedItem.second, swapItem.first, swapItem.second);
            if (!Annihilate())
            {
                engine.SwapItems(selectedItem.first, selectedItem.second, swapItem.first, swapItem.second);
                animator.InitReversSwapAnimation(selectedItem.first, selectedItem.second,
                    swapItem.first, swapItem.second);
                animator.StartAnimation();
            }
            selectedItem.second = -1;
            selectedItem.first = -1;
            turnInProgress = false;
        }

        public bool Annihilate()
        {
            if(engine.Annihilate()){
                animator.InitGemsFallAnimation(engine.DropElements());
                animator.AddMissingItemsToFallAnimation(engine.GetMap());
                engine.RegenerateAnnihilatedItems();
                animator.StartAnimation();
                return true;
            }
            return false;
        }

        public void DrawField(List<List<int>> field)
        {
            if (turnInProgress && !animator.IsAnimationSet())
            {
                Turn();
            }
            else if (!animator.IsAnimationSet()) {
                Annihilate();
            }
            g.DrawImage((Image)Properties.Resources.background, 0, 0, 900, 600);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (field[i][j] != -1)
                    {
                        String name = "diamond_" + field[i][j].ToString();
                        Image image = new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(name, Properties.Resources.Culture));
                        g.DrawImage(image, 194 + animator.GetCoordinates(i, j).second,
                            44 + animator.GetCoordinates(i, j).first);
                        if ((i == selectedItem.first) && (j == selectedItem.second) && !animator.IsAnimationSet())
                        {
                            Pen pen = new Pen(Color.Red, 3);
                            g.DrawRectangle(pen, 194 + j * 64, 44 + i * 64, 64, 64);
                        }
                    }
                }
            }
            scoreLabel.Text = "Score: " + engine.GetScore().ToString();
            pictureBox.Invalidate();
        }

        private void GameDurationTimerTick(object sender, EventArgs e)
        {
            gameTimeRest--;
            if (gameTimeRest == 0)
            {
                EndGame();
            }
            timeLabel.Text = gameTimeRest.ToString();
        }
    }
}
