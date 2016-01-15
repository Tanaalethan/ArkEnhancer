Public Class Form3

    Dim arkbuildline As String
    Dim arkautoupdate As String


    Private Sub Form3_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        TextBox1.Text = Form1.arkess

        arkbuildline = Form1.arkessini.GetKeyValue("AppSettings", "BuildLine")
        If arkbuildline = "" Then
            arkbuildline = "0"
        End If
        ComboBox1.SelectedIndex = arkbuildline

        arkautoupdate = Form1.arkessini.GetKeyValue("AppSettings", "AutoUpdate")
        If arkautoupdate = "" Then
            arkautoupdate = "false"
        End If
        CheckBox1.Checked = arkautoupdate

        Label4.Text = "Last Checked: " & Form1.ARKE_VLUT

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Form1.arkessini.SetKeyValue("SystemSettings", "programfilesname", TextBox1.Text)

        Form1.arkessini.SetKeyValue("AppSettings", "BuildLine", ComboBox1.SelectedIndex.ToString)
        Form1.arkessini.SetKeyValue("AppSettings", "AutoUpdate", CheckBox1.Checked)

        Form1.arkessini.Save(Form1.localfolder & "\arkespecialsettings.ini")

        Form1.GetINIFiles()
        Form1.DoLoadINI()

        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form1.CheckForUpdate()
        Label4.Text = "Last Checked: " & Form1.ARKE_VLUT
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Form1.ARKE_VLin = ComboBox1.SelectedIndex.ToString
    End Sub
End Class