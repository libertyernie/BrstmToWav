﻿Imports System.IO
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

        For Each item As ListViewItem In ListView1.Items
            Using node As ResourceNode = NodeFactory.FromFile(Nothing, item.Text)
                If Not TypeOf node Is RSTMNode Then
                    Continue For
                End If

                Dim brstm = CType(node, RSTMNode)
                Dim audioStream = brstm.CreateStreams().First()
                If chk0ToStart.Checked Then
                    WAV.ToFile(audioStream,
                               txtOutputDir.Text & Path.DirectorySeparatorChar & brstm.Name & " (beginning).wav",
                               0,
                               brstm.LoopStartSample)
                End If
                If chkStartToEnd.Checked Then
                    WAV.ToFile(audioStream,
                               txtOutputDir.Text & Path.DirectorySeparatorChar & brstm.Name & " (loop).wav",
                               brstm.LoopStartSample)
                End If
                If chk0ToEnd.Checked Then
                    WAV.ToFile(audioStream,
                               txtOutputDir.Text & Path.DirectorySeparatorChar & brstm.Name & ".wav")
                End If
            End Using
        Next
    End Sub

    Private Sub btnOpenFolder_Click(sender As Object, e As EventArgs) Handles btnOpenFolder.Click
        If Not Directory.Exists(txtOutputDir.Text) Then
            MessageBox.Show("The specified output directory does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Process.Start(txtOutputDir.Text)
    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click
        MessageBox.Show("BRSTM to WAV Converter" + Environment.NewLine + "© 2015 libertyernie" + Environment.NewLine + "https://github.com/libertyernie/BrstmToWav")
    End Sub
End Class
