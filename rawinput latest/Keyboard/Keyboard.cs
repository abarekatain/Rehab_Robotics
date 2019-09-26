using System;
using RawInput_dll;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;


namespace Keyboard
{
    public partial class Keyboard : Form
    {
        private readonly RawInput _rawinput;
        
        const bool CaptureOnlyInForeground = true;
        // Todo: add checkbox to form when checked/uncheck create method to call that does the same as Keyboard ctor 

        float X = 0, Y = 0;
        float cpi = 1000, DeviceHandle = 0;
        bool CaptureHandleId = false, Track = false;
        public Keyboard()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
           
            _rawinput.AddMessageFilter();   // Adding a message filter will cause keypresses to be handled
            Win32.DeviceAudit();            // Writes a file DeviceAudit.txt to the current directory

            _rawinput.KeyPressed += OnKeyPressed;   
        }

        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            if (CaptureHandleId)
            {
                DeviceHandle = Convert.ToInt32(e.KeyPressEvent.DeviceName);
                CaptureHandleId = false;
                lblNotification.ForeColor = System.Drawing.Color.Green;
                lblNotification.Text = "Device Registered Successfully";
            }

            if(Track)
            if (e.KeyPressEvent.DeviceName == DeviceHandle.ToString())
            {
                X += e.KeyPressEvent.LastX / cpi/0.022f;
                Y += e.KeyPressEvent.LastY / cpi/0.022f;
                    lblTotal.Text = Math.Sqrt(Math.Pow(X,2) + Math.Pow(Y,2)).ToString(); ;
                lblX.Text = X.ToString();
                lblY.Text = Y.ToString();
            }


        }

















        private void Keyboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (null == ex) return;

            // Log this error. Logging the exception doesn't correct the problem but at least now
            // you may have more insight as to why the exception is being thrown.
            Debug.WriteLine("Unhandled Exception: " + ex.Message);
            Debug.WriteLine("Unhandled Exception: " + ex);
            MessageBox.Show(ex.Message);
        }

        private void Keyboard_Load(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            X = 0;
            Y = 0;
            lblX.Text = X.ToString();
            lblY.Text = Y.ToString();
        }

        private void btnChangeState_Click(object sender, EventArgs e)
        {
            ToggleState();
        }

        private void Keyboard_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Keyboard_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                Reset();
            }
            if (e.KeyCode == Keys.S)
            {
                ToggleState();
            }
        }

        private void ToggleState()
        {
            if (Track)
            {
                Track = false;
                btnChangeState.BackColor = System.Drawing.Color.Red;
            }
            else if (!Track)
            {
                Track = true;
                btnChangeState.BackColor = System.Drawing.Color.Green;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            lblNotification.Text = "Move The Device You Want to Track!";
            lblNotification.ForeColor = System.Drawing.Color.Red;
            lblNotification.Visible = true;
            CaptureHandleId = true;
        }
    }
}
