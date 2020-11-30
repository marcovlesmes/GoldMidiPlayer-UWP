using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace GoldMidiPlayer
{
    public sealed partial class DialControl : UserControl, INotifyPropertyChanged
    {
        public DialControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        private void GridManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double NewAngle = GetAngle(e.Position, RenderSize);
            
            if (!double.IsNaN(NewAngle))
            {
                Angle = NewAngle;
                Amount = (int)((Angle / 360) * 100);
            }
        }

        private int m_Amount = default(int);


        public int Amount
        {
            get { return m_Amount; }
            set { SetProperty(ref m_Amount, value); }
        }

        double m_Angle = default(double);
        public double Angle { get { return m_Angle; } set { SetProperty(ref m_Angle, value); } }

        private string m_DialName;

        public string DialName
        {
            get { return m_DialName; }
            set { SetProperty(ref m_DialName, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void SetProperty<T>(ref T storage, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(storage, value))
            {
                storage = value;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public enum Quadrants : int { nw = 2, ne = 1, sw = 4, se = 3 }
        private double GetAngle(Point touchPoint, Size circleSize)
        {
            var _X = touchPoint.X - (circleSize.Width / 2d);
            var _Y = circleSize.Height - touchPoint.Y - (circleSize.Height / 2d);
            var _Hypot = Math.Sqrt(_X * _X + _Y * _Y);
            var _Value = Math.Asin(_Y / _Hypot) * 180 / Math.PI;
            var _Quadrant = (_X >= 0) ? 
                (_Y >= 0) ? Quadrants.ne : Quadrants.se : 
                (_Y >= 0) ? Quadrants.nw : Quadrants.sw;
            switch (_Quadrant )
            {
                case Quadrants.ne: _Value = 090 - _Value; break;
                case Quadrants.nw: _Value = 270 + _Value; break;
                case Quadrants.se: _Value = 090 - _Value; break;
                case Quadrants.sw: _Value = 270 + _Value; break;
            }
            if (_Value > 144 && _Value < 220)
            {
                return double.NaN;
            }
            return _Value;
        }
    }
}
