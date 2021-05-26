''' <summary>
''' jeremy johnson. modi123_1
''' dreamincode.net tutorial''' 
''' </summary>
''' <remarks></remarks>
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
            _bitmapImage = New Bitmap("red_triangle.jpg")
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
