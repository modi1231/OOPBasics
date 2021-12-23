# OOPBasics
Adding the 2011 VB.NET code and C# translation from this tutorial.

https://www.dreamincode.net/forums/topic/245268-oop-with-video-game-basics-part-1/



=================
dreamincode.net tutorial backup ahead of decommissioning

 Posted 28 August 2011 - 11:05 PM 

[u][b]GITHUB[/b][/u]: https://github.com/modi1231/OOPBasics

[u][b]Topics covered:[/b][/u]
[list]
[*]graphic display
[*]video game basics
[*]engine basics
[*]object orientated programming.
[/list]

Compiled in VS 2010, 3.5.  Recommended Visual Studios 2005 or greater and framework 3.5 or greater.

[i]Preface 1: I am going to assume you know how to create a Windows Form project and add class files to it.  [/i]

[i]Preface 2: This will be a quick walk through morphing the a VB.NET form into a game engine, show casing interesting bits of form styles to make it work, and a good example of object orientated programming fundamentals.  It's not a serious engine or game programming how to, but a fun exercise.  
[/i]

Video games - gotta love them, but sometimes you just need to prototype something out fast.  It turns out with some forward thinking you can make a pretty quick proof of concept game in a .NET windows form!  

It's at this point I would highly advocate hitting up the XNA subforum and the XNA tutorials for games to be done in a proper setting through .NET.  The flexibility and reliability is greatly held there so when you are done playing with this example upgrade and move over to the game programming tutorials!

By and by, to get started let's break down what is a video game.  You have a user input, a game engine to process the current state in a consistently periodic time slice, and a video display.  With in a windows form this means your form's keydown or mouse movement is the user input, a simple timer control will provide the periodic time, and the form's paint even for the display!

Honestly, it's that simple.  

When the timer ticks (aka engine) any of our objects must process it's state and then draw itself.  The state is any processing for that object.  The user class processes where to move.  The 'enemy' processes where on the path it needs to move by itself.  The 'user' class could be adapted for attacking, using items, or feeling the effects of gravity in a jump!  

Conceptually this is how the classes interact:
[attachment=25355:game interaction.jpg]

The trick is to compartmentalize your code in to object for reuse.  In this case I have an abstract class that holds the basic information I think future classes will need to function.  The bare necessities.  An image to draw, a location, a required paint method, a required state to process, and a few other things.  

[code]
Public MustInherit Class Entity

    Public Enum MovementDirection
        NONE = 0
        UP = 1
        RIGHT = 2
        DOWN = 3
        LEFT = 4
    End Enum

    Protected _bitmapImage As Bitmap = Nothing '-- the image to be drawn.
    Protected _pointLocation As Point = Nothing '-- where on the map is the user.
    Protected _sID As String = String.Empty '-- ids are usually important
    Protected _lMovementRate As Int32 = 0 '-- how fast the location changes when a move command is issued.
    Protected _lScreenWidth As Int32 = 0 '-- used to determine the boundaries so the location is still visible.
    Property _lScreenHeight As Int32 = 0 '-- used to determine the boundaries so the location is still visible.

    Public WriteOnly Property ScreenHeight As Int32
        Set(ByVal value As Int32)
            _lScreenHeight = value
        End Set
    End Property

    Public WriteOnly Property ScreenWidth As Int32
        Set(ByVal value As Int32)
            _lScreenWidth = value
        End Set
    End Property

    ''' <summary>
    ''' Print pertinent information
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String
        Return String.Format("{0}: x={1}, y={2}", _sID, _pointLocation.X, _pointLocation.Y)
    End Function

    '-- Everything must be drawn!
    Public MustOverride Sub Paint(ByVal e As Graphics)

    '-- Everything must be provided a consistant entry point to process that state for that given time slice.
    Public MustOverride Function ProcessState() As Boolean
End Class
[/code]

