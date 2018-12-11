﻿
Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.MachineLearning.NeuralNetwork.Activations
Imports Microsoft.VisualBasic.Text.Xml.Models

Namespace NeuralNetwork.StoreProcedure

    Public Class ActiveFunction

        ''' <summary>
        ''' The function name
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute>
        Public Property Name As String
        ''' <summary>
        ''' 函数对象的构造参数列表
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' 因为无法将字典对象进行Xml序列化, 所以在这里使用键值对集合来表示
        ''' </remarks>
        Public Property Arguments As NamedValue()

        Default Public ReadOnly Property Item(name As String) As Double
            Get
                Return Arguments _
                    .FirstOrDefault(Function(tag) tag.name.TextEquals(name)) _
                   ?.text
            End Get
        End Property

        Public ReadOnly Property [Function]() As IActivationFunction
            Get
                With Me
                    Select Case Name
                        Case NameOf(Activations.BipolarSigmoidFunction)
                            Return New BipolarSigmoidFunction(!alpha)
                        Case NameOf(Activations.Sigmoid)
                            Return New Sigmoid
                        Case NameOf(Activations.SigmoidFunction)
                            Return New SigmoidFunction(!alpha)
                        Case NameOf(Activations.ThresholdFunction)
                            Return New ThresholdFunction
                        Case Else
#If DEBUG Then
                            Call "".Warning
#End If
                            Return New Activations.Sigmoid
                    End Select
                End With
            End Get
        End Property

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return $"{Name}({Arguments.Select(Function(a) $"{a.name}:={a.text}").JoinBy(", ")})"
        End Function
    End Class
End Namespace