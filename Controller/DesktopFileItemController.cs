using svchost.Model;
using System;
using System.Drawing;

namespace svchost.Controller
{
    public class DesktopFileItemController
    {
        /// <summary>
        /// DesktopFileItemModel for controling
        /// </summary>
        public DesktopFileItemModel File { get; }

        /// <summary>
        /// Horizontal shift
        /// </summary>
        public int DeltaX { get; private set; }

        /// <summary>
        /// Vertical shift
        /// </summary>
        public int DeltaY { get; private set; }

        /// <summary>
        /// Bounds to the moving File
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Create and initialize File with specified value, shifts initialize randomly
        /// </summary>
        /// <param name="file">File for controlling</param>
        /// <param name="bounds">Bounds to the moving File</param>
        /// <param name="maxDelta">Maximum value of shift</param>
        public DesktopFileItemController(DesktopFileItemModel file, Rectangle bounds, int maxDelta)
        {
            File = file;

            Random random = new Random();
            DeltaX = random.Next(-maxDelta, maxDelta);
            DeltaY = random.Next(-maxDelta, maxDelta);

            Bounds = bounds;
        }

        /// <summary>
        /// Move file to the new location according to horizontal and vertical shifts
        /// </summary>
        public void Move()
        {
            if (File.Location.X < Bounds.Location.X || File.Location.X > Bounds.Location.X + Bounds.Width)
                DeltaX = -DeltaX;

            if (File.Location.Y < Bounds.Location.Y || File.Location.Y > Bounds.Location.Y + Bounds.Height)
                DeltaY = -DeltaY;

            File.Move(DeltaX, DeltaY);
        }
    }
}