I then have a 'user' and 'enemy' class that inherit from the 'entity' abstract class.  This means user and enemy have all the functionality of 'entity' and can add to it!  Future classes like items could also extend 'entity' and be mostly complete!

My 'user' class processes what action is next when the engine ticks.  This means if a user pushed left then it moves left a set number of pixels.  It's bound by the dimensions of the form so hopefully the location will not wander off the grid.

[u]Note - you will need to define the path to the blue image here.  If you don't it won't show up.[/u]

[code]
Public Class User
    Inherits Entity

    Private _enumNextAction As MovementDirection = MovementDirection.NONE
    Private _lHealth As Int32 = 1

    Public Property NextAction As MovementDirection
        Get
            Return _enumNextAction
        End Get
        Set(ByVal value As MovementDirection)
            _enumNextAction = value
        End Set
    End Property

    Public Sub New()
        _pointLocation = New Point(50, 50)
        _lMovementRate = 10
        _sID = "User"

        Try
            _bitmapImage = New Bitmap("your file path\blue.jpg")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, String.Format("Class: {0}", Me.GetType().Name))
            _bitmapImage = New Bitmap(10, 10)
        End Try
    End Sub

    ''' <summary>
    ''' A constructor that takes in all the important bits of information to show the image on the screen.
    ''' </summary>
    ''' <param name="startingLocation"></param>
    ''' <param name="movementRate"></param>
    ''' <param name="imageLocation"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal startingLocation As Point, ByVal movementRate As Int32, ByVal imageLocation As String)
        _pointLocation = startingLocation
        _lMovementRate = movementRate
        _bitmapImage = New Bitmap(imageLocation)
        _sID = "User"
    End Sub

    '-- Sets up drawing the image.  
    Public Overrides Sub Paint(ByVal e As System.Drawing.Graphics)
        e.DrawImage(_bitmapImage, _pointLocation)
    End Sub

    Public Overrides Function ProcessState() As Boolean
        '-- The only state the user needs to worry about is which direction the user wants to move.
        Return DoMovement(_enumNextAction)
    End Function

    ''' <summary>
    ''' Takes the information from the user's key press and update the location.
    ''' </summary>
    ''' <param name="direction"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DoMovement(ByVal direction As MovementDirection) As Boolean
        Dim bMoveOccured As Boolean = False

        If _enumNextAction <> MovementDirection.NONE Then
            Select Case direction
                Case MovementDirection.UP
                    If _pointLocation.Y - _lMovementRate >= 0 Then '-- make sure user is not moving off the screen 
                        _pointLocation.Y -= _lMovementRate
                        bMoveOccured = True
                    Else
                        _enumNextAction = MovementDirection.NONE
                    End If
                Case MovementDirection.DOWN
                    If _pointLocation.Y + _bitmapImage.Height + _lMovementRate < _lScreenHeight Then '-- make sure user is not moving off the screen 
                        _pointLocation.Y += _lMovementRate
                        bMoveOccured = True
                    Else
                        _enumNextAction = MovementDirection.NONE
                    End If
                Case MovementDirection.RIGHT
                    If _pointLocation.X + _bitmapImage.Width + _lMovementRate < _lScreenWidth Then '-- make sure user is not moving off the screen 
                        _pointLocation.X += _lMovementRate
                        bMoveOccured = True
                    Else
                        _enumNextAction = MovementDirection.NONE
                    End If
                Case MovementDirection.LEFT
                    If _pointLocation.X - _lMovementRate >= 0 Then '-- make sure user is not moving off the screen 
                        _pointLocation.X -= _lMovementRate
                        bMoveOccured = True
                    Else
                        _enumNextAction = MovementDirection.NONE
                    End If
                Case Else
                    bMoveOccured = False
            End Select

            If bMoveOccured Then _enumNextAction = MovementDirection.NONE

        End If
        Return bMoveOccured
    End Function

    ''' <summary>
    ''' This shows we can tack more information as we add variables to the print method already in the abstract class.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function Print() As String
        Return MyBase.Print + String.Format(" Health: {0}", _lHealth)
    End Function
