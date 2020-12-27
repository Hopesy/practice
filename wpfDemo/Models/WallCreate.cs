using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfDemo.Models
{
    class WallCreate
    {
        public double StartPointX { get; set; }
        public double StartPointY { get; set; }
        public double StartPointZ{ get; set; }
        public double EndPointX { get; set; }
        public double EndPointY { get; set; }
        public double EndPointZ { get; set; }
        public double WallHeight { get; set; }
    }
}
