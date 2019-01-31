﻿#Region "Microsoft.VisualBasic::423aca4487682181fce1ee2f9fa6738a, mime\application%vnd.openxmlformats-officedocument.spreadsheetml.sheet\Excel\Model\Worksheet.vb"

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

    '     Class SheetTable
    ' 
    '         Properties: (+2 Overloads) Column, Row
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.MIME.Office.Excel.XML.xl
Imports Microsoft.VisualBasic.MIME.Office.Excel.XML.xl.worksheets

Namespace Model

    Public Class SheetTable

        Dim strings As sharedStrings
        Dim table As worksheet
        Dim name$

        Public Property Row(i%) As RowObject
            Get
                Throw New NotImplementedException
            End Get
            Set(value As RowObject)
                Throw New NotImplementedException
            End Set
        End Property

        Public Property Column(i%) As String()
            Get
                Throw New NotImplementedException
            End Get
            Set(value As String())
                Throw New NotImplementedException
            End Set
        End Property

        Public Property Column(ID$) As String()
            Get
                Throw New NotImplementedException
            End Get
            Set(value As String())
                Throw New NotImplementedException
            End Set
        End Property

        Default Public Property Cell(point$) As String
            Get
                Throw New NotImplementedException
            End Get
            Set(value As String)
                Throw New NotImplementedException
            End Set
        End Property

        Default Public Property Cell(X%, Y%) As String
            Get
                Throw New NotImplementedException
            End Get
            Set(value As String)
                Throw New NotImplementedException
            End Set
        End Property

        Sub New(xlsx As File, sheetName$)
            strings = xlsx.xl.sharedStrings
            name = sheetName

            If xlsx.xl.Exists(worksheet:=sheetName) Then
                table = xlsx.xl.GetWorksheet(sheetName)
            Else
                table = xlsx.AddSheetTable(sheetName)
            End If
        End Sub

        Public Overrides Function ToString() As String
            Return name
        End Function
    End Class
End Namespace
