using System;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Runtime.InteropServices;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace Auto{
    class Auto : WinApi{
        Bitmap targetImg;
        OpenCvSharp.Point minloc, maxloc;
        double minval, maxval = 0;
        bool targetLoadFlag;
        
        public Auto(string path){
            loadImg(path);
        }
        void loadImg(string path){
            try{
                this.targetImg = new Bitmap(path);
                this.targetLoadFlag = true;
            }
            catch{
                this.targetLoadFlag = false;
            }
        }
        Bitmap ScreenCopy(){
            // 주화면의 크기 정보를 구합니다.
            int width = (int)SystemParameters.PrimaryScreenWidth;
            int height = (int)SystemParameters.PrimaryScreenHeight;

            // 화면 크기만큼의 Bitmap 생성합니다.
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            // Bitmap 이미지 변경을 위해 Graphics 객체 생성합니다.
            using(Graphics gr = Graphics.FromImage(bmp)){
                // 화면을 그대로 카피해서 Bitmap 메모리에 저장합니다.
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
            }
            return bmp;
        }
        public bool searchImg(){
            if(this.targetLoadFlag){
                using (Bitmap screenImg = ScreenCopy())
                using (Mat ScreenMat = BitmapConverter.ToMat(screenImg))
                using (Mat FindMat = BitmapConverter.ToMat(this.targetImg))
                //스크린 이미지에서 FindMat 이미지를 찾습니다.
                using (Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed)){
                    //찾은 이미지의 유사도 및 위치 값을 받습니다. 
                    Cv2.MinMaxLoc(res, out this.minval, out this.maxval, out this.minloc, out this.maxloc);
                }
                return similarityVerification(this.maxval);
            }
            else{
                Console.WriteLine("First loadImg");
                return this.targetLoadFlag;
            }
        }
        bool similarityVerification(double similer){
            //유사도를 0.7 초과로 설정합니다.
            return (similer > 0.7) ? true : false;
        }
        void click(int count=1){
            if (similarityVerification(this.maxval)){
                // Target 이미지 중앙의 좌표를 계산합니다.
                int x = this.maxloc.X + (this.targetImg.Size.Width/2);
                int y = this.maxloc.Y + (this.targetImg.Size.Height/2);
                while (count > 0){
                    SetCursorPos(x, y);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    count--;
                }
            }
            else{
                Console.WriteLine("Not Found Invaild Target");
            }
        }
        public void oneClick(){
            this.click(1);
        }
        public void doubleClick(){
            this.click(2);
        }
    }
}