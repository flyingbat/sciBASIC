#Region "GNU Lesser General Public License"
'
'This file is part of DotFuzzy.
'
'DotFuzzy is free software: you can redistribute it and/or modify
'it under the terms of the GNU Lesser General Public License as published by
'the Free Software Foundation, either version 3 of the License, or
'(at your option) any later version.
'
'DotFuzzy is distributed in the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty of
'MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU Lesser General Public License for more details.
'
'You should have received a copy of the GNU Lesser General Public License
'along with DotFuzzy.  If not, see <http://www.gnu.org/licenses/>.
'

#End Region

Imports System.Collections.Generic
Imports System.Text

Namespace FuzzyLogic

    ''' <summary>
    ''' Represents a linguistic variable.
    ''' </summary>
    Public Class LinguisticVariable
#Region "Private Properties"

        Private m_name As String = [String].Empty
        Private m_membershipFunctionCollection As New MembershipFunctionCollection()
        Private m_inputValue As Double = 0

#End Region

#Region "Constructors"

        ''' <summary>
        ''' Default constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <param name="name">The name that identificates the linguistic variable.</param>
        Public Sub New(name As String)
            Me.Name = name
        End Sub

        ''' <param name="name">The name that identificates the linguistic variable.</param>
        ''' <param name="membershipFunctionCollection">A membership functions collection for the lingusitic variable.</param>
        Public Sub New(name As String, membershipFunctionCollection As MembershipFunctionCollection)
            Me.Name = name
            Me.MembershipFunctionCollection = membershipFunctionCollection
        End Sub

#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The name that identificates the linguistic variable.
        ''' </summary>
        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set
                m_name = value
            End Set
        End Property

        ''' <summary>
        ''' A membership functions collection for the lingusitic variable.
        ''' </summary>
        Public Property MembershipFunctionCollection() As MembershipFunctionCollection
            Get
                Return m_membershipFunctionCollection
            End Get
            Set
                m_membershipFunctionCollection = value
            End Set
        End Property

        ''' <summary>
        ''' The input value for the linguistic variable.
        ''' </summary>
        Public Property InputValue() As Double
            Get
                Return m_inputValue
            End Get
            Set
                m_inputValue = value
            End Set
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Implements the fuzzification of the linguistic variable.
        ''' </summary>
        ''' <param name="membershipFunctionName">The membership function for which fuzzify the variable.</param>
        ''' <returns>The degree of membership.</returns>
        Public Function Fuzzify(membershipFunctionName As String) As Double
            Dim membershipFunction As MembershipFunction = Me.m_membershipFunctionCollection.Find(membershipFunctionName)

            If (membershipFunction.X0 <= Me.InputValue) AndAlso (Me.InputValue < membershipFunction.X1) Then
                Return (Me.InputValue - membershipFunction.X0) / (membershipFunction.X1 - membershipFunction.X0)
            ElseIf (membershipFunction.X1 <= Me.InputValue) AndAlso (Me.InputValue <= membershipFunction.X2) Then
                Return 1
            ElseIf (membershipFunction.X2 < Me.InputValue) AndAlso (Me.InputValue <= membershipFunction.X3) Then
                Return (membershipFunction.X3 - Me.InputValue) / (membershipFunction.X3 - membershipFunction.X2)
            Else
                Return 0
            End If
        End Function

        ''' <summary>
        ''' Returns the minimum value of the linguistic variable.
        ''' </summary>
        ''' <returns>The minimum value of the linguistic variable.</returns>
        Public Function MinValue() As Double
            Dim minValue__1 As Double = Me.m_membershipFunctionCollection(0).X0

            For i As Integer = 1 To Me.m_membershipFunctionCollection.Count - 1
                If Me.m_membershipFunctionCollection(i).X0 < minValue__1 Then
                    minValue__1 = Me.m_membershipFunctionCollection(i).X0
                End If
            Next

            Return minValue__1
        End Function

        ''' <summary>
        ''' Returns the maximum value of the linguistic variable.
        ''' </summary>
        ''' <returns>The maximum value of the linguistic variable.</returns>
        Public Function MaxValue() As Double
            Dim maxValue__1 As Double = Me.m_membershipFunctionCollection(0).X3

            For i As Integer = 1 To Me.m_membershipFunctionCollection.Count - 1
                If Me.m_membershipFunctionCollection(i).X3 > maxValue__1 Then
                    maxValue__1 = Me.m_membershipFunctionCollection(i).X3
                End If
            Next

            Return maxValue__1
        End Function

        ''' <summary>
        ''' Returns the difference between MaxValue() and MinValue().
        ''' </summary>
        ''' <returns>The difference between MaxValue() and MinValue().</returns>
        Public Function Range() As Double
            Return Me.MaxValue() - Me.MinValue()
        End Function

#End Region
    End Class
End Namespace