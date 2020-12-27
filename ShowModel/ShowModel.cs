using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ShowModel
{
    [Transaction(TransactionMode.Manual)]
    class ShowModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //获取当前文档
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            MainWindow wpf = new MainWindow();
           PreviewControl pc = new PreviewControl(doc, commandData.Application.ActiveUIDocument.ActiveGraphicalView.Id);
            wpf.MainGrid.Children.Add(pc);
            wpf.ShowDialog();//非模态窗体，没有确定的状态，即使不关掉窗体，也会执行下面的代码，自动创建墙体
            return Result.Succeeded;

        }
    }
}
