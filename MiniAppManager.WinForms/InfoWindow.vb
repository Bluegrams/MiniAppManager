Imports System.Drawing
Imports System.Resources
Imports System.Windows.Forms
Imports System.Globalization

Friend Class InfoWindow

    Private manager As MiniAppManager
    Private resources, main_resources As ResourceManager

    Private ReadOnly Property ProductWebsite As Link
        Get
            Return If(AppInfo.ProductWebsite, manager.ProductWebsite)
        End Get
    End Property

    Private ReadOnly Property ProductLicense As Link
        Get
            Return If(AppInfo.ProductLicense, manager.ProductLicense)
        End Get
    End Property

    Private ReadOnly Property ProductColor As Color
        Get
            If IsNothing(AppInfo.ProductColor) Then
                Return manager.ProductColor
            Else
                Return CType(AppInfo.ProductColor, Color)
            End If
        End Get
    End Property

    Private ReadOnly Property SupportedCultures As CultureInfo()
        Get
            Return If(AppInfo.SupportedCultures, manager.SupportedCultures)
        End Get
    End Property

    Friend Sub New(manager As MiniAppManager, productIcon As Bitmap)
        Me.manager = manager
        Me.KeyPreview = True
        InitializeComponent()
        Me.lblTitle.ForeColor = ProductColor
        Me.picIcon.BackColor = ProductColor
        InitializeText()
        If Not productIcon Is Nothing Then
            Me.picIcon.Image = productIcon
            Me.picIcon.BackColor = Color.Transparent
            Me.Icon = Icon.FromHandle(productIcon.GetHicon())
        End If
    End Sub

    Private Sub BlueInfoWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SupportedCultures.Length > 0 Then
            For Each cu As CultureInfo In SupportedCultures
                comLang.Items.Add(cu.DisplayName)
                If cu.TwoLetterISOLanguageName = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName Then
                    comLang.SelectedIndex = comLang.Items.Count - 1
                End If
            Next
        Else
            grpLanguages.Visible = False
            Me.Height -= 50
        End If
        If manager.UpdateAvailable Then
            butUpdate.Visible = True
        End If
    End Sub

    Private Sub butChangeLang_Click(sender As Object, e As EventArgs) Handles butChangeLang.Click
        If MessageBox.Show(Properties.Resources.InfoWindow_RestartNewLang, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then
            manager.ChangeCulture(SupportedCultures(comLang.SelectedIndex))
        End If
    End Sub

    Private Sub InitializeText()
        Me.Text = String.Format("{1} {0}", AppInfo.ProductName, Properties.Resources.strAbout)
        lblTitle.Text = AppInfo.ProductName
        lblName.Text = AppInfo.Title
        lblCompanyText.Text = Properties.Resources.strDeveloper
        lblCompany.Text = AppInfo.Company
        lblVersionText.Text = "Version"
        lblVersion.Text = " " & AppInfo.Version
        lblCopyright.Text = AppInfo.Copyright
        lblLicenseText.Text = Properties.Resources.strLicense
        If Not String.IsNullOrEmpty(ProductLicense.Description) Then
            lnkLicense.Text = ProductLicense.Description
            lnkLicense.Links.Add(0, lnkLicense.Text.Length, ProductLicense.Url)
        Else
            lblLicenseText.Visible = False
            lnkLicense.Visible = False
        End If
        lnkWebsite.Text = ProductWebsite.Description
        lnkWebsite.Links.Add(0, lnkWebsite.Text.Length, ProductWebsite.Url)
        butChangeLang.Text = Properties.Resources.strRestart
        grpLanguages.Text = Properties.Resources.strAppLanguage
        butUpdate.Text = Properties.Resources.strUpdate
    End Sub

    Private Sub InfoWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub butUpdate_Click(sender As Object, e As EventArgs) Handles butUpdate.Click
        manager.CheckForUpdates()
    End Sub

    Private Sub lnkWebsite_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkWebsite.LinkClicked, lnkLicense.LinkClicked
        If Not String.IsNullOrEmpty(e.Link.LinkData.ToString()) Then
            Process.Start(e.Link.LinkData)
        End If
    End Sub

End Class