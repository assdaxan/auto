using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using OpenCvSharp.Blob;
using OpenCvSharp.UserInterface;

namespace auto
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Bitmap screen = ScreenCopy();
            Bitmap target = loadImg();
            searchIMG(screen, target);
        }
        public static Bitmap loadImg()
        {
            return new Bitmap(@"C:\Users\Administrator\Desktop\target.PNG");
        }
        public static Bitmap ScreenCopy()
        {
            // 주화면의 크기 정보 읽기
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;

            // 화면 크기만큼의 Bitmap 생성
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            // Bitmap 이미지 변경을 위해 Graphics 객체 생성
            using(Graphics gr = Graphics.FromImage(bmp))
            {
                // 화면을 그대로 카피해서 Bitmap 메모리에 저장
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            }
            return bmp;
        }
        public static void searchIMG(Bitmap screen_img, Bitmap find_img)
        {
            //스크린 이미지 선언
            using (Mat ScreenMat = BitmapConverter.ToMat(screen_img))
            //찾을 이미지 선언
            using (Mat FindMat = BitmapConverter.ToMat(find_img))
            //스크린 이미지에서 FindMat 이미지를 찾아라
            using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed))
            {
                //찾은 이미지의 유사도를 담을 더블형 최대 최소 값을 선언합니다.
                double minval, maxval = 0;
                //찾은 이미지의 위치를 담을 포인트형을 선업합니다.
                OpenCvSharp.Point minloc, maxloc;
                //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                Console.WriteLine("찾은 이미지의 유사도 : " + maxval);
            }
        }
    }
}
