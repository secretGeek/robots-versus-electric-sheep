using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nimble_life
{
    public class WorldSettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        //public int MaxAge { get; set; }
        public int Delay { get; set; }
        public int AgeOfMaturity { get; internal set; }
        public int OneInThisIsHerby { get; set; }
        public int OneInThisIsRobot { get; set; }
        public int StartingEnergyLimit { get; set; }
        public int StartingAgeLimit { get; set; }

        internal static WorldSettings GetDefault()
        =>  new WorldSettings
            {
                Width = 60,
                Height = 60,
                Delay = 0, //artificial delay in milliseconds. 0 for none
                AgeOfMaturity = 16,
                OneInThisIsHerby = 45,
                OneInThisIsRobot = 45,
                StartingEnergyLimit = 75,
                StartingAgeLimit = 30,
        };
        
    }
}
