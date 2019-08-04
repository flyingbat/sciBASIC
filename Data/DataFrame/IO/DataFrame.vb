﻿#Region "Microsoft.VisualBasic::951915b38f812f3904aef20fd44dd23c, Data\DataFrame\IO\DataFrame.vb"

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

    '     Class DataFrame
    ' 
    '         Properties: Depth, FieldCount, Headers, HeadTitles, IDataRecord_Item
    '                     IsClosed, Item, RecordsAffected, SchemaOridinal
    ' 
    '         Constructor: (+2 Overloads) Sub New
    ' 
    '         Function: [Select], __createSchemaOridinal, __createTableVector, AddAttribute, ColumnRows
    '                   CreateDataSource, CreateObject, createObjectInternal, csv, EnumerateData
    '                   Generate, GetBoolean, GetByte, GetBytes, GetChar
    '                   GetChars, getColumnList, GetData, GetDataTypeName, GetDateTime
    '                   GetDecimal, GetDouble, GetEnumerator2, GetFieldType, GetFloat
    '                   GetGuid, GetInt16, GetInt32, GetInt64, GetName
    '                   GetOrdinal, GetOrdinalSchema, GetSchemaTable, GetString, GetValue
    '                   GetValueLambda, GetValues, IDataRecord_GetValue, IsDBNull, Load
    '                   LoadDataSet, Parse, Read, reviewColumnHeader, ToString
    ' 
    '         Sub: ChangeMapping, Close, CopyFrom, (+2 Overloads) Dispose, Initialize
    '              Reset
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.ComponentModels
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Linq.Extensions
Imports Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.Serialization.JSON
Imports Microsoft.VisualBasic.Terminal

