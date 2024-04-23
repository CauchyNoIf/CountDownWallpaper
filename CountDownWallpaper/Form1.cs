using System;
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
using System.Diagnostics;

namespace CountDownWallpaper
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        DateTime targetTime = DateTime.Today.AddDays(9.0);

        string strImageDirectory = Environment.CurrentDirectory + "\\Wallpaper\\";
        string[] strImgList;

        Rectangle screenBounds = Screen.PrimaryScreen.Bounds;

        Image srcImg;
        Bitmap srcBitmap;
        Bitmap showBitmap;

        int nCount = 0;
        int nDisplayTime = 30000;

        Timer m_updateTimer = new Timer();
        Stopwatch m_elapsedWatch = new Stopwatch();


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
                    strlistNames.Add(Path.GetFullPath(file));
                }
            }

            return strlistNames.ToArray();
        }

        private string[] generateDigitalList(int nMaxNum)
        {
            List<string> hourList = new List<string>();

            for (int i = 0; i < nMaxNum; i++)
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

       
        public Form1()
        {
            InitializeComponent();

            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory + "\\Wallpaper");
            if (!directoryInfo.Exists)
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Wallpaper");

            directoryInfo = new DirectoryInfo(Environment.CurrentDirectory + "\\tmp");
            if (!directoryInfo.Exists)
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\tmp");

            strImgList = getImageNames(strImageDirectory);

            if (strImgList.Length <= 0)
            {
                MessageBox.Show("Wallpaper目录下没有图片!");
                Environment.Exit(0);
            }

            hourDomainUpDown.Items.AddRange(generateDigitalList(24));
            minuteDomainUpDown.Items.AddRange(generateDigitalList(60));
            secondDomainUpDown.Items.AddRange(generateDigitalList(60));

            targetTime = getIniTime();
            nDisplayTime = getDisplayTime();

            nCount = strImgList.Length;
            srcImg = Image.FromFile(strImgList[0]);
            srcBitmap = new Bitmap(srcImg, screenBounds.Size);

            m_updateTimer.Tick += M_updateTimer_Tick;
            m_updateTimer.Interval = 1000;
            m_updateTimer.Start();
            m_elapsedWatch.Start();
        }

        private void M_updateTimer_Tick(object sender, EventArgs e)
        {
            if (getImageNames(strImageDirectory).Length <= 0)
            {
                MessageBox.Show("Wallpaper目录下没有图片!");
                Environment.Exit(0);
            }

            showBitmap = new Bitmap(srcBitmap);

            DateTime currentTime = DateTime.Now;
            TimeSpan span = targetTime - currentTime;
            string strCurrentTime = currentTime.ToString("yyyy/MM/dd HH:mm:ss");

            int dMtime = Convert.ToInt32(currentTime.Millisecond);
            int lMElapsed1 = Convert.ToInt32(m_elapsedWatch.ElapsedMilliseconds);

            // 绘制时间
            using (Graphics graphics = Graphics.FromImage(showBitmap))

            {
                string drawText = "It is " +
                    strCurrentTime +
                    ",\n" +
                    (Convert.ToInt32(span.TotalMilliseconds / 1000)).ToString() +
                    "s until " +
                    targetTime.ToString("yyyy/MM/dd HH:mm:ss") +
                    ".";
                graphics.DrawString(drawText, new Font("TimesNewRoman", 34), Brushes.White, screenBounds.Width / 4, 50);
            }

            try
            {
                showBitmap.Save(Environment.CurrentDirectory + "\\tmp\\createdImg.bmp");
            }
            catch (Exception)
            {

            }

            // 读取本地图片并设置桌面背景
            SystemParametersInfo(0x0014, 0, Environment.CurrentDirectory + "\\tmp\\createdImg.bmp", 0);

            hourDomainUpDown.Text = strCurrentTime.Substring(11, 2);
            minuteDomainUpDown.Text = strCurrentTime.Substring(14, 2);
            secondDomainUpDown.Text = strCurrentTime.Substring(17, 2);

            int lMElapsed2 = Convert.ToInt32(m_elapsedWatch.ElapsedMilliseconds);

            int offset = lMElapsed2 - lMElapsed1 + dMtime;

            // 避免太小的Interval
            if (offset >= 800)
                offset -= 1000;

            m_updateTimer.Interval = 1000 - offset;
            m_updateTimer.Start();
            m_elapsedWatch.Restart();

            GC.Collect();
        }

        public class AppSettings
        {
            public string TargetTime { get; set; }      // 目标时间
            public int DisplayTime { get; }             // 单张展示时长，暂无使用
        }

        private DateTime getIniTime()
        {
            // 反序列化
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

            // 序列化
            XmlSerializer serializer = new XmlSerializer(typeof(AppSettings));
            using (StreamWriter writer = new StreamWriter("TargetTime.xml"))
            {
                serializer.Serialize(writer, settings);
            }
        }

        private int getDisplayTime()
        {
            // 反序列化
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
                m_updateTimer.Interval = 1000 - ms;
                m_updateTimer.Start();
                m_elapsedWatch.Restart();
                MessageBox.Show("无效的输入");
                return;
            }
            if (Convert.ToInt32(minuteDomainUpDown.Text) < 0 || Convert.ToInt32(minuteDomainUpDown.Text) > 59)
            {
                int ms = DateTime.Now.Millisecond;
                m_updateTimer.Interval = 1000 - ms;
                m_updateTimer.Start();
                m_elapsedWatch.Restart();
                MessageBox.Show("无效的输入");
                return;
            }
            if (Convert.ToInt32(secondDomainUpDown.Text) < 0 || Convert.ToInt32(secondDomainUpDown.Text) > 59)
            {
                int ms = DateTime.Now.Millisecond;
                m_updateTimer.Interval = 1000 - ms;
                m_updateTimer.Start();
                m_elapsedWatch.Restart();
                MessageBox.Show("无效的输入");
                return;
            }

            int inputHour = DateTime.ParseExact(hourDomainUpDown.Text, "HH", null).Hour;
            int inputMinute = DateTime.ParseExact(minuteDomainUpDown.Text, "mm", null).Minute;
            int inputSecond = DateTime.ParseExact(secondDomainUpDown.Text, "ss", null).Second;

            // 更新目标时间
            targetTime = inputDate.AddHours(inputHour).AddMinutes(inputMinute).AddSeconds(inputSecond);
            setIniTime(targetTime);

            // 设置定时器间隔
            int nTimeNow = DateTime.Now.Millisecond;
            m_updateTimer.Interval = 1000 - nTimeNow;
            m_updateTimer.Start();
            m_elapsedWatch.Restart();

            if (targetTime < DateTime.Now)
            {
                MessageBox.Show("不能是过去的时间!");
                return;
            }
        }

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
