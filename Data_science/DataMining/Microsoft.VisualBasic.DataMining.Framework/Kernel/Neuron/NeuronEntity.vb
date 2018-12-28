﻿#Region "Microsoft.VisualBasic::d772977bfb23edfdd2bd6c5400627985, Data_science\DataMining\Microsoft.VisualBasic.DataMining.Framework\Kernel\Neuron\NeuronEntity.vb"

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

    '     Class NeuronEntity
    ' 
    '         Properties: Y
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.DataMining.ComponentModel

Namespace Kernel.Classifier

    Public Class NeuronEntity : Inherits EntityBase(Of Double)

        <XmlAttribute> Public Property Y As Double

        Public Overrides Function ToString() As String
            Dim sBuilder As New StringBuilder(1024)
            For Each p As Integer In Properties
                Call sBuilder.AppendFormat("{0}, ", p)
            Next
            Call sBuilder.Remove(sBuilder.Length - 1, length:=1)

            Return String.Format("<{0}> --> {1}", sBuilder.ToString, Y)
        End Function

        Public Shared Widening Operator CType(properties As Double()) As NeuronEntity
            Return New NeuronEntity With {
                .Properties = properties
            }
        End Operator

        Public Shared Widening Operator CType(properties As Integer()) As NeuronEntity
            Return New NeuronEntity With {
                .Properties = (From n In properties Select CType(n, Double)).ToArray
            }
        End Operator
    End Class
End Namespace