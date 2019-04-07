Imports System.Windows.Forms
Imports System.Drawing
Imports System.Configuration

''' <summary>
''' A class managing settings such as size and location for a WinForms application. Also includes an 'About' form for applications.
''' </summary>
Public Class MiniAppManager
    Inherits MiniAppManagerBase
    Private parent As Form
    Private sizeable As Boolean
    Private savedSize As Size
    Private savedLocation As Point
    Private savedWindowState As FormWindowState

    ''' <summary>
    ''' The settings of the manager.
    ''' </summary>
    ''' <returns></returns>
    Public Overrides ReadOnly Property Settings As ApplicationSettingsBase
        Get
            Return My.Settings
        End Get
    End Property

    ''' <summary>
    ''' Creates a New instance of MiniAppManager
    ''' </summary>
    ''' <param name="parent">The parent window of the manager. (Project's main window.)</param>
    ''' <param name="portable">Indicates whether the manager should be run in portable mode.</param>
    Sub New(parent As Form, Optional portable As Boolean = False)
        MyBase.New(parent, portable)
        Me.parent = parent
        setCulture()
    End Sub

    Private Sub setCulture()
        If Not String.IsNullOrEmpty(My.Settings.Culture) Then
            Dim culture = New System.Globalization.CultureInfo(My.Settings.Culture)
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture
            System.Threading.Thread.CurrentThread.CurrentCulture = culture
        End If
    End Sub

    ''' <summary>
    ''' Sets up the automatic saving of the window state And custom properties.
    ''' (This method should be called before the window Is initialized.)
    ''' </summary>
    Public Overrides Sub Initialize()
        MyBase.Initialize()
        If Not My.Settings.Updated Then
            My.Settings.Upgrade()
            MyBase.Upgrade()
            My.Settings.Updated = True
            My.Settings.Save()
        End If
        AddHandler parent.Load, AddressOf parent_Load
        AddHandler parent.FormClosing, AddressOf parent_FormClosing
        AddHandler parent.Move, AddressOf parent_Move
    End Sub

#Region "Parent Events"
    Private Sub parent_Load(sender As Object, e As EventArgs)
        MyBase.Parent_Loaded()
        sizeable = (parent.FormBorderStyle = FormBorderStyle.Sizable) OrElse AlwaysTrackResize
        If sizeable Then
            AddHandler parent.Resize, AddressOf parent_Resize
        End If
        parent.Location = My.Settings.Location
        If sizeable Then
            Try
                savedSize = parent.Size
                If My.Settings.Size.Width <> -1 Then parent.Size = My.Settings.Size
                parent.WindowState = My.Settings.WindowState
            Catch
                My.Settings.Reset()
                parent.WindowState = My.Settings.WindowState
            End Try
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
        MyBase.Parent_Closing()
        My.Settings.Location = savedLocation
        If sizeable Then
            My.Settings.Size = savedSize
            My.Settings.WindowState = savedWindowState
        End If
        My.Settings.Save()
    End Sub
#End Region

#Region "Update Check"

    Protected Overrides Sub OnCheckForUpdatesCompleted(e As UpdateCheckEventArgs)
        MyBase.OnCheckForUpdatesCompleted(e)
        If UpdateNotifyMode = UpdateNotifyMode.Never Then Exit Sub
        Dim always As Boolean = UpdateNotifyMode = UpdateNotifyMode.Always
        If e.NewVersion Then
            My.Settings.CheckedUpdate = e.Update.Version
            My.Settings.Save()
        End If
        If e.Successful AndAlso e.NewVersion Then
            If MessageBox.Show(parent,
                   String.Format(Application.Properties.Resources.strNewUpdate, AppInfo.ProductName, e.Update.Version),
                   Application.Properties.Resources.strSoftwareUpdate, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                Process.Start(e.Update.DownloadLink)
            End If
        ElseIf always Then
            MessageBox.Show(parent, Application.Properties.Resources.Box_NoNewUpdate,
                            Application.Properties.Resources.strSoftwareUpdate)
        End If
    End Sub
#End Region

#Region "InfoWindow"
    ''' <summary>
    ''' Changes the application culture to the given culture.
    ''' </summary>
    ''' <param name="culture">The new culture to be set.</param>
    Public Overrides Sub ChangeCulture(culture As System.Globalization.CultureInfo)
        My.Settings.Culture = culture.Name
        parent_FormClosing(Nothing, Nothing)
        If Environment.GetCommandLineArgs().Length > 1 Then
            Dim args(Environment.GetCommandLineArgs().Length - 1) As String
            Array.Copy(Environment.GetCommandLineArgs(), 1, args, 0, args.Length)
            Process.Start(Windows.Forms.Application.ExecutablePath, String.Join(" ", args))
        Else
            Process.Start(Windows.Forms.Application.ExecutablePath)
        End If
        Windows.Forms.Application.Exit()
    End Sub

    ''' <summary>
    ''' Shows an 'About' box with application information.
    ''' </summary>
    Public Overrides Sub ShowAboutBox()
        ShowAboutBox(Nothing)
    End Sub

    ''' <summary>
    ''' Shows an 'About' box with application information.
    ''' </summary>
    ''' <param name="icon">The product icon of the application.</param>
    Public Overloads Sub ShowAboutBox(icon As Bitmap)
        Dim info = New InfoWindow(Me, icon)
        info.TopMost = parent.TopMost
        info.ShowDialog()
    End Sub
#End Region

End Class
