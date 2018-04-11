Imports System.Drawing
Imports System.Resources
Imports System.Windows.Forms
Imports System.Globalization

Friend Class InfoWindow

    Private manager As MiniAppManager
    Private resources, main_resources As ResourceManager

    Friend Sub New(manager As MiniAppManager)
        Me.manager = manager
        Me.KeyPreview = True
        InitializeComponent()
        Me.lblTitle.ForeColor = manager.ProductColor
        Me.picIcon.BackColor = manager.ProductColor
        InitializeText()
        If Not manager.ProductImage Is Nothing Then
            Me.picIcon.Image = manager.ProductImage
            Me.picIcon.BackColor = Color.Transparent
            Me.Icon = Icon.FromHandle(manager.ProductImage.GetHicon())
        End If
    End Sub

    Private Sub BlueInfoWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If manager.SupportedCultures.Length > 0 Then
            For Each cu As CultureInfo In manager.SupportedCultures
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
        If MessageBox.Show(Properties.Resources.strRestartNewLang, "", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then
            manager.ChangeCulture(manager.SupportedCultures(comLang.SelectedIndex))
        End If
    End Sub

    Private Sub InitializeText()
        Me.Text = String.Format("{1} {0}", AppInfo.ProductName, Properties.Resources.strTitle)
        lblTitle.Text = AppInfo.ProductName
        lblName.Text = AppInfo.Title
        lblCompanyText.Text = Properties.Resources.lblDeveloper_Content
        lblCompany.Text = AppInfo.Company
        lblVersionText.Text = "Version"
        lblVersion.Text = " " & AppInfo.Version
        lblCopyright.Text = AppInfo.Copyright
        lblLicenseText.Text = Properties.Resources.lblLicense_Content
        If Not String.IsNullOrEmpty(manager.ProductLicense.Description) Then
            lnkLicense.Text = manager.ProductLicense.Description
            lnkLicense.Links.Add(0, lnkLicense.Text.Length, manager.ProductLicense.Url)
        Else
            lblLicenseText.Visible = False
            lnkLicense.Visible = False
        End If
        lnkWebsite.Text = manager.ProductWebsite.Description
        lnkWebsite.Links.Add(0, lnkWebsite.Text.Length, manager.ProductWebsite.Url)
        butChangeLang.Text = Properties.Resources.butRestart_Content
        grpLanguages.Text = Properties.Resources.grpLanguage_Header
        butUpdate.Text = Application.Properties.Resources.strNewUpdateTitle
    End Sub

    Private Sub InfoWindow_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub butUpdate_Click(sender As Object, e As EventArgs) Handles butUpdate.Click
        Process.Start(manager.LatestUpdate.DownloadLink)
    End Sub

    Private Sub lnkWebsite_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lnkWebsite.LinkClicked, lnkLicense.LinkClicked
        If Not String.IsNullOrEmpty(e.Link.LinkData.ToString()) Then
            Process.Start(e.Link.LinkData)
        End If
    End Sub

End Class