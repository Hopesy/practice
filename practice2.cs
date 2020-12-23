using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace practice
{
    [Transaction(TransactionMode.Manual)]
    class practice2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //获取当前文档
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //获取墙体注意这里是wall（墙实例familyinstance）而不是wallType（族类型familySimbol）
            //Wall wall = doc.GetElement(new ElementId(453726)) as Wall;//ID经常改变，不建议这样获取
            Wall wallInstance1 = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).OfClass(typeof(Wall)).FirstOrDefault(x => x.Name == "test") as Wall;
            //lookupparameter获取属性，注意单位转换
            double height = wallInstance1.LookupParameter("无连接高度").AsDouble()*0.3048;
            int roomBound = wallInstance1.LookupParameter("房间边界").AsInteger();
            string id = wallInstance1.Id.ToString();
           // TaskDialog.Show("显示高度",$"墙的高度为:{height},墙的边界值是:{roomBound}\n墙体的ID为{id}");
            //修改墙体的属性，需要开启事务
            Transaction trans = new Transaction(doc, "修改墙体高度");
            trans.Start();
            wallInstance1.LookupParameter("无连接高度").Set(5 / 0.3048);
            double heightChanged = wallInstance1.LookupParameter("无连接高度").AsDouble()*0.3048;
            TaskDialog.Show("显示信息", $"墙修改后的高度为:{heightChanged},墙的边界值是:{roomBound}\n墙体的ID为{id}");
            trans.Commit();
            return Result.Succeeded;
        }
    }
}
