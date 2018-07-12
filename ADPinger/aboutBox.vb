Public NotInheritable Class aboutBox

    Private Sub aboutBox_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the title of the form.
        lblWebsite.Focus()
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)
        ' Initialize all of the text displayed on the About Box.
        ' TODO: Customize the application's assembly information in the "Application" pane of the project 
        '    properties dialog (under the "Project" menu).
        'Me.lblName.Text = My.Application.Info.ProductName
        'Me.lblVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        'Me.lblCopyRight.Text = My.Application.Info.Copyright
        'Me.lblUS.Text = My.Application.Info.CompanyName
        'Me.txtDescription.Text = My.Application.Info.Description
        btnClose.Focus()
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click, btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnDonate_Click(sender As Object, e As EventArgs) Handles btnDonate.Click
        Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=YMWU8YBV8FBV6")
    End Sub

    Private Sub lblWebSite_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles lblWebSite.LinkClicked
        Diagnostics.Process.Start("http://techthoughts.jakemorrison.name/adpinger")
    End Sub
End Class
