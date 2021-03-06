﻿#Region "Microsoft.VisualBasic::218108c165c9e0fcd0f2ba888d7856f0, Data_science\MachineLearning\MachineLearning\Extensions.vb"

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

    ' Module Extensions
    ' 
    '     Function: Delta, ToDataMatrix
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.MachineLearning.StoreProcedure
Imports stdNum = System.Math

<HideModuleName> Public Module Extensions

    ''' <summary>
    ''' Generate small delta for GA mutations
    ''' </summary>
    ''' <param name="x#"></param>
    ''' <param name="d#"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 1 = 10 ^ 0  ~  0.1 = 10 ^ 1 * 0.1
    ''' 10 = 10 ^ 1  ~ 1 = 10 ^ 2 * 0.1
    ''' </remarks>
    <Extension>
    Public Function Delta(x#, Optional d# = 1 / 10) As Double
        Dim p10 = Fix(stdNum.Log10(x))
        Dim small = (10 ^ (p10 + 1)) * d
        Return small
    End Function

    ''' <summary>
    ''' Convert samples data to dataset matrix
    ''' </summary>
    ''' <typeparam name="T">The type of the target output dataset.</typeparam>
    ''' <param name="samples"></param>
    ''' <param name="names">The property names of the sample data vector.</param>
    ''' <param name="outputNames">The property names of the output vector for each sample</param>
    ''' <returns>
    ''' data layout of the populated matrix row:
    ''' 
    ''' ```
    ''' ID|names|outputNames
    ''' ```
    ''' </returns>
    <Extension>
    Public Function ToDataMatrix(Of T As {New, DynamicPropertyBase(Of Double), INamedValue})(samples As IEnumerable(Of Sample), names$(), outputNames$()) As IEnumerable(Of T)
        Dim nameIndex = names.SeqIterator.ToArray
        Dim outsIndex = outputNames.SeqIterator.ToArray

        Return samples _
            .Select(Function(sample)
                        Dim row As New T
                        Dim vector As Double() = sample.vector

                        row.Key = sample.ID
                        row.Properties = New Dictionary(Of String, Double)

                        Call nameIndex.DoEach(Sub(i) Call row.Add(i.value, vector(i)))
                        Call outsIndex.DoEach(Sub(i) Call row.Add(i.value, sample.target(i)))

                        Return row
                    End Function)
    End Function
End Module
