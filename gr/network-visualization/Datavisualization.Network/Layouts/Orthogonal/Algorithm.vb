﻿Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Math.Statistics.Linq
Imports randf = Microsoft.VisualBasic.Math.RandomExtensions
Imports stdNum = System.Math

Namespace Layouts.Orthogonal

    Friend Class Workspace

        Public g As NetworkGraph
        Public grid As Grid
        Public V As Node()
        ''' <summary>
        ''' c
        ''' </summary>
        Public cellSize As Double

        Public width As Dictionary(Of String, Double)
        Public height As Dictionary(Of String, Double)

    End Class

    Public Module Algorithm

        <Extension>
        Public Sub DoLayout(graph As NetworkGraph, gridSize As Size, Optional delta# = 0)
            Dim V As Node() = graph.vertex.ToArray
            Dim compactionDir = True
            Dim iterationCount = 90 * V.Length
            Dim T = 2 * V.Length
            Dim k = (0.2 / T) ^ (1 / iterationCount)
            Dim cellSize As Double = V.GridCellSize
            Dim grid As New Grid(gridSize, cellSize)
            Dim workspace As New Workspace With {
                .g = graph,
                .grid = grid,
                .V = V,
                .cellSize = cellSize,
                .width = V.ToDictionary(Function(n) n.label, Function(n) n.width(cellSize, delta)),
                .height = V.ToDictionary(Function(n) n.label, Function(n) n.height(cellSize, delta))
            }

            For i As Integer = 0 To iterationCount \ 2
                For j As Integer = 0 To V.Length - 1
                    ' To perform local optimization, every node is moved to a location that minimizes
                    ' the total length of its adjacent edges.
                    Dim x = graph.neighboursMedianX(V(j)) + randf.randf(-T, T)
                    Dim y = graph.neighboursMedianY(V(j)) + randf.randf(-T, T)
                    Dim cell As Point = grid.FindIndex(x, y)
                    Dim gridCell As GridCell = grid(cell)

                    ' if vj has not changed it’s place from the previous iteration then
                    If Not gridCell.node Is V(j) Then
                        Call grid.MoveNode(V(j).label, gridCell)
                    Else
                        ' Try to swap vj with nodes nearby;

                    End If
                Next

                If iterationCount Mod 9 = 0 Then
                    workspace.compact(compactionDir, 3, False)
                    compactionDir = Not compactionDir
                End If

                T = T * k
            Next

            workspace.compact(True, 3, True)
            workspace.compact(False, 3, True)

            For i As Integer = iterationCount \ 2 + 1 To iterationCount
                For j As Integer = 0 To V.Length - 1
                    Dim wj = workspace.width(V(j).label)
                    Dim hj = workspace.height(V(j).label)
                    Dim x = graph.neighboursMedianX(V(j)) + randf.randf(-T * wj, T * wj)
                    Dim y = graph.neighboursMedianY(V(j)) + randf.randf(-T * hj, T * hj)
                    Dim cell As Point = grid.FindIndex(x, y)
                    Dim gridCell As GridCell = grid(cell)

                    ' if vj has not changed it’s place from the previous iteration then
                    If gridCell.node Is V(j) Then
                        ' Try to swap vj with nodes nearby;
                    End If
                Next

                If iterationCount Mod 9 = 0 Then
                    workspace.compact(compactionDir, stdNum.Max(1, 1 + 2 * (iterationCount - i - 30) / (0.5 * iterationCount)), False)
                    compactionDir = Not compactionDir
                End If

                T = T * k
            Next
        End Sub

        <Extension>
        Private Function neighboursMedianX(g As NetworkGraph, v As Node) As Double
            Dim edges = g.GetEdges(v)
            Dim x = edges _
                .Select(Function(e)
                            If e.U Is v Then
                                Return e.V.data.initialPostion.x
                            Else
                                Return e.U.data.initialPostion.x
                            End If
                        End Function)
            Dim median = x.Median

            Return median
        End Function

        <Extension>
        Private Function neighboursMedianY(g As NetworkGraph, v As Node) As Double
            Dim edges = g.GetEdges(v)
            Dim y = edges _
                .Select(Function(e)
                            If e.U Is v Then
                                Return e.V.data.initialPostion.y
                            Else
                                Return e.U.data.initialPostion.y
                            End If
                        End Function)
            Dim median = y.Median

            Return median
        End Function
    End Module
End Namespace