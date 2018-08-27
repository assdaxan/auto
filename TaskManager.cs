using System;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;

namespace Auto{
    using HWND = System.IntPtr;
    using HANDLE = System.IntPtr;
    using HMODULE = System.IntPtr;
    class TaskManager : WinApi{
        public void processKill(string name){
            name = name.Split('.').GetValue(0) as string;
            foreach(Process process in Process.GetProcessesByName(name) ){
                process.Kill();
            }
        }
        public void processKill(int pid){
            if (pid != 0){
                using (Process process = Process.GetProcessById(pid))
                    process.Kill();
            }
            else{
                Console.WriteLine("Not Found PrecessId");
            }
        }
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
            // PID를 이용하여 핸들을 구합니다.
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
        public bool setForegroundWindow(HWND hwnd){
            return SetForegroundWindow(hwnd);
        }
    }
}