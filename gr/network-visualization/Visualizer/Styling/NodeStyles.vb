﻿#Region "Microsoft.VisualBasic::6e4c17725a3acad275e7a2f429d7d6f9, gr\network-visualization\Visualizer\Styling\NodeStyles.vb"

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

'     Module NodeStyles
' 
'         Function: ColorExpression, ColorFromTypes, (+2 Overloads) DegreeAsSize, MapExpressionParser, SizeExpression
'                   ValDegreeAsSize
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports names = Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic.NameOf
Imports r = System.Text.RegularExpressions.Regex

Namespace Styling

    Public Module NodeStyles

        <Extension> Public Function DegreeAsSize(nodes As IEnumerable(Of Node),
                                                 getDegree As Func(Of Node, Double),
                                                 sizeRange As DoubleRange) As Map(Of Node, Double)()
            Return nodes.RangeTransform(getDegree, sizeRange)
        End Function

        <Extension>
        Public Function DegreeAsSize(nodes As IEnumerable(Of Node), sizeRange As DoubleRange, Optional degree$ = names.REFLECTION_ID_MAPPING_DEGREE) As Map(Of Node, Double)()
            Dim valDegree = Function(node As Node)
                                Return node.Data(degree).ParseDouble
                            End Function
            Return nodes.DegreeAsSize(
                getDegree:=valDegree,
                sizeRange:=sizeRange
            )
        End Function

        ''' <summary>
        ''' 根据节点类型来赋值颜色值
        ''' </summary>
        ''' <param name="nodes">
        ''' 要求节点对象模型之中必须要具备有<see cref="names.REFLECTION_ID_MAPPING_NODETYPE"/>这个动态属性值
        ''' </param>
        ''' <param name="schema$"></param>
        ''' <returns></returns>
        <Extension>
        Public Function ColorFromTypes(nodes As IEnumerable(Of Node), schema$) As Map(Of Node, Color)()

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="expression$">
        ''' + rgb(x,x,x,x)|#xxxxxx|xxxxxxx 所有的节点都使用相同的颜色
        ''' + 映射类型                
        '''    1. map(property, Continuous, schemaName, 250)，连续的数值型的映射
        '''    2. map(property, Continuous, levels, startColor, endColor), 连续数值型的渐变映射
        '''    3. map(property, Discrete, color1, color2, color3, color4, ...)，分类型的颜色离散映射
        ''' + val(propertyName) 直接属性映射：即属性的值就是颜色值
        ''' </param>
        ''' <returns></returns>
        Public Function ColorExpression(expression$) As Func(Of Node(), Map(Of Node, Color)())
            If expression.IsColorExpression Then
                Dim color As Color = expression.TranslateColor
                Return Function(nodes)
                           Return nodes _
                               .Select(Function(n)
                                           Return New Map(Of Node, Color) With {
                                               .Key = n,
                                               .Maps = color
                                           }
                                       End Function) _
                               .ToArray
                       End Function
            ElseIf expression.MatchPattern("map\(.+\)", RegexICSng) Then
                ' 先match rgb表达式，再执行替换之后，再正常的解析
                ' 网络之中的graph模型对象的颜色映射有三种类型：
                ' map(property, Continuous, schemaName, 250)，连续的数值型的映射
                ' map(property, Continuous, levels, startColor, endColor), 连续数值型的渐变映射
                ' map(property, Discrete, color1, color2, color3, color4, ...)，分类型的颜色离散映射
                '
                Dim rgbs = r.Matches(expression, rgbExpr, RegexICSng) _
                    .ToArray _
                    .Distinct _
                    .ToDictionary(Function(key) key.MD5)
                For Each hashValue In rgbs
                    With hashValue
                        expression = expression.Replace(.Value, .Key)
                    End With
                Next

                ' 解析映射表达式字符串
                Dim t = expression.MapExpressionParser

                If t.type = MapperTypes.Continuous Then
                    Dim colors As Color()

                    If (Not t.values(0).IsColorExpression) AndAlso t.values(1).MatchPattern(RegexpDouble) Then
                        ' map(property, Continuous, schemaName, 250)
                        ' 使用colorbrewer生成颜色谱
                        colors = Designer.GetColors(t.values(Scan0), Val(t.values(1)))
                    Else
                        Dim colorValues$() = t _
                            .values _
                            .Select(Function(c)
                                        Return If(rgbs.ContainsKey(c), rgbs(c), c)
                                    End Function) _
                            .ToArray
                        Dim min$ = colorValues(1), max$ = colorValues(2)  ' 和graph对象的属性值等级相关的连续渐变映射
                        Dim levels% = Val(colorValues(0))
                        Dim startColor = min.TranslateColor
                        Dim endColor = max.TranslateColor
                        Dim middle = GDIColors.Middle(startColor, endColor)

                        ' 进行颜色三次插值获取渐变结果
                        colors = {startColor, middle, endColor}.CubicSpline(levels)
                    End If

                    Dim range As DoubleRange = $"0,{colors.Length}"
                    Dim selector = t.propertyName.SelectNodeValue
                    Dim getValue = Function(node As Node) Val(selector(node))
                    Return Function(nodes)
                               Dim index = nodes.RangeTransform(getValue, range) ' 在这里将属性值映射为等级的index，后面就可以直接引用颜色谱之中的结果了
                               Dim out = index _
                                   .Select(Function(map)
                                               Return New Map(Of Node, Color) With {
                                                   .Key = map.Key,
                                                   .Maps = colors(map.Maps) ' 将等级映射为网络之中的节点或者边的颜色
                                               }
                                           End Function) _
                                   .ToArray
                               Return out
                           End Function
                Else
                    Dim colorValues = t _
                        .values _
                        .Select(Function(c)
                                    Return If(rgbs.ContainsKey(c), rgbs(c), c).TranslateColor
                                End Function) _
                        .ToArray
                    Return Function(nodes)
                               Dim maps = nodes.DiscreteMapping(t.propertyName)
                               Dim out = maps _
                                   .Select(Function(map)
                                               Return New Map(Of Node, Color) With {
                                                   .Key = map.Key,
                                                   .Maps = colorValues(map.Maps)
                                               }
                                           End Function) _
                                   .ToArray
                               Return out
                           End Function
                End If
            Else
                ' 使用单词进行直接映射
                Dim selector = expression.SelectNodeValue
                Return Function(nodes)
                           Return nodes _
                               .Select(Function(n)
                                           Return New Map(Of Node, Color) With {
                                               .Key = n,
                                               .Maps = CStrSafe(selector(n)).TranslateColor
                                           }
                                       End Function) _
                               .ToArray
                       End Function
            End If
        End Function
    End Module
End Namespace
