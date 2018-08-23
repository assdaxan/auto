﻿using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace auto{
    class Program{
        // Win API 정의
        [DllImport("user32.dll")]
        static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString,int nMaxCount);

        // Win API에서 사용되는 변수 정의
        // mouse_event
        const int MOUSEEVENTF_LEFTDOWN = 0x2;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        const int MOUSEEVENTF_LEFTUP = 0x4;
        const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        const int MOUSEEVENTF_MIDDLEUP = 0x40;
        const int MOUSEEVENTF_MOVE = 0x1;
        const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        const int MOUSEEVENTF_RIGHTUP = 0x10;
        const int MOUSEEVENTF_XDOWN = 0x80;
        const int MOUSEEVENTF_XUP = 0x100;
        const int MOUSEEVENTF_WHEEL = 0x800;
        const int MOUSEEVENTF_HWHEEL = 0x1000;
        // GetWindow
        const int GW_CHILD = 0x5;
        const int GW_ENABLEDPOPUP = 0x6;
        const int GW_HWNDFIRST = 0x0;
        const int GW_HWNDLAST = 0x1;
        const int GW_HWNDNEXT = 0x2;
        const int GW_HWNDPREV = 0x3;
        const int GW_OWNER = 0x4;

        static void Main(string[] args){
            Console.WriteLine("UBUN Image Auto");
            using (Bitmap target = loadImg(@"C:\Users\Administrator\Desktop\target.PNG"))
                searchImg(target);
        }
        public static IntPtr findHwnd(string targetName){
            StringBuilder s = new StringBuilder(256);
            IntPtr hwnd = FindWindow(null, null);
            while(hwnd != IntPtr.Zero){
                GetWindowText(hwnd, s, 256);
                if(s.ToString().Equals(targetName))
                    break;
                hwnd = GetWindow(hwnd, GW_HWNDNEXT);
            }
            return hwnd;
        }
        public static Bitmap loadImg(string path){
            return new Bitmap(path);
        }
        public static Bitmap ScreenCopy(){
            // 주화면의 크기 정보 읽기
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;

            // 화면 크기만큼의 Bitmap 생성
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            // Bitmap 이미지 변경을 위해 Graphics 객체 생성
            using(Graphics gr = Graphics.FromImage(bmp)){
                // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            }
            return bmp;
        }
        public static void searchImg(Bitmap find_img){
            using (Bitmap screen_img = ScreenCopy())
            using (Mat ScreenMat = BitmapConverter.ToMat(screen_img))
            using (Mat FindMat = BitmapConverter.ToMat(find_img))
            //스크린 이미지에서 FindMat 이미지를 찾습니다.
            using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed)){
                double minval, maxval = 0;
                OpenCvSharp.Point minloc, maxloc;
                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                Console.WriteLine("찾은 이미지의 유사도 : " + maxval);
                //유사도를 0.7 초과로 설정합니다.
                if (maxval > 0.7){
                    int width = find_img.Size.Width;
                    int height = find_img.Size.Height;
                    //이미지의 중앙을 누름니다.
                    click(maxloc.X+(width/2), maxloc.Y+(height/2));
                    Console.WriteLine($"{maxloc.X}, {maxloc.Y}");
                }
            }
        }
        public static void click(int x, int y){
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
