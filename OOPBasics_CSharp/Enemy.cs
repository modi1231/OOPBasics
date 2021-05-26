using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOPBasics_CSharp
{
    class Enemy : Entity
    {
        private bool _bDirection = true;
        private Point _pointStart;
        private Point _pointEnd;

        public Enemy()
        {
            _pointLocation = new Point(400, 50);
            _lMovementRate = 10;
            _sID = "Enemy1";

            SetupPath();

            try
            {
                _bitmapImage = new Bitmap("red_triangle.jpg");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Class: {0}", GetType().Name), MessageBoxButtons.OK);
                _bitmapImage = new Bitmap(10, 10);
            }

        }

        //'-- determine the start and stop points for the path to follow.
        //'-- This is hard coded to be just two hundred pixels below the current starting position.  

        private void SetupPath()
        {
            _pointStart = _pointLocation;

            _pointEnd = new Point(_pointStart.X, _pointStart.Y + 200);
        }

        public override void Paint(Graphics e)
        {
            //'-- Paints the image at the current location.
            e.DrawImage(_bitmapImage, _pointLocation);
        }

        public override bool ProcessState()
        {
            //'-- The only state the enemy is required to care about is if it is following the path hard coded.
            WalkPath();
            return true;

        }

        /// <summary>
        /// A simple back and forth path.  The location moves in one direction until it hits that direction, flips a boolean, and goes the opposite.
        /// </summary>
        private void WalkPath()
        {
            if (_bDirection)
            {
                if (_pointLocation.Y + _lMovementRate + _bitmapImage.Height <= _pointEnd.Y)
                    _pointLocation.Y += _lMovementRate;
                else
                    _bDirection = false;
            }
            else
            {

                if (_pointLocation.Y - _lMovementRate >= _pointStart.Y)
                    _pointLocation.Y -= _lMovementRate;
                else
                    _bDirection = true;
            }
        }
    }
}
