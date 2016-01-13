Public Class Form2

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ListBox1.Items.Clear()
        For Each filename As String In IO.Directory.GetFiles(Form1.arkess & "\Saved\Config\WindowsNoEditor\", "Engine-arke*.ini")
            Dim fName As String = IO.Path.GetFileNameWithoutExtension(filename)
            fName = Replace(fName, "Engine-", "")
            ListBox1.Items.Add(fName)
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Not ListBox1.SelectedItem = "" Then
            'Dim FileToLoad As String = Form1.arkess & "\Saved\Config\WindowsNoEditor\Engine-" & ListBox1.SelectedItem & ".ini"
            Form1.msfile = Form1.arkess & "\Saved\Config\WindowsNoEditor\Engine-" & ListBox1.SelectedItem & ".ini"
            Form1.gsfile = Form1.arkess & "\Saved\Config\WindowsNoEditor\GameUserSettings-" & ListBox1.SelectedItem & ".ini"
            Form1.bsfile = Form1.arkess.Substring(0, Form1.arkess.Length - 12) & "\Engine\Config\BaseScalability-" & ListBox1.SelectedItem & ".ini"
            'arkess.Substring(0, arkess.Length - 12) & "\Engine\Config\BaseScalability.ini"
            'MsgBox(FileToLoad)
            Form1.DoLoadINI()
            Me.Close()
        Else
            MsgBox("No selected backup")
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not ListBox1.SelectedItem = "" Then
            Dim onyn = MsgBox("Are you absolutely sure you want to remove this backup?", MsgBoxStyle.YesNo, "RUH ROH")
            If onyn = MsgBoxResult.Yes Then
                System.IO.File.Delete(Form1.arkess & "\Saved\Config\WindowsNoEditor\Engine-" & ListBox1.SelectedItem & ".ini")
                System.IO.File.Delete(Form1.arkess & "\Saved\Config\WindowsNoEditor\GameUserSettings-" & ListBox1.SelectedItem & ".ini")
                System.IO.File.Delete(Form1.arkess.Substring(0, Form1.arkess.Length - 12) & "\Engine\Config\BaseScalability-" & ListBox1.SelectedItem & ".ini")
                ListBox1.Items.Remove(ListBox1.SelectedItem)
                MsgBox("Selected backup has been deleted")
            End If
        Else
            MsgBox("No selected backup")
        End If
    End Sub
End Class