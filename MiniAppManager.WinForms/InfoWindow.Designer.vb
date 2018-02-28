<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class InfoWindow
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InfoWindow))
        Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.lnkWebsite = New System.Windows.Forms.LinkLabel()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.lblCompany = New System.Windows.Forms.Label()
        Me.lblLicenseText = New System.Windows.Forms.Label()
        Me.lblCopyright = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblCompanyText = New System.Windows.Forms.Label()
        Me.lblVersionText = New System.Windows.Forms.Label()
        Me.lnkLicense = New System.Windows.Forms.LinkLabel()
        Me.splitter = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.picIcon = New System.Windows.Forms.PictureBox()
        Me.grpLanguages = New System.Windows.Forms.GroupBox()
        Me.butChangeLang = New System.Windows.Forms.Button()
        Me.comLang = New System.Windows.Forms.ComboBox()
        Me.tableLayoutPanel1.SuspendLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpLanguages.SuspendLayout()
        Me.SuspendLayout()
        '
        'tableLayoutPanel1
        '
        resources.ApplyResources(Me.tableLayoutPanel1, "tableLayoutPanel1")
        Me.tableLayoutPanel1.Controls.Add(Me.lnkWebsite, 0, 5)
        Me.tableLayoutPanel1.Controls.Add(Me.lblVersion, 1, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.lblCompany, 1, 2)
        Me.tableLayoutPanel1.Controls.Add(Me.lblLicenseText, 0, 4)
        Me.tableLayoutPanel1.Controls.Add(Me.lblCopyright, 0, 3)
        Me.tableLayoutPanel1.Controls.Add(Me.lblName, 0, 0)
        Me.tableLayoutPanel1.Controls.Add(Me.lblCompanyText, 0, 2)
        Me.tableLayoutPanel1.Controls.Add(Me.lblVersionText, 0, 1)
        Me.tableLayoutPanel1.Controls.Add(Me.lnkLicense, 1, 4)
        Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
        '
        'lnkWebsite
        '
        resources.ApplyResources(Me.lnkWebsite, "lnkWebsite")
        Me.tableLayoutPanel1.SetColumnSpan(Me.lnkWebsite, 2)
        Me.lnkWebsite.Name = "lnkWebsite"
        Me.lnkWebsite.TabStop = True
        '
        'lblVersion
        '
        resources.ApplyResources(Me.lblVersion, "lblVersion")
        Me.lblVersion.Name = "lblVersion"
        '
        'lblCompany
        '
        resources.ApplyResources(Me.lblCompany, "lblCompany")
        Me.lblCompany.Name = "lblCompany"
        '
        'lblLicenseText
        '
        Me.lblLicenseText.AllowDrop = True
        resources.ApplyResources(Me.lblLicenseText, "lblLicenseText")
        Me.lblLicenseText.Name = "lblLicenseText"
        '
        'lblCopyright
        '
        resources.ApplyResources(Me.lblCopyright, "lblCopyright")
        Me.tableLayoutPanel1.SetColumnSpan(Me.lblCopyright, 2)
        Me.lblCopyright.Name = "lblCopyright"
        '
        'lblName
        '
        resources.ApplyResources(Me.lblName, "lblName")
        Me.tableLayoutPanel1.SetColumnSpan(Me.lblName, 2)
        Me.lblName.Name = "lblName"
        '
        'lblCompanyText
        '
        resources.ApplyResources(Me.lblCompanyText, "lblCompanyText")
        Me.lblCompanyText.Name = "lblCompanyText"
        '
        'lblVersionText
        '
        resources.ApplyResources(Me.lblVersionText, "lblVersionText")
        Me.lblVersionText.Name = "lblVersionText"
        '
        'lnkLicense
        '
        resources.ApplyResources(Me.lnkLicense, "lnkLicense")
        Me.lnkLicense.Name = "lnkLicense"
        Me.lnkLicense.TabStop = True
        '
        'splitter
        '
        Me.splitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        resources.ApplyResources(Me.splitter, "splitter")
        Me.splitter.Name = "splitter"
        '
        'lblTitle
        '
        resources.ApplyResources(Me.lblTitle, "lblTitle")
        Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.lblTitle.Name = "lblTitle"
        '
        'picIcon
        '
        Me.picIcon.BackColor = System.Drawing.SystemColors.ControlDark
        resources.ApplyResources(Me.picIcon, "picIcon")
        Me.picIcon.Name = "picIcon"
        Me.picIcon.TabStop = False
        '
        'grpLanguages
        '
        Me.grpLanguages.Controls.Add(Me.butChangeLang)
        Me.grpLanguages.Controls.Add(Me.comLang)
        resources.ApplyResources(Me.grpLanguages, "grpLanguages")
        Me.grpLanguages.Name = "grpLanguages"
        Me.grpLanguages.TabStop = False
        '
        'butChangeLang
        '
        resources.ApplyResources(Me.butChangeLang, "butChangeLang")
        Me.butChangeLang.Name = "butChangeLang"
        Me.butChangeLang.UseVisualStyleBackColor = True
        '
        'comLang
        '
        Me.comLang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.comLang.FormattingEnabled = True
        resources.ApplyResources(Me.comLang, "comLang")
        Me.comLang.Name = "comLang"
        '
        'InfoWindow
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpLanguages)
        Me.Controls.Add(Me.tableLayoutPanel1)
        Me.Controls.Add(Me.splitter)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.picIcon)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "InfoWindow"
        Me.ShowInTaskbar = False
        Me.tableLayoutPanel1.ResumeLayout(False)
        Me.tableLayoutPanel1.PerformLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpLanguages.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblCopyright As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblCompanyText As System.Windows.Forms.Label
    Private WithEvents lblVersionText As System.Windows.Forms.Label
    Private WithEvents splitter As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents picIcon As System.Windows.Forms.PictureBox
    Friend WithEvents grpLanguages As Windows.Forms.GroupBox
    Friend WithEvents butChangeLang As Windows.Forms.Button
    Friend WithEvents comLang As Windows.Forms.ComboBox
    Friend WithEvents lblLicenseText As Windows.Forms.Label
    Friend WithEvents lnkLicense As Windows.Forms.LinkLabel
    Friend WithEvents lblVersion As Windows.Forms.Label
    Friend WithEvents lblCompany As Windows.Forms.Label
    Friend WithEvents lnkWebsite As Windows.Forms.LinkLabel
End Class
