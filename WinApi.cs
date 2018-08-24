using System;
using System.Text;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using HANDLE = System.IntPtr;
using HMODULE = System.IntPtr;

namespace auto{
    public class WinApi{
        // Win API 정의
        [DllImport("user32.dll")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern HWND FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern HWND GetWindow(HWND hWnd, int wCmd);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(HWND hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        public static extern HWND GetParent(HWND hWnd);
        [DllImport("psapi.dll")]
        public static extern int GetModuleBaseName(HANDLE hProcess, HMODULE hModule, StringBuilder lpBaseName, int nSize);
        [DllImport("kernel32.dll")]
        public static extern IntPtr CreateToolhelp32Snapshot(int dwFlags, int th32ProcessID);
        [DllImport("kernel32.dll")]
        static extern HANDLE OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        // Win API에서 사용되는 변수 정의
        // mouse_event
        public const int MOUSEEVENTF_LEFTDOWN = 0x2;
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public const int MOUSEEVENTF_LEFTUP = 0x4;
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x20;
        public const int MOUSEEVENTF_MIDDLEUP = 0x40;
        public const int MOUSEEVENTF_MOVE = 0x1;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x8;
        public const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const int MOUSEEVENTF_XDOWN = 0x80;
        public const int MOUSEEVENTF_XUP = 0x100;
        public const int MOUSEEVENTF_WHEEL = 0x800;
        public const int MOUSEEVENTF_HWHEEL = 0x1000;
        // GetWindow
        public const int GW_CHILD = 0x5;
        public const int GW_ENABLEDPOPUP = 0x6;
        public const int GW_HWNDFIRST = 0x0;
        public const int GW_HWNDLAST = 0x1;
        public const int GW_HWNDNEXT = 0x2;
        public const int GW_HWNDPREV = 0x3;
        public const int GW_OWNER = 0x4;
        //CreateToolhelp32Snapshot
        public const uint TH32CS_INHERIT = 0x80000000;
        public const int TH32CS_SNAPALL = TH32CS_SNAPHEAPLIST | TH32CS_SNAPMODULE | TH32CS_SNAPPROCESS | TH32CS_SNAPTHREAD;
        public const int TH32CS_SNAPHEAPLIST = 0x1;
        public const int TH32CS_SNAPMODULE = 0x8;
        public const int TH32CS_SNAPMODULE32 = 0x10;
        public const int TH32CS_SNAPPROCESS = 0x2;
        public const int TH32CS_SNAPTHREAD = 0x4;
        public const int INVALID_HANDLE_VALUE = -1;
        public const int ERROR_BAD_LENGTH = 0x18;
    }
}