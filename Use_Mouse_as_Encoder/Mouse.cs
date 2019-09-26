using System;
using RawInput_dll;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;


namespace Mouse
{
    public partial class Mouse : Form
    {
        private readonly RawInput _rawinput;
        
        const bool CaptureOnlyInForeground = true;
       

        float X = 0, Y = 0;
        float cpi = 1000, DeviceHandle = 0;
        bool CaptureHandleId = false, Track = false;
        public Mouse()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _rawinput = new RawInput(Handle, CaptureOnlyInForeground);
           
            _rawinput.AddMessageFilter();  
             Win32.DeviceAudit();            

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

















        private void Mouse_FormClosing(object sender, FormClosingEventArgs e)
        {
            _rawinput.KeyPressed -= OnKeyPressed;
        }

        private static void CurrentDomain_UnhandledException(Object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;

            if (null == ex) return;

            Debug.WriteLine("Unhandled Exception: " + ex.Message);
            Debug.WriteLine("Unhandled Exception: " + ex);
            MessageBox.Show(ex.Message);
        }

        private void Mouse_Load(object sender, EventArgs e)
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

        private void Mouse_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Mouse_KeyUp(object sender, KeyEventArgs e)
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
