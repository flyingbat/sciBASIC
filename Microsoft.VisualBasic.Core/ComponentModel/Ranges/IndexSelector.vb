﻿#Region "Microsoft.VisualBasic::10a8b366dc2c8e18e8299e0babb78a67, Microsoft.VisualBasic.Core\ComponentModel\Ranges\IndexSelector.vb"

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

    '     Class OrderSelector
    ' 
    '         Properties: Desc
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: FirstGreaterThan, SelectUntilGreaterThan, SelectUntilLessThan, ToString
    ' 
    '     Structure IntTag
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: CompareTo, OrderSelector, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Serialization.JSON
Imports Numeric = Microsoft.VisualBasic.Language.Numeric

Namespace ComponentModel.Ranges

    Public Class OrderSelector(Of T As IComparable)

        ReadOnly source As T()
        ReadOnly direct$

        ''' <summary>
        ''' 是否为降序排序?
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Desc As Boolean

        Default Public ReadOnly Property value(index%) As T
            Get
                Return source(index)
            End Get
        End Property

        Shared ReadOnly compare As Func(Of T, T) = Function(x) x

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="asc">
        ''' 当这个参数为真的时候为升序排序
        ''' </param>
        Sub New(source As IEnumerable(Of T), Optional asc As Boolean = True)
            If asc Then
                source = source.OrderBy(compare).ToArray
                direct = " -> "
            Else
                source = source _
                    .OrderByDescending(compare) _
                    .ToArray
                direct = " <- "
            End If

            Desc = Not asc
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{direct}] {GetType(T).ToString}"
        End Function

        ''' <summary>
        ''' 直到当前元素大于指定值
        ''' </summary>
        ''' <param name="n"></param>
        ''' <returns></returns>
        Public Iterator Function SelectUntilGreaterThan(n As T) As IEnumerable(Of T)
            For Each x In source
                If Numeric.LessThanOrEquals(x, n) Then
                    Yield x
                Else
                    ' 由于是经过排序了的，所以在这里不再小于的话，则后面的元素都不会再比他小了
                    Exit For
                End If
            Next
        End Function

        ''' <summary>
        ''' 直到当前元素小于指定值
        ''' </summary>
        ''' <param name="n"></param>
        ''' <returns></returns>
        Public Iterator Function SelectUntilLessThan(n As T) As IEnumerable(Of T)
            For Each x In source
                If Numeric.GreaterThanOrEquals(x, n) Then
                    Yield x
                Else
                    Exit For
                End If
            Next
        End Function

        ''' <summary>
        ''' 遍历整个列表直到找到第一个大于<paramref name="o"/>的元素，然后函数会返回这第一个元素的index
        ''' </summary>
        ''' <param name="o"></param>
        ''' <returns>
        ''' 返回-1表示这个列表之中没有任何元素是大于输入的参数<paramref name="o"/>的
        ''' </returns>
        Public Function FirstGreaterThan(o As T) As Integer
            For i As Integer = 0 To source.Length - 1
                If Not Numeric.GreaterThan(o, source(i)) Then
                    Return i
                End If
            Next

            Return -1
        End Function

        Public Function SelectByRange(min As T, max As T) As IEnumerable(Of T)
            Return source.SkipWhile(Function(o) Numeric.LessThan(o, min)) _
                         .TakeWhile(Function(o) Numeric.LessThanOrEquals(o, max))
        End Function
    End Class

    Public Structure IntTag(Of T)
        Implements IComparable

        Public ReadOnly n As Integer
        Public ReadOnly x As T

        Sub New(x As T, getInt As Func(Of T, Integer))
            Me.x = x
            Me.n = getInt(x)
        End Sub

        Sub New(n As Integer)
            Me.n = n
        End Sub

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        Public Function CompareTo(obj As Object) As Integer Implements IComparable.CompareTo
            If obj Is Nothing Then
                Return 1
            Else
                If TypeOf obj Is Integer Then
                    Return n.CompareTo(DirectCast(obj, Integer))
                ElseIf TypeOf obj Is IntTag(Of T) Then
                    Return n.CompareTo(DirectCast(obj, IntTag(Of T)).n)
                Else
                    Return 0
                End If
            End If
        End Function

        Public Shared Function OrderSelector(source As IEnumerable(Of T),
                                             getInt As Func(Of T, Integer),
                                             Optional asc As Boolean = True) As OrderSelector(Of IntTag(Of T))
            Dim array As IEnumerable(Of IntTag(Of T)) = source.Select(Function(x) New IntTag(Of T)(x, getInt))
            Dim selects As New OrderSelector(Of IntTag(Of T))(array, asc)
            Return selects
        End Function

        Public Shared Widening Operator CType(n As Integer) As IntTag(Of T)
            Return New IntTag(Of T)(n)
        End Operator
    End Structure
End Namespace
