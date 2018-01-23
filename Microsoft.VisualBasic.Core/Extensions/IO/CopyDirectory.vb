﻿#Region "Microsoft.VisualBasic::806df24a72c2d82c573948ee8e9e9101, ..\sciBASIC#\Microsoft.VisualBasic.Core\Extensions\IO\CopyDirectory.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.IO
Imports Microsoft.VisualBasic.Language.UnixBash

Namespace FileIO

    Public Class CopyDirectoryAction

        ReadOnly progress As IProgress(Of String)

        Sub New(progress As Progress(Of String))
            Me.progress = progress
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="src$"></param>
        ''' <param name="destination$"></param>
        Public Sub Copy(src$, destination$)
            Dim directory As New DirectoryInfo(src)

            If FileIO.Directory.Exists(Path.Combine(destination, directory.Name)) Then
                Call $"Directory '{directory.Name}' already exists in '{destination}'".Warning
            End If

            destination = CreateDestinationFolderAndReturnNewPath(src, destination)

            Call CopyFilesToTargetDirectory(src, destination)
            Call CopySubDirectoriesWithFiles(src, destination)
        End Sub

        Private Shared Function CreateDestinationFolderAndReturnNewPath(src As String, destinationFolder$) As String
            Dim directory As New DirectoryInfo(src)
            Dim path = IO.Path.Combine(destinationFolder, directory.Name)
            Call path.MkDIR
            Return path
        End Function

        Private Sub CopyFilesToTargetDirectory(src As String, destinationFolder$)
            Dim resultFilePath$

            ' 2017-12-24
            ' 因为只是复制当前的文件夹的文件，所以在这里就不需要添加-r递归参数了
            For Each path As String In ls - l - "*.*" <= src
                With New FileInfo(path)
                    resultFilePath = IO.Path.Combine(destinationFolder, IO.Path.GetFileName(.Name))
                End With

                Call progress.Report(resultFilePath)
                Call path.FileCopy(resultFilePath)
            Next
        End Sub

        Private Sub CopySubDirectoriesWithFiles(pathToSourceFolder As String, pathToDestinationFolder As String)
            For Each subDirectory As String In IO.Directory.GetDirectories(pathToSourceFolder)
                Copy(subDirectory, pathToDestinationFolder)
            Next
        End Sub
    End Class
End Namespace