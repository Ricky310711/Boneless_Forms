using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomBonedForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            // Top handle
            FormBone(TopBone);
            FormControlBox(Exit, Maximize, Minimize);
            FormCreateIcon(Icon, this.Text, true);

            // Bottom handle
            FormBone(BottomBone);
            FormControlBox(BottomExit, BottomMaximize, BottomMinimize);
            FormCreateIcon(BottomIcon, null, true);

            // Left handle
            FormBone(LeftBone);
            FormControlBox(LeftExit, LeftMaximize, LeftMinimize);
            FormCreateIcon(LeftIcon, null, true);

            // Right handle
            FormBone(RightBone);
            FormControlBox(RightExit, RightMaximize, RightMinimize);
            FormCreateIcon(RightIcon, null, true);
        }

        // Assigns control with drag and doubleclick to maximize capabillities - USAGE: FormBone(NameOfControl);
        private void FormBone(Control Control) { Control.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown); }

        // Drag syntax
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        // Handles mousedown drag event and doubleclick to maximize
        private void DragForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ReleaseCapture();
                    SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                    if (this.Location.Y == 0) { this.WindowState = FormWindowState.Maximized; }
                }
            }
            else { if (this.WindowState == FormWindowState.Normal) { this.WindowState = FormWindowState.Maximized; } else { this.WindowState = FormWindowState.Normal; } }
        }

        // Assigns controls as form control boxes), i.e exit, maximize, minimize - USAGE: FormControlBox(NameOfControl, "exit")
        private void FormControlBox(Control ExitControl, Control MaximizeControl, Control MinimizeControl)
        {
            if (ExitControl != null) ExitControl.Click += new System.EventHandler(this.FormControlExit);
            if (MaximizeControl != null) MaximizeControl.Click += new System.EventHandler(this.FormControlMaximize);
            if (MinimizeControl != null) MinimizeControl.Click += new System.EventHandler(this.FormControlMinimize);
        }

        // Form control exit
        private void FormControlExit(object sender, EventArgs e) { Application.Exit(); }
        // Form control maximize
        private void FormControlMaximize(object sender, EventArgs e) { if (this.WindowState == FormWindowState.Normal) { this.WindowState = FormWindowState.Maximized; } else { this.WindowState = FormWindowState.Normal; } }
        // Form control minimize
        private void FormControlMinimize(object sender, EventArgs e) { this.WindowState = FormWindowState.Minimized; }

        // Specifies an icon-like control for the form
        private void FormCreateIcon(PictureBox IconControl, string FormName, bool HasContext)
        {
            IconControl.SizeMode = PictureBoxSizeMode.Zoom;
            if (IconControl.Image != null) IconControl.Image.Dispose();
            IconControl.Image = CustomBonedForm.Properties.Resources.FavoritesSelected;

            // Creates context, positions and assigns 
            if (HasContext == true)
            {
                IconControl.Click += new System.EventHandler(this.FormIconCick);

                MenuItem iconMinimize = new MenuItem();
                iconMinimize.Text = "Minimize";
                iconMinimize.Click += new System.EventHandler(this.FormControlMinimize);

                MenuItem iconMaximize = new MenuItem();
                iconMaximize.Text = "Maximize";
                iconMaximize.Click += new System.EventHandler(this.FormControlMaximize);

                MenuItem iconExit = new MenuItem();
                iconExit.Text = "Close";
                iconExit.Shortcut = Shortcut.AltF4;
                iconExit.Click += new System.EventHandler(this.FormControlExit);

                ContextMenu iconContext = new ContextMenu();
                iconContext.MenuItems.Add(iconMinimize);
                iconContext.MenuItems.Add(iconMaximize);
                iconContext.MenuItems.Add("-");
                iconContext.MenuItems.Add(iconExit);
                IconControl.ContextMenu = iconContext;
            }

            if (FormName != null && !string.IsNullOrWhiteSpace(FormName))
            {
                Label FormNameText = new Label();
                FormNameText.Text = FormName;
                FormNameText.Anchor = AnchorStyles.Left;
                IconControl.Parent.Controls.Add(FormNameText);
                FormNameText.BringToFront();
                if (IconControl.Dock == DockStyle.Left) { FormNameText.Location = new Point(IconControl.Location.Y + IconControl.Width, IconControl.Location.X + IconControl.Size.Height / 4); }
                if (IconControl.Dock == DockStyle.Right) { FormNameText.Location = new Point(IconControl.Location.Y - FormNameText.Width, IconControl.Location.X + IconControl.Size.Height / 4); }
                FormBone(FormNameText);

                // this line is optional
                FormNameText.BackColor = Color.FromArgb(30, 30, 30); FormNameText.ForeColor = Color.White;
            }

        }

        // Displays context menu
        private void FormIconCick(object sender, EventArgs e)
        {
            ((PictureBox)sender).ContextMenu.Show(this, new Point(((PictureBox)sender).Location.X, ((PictureBox)sender).Location.Y + ((PictureBox)sender).Height));
        }



        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> NONE BELOW IS NEEDED


        private void HideAll()
        {
            TopBone.Visible = false;
            BottomBone.Visible = false;
            LeftBone.Visible = false;
            RightBone.Visible = false;
        }


        private void Top_Click(object sender, EventArgs e)
        {
            HideAll();
            TopBone.Visible = true;
        }

        private void Right_Click(object sender, EventArgs e)
        {
            HideAll();
            RightBone.Visible = true;
        }

        private void Left_Click(object sender, EventArgs e)
        {
            HideAll();
            LeftBone.Visible = true;
        }

        private void Bottom_Click(object sender, EventArgs e)
        {
            HideAll();
            BottomBone.Visible = true;
        }
    }
}
