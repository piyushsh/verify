<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    
    <!-- Library CSS Files -->
    <link rel="stylesheet" href="../App_Themes/lib/bootstrap.min.css"/>
    
    <link href="../jg_styles.css" rel="stylesheet" type="text/css" />
    <link href="../Verify_Infragistics.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/VT_Responsive_V1.css" rel="stylesheet" />
    
    <!-- New CSS Files -->
    <link rel="stylesheet" href="../App_Themes/resp/main.css"/>
    <link rel="stylesheet" href="../App_Themes/resp/common.css"/>
    <link rel="stylesheet" href="../App_Themes/resp/edit_form.css"/>

    <style type="text/css">
        .VerifyGrid_Report_Frame {
            margin-right: 0px;
        }
            
.VT_ActionButton
{
	border: 1px solid #682225;
	background-color: #FBF2ED;
	font-family: Arial;
	font-style: italic;
	font-weight: bold;
	color: #682225;
	border-radius: 2px 2px 2px 2px;
}
        .auto-style16 {
            height: 32px;
        }
        .auto-style17 {
            height: 28px;
        }
        .auto-style20 {
            float: inherit;
            width: 2102px;
        }
        </style>
        <script language="javascript" type="text/javascript" src="../BusyBox.js"></script>

</head>
<body style="margin-top:0px" onbeforeunload="ShowSpinner();">

 
        <script type="text/javascript" src="..\scripts\json2.js">    </script>
        <script type="text/javascript" src="..\Scripts\jquery-1.10.2.js"></script>
        <script type="text/javascript" src="..\Scripts\jquery.tmpl.js"></script>
    
    <!-- Library Scripts -->
    <script src="../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/respond.min.js"></script>
    <script src="../Scripts/resp_common_scripts.js"></script>
    


    <form id="form1" runat="server">


        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" AsyncPostBackTimeout="360000">
                <Scripts>
                    <asp:ScriptReference Path="../Scripts/CommonScripts.js" />
                    <asp:ScriptReference Path="../Scripts/VT_Infragistics.js" />
                </Scripts>
            </asp:ScriptManager>
            

              <script language="javascript" type="text/javascript">

                 
            </script>
        
        <div class="fixed-on-top">
            <!-- Header & Navigation -->
            <header>
                <div class="navbar navbar-default">
                        <div class="container">
                            <div class="navbar-header">
                                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                    <span class="icon-bar"></span>
                                </button>
                                <div class="navbar-brand" runat="server" href="#">
                                    <asp:ImageButton ID="btnAnchorPoint" runat="server" ImageUrl="~/App_Themes/TabButtons/AnchorButton_Light.gif" CssClass="menu-btn"/>
                                </div>
                            </div>
                            <div class="navbar-collapse collapse pull-left">
                                <ul class="nav navbar-nav">
                                    <li><asp:Button ID="btnTOP_Quotes" runat="server"  Text="Quotes" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li><asp:Button ID="btnTOP_Planning" runat="server" Text="Planning" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li><asp:Button ID="btnTOP_Orders" runat="server" Text="SalesOrders" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li><asp:Button ID="btnTOP_Deliveries" runat="server" Text="Dispatches" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li><asp:Button ID="btnTOP_Printouts" runat="server" Text="Reports" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li><asp:Button ID="btnTOP_SliceAndDice" runat="server" Text="Slice & Dice" CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li><asp:Button ID="btnTOP_SYSADMIN" runat="server" Text="More..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                    <li class="dropdown">
                                        <a id="more_links" href="" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">More Link &gt;&gt;</a>
                                        <ul class="dropdown-menu" aria-labelledby="more_links">
                                            <li><asp:Button ID="Button1" runat="server" Text="More Button..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                            <li><asp:Button ID="Button2" runat="server" Text="More Button..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                            <li><asp:Button ID="Button3" runat="server" Text="More Button..." CssClass="VT_UnSelectTab_Light" BorderStyle="None" /></li>
                                            <li role="separator" class="divider"></li>
                                            <li><a href="">Manage Customers link</a></li>
                                            <li role="separator" class="divider"></li>
                                            <li><a href="">Manage Customers link</a></li>
                                            <li><a href="">Manage Customers link</a></li>
                                            <li><a href="">Manage Customers</a></li>
                                            <li role="separator" class="divider"></li>
                                            <li><a href="">Manage Customers</a></li>
                                            <li><a href="">Manage Customers</a></li>
                                            <li><a href="">Manage Customers</a></li>
                                        </ul>
                                    </li>
                                </ul>
                                <!-- Verify Logo -->
                            </div>
                            <div class="pull-right logo_container">
                                <asp:Image ID="imgBannerLogo" runat="server" ImageUrl="~/App_Themes/TabButtons/VerifyLogo_Responsive.jpg" CssClass="img-responsive"/>
    <%--                            <img src="../assets/images/logo.png"  class="img-responsive"/>--%>
                                <span class="portal_title">Sales Management Portal</span>
                            </div>
                        </div>
                    </div>
        </header>

            <!-- Page Info Section -->
            <div class="container">
                <div class="row page-info">
                    <div class="col-md-1">
                        <a href="#" class="back_link"><span class="glyphicon glyphicon-circle-arrow-left"></span> Back</a>
                    </div>
                    <div class="col-md-3 text-left">
                        <asp:Label ID="Label1" runat="server" CssClass="page-title" Text="Edit Sales Order"></asp:Label>
                    </div>
                    <div class="col-md-6 user_info_container">
                        <div class="user-info">
                            <span>Welcome : <strong>John Gleeson</strong></span>
                            <span>Today: 25/06/2014</span>
                            <span>Time Zone: Eastern Standard Time</span>
                            <span>10:24</span>
                        </div>
                    </div>
                    <div class="col-md-1 help text-center">
                        <a href="#"><span class="glyphicon glyphicon-question-sign"></span></a>
                    </div>
                    <div class="col-md-1 logout">
                        <a href="#"><span class="glyphicon glyphicon-arrow-right"></span> Logout</a>
                    </div>
                </div>
            </div>

            <!-- Tool belt -->
            <div class="container tool_belt">
                <div class="row">
                    <div class="col-md-offset-7 col-md-3 text-right">
                        <button class="action_button">Print Form</button>
                        <button class="action_button">Issue</button>
                    </div>
                    <div class="col-xs-2 text-right">
                        <button class="action_button edit-mode">Enter Edit Mode</button>
                    </div>
                </div>
                <div class="section_drop_down_container">
                    <div class="row">
                        <div class="col-xs-5">
                            <h3>SECTIONS</h3>
                        </div>
                        <div class="col-xs-7">
                            <select>
                                <option value="">Details</option>
                                <option value="">Details</option>
                                <option value="">Details</option>
                                <option value="">Details</option>
                            </select>
                        </div>
                    </div>
                    <div class="diagonal_div"></div>
                </div>
            </div>    
        </div>
        
        <div class="main-body after-fix-header">
            <!-- Section Detail -->
            <div class="container">
                <div class="section_detail_container">
                    <div class="row">
                        <div class="col-md-4 col-md-offset-5 text-center">
                            <span>53425: Sales Order No. SO1962</span>
                        </div>
                        <div class="col-md-2 text-center">
                            <span class="status">Status:</span> <span>Pre-Issued</span>
                        </div>
                        <div class="col-md-1 text-right">
                            <h3 class="section_name">Details</h3>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Sold-to-Customer-Detail Form -->
            <div class="container section-form-container dark_yellow_background">
                <!-- Section heading -->
                <div class="row">
                    <div class="col-md-4">
                        <h2 class="section-form-heading">Sold To - Customer Details:</h2>
                    </div>
                </div>
                <!-- Form fields -->
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Customer:</label>
                                </div>
                                <div class="col-md-8">
                                    <select name="customer">
                                        <option value="">Lidl Ireland Ltd.</option>
                                        <option value="">Dummy 1</option>
                                        <option value="">Dummy 2</option>
                                        <option value="">Dummy 3</option>
                                        <option value="">Dummy 4</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Categorey:</label>
                                </div>
                                <div class="col-md-8">
                                    <select name="category">
                                        <option value="">D</option>
                                        <option value="">A</option>
                                        <option value="">B</option>
                                        <option value="">C</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Priority:</label>
                                </div>
                                <div class="col-md-8">
                                    <select name="priority">
                                        <option value="">1</option>
                                        <option value="">2</option>
                                        <option value="">3</option>
                                        <option value="">4</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Contact:</label>
                                </div>
                                <div class="col-md-8">
                                    <select name="customer">
                                        <option value="">John Ryan</option>
                                        <option value="">John Ryan 2</option>
                                        <option value="">John Ryan 3</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Currency:</label>
                                </div>
                                <div class="col-md-8">
                                   <span>N/A</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Phone:</label>
                                </div>
                                <div class="col-md-8">
                                    <span>01 9876543</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>% Discount:</label>
                                </div>
                                <div class="col-md-8">
                                   <span>0%</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Country:</label>
                                </div>
                                <div class="col-md-8">
                                    <span>Ireland</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Tax Exempt?</label>
                                </div>
                                <div class="col-md-8">
                                   <span>No</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Deliver-to-customer-details Form -->
            <div class="container section-form-container light_yellow_background">
                <!-- Section heading -->
                <div class="row">
                    <div class="col-md-4">
                        <h2 class="section-form-heading">Deliver To - Customer Details:</h2>
                    </div>
                    <div class="col-md-5 form-group">
                        <input type="checkbox" class="inline-form-field"/> <label class="inline-label">Only show delivery customers for this billing customer</label>
                    </div>
                    <div class="col-md-1 form-group">
                        <button class="refresh_btn">Refresh</button>
                    </div>
                </div>
                <!-- Form fields -->
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Category:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <select name="category">
                                                <option value="">Dummy</option>
                                                <option value="">Dummy 1</option>
                                                <option value="">Dummy 2</option>
                                                <option value="">Dummy 3</option>
                                                <option value="">Dummy 4</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Customer:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <select name="customer">
                                                <option value="">C1</option>
                                                <option value="">C2</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-3">
                                    <label>Delivery Address:</label>
                                </div>
                                <div class="col-md-8">
                                    <p>Unit 2B, Beechwood Drive, Sandyford Ind. Est. Dublin 18</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Route:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <textarea></textarea>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Alt. Route:</label>
                                        </div>
                                        <div class="col-md-8">
                                            <textarea></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Bill-to-customer-details Form -->
            <div class="container section-form-container light_yellow_background">
                <!-- Section heading -->
                <div class="row">
                    <div class="col-md-4">
                        <h2 class="section-form-heading">Bill To - Customer Details:</h2>
                    </div>
                </div>
                <!-- Form fields -->
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row">
                                <div class="col-md-4">
                                    <label>Customer:</label>
                                </div>
                                <div class="col-md-8">
                                    <span>Lidl Ireland Ltd.</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="row">
                                <div class="col-md-2">
                                    <label>Address:</label>
                                </div>
                                <div class="col-md-10">
                                    <p>Unit 2B, Beechwood Drive, Sandyford Ind. Est. Dublin 18</p>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="row">
                                <div class="col-md-5">
                                    <label>Contact:</label>
                                </div>
                                <div class="col-md-7">
                                    <span>John Ryan</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="row">
                                <div class="col-md-5">
                                    <label>Phone:</label>
                                </div>
                                <div class="col-md-7">
                                    <span>01 9876543</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- Date&Other-information-details Form -->
            <div class="container section-form-container light_yellow_background">
                <!-- Section heading -->
                <div class="row">
                    <div class="col-md-4">
                        <h2 class="section-form-heading">Dates & Other Information</h2>
                    </div>
                </div>
                <!-- Form fields -->
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-7">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label>Customer Order Date:</label>
                                        </div>
                                        <div class="col-md-6">
                                            <input type="date" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label>Customer PO No.</label>
                                        </div>
                                        <div class="col-md-6">
                                            <input type="text" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label>Requested Delivery Date:</label>
                                        </div>
                                        <div class="col-md-6">
                                            <input type="date" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <label>Delivery Terms:</label>
                                        </div>
                                        <div class="col-md-6">
                                            <span>30 days</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-5">
                            <div class="row">
                                <div class="col-md-3">
                                    <label class="allow-word-wrap">Comment for this Sales Order:</label>
                                    <label class="informative-text allow-word-wrap">(This comment is displayed on the customer acknowledgement)</label>
                                </div>
                                <div class="col-md-9">
                                    <textarea></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="action-section-container">
            <div class="container">
                <div class="row">
                    <div class="col-md-2 col-md-offset-10 text-right">
                        <a href="#" class="action-link"><label>Next Section</label><span class="glyphicon glyphicon-circle-arrow-right"></span></a>
                    </div>
                </div>
            </div>
        </div>
        
    <footer class="top-margin-0">
        <div class="container">
            <p class="text-center powered_by">POWERED BY</p>
            <div class="green_horizontal_line">
                <div class="verify_signature"><img src="../assets/images/logo.png"/></div>
            </div>
        </div>
    </footer>
        
    </form>
</body>
</html>


