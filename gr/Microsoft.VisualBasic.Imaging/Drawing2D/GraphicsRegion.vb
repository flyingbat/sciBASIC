﻿#Region "Microsoft.VisualBasic::5f54cd97d7c117c0e45feaf20a766edc, gr\Microsoft.VisualBasic.Imaging\Drawing2D\GraphicsRegion.vb"

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

    '     Structure GraphicsRegion
    ' 
    '         Properties: Bottom, EntireArea, Height, PlotRegion, Width
    '                     XRange, YRange
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: scaler, TopCentra, ToString, XScaler, YScaler
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Drawing2D

    ''' <summary>
    ''' 绘图区域的参数
    ''' </summary>
    Public Structure GraphicsRegion

        ''' <summary>
        ''' 整张画布的大小
        ''' </summary>
        Public Size As Size
        ''' <summary>
        ''' 画布的边留白
        ''' </summary>
        Public Padding As Padding

        ''' <summary>
        ''' 绘图区域的底部Y坐标值
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Bottom As Integer
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Size.Height - Padding.Bottom
            End Get
        End Property

        Public ReadOnly Property Width As Integer
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Size.Width
            End Get
        End Property

        Public ReadOnly Property Height As Integer
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return Size.Height
            End Get
        End Property

        ''' <summary>
        ''' 整张画布出去margin部分剩余的可供绘图的区域
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property PlotRegion As Rectangle
            Get
                Dim topLeft As New Point(Padding.Left, Padding.Top)
                Dim size As New Size With {
                    .Width = Me.Size.Width - Padding.Horizontal,
                    .Height = Me.Size.Height - Padding.Vertical
                }

                Return New Rectangle(topLeft, size)
            End Get
        End Property

        ''' <summary>
        ''' 整张画布的大小区域
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EntireArea As Rectangle
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return New Rectangle(New Point, Size)
            End Get
        End Property

        ''' <summary>
        ''' ``[left, right]`` as <see cref="DoubleRange"/>
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property XRange As String
            Get
                With Padding
                    Return $"{ .Left},{Width - .Right}"
                End With
            End Get
        End Property

        ''' <summary>
        ''' ``[top, bottom]`` as <see cref="DoubleRange"/>
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property YRange As String
            Get
                With Padding
                    Return $"{ .Top},{Height - .Bottom}"
                End With
            End Get
        End Property

        Sub New(size As Size, padding As Padding)
            Me.Size = size
            Me.Padding = padding
        End Sub

        Sub New(padding As Padding, size As Size)
            Me.Size = size
            Me.Padding = padding
        End Sub

        Public Function TopCentra(size As Size) As Point
            Dim left = (Me.Size.Width - size.Width) / 2
            Dim top = (Padding.Top - size.Height) / 2
            Return New Point(left, top)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function XScaler(xrange As DoubleRange) As Func(Of Double, Double)
            Return scaler(xrange, Me.XRange)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function YScaler(yrange As DoubleRange) As Func(Of Double, Double)
            Dim scaler = GraphicsRegion.scaler(yrange, {0, PlotRegion.Height})
            Dim bottom = PlotRegion.Bottom

            Return Function(y) bottom - scaler(y)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Shared Function scaler(range As DoubleRange, plotRange As DoubleRange) As Func(Of Double, Double)
            Return Function(x)
                       Return range.ScaleMapping(x, plotRange)
                   End Function
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Structure
End Namespace
