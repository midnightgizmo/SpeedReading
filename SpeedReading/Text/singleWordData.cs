using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SpeedReading.Text
{
    public class singleWordData
    {

        /// <summary>
        /// Work out where the 40% mark is from left to right
        /// of the string of chars and then find the nearest
        /// char to the 40% mark
        /// </summary>
        double PercentageWhereToLookForCharInText = 40.00;
        /// <summary>
        /// This is the char position within the string
        /// to use to centre on the screen
        /// </summary>
        int charPositionUsedForMiddle;
        private string _Text;
        private double _TotalTextWidth;
        private IndividualLetterData[] charList;

        private ushort[] glyphIndexes;
        private double[] advanceWidths;

        private double _FontSize = 20.00;

        private GlyphRun glyphRun;

        Point origin;

        public void CaculateText(string text)
        {
            double StartXPosition = 0;

            _Text = text;

            Typeface typeface = new Typeface(new FontFamily("Arial"),
                                FontStyles.Normal,
                                FontWeights.Normal,
                                FontStretches.Normal);

            GlyphTypeface glyphTypeface;
            if (!typeface.TryGetGlyphTypeface(out glyphTypeface))
                throw new InvalidOperationException("No glyphtypeface found");

            double size = _FontSize;

            glyphIndexes = new ushort[text.Length];
            advanceWidths = new double[text.Length];

            charList = new IndividualLetterData[text.Length];

            _TotalTextWidth = 0;

            for (int n = 0; n < text.Length; n++)
            {
                charList[n] = new IndividualLetterData(text[n], _TotalTextWidth, size, glyphTypeface);

                glyphIndexes[n] = charList[n].GlyphIndex;
                advanceWidths[n] = charList[n].CharWidth;

                _TotalTextWidth += charList[n].CharWidth;
            }

            switch(charList.Length)
            {
                case 4:

                    charPositionUsedForMiddle = 1;
                            StartXPosition = 165 - charList[1].GetMiddlePointOfCharInRelationToHoleOfString();
                    break;

                default:

                    // find the char that falls in the 40% range
                    for (int eachChar = 0; eachChar < charList.Length; eachChar++)
                    {
                        if (charList[eachChar].IsCharWithinPercentage(_TotalTextWidth, PercentageWhereToLookForCharInText))
                        {
                            charPositionUsedForMiddle = eachChar;
                            StartXPosition = 165 - charList[eachChar].GetMiddlePointOfCharInRelationToHoleOfString();
                            break;
                        }
                    }
                    break;
            }
            


            origin = new Point(StartXPosition, 37);

            glyphRun = new GlyphRun(glyphTypeface, 0, false, size,
                glyphIndexes, origin, advanceWidths, null, null, null, null,
                null, null);



        }

        public ushort[] GlyphIndexes
        {
            get
            {
                return glyphIndexes;
            }
        }

        public double[] AdvanceWidths
        {
            get
            {
                return advanceWidths;
            }
        }

        public GlyphRun GetGlyphRun()
        {
            return glyphRun;
        }

        public int GetMiddleCharPosition
        {
            get
            {
                return charPositionUsedForMiddle;
            }
        }

        public Point PositionToPlaceText
        {
            get
            {
                return origin;
            }
        }

        public double FontSize
        {
            get
            {
                return _FontSize;
            }
        }




    }

    public class IndividualLetterData
    {
        private char _Char;
        private ushort _GlyphIndex;
        private double _CharWidth;

        private double StartPercentageWithinString;
        private double EndPercentageWithinString;

        private double _StartPositionWithinString;

        public IndividualLetterData(char Char,double StartPositionWithinString, double CharSize, GlyphTypeface glyphTypeface)
        {
            try
            {
                _Char = Char;
                _StartPositionWithinString = StartPositionWithinString;

                _GlyphIndex = glyphTypeface.CharacterToGlyphMap[_Char];
                _CharWidth = glyphTypeface.AdvanceWidths[_GlyphIndex] * CharSize;
            }
            catch
            {
                _Char = '\'';
                _StartPositionWithinString = StartPositionWithinString;

                _GlyphIndex = glyphTypeface.CharacterToGlyphMap[_Char];
                _CharWidth = glyphTypeface.AdvanceWidths[_GlyphIndex] * CharSize;
            }
        }

        public ushort GlyphIndex
        {
            get
            {
                return _GlyphIndex;
            }
        }
        
        public double CharWidth
        {
            get
            {
                return _CharWidth;
            }
        }

        public bool IsCharWithinPercentage(double TotalStringLength,double Percentage)
        {
            StartPercentageWithinString = (_StartPositionWithinString / TotalStringLength) * 100;
            EndPercentageWithinString = ((_StartPositionWithinString + _CharWidth) / TotalStringLength) * 100;

            if (Percentage >= StartPercentageWithinString && Percentage <= EndPercentageWithinString)
                return true;
            else
                return false;
        }

        public double GetMiddlePointOfCharInRelationToHoleOfString()
        {
            return _StartPositionWithinString + (_CharWidth / 2);
        }

        
    }


}
