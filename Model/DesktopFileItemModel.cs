using System;
using System.Drawing;
using System.Text;

namespace svchost.Model
{
    public class DesktopFileItemModel
    {
        /// <summary>
        /// Text to display
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Icon of the file
        /// </summary>
        public Icon Icon { get; }

        /// <summary>
        /// Size of the icon
        /// </summary>
        public Size IconSize { get; }

        /// <summary>
        /// Complete image including icon ant text
        /// </summary>
        public Bitmap Image { get; }

        /// <summary>
        /// Location of the complete image
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Bounds of the complete image
        /// </summary>
        public Rectangle ImageRectangle
        {
            get => new Rectangle(Location, Image.Size);
        }

        /// <summary>
        /// Margin from the bounds of the complete image to the icon
        /// </summary>
        private const int MARGIN = 20;

        /// <summary>
        /// Create and initialize with specified values. Generate the complete image
        /// </summary>
        /// <param name="icon">Icon of the file</param>
        /// <param name="size">Size of the icon</param>
        /// <param name="text">Text to display</param>
        public DesktopFileItemModel(Icon icon, Size size, string text)
        {
            Text = text;
            Icon = icon;
            IconSize = size;

            Image = new Bitmap(IconSize.Width + 2 * MARGIN, IconSize.Height + 2 * MARGIN);
            Image = GenerateImage();
        }

        /// <summary>
        /// Move to the new location
        /// </summary>
        /// <param name="deltaX">Horizontal shift</param>
        /// <param name="deltaY">Vertical shift</param>
        public void Move(int deltaX, int deltaY) => this.Location = new Point(this.Location.X + deltaX, this.Location.Y + deltaY);

        /// <summary>
        /// Generate the complete image
        /// </summary>
        /// <returns>Complete image</returns>
        private Bitmap GenerateImage()
        {
            Graphics graphics = Graphics.FromImage(Image);
            graphics.DrawImage(Icon.ToBitmap(), new Rectangle(new Point(MARGIN, 0), IconSize));

            DrawText(graphics);

            return Image;
        }

        /// <summary>
        /// Draw the text under the icon
        /// </summary>
        /// <param name="graphics">Graphics to draw</param>
        private void DrawText(Graphics graphics)
        {
            SizeF size = graphics.MeasureString(Text, new Font("Segoe UI", 10));

            if (size.Width > Image.Width)
            {
                string[] lines = SplitTextIntoLines(graphics);

                DrawTextLine(graphics, lines[0]);

                DrawTextLine(graphics, lines[1], true);
            }
            else
            {
                DrawTextLine(graphics, Text);
            }
        }

        /// <summary>
        /// Split text into two lines, first - by width of the complete image, second - the rest
        /// </summary>
        /// <param name="graphics">Graphics to measure string</param>
        /// <returns>Array of splitted text</returns>
        private string[] SplitTextIntoLines(Graphics graphics)
        {
            string[] result = new string[2];

            StringBuilder sb = new StringBuilder();
            var words = Text.Split(' ');

            sb.Append(words[0]);

            SizeF size = graphics.MeasureString(sb.ToString(), new Font("Segoe UI", 10));

            int i = 0;

            while (size.Width < Image.Width)
            {
                sb.Append(' ');
                sb.Append(words[++i]);
                size = graphics.MeasureString(sb.ToString(), new Font("Segoe UI", 10));
            }

            result[0] = String.Join(" ", words, 0, i);
            result[1] = Text.Substring(result[0].Length + 1);

            return result;
        }

        /// <summary>
        /// Draw one line of the text at the center
        /// </summary>
        /// <param name="graphics">Graphics of the image</param>
        /// <param name="text">Text to draw</param>
        /// <param name="isVerticalShift">Is vertical shift needed</param>
        private void DrawTextLine(Graphics graphics, string text, bool isVerticalShift = false)
        {
            SizeF size = graphics.MeasureString(text, new Font("Segoe UI", 10));

            int verticalShift = isVerticalShift ? (int)size.Height : 0;

            int x = size.Width > Image.Size.Width ? 0 : (int)(Image.Width - size.Width) / 2;
            int y = isVerticalShift ? IconSize.Height + verticalShift : IconSize.Height;

            graphics.DrawString(text, new Font("Segoe UI", 10), Brushes.White, new Point(x, y));
        }

        /// <summary>
        /// Deterine is a point is inside the bounds of the file image
        /// </summary>
        /// <param name="point">Point to check</param>
        /// <returns>True if image rectangle contains the point otherwise false</returns>
        public bool IsHit(Point point) => ImageRectangle.Contains(point);
    }
}
