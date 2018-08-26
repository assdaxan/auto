using System;
using System.Diagnostics;
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

namespace Auto{
    public class auto : WinApi{
        Bitmap targetImg;
        OpenCvSharp.Point minloc, maxloc;
        double minval, maxval = 0;
        bool targetLoadFlag;

        public void getProcessInfo(){
            // 테스트 코드
            // 시스템의 프로세스의 정보를 출력합니다.
            PROCESSENTRY32 procEntry = new PROCESSENTRY32();
            procEntry.dwSize = (UInt32)Marshal.SizeOf(typeof(PROCESSENTRY32));
            HANDLE handleToSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (Process32First(handleToSnapshot, ref procEntry)){
                do{
                    Console.WriteLine($"HANDLE : {getHandleFromPid(procEntry.th32ProcessID)}, PPID : {procEntry.th32ParentProcessID} PID : {procEntry.th32ProcessID} Name : {procEntry.szExeFile}");
                }while(Process32Next(handleToSnapshot, ref procEntry));
            }
        }
        public HANDLE getHandleFromPid(uint pid){
            // PID를 이용하여 핸들윈도우를 구합니다.
            HANDLE handle = OpenProcess(PROCESS_ALL_ACCESS, false, pid);
            return handle;
        }
        public int getPidFromHwnd(HWND hwnd){
            // 핸들윈도우를 이용하여 PID를 구합니다.
            GetWindowThreadProcessId(hwnd, out int a);
            return a;
        }
        public HWND getHwndFromCaption(string targetName){
            // 캡션을 이용하여 핸들윈도우를 구합니다.
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
        public bool loadImg(string path){
            try{
                this.targetImg = new Bitmap(path);
                this.targetLoadFlag = true;
            }
            catch{
                this.targetLoadFlag = false;
            }
            return this.targetLoadFlag;
        }
        protected Bitmap ScreenCopy(){
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
        bool similarityVerification(double similer){
            //유사도를 0.7 초과로 설정합니다.
            if( similer > 0.7)
                return true;
            else{
                return false;
            }
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
        protected void click(int count=1){
            if (similarityVerification(this.maxval)){
                // Target 이미지 중앙의 좌표를 계산합니다.
                int x = this.maxloc.X + (this.targetImg.Size.Width/2);
                int y = this.maxloc.Y + (this.targetImg.Size.Height/2);
                SetCursorPos(x, y);
                while (count > 0){
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