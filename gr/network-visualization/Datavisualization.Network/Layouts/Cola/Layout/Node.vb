﻿#Region "Microsoft.VisualBasic::064f88a98661b0cfface4cdf684fb6d3, gr\network-visualization\Datavisualization.Network\Layouts\Cola\Layout\Node.vb"

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

'     Class InputNode
' 
' 
' 
'     Class Node
' 
'         Function: makeRBTree
' 
'     Class Group
' 
'         Function: isGroup
' 
'     Class Link
' 
'         Properties: length, source, target, weight
' 
'     Delegate Function
' 
' 
'     Class LinkLengthTypeAccessor
' 
'         Function: [getType]
' 
' 
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Algorithm.BinaryTree
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.Language.JavaScript

Namespace Layouts.Cola

    Public Class InputNode : Inherits JavaScriptObject

        ''' <summary>
        ''' index in nodes array, this is initialized by Layout.start()
        ''' </summary>
        Public index As Integer

        ''' <summary>
        ''' x and y will be computed by layout as the Node's centroid
        ''' </summary>
        Public x As Double

        ''' <summary>
        ''' x and y will be computed by layout as the Node's centroid
        ''' </summary>
        Public y As Double

        ''' <summary>
        ''' specify a width and height of the node's bounding box if you turn on avoidOverlaps
        ''' </summary>
        Public width As Double

        ''' <summary>
        ''' specify a width and height of the node's bounding box if you turn on avoidOverlaps
        ''' </summary>
        Public height As Double

        ''' <summary>
        ''' if fixed, layout will not move the node from its specified starting position
        ''' selective bit mask.  !=0 means layout will not move.
        ''' </summary>
        Public fixed As Double
    End Class

    ''' <summary>
    ''' Client-passed node may be missing these properties, which will be set
    ''' upon ingestion
    ''' </summary>
    Public Class Node : Inherits InputNode
        Implements Indexed

        Public Property id As Integer Implements Indexed.id
        Public name As String
        Public routerNode As Node
        Public prev As RBTree(Of Integer, Node)
        Public [next] As RBTree(Of Integer, Node)
        Public children As Integer()

        'Default Public Property GetNode(direction As String) As RBTree(Of Integer, Node)
        '    Get
        '        Select Case direction
        '            Case NameOf(prev)
        '                Return prev
        '            Case NameOf([next])
        '                Return [next]
        '            Case Else
        '                Throw New NotImplementedException(direction)
        '        End Select
        '    End Get
        '    Set
        '        Select Case direction
        '            Case NameOf(prev)
        '                prev = Value
        '            Case NameOf([next])
        '                [next] = Value
        '            Case Else
        '                Throw New NotImplementedException(direction)
        '        End Select
        '    End Set
        'End Property

        Public r As Rectangle2D
        Public v As Variable
        Public bounds As Rectangle2D
        Public pos As Double
        Public _dragGroupOffsetY As Double
        Public _dragGroupOffsetX As Double
        Public Property px As Double?
        Public Property py As Double?
        Public parent As Group

        Sub New()
        End Sub

        Sub New(v As Variable, r As Rectangle2D, pos As Double)
            Me.v = v
            Me.r = r
            Me.pos = pos

            prev = makeRBTree()
            [next] = makeRBTree()
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function makeRBTree() As RBTree(Of Integer, Node)
            Return New RBTree(Of Integer, Node)(Function(x, y) x - y, Function(i) i.ToString)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(node As Node) As Point2D
            Return New Point2D(node.x, node.y)
        End Operator
    End Class
End Namespace
