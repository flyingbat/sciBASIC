﻿#Region "Microsoft.VisualBasic::11a32cbf1fad335af1b0e7870f15cd60, Data_science\Mathematica\Math\Math\Scripting\Expression\MathToken.vb"

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

    '     Class MathToken
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '     Enum MathTokens
    ' 
    '         [Operator], Close, Comma, Invalid, Literal
    '         Open, Symbol, Terminator
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Scripting.TokenIcer

Namespace Scripting.MathExpression

    Public Class MathToken : Inherits CodeToken(Of MathTokens)

        Sub New(name As MathTokens, text As String)
            Call MyBase.New(name, text)
        End Sub
    End Class

    Public Enum MathTokens
        Invalid
        Literal
        [Operator]
        Open
        Close
        Symbol
        Comma
        Terminator
    End Enum
End Namespace
