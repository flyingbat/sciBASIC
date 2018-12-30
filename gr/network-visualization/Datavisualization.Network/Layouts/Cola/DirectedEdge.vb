﻿Imports Microsoft.VisualBasic.ComponentModel.Algorithm.BinaryTree
Imports Microsoft.VisualBasic.Imaging.LayoutModel
Imports Microsoft.VisualBasic.Language
Imports number = System.Double

Namespace Layouts.Cola

    ''' <summary>
    ''' An object with three point properties, the intersection with the
    ''' source rectangle (sourceIntersection), the intersection with then
    ''' target rectangle (targetIntersection), And the point an arrow
    ''' head of the specified size would need to start (arrowStart).
    ''' </summary>
    Public Structure DirectedEdge

        Public sourceIntersection As Point2D
        Public targetIntersection As Point2D
        Public arrowStart As Point2D

    End Structure

    Public Class Node

        Public prev As RBNode(Of Node, Object)
        Public [next] As RBNode(Of Node, Object)

        Public r As Rectangle2D
        Public pos As number

        Public Shared Function makeRBTree() As RBNode(Of Node, Object)
            Return New RBNode(Of Node, Object)(Nothing, Nothing)
        End Function

    End Class

    Public Class Variable
        Public offset As number = 0
        Public block As Block
        Public cIn As Constraint()
        Public cOut As Constraint()
        Public weight As number = 1
        Public scale As number = 1
        Public desiredPosition As number

        Public ReadOnly Property dfdv As number
            Get
                Return 2 * weight * (position - desiredPosition)
            End Get
        End Property

        Public ReadOnly Property position As number
            Get
                Return (block.ps.scale * block.posn + offset) / scale
            End Get
        End Property

        Public Sub visitNeighbours(prev As Variable, f As Action(Of Constraint, Variable))
            Dim ff = Sub(c As Constraint, [next] As Variable)
                         If c.active AndAlso Not prev Is [next] Then
                             Call f(c, [next])
                         End If
                     End Sub

            cOut.ForEach(Sub(c, i) ff(c, c.right))
            cIn.ForEach(Sub(c, i) ff(c, c.left))
        End Sub
    End Class

    Public Class Block
        Public ps As PositionStats
        Public posn As number
        Public vars As New List(Of Variable)
        Public blockId As number

        Sub New(v As Variable)
            v.offset = 0
            ps = New PositionStats(v.scale)
            addVariable(v)
        End Sub

        Private Sub addVariable(v As Variable)
            v.block = Me
            vars.Add(v)
            ps.addVariable(v)
            posn = ps.Posn
        End Sub

        ''' <summary>
        ''' move the block where it needs to be to minimize cost
        ''' </summary>
        Sub updateWeightedPosition()
            ps.AB = ps.AD = ps.A2 = 0

            For i As Integer = 0 To vars.Count - 1
                ps.addVariable(vars(i))
            Next

            posn = ps.Posn
        End Sub

        Private Function compute_lm(v As Variable, u As Variable, postAction As Action(Of Constraint)) As number
            Dim dfdv = v.dfdv

            v.visitNeighbours(u, Sub(c, [next])
                                     Dim _dfdv = compute_lm([next], v, postAction)
                                     If ([next] Is c.right) Then
                                         dfdv += _dfdv * c.left.scale
                                         c.lm = _dfdv
                                     Else
                                         dfdv += _dfdv * c.right.scale
                                         c.lm = -_dfdv
                                     End If

                                     postAction(c)
                                 End Sub)
            Return dfdv / v.scale
        End Function

        Private Sub populateSplitBlock(v As Variable, prev As Variable)
            v.visitNeighbours(prev, Sub(c, [next])
                                        [next].offset = v.offset + If([next] Is c.right, c.gap, -c.gap)
                                        addVariable([next])
                                        populateSplitBlock([next], v)
                                    End Sub)
        End Sub

        ''' <summary>
        ''' traverse the active constraint tree applying visit to each active constraint
        ''' </summary>
        ''' <param name="visit"></param>
        ''' <param name="acc"></param>
        ''' <param name="v"></param>
        ''' <param name="prev"></param>
        Public Sub traverse(visit As Func(Of Constraint, Object), acc As List(Of Object), Optional v As Variable = Nothing, Optional prev As Variable = Nothing)
            If v Is Nothing Then
                v = vars(0)
            End If

            v.visitNeighbours(prev, Sub(c, [next])
                                        acc.Add(visit(c))
                                        traverse(visit, acc, [next], v)
                                    End Sub)
        End Sub

        ''' <summary>
        ''' Calculate lagrangian multipliers on constraints And
        ''' find the active constraint in this block with the smallest lagrangian.
        ''' if the lagrangian Is negative, then the constraint Is a split candidate.
        ''' </summary>
        ''' <returns></returns>
        Public Function findMinLM() As Constraint
            Dim m As Constraint = Nothing
            compute_lm(vars(0), Nothing, Sub(c)
                                             If (Not c.equality AndAlso (m Is Nothing OrElse c.lm < m.lm)) Then
                                                 m = c
                                             End If
                                         End Sub)
            Return m
        End Function

        Private Function findMinLMBetween(lv As Variable, rv As Variable) As Constraint
            compute_lm(lv, Nothing, Sub(c)
                                        ' do nothing
                                    End Sub)
            Dim m = Nothing
            findPath(lv, Nothing, rv, Sub(c, [next])
                                          If (Not c.equality AndAlso c.right Is [next] AndAlso (m Is Nothing OrElse c.lm < m.lm)) Then
                                              m = c
                                          End If
                                      End Sub)
            Return m
        End Function

        Private Function findPath(v As Variable, prev As Variable, [to] As Variable, visit As Action(Of Constraint, Variable)) As Boolean
            Dim endFound = False
            v.visitNeighbours(prev, Sub(c, [next])
                                        If (Not endFound AndAlso ([next] Is [to] OrElse findPath([next], v, [to], visit))) Then
                                            endFound = True
                                            visit(c, [next])
                                        End If
                                    End Sub)
            Return endFound
        End Function

        ''' <summary>
        ''' Search active constraint tree from u to see if there Is a directed path to v.
        ''' Returns true if path Is found.
        ''' </summary>
        ''' <param name="u"></param>
        ''' <param name="v"></param>
        ''' <returns></returns>
        Public Function isActiveDirectedPathBetween(u As Variable, v As Variable) As Boolean
            If (u Is v) Then Return True
            Dim i As int = u.cOut.Length
            While (--i)
                Dim C = u.cOut(i)
                If (C.active AndAlso isActiveDirectedPathBetween(C.right, v)) Then
                    Return True
                End If
            End While
            Return False
        End Function
    End Class

    Public Class PositionStats
        Public scale As number

        Public AB As number = 0
        Public AD As number = 0
        Public A2 As number = 0

        Public ReadOnly Property Posn As number
            Get
                Return (AD - AB) / A2
            End Get
        End Property

        Sub New(scale As number)
            Me.scale = scale
        End Sub

        Public Sub addVariable(v As Variable)
            Dim ai = scale / v.scale
            Dim bi = v.offset / v.scale
            Dim wi = v.weight

            AB += wi * ai * bi
            AD += wi * ai * v.desiredPosition
            A2 += wi * ai * ai
        End Sub
    End Class

    Public Class Constraint
        Public lm As number
        Public active As Boolean = False
        Public unsatisfiable As Boolean = False

        Public left As Variable
        Public right As Variable
        Public gap As number
        Public equality As Boolean = False

        Public ReadOnly Property slack As number
            Get
                If unsatisfiable Then
                    Return number.MaxValue
                Else
                    Return right.scale * right.position - gap - left.scale * left.position
                End If
            End Get
        End Property
    End Class
End Namespace