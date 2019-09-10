using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace FQC
{
    public class UserMessageHelper
    {
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// windowapi 找到指定窗体的句柄函数
        /// </summary>
        /// <param name="lpClassName">窗体类名</param>
        /// <param name="lpWindowName">窗体标题名</param>
        /// <returns>返回窗体句柄（IntPtr）</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void SendStopPumpMessage()
        {
            IntPtr handle = FindWindow(null, "FQC压力检验工具");
            if (handle != IntPtr.Zero)
                SendMessage(handle, 0x1EE1, 0, 0);
        }

        public static void EnableContols(bool bEnable)
        {
            IntPtr handle = FindWindow(null, "FQC压力检验工具");
            if (handle != IntPtr.Zero)
            {
                if(bEnable)
                    SendMessage(handle, 0x1002, 0, 0);
                else
                    SendMessage(handle, 0x2001, 0, 0);
            }
        }

        public static void SendSetPumpOcclusionMessage()
        {
            IntPtr handle = FindWindow(null, "FQC压力检验工具");
            if (handle != IntPtr.Zero)
                SendMessage(handle, 0x1EE2, 0, 0);
        }
    }
}
