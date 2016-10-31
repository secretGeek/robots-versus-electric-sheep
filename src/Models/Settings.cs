using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nimble_life
{
    public class Settings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        //public int MaxAge { get; set; }
        public int Delay { get; set; }
        public int AgeOfMaturity { get; internal set; }
        public int OneInThisIsHerby { get; set; }
    }
}
