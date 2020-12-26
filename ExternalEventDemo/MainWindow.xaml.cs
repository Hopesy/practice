using Autodesk.Revit.UI;
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

namespace ExternalEventDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //之前是吧创建墙体的代码放到了主程序里面，现在放在了事件类(继承IExternalEventHandler接口)
        //[1]要想在UI界面调用外部事件，首先要注册事件，都是套路
        
        CreateWall createWallTest1 = null;
        ExternalEvent createWallEvent1 = null;
        //第二个事件完全照着第一个事件写就行了
        CreateWallTwo createWallTest2 = null;
        ExternalEvent createWallEvent2 = null;
        public MainWindow()
        {
            InitializeComponent();
            //【2】初始化
            createWallTest1 = new CreateWall();
            createWallEvent1 = ExternalEvent.Create(createWallTest1);
           createWallTest2 = new CreateWallTwo();
            createWallEvent2 = ExternalEvent.Create(createWallTest2);
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {//【3】执行命令
            //【4】传值进去
            createWallTest1.height1 = Convert.ToDouble(this.height1.Text);
            createWallEvent1.Raise();
           }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
          createWallTest2.height2 = Convert.ToDouble(this.height2.Text);
           createWallEvent2.Raise();
        }
    }
}
