<%@ Page Title="Catálogo de Parametros" Language="C#" MasterPageFile="~/Paginas/Paginas_Generales/MasterPage.Master" AutoEventWireup="true" CodeBehind="Frm_Tra_Cat_Parametros.aspx.cs" Inherits="web_cambios_procesos.Paginas.Catalogos.Frm_Tra_Cat_Parametros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Recursos/bootstrap-table/bootstrap-table.min.css" rel="stylesheet" />
    <link href="../../Recursos/estilos/center_loader.css" rel="stylesheet" />
    <link href="../../Recursos/estilos/demo_form.css" rel="stylesheet" />

    <script src="../../Recursos/plugins/center-loader.min.js"></script>
    <script src="../../Recursos/plugins/parsley.js"></script>
    <script src="../../Recursos/bootstrap-table/bootstrap-table.min.js"></script>
    <script src="../../Recursos/bootstrap-table/locale/bootstrap-table-es-MX.min.js"></script>
    <script src="../../Recursos/javascript/seguridad/Js_Controlador_Sesion.js"></script>
    <script src="../../Recursos/javascript/catalogos/Js_Tra_Cat_Parametros.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid" style="height: 100vh;">

        <div class="page-header" align="left">
            <h3 style="font-family: 'Roboto Light', cursive !important; font-size: 24px; font-weight: bold; color: #808080;">Cat&aacute;logo de Par&aacute;metros</h3>
        </div>

        <div class="panel panel-color panel-info" id="panel1">
            <div class="panel-heading">
                <h3 class="panel-title">
                    <i style="color: white;" class="glyphicon glyphicon-filter"></i>&nbsp;Parametros
                </h3>
            </div>
            <div class="panel-body">
                <input type="hidden" id="txt_parametro_id"/>
                <div class="row">
                    <div class="col-md-12 text-right">
                        <div class="btn-group" role="group" style="margin-left: 5px;">
                            <button id="btn_salir" type="button" class="btn btn-info btn-sm" title="Salir"><i class="fa fa-home"></i>&nbsp;<span>Inicio</span></button>
                            <button id="btn_nuevo" type="button" class="btn btn-info btn-sm" title="Actualizar"><i class="fa fa-plus"></i>&nbsp;<span>Nuevo</span></button>
                        </div>
                    </div>
                </div>

                <%--<div class="row">
                    <div class="col-md-6 col-md-offset-3">
                        <label class="fuente_lbl_controles">Area de Captura de Facturas</label>
                        <select class="form-control" id="cmb_area_captura_factura" style="width:100%"></select>
                    </div>
                </div>--%>
                <div class="row">
                    <div class="col-md-6 col-md-offset-3">
                        <label class="fuente_lbl_controles">Folio de Inicio de Captura de Facturas</label>
                        <input type="text" class="form-control" id="txt_folio_inicia" placeholder="Folio de Inicio" />
                    </div>
                </div>
            </div>
        </div>

       
    </div>
</asp:Content>
