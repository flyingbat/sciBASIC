﻿#Region "Microsoft.VisualBasic::558c3dd2473fdb9482d5aa392b6cd792, Data_science\Mathematica\Math\DataFittings\Linear\IFitted.vb"

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

    ' Interface IFitted
    ' 
    '     Properties: ErrorTest, GetY, Polynomial, R2
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Interface IFitted

    ''' <summary>
    ''' 相关系数 R2
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property R2 As Double
    ''' <summary>
    ''' 线性模型的多项式
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property Polynomial As Polynomial

    ''' <summary>
    ''' f(x)
    ''' </summary>
    ''' <param name="x"></param>
    ''' <returns></returns>
    Default ReadOnly Property GetY(x As Double) As Double

    ''' <summary>
    ''' 保存拟合后的y值，在拟合时可设置为不保存节省内存
    ''' </summary>
    Property ErrorTest As TestPoint()

End Interface
