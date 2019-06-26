<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="WebFormsControlls.WebControls" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<html lang="en">
<head runat="server">
    <meta http-equiv="refresh" content="120">     
    
    <title> Legrand Build Release status </title>
    <metacharset="UTF-8">

    <meta name="viewport" content="widht=device-width, initial-scale=1.0", shrink-to-fit="Yes" />
    
    <link rel="stylesheet" href="css/bootstrap.min.css" />
    <link rel="stylesheet" href="css/custom.css" />
     <link rel="stylesheet" href="css.css" type="text/css">
    
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <!-- <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"> -->

        
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.4.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js"></script>

</head>    

    <nav class="navbar navbar-inverse navbar-fixed-top">
        <div class="container-fluid">
            
        <div class="row">              
            <div class="col-sm-8">
                <h2 style="color:whitesmoke ">Legrand Azure-DevOps Build and Release Status</h2>            
            </div>

            <div class="col-sm-4">
                <img class="pull-right img-responsive" style="max-width:120px; margin-top: 10px; align-right" src="/images/legrand-logo.png" alt="LegrandLogo" />        
                <br>
                <br>                
                <!--  <p id="dt" style="color:whitesmoke; text-align:right"> last updated on: ></p> -->
                <p id="dt" style="color:whitesmoke; text-align:right; padding-top: 5px; padding-bottom: 0px;"> </p>
                <script>
                    var d = new Date();
                    document.getElementById("dt").innerHTML = "Last updated on " + d.toUTCString();
                </script>                      
                
            </div>                     
        </div>    

            

        <div class="navbar-header">
            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#myNavbar">
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span> 
            </button>
            <div class="collapse navbar-collapse" id="myNavbar">
                <ul class="nav navbar-nav">
                    <li><a href="#" onserverclick="OverallStat" id="overallstatus" runat='server'>Overall Status</a></li>                    
                    <!-- <li><a href="#" onclick="myFunction()" id="buildstat">Build Status</a></li> -->
                    <li><a href="#" onserverclick='OverallStat' id='buildstat' runat='server'>Build Status </a></li>
                    <li><a href="#" onserverclick="OverallStat" id="relstat" runat="server">Release Status </a></li>
                    <li><a href="#" onserverclick="OverallStat" id="failstat" runat="server">Failed Status </a></li>                   
                    <li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="#">Project status <span class="caret"></span></a>
                        <ul class="dropdown-menu" runat="server" id="projectmenu">                                   
                            
                        </ul>
                    </li>
                </ul>                
                <!-- <img class='pull-right img - responsive ' src = '/Images/notDeployed.png'  width= '35' height= '35'> -->
            </div>            
        </div>            
    </nav>    
    

    <style>
        body {
            padding-top: 110px;
        }

        div.table-wrapper {
            border: 1px solid #ccc;
            height: 500px;
            width: 100%;
            overflow-y: auto;
        }

        table {
            width: 100%
        }

            table thead tr th {
                text-align: left;
                position: sticky;
                top: 0px;
                background-color: #666;
                color: #fff;
            }

        .well {
            min-height: 20px;
            padding: 19px;
            margin-bottom: -10px;
            background-color: #f5f5f5;
            border: 1px solid #e3e3e3;
            border-radius: 4px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.05);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.05);
        }

        .navbar-inverse .nav-item.active .nav-link,
        {
            color: #ffffff;
        }

        .selectedMenu {
            color: white !important;
        }

        .table-row{
            cursor:pointer;
        }
        .vcenter {
            display: inline-flex;
            vertical-align: middle;
            float: none;
            text-align: center;
        }

    </style>

    <body>       
        <form id="form1" runat="server">                     
            <asp:Literal id = "TabOverStat" runat = "server" />                       
           
        </form>
        <script type="text/javascript" src="Scripts/jquery-3.4.1.min.js"></script>
    <%--<script type="text/javascript" src="Scripts/bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
     <script type="text/javascript">

         
             function confirm(val) {
                 if ($('.selectedmenu').length > 0) {
                     $('.selectedmenu').removeclass('selectedmenu');
                 }
                 if (val.length > 0) {
                     var menuaid = '#' + val;
                     $(menuaid).addclass('selectedmenu');
                 }
             }
        


                $(document).ready(function($) {
                    $(".table-row").click(function () {
                        var damn = window.open();                        
                        damn.location = $(this).data("href");
                        //Window.open().location = $(this).data("href");
                        //window.document.location = $(this).data("href");
                        //EventTarget = _blank; 
                        
                    });
         });

         var $table = $('#table');
         $(function () {
             $('#toolbar').find('select').change(function () {
                 $table.bootstrapTable('refreshOptions', {
                     exportDataType: $(this).val()
                 });
             });
         })

         var trBoldBlue = $("table");

         $(trBoldBlue).on("click", "tr", function () {
             $(this).toggleClass("bold-blue");
         });

     </script>
    </body>       

    

</html>