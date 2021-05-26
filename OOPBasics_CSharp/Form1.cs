using System.Diagnostics;
using System.Windows.Forms;

/// <summary>
/// ''' modi123_1
///''' dreamincode.net tutorial'''
///''' https://www.dreamincode.net/forums/topic/245268-oop-with-video-game-basics-part-1/
//''' 2011.08.28
/// </summary>
namespace OOPBasics_CSharp
{
    public partial class Form1 : Form
    {
        private bool _IsDebug = false;// '-- turn on if you want to see information in the debug window about the object's state.
        private Timer _timer = null;//'-- the 'game engine'.
        private User _oUser;//'-- our user class
        private Enemy _oEnemy;//'-- An enemy class that follows a way point that is hard coded.

        public Form1()
        {
            InitializeComponent();
            //'-- Setup the display to not flicker and procss the graphics smoothly.
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            Height = 500;
            Width = 500;

            //'-- create our user to be drawn.
            _oUser = new User();
            _oUser.ScreenHeight = Height;// '-- to prevent moving off the screen!
            _oUser.ScreenWidth = Width;//'-- to prevent moving off the screen!
            if (_IsDebug) Debug.WriteLine(_oUser.Print());

            //'-- create our user to be drawn.
            _oEnemy = new Enemy();
            _oEnemy.ScreenHeight = Height;// '-- to prevent moving off the screen!
            _oEnemy.ScreenWidth = Width;//'-- to prevent moving off the screen!
            if (_IsDebug) Debug.WriteLine(_oEnemy.Print());

            //'-- setup our timer as the engine to process.
            _timer = new Timer();
            _timer.Interval = 100;
            _timer.Start();
            _timer.Tick += _timer_Tick;
        }

        //'-- the "game engine".
        private void _timer_Tick(object sender, System.EventArgs e)
        {
            //'-- When the engine ticks proces the input into something our user class can do.
            _oUser.ProcessState();
            if (_IsDebug) Debug.WriteLine(_oUser.Print());

            _oEnemy.ProcessState();
            if (_IsDebug) Debug.WriteLine(_oEnemy.Print());

            //'-- Redraw the scene
            Refresh();
        }

        //    '-- The take the form's input and set that action to our user object's next action on deck.
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // '-- To avoid the issue of having a stack of key presses if the user holds the key down only accept one 
            //'-- key stroke until it is processed and the graphics updated.
            if (_oUser.NextAction == Entity.MovementDirection.NONE)
            {
                switch (e.KeyCode)
                {
                    case System.Windows.Forms.Keys.Up:
                    case System.Windows.Forms.Keys.W:
                        _oUser.NextAction = Entity.MovementDirection.UP;
                        break;
                    case System.Windows.Forms.Keys.Down:
                    case System.Windows.Forms.Keys.S:
                        _oUser.NextAction = Entity.MovementDirection.DOWN;
                        break;

                    case System.Windows.Forms.Keys.Right:
                    case System.Windows.Forms.Keys.D:
                        _oUser.NextAction = Entity.MovementDirection.RIGHT;
                        break;

                    case System.Windows.Forms.Keys.Left:
                    case System.Windows.Forms.Keys.A:
                        _oUser.NextAction = Entity.MovementDirection.LEFT;
                        break;

                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            _oEnemy.Paint(e.Graphics);

            //'-- Pain the changes to the screen.
            _oUser.Paint(e.Graphics);
        }
    }
}
