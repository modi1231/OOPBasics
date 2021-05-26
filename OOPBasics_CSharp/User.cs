using System;
using System.Drawing;
using System.Windows.Forms;

namespace OOPBasics_CSharp
{
    class User : Entity
    {
        private MovementDirection _enumNextAction = MovementDirection.NONE;
        private Int32 _lHealth = 1;

        public MovementDirection NextAction { get { return _enumNextAction; } set { _enumNextAction = value; } }

        public User()
        {
            _pointLocation = new Point(50, 50);
            _lMovementRate = 10;
            _sID = "User";

            try
            {
                _bitmapImage = new Bitmap("blue.jpg");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, String.Format("Class: {0}", GetType().Name), MessageBoxButtons.OK);
                _bitmapImage = new Bitmap(10, 10);
            }
        }

        /// <summary>
        /// A constructor that takes in all the important bits of information to show the image on the screen.
        /// </summary>
        /// <param name="startingLocation"></param>
        /// <param name="movementRate"></param>
        /// <param name="imageLocation"></param>
        public User(Point startingLocation, Int32 movementRate, string imageLocation)
        {
            _pointLocation = startingLocation;
            _lMovementRate = movementRate;
            _bitmapImage = new Bitmap(imageLocation);
            _sID = "User";
        }

        /// <summary>
        ///     '-- Sets up drawing the image.  
        /// </summary>
        /// <param name="e"></param>
        public override void Paint(Graphics e)
        {
            e.DrawImage(_bitmapImage, _pointLocation);
        }

        public override bool ProcessState()
        {
            //'-- The only state the user needs to worry about is which direction the user wants to move.
            return DoMovement(_enumNextAction);
        }

        /// <summary>
        /// Takes the information from the user's key press and update the location.
        /// </summary>
        /// <param name="enumNextAction"></param>
        /// <returns></returns>
        private bool DoMovement(MovementDirection direction)
        {
            bool bMoveOccured = false;
            if (_enumNextAction != MovementDirection.NONE)
            {
                switch (direction)
                {

                    case MovementDirection.UP:
                        if (_pointLocation.Y - _lMovementRate >= 0) //'-- make sure user is not moving off the screen 
                        {
                            _pointLocation.Y -= _lMovementRate;
                            bMoveOccured = true;
                        }
                        else
                        {
                            _enumNextAction = MovementDirection.NONE;
                        }
                        break;
                    case MovementDirection.RIGHT:
                        if (_pointLocation.X + _bitmapImage.Width + _lMovementRate < _lScreenWidth) //'-- make sure user is not moving off the screen 
                        {
                            _pointLocation.X += _lMovementRate;
                            bMoveOccured = true;
                        }
                        else
                        {
                            _enumNextAction = MovementDirection.NONE;
                        }


                        break;
                    case MovementDirection.DOWN:
                        if (_pointLocation.Y + _bitmapImage.Height + _lMovementRate < _lScreenHeight) //'-- make sure user is not moving off the screen 
                        {
                            _pointLocation.Y += _lMovementRate;
                            bMoveOccured = true;
                        }
                        else
                        {
                            _enumNextAction = MovementDirection.NONE;
                        }
                        break;
                    case MovementDirection.LEFT:
                        if (_pointLocation.X - _lMovementRate >= 0) //'-- make sure user is not moving off the screen 
                        {
                            _pointLocation.X -= _lMovementRate;
                            bMoveOccured = true;
                        }
                        else
                        {
                            _enumNextAction = MovementDirection.NONE;
                        }

                        break;
                    case MovementDirection.NONE:
                    default:
                        bMoveOccured = false;
                        break;
                }
                if (bMoveOccured) _enumNextAction = MovementDirection.NONE;

            }

            return bMoveOccured;
        }

        override public string Print()
        {
            return   base.Print() + String.Format(" Health: {0}", _lHealth);
        }
    }
}
