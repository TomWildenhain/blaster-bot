using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlasterBot
{
    enum GemTypes
    {
        Unknown, Red, Orange, Yellow, Green, Blue, Pink, White, Cube
    }
    static class GemInfo
    {
        public static string gemToString(GemTypes gem)
        {
            if (gem == GemTypes.Unknown)
            {
                return "???";
            }
            return gem.ToString();
        }
    }
}
