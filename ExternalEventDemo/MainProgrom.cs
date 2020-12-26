using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ExternalEventDemo
{
    [Transaction(TransactionMode.Manual)]
    class MainProgrom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //获取当前文档
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            MainWindow windowTest = new MainWindow();
            windowTest.Show();//非模态窗体，没有确定的状态，即使不关掉窗体，也会执行下面的代码，自动创建墙体

            return Result.Succeeded;

        }
    }
}
