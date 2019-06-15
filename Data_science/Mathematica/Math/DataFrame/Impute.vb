﻿#Region "Microsoft.VisualBasic::404333c4420507da6ef36d9fc44b1369, Data_science\Mathematica\Math\DataFrame\Impute.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
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



    ' /********************************************************************************/

    ' Summaries:

    ' Module Impute
    ' 
    ' 
    '     Enum InferMethods
    ' 
    '         Average, Min
    ' 
    ' 
    ' 
    '  
    ' 
    '     Function: inferByAverage, inferByMin, SimulateMissingValues, SimulateMissingValuesByProtein, SimulateMissingValuesBySample
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Module Impute

    ''' <summary>
    ''' 缺失值的模拟推断方法
    ''' </summary>
    Public Enum InferMethods
        Average
        Min
    End Enum

    <Extension>
    Public Function SimulateMissingValues(rawMatrix As IEnumerable(Of DataSet), Optional byRow As Boolean = True, Optional infer As InferMethods = InferMethods.Average) As IEnumerable(Of DataSet)
        Dim method As Func(Of Vector, Double)

        If infer = InferMethods.Average Then
            method = AddressOf inferByAverage
        Else
            method = AddressOf inferByMin
        End If

        If byRow Then
            Return rawMatrix.SimulateMissingValuesByProtein(infer:=method)
        Else
            Return rawMatrix.SimulateMissingValuesBySample(infer:=method)
        End If
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Private Function inferByAverage(iBAQ As Vector) As Double
        Return iBAQ.Average
    End Function

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Private Function inferByMin(iBAQ As Vector) As Double
        Return iBAQ.Where(Function(x) x > 0).Min
    End Function

    <Extension>
    Public Iterator Function SimulateMissingValuesByProtein(rawMatrix As IEnumerable(Of DataSet), infer As Func(Of Vector, Double)) As IEnumerable(Of DataSet)
        For Each protein As DataSet In rawMatrix
            Dim iBAQ As Vector = protein.Vector

            If iBAQ.Min = 0R Then
                ' 有缺失值
                ' 需要对缺失值使用平均值/最小值来代替
                With infer(iBAQ)
                    For Each sampleName As String In protein.EnumerateKeys
                        If protein(sampleName) = 0R Then
                            protein(sampleName) = .ByRef
                        End If
                    Next
                End With
            End If

            Yield protein
        Next
    End Function

    <Extension>
    Public Function SimulateMissingValuesBySample(rawMatrix As IEnumerable(Of DataSet), infer As Func(Of Vector, Double)) As IEnumerable(Of DataSet)
        Dim data As DataSet() = rawMatrix.ToArray
        Dim sampleNames$() = data.PropertyNames

        For Each sampleName As String In sampleNames
            Dim iBAQ As Vector = data.Vector(sampleName)

            If iBAQ.Min = 0R Then
                ' 有缺失值
                ' 需要对缺失值使用平均值/最小值来代替
                With infer(iBAQ)
                    For Each protein As DataSet In data
                        If protein(sampleName) = 0R Then
                            protein(sampleName) = .ByRef
                        End If
                    Next
                End With
            End If
        Next

        Return data
    End Function
End Module

