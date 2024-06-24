﻿using System;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
    public interface ILine
    {
        
    }
    
    internal enum LineType
    {
        Vertical,
        Horizontal
    }

    internal sealed class Line : Element, ILine, IStateResettable
    {
        private bool IsRendered { get; set; }
        
        public LineType Type { get; set; } = LineType.Vertical;
        public Color Color { get; set; } = Colors.Black;
        public float Thickness { get; set; } = 1;
        
        public void ResetState(bool hardReset)
        {
            IsRendered = false;
        }
        
        internal override SpacePlan Measure(Size availableSpace)
        {
            if (IsRendered)
                return SpacePlan.Empty();
            
            if (availableSpace.IsNegative())
                return SpacePlan.Wrap("The available space is negative.");

            if (Type == LineType.Vertical)
            {
                if (availableSpace.Width + Size.Epsilon < Thickness)
                    return SpacePlan.Wrap("The line thickness is greater than the available horizontal space.");

                return SpacePlan.FullRender(Thickness, 0);
            }
            
            if (Type == LineType.Horizontal)
            {
                if (availableSpace.Height + Size.Epsilon < Thickness)
                    return SpacePlan.Wrap("The line thickness is greater than the available vertical space.");

                return SpacePlan.FullRender(0, Thickness);
            }

            throw new NotSupportedException();
        }

        internal override void Draw(Size availableSpace)
        {
            if (IsRendered)
                return;
            
            if (Type == LineType.Vertical)
            {
                Canvas.DrawFilledRectangle(new Position(-Thickness/2, 0), new Size(Thickness, availableSpace.Height), Color);
            }
            else if (Type == LineType.Horizontal)
            {
                Canvas.DrawFilledRectangle(new Position(0, -Thickness/2), new Size(availableSpace.Width, Thickness), Color);
            }
            
            IsRendered = true;
        }
    }
}