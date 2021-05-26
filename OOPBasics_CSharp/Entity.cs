using System;
using System.Drawing;

namespace OOPBasics_CSharp
{
    abstract class Entity
    {
        public enum MovementDirection
        {
            NONE = 0,
            UP = 1,
            RIGHT = 2,
            DOWN = 3,
            LEFT = 4
        }

        protected Bitmap _bitmapImage = null;// '-- the image to be drawn.
        protected Point _pointLocation;//'-- where on the map is the user.
        protected string _sID = String.Empty;// '-- ids are usually important
        protected Int32 _lMovementRate = 0;// '-- how fast the location changes when a move command is issued.
        protected Int32 _lScreenWidth = 0;// '-- used to determine the boundaries so the location is still visible.
        protected Int32 _lScreenHeight = 0;// '-- used to determine the boundaries so the location is still visible.

        public int ScreenHeight { set { _lScreenHeight = value; } }
        public int ScreenWidth { set { _lScreenWidth = value; } }

        /// <summary>
        /// Print pertinent information
        /// </summary>
        /// <returns></returns>
        virtual public string Print()
        {
            return String.Format("{0}: x={1}, y={2}", _sID, _pointLocation.X, _pointLocation.Y);
        }

        //    '-- Everything must be drawn!
        public abstract void Paint(Graphics e );

        //    '-- Everything must be provided a consistant entry point to process that state for that given time slice.
        public abstract bool ProcessState();
    }
}
