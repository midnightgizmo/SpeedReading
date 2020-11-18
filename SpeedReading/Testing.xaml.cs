using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedReading
{
    /// <summary>
    /// Interaction logic for Testing.xaml
    /// </summary>
    public partial class Testing : UserControl
    {
        public Testing()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {


            DrawingContext dc = drawingContext;

            Typeface typeface = new Typeface(new FontFamily("Arial"),
                                FontStyles.Normal,
                                FontWeights.Bold,
                                FontStretches.Normal);

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                throw new InvalidOperationException("No glyphtypeface found");

            string text = "Hello, world!";
            double size = 40;

            ushort[] glyphIndexes = new ushort[text.Length];
            double[] advanceWidths = new double[text.Length];

            double totalWidth = 0;

            for (int n = 0; n < text.Length; n++)
            {
                ushort glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
                glyphIndexes[n] = glyphIndex;

                double width = glyphTypeface.AdvanceWidths[glyphIndex] * size;
                advanceWidths[n] = width;

                totalWidth += width;
            }

            Point origin = new Point(100, 500);

            GlyphRun glyphRun = new GlyphRun(glyphTypeface, 0, false, size,
                glyphIndexes, origin, advanceWidths, null, null, null, null,
                null, null);

            dc.DrawGlyphRun(Brushes.Black, glyphRun);
            /*
            double y = origin.Y;
            dc.DrawLine(new Pen(Brushes.Red, 1), new Point(origin.X, y),
                new Point(origin.X + totalWidth, y));

            y -= (glyphTypeface.Baseline * size);
            dc.DrawLine(new Pen(Brushes.Green, 1), new Point(origin.X, y),
                new Point(origin.X + totalWidth, y));

            y += (glyphTypeface.Height * size);
            dc.DrawLine(new Pen(Brushes.Blue, 1), new Point(origin.X, y),
                new Point(origin.X + totalWidth, y));
            */

            base.OnRender(drawingContext);
        }
    }
}
