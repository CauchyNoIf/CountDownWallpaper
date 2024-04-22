﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime;
using System.Runtime.InteropServices;
using System.IO;
using System.Configuration;
using System.Xml.Serialization;

namespace CountDownWallpaper
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        DateTime destTime = DateTime.Today.AddDays(9.0);

        string strImageDirectory = Environment.CurrentDirectory + "\\Wallpaper\\";
        string[] strImgList;

        Rectangle screenBounds = Screen.PrimaryScreen.Bounds;

        // 创建一个与屏幕大小相同的位图
        Image srcImg;
        Bitmap srcBitmap;

        int nCount = 0;
        int nDisplayTime = 30000;

        Timer m_updateTimer = new Timer();




        private string[] getImageNames(string dir)
        {
            string[] strlistAllFiles = Directory.GetFiles(dir);
            string[] strlistExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".tiff" };
            List<string> strlistNames = new List<string>();

            foreach (string file in strlistAllFiles)
            {
                string extension = Path.GetExtension(file).ToLower();
                if (Array.Exists(strlistExtensions, ext => ext == extension))
                {
                    //numImages++;
                    strlistNames.Add(Path.GetFullPath(file));
                }
            }

            return strlistNames.ToArray();
        }

        private string[] generateHourList()
        {
            List<string> hourList = new List<string>();

            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {
                    hourList.Add("0" + Convert.ToString(i));
                }
                else
                {
                    hourList.Add(Convert.ToString(i));
                }
            }

            return hourList.ToArray();
        }

        private string[] generateMinuteList()
        {
            List<string> minuteList = new List<string>();

            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    minuteList.Add("0" + Convert.ToString(i));
                }
                else
                {
                    minuteList.Add(Convert.ToString(i));
                }
            }

            return minuteList.ToArray();
        }

        private string[] generateSecondList()
        {
            List<string> secondList = new List<string>();

            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {
                    secondList.Add("0" + Convert.ToString(i));
                }
                else
                {
                    secondList.Add(Convert.ToString(i));
                }
            }

            return secondList.ToArray();
        }
       
        public Form1()
        {
            InitializeComponent();

            hourDomainUpDown.Items.AddRange(generateHourList());
            minuteDomainUpDown.Items.AddRange(generateMinuteList());
            secondDomainUpDown.Items.AddRange(generateSecondList());

            destTime = getIniTime();
            nDisplayTime = getDisplayTime();

            strImgList = getImageNames(strImageDirectory);

            if (strImgList.Length <= 0)
            {
                MessageBox.Show("Wallpaper目录下没有图片!");
                Environment.Exit(0);
            }

            nCount = strImgList.Length;
            srcImg = Image.FromFile(strImgList[0]);
            srcBitmap = new Bitmap(srcImg, screenBounds.Size);

            m_updateTimer.Tick += M_updateTimer_Tick;
            m_updateTimer.Interval = 1000;

            int milliseconds = DateTime.Now.Millisecond;
            if (milliseconds % 1000 != 0)
                CustomSleep(1000 - milliseconds);

            m_updateTimer.Start();
        }

        public static void CustomSleep(int milliseconds)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliseconds)
            {
                // 允许界面响应消息
                Application.DoEvents();
            }
        }

        private void M_updateTimer_Tick(object sender, EventArgs e)
        {
            if (getImageNames(strImageDirectory).Length <= 0)
            {
                MessageBox.Show("Wallpaper目录下没有图片!");
                Environment.Exit(0);
            }

            //if (isSlideShowCheckBox.Checked)
            //{
            //    nCount++;
            //    srcImg = Image.FromFile(strImgList[nCount%strImgList.Length]);
            //    srcBitmap = new Bitmap(srcImg, screenBounds.Size);
            //}


                

            DateTime currentTime = DateTime.Now;
            TimeSpan span = destTime - currentTime;
            string strCurrentTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");

            hourDomainUpDown.Text = DateTime.Now.ToString("HH");
            minuteDomainUpDown.Text = DateTime.Now.ToString("mm");
            secondDomainUpDown.Text = DateTime.Now.ToString("ss");

            Bitmap bitmap = new Bitmap(srcBitmap);

            // 绘制时间
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                string drawText = "Now is: " + 
                    strCurrentTime + 
                    ".\n" + 
                    ((long)span.TotalSeconds).ToString() + 
                    "s  to " + 
                    destTime.ToString("yyyy/MM/dd HH:mm:ss") +
                    ".";
                //graphics.FillRectangle(Brushes.White, 0, 0, screenBounds.Width, screenBounds.Height);
                graphics.DrawString(drawText, new Font("TimesNewRoman", 34), Brushes.White, screenBounds.Width/4, 50);

            }

            bitmap.Save(strImageDirectory + "createdImg.bmp");

            int ret = SystemParametersInfo(0x0014, 0, strImageDirectory + "createdImg.bmp", 0);
        }

        public class AppSettings
        {
            public string TargetTime { get; set; }
            public int DisplayTime { get; }
        }

        private DateTime getIniTime()
        {
            // Deserialize
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            using (StreamReader reader = new StreamReader("TargetTime.xml"))
            {
                AppSettings loadedSettings = (AppSettings)serializer.Deserialize(reader);
                return DateTime.ParseExact(loadedSettings.TargetTime, "yyyy/MM/dd HH:mm:ss", null);
            }
        }

        private void setIniTime(DateTime writeDateTime)
        {
            AppSettings settings = new AppSettings { TargetTime = writeDateTime.ToString("yyyy/MM/dd HH:mm:ss") };

            // Serialize
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            using (StreamWriter writer = new StreamWriter("TargetTime.xml"))
            {
                serializer.Serialize(writer, settings);
            }
        }

        private int getDisplayTime()
        {
            // Deserialize
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            using (StreamReader reader = new StreamReader("TargetTime.xml"))
            {
                AppSettings loadedSetting = (AppSettings)serializer.Deserialize(reader);
                return loadedSetting.DisplayTime;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dateDateTimePicker.Value = DateTime.Now;
            hourDomainUpDown.Text = DateTime.Now.ToString("hh");
            minuteDomainUpDown.Text = DateTime.Now.ToString("mm");
            secondDomainUpDown.Text = DateTime.Now.ToString("ss");
        }

        private void dateDateTimePicker_Enter(object sender, EventArgs e)
        {
            m_updateTimer.Stop();
        }

        private void hourDomainUpDown_Enter(object sender, EventArgs e)
        {
            m_updateTimer.Stop();
            hourDomainUpDown.SelectedIndex = hourDomainUpDown.Items.IndexOf(hourDomainUpDown.Text);
        }

        private void minuteDomainUpDown_Enter(object sender, EventArgs e)
        {
            m_updateTimer.Stop();
            minuteDomainUpDown.SelectedIndex = minuteDomainUpDown.Items.IndexOf(minuteDomainUpDown.Text);
        }

        private void secondDomainUpDown_Enter(object sender, EventArgs e)
        {
            m_updateTimer.Stop();
            secondDomainUpDown.SelectedIndex = secondDomainUpDown.Items.IndexOf(secondDomainUpDown.Text);
        }

        private void setTargetTimeBtn_Click(object sender, EventArgs e)
        {
            DateTime inputDate = dateDateTimePicker.Value.Date;

            if (Convert.ToInt32(hourDomainUpDown.Text) < 0 || Convert.ToInt32(hourDomainUpDown.Text) > 23)
            {
                int ms = DateTime.Now.Millisecond;
                if (ms % 1000 != 0)
                    CustomSleep(1000 - ms);
                m_updateTimer.Start();
                MessageBox.Show("无效的输入");
                return;
            }
            if (Convert.ToInt32(minuteDomainUpDown.Text) < 0 || Convert.ToInt32(minuteDomainUpDown.Text) > 59)
            {
                int ms = DateTime.Now.Millisecond;
                if (ms % 1000 != 0)
                    CustomSleep(1000 - ms);
                m_updateTimer.Start();
                MessageBox.Show("无效的输入");
                return;
            }
            if (Convert.ToInt32(secondDomainUpDown.Text) < 0 || Convert.ToInt32(secondDomainUpDown.Text) > 59)
            {
                int ms = DateTime.Now.Millisecond;
                if (ms % 1000 != 0)
                    CustomSleep(1000 - ms);
                m_updateTimer.Start();
                MessageBox.Show("无效的输入");
                return;
            }

            int inputHour = DateTime.ParseExact(hourDomainUpDown.Text, "HH", null).Hour;
            int inputMinute = DateTime.ParseExact(minuteDomainUpDown.Text, "mm", null).Minute;
            int inputSecond = DateTime.ParseExact(secondDomainUpDown.Text, "ss", null).Second;

            destTime = inputDate.AddHours(inputHour).AddMinutes(inputMinute).AddSeconds(inputSecond);
            setIniTime(destTime);

            int milliseconds = DateTime.Now.Millisecond;
            if (milliseconds % 1000 != 0 && milliseconds > 300)
                CustomSleep(1000 - milliseconds);

            m_updateTimer.Start();

            if (destTime < DateTime.Now)
            {
                MessageBox.Show("不能是过去的时间!");
                return;
            }
        }

        //private void isSlideShowCheckBox_CheckedChanged(object sender, EventArgs e)
        //{
        //    m_updateTimer.Stop();

        //    if (isSlideShowCheckBox.Checked)
        //    {
        //        m_updateTimer.Interval = 10000;
        //    }
        //    else
        //    {
        //        m_updateTimer.Interval = 1000;
        //    }

        //    m_updateTimer.Start();
        //}

        private void hourDomainUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字和退格键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void minuteDomainUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字和退格键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void secondDomainUpDown_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 只允许输入数字和退格键
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

    }
}
