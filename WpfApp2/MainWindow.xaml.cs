using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using SharpDX.DirectSound;
using System.Media;
using System.Text.RegularExpressions;


namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string slidervalue;
        public static bool is_verifying { get; set; }

        public bool running;
        public string bpms;

        public int behindms;
        public int aheadms;


        public string aheadsound;
        public string behindsound;
        public string clicksound;

        public string origvaltoparse;

        public System.Timers.Timer timer1;

        public SoundPlayer Player;
        private System.IO.Stream path;

        //public var path;

        public MainWindow()
        {
            InitializeComponent();

            running = false;
            aheadms = 0;
            behindms = 0;


            clicksound = "Click-loud.wav";
            behindsound = "ClickBehind-normal.wav";
            aheadsound = "ClickAhead-normal.wav";

        }

       
        public void loadSoundAsync()
        {
           System.Media.SoundPlayer Player = new System.Media.SoundPlayer();


            
            var path = Resource1.Click_loud;
                //"E:\\" + clicksound;
           Player.Stream = path;
           Player.Play();

        }

        public void loadSoundAsyncAhead()
        {

            System.Media.SoundPlayer Player = new System.Media.SoundPlayer();


            this.Dispatcher.Invoke(() =>
            {
                if (Check1.IsChecked == true)
                { Player.Stream = Resource1.ClickAhead_quiet; }
                if (Check2.IsChecked == true)
                { Player.Stream = Resource1.ClickAhead_loud; }
                if (Check1.IsChecked == false && Check2.IsChecked == false)
                { Player.Stream = Resource1.ClickAhead_normal; }
                Player.Play();
            });
        }

        public void loadSoundAsyncBehind()
        {

            System.Media.SoundPlayer Player = new System.Media.SoundPlayer();

            this.Dispatcher.Invoke(() =>
            {
                if (Check1.IsChecked == true)
                { Player.Stream = Resource1.ClickBehind_quiet; }
                if (Check2.IsChecked == true)
                { Player.Stream = Resource1.ClickBehind_loud; }
                if (Check1.IsChecked == false && Check2.IsChecked == false)
                { Player.Stream = Resource1.ClickBehind_normal; }
                Player.Play();
            });
        }



        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {



            int val = ((int)Sld1.Value);


            if (val == 0)
            {
                PlacementLabel.Content = "Silent";
                behindms = 0;
                aheadms = 0;
            }

            if (val < 0)
            {
                PlacementLabel.Content = "Behind by " + (-val * 25) + "ms";
                behindms = -val*25;
            }

            if (val > 0)
            {
                PlacementLabel.Content = "Ahead by " + (val * 25) + "ms";
                aheadms = val*25;

            }

        }



        private void Button_Click(object sender, RoutedEventArgs e)

        {
            SetMinMax(sender, e);
            Set_BPM(sender, e);

        }


       async void InitTimer()
        {
            this.Dispatcher.Invoke(() =>
            {
                if (BPMbox.Text.ToString() == "")
                { bpms = "40"; BPMbox.Text = "40"; }

                if (BPMbox.Text.ToString() != "")
                { bpms = BPMbox.Text.ToString(); }


            });

            timer1 = new System.Timers.Timer();
            timer1.Elapsed += new ElapsedEventHandler(timer1_Tick);
            int beats = int.Parse(bpms);
            int bpm = 60000 / beats;
            timer1.Interval = bpm;
            timer1.Start();

        }

      



        async void timer1_Tick(object sender, EventArgs e)
        {

            Start_Metronome(sender, e);

        }



        public async void Set_BPM(object sender, EventArgs e)
        {

            if (running == true)

            {
                running = false;
                StrtBtn.Content = "GO!";
                timer1.Dispose();
                return;
            }



            else if (running == false)

            {
                running = true;
                StrtBtn.Content = "STOP!";
                InitTimer();
                return;
            }


            

        }

        public async void Start_Metronome(object sender, EventArgs e)

        {
           
            if (aheadms > 0)
            {
                behindms = 0;
                loadSoundAsyncAhead();
                Thread.Sleep(aheadms);
                loadSoundAsync();
            }



            if (behindms > 0)
            {
                aheadms = 0;
                loadSoundAsync();
                Thread.Sleep(behindms);
                loadSoundAsyncBehind();     
            }

            if (behindms == 0 && aheadms == 0)
            {

                loadSoundAsync();
            }


        }


        private void MakeInt(object sender, TextCompositionEventArgs e)
        {

            if (running == true)
            {
                Set_BPM(sender, e);
            }

            BPMbox.MaxLength = 3;
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);

        }


        private void SetMinMax(object Sender, EventArgs e)
        {
            string origval;

            if (BPMbox.Text.ToString() == "")
            {origvaltoparse = "40"; }

            if (BPMbox.Text.ToString() != "")
            {origvaltoparse = BPMbox.Text.ToString();}

            int value = int.Parse(origvaltoparse);
            string empt = "";

            if (value < 40)
                {
                BPMbox.Text = "40";
                }

            if (value > 240)
                {
                BPMbox.Text = "240";
                }
        }


        private void MakeInt(object sender, TextChangedEventArgs e)
        {
            //BPMbox.MaxLength = 3;
            //Regex regex = new Regex("[^0-9]+");
            //e.Handled = regex.IsMatch(BPMbox.Text);

        }



        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

            if (Check1.IsChecked == true)
            {
                Check2.IsChecked = false;
                //aheadsound = path
                //behindsound = "ClickBehind-quiet.wav";
            }

            if (Check1.IsChecked == false && Check2.IsChecked == false)
            {
                //aheadsound = "ClickAhead-normal.wav";
                //behindsound = "ClickBehind-normal.wav";
            }

        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {        

            if (Check2.IsChecked == true)
            {
                Check1.IsChecked = false;
                //aheadsound = "ClickAhead-loud.wav";
                //behindsound = "ClickBehind-loud.wav";
            }

            if (Check1.IsChecked == false && Check2.IsChecked == false)
            {
                //aheadsound = "ClickAhead-normal.wav";
                //behindsound = "ClickBehind-normal.wav";
            }

        }
}

   
} 


