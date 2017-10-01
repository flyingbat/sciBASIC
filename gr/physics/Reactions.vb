﻿#Region "Microsoft.VisualBasic::cbed10f9625ec05514e93c4e96fd3fc5, ..\sciBASIC#\gr\physics\Reactions.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
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

#End Region

Imports Microsoft.VisualBasic.Language

''' <summary>
''' How does two <see cref="MassPoint"/> object reacting with each other.
''' </summary>
Public Module Reactions

    ''' <summary>
    ''' 两个点电荷之间的库仑力
    ''' </summary>
    ''' <param name="m1"></param>
    ''' <param name="m2"></param>
    ''' <param name="k#"></param>
    ''' <returns></returns>
    Public Function CoulombsLaw(m1 As MassPoint, m2 As MassPoint, Optional k# = 9000000000.0) As Force
        Dim d = m1.Point - m2.Point
        Dim f = Math.CoulombsLaw(m1.Charge, m2.Charge, d.SumMagnitude, k)

        With RepulsiveForce(f, m1.Point, m2.Point)
            .source = NameOf(CoulombsLaw)
            Return .ref
        End With
    End Function
End Module