End Class
[/code]

The 'enemy' class displays a different graphic and just walks a two hundred pixel path.  It's all hard coded and you can alter the class later on to make the path be different.  Notice how this is very similar in setup to the 'user' class, but different in the state processing and a few other areas.  Oh the joys of object orientated programming!

[u]Note - you will need to define the path to the red triangle image here.  If you don't it won't show up.[/u]

[code]
Public Class Enemy
    Inherits Entity

    Private _bDirection As Boolean = True
    Private _pointStart As Point = Nothing
    Private _pointEnd As Point = Nothing

    Public Sub New()
        _pointLocation = New Point(400, 50)
        _lMovementRate = 10
        _sID = "Enemy1"

        SetupPath()

        Try
            _bitmapImage = New Bitmap("your path to\red_triangle.jpg")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, String.Format("Class: {0}", Me.GetType().Name))
            _bitmapImage = New Bitmap(10, 10)
        End Try


    End Sub

    '-- determine the start and stop points for the path to follow.
    '-- This is hard coded to be just two hundred pixels below the current starting position.  
    Private Sub SetupPath()
        _pointStart = _pointLocation

        _pointEnd = New Point(_pointStart.X, _pointStart.Y + 200)
    End Sub

    Public Overrides Sub Paint(ByVal e As System.Drawing.Graphics)
        '-- Paints the image at the current location.
        e.DrawImage(_bitmapImage, _pointLocation)
    End Sub

    Public Overrides Function ProcessState() As Boolean
        '-- The only state the enemy is required to care about is if it is following the path hard coded.
        WalkPath()
        Return True
    End Function

    ''' <summary>
    ''' A simple back and forth path.  The location moves in one direction until it hits that direction, flips a boolean, and goes the opposite.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub WalkPath()

        If _bDirection Then
            If _pointLocation.Y + _lMovementRate + _bitmapImage.Height <= _pointEnd.Y Then
                _pointLocation.Y += _lMovementRate
            Else
                _bDirection = False
            End If
        Else
            If _pointLocation.Y - _lMovementRate >= _pointStart.Y Then
                _pointLocation.Y -= _lMovementRate
            Else
                _bDirection = True
            End If
        End If
    End Sub
End Class
[/code]

My form creates the user and enemy objects, reacts to keyboard input by setting the user's next action, takes care of the periodic engine processing for the user and enemy,and draws them both.

There are some critical styles set in the form's new that keep the movement smooth and not flickering.  Also notice how when the key down is pushed and held only one 'next action' is set - this prevents a backup of key presses where the 'user' object tries to catch up and process a very long series of events.  

Also notice the complexity of processing the states or graphics are all pushed into the objects.  This makes the window form very simple to read and understand!

[code]
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

[/code]

In theory this is what your screen should look like when you run the code.  
[attachment=25354:example_output.jpg]

[b][u]Zip[/u][/b]
[attachment=25356:VBNET_VidoGame_Basics_1.zip]

[u][b]Helpful links:[/b][/u]
[url="http://www.dreamincode.net/forums/forum/125-xna/"]XNA subforum[/url]
[url="http://www.dreamincode.net/forums/forum/69-game-programming/"]Game Programming[/url]
[url="http://msdn.microsoft.com/en-us/library/aa289512%28v=vs.71%29.aspx"]MSDN: Object-Oriented Programming in Visual Basic .NET[/url]

[u][b]Advance topics[/b][/u]
[list]
[*]change the images to something you created.
[*]animation in a windows form
[*]collisions between objects
[*]inventory: tracking, collecting, and using.
[*]interaction between objects (attacking and damage!)
[*]menus - drawing them
[*]more complex way-points for the enemy to follow.
[*]flesh out the enemy's constructor
[/list]
