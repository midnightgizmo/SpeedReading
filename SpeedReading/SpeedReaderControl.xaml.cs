using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedReading
{
    /// <summary>
    /// Interaction logic for SpeedReaderControl.xaml
    /// </summary>
    public partial class SpeedReaderControl : UserControl
    {
        string someText = "default text. The End.";
        string[] splitText;
        int Position = 0;

        System.Windows.Threading.DispatcherTimer timer, blankSpaceTimer, firstWordTimer;

        TimeSpan NormalTime, LongWord, DelayedTime, BlackSpaceTime;

        public SpeedReaderControl()
        {


            //NormalTime = new TimeSpan(0, 0, 0, 0, 300);
            //DelayedTime = new TimeSpan(0, 0, 0, 0, 600);
            NormalTime = new TimeSpan(0, 0, 0, 0, 200);
            LongWord = new TimeSpan(0, 0, 0, 0, 300);
            DelayedTime = new TimeSpan(0, 0, 0, 0, 400);
            BlackSpaceTime = new TimeSpan(0, 0, 0, 0, 300);

            string fileName;



            fileName = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            someText = System.IO.File.ReadAllText(fileName + "\\TextToRead.txt").Replace('\r', ' ').Replace('\n', ' ');
            splitText = someText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            //splitText = someText.Split(new char[]{' '});


            blankSpaceTimer = new System.Windows.Threading.DispatcherTimer();
            blankSpaceTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            blankSpaceTimer.Tick += blankSpaceTimer_Tick;

            firstWordTimer = new System.Windows.Threading.DispatcherTimer();
            firstWordTimer.Interval = DelayedTime;
            firstWordTimer.Tick += firstWordTimer_Tick;

            timer = new System.Windows.Threading.DispatcherTimer();


            timer.Interval = NormalTime;
            timer.Tick += timer_Tick;

            InitializeComponent();
            
            
        }

        public void Start()
        {
            firstWordTimer.Start();
        }





        void timer_Tick(object sender, EventArgs e)
        {
            timer.Interval = NormalTime;

            if (Position >= splitText.Length)
            {
                //Position = 0;
                timer.Stop();
                return;
            }



            textDrawer.Text = splitText[Position++];

            string lastLetterOfCurrentWord = splitText[Position - 1].Substring(splitText[Position - 1].Length - 1);
            //if (splitText[Position -1].Substring(splitText[Position -1].Length - 1) == ".")
            if (IsPunctuation(lastLetterOfCurrentWord))
            {
                timer.Stop();
                blankSpaceTimer.Start();
            }
            else if(textDrawer.Text.Length > 8)
            {
                switch(textDrawer.Text.Length)
                {
                    case 9:
                    case 10:
                    case 11:

                        timer.Interval = LongWord;
                        break;

                    default:

                        timer.Interval = LongWord.Add(new TimeSpan(0, 0, 0, 0, 100));
                        break;

                }
                
            }
        }

        private bool IsPunctuation(string letter)
        {
            switch (letter)
            {
                case ".":
                    return true;

                case ",":
                    return true;

                case ";":
                    return true;

                default:
                    return false;
            }
        }


        void firstWordTimer_Tick(object sender, EventArgs e)
        {
            firstWordTimer.Stop();

            if (Position >= splitText.Length)
            {
                return;
            }

            textDrawer.Text = splitText[Position++];

            timer.Interval = DelayedTime;
            timer.Start();
        }

        void blankSpaceTimer_Tick(object sender, EventArgs e)
        {
            blankSpaceTimer.Stop();
            //textDrawer.Text = " ";
            if (textDrawer.Text.EndsWith("."))
            {
                Task.Delay(500).ContinueWith(t =>
                {
                    textDrawer.Text = " ";
                    firstWordTimer.Interval = NormalTime;
                    firstWordTimer.Start();
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                firstWordTimer.Interval = NormalTime;
                firstWordTimer.Start();
            }
        }


        private void cbSpeedSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int speedSetting = 200;
            switch (cbSpeedSetting.SelectedIndex)
            {
                //Slow
                case 0:

                    speedSetting = 400;
                    break;

                case 1:

                    speedSetting = 300;
                    break;

                case 3:

                    speedSetting = 200;
                    break;
            }

            NormalTime = new TimeSpan(0, 0, 0, 0, speedSetting);
            LongWord = new TimeSpan(0, 0, 0, 0, speedSetting + 100);
            DelayedTime = new TimeSpan(0, 0, 0, 0, speedSetting + 200);
            BlackSpaceTime = new TimeSpan(0, 0, 0, 0, speedSetting + 100);

            blankSpaceTimer.Interval = BlackSpaceTime;
            firstWordTimer.Interval = DelayedTime;
            timer.Interval = NormalTime;
        }

        private void Pause()
        {
            blankSpaceTimer.Stop();
            firstWordTimer.Stop();
            timer.Stop();


            //splitText
            // find the begining of the current sentance and set that as the starting position
            for(int i = Position; i >=0; i--)
            {
                if(splitText[i].EndsWith("."))
                {
                    Position = i + 1;
                    break;
                }
            }


        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Pause();
        }
    }
}
