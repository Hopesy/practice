﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalEventDemo 
{
    class CreateWall : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            //获取当前文档
            // UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            // Document doc = uiDoc.Document;
            Document doc = app.ActiveUIDocument.Document;
            MainWindow windowTest = new MainWindow();
            //windowTest.Show();//非模态窗体，没有确定的状态，即使不关掉窗体，也会执行下面的代码，自动创建墙体
            windowTest.ShowDialog();//模态窗体，叉掉窗口才能进行下一步，执行这个函数之后的代码
            //使用过滤器获取墙的类别
    
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            Element ele = collector.OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(WallType))
                .FirstOrDefault(X => X.Name == "内墙-100");
            WallType wallType = ele as WallType;
            //获取标高
            //元素收集器需要再次创建一个
            Level le = new FilteredElementCollector(doc).OfClass(typeof(Level)).FirstOrDefault(X => X.Name == "1F") as Level;
            //生成墙体的参照线
            XYZ start = new XYZ(0, 0, 0);
            XYZ end = new XYZ(100, 100, 0);
            Line geomLine = Line.CreateBound(start, end);
            //墙体的高度,显示为毫米，存储为英尺，0.3048换算
            double height = 5000/ 0.3048 / 1000;

            double offset = 0;
            Transaction trans = new Transaction(doc, "创建墙体");
            trans.Start();
            //创建墙体，增删改需要在事务中执行，事务中的代码耗费资源
            //即使写了commit，但是代码有问题，也会报错显示事务未关闭
            Wall wall = Wall.Create(doc, geomLine, wallType.Id, le.Id, height, offset, true, true);
            trans.Commit();
            if(!(wall.Id == null))
            {
                TaskDialog.Show("eventTEST", "墙体生成成功");
            }
            else
            {
                TaskDialog.Show("eventTEST", "墙体没有生成");
            }
        
        }

        public string GetName()
        {
            //套路，返回类名就行CreateWall
            return "CreateWall";
        }
    }
}