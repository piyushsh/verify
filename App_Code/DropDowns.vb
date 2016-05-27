Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports BPADotNetCommonFunctions
Imports BPADotNetCommonFunctions.SupplierModuleFunctions
Imports BPADotNetCommonFunctions.VT_CommonFunctions.MatrixFunctions
Imports AjaxControlToolkit
Imports VTDBFunctions.VTDBFunctions



<WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Script.Services.ScriptService()> _
Public Class ModuleServices
    Inherits MyWebServicePage

    '<WebMethod()> _
    'Public Function GetSuppliers() As Data.DataTable

    '    Dim objSupp As New SupplierModuleFunctions

    '    GetSuppliers = objSupp.GetSuppliers
    'End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        Dim dtCats As Data.DataTable = objMatrix.GetCategories(Session("_VT_DotNetConnString"), Me.CurrentSession.FormListTable, Me.CurrentSession.ModuleRootNode, Me.CurrentSession.ModuleCategoriesNode, Me.CurrentSession.ModuleRootNodeID)
        Dim values As New  _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetItemsInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategoryId As String = knownCategoryValuesDictionary("Category").ToString
        Dim dtSupps As Data.DataTable = objMatrix.GetItemsInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId, Me.CurrentSession.FormListTable)
        Dim values As New  _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetSupplierCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objSupp As New SupplierModuleFunctions
        Dim objPops As New POPsFunctions


        Dim dsCats As Data.DataSet = objPops.GetSupplierCategoriesDataset()

        Dim dtCats As Data.DataTable = dsCats.Tables(0)

        ' Dim dtCats As Data.DataTable = objSupp.GetSupplierCategories(Session("_VT_DotNetConnString"))
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("CategoryName"), dr.Item("SupplierCategoryId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
    Public Function GetSuppliersInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objSupp As New SupplierModuleFunctions
        Dim objPops As New POPsFunctions
        Dim intSupplierID As Integer

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategory As String = knownCategoryValuesDictionary("Category").ToString

        intSupplierID = CInt(strSelectedCategory)

        Dim dsSupps As Data.DataSet = objPops.GetSuppliersInCategory(intSupplierID)
        Dim dtSupps As Data.DataTable = dsSupps.Tables(0)

        '    Dim dtSupps As Data.DataTable = objSupp.GetSuppliersInCategory(Session("_VT_DotNetConnString"), strSelectedCategory)
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("SupplierName"), dr.Item("SupplierId")))
        Next

        Return values.ToArray

    End Function



    <WebMethod(EnableSession:=True)> _
Public Function GetSubContractCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objSupp As New SubContractModuleFunctions


        Dim dtCats As Data.DataTable = objSupp.GetSubContractCategories(Session("_VT_DotNetConnString"))
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
    Public Function GetSubContractsInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objSupp As New SubContractModuleFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategory As String = knownCategoryValuesDictionary("Category").ToString
        Dim dtSupps As Data.DataTable = objSupp.GetSubContractsInCategory(Session("_VT_DotNetConnString"), strSelectedCategory)
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function

    <WebMethod(EnableSession:=True)> _
 Public Function GetProductCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        '  Dim objProd As New MaterialsModuleFunctions


        Dim ds As New Data.DataSet
        Dim objBPA As New VTDBFunctions.VTDBFunctions.ProductsFunctions
       
        ds = objBPA.GetProductLines

        Dim dtCats As Data.DataTable = ds.Tables(0) '  = objProd.GetMaterialsCategories(Session("_VT_DotNetConnString"))
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("Product_Line_Text"), dr.Item("Product_LineID")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
    Public Function GetProductsInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objProd As New MaterialsModuleFunctions
        Dim astrItems(2) As String

        Dim ds As New Data.DataSet
        Dim objBPA As New VTDBFunctions.VTDBFunctions.ProductsFunctions
      
        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategoryId As String = knownCategoryValuesDictionary("Category").ToString
        ''Dim dtSupps As Data.DataTable = objProd.GetMaterialsInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId)
        'astrItems(0) = "ItemId"
        'astrItems(1) = "txtProdName"
        'astrItems(2) = "txtProdCode"

        ds = objBPA.GetProductsForLine(strSelectedCategoryId)

        Dim dtSupps As Data.DataTable = ds.Tables(0) ' objProd.GetDataItemsForProductsInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId, astrItems)

        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)


        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("Product_Name"), dr.Item("ProductID")))
            '  values.Add(New CascadingDropDownNameValue(dr.Item("Field1") + " - " + dr.Item("Field2"), dr.Item("Field0")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
 Public Function GetPersonnelCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String

        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions
        Dim dtCats As Data.DataTable = objMatrix.GetCategories(Session("_VT_DotNetConnString"), PersonnelFormListTable, PersonnelModuleRootNode, PersonnelModuleCategoriesNode, Session("_VT_PersonnelRootId"))
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetPersonnelInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objPer As New PersonnelModuleFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategoryId As String = knownCategoryValuesDictionary("Category").ToString
        Dim dtSupps As Data.DataTable = objPer.GetPersonnelInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId)
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function

    <WebMethod(EnableSession:=True)> _
    Public Function GetCustomerSchemaCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions


        Dim dsCats As Data.DataSet = objCust.getAllCustomerAreas()
        Dim values As New  _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dsCats.Tables(0).Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("AreaDescription"), dr.Item("CustomerAreaId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
    Public Function GetCustomersInSchemaCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objCust As New VTDBFunctions.VTDBFunctions.CustomerFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategory As String = knownCategoryValuesDictionary("Category").ToString
        Dim dsCusts As Data.DataSet = objCust.getCustomersForArea(strSelectedCategory)
        Dim values As New  _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dsCusts.Tables(0).Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("CustomerName"), dr.Item("CustomerId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetCustomerCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objCust As New CustomerModuleFunctions


        Dim dtCats As Data.DataTable = objCust.GetCustomerCategories(Session("_VT_DotNetConnString"))
        Dim values As New  _
           System.Collections.Generic.List(Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetCustomersInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'ByVal knownCategoryValues As String, ByVal category As String
        Dim objCust As New CustomerModuleFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategory As String = knownCategoryValuesDictionary("Category").ToString
        Dim dtSupps As Data.DataTable = objCust.GetCustomersInCategory(Session("_VT_DotNetConnString"), strSelectedCategory)
        Dim values As New  _
           System.Collections.Generic.List(Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function
    <WebMethod(EnableSession:=True)> _
Public Function GetModuleCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        'Dim dtCats As Data.DataTable = objMatrix.GetCategories(Session("_VT_DotNetConnString"), ModulesFormListTable, ModulesModuleRootNode, ModulesModuleCategoriesNode, Session("_VT_ModulesRootId"))
        'Dim values As New  _
        '   System.Collections.Generic.List( _
        '   Of AjaxControlToolkit.CascadingDropDownNameValue)

        'For Each dr As Data.DataRow In dtCats.Rows
        '    values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        'Next

        'Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetModulesInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        'Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        'Dim knownCategoryValuesDictionary As New StringDictionary
        'knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        'Dim strSelectedCategoryId As String = knownCategoryValuesDictionary("Category").ToString
        'Dim dtSupps As Data.DataTable = objMatrix.GetItemsInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId, ModulesFormListTable)
        'Dim values As New  _
        '   System.Collections.Generic.List( _
        '   Of AjaxControlToolkit.CascadingDropDownNameValue)

        'For Each dr As Data.DataRow In dtSupps.Rows
        '    values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        'Next

        'Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetCourseCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        Dim dtCats As Data.DataTable = objMatrix.GetCategories(Session("_VT_DotNetConnString"), CoursesFormListTable, CoursesModuleRootNode, CoursesModuleCategoriesNode, Session("_VT_CoursesRootId"))
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetCoursesInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategoryId As String = knownCategoryValuesDictionary("Category").ToString
        Dim dtSupps As Data.DataTable = objMatrix.GetItemsInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId, CoursesFormListTable)
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetRoleCategories(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        Dim dtCats As Data.DataTable = objMatrix.GetCategories(Session("_VT_DotNetConnString"), RolesFormListTable, RolesModuleRootNode, RolesModuleCategoriesNode, Session("_VT_RolesMatrixRootId"))
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtCats.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function


    <WebMethod(EnableSession:=True)> _
Public Function GetRolesInCategory(ByVal knownCategoryValues As String, ByVal category As String) As AjaxControlToolkit.CascadingDropDownNameValue()
        Dim objMatrix As New VT_CommonFunctions.MatrixFunctions

        Dim knownCategoryValuesDictionary As New StringDictionary
        knownCategoryValuesDictionary = AjaxControlToolkit.CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim strSelectedCategoryId As String = knownCategoryValuesDictionary("Category").ToString
        Dim dtSupps As Data.DataTable = objMatrix.GetItemsInCategory(Session("_VT_DotNetConnString"), strSelectedCategoryId, RolesFormListTable)
        Dim values As New _
           System.Collections.Generic.List( _
           Of AjaxControlToolkit.CascadingDropDownNameValue)

        For Each dr As Data.DataRow In dtSupps.Rows
            values.Add(New CascadingDropDownNameValue(dr.Item("FormName"), dr.Item("FormId")))
        Next

        Return values.ToArray

    End Function

End Class