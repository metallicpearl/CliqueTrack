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
using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.Dsp;
using NAudio.MediaFoundation;
using Microsoft.Win32;




using System.IO;


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

        public string primarypath;
        public string secondarypath;    

        public System.Timers.Timer timer1;

        public SoundPlayer Player;

        public float vol;
        public float vol2;
        public float alt;

        public static float pan1;
        public static float pan2;

        static public int fltype;
        static public int fltval;
        static public int fltval2;

        public static int notelength;
        static public int currentval;
        static public int divider;

        public Color Green = Color.FromArgb(255,161,242,56);
        public Color Orange = Color.FromArgb(255,255,157,7);
        public Color Blue = Color.FromArgb(255, 0, 220, 230);






        //public var path;

        public MainWindow()
        {

            //running = false;
            //aheadms = 0;
            //behindms = 0;
            //vol = 0.5F;
            //vol2 = 0.5F;
            //fltval = 3000;
            //fltval2 = 5000;
            //alt = 1;


            InitializeComponent();
            invertpan.IsEnabled = false;
            hardpan.IsEnabled = false;
            AltSec.IsEnabled = false;


            primarypath = null;




        }


        public void loadSoundAsync()
        {

         
        

            WaveFormat waveFormat = new WaveFormat(44100, 1);



            RawSourceWaveStream Aud = new RawSourceWaveStream(Resource5.shortNewClkNormaltreated2, waveFormat: waveFormat);
            RawSourceWaveStream Aud2 = new RawSourceWaveStream(Resource5.shortNewClkNormal_loudALT2, waveFormat: waveFormat);



            WaveOutEvent woe = new WaveOutEvent();

            var prov = Aud.ToSampleProvider();
            var prov2 = Aud2.ToSampleProvider();
            var panProvider = new PanningSampleProvider(prov);
            var panProvider2 = new PanningSampleProvider(prov2);



            this.Dispatcher.Invoke(() =>
            {

                if (hardpan.IsChecked == true)
                {

                    if (invertpan.IsChecked == true)
                    {

                        pan1 = -1.0f;
                        pan2 = 1.0f;
                    }

                    else
                    {

                        pan1 = 1.0f;
                        pan2 = -1.0f;

                    }



                }

                if (hardpan.IsChecked == false)
                {
                    pan1 = 0.0f;
                    pan2 = 0.0f;
                }




            });

            panProvider.Pan = pan1;
            panProvider2.Pan = pan1;
  


            this.Dispatcher.Invoke(() =>
            {
                if (AltClk.IsChecked == true)
                {
                    alt = 0.9F;
                    woe.Volume = vol/alt;
                    woe.Init(panProvider);
                    woe.Play();
                    

                }

                else

                {
                    alt = 0.8F;
                    woe.Volume = vol/alt;
                    woe.Init(panProvider2);
                    woe.Play();
                    
                }


            });

        }


        public void loadSoundAsyncAhead()
        {

            
           
            
            

            var res = Resource5.shortNewClkAheadtreated2;
            //var extres = Resource4.shortNewClkHigh_Louder3_ext2;
      


            

                this.Dispatcher.Invoke(() =>
                {
                    if (AltSec.IsChecked == true)
                    {
                        res = Resource5.shortNewClkHigh_loudALT2;
                        divider = 100;
                    }

                    else
                    {
                        divider = 100;
                    }


                });
            


            WaveFormat waveFormatAhead = new WaveFormat(44100, 1);

            RawSourceWaveStream Ahd = new RawSourceWaveStream(res, waveFormat: waveFormatAhead);
            //RawSourceWaveStream Prf = new RawSourceWaveStream(primaryread, waveFormat: waveFormatAhead);
            //RawSourceWaveStream AhdExt = new RawSourceWaveStream(extres, waveFormat: waveFormatAhead);

            WaveOutEvent woe = new WaveOutEvent();



          



            this.Dispatcher.Invoke(() =>
            {

               

                var samp1 = Ahd.ToSampleProvider();
                var filter1 = new Waveprovider(samp1, fltval);
                ISampleProvider sampleProvider = filter1 as ISampleProvider;
                var panProvider = new PanningSampleProvider(sampleProvider);
                panProvider.Pan = pan2;

                WaveOut waveOut1 = new WaveOut();
                waveOut1.Init(panProvider);

                if (AltClk.IsChecked == true)
                {
                    alt = 2.5F;
                }
                else
                {
                    alt = 1;
                }

                waveOut1.Volume = vol2/alt;
                waveOut1.Play();
                woe.Dispose();

            

            });



        }

        public void loadSoundAsyncBehind()
        {


        

            var res2 = Resource5.shortNewClkBehindtreated2;

            this.Dispatcher.Invoke(() =>
            {
                if (AltSec.IsChecked == true)
                {
                    res2 = Resource5.shortNewClkLow_loudALT2;
                    divider = 100;
                }

                else
                {
                    divider = 100;
                }


            });
           


            WaveFormat waveFormatBhnd = new WaveFormat(44100, 1);

            RawSourceWaveStream Bhd = new RawSourceWaveStream(res2, waveFormat: waveFormatBhnd);
            //RawSourceWaveStream AhdExt = new RawSourceWaveStream(Resource4.shortNewClkLow_loud_ext2, waveFormat: waveFormatAhead);

            WaveOutEvent woe = new WaveOutEvent();


            this.Dispatcher.Invoke(() =>
            {



                var samp2 = Bhd.ToSampleProvider();
                var filter2 = new Waveprovider2(samp2, fltval2);
                WaveOut waveOut2 = new WaveOut();

                ISampleProvider sampleProvider = filter2 as ISampleProvider;
                var panProvider = new PanningSampleProvider(sampleProvider);
                panProvider.Pan = pan2;
                waveOut2.Init(panProvider);


                if (AltClk.IsChecked == true)
                {
                    alt = 2.5F;
                }
                else
                {
                    alt = 1;
                }

                waveOut2.Volume = vol2;
                waveOut2.Play();
                woe.Dispose();





            });

        }



        public void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            SolidColorBrush green = new SolidColorBrush(Green);
            SolidColorBrush blue = new SolidColorBrush(Blue);
            SolidColorBrush orange = new SolidColorBrush(Orange);

            int val = ((int)Sld1.Value);
            currentval = val;


            if (val == 0)
            {
                PlacementLabel.Content = "Silent";
                behindms = 0;
                aheadms = 0;
                hardpan.IsEnabled = false;
                AltSec.IsEnabled = false;
                pan1 = 0.0f;
                pan2 = 0.0f;
                hardpan.IsChecked = false;
                invertpan.IsChecked = false;
                SecLab.Content = "Secondary Click Volume";
                PrimLab.Content = "Primary Click Volume";
                
                PlacementLabel.Foreground = green;
                
            }

            if (val < 0)
            {
                PlacementLabel.Content = "Behind by " + (-val * 25) + "ms";
                behindms = -val * 25;
                hardpan.IsEnabled = true;
                AltSec.IsEnabled = true;

                if (hardpan.IsChecked == true)
                {

                    if (invertpan.IsChecked == true)
                    {
                        PrimLab.Content = "(Left) Primary Click Volume";
                        SecLab.Content = "(Right) Secondary Click Volume";
                    }
                    if (invertpan.IsChecked == false)
                    {
                        SecLab.Content = "(Left) Secondary Click Volume";
                        PrimLab.Content = "(Right) Primary Click Volume";
                    }
                }

                if (hardpan.IsChecked == false)
                {
                    SecLab.Content = "Secondary Click Volume";
                    PrimLab.Content = "Primary Click Volume";
                }

                PlacementLabel.Foreground = blue;

            }



            if (val > 0)
            {
                PlacementLabel.Content = "Ahead by " + (val * 25) + "ms";
                aheadms = val * 25;
                hardpan.IsEnabled = true;
                AltSec.IsEnabled = true;

                if (hardpan.IsChecked == true)
                {

                    if (invertpan.IsChecked == true)
                    {
                        PrimLab.Content = "(Left) Primary Click Volume";
                        SecLab.Content = "(Right) Secondary Click Volume";
                    }
                    if (invertpan.IsChecked == false)
                    {
                        SecLab.Content = "(Left) Secondary Click Volume";
                        PrimLab.Content = "(Right) Primary Click Volume";
                    }
                }

                if (hardpan.IsChecked == false)
                {
                    SecLab.Content = "Secondary Click Volume";
                    PrimLab.Content = "Primary Click Volume";
                }

                PlacementLabel.Foreground = orange;
            }

            if (running == true)
            {
                timer1.Dispose();
                running = false;
                Set_BPM(sender, e);
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
                
                loadSoundAsyncAhead();
                Thread.Sleep(aheadms);
                loadSoundAsync();
                behindms = 0;
              


            }



            if (behindms > 0)
            {

                loadSoundAsync();
                Thread.Sleep(behindms);
                loadSoundAsyncBehind();
                aheadms = 0;

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
            { origvaltoparse = "40"; }

            if (BPMbox.Text.ToString() != "")
            { origvaltoparse = BPMbox.Text.ToString(); }

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





        private void SecondaryVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float val = ((float)SecondaryVolume.Value);
            vol2 = val;
        }

        private void PrimaryVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            float val = ((float)PrimaryVolume.Value);
            vol = val;

        }


        public class Waveprovider : ISampleProvider
        {




            public ISampleProvider sourceProvider;
            public ISampleProvider outputProvider;
            public int cutOffFreq;
            public int channels;
            public WaveFileReader source;
            BiQuadFilter[] filters;
            //BiQuadFilter[] filters2;



            public Waveprovider(ISampleProvider sourceProvider, int cutOffFreq)
            {
                this.sourceProvider = sourceProvider;
                this.cutOffFreq = cutOffFreq;

                channels = sourceProvider.WaveFormat.Channels;
                filters = new NAudio.Dsp.BiQuadFilter[channels];
                //filters2 = new NAudio.Dsp.BiQuadFilter[channels];
                //WaveFileReader source = new WaveFileReader(Resource4.shortNewClkNormal_loud);
                CreateFilters();
            }

            public void CreateFilters()
            {



                for (int n = 0; n < channels; n++)
                    if (filters[n] == null)
                        filters[n] = NAudio.Dsp.BiQuadFilter.HighPassFilter(44100, cutOffFreq, 1);
                    else
                        filters[n].SetHighPassFilter(44100, cutOffFreq, 1);



            }



            public WaveFormat WaveFormat { get { return sourceProvider.WaveFormat; } }


            public int Read(float[] buffer, int offset, int count)
            {
                int samplesRead = sourceProvider.Read(buffer, offset, count);

                    for (int i = 0; i < samplesRead; i++)
                        buffer[offset + i] = filters[(i % channels)].Transform(buffer[offset + i]);


                //if (fltype == 2)
                //{

                //    for (int i = 0; i < samplesRead; i++)
                //        buffer[offset + i] = filters2[(i % channels)].Transform(buffer[offset + i]);
                //}


               

                return(samplesRead / divider) * notelength;
            }





        }


        public class Waveprovider2 : ISampleProvider
        {




            public ISampleProvider sourceProvider2;
            public ISampleProvider outputProvider;
            public int cutOffFreq2;
            public int channels2;
            public WaveFileReader source2;
            BiQuadFilter[] filters2;
            //BiQuadFilter[] filters2;



            public Waveprovider2(ISampleProvider sourceProvider2, int cutOffFreq)
            {
                this.sourceProvider2 = sourceProvider2;
                this.cutOffFreq2 = cutOffFreq;

                channels2 = sourceProvider2.WaveFormat.Channels;
                filters2 = new NAudio.Dsp.BiQuadFilter[channels2];
                //filters2 = new NAudio.Dsp.BiQuadFilter[channels];
                //WaveFileReader source = new WaveFileReader(Resource4.shortNewClkNormal_loud);
                CreateFilters2();
            }



            public void CreateFilters2()
            {



                for (int n = 0; n < channels2; n++)
                    if (filters2[n] == null)
                        filters2[n] = NAudio.Dsp.BiQuadFilter.LowPassFilter(44100, cutOffFreq2, 1);
                    else
                        filters2[n].SetLowPassFilter(44100, cutOffFreq2, 1);



            }



            public WaveFormat WaveFormat { get { return sourceProvider2.WaveFormat; } }


            public int Read(float[] buffer, int offset, int count)
            {
                
           
                
                int samplesRead = sourceProvider2.Read(buffer, offset, count);



                for (int i = 0; i < samplesRead; i++)
                    buffer[offset + i] = filters2[(i % channels2)].Transform(buffer[offset + i]);


                //if (fltype == 2)
                //{

                //    for (int i = 0; i < samplesRead; i++)
                //        buffer[offset + i] = filters2[(i % channels)].Transform(buffer[offset + i]);
                //}


                return (samplesRead/divider) * notelength;
            }


        }




       



        public int freqs;

        private void Contrast_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (running == true)
            {
                timer1.Dispose();
                running = false;
                Set_BPM(sender, e);
            }

            int val = ((int)sldcon.Value);
            currentval = val;


                    fltval = 3000;
                    fltval = (fltval + (val/3));

                    fltval2 = 5000;
                    fltval2 = (fltval2 - (val/2));

         
        }

        private void NoteLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {


            if (running == true)
            {
                timer1.Dispose();
                running = false;
                Set_BPM(sender, e);
            }


            int val = ((int)sldlen.Value);
            notelength = val;
     


        }

        private void invertcheck(object sender, RoutedEventArgs e)
        {

            Slider_ValueChanged(sender, null);


            if (hardpan.IsChecked == true)
            {
                invertpan.IsEnabled = true;
            }


            if (hardpan.IsChecked == false)
            {
                invertpan.IsEnabled = false;
            }

            if (running == true)
            {
                timer1.Dispose();
                running = false;
                Set_BPM(sender, e);
            }
        }


        //private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    openFileDialog.FileName = ""; // Default file name
        //    openFileDialog.DefaultExt = ".wav"; // Default file extension
        //    openFileDialog.Filter = "WAVE files (.wav)|*.wav"; // Filter files by extension
        //    openFileDialog.CheckFileExists = true;

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        primarypath = openFileDialog.FileName;

        //        var filepath = primarypath.Substring(primarypath.LastIndexOf('\\') + 1);

        //        ChoosePrim.Content = "Primary = "+filepath;
        //    }
        //}


        //private void btnOpenFile_Click2(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();

        //    openFileDialog.FileName = ""; // Default file name
        //    openFileDialog.DefaultExt = ".wav"; // Default file extension
        //    openFileDialog.Filter = "WAVE files (.wav)|*.wav"; // Filter files by extension
        //    openFileDialog.CheckFileExists = true;

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        secondarypath = openFileDialog.FileName;

        //        var filepath = secondarypath.Substring(secondarypath.LastIndexOf('\\') + 1);

        //        ChooseSec.Content = "Secondary = " + filepath;
        //    }
        //}

        private void AltSec_Copy_Checked(object sender, RoutedEventArgs e)
        {

            SolidColorBrush green = new SolidColorBrush(Green);
            SolidColorBrush blue = new SolidColorBrush(Blue);
            SolidColorBrush orange = new SolidColorBrush(Orange);

            //if (customclick.IsChecked == true)
            //{
     




            //    ChoosePrim.Background = green;
            //    ChooseSec.Background = green;
            //    ChoosePrim.Foreground = red;
            //    ChooseSec.Foreground = red;


            //    ChoosePrim.IsEnabled = true;
            //    ChooseSec.IsEnabled = true;

            //}

            //if (customclick.IsChecked == false)
            //{

            //    ChoosePrim.IsEnabled = false;
            //    ChooseSec.IsEnabled = false;

            //    ChoosePrim.Background = Brushes.DarkGray;
            //    ChooseSec.Background = Brushes.DarkGray;
            //    ChoosePrim.Foreground = Brushes.Gray;
            //    ChooseSec.Foreground = Brushes.Gray;


            //}
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (timer1 != null)
            {
                timer1.Dispose();
            }
        }
    }




}



