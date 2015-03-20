Imports System.IO

Public Class Form2
    Dim drive As String
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        progress2.Visible = True

        Shell("cmd /c attrib -s -h -r /s /d " & drive & "*.* ", AppWinStyle.Hide)
        Timer1.Start()

        status.Text = "Scaning and Removing Virus"
        'Sh=ell("cmd /c attrib -s -h -r " & drive & "*.* /s /d", AppWinStyle.Hide)
        'MsgBox("attrib -s -h -r /s /d " & drive & "*.* ")
    End Sub
    Sub showdv()
        Dim drives() As String
        drives = Directory.GetLogicalDrives()
        Dim adrive As String
        Dim ff As Integer = 0
        ComboBox1.Items.Clear()
        For Each adrive In drives
            ComboBox1.Items.Add(adrive)
            ff += 1
        Next
        ComboBox1.SelectedIndex = ff - 1
        If CheckBox1.Checked = True Then

        End If
    End Sub
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        showdv()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        drive = ComboBox1.SelectedItem
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        On Error Resume Next
        Randomize()
        Timer1.Interval = Int((Rnd() * 10000) + 1)
        progress2.Value = progress2.Value + 10
        'If RadioButton1.Checked = True And progress2.Value = 50 Then
        '    Shell("cmd /c del /s " & drive & "*.ink", AppWinStyle.Hide)
        'End If
        If progress2.Value = 100 Then
            Timer1.Stop()
            If CheckBox1.Checked = True Or CheckBox2.Checked = True Then
                functd()
                status.Text = "Deleting Rouge Files.."
            End If

            MsgBox("Cleanup Succesful", "Shortcut Virus Remover")
            progress2.Value = 0
        End If

    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs)
        On Error Resume Next
        Randomize()
        Timer1.Interval = Int((Rnd() * 10000) + 1)
        progress2.Value = progress2.Value + 5
        'If RadioButton1.Checked = True And progress2.Value = 50 Then
        '    Shell("cmd /c del /s " & drive & "*.ink", AppWinStyle.Hide)
        'End If
        If progress2.Value = 100 Then

            Timer1.Enabled = False

        End If
    End Sub
    Sub functd()
        If My.Computer.FileSystem.FileExists(drive & "autorun.inf") Then
            My.Computer.FileSystem.DeleteFile(drive & "autorun.inf")
        End If
        If My.Computer.FileSystem.DirectoryExists(drive & "\RECYCLER") Then
            My.Computer.FileSystem.DeleteDirectory(drive & "\RECYCLER", FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
        If CheckBox1.Checked = True Then
            Shell("cmd /c del /s " & drive & "*.lnk", AppWinStyle.Hide)
            Shell("cmd /c del /s " & drive & "*.tmp", AppWinStyle.Hide)
            Shell("cmd /c del /s " & drive & "*.vbs", AppWinStyle.Hide)
            MsgBox("Operation Complete", MsgBoxStyle.Information)
            status.Text = "Completed"
            progress2.Value = 0
            progress2.Visible = False
        End If
        If CheckBox2.Checked = True Then

            If MsgBox("This Option Is St=rict=ly For A certain Usb Virus That Turns Folders to Exe files" & vbCrLf & "Make Sure Your Important files are In folders" & vbCrLf & "As it May result to loss of files" & vbCrLf & "Proceed>???", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                Shell("cmd /c del /s " & drive & "*.exe", AppWinStyle.Hide)
                System.Threading.Thread.Sleep(2000)
                MsgBox("Operation Complete", MsgBoxStyle.Information)
                progress2.Value = 0
            Else
                MsgBox("Please Untick Exe Cleanup Option in GUi", MsgBoxStyle.Critical)
                Application.Restart()
            End If


            ' MsgBox("Operation Complete", MsgBoxStyle.Exclamation)
        End If

    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim alls() As String = Directory.GetDirectories(drive)
        Dim all As String
        For Each all In alls
            'TextBox1.Text += vbNewLine & all
        Next

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        showdv()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        MsgBox("This is Another Jhony112 Works" & vbCrLf & "Helps In the Removal Of The Usb drive virus From Removable Disks" & vbCrLf & "jhony112 inc" & vbCrLf & "Thanks")
    End Sub
End Class