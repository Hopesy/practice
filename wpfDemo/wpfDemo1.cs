using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfDemo.Models;

namespace wpfDemo
{
    [Transaction(TransactionMode.Manual)]
    class wpfDemo1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            #region 与从sqlServer中读取数据
            //[1]编写连接字符串与SQL语句
            string connString = @"Server=.;DataBase=WallDB;Uid=sa;Pwd=Zz1416200020";
            string sql = $"select * from WallCreate where WallId =1";
            //【2】建立连接
            SqlConnection conn = new SqlConnection(connString);
            //【3】打开连接
            conn.Open();
            //【4】执行命令
            SqlCommand cmd = new SqlCommand(sql, conn);
            //【5】读取返回值，返回值为object
            SqlDataReader sqlData = cmd.ExecuteReader();
            WallCreate wallCreate = new WallCreate();
            if (sqlData.Read())
            {
                wallCreate.WallHeight = Convert.ToDouble(sqlData["WallHeight"]);
                wallCreate.StartPointX = Convert.ToDouble(sqlData["StartPointX"]);
                wallCreate.StartPointY = Convert.ToDouble(sqlData["StartPointY"]);
                wallCreate.StartPointZ = Convert.ToDouble(sqlData["StartPointZ"]);
                wallCreate.EndPointX = Convert.ToDouble(sqlData["EndPointX"]);
                wallCreate.EndPointY = Convert.ToDouble(sqlData["EndPointY"]);
                wallCreate.EndPointZ = Convert.ToDouble(sqlData["EndPointZ"]);
            }
            #endregion 
            //获取当前文档
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            MainWindow windowTest= new MainWindow();
            //windowTest.Show();//非模态窗体，没有确定的状态，即使不关掉窗体，也会执行下面的代码，自动创建墙体
            windowTest.ShowDialog();//模态窗体，叉掉窗口才能进行下一步，执行这个函数之后的代码
            //使用过滤器获取墙的类别
            if (!windowTest.IsClicked)
            {
                return Result.Cancelled;
            }
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Element ele = collector.OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(WallType))
                .FirstOrDefault(X => X.Name == "内墙-100");
            WallType wallType = ele as WallType;
            //获取标高
            //元素收集器需要再次创建一个
            Level le = new FilteredElementCollector(doc).OfClass(typeof(Level)).FirstOrDefault(X => X.Name == "1F") as Level;
            //生成墙体的参照线
            XYZ start = new XYZ(wallCreate.StartPointX, wallCreate.StartPointY, wallCreate.StartPointZ);
            XYZ end = new XYZ(wallCreate.EndPointX, wallCreate.EndPointY, wallCreate.EndPointZ);
            Line geomLine = Line.CreateBound(start, end);
            //墙体的高度,显示为毫米，存储为英尺，0.3048换算
            double height = Convert.ToDouble(windowTest.textBox.Text)/0.3048/1000;

            double offset = 0;
            Transaction trans = new Transaction(doc, "创建墙体");
            trans.Start();
            //创建墙体，增删改需要在事务中执行，事务中的代码耗费资源
            //即使写了commit，但是代码有问题，也会报错显示事务未关闭
            Wall wall = Wall.Create(doc, geomLine, wallType.Id, le.Id, height, offset, true, true);
            trans.Commit();
            return Result.Succeeded;

        }
    }
}
