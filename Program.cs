using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Auto{
    class Program{        
        static void Main(string[] args){
            Console.WriteLine("UBUN Image Auto");
            auto auto = new auto();
            if (auto.loadImg(@"C:\Users\Administrator\Desktop\target.PNG")){
                if(auto.searchImg())
                    auto.oneClick();
            }
        }
    }
}
