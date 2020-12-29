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
using System.Windows.Shapes;

namespace ManaTranslation
{
    /// <summary>
    /// yuyan.xaml 的交互逻辑
    /// </summary>
    public partial class yuyan : Window
    {
        TranClass tranClass = new TranClass();
        private static yuyan staticInstance = null;
        int toClick;
        public yuyan()
        {
            InitializeComponent();
            this.Closed += WindowOnClosed;
            this.ShowInTaskbar = false;//禁止窗口显示在任务栏
            toClick = Properties.Settings.Default.myClick;
            switch (toClick)
            {
                case 1:
                    rad1.IsChecked = true;
                    tranClass.From = "auto";
                    tranClass.To = "auto";
                     toClick = 1;
                    break;
                case 2:
                    rad2.IsChecked = true;
                    tranClass.To = "jp";
                    toClick = 2;
                    break;
                case 3:
                    rad3.IsChecked = true;
                    tranClass.To = "kor";
                    toClick = 3;
                    break;
                case 4:
                    rad4.IsChecked = true;
                    tranClass.To = "spa";
                    toClick = 4;
                    break;
                default:
                    break;
            }
        }
        public static yuyan GetInstance(double x,double y)
        {
            if(staticInstance == null)
            {
                staticInstance = new yuyan();
                staticInstance.Top = x;
                staticInstance.Left = y;
            }
            return staticInstance;
        }

        private void rad1_click(object sender, RoutedEventArgs e)
        {
            toClick = 1;
        }

        private void btn_Close(object sender, MouseButtonEventArgs e)
        {
            Properties.Settings.Default.myClick = toClick;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void rad4_click(object sender, RoutedEventArgs e)
        {
            toClick = 4;
        }

        private void rad3_click(object sender, RoutedEventArgs e)
        {
            toClick = 3;
        }

        private void rad2_click(object sender, RoutedEventArgs e)
        {
            toClick = 2;
        }
        private void WindowOnClosed(object sender,System.EventArgs e)
        {
            staticInstance = null;
        }
        
    }
}
