Imports BPADotNetCommonFunctions
Imports System.Data.SqlClient
Imports System.Data
Imports System.Drawing
Imports VTDBFunctions.VTDBFunctions

Partial Class TabPages_HandheldTxns
    Inherits MyBasePage



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' register the Activate button with the ScriptManager
        Dim TheManager As New ScriptManager
        TheManager = Master.FindControl("ScriptManager1")
        '     TheManager.RegisterAsyncPostBackControl(btnAddDelivery)

       

        If Not IsPostBack Then

            If Me.CurrentSession.VT_SalesOrderNum <> 0 Then
                FillSalesOrderGrid(Me.CurrentSession.VT_SalesOrderNum)

                FillTxnsOrderGrids(Me.CurrentSession.VT_SalesOrderNum)
            End If
        End If
    End Sub

    Sub FillSalesOrderGrid(Optional ByVal OrderNumber As String = "")

        Dim objtrace As New LocalTraceFunctions.Trace
        Dim strQuery As String
        Dim dt As New Data.DataTable

        strQuery = "OrderNumber = " & OrderNumber
        dt = objtrace.GetRawTableData("tcd_tblSalesOrders", strQuery)

        uwgtcd_tblSalesOrders.DataSource = dt
        uwgtcd_tblSalesOrders.DataBind()

    End Sub


    Sub FillTxnsOrderGrids(Optional ByVal strOrderNumber As String = "")


        Dim objtrace As New LocalTraceFunctions.Trace
        Dim strDetailQuery As String
        Dim strTxnQuery As String
        Dim strBackupQuery As String


        Dim dt As New Data.DataTable
        If strOrderNumber <> "" Then
            strTxnQuery = "SalesOrderNum =" & strOrderNumber & " AND TransactionType = 6 "
        Else
            strTxnQuery = " TransactionType = 6 "
        End If
        dt = objtrace.GetRawTableData("trc_Transactions", strTxnQuery)

        uwgTransactions.DataSource = dt
        uwgTransactions.DataBind()


        Dim dtInter As New Data.DataTable
        If strOrderNumber <> "" Then
            strDetailQuery = "SalesOrderNumber =" & strOrderNumber & " AND Status = 6 "
        Else
            strDetailQuery = " Status = 6 "
        End If

        dtInter = objtrace.GetRawTableData("tcd_TransactionDetails", strDetailQuery)

        uwgIntermediateTxns.DataSource = dtInter
        uwgIntermediateTxns.DataBind()


        Dim dtBack As New Data.DataTable
        If strOrderNumber <> "" Then
            strBackupQuery = "SalesOrderNumber =" & strOrderNumber & " AND TransactionType = 6 "
        Else
            strBackupQuery = " TransactionType = 6 "
        End If

        dtBack = objtrace.GetRawTableData("tcd_Backup", strBackupQuery)

        uwgHHBackup.DataSource = dtBack
        uwgHHBackup.DataBind()


        Dim dtHHTxn As New Data.DataTable

        dtHHTxn = objtrace.GetRawTableData("tcd_tblTransactions", "")

        uwgHHTxns.DataSource = dtHHTxn
        uwgHHTxns.DataBind()

    End Sub

    Protected Sub LoadHeaderInfo()


    End Sub

    

    

    

   

    

  

    
End Class
