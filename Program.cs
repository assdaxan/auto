using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using OpenCvSharp;

namespace auto
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //screenshot();
            //Bitmap bigbmp = new Bitmap(@"C:\Users\Administrator\Desktop\i.bmp");
        }
        public static void ScreenCopy()
        {
            // 주화면의 크기 정보 읽기
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;

            // 화면 크기만큼의 Bitmap 생성
            using (Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb))
            {
                // Bitmap 이미지 변경을 위해 Graphics 객체 생성
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                }
                // Bitmap 데이타를 파일로 저장
                bmp.Save("test.png", ImageFormat.Png);
            }
        }
        // static void screenshot(){
        //     Rectangle rect = Screen.PrimaryScreen.Bounds;

        //     using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb)){
        //         using (Graphics gr = Graphics.FromImage(bmp)){
        //             gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        //         }
        //         bmp.Save(@"i.bmp");
        //     }
        // }
    }
}
