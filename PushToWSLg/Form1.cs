using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PushToWSLg
{
    public partial class PushToWSLg : Form
    {
        HotKey hotKey;

        // caret position
        [DllImport("user32.dll", EntryPoint = "GetCaretPos")]
        static extern bool GetCaretPos(out Point lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, bool fAttach);

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetCurrentThreadId();

        [DllImport("user32.dll")]
        static extern IntPtr GetFocus();

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hwnd, out Point lpPoint);

        public PushToWSLg()
        {
            InitializeComponent();
        }

        public PushToWSLg instance;

        private void Form1_Load(object sender, EventArgs e)
        {
            hotKey = new HotKey(MOD_KEY.ALT | MOD_KEY.CONTROL | MOD_KEY.ALT, Keys.H);
            hotKey.HotKeyPush += new EventHandler(hotKey_HotKeyPush);
        }

        void hotKey_HotKeyPush(object sender, EventArgs e)
        {
            // get caret xy
            IntPtr hWnd = GetForegroundWindow();

            IntPtr current = GetCurrentThreadId();
            IntPtr target = GetWindowThreadProcessId(hWnd, IntPtr.Zero);

            Point p;
            AttachThreadInput(current, target, true);

            GetCaretPos(out p);
            IntPtr fWnd = GetFocus();
            ClientToScreen(fWnd, out p);

            AttachThreadInput(current, target, false);

            // get mouse cursor position
            //POINT cursorPos;
            //GetCursorPos(out cursorPos);

            //int x = cursorPos.X - this.Width;
            //int y = cursorPos.Y - this.Height;

            int x = p.X - this.Width;
            int y = p.Y - this.Height;

            if (x<0) { x = 0; }
            if (y<0) { y = 0; }
            
            // set right bottom corner to cursor
            this.SetDesktopLocation(x,y);

            this.Activate();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            hotKey.Dispose();
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        // カーソル位置にクリップボードのテキストを挿入
        private void InsertClipboardTextAtCursor(string text)
        {


            Clipboard.SetText(text);
            //Clipboard.SetDataObject(text, false);

            SendKeyEventToAnotherWindow("^v");


        }

        // send key event
        private async void SendKeyEventToAnotherWindow(string etext)
        {
            POINT cursorPos;
            this.Hide();

            await Task.Delay(200);

            if (GetCursorPos(out cursorPos))
            {
                // カーソル位置にクリップボードのテキストを挿入
                SendKeys.SendWait(etext);
            }

            this.Show();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int textlen = textBox1.Text.Length;
            string text = "";
            bool flagpaste = true;

            // enter, arrow, backspace

            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    text = textBox1.Text;
                    textBox1.Clear();
                    if (textlen == 0)
                    {
                        text += e.KeyChar;
                    }
                    e.Handled = true;
                    break;
                default:
                    flagpaste = false;
                    break;
            }

            if (flagpaste)
            {
                InsertClipboardTextAtCursor(text);
            }
            
            /*
            if (e.KeyChar == (char)Keys.Enter)
            {
                string text = textBox1.Text;
                textBox1.Clear();

                if (text.Length == 0)
                {
                    text += (char)Keys.Enter;
                }
                
                InsertClipboardTextAtCursor(text);

                e.Handled = true;

            }
            */
            
        }

        private void textBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            int textlen = textBox1.Text.Length;
            string etext = "";

            // enter, arrow, backspace
            if (textlen == 0)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        etext = "{UP}";
                        break;
                    case Keys.Down:
                        etext = "{DOWN}";
                        break;
                    case Keys.Left:
                        etext = "{LEFT}";
                        break;
                    case Keys.Right:
                        etext = "{RIGHT}";
                        break;
                    case Keys.Back:
                        etext = "{BS}";
                        break;
                    default:
                        break;
                }
                if (etext.Length != 0)
                {
                    SendKeyEventToAnotherWindow(etext);
                }

            }

        }

    }
}
