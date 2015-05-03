Imports System.IO
Imports BrawlLib.SSBB.ResourceNodes
Imports System.Audio

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtOutputDir.Text = Environment.CurrentDirectory
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            For Each f In OpenFileDialog1.FileNames
                ListView1.Items.Add(f)
            Next
        End If
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        For Each item As ListViewItem In ListView1.SelectedItems
            ListView1.Items.Remove(item)
        Next
    End Sub

    Private Sub ListView1_DragEnter(sender As Object, e As DragEventArgs) Handles ListView1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Link
        End If
    End Sub

    Private Sub ListView1_DragDrop(sender As Object, e As DragEventArgs) Handles ListView1.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim filenames As String() = e.Data.GetData(DataFormats.FileDrop)
            For Each filename As String In filenames
                ListView1.Items.Add(filename)
            Next
        End If
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        If Not String.IsNullOrEmpty(txtOutputDir.Text) Then
            FolderBrowserDialog1.SelectedPath = txtOutputDir.Text
            If FolderBrowserDialog1.ShowDialog = DialogResult.OK Then
                txtOutputDir.Text = FolderBrowserDialog1.SelectedPath
            End If
        End If
    End Sub

    Private Sub btnStart_Click(sender As Object, e As EventArgs) Handles btnStart.Click
        If Not Directory.Exists(txtOutputDir.Text) Then
            MessageBox.Show("The specified output directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim wavsPerBrstm As Integer
        If chk0ToStart.Checked Then
            wavsPerBrstm += 1
        End If
        If chkStartToEnd.Checked Then
            wavsPerBrstm += 1
        End If
        If chk0ToEnd.Checked Then
            wavsPerBrstm += 1
        End If

        Using pw As New ProgressWindow(Me, "Progress", "Caption", True)
            pw.Begin(0, wavsPerBrstm * ListView1.Items.Count, 0)

            For Each item As ListViewItem In ListView1.Items
                If pw.Cancelled Then
                    pw.Finish()
                    Exit For
                End If

                Using node As ResourceNode = NodeFactory.FromFile(Nothing, item.Text)
                    If Not TypeOf node Is RSTMNode Then
                        Continue For
                    End If

                    Dim brstm = CType(node, RSTMNode)
                    Dim audioStream = brstm.CreateStreams().First()
                    If chk0ToStart.Checked Then
                        Dim filename = txtOutputDir.Text & Path.DirectorySeparatorChar & brstm.Name & " (beginning).wav"
                        pw.Caption = Path.GetFileName(filename)
                        WAV.ToFile(audioStream,
                                   filename,
                                   0,
                                   brstm.LoopStartSample)
                        pw.Update(pw.CurrentValue + 1)
                    End If
                    If chkStartToEnd.Checked Then
                        Dim filename = txtOutputDir.Text & Path.DirectorySeparatorChar & brstm.Name & " (loop).wav"
                        pw.Caption = Path.GetFileName(filename)
                        WAV.ToFile(audioStream,
                                   filename,
                                   brstm.LoopStartSample)
                        pw.Update(pw.CurrentValue + 1)
                    End If
                    If chk0ToEnd.Checked Then
                        Dim filename = txtOutputDir.Text & Path.DirectorySeparatorChar & brstm.Name & ".wav"
                        pw.Caption = Path.GetFileName(filename)
                        WAV.ToFile(audioStream,
                                   filename)
                        pw.Update(pw.CurrentValue + 1)
                    End If
                End Using
            Next
        End Using
    End Sub

    Private Sub btnOpenFolder_Click(sender As Object, e As EventArgs) Handles btnOpenFolder.Click
        If Not Directory.Exists(txtOutputDir.Text) Then
            MessageBox.Show("The specified output directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Process.Start(txtOutputDir.Text)
    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click
        Dim N = Environment.NewLine
        MessageBox.Show("BRSTM to WAV Converter" & N & "© 2015 libertyernie" & N & N & "https://github.com/libertyernie/BrstmToWav" & N & N &
                        "This program is provided as-is without any warranty, implied or otherwise. By using this program, the end user agrees to take full responsibility regarding its proper and lawful use. The authors/hosts/distributors cannot be held responsible for any damage resulting in the use of this program, nor can they be held accountable for the manner in which it is used.")
    End Sub
End Class
