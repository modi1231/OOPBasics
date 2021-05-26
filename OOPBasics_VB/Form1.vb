''' <summary>
''' modi123_1
''' dreamincode.net tutorial''' 
''' https://www.dreamincode.net/forums/topic/245268-oop-with-video-game-basics-part-1/
''' 2011.08.28
''' </summary>
''' <remarks></remarks>
Public Class Form1

    Private _IsDebug As Boolean = False '-- turn on if you want to see information in the debug window about the object's state.

    Private WithEvents _timer As Timer = Nothing '-- the 'game engine'.
    Private _oUser As User '-- our user class
    Private _oEnemy As Enemy = Nothing '-- An enemy class that follows a way point that is hard coded.
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.

        '-- Setup the display to not flicker and procss the graphics smoothly.
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)

        Me.Height = 500
        Me.Width = 500

        '-- create our user to be drawn.
        _oUser = New User
        _oUser.ScreenHeight = Me.Height '-- to prevent moving off the screen!
        _oUser.ScreenWidth = Me.Width '-- to prevent moving off the screen!
        If _IsDebug Then Debug.WriteLine(_oUser.Print)

        _oEnemy = New Enemy
        _oEnemy.ScreenHeight = Me.Height '-- to prevent moving off the screen!
        _oEnemy.ScreenWidth = Me.Width '-- to prevent moving off the screen!
        If _IsDebug Then Debug.WriteLine(_oEnemy.Print)

        '-- setup our timer as the engine to process.
        _timer = New Timer
        _timer.Interval = 100
        _timer.Start()
    End Sub

    '-- the "game engine".
    Private Sub _timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles _timer.Tick
        '-- When the engine ticks proces the input into something our user class can do.
        _oUser.ProcessState()
        If _IsDebug Then Debug.WriteLine(_oUser.Print)

        _oEnemy.ProcessState()
        If _IsDebug Then Debug.WriteLine(_oEnemy.Print)

        '-- Redraw the scene
        Me.Refresh()
    End Sub

    '-- The take the form's input and set that action to our user object's next action on deck.
    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        '-- To avoid the issue of having a stack of key presses if the user holds the key down only accept one 
        '-- key stroke until it is processed and the graphics updated.
        If _oUser.NextAction = Entity.MovementDirection.NONE Then
            Select Case e.KeyCode
                Case Keys.Up, Keys.W
                    _oUser.NextAction = Entity.MovementDirection.UP
                Case Keys.Down, Keys.S
                    _oUser.NextAction = Entity.MovementDirection.DOWN
                Case Keys.Right, Keys.D
                    _oUser.NextAction = Entity.MovementDirection.RIGHT
                Case Keys.Left, Keys.A
                    _oUser.NextAction = Entity.MovementDirection.LEFT
            End Select
        End If
    End Sub

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        _oEnemy.Paint(e.Graphics)

        '-- Pain the changes to the screen.
        _oUser.Paint(e.Graphics)
    End Sub

End Class
