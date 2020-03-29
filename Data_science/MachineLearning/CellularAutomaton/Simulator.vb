﻿Imports System.Drawing

Public Class Simulator(Of T As Individual)

    ReadOnly grid As CellEntity(Of T)()()
    ReadOnly size As Size

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i">行编号</param>
    ''' <param name="j">列编号</param>
    ''' <returns></returns>
    Default Public ReadOnly Property getCell(i As Integer, j As Integer) As CellEntity(Of T)
        Get
            If i < 0 OrElse j < 0 Then
                Return Nothing
            ElseIf i >= size.Width OrElse j >= size.Width Then
                Return Nothing
            Else
                Return grid(i)(j)
            End If
        End Get
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="size">the grid size</param>
    Sub New(size As Size, activator As Func(Of T))
        Dim grid = MAT(Of CellEntity(Of T))(size.Height, size.Width)

        Me.grid = grid
        Me.size = size

        For j As Integer = 0 To size.Width - 1
            For i As Integer = 0 To size.Height - 1
                grid(i)(j) = New CellEntity(Of T)(i, j, activator())
            Next
        Next

        For j As Integer = 0 To size.Width - 1
            For i As Integer = 0 To size.Height - 1
                Call grid(i)(j).config(Me)
            Next
        Next
    End Sub

    Public Sub Run(Optional random As Boolean = True)
        If random Then
            Call runRandom()
        Else
            Call runOrder()
        End If
    End Sub

    Private Sub runOrder()
        For j As Integer = 0 To size.Width - 1
            For i As Integer = 0 To size.Height - 1
                Call grid(i)(j).Tick()
            Next
        Next
    End Sub

    Private Sub runRandom()
        For Each cell In RandomCells()
            Call cell.Tick()
        Next
    End Sub

    Public Iterator Function RandomCells() As IEnumerable(Of CellEntity(Of T))
        Dim x As Integer() = size.Width.SeqRandom
        Dim y As Integer() = size.Height.SeqRandom

        For Each xi As Integer In x
            For Each yi As Integer In y
                Yield grid(yi)(xi)
            Next
        Next
    End Function
End Class