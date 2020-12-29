using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Drawing;
using System.Windows.Shapes;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ManaTranslation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 创建托盘图标
        /// </summary>
         private NotifyIcon notifyIcon = null;
        /// <summary>
        /// 一个接收本地存储的布尔值，用来指示是否开启背景图像等关闭软件时会再把值返回给本地值
        /// </summary>
         bool bl2 = Properties.Settings.Default.KG;
        /// <summary>
        /// 开启背景时所要播放的动画
        /// </summary>
        Storyboard story;
        /// <summary>
        /// 关闭背景时所要播放的动画
        /// </summary>
        Storyboard story2;
        /// <summary>
        /// 储存用户现在点击的是那种翻译,为1代码自动，为2代码中日，为3代码中韩，为4代码中西班牙
        /// </summary>
        int st1=0; 
        /// <summary>
        /// 储存翻译语言方式的类
        /// </summary>
        public TranClass tranClass = new TranClass();
        //public yuyan yy = yuyan.GetInstance();
        public MainWindow()
        {

            
            InitializeComponent();
            InitialTray();
            bool bl = Properties.Settings.Default.KG;//每次启动时把本地值传给bl来确定用户上次关闭时是否开启背景
            this.Closing += new System.ComponentModel.CancelEventHandler(window1_Closing);//窗口关闭时发生的事件
            story = (Storyboard)this.FindResource("Storyboard1");//开启动画
            story2 = (Storyboard)this.FindResource("Storyboard2");//关闭动画
            story.Completed += story_Completed;//开启动画播放完毕后发生的事件
            story2.Completed += story2_Completed;//关闭动画播放完毕发生的事件
            
            if (bl == false)
            {
                //判断背景是否开启，bl=false为没有开启，把关闭按钮隐藏
                m1.Visibility = System.Windows.Visibility.Hidden;
                chuangti(false);//传给窗体的显示方法为假表示没有背景
            }
            else
            {
                //否则为真，把开启按钮隐藏
                _221.Visibility = System.Windows.Visibility.Hidden;
                chuangti(true);//传给窗体方法现在为显示图像背景
            }

            //显示时间
            DateTime da = DateTime.Now;
            string yue = da.Month.ToString();
            string ri = da.Day.ToString();
            string shi = da.ToShortTimeString().ToString();
           
            string[] Day= new string[]{"星期日","星期一","星期二","星期三","星期四","星期五","星期六" };                   
           string xingqi =  Day[Convert.ToInt16(DateTime.Now.DayOfWeek)]; 
            TimerText.Text = string.Format("{0}月{1}日{2}", yue, ri,xingqi);
            month.Text = string.Format("{0}", shi);
            //显示时间
        }
        /// <summary>
        /// 关闭动画播放完毕的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void story2_Completed(object sender, EventArgs e)
        {
            //关闭的动画播放完毕生把关闭按钮隐藏显示开启按钮
            Storyboard st = (sender) as Storyboard;
            _221.Visibility = System.Windows.Visibility.Visible;
            m1.Visibility = System.Windows.Visibility.Hidden;
            //移除关闭按钮动画
            story2.Remove();
            bl2 = false;
        }
        /// <summary>
        /// 开启动画播放完毕后的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void story_Completed(object sender, EventArgs e)
        {
            //开启动画播放完毕生即隐藏自己，显示关闭按钮
            Storyboard st = (sender) as Storyboard;
            _221.Visibility = System.Windows.Visibility.Hidden;
            m1.Visibility = System.Windows.Visibility.Visible;
            story.Remove();//移除开启动画
            bl2 = true;
        }
        /// <summary>
        /// 点击开启按钮时播放开启动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn221(object sender, MouseButtonEventArgs e)
        {
           // bool bl = Properties.Settings.Default.KG;
            story.Begin();//播放动画
            chuangti(true);//让窗体显示背景


        }
        /// <summary>
        /// 窗口关闭的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void window1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //当窗口关闭时把储存背景是否开启的值再传回给本地的布尔值
            Properties.Settings.Default.KG = bl2;
            Properties.Settings.Default.Save();
        }
        /// <summary>
        /// 点击关闭按钮时播放动画
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn2212(object sender, MouseButtonEventArgs e)
        {
            story2.Begin();
            chuangti(false);//让窗体为纯色背景
        }
        /// <summary>
        /// 翻译按钮点击事件，翻译方式只支持中文翻译为其他4国语言，或者其他4国语言翻译为中文，不支持国外与国外语言的翻译
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fanyi_Click(object sender, RoutedEventArgs e)
        {
            
            if (yuan_txt.Text == "manasxx")
            {
                //判断
                yi_txt.Text = "本软件开发者的昵称。。。";
                return;
            }
            st1 = Properties.Settings.Default.myClick;//接收本地储存的翻译方式
            tranClass.From = "zh"; //要翻译的放言为中文
            switch (st1)
            {
                case 1://1为自动，翻译的结果始终为中文
                    tranClass.From = "auto";
                    tranClass.To = "auto";
                    break;
                case 2://2为中-日
                    tranClass.To = "jp";
                    break;
                case 3://3为中-韩
                    tranClass.To = "kor";
                    break;
                case 4://4为中-西班牙
                    tranClass.To = "spa";
                    break;
                default:
                    break;
            }

            WebClient client = new WebClient();  //引用System.Net
            string fromTranslate = yuan_txt.Text; //存储翻译前的内容
            if (fromTranslate.Trim() == "")//为空则退出此事件
                return;
            if (!string.IsNullOrEmpty(fromTranslate))//不为空时发生
            {
                //client_id为自己的api_id，q为翻译对象，from为翻译语言，to为翻译后语言
                //旧版本已下线
                Random random = new Random();
                string salt = random.Next(10000, 100000).ToString();
                string appid = "20201223000653939";
                string sign = appid + fromTranslate + salt + "Ix3iPGz1C6NpNF17XGR7";
                string md5Value = GetMD5(sign);
                string url = string.Format("http://api.fanyi.baidu.com/api/trans/vip/translate?appid={0}&q={1}&from={2}&to={3}&salt={4}&sign={5}", appid, fromTranslate, tranClass.From, tranClass.To,salt,md5Value);

                var buffer    = client.DownloadData(url);//把接收到的数据传给字节类型的变量
                string result = Encoding.UTF8.GetString(buffer);
                string json = Regex.Unescape(result);
                JObject jo    = JObject.Parse(json);//用Json来解析字符串
                var name = jo["trans_result"][0]["dst"];//接收解释后得到的翻译结果值
                JToken st = name;
                yi_txt.Text = st.ToString();//把翻译结显示到结果文本框
            }
        }
        private string GetMD5(string sDataIn)

        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bytValue, bytHash;

            bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);

            bytHash = md5.ComputeHash(bytValue);

            md5.Clear();

            string sTemp = "";

            for (int i = 0; i < bytHash.Length; i++)

            {

                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');

            }

            return sTemp.ToLower();

        }
        /// <summary>
        /// 用来窗体是否显示背景的方法
        /// </summary>
        /// <param name="bo">值为真是显示图像，为假显示纯色</param>
        private void chuangti(bool bo)
        {

            string[] lie = { "image/2.jpg", "image/3.jpg", "image/1.jpg", "image/4.jpg" };//4张背景图片
            Random r = new Random();
            int ge = r.Next(lie.Length);//随机显示背景
            if (bo == true)
            {
                ImageBrush berriesBrush = new ImageBrush();
                berriesBrush.Stretch = Stretch.UniformToFill;//图像显示方式为居中对齐不拉伸图像
                berriesBrush.Opacity = 0.4;//透明度为0.8
                berriesBrush.ImageSource = new BitmapImage(new Uri(lie[ge], UriKind.Relative));//接收要显示的图像
                this.Background = berriesBrush;//显示图像
            }
            else
            {
                this.Background = System.Windows.Media.Brushes.WhiteSmoke;//否则为纯色背景
            }
        }
        /// <summary>
        /// 点击软件右上角的图像关闭按钮时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_btn(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
            //if (System.Windows.MessageBox.Show("确定要褪、tui、腿 垦算了你爱菜就好！", "退出", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            //{
            //    notifyIcon.Dispose();
            //    System.Windows.Application.Current.Shutdown();

            //}
        }
        /// <summary>
        /// 移动窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void window_Move(object sender,System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        /// <summary>
        /// 点击语言按钮时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yuyan_Click(object sender, RoutedEventArgs e)
        {
            //让选择语言的窗口显示在主窗体上方
            yuyan.GetInstance(this.Top + 20,this.Left).ShowDialog();
        }
        /// <summary>
        /// 当鼠标离开文本框再移动进去时让文字属于选中状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textbox_gotFocus(object sender, RoutedEventArgs e)
        {
            yuan_txt.SelectAll();
        }

        private void textbox_Buttondown(object sender, MouseButtonEventArgs e)
        {
            var control = sender as System.Windows.Controls.TextBox;
            if (control == null)
                return;
            Keyboard.Focus(control);
            e.Handled = true;
        }

        private void textbox2_gotfocus(object sender, RoutedEventArgs e)
        {
            yi_txt.SelectAll();
        }
        /// <summary>
        /// 点击发音按钮时随机播放3个音乐中的其中一个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void yin_Click(object sender, RoutedEventArgs e)
        {
            //string[] lie = { "mana1.wav", "mana2.wav","mana3.wav" };
            //Random r = new Random();
            //int ge = r.Next(lie.Length);
            //SoundPlayer su = new SoundPlayer();
            //su.SoundLocation = lie[ge];
            //su.Play();
            System.Windows.MessageBox.Show("悲剧！这个我不会！");
        }


        private void InitialTray()
        {
            this.ShowInTaskbar = false;//禁止窗口显示在任务栏

            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            //notifyIcon.BalloonTipText = "程序开始运行";//冒泡通知
            //notifyIcon.ShowBalloonTip(100);//冒泡通知显示时间
            notifyIcon.Text = "托盘图标";
            notifyIcon.Icon = new System.Drawing.Icon("image/22.ico");
            notifyIcon.Visible = true;

            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("关于");
            menu.Click += menu_Click;
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("exit");
            exit.Click +=exit_Click;

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { menu, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            this.StateChanged += new EventHandler(SysTray_StateChanged);
        }

        /// <summary>
        /// 点击托盘的关于菜单时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void menu_Click(object sender, EventArgs e)
        {
           System.Windows.MessageBox.Show("对于煤球王这个称呼你表示赞同吗？", "关于", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
        }
        private void SysTray_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.Visibility = Visibility.Collapsed;
            }
        }
/// <summary>
/// 点击托盘的退出菜单时发生
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
        private void exit_Click(object sender, EventArgs e)
        {
            if (System.Windows.MessageBox.Show("拜拜！", "退出", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();

            }
        }
        /// <summary>
        /// 点击托盘图标时发生，判断是隐藏主界面还是显示主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {

           
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.Visibility == Visibility.Visible)
                {
                    this.Visibility = Visibility.Hidden;
                }
                else
                {
                    this.Visibility = Visibility.Visible;
                    this.Activate();
                }
            }
        }

    }
}
