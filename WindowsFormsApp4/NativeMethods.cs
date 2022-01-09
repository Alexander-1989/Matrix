using System.Runtime.InteropServices;

namespace WindowsFormsApp4
{
    static class NativeMethods
    {
        static int mouseCount = 0;

        [DllImport("user32.dll", EntryPoint = "ShowCursor")]
        static extern int _showCursor(bool show);

        public static void ShowCursor(bool show)
        {
            if ((!show && mouseCount >= 0) || (show && mouseCount < 0))
            {
                mouseCount = _showCursor(show);
            }
        }
    }
}