Namespace IO

    ''' <summary>
    ''' The dynamics data frame object which its first line is not contains the but using for the title property.
    ''' (第一行总是没有的，即本对象类型适用于第一行为列标题行的数据)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DataFrame : Inherits File
        Implements ISchema
        Implements IDataReader
        Implements IDisposable
        Implements IEnumerable(Of DynamicObjectLoader)

        ''' <summary>
        ''' <see cref="current"></see>在<see cref="_innerTable"></see>之中的位置
        ''' </summary>
        ''' <remarks></remarks>
        Dim p% = -1
        Dim current As RowObject

        ''' <summary>
        ''' Using the first line of the csv row as the column headers in this csv file.
        ''' </summary>
        ''' <remarks></remarks>
        Protected columnList As List(Of String)

        Public ReadOnly Property SchemaOridinal As Dictionary(Of String, Integer) Implements ISchema.SchemaOridinal

        Const FieldExists$ = "Required change column name mapping from `{0}` to `{1}`, but the column ``{1}`` is already exists in your file data!"

        ''' <summary>
        ''' ``Csv.Field -> <see cref="PropertyInfo.Name"/>``
        ''' </summary>
        ''' <param name="mapping">``{oldFieldName, newFieldName}``</param>
        ''' <remarks></remarks>
        Public Sub ChangeMapping(mapping As Dictionary(Of String, String))
            Dim p As VBInteger = 0
            Dim oridinal = SchemaOridinal

            For Each map As NamedValue(Of String) In mapping.IterateNameValues
                ' 由于只是改变映射的名称，并不要添加新的列，所以在这里忽略掉不存在的列
                If (p = columnList.IndexOf(map.Name)) = -1 Then
                    Continue For
                End If

                columnList(p) = map.Value

                If oridinal.ContainsKey(map.Value) AndAlso map.Name <> map.Value Then
                    Dim msg = String.Format(FieldExists, map.Name, map.Value)
                    Dim ex As New Exception(msg)

                    ' 2017-11-4 假设在原来的文件之中存在一个名字叫做ID的列
                    ' 但是在这里进行名称映射的变化的结果也是ID名字的话，
                    ' 则在这里会出现重复键名称的错误
                    Throw ex
                End If

                Call oridinal.Remove(map.Name)
                Call oridinal.Add(map.Value, p)
            Next
        End Sub

        Public Function AddAttribute(Name As String) As Integer
            If SchemaOridinal.ContainsKey(Name) Then
                Return SchemaOridinal(Name)
            Else
                Dim p As Integer = columnList.Count
                Call columnList.Add(Name)
                Call _SchemaOridinal.Add(Name, p)
                Return p
            End If
        End Function

        ''' <summary>
        ''' There is an duplicated key exists in your csv table, please delete the duplicated key and try load again!
        ''' </summary>
        Const DuplicatedKeys As String = "There is an duplicated key exists in your csv table, please delete the duplicated key and try load again!"

        ''' <summary>
        ''' Indexing the column headers
        ''' </summary>
        ''' <param name="data"></param>
        ''' <returns></returns>
        Private Shared Function __createSchemaOridinal(data As DataFrame) As Dictionary(Of String, Integer)
            Dim arrayCache$() = data.columnList.ToArray
            Dim duplicates$() = arrayCache _
                .GroupBy(Function(s) s) _
                .Where(Function(g) g.Count > 1) _
                .Select(Function(g) g.Key) _
                .ToArray

            If Not duplicates.IsNullOrEmpty Then
                Dim sb As New StringBuilder(DuplicatedKeys)

                Call sb.AppendLine()
                Call sb.AppendLine("Duplicated headers: " & duplicates.GetJson)
                Call sb.AppendLine()

                Call sb.AppendLine("Here is the column header keys in you data: ")
                Call sb.AppendLine()
                Call sb.AppendLine("  " & arrayCache.GetJson)

                Throw New DuplicateNameException(sb.ToString)
            End If

            Return arrayCache _
                .SeqIterator _
                .ToDictionary(Function(i) i.value,
                              Function(i) i.i)
        End Function

        ''' <summary>
        ''' Get the lines data for the convinent data operation.(为了保持一致的顺序，这个函数是非并行化的)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateDataSource() As DynamicObjectLoader()
            Dim LQuery As DynamicObjectLoader() = LinqAPI.Exec(Of DynamicObjectLoader) <=
 _
                From i As Integer
                In RowNumbers.Sequence.AsParallel
                Let line As RowObject = _innerTable(i)  ' 已经去掉了首行标题行了的
                Select row = New DynamicObjectLoader With {
                    .LineNumber = i,
                    .RowData = line,
                    .Schema = Me.SchemaOridinal,
                    ._innerDataFrame = Me
                }
                Order By row.LineNumber Ascending

            Return LQuery
        End Function

        Public Iterator Function EnumerateData() As IEnumerable(Of Dictionary(Of String, String))
            For Each row In _innerTable
                Dim out As New Dictionary(Of String, String)

                For Each key In SchemaOridinal
                    out.Add(key.Key, row(key.Value))
                Next

                Yield out
            Next
        End Function

        ''' <summary>
        ''' The column headers in the csv file first row.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property HeadTitles As String()
            Get
                Return columnList.ToArray
            End Get
        End Property

        ''' <summary>
        ''' The column headers in the csv file first row.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides ReadOnly Property Headers As RowObject
            Get
                Return New RowObject(columnList)
            End Get
        End Property

        Private ReadOnly Property Depth As Integer Implements IDataReader.Depth
            Get
                Return 0
            End Get
        End Property

        Private ReadOnly Property IsClosed As Boolean Implements IDataReader.IsClosed
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property RecordsAffected As Integer Implements IDataReader.RecordsAffected
            Get
                Return 0
            End Get
        End Property

        Public ReadOnly Property FieldCount As Integer Implements IDataRecord.FieldCount
            Get
                Return columnList.Count
            End Get
        End Property

        Private ReadOnly Property IDataRecord_Item(i As Integer) As Object Implements IDataRecord.Item
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return IDataRecord_GetValue(i)
            End Get
        End Property

        Public Overloads ReadOnly Property Item(name As String) As Object Implements IDataRecord.Item
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return IDataRecord_GetValue(GetOrdinal(name))
            End Get
        End Property

        ''' <summary>
        ''' Convert this dataframe object as a csv document object
        ''' </summary>
        ''' <returns></returns>
        Public Function csv() As File
            Dim file As New File
            file += New RowObject(columnList)
            file += DirectCast(_innerTable, IEnumerable(Of RowObject))
            Return file
        End Function

        Protected Friend Sub New()
        End Sub

        ''' <summary>
        ''' Create a new dataframe with column value assigned
        ''' </summary>
        ''' <param name="columns">
        ''' 只支持基础类型,不支持复杂类型,因为csv文件的单元格不适用于复杂数据类型的数据文本的存储
        ''' </param>
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(ParamArray columns As ArgumentReference())
            Call Initialize(ColumnRows(columns).AsList, Me)
        End Sub

        Private Shared Iterator Function ColumnRows(columns As ArgumentReference()) As IEnumerable(Of RowObject)
            Dim collectionType As Type() = {GetType(Array), GetType(IEnumerable), GetType(IList)}
            Dim matrix As Object()() = columns _
                .Select(Function(c)
                            Dim type As Type = c.ValueType

                            If collectionType.Any(Function(base)
                                                      Return type.IsInheritsFrom(base)
                                                  End Function) Then
                                ' 是一个值的集合
                                Return DirectCast(c.Value, IEnumerable).ToVector
                            Else
                                ' 是一个单个的值,转换为值的集合
                                Return New Object() {c.Value}
                            End If
                        End Function) _
                .ToArray
            Dim maxLen = Aggregate c In matrix Into Max(c.Length)
            Dim row As IEnumerable(Of String)

            ' yield title row
            Yield New RowObject(columns.Select(Function(c) c.Name))

            For i As Integer = 0 To maxLen - 1
