
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>


<%@ Page Language="C#" 
    AutoEventWireup="true" 
    MasterPageFile="~/Paginas/Paginas_Generales/MasterPage.Master"
    CodeBehind="Frm_Ope_Facturas_Manual.aspx.cs" 
    Inherits="web_cambios_procesos.Paginas.Operacion.Frm_Ope_Facturas_Manual" %>



<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="../../Recursos/bootstrap-table/bootstrap-table.min.css" rel="stylesheet" />
    <link href="../../Recursos/bootstrap-table/extensions/editable/bootstrap-editable.css" rel="stylesheet" />
    <link href="../../Recursos/estilos/center_loader.css" rel="stylesheet" />
    <link href="../../Recursos/estilos/demo_form.css" rel="stylesheet" />
    <link href="../../Recursos/plugins/toastr/toastr.css" rel="stylesheet" />
    <link href="../../Recursos/bootstrap-combo/select2.css" rel="stylesheet" />
    <script src="../../Recursos/lightbox/ekko-lightbox.min.js"></script>
    <script src="../../Recursos/jquery-validate/jquery.validate.min.js"></script>
    <script src="../../Recursos/xenon/js/xenon-widgets.js"></script>
    <script src="../../Recursos/xenon/js/xenon-custom.js"></script>
    <script src="../../Recursos/bootstrap-table-current/bootstrap-table.js"></script>
    <script src="../../Recursos/bootstrap-table-current/locale/bootstrap-table-es-MX.js"></script>
    <script src="../../Recursos/bootstrap-table/extensions/editable/bootstrap-editable.js"></script>
    <script src="../../Recursos/bootstrap-table/extensions/editable/bootstrap-table-editable.min.js"></script>
    <script src="../../Recursos/bootstrap-table-current/extensions/editable/bootstrap-table-editable.js"></script>
    <script src="../../Recursos/bootstrap-combo/select2.js"></script>
    <script src="../../Recursos/bootstrap-combo/es.js"></script>
    <script src="../../Recursos/plugins/center-loader.min.js"></script>
    <script src="../../Recursos/plugins/parsley.js"></script>
    <script src="../../Recursos/jquery-numeric/jquery.numeric.js"></script>
    <script src="../../Recursos/jquery-numeric/accounting.min.js"></script>
    <script src="../../Recursos/jquery-numeric/jquery.formatCurrency-1.4.0.min.js"></script>
    <link href="../../Recursos/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
    <script src="../../Recursos/bootstrap-combo/select2.js"></script>
    
    <script src="../../Recursos/icon-picker/js/iconPicker.js"></script>
    <link href="../../Recursos/icheck/skins/all.css" rel="stylesheet" />
    <script src="../../Recursos/icheck/icheck.min.js"></script>

    <script src="../../Recursos/bootstrap-date/moment.min.js"></script>
    <script src="../../Recursos/bootstrap-date/bootstrap-datetimepicker.min.js"></script>
    <script src="../../Recursos/icon-picker/js/iconPicker.js"></script>
    <link href="../../Recursos/bootstrap-date/bootstrap-datetimepicker.css" rel="stylesheet" />
    <link href="../../Recursos/icon-picker/css/icon-picker.css" rel="stylesheet" />
    <script src="../../Recursos/bootstrap-date/bootstrap-datetimepicker.min.js"></script>

    
     <link href="../../Recursos/bootstrap-fileinput/fileinput.css" rel="stylesheet" />
     <script src="../../Recursos/bootstrap-fileinput/fileinput.min.js"></script>

    <script src="../../Recursos/icon-picker/js/iconPicker.js"></script>

    <script src="../../Recursos/javascript/seguridad/Js_Controlador_Sesion.js"></script>

    <script src="../../Recursos/javascript/operacion/Facturas_Manual/Js_Ope_Facturas_Manual.js"></script>
    <script src="../../Recursos/javascript/operacion/Facturas_Manual/Js_Ope_Facturas_Manual_Modal.js"></script>
    <script src="../../Recursos/javascript/operacion/Facturas_Manual/Js_Ope_Facturas_Manual_Historico.js"></script>
    <script src="../../Recursos/javascript/operacion/Facturas_Manual/Js_Ope_Facturas_Manual_Adjuntos.js"></script>
    <script src="../../Recursos/javascript/operacion/Facturas_Manual/Js_Ope_Facturas_Manual_Anticipo.js"></script>

    <style>
        .item-list-success-selected  {
            background-color:  #3498db  !important;
            
        }
        .item-list-success-selected a span {
            color: white !important;
        }
        .item-list-success {
            background-color:#FFF;
            
        }
            .item-list-success :hover {
                background-color:  #3498db ;
            }
            .item-list-success :hover span{
                
                color: #fff;
            }
            /*.item-list-success a span {
                color: black !important;
            }*/

        .item-list-success-disabled {
            background-color: #eee;  /*#aed6f1;*/
            color: blue !important;
        }
        .item-list-success-disabled :hover {
                background-color:  #3498db ;
                color: #fff;
            }
            /*.item-list-success-disabled a span {
                color: black !important;
            }*/
        .form-control {
            border-radius: 0px !important;
        }
        .input-group-btn {
            position: relative;
            font-size: 0;
            white-space: nowrap;
            padding: 4px !important;
            z-index: 3 !important;
        }
        .style_totales {
            font-size: 18px;
            float: right;
        }
        .nav.nav-tabs > li.active > a {
            background-color: #275C84;
            color: #fff;
        }
        .nav.nav-tabs + .tab-content {
            background-color: #EEE;
            border-radius: 0px 10px 10px 10px;
        }

    </style>

</asp:Content>

<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid" style="height: 100vh;">
        <div class="page-header">
            <h3 style="font-family: 'Roboto Light', cursive !important; font-size: 24px; font-weight: bold; color: #808080;">Facturas Manuales</h3>
        </div>

        <div id="Principal"></div>
        <div id="Modal"></div>

    </div>

    <div id="Historico"></div>
    <div id="Adjuntar"></div>
    <div id="Anticipo"></div>
    <div id="Conceptos_Xml"></div>

    
</asp:Content>



<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>
<%-- --------------------------------------------------------------------------------------------------------------------------------------- --%>

