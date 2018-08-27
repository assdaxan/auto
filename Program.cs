using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Auto{
    using HWND = System.IntPtr;
    using HANDLE = System.IntPtr;
    using HMODULE = System.IntPtr;
    class Program{        
        static void Main(string[] args){
            Console.WriteLine("UBUN Image Auto");
            TaskManager manager = new TaskManager();
            manager.getProcessInfo();
            HWND hwnd = manager.getHwndFromCaption("제목 없음 - 메모장");
            Console.WriteLine(hwnd);
            manager.setForegroundWindow(hwnd);
            // Auto auto = new Auto(@"C:\Users\Administrator\Desktop\target.PNG");
            // if(auto.searchImg())
            //     auto.oneClick();
        }
    }
}