#Disable Warning
                row = matrix _
                    .Select(Function(v)
                                Return v.ElementAtOrNull(i)
                            End Function) _
                    .Select(AddressOf Scripting.ToString)
#Enable Warning
                Yield New RowObject(row)
            Next
        End Function

        ''' <summary>
        ''' Try loading a excel csv data file as a dynamics data frame object.(尝试加载一个Csv文件为数据框对象，请注意，第一行必须要为标题行)
        ''' </summary>
        ''' <param name="path"></param>
        ''' <param name="encoding"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Shared Function Load(path As String,
                                              Optional encoding As Encoding = Nothing,
                                              Optional fast As Boolean = False,
                                              Optional skipWhile As NamedValue(Of Func(Of String, Boolean)) = Nothing) As DataFrame
            Dim file As File

            If fast Then
                file = FileLoader.FastLoad(path, True, encoding, skipWhile)
            Else
                file = File.Load(path, encoding, , skipWhile)
            End If

            Return CreateObject(file)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function LoadDataSet(path$, Optional encoding As Encoding = Nothing) As IEnumerable(Of DataSet)
            Return DataSet.LoadDataSet(path, encoding:=encoding)
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overloads Shared Function Parse(Of T As Class)(content As String) As IEnumerable(Of T)
            Return File.Parse(content).AsDataSource(Of T)
        End Function

        Private Shared Function getColumnList(table As IEnumerable(Of RowObject)) As List(Of String)
            Return LinqAPI.MakeList(Of String) _
 _
                () <= From strValue As String
                      In table.First
                      Let s = reviewColumnHeader(strValue)
                      Select s

        End Function

        ''' <summary>
        ''' ``[CSV::Reflector::Warnning] There are empty column header in your data!``
        ''' </summary>
        Const EmptyWarning$ = "[CSV::Reflector::Warnning] There are empty column header in your data!"

        ''' <summary>
        ''' 这里不能够使用Trim函数，因为Column也可能是故意定义了空格在其实或者结束的位置的，
        ''' 使用Trim函数之后，反而会导致GetOrder函数执行失败。故而在这里只给出警告信息即可
        ''' </summary>
        ''' <param name="strValue"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function reviewColumnHeader(strValue As String) As String
            If String.IsNullOrEmpty(strValue) Then
                Call EmptyWarning.Warning
                Return ""
            End If

            Dim ch As Char = strValue.First

            If ch = " "c OrElse ch = vbTab Then
                Call xConsole.WriteLine($"^y{String.Format(FailureWarning, strValue)}^!")
            End If
            ch = strValue.Last
            If ch = " "c OrElse ch = vbTab Then
                Call xConsole.WriteLine($"^y{String.Format(FailureWarning, strValue)}^!")
            End If

            ' 这里不能够使用Trim函数，因为Column也可能是故意定义了空格在其实或者结束的位置的，
            ' 使用Trim函数之后，反而可能会导致GetOrder函数执行失败。故而在这里只给出警告信息即可
            Return strValue
        End Function

        Const FailureWarning As String =
            "[CSV::Reflector::Warning] The Column header ""{0}"" end with the space character value, this may caused the ``GetOrder()`` function execute failure!"

        ''' <summary>
        ''' Creates the data frame object from the csv docs.
        ''' </summary>
        ''' <param name="file"></param>
        ''' <returns></returns>
        Public Overloads Shared Function CreateObject(file As File) As DataFrame
            Return createObjectInternal(file)
        End Function

        Private Shared Sub Initialize(table As List(Of RowObject), dataframe As DataFrame)
            dataframe._innerTable = table.Skip(1).AsList
            dataframe.columnList = getColumnList(table)
            dataframe._SchemaOridinal = __createSchemaOridinal(dataframe)
        End Sub

        Private Shared Function createObjectInternal(file As File) As DataFrame
            Dim dataframe As New DataFrame
            Call Initialize(file._innerTable, dataframe)
            Return dataframe
        End Function

        Protected Friend Overrides Function __createTableVector() As RowObject()
            Dim readBuffer As New List(Of RowObject)({CType(Me.columnList, RowObject)})
            Call readBuffer.AddRange(_innerTable)
            Return readBuffer.ToArray
        End Function

        Public Overrides Function Generate() As String
            Dim sb As New StringBuilder(1024)
            Dim head As String = New RowObject(columnList).AsLine

            Call sb.AppendLine(head)

            For Each row As RowObject In _innerTable
                Call sb.AppendLine(row.AsLine)
            Next

            Return sb.ToString
        End Function

        ''' <summary>
        ''' Function return -1 when column not found. 
        ''' </summary>
        ''' <param name="Column"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetOrdinal(Column As String) As Integer Implements IDataRecord.GetOrdinal, ISchema.GetOrdinal
            Return columnList.IndexOf(Column)
        End Function

        ''' <summary>
        ''' Gets the order list of the specific column list, -1 value will be returned when it is not exists in the table.
        ''' (获取列集合的位置列表，不存在的列则返回-1)
        ''' </summary>
        ''' <param name="columns"></param>
        ''' <returns></returns>
        ''' <remarks>由于存在一一对应关系，这里不会再使用并行拓展</remarks>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetOrdinalSchema(columns As String()) As Integer()
            Return columns.Select(Function(c) columnList.IndexOf(c)).ToArray
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function GetValue(ordinal As Integer) As String
#If DEBUG Then
            If ordinal > Me.current.Count - 1 Then
                Return ""
            End If
#End If
            Return current.Column(ordinal)
        End Function

        Public Function GetValueLambda(columnName$, Optional caseSensitive As Boolean = True) As Func(Of RowObject, String)
            Dim index As Integer = GetOrdinal(Column:=columnName)

            If index = -1 AndAlso caseSensitive Then
                Return Function(r) Nothing
            ElseIf index = -1 Then
                With Which.IsTrue(columnList.Select(Function(c) c.TextEquals(columnName)))
                    If .IsNullOrEmpty Then
                        Return Function(r) Nothing
                    Else
                        index = .ByRef(Scan0)
                    End If
                End With
            End If

            Return Function(r) r(index)
        End Function

        ''' <summary>
        ''' The data frame object start to reading the data in this table, if the current pointer is reach 
        ''' the top of the lines then this function will returns FALSE to stop the reading loop.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Read() As Boolean Implements IDataReader.Read, IDataReader.NextResult
            If p = _innerTable.Count - 1 Then
                Return False
            Else
                p += 1
                current = _innerTable(p)

                Return True
            End If
        End Function

        ''' <summary>
        ''' Reset the reading position in the data frame object.
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Reset()
            p = -1
        End Sub

        ''' <summary>
        ''' 这个方法会清除当前对象之中的原有数据
        ''' </summary>
        ''' <param name="source"></param>
        ''' <remarks></remarks>
        Public Sub CopyFrom(source As File)
            _innerTable = source._innerTable.Skip(1).AsList
            columnList = source._innerTable.First.AsList
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return _innerTable(p).ToString
        End Function

        ''' <summary>
        ''' Subset of the dataframe object by given column field names
        ''' </summary>
        ''' <param name="columnList"></param>
        ''' <returns></returns>
        Public Function [Select](columnList As String()) As DataFrame
            Dim newTable As New List(Of RowObject)
            ' Location pointer to the column
            Dim pList As Integer() = GetOrdinalSchema(columnList)

            Call Me.Reset()

            Do While Me.Read
                newTable += New RowObject(
                    pList.Select(
                    Function(i) current.Column(i)))
            Loop

            Return New DataFrame With {
                .columnList = columnList.AsList,
                ._innerTable = newTable
            }
        End Function

        Public Iterator Function GetEnumerator2() As IEnumerator(Of DynamicObjectLoader) Implements IEnumerable(Of DynamicObjectLoader).GetEnumerator
            Dim schema As Dictionary(Of String, Integer) =
                columnList _
                .SeqIterator _
                .ToDictionary(Function(x) x.value,
                              Function(x) x.i)

            For Each l As DynamicObjectLoader In From i As SeqValue(Of RowObject)
                                                 In Me._innerTable.SeqIterator
                                                 Let line As RowObject = i.value
                                                 Let loader = New DynamicObjectLoader With {
                                                     .LineNumber = i.i,
                                                     .RowData = line,
                                                     .Schema = schema
                                                 }
                                                 Select loader
                Yield l
            Next
        End Function

        ''' <summary>
        ''' Closes the <see cref="System.Data.IDataReader"/>:<see cref="DataFrame"/> Object.  
        ''' </summary>
        Private Sub Close() Implements IDataReader.Close
            ' Do Nothing
        End Sub

        ''' <summary>
        ''' Returns a System.Data.DataTable that describes the column metadata of the System.Data.IDataReader.
        ''' </summary>
        ''' <returns>A System.Data.DataTable that describes the column metadata.</returns>
        Private Function GetSchemaTable() As DataTable Implements IDataReader.GetSchemaTable
            Throw New NotImplementedException()
        End Function

        Public Function GetName(i As Integer) As String Implements IDataRecord.GetName
            Return columnList(i)
        End Function

        Public Function GetDataTypeName(i As Integer) As String Implements IDataRecord.GetDataTypeName
            Dim value As String = GetValue(i)

            If value.IsNumeric Then
                Return "System.Double"
            ElseIf InStr(value, ", ") > 0 OrElse InStr(value, "; ") > 0 Then
                Return "System.String()"
            Else
                Return "System.String"
            End If
        End Function

        Public Function GetFieldType(i As Integer) As Type Implements IDataRecord.GetFieldType
            Dim typeName As String = GetDataTypeName(i)
            Return InputHandler.GetType(typeName, True)
        End Function

        Private Function IDataRecord_GetValue(i As Integer) As Object Implements IDataRecord.GetValue
            Return current.Column(i)
        End Function

        Public Function GetValues(values() As Object) As Integer Implements IDataRecord.GetValues
            If values.IsNullOrEmpty Then
                Return 0
            Else
                For i As Integer = 0 To values.Length - 1
                    values(i) = current.Column(i)
                Next

                Return values.Length
            End If
        End Function

        Public Function GetBoolean(i As Integer) As Boolean Implements IDataRecord.GetBoolean
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Boolean)(value)
        End Function

        Public Function GetByte(i As Integer) As Byte Implements IDataRecord.GetByte
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Byte)(value)
        End Function

        Public Function GetBytes(i As Integer, fieldOffset As Long, buffer() As Byte, bufferoffset As Integer, length As Integer) As Long Implements IDataRecord.GetBytes
            Throw New NotImplementedException()
        End Function

        Public Function GetChar(i As Integer) As Char Implements IDataRecord.GetChar
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Char)(value)
        End Function

        Public Function GetChars(i As Integer, fieldoffset As Long, buffer() As Char, bufferoffset As Integer, length As Integer) As Long Implements IDataRecord.GetChars
            Throw New NotImplementedException()
        End Function

        Public Function GetGuid(i As Integer) As Guid Implements IDataRecord.GetGuid
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Guid)(value)
        End Function

        Public Function GetInt16(i As Integer) As Short Implements IDataRecord.GetInt16
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Short)(value)
        End Function

        Public Function GetInt32(i As Integer) As Integer Implements IDataRecord.GetInt32
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Integer)(value)
        End Function

        Public Function GetInt64(i As Integer) As Long Implements IDataRecord.GetInt64
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Long)(value)
        End Function

        Public Function GetFloat(i As Integer) As Single Implements IDataRecord.GetFloat
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Single)(value)
        End Function

        Public Function GetDouble(i As Integer) As Double Implements IDataRecord.GetDouble
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Double)(value)
        End Function

        Public Function GetString(i As Integer) As String Implements IDataRecord.GetString
            Dim value As String = current.Column(i)
            Return value
        End Function

        Public Function GetDecimal(i As Integer) As Decimal Implements IDataRecord.GetDecimal
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Decimal)(value)
        End Function

        Public Function GetDateTime(i As Integer) As Date Implements IDataRecord.GetDateTime
            Dim value As String = current.Column(i)
            Return Scripting.CTypeDynamic(Of Date)(value)
        End Function

        Public Function GetData(i As Integer) As IDataReader Implements IDataRecord.GetData
            Return Me
        End Function

        Public Function IsDBNull(i As Integer) As Boolean Implements IDataRecord.IsDBNull
            Return String.IsNullOrEmpty(current.Column(i))
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace
