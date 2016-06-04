'! 
'@file AbstractVector.cs
'@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
'		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
'@date August 08, 2013
'@brief AbstractVector Interface
'@version 1.0
'
'@section LICENSE
'
'The MIT License (MIT)
'
'Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>
'
'Permission is hereby granted, free of charge, to any person obtaining a copy
'of this software and associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
'copies of the Software, and to permit persons to whom the Software is
'furnished to do so, subject to the following conditions:
'
'The above copyright notice and this permission notice shall be included in
'all copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
'LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
'OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
'THE SOFTWARE.
'
'@section DESCRIPTION
'
'An Interface for the AbstractVector Class.
'
'

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Layouts

    Public MustInherit Class AbstractVector
        Implements IVector

        Public Property x() As Single Implements IVector.x
            Get
                Return m_x
            End Get
            Set
                m_x = Value
            End Set
        End Property
        Private m_x As Single

        Public Property y() As Single Implements IVector.y
            Get
                Return m_y
            End Get
            Set
                m_y = Value
            End Set
        End Property
        Private m_y As Single

        Public Property z() As Single Implements IVector.z
            Get
                Return m_z
            End Get
            Set
                m_z = Value
            End Set
        End Property
        Private m_z As Single

        Public Sub New()
        End Sub

        Public MustOverride Function Add(v2 As AbstractVector) As AbstractVector
        Public MustOverride Function Subtract(v2 As AbstractVector) As AbstractVector
        Public MustOverride Function Multiply(n As Single) As AbstractVector
        Public MustOverride Function Divide(n As Single) As AbstractVector
        Public MustOverride Function Magnitude() As Single
        'public abstract public abstract AbstractVector Normal();
        Public MustOverride Function Normalize() As AbstractVector
        Public MustOverride Function SetZero() As AbstractVector
        Public MustOverride Function SetIdentity() As AbstractVector

        Public Shared Operator +(a As AbstractVector, b As AbstractVector) As AbstractVector
            If TypeOf a Is FDGVector2 AndAlso TypeOf b Is FDGVector2 Then
                Return TryCast(a, FDGVector2) + TryCast(b, FDGVector2)
            ElseIf TypeOf a Is FDGVector3 AndAlso TypeOf b Is FDGVector3 Then
                Return TryCast(a, FDGVector3) + TryCast(b, FDGVector3)
            End If
            Return Nothing
        End Operator
        Public Shared Operator -(a As AbstractVector, b As AbstractVector) As AbstractVector
            If TypeOf a Is FDGVector2 AndAlso TypeOf b Is FDGVector2 Then
                Return TryCast(a, FDGVector2) - TryCast(b, FDGVector2)
            ElseIf TypeOf a Is FDGVector3 AndAlso TypeOf b Is FDGVector3 Then
                Return TryCast(a, FDGVector3) - TryCast(b, FDGVector3)
            End If
            Return Nothing
        End Operator
        Public Shared Operator *(a As AbstractVector, b As Single) As AbstractVector
            If TypeOf a Is FDGVector2 Then
                Return TryCast(a, FDGVector2) * b
            ElseIf TypeOf a Is FDGVector3 Then
                Return TryCast(a, FDGVector3) * b
            End If
            Return Nothing
        End Operator
        Public Shared Operator *(a As Single, b As AbstractVector) As AbstractVector
            If TypeOf b Is FDGVector2 Then
                Return a * TryCast(b, FDGVector2)
            ElseIf TypeOf b Is FDGVector3 Then
                Return a * TryCast(b, FDGVector3)
            End If
            Return Nothing
        End Operator

        Public Shared Operator /(a As AbstractVector, b As Single) As AbstractVector
            If TypeOf a Is FDGVector2 Then
                Return TryCast(a, FDGVector2) / b
            ElseIf TypeOf a Is FDGVector3 Then
                Return TryCast(a, FDGVector3) / b
            End If
            Return Nothing
        End Operator
        Public Overrides Function GetHashCode() As Integer
            Return MyBase.GetHashCode()
        End Function
        Public Overrides Function Equals(obj As System.Object) As Boolean
            Return Me Is TryCast(obj, AbstractVector)
        End Function
        Public Shared Operator =(a As AbstractVector, b As AbstractVector) As Boolean
            ' If both are null, or both are same instance, return true.
            If System.[Object].ReferenceEquals(a, b) Then
                Return True
            End If

            ' If one is null, but not both, return false.
            If (DirectCast(a, Object) Is Nothing) OrElse (DirectCast(b, Object) Is Nothing) Then
                Return False
            End If

            ' Return true if the fields match:
            If TypeOf a Is FDGVector2 AndAlso TypeOf b Is FDGVector2 Then
                Return TryCast(a, FDGVector2) Is TryCast(b, FDGVector2)
            ElseIf TypeOf a Is FDGVector3 AndAlso TypeOf b Is FDGVector3 Then
                Return TryCast(a, FDGVector3) Is TryCast(b, FDGVector3)
            End If

            Return False
        End Operator

        Public Shared Operator <>(a As AbstractVector, b As AbstractVector) As Boolean
            Return Not (a = b)
        End Operator



    End Class


End Namespace