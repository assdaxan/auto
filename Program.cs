using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;

namespace auto{
    class Program : WinApi{        
        static void Main(string[] args){
            Console.WriteLine("UBUN Image Auto");
            HWND h = getHwndFromCaption("제목 없음 - 메모장");

            // using (Bitmap target = loadImg(@"C:\Users\Administrator\Desktop\target.PNG")){
            //     OpenCvSharp.Point p = searchImg(target);
            //     if(p.X != -1 && p.Y != -1){
            //         int width = target.Size.Width;
            //         int height = target.Size.Height;
            //         click(p.X+(width/2), p.Y+(height/2));
            //     }
            // }
        }
        static int getPidFromHwnd(HWND hwnd){
            int a = 0;
            GetWindowThreadProcessId(hwnd, out a);
            return a;
        }
        static HWND getHwndFromCaption(string targetName){
            StringBuilder s = new StringBuilder(256);
            HWND hwnd = FindWindow(null, null);
            while(hwnd != HWND.Zero){
                GetWindowText(hwnd, s, s.Capacity);
                if(s.ToString().Equals(targetName))
                    break;
                hwnd = GetWindow(hwnd, GW_HWNDNEXT);
            }
            return hwnd;
        }
        static Bitmap loadImg(string path){
            return new Bitmap(path);
        }
        static Bitmap ScreenCopy(){
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
        static OpenCvSharp.Point searchImg(Bitmap find_img){
            using (Bitmap screen_img = ScreenCopy())
            using (Mat ScreenMat = BitmapConverter.ToMat(screen_img))
            using (Mat FindMat = BitmapConverter.ToMat(find_img))
            //스크린 이미지에서 FindMat 이미지를 찾습니다.
            using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed)){
                double minval, maxval = 0;
                OpenCvSharp.Point minloc, maxloc;
                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                Console.WriteLine(maxloc);
                Console.WriteLine("찾은 이미지의 유사도 : " + maxval);
                //유사도를 0.7 초과로 설정합니다.
                if (maxval > 0.7){
                    Console.WriteLine($"{maxloc.X}, {maxloc.Y}");
                    return maxloc;
                }
            }
            return new OpenCvSharp.Point(-1, -1);
        }
        static void click(int x, int y){
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }
    }
}
