using System;
using System.Text;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;

namespace Auto{
    public class WinApi{
        // Win API 정의
        [DllImport("user32.dll")]
        protected static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        protected static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        protected static extern HWND FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        protected static extern HWND GetWindow(HWND hWnd, int wCmd);
        [DllImport("user32.dll")]
        protected static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        protected static extern int GetWindowThreadProcessId(HWND hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        protected static extern HWND GetParent(HWND hWnd);
        [DllImport("psapi.dll")]
        protected static extern int GetModuleBaseName(HANDLE hProcess, HMODULE hModule, StringBuilder lpBaseName, int nSize);
        [DllImport("kernel32.dll")]
        protected static extern HANDLE OpenProcess(int dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        [DllImport("kernel32.dll")]
        protected static extern bool CloseHandle(HANDLE hObject);
        [DllImport("kernel32")]
        protected static extern bool Process32First(HANDLE hSnapshot, ref PROCESSENTRY32 lppe);
        [DllImport("kernel32")]
        protected static extern bool Process32Next(HANDLE hSnapshot, ref PROCESSENTRY32 lppe);
        [DllImport("kernel32.dll")]
        protected static extern HANDLE CreateToolhelp32Snapshot(int dwFlags, int th32ProcessID);

        //CreateToolhelp32Snapshot struct
        protected struct PROCESSENTRY32{
            const int MAX_PATH = 260;
            internal UInt32 dwSize;
            internal UInt32 cntUsage;
            internal UInt32 th32ProcessID;
            internal IntPtr th32DefaultHeapID;
            internal UInt32 th32ModuleID;
            internal UInt32 cntThreads;
            internal UInt32 th32ParentProcessID;
            internal Int32 pcPriClassBase;
            internal UInt32 dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            internal string szExeFile;
        }
        // Win API에서 사용되는 변수 정의
        // mouse_event
        protected const int MOUSEEVENTF_LEFTDOWN = 0x2;
        protected const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        protected const int MOUSEEVENTF_LEFTUP = 0x4;
        protected const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        protected const int MOUSEEVENTF_MIDDLEUP = 0x40;
        protected const int MOUSEEVENTF_MOVE = 0x1;
        protected const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        protected const int MOUSEEVENTF_RIGHTUP = 0x10;
        protected const int MOUSEEVENTF_XDOWN = 0x80;
        protected const int MOUSEEVENTF_XUP = 0x100;
        protected const int MOUSEEVENTF_WHEEL = 0x800;
        protected const int MOUSEEVENTF_HWHEEL = 0x1000;
        // GetWindow
        protected const int GW_CHILD = 0x5;
        protected const int GW_ENABLEDPOPUP = 0x6;
        protected const int GW_HWNDFIRST = 0x0;
        protected const int GW_HWNDLAST = 0x1;
        protected const int GW_HWNDNEXT = 0x2;
        protected const int GW_HWNDPREV = 0x3;
        protected const int GW_OWNER = 0x4;
        //CreateToolhelp32Snapshot
        protected const uint TH32CS_INHERIT = 0x80000000;
        protected const int TH32CS_SNAPALL = TH32CS_SNAPHEAPLIST | TH32CS_SNAPMODULE | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD;
        protected const int TH32CS_SNAPHEAPLIST = 0x1;
        protected const int TH32CS_SNAPMODULE = 0x8;
        protected const int TH32CS_SNAPMODULE32 = 0x10;
        protected const int TH32CS_SNAPPROCESS = 0x2;
        protected const int TH32CS_SNAPTHREAD = 0x4;
        protected const int INVALID_HANDLE_VALUE = -1;
        protected const int ERROR_BAD_LENGTH = 0x18;
        //OpenProcess
        protected const int PROCESS_ALL_ACCESS = PROCESS_CREATE_PROCESS | PROCESS_CREATE_THREAD | 
                    PROCESS_DUP_HANDLE | PROCESS_QUERY_INFORMATION | 
                    PROCESS_QUERY_LIMITED_INFORMATION | PROCESS_SET_INFORMATION | 
                    PROCESS_SET_QUOTA | PROCESS_SUSPEND_RESUME | PROCESS_TERMINATE | 
                    PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE | SYNCHRONIZE;
        protected const int PROCESS_CREATE_PROCESS = 0x80;
        protected const int PROCESS_CREATE_THREAD = 0x2;
        protected const int PROCESS_DUP_HANDLE = 0x40;
        protected const int PROCESS_QUERY_INFORMATION = 0x400;
        protected const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;
        protected const int PROCESS_SET_INFORMATION = 0x200;
        protected const int PROCESS_SET_QUOTA = 0x100;
        protected const int PROCESS_SUSPEND_RESUME = 0x800;
        protected const int PROCESS_TERMINATE = 0x1;
        protected const int PROCESS_VM_OPERATION = 0x8;
        protected const int PROCESS_VM_READ = 0x10;
        protected const int PROCESS_VM_WRITE = 0x20;
        protected const int SYNCHRONIZE = 0x100000;
    }
}