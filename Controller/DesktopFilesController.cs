using svchost.Model;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace svchost.Controller
{
    public class DesktopFilesController
    {
        /// <summary>
        /// List of DesktopFileItemModel describing icons at the desktop
        /// </summary>
        public List<DesktopFileItemModel> Files { get; }

        /// <summary>
        /// Bounds to the moving items
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// List of DesktopFileItemController that controling each file in Files
        /// </summary>
        private List<DesktopFileItemController> controllers;

        /// <summary>
        /// Create and initialize list of DesktopFileItemModel
        /// </summary>
        /// <param name="bounds">Bounds to the moving items</param>
        public DesktopFilesController(Rectangle bounds)
        {
            Files = WindowsSettings.GetDesktopFiles();
            Bounds = bounds;

            controllers = new List<DesktopFileItemController>();
            InitializeControllers();

            SetLocations();
        }

        /// <summary>
        /// Initialize controllers with specified bounds and maximum delta
        /// </summary>
        private void InitializeControllers()
        {
            foreach(DesktopFileItemModel file in Files)
            {
                controllers.Add(new DesktopFileItemController(file, Bounds, 50));
                Thread.Sleep(100); //It's magic, dont't touch this!
            }
        }

        /// <summary>
        /// Set the initial location of each DesktopFileItemModel
        /// </summary>
        private void SetLocations()
        {
            int width = 0;
            int height = 0;
            int margin = 30;

            foreach (DesktopFileItemModel file in Files)
            {
                file.Location = new Point(width, height);

                height += file.Image.Size.Height + margin;

                if (height + file.Image.Size.Height + margin >= Bounds.Height)
                {
                    height = margin;
                    width += file.Image.Size.Width + margin;
                }
            }

        }

        /// <summary>
        /// Move each DesktopFileItemModel at the list
        /// </summary>
        public void MoveAll()
        {
            foreach(DesktopFileItemController controller in controllers)
            {
                controller.Move();
            }
        }
    }
}
