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
using System.Windows.Media;
using System.Timers;

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

        public MainWindow()
        {
            InitializeComponent();

            running = false;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {



            int val = ((int)Sld1.Value);


            if (val == 0)
            {
                PlacementLabel.Content = "On the Beat";
            }

            if (val < 0)
            {
                PlacementLabel.Content = "Behind by " + (-val * 10) + "ms";

            }

            if (val > 0)
            {
                PlacementLabel.Content = "Ahead by " + (val * 10) + "ms";


            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            Set_BPM(sender, e);

        }


        private System.Timers.Timer timer1;
        public void InitTimer()
        {
            this.Dispatcher.Invoke(() =>
            {
                bpms = BPMbox.Text.ToString();
            });

            timer1 = new System.Timers.Timer();
            timer1.Elapsed += new ElapsedEventHandler(timer1_Tick);
            timer1.Interval = int.Parse(bpms);
            int beats = int.Parse(bpms);
            var delayTask = Task.Delay(beats / 1000); ; // in miliseconds
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            Start_Metronome(sender, e);

        }


        public void Set_BPM(object sender, EventArgs e)
        {



            if (running == true)

            {
                running = false;
                StrtBtn.Content = "GO!";
                return;
            }



            if (running == false)

            {



                while (running == true)
                {
                    running = true;
                    StrtBtn.Content = "STOP!";
                    InitTimer();

                }


            }



        }
            private async void Start_Metronome(object sender, EventArgs e)

            {





                int freq = 800;
                int dur = 10;
                Console.Beep(freq, dur);


            }
        } 

}
