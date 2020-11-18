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

namespace SpeedReading.Text
{
    /// <summary>
    /// Interaction logic for DrawTextControl.xaml
    /// </summary>
    public partial class DrawTextControl : UserControl
    {
        private singleWordData aWord;
        public DrawTextControl()
        {
            aWord = new singleWordData();
            aWord.CaculateText(" ");
            InitializeComponent();

            this.Height = 50;
            this.Width = 339;
            
        }

        private string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;

                aWord.CaculateText(_Text);
                this.InvalidateVisual();

                this.txt.FontSize = aWord.FontSize;
                this.txt.Inlines.Clear();
                for (int i = 0; i < _Text.Length; i++)
                {
                    Run aRun;

                    aRun = new Run(_Text[i].ToString());
                    //aRun.FontWeight = FontWeights.Bold;

                    if (aWord.GetMiddleCharPosition == i)
                        aRun.Foreground = Brushes.Red;

                    this.txt.Inlines.Add(aRun);

                }

                this.Margin = new Thickness(aWord.PositionToPlaceText.X,aWord.PositionToPlaceText.Y-15,0,0);
                
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {

            //drawingContext.DrawGlyphRun(Brushes.Black, aWord.GetGlyphRun());

            base.OnRender(drawingContext);
        }
    }
}
