﻿#Region "Microsoft.VisualBasic::8aff8945bb31377009959a13bb3de6d6, Data_science\DataMining\Microsoft.VisualBasic.DataMining.Framework\AprioriRules\Algorithm\Entities\Rule.vb"

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

    '     Class Rule
    ' 
    '         Properties: SupportX, SupportXY
    ' 
    '         Function: CompareTo, Equals, GetHashCode, ToString
    ' 
    '         Sub: New
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection
Imports Microsoft.VisualBasic.DataMining.AprioriRules.Impl

Namespace AprioriRules.Entities

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Rule : Implements IComparable(Of Rule)

#Region "Member Variables"

        Dim combination As String
        Dim remaining As String

#End Region

#Region "Public Properties"

        <Column("rule.X")> Public ReadOnly Property X As String
            Get
                Return combination
            End Get
        End Property

        <Column("rule.Y")> Public ReadOnly Property Y As String
            Get
                Return remaining
            End Get
        End Property

        <Column("support(XY)")>
        Public ReadOnly Property SupportXY As Double
        <Column("support(X)")>
        Public ReadOnly Property SupportX As Double

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <Column("confidence")> Public ReadOnly Property Confidence As Double

        Public Sub New(combination$, remaining$, confidence#, supports As (XY#, X#))
            Me.combination = combination
            Me.remaining = remaining
            Me.Confidence = confidence
            Me.SupportX = supports.X
            Me.SupportXY = supports.XY
        End Sub
#End Region

        Public Overrides Function ToString() As String
            Return $"({SupportXY}/{SupportX} = {Math.Round(Confidence, 4)}) {{ {X} }} -> {{ {Y} }}"
        End Function

#Region "IComparable<clssRules> Members"

        Public Function CompareTo(other As Rule) As Integer Implements IComparable(Of Rule).CompareTo
            Return X.CompareTo(other.X)
        End Function
#End Region

        Public Overrides Function GetHashCode() As Integer
            Dim sortedXY$ = Apriori.SorterSortTokens(X & Y)
            Return sortedXY.GetHashCode()
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            Dim other = TryCast(obj, Rule)

            If other Is Nothing Then
                Return False
            End If

            Return other.X = X AndAlso other.Y = Y OrElse other.X = Y AndAlso other.Y = X
        End Function
    End Class
End Namespace