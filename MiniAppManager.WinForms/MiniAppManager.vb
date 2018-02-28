Imports System.Windows.Forms
Imports System.Drawing
Imports Bluegrams.Application
Imports System.Globalization

''' <summary>
''' A class managing settings such as size and location for a WinForms application. Also includes an 'About' form for applications.
''' </summary>
Public Class MiniAppManager
    Implements IMiniAppManager
    Private parent As Form
    Private sizeable As Boolean
    Private savedSize As Size
    Private savedLocation As Point
    Private savedWindowState As FormWindowState

    ''' <summary>
    ''' The project's website shown in the 'About' box.
    ''' </summary>
    ''' <returns></returns>
    Public Property ProductWebsite As Link Implements IMiniAppManager.ProductWebsite
    ''' <summary>
    ''' A link to the license, under which the project is published.
    ''' </summary>
    ''' <returns></returns>
    Public Property ProductLicense As Link Implements IMiniAppManager.ProductLicense
    ''' <summary>
    ''' A color object of the respective technology that is used for the title of the 'About' box.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ProductColorObject As Object Implements IMiniAppManager.ProductColorObject
        Get
            Return ProductColor
        End Get
    End Property
    ''' <summary>
    ''' An icon object of the respective technology that is displayed in the 'About' box.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ProductImageObject As Object Implements IMiniAppManager.ProductImageObject
        Get
            Return ProductImage
        End Get
    End Property
    ''' <summary>
    ''' A list containing cultures supported by the application.
    ''' </summary>
    ''' <returns></returns>
    Public Property SupportedCultures As CultureInfo() Implements IMiniAppManager.SupportedCultures

    ''' <summary>
    ''' The color used for the title of the 'About' box.
    ''' </summary>
    ''' <returns></returns>
    Public Property ProductColor As Color
    ''' <summary>
    ''' The icon of the project used for the 'About' box.
    ''' </summary>
    ''' <returns></returns>
    Public Property ProductImage As Bitmap

    ''' <summary>
    ''' Creates a New instance of MiniAppManager
    ''' </summary>
    ''' <param name="parent">The parent window of the manager. (Project's main window.)</param>
    Sub New(parent As Form)
        Me.New(parent, Color.Gray, Nothing)
    End Sub

    ''' <summary>
    ''' Creates a New instance of MiniAppManager
    ''' </summary>
    ''' <param name="parent">The parent window of the manager. (Project's main window.)</param>
    ''' <param name="color">The color used for the title of the 'About' box.</param>
    Sub New(parent As Form, color As Color)
        Me.New(parent, color, Nothing)
    End Sub

    ''' <summary>
    ''' Creates a New instance of MiniAppManager
    ''' </summary>
    ''' <param name="parent">The parent window of the manager. (Project's main window.)</param>
    ''' <param name="color">The color used for the title of the 'About' box.</param>
    ''' <param name="image">The icon of the project used for the 'About' box.</param>
    Sub New(parent As Form, color As Color, image As Image)
        Me.New(parent, color, image, New Link(""), New Link(""))
    End Sub

    ''' <summary>
    ''' Creates a New instance of MiniAppManager
    ''' </summary>
    ''' <param name="parent">The parent window of the manager. (Project's main window.)</param>
    ''' <param name="color">The color used for the title of the 'About' box.</param>
    ''' <param name="image">The icon of the project used for the 'About' box.</param>
    ''' <param name="website">The project's website shown in the 'About' box.</param>
    ''' <param name="license">A link to the license, under which the project Is published.</param>
    Sub New(parent As Form, color As Color, image As Image, website As Link, license As Link)
        Me.parent = parent
        ProductWebsite = website
        ProductLicense = license
        ProductColor = color
        ProductImage = image
        SupportedCultures = New CultureInfo() {}
    End Sub

    ''' <summary>
    ''' Initializes the app manager. (This method should be called before the window is initialized.)
    ''' </summary>
    Public Sub Initialize() Implements IMiniAppManager.Initialize
        If Not My.Settings.Updated Then
            My.Settings.Upgrade()
            My.Settings.Updated = True
            My.Settings.Save()
        End If
        If Not String.IsNullOrEmpty(My.Settings.Culture) Then
            Dim culture = New System.Globalization.CultureInfo(My.Settings.Culture)
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture
            System.Threading.Thread.CurrentThread.CurrentCulture = culture
        End If
        sizeable = (parent.FormBorderStyle = FormBorderStyle.Sizable)
        AddHandler parent.Load, AddressOf parent_Load
        AddHandler parent.FormClosing, AddressOf parent_FormClosing
        AddHandler parent.Move, AddressOf parent_Move
        If sizeable Then
            AddHandler parent.Resize, AddressOf parent_Resize
        End If
    End Sub

#Region "Parent Events"
    Private Sub parent_Load(sender As Object, e As EventArgs)
        parent.Location = My.Settings.Location
        If sizeable Then
            parent.Size = My.Settings.Size
            parent.WindowState = My.Settings.WindowState
        End If
        checkOutOfBorders()
    End Sub

    Private Sub checkOutOfBorders()
        Dim windowRec As New Rectangle(parent.Location, parent.Size)
        Dim screenRec = Screen.FromRectangle(windowRec).WorkingArea
        If Not screenRec.IntersectsWith(windowRec) Then
            Dim newLocation As Point = parent.Location
            If parent.Location.X < screenRec.X Then
                newLocation.X = screenRec.X
            ElseIf parent.Location.X > screenRec.Right Then
                newLocation.X = screenRec.Right - parent.Width
            End If
            If parent.Location.Y < screenRec.Y Then
                newLocation.Y = screenRec.Y
            ElseIf parent.Location.Y > screenRec.Bottom Then
                newLocation.Y = screenRec.Bottom - parent.Height
            End If
            parent.Location = newLocation
        End If
    End Sub

    Private Sub parent_Move(sender As Object, e As EventArgs)
        If parent.WindowState = FormWindowState.Normal Then savedLocation = parent.Location
    End Sub

    Private Sub parent_Resize(sender As Object, e As EventArgs)
        If parent.WindowState = FormWindowState.Minimized Then Exit Sub
        savedWindowState = parent.WindowState
        If parent.WindowState = FormWindowState.Normal Then savedSize = parent.Size
    End Sub

    Private Sub parent_FormClosing(sender As Object, e As EventArgs)
        My.Settings.Location = savedLocation
        If sizeable Then
            My.Settings.Size = savedSize
            My.Settings.WindowState = savedWindowState
        End If
        My.Settings.Save()
    End Sub
#End Region

#Region "InfoWindow"
    ''' <summary>
    ''' Changes the application culture to the given culture.
    ''' </summary>
    ''' <param name="culture">The new culture to be set.</param>
    Public Sub ChangeCulture(culture As System.Globalization.CultureInfo) Implements IMiniAppManager.ChangeCulture
        My.Settings.Culture = culture.Name
        parent_FormClosing(Nothing, Nothing)
        Process.Start(Windows.Forms.Application.ExecutablePath)
        Windows.Forms.Application.Exit()
    End Sub

    ''' <summary>
    ''' Shows an 'About' box with application information.
    ''' </summary>
    Public Sub ShowAboutBox() Implements IMiniAppManager.ShowAboutBox
        Dim info = New InfoWindow(Me)
        info.ShowDialog()
    End Sub
#End Region

End Class
