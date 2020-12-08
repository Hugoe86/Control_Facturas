<%@ Page Title="Catálogo Roles" Language="C#" MasterPageFile="~/Paginas/Paginas_Generales/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="Frm_Apl_Control_Acceso_Sistema.aspx.cs"
    Inherits="web_cambios_procesos.Paginas.Paginas_Generales.Frm_Apl_Control_Acceso_Sistema" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Recursos/bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="../../Recursos/estilos/css_roles.css" rel="stylesheet" />
    <script src="../../Recursos/javascript/seguridad/Js_Apl_Roles.js"></script>
    <script src="../../Recursos/javascript/seguridad/Js_Apl_Roles.js"></script>

    <script type="text/javascript" language="javascript">
        function switchViews(obj,row) 
        { 
            var div = document.getElementById(obj); 
            var img = document.getElementById('img' + obj); 
             
            if (div.style.display=="none") 
            { 
                div.style.display = "inline"; 
                if (row=='alt') 
                { 
                    img.src="../../Recursos/img/stocks_indicator_down.png";
                } 
                else 
                { 
                    img.src="../../Recursos/img/stocks_indicator_down.png";
                } 
                img.alt = "Close to view other customers"; 
            } 
            else 
            { 
                div.style.display = "none"; 
                if (row=='alt') 
                { 
                    img.src="../../Recursos/img/add_up.png";
                } 
                else 
                { 
                    img.src="../../Recursos/img/add_up.png";
                } 
                img.alt = "Expand to show orders"; 
            } 
        }

        //SCRIPT PARA LA VALIDACION QUE NO EXPERE LA SESSION-->  
    
            //El nombre del controlador que mantiene la sesión
            var CONTROLADOR = "../../Mantenedor_Session.ashx";

        //Ejecuta el script en segundo plano evitando así que caduque la sesión de esta página
        function MantenSesion() {
            var head = document.getElementsByTagName('head').item(0);
            script = document.createElement('script');
            script.src = CONTROLADOR;
            script.setAttribute('type', 'text/javascript');
            script.defer = true;
            head.appendChild(script);
        }

        //Temporizador para matener la sesión activa
        setInterval("MantenSesion()", <%=(int)(0.9*(Session.Timeout * 60000))%>);         
        //-->
    </script>
    <style>
        .ajax__tab_xp .ajax__tab_tab {
            min-height: 21px !important;
        }

        .footer-type-1 {
            left: 80px !important;
        }

        .main-content {
            width: 100% !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sm_catalogo_roles" runat="server" AsyncPostBackTimeout="360000" ScriptMode="Release"></asp:ScriptManager>
    <asp:UpdatePanel ID="UPnl_Rpt_Totales_Tipo_Nomina_Banco" runat="server">
        <ContentTemplate>

            <asp:UpdateProgress ID="UPgs_Rpt_Totales_Tipo_Nomina_Banco" runat="server"
                AssociatedUpdatePanelID="UPnl_Rpt_Totales_Tipo_Nomina_Banco" DisplayAfter="0">
                <ProgressTemplate>
                    <div id="progressBackgroundFilter" class="progressBackgroundFilter"></div>
                    <div class="processMessage" id="div_progress">
                        <img alt="" src="../../Recursos/img/Updating2.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <div class="container-fluid" style="height: 100vh;">
                <div class="page-header" align="left">
                    <h3 style="font-family: 'Roboto Light', cursive !important; font-size: 24px; font-weight: bold; color: #808080;">Cat&aacute;logo de Roles</h3>
                </div>

                <div class="page-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <div id="Div_Contenedor_Msj_Error" runat="server" visible="false">
                                        <img src="../../Recursos/img/sias_warning.png" style="width: 24px; height: 24px;" />
                                        Es necesario introducir:
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="Lbl_Mensaje_Error" runat="server" Text="" CssClass="estilo_fuente_mensaje_error" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 col-md-offset-4" align="right">
                            <asp:ImageButton ID="Btn_Nuevo" ToolTip="Nuevo" runat="server" OnClick="Btn_Nuevo_Click" CausesValidation="false" Style="width: 24px; height: 24px;" />
                            <asp:ImageButton ID="Btn_Modificar" ToolTip="Modificar" runat="server" OnClick="Btn_Modificar_Click" CausesValidation="false" Style="width: 24px; height: 24px;" />
                            <asp:ImageButton ID="Btn_Eliminar" ToolTip="Eliminar" runat="server" OnClick="Btn_Eliminar_Click" CausesValidation="false" Style="width: 24px; height: 24px;"
                                ImageUrl="../../Recursos/img/icono_eliminar.png" OnClientClick="return confirm('¿Estas seguro de eliminar el rol seleccionado?');" />
                            <asp:ImageButton ID="Btn_Salir" ToolTip="Salir" runat="server" OnClick="Btn_Salir_Click" CausesValidation="false" Style="width: 24px; height: 24px;" />
                        </div>

                        <div class="col-md-2" align="right">
                            <asp:TextBox ID="Txt_Busqueda_Roles" runat="server" Width="80%" placeholder="buscar por nombre"
                                ToolTip="Ingrese el nombre del evento a buscar" />
                            <asp:ImageButton ID="Btn_Busqueda_Roles" runat="server" CausesValidation="False"
                                ImageUrl="../../Recursos/img/busqueda.png" ToolTip="Consultar" Height="18%"
                                OnClick="Btn_Busqueda_Roles_Click" />
                        </div>
                    </div>

                    <div style="margin-top: 10px;"></div>

                    <div class="row">
                        <div class="col-md-2">
                            Nivel
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="Cmb_Nivel" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            Estatus
                        </div>
                        <div class="col-md-4">
                            <asp:DropDownList ID="Cmb_Estatus" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            Nombre
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="Txt_Nombre" runat="server" CssClass="form-control input-sm" />
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="Txt_Nombre"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters" ValidChars=" Ñ" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-2">
                            Comentarios
                        </div>
                        <div class="col-md-10">
                            <asp:TextBox ID="Txt_Comentarios" runat="server" TextMode="MultiLine" CssClass="form-control" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <cc1:TabContainer ID="Contenedor_Roles_Setup_Access" runat="server" Width="100%"
                            ActiveTabIndex="0">
                            <cc1:TabPanel HeaderText="Lista Roles" ID="Tab_Listar_Roles" runat="server">
                                <HeaderTemplate>
                                    Roles
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="Tbl_Lista_Roles_Sistema" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="4" Width="100%" OnSelectedIndexChanged="Tbl_Lista_Roles_Sistema_SelectedIndexChanged"
                                                    OnPageIndexChanging="Tbl_Lista_Roles_Sistema_PageIndexChanging" AllowPaging="True" PageSize="10"
                                                    CssClass="GridView_1">
                                                    <Columns>
                                                        <asp:ButtonField ButtonType="Image" CommandName="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                            ImageUrl="~/Recursos/img/blue_button.png"
                                                            HeaderText="">
                                                            <ItemStyle Width="5%" />
                                                        </asp:ButtonField>
                                                        <asp:BoundField DataField="ROL_ID" HeaderText="Rol ID">
                                                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="10%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                            <HeaderStyle Width="90%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="90%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="DESCRIPCION" HeaderText="Comentarios">
                                                            <HeaderStyle Width="0%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Estatus_ID" HeaderText="">
                                                            <HeaderStyle Width="0%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Nivel_ID" HeaderText="">
                                                            <HeaderStyle Width="0%" HorizontalAlign="Left" />
                                                            <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                                                                       <RowStyle CssClass="GridItem" />
                                                    <PagerStyle CssClass="GridHeader" />
                                                    <SelectedRowStyle CssClass="GridSelected" />
                                                    <HeaderStyle CssClass="GridHeader" />
                                                    <AlternatingRowStyle CssClass="GridAltItem" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="Listar_Accesos" HeaderText="Configuración Accesos" runat="server">
                                <ContentTemplate>
                                    <div id="Div_Grid_Menus">
                                        <asp:GridView ID="Grid_Menus" runat="server" CssClass="GridView_Nested"
                                            AutoGenerateColumns="False" GridLines="None" DataKeyNames="MENU_ID"
                                            OnRowDataBound="Grid_Menus_RowDataBound">

                                            <SelectedRowStyle CssClass="GridSelected_Nested" />
                                            <PagerStyle CssClass="GridHeader_Nested" />
                                            <HeaderStyle CssClass="GridHeader_Nested" />
                                            <AlternatingRowStyle CssClass="GridAltItem_Nested" />

                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a href="javascript:switchViews('div<%# Eval("MENU_ID") %>', 'one');">
                                                            <img id="imgdiv<%# Eval("MENU_ID") %>" alt="Click to show/hide orders" border="0" src="../../Recursos/img/add_up.png" />
                                                        </a>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <a href="javascript:switchViews('div<%# Eval("MENU_ID") %>', 'alt');">
                                                            <img id="imgdiv<%# Eval("MENU_ID") %>" alt="Click to show/hide orders" border="0" src="../../Recursos/img/add_up.png" />
                                                        </a>
                                                    </AlternatingItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MENU_ID" HeaderText="Menú ID">
                                                    <HeaderStyle Width="0%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="true" />
                                                    <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="false" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NOMBRE" HeaderText="Nombre">
                                                    <HeaderStyle Width="20%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="true" />
                                                    <ItemStyle Width="20%" HorizontalAlign="Left" Font-Size="11px" Font-Bold="false" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Habilitar">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Habilitar" runat="server" ToolTip='<%# Eval("MENU_ID") %>' OnClick="javascript:Chk_Habilitar_CheckedChanged(this);" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        </td>
                                                    </tr> 
                                                    <tr>
                                                        <td colspan="100%">
                                                            <div id="div<%# Eval("MENU_ID") %>" style="display: none; position: relative; left: 25px;">
                                                                <asp:GridView ID="Grid_Submenus" runat="server" CssClass="GridView_Nested"
                                                                    AutoGenerateColumns="False" GridLines="None" Width="78%">

                                                                    <SelectedRowStyle CssClass="GridSelected_Nested" />
                                                                    <PagerStyle CssClass="GridHeader_Nested" />
                                                                    <HeaderStyle CssClass="GridHeader_Nested" />
                                                                    <AlternatingRowStyle CssClass="GridAltItem_Nested" />
                                                                    <Columns>
                                                                        <asp:BoundField DataField="MENU_ID" HeaderText="SubMenú"
                                                                            Visible="True">
                                                                            <HeaderStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="0%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="NOMBRE" HeaderText="Nombre"
                                                                            Visible="True">
                                                                            <HeaderStyle Width="25%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="25%" HorizontalAlign="Left" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Habilitar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Chk_Habilitar" runat="server" CssClass='<%# Eval("PARENT_ID") %>' ToolTip='<%# Eval("MENU_ID") %>' data-rootleaf='<%# Eval("PARENT_ID").ToString().Trim() + "" + Eval("MENU_ID").ToString().Trim() %>'
                                                                                    OnClick="javascript:Chk_Habilitar_Interno_CheckedChanged(this);" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Alta" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Chk_Alta" runat="server" CssClass='<%# Eval("PARENT_ID") %>' ToolTip='<%# Eval("MENU_ID") %>'
                                                                                    OnClick="javascript:change_access_page(this);" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Cambio" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Chk_Cambio" runat="server" CssClass='<%# Eval("PARENT_ID") %>' ToolTip='<%# Eval("MENU_ID") %>'
                                                                                    OnClick="javascript:change_access_page(this);" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Eliminar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Chk_Eliminar" runat="server" CssClass='<%# Eval("PARENT_ID") %>' ToolTip='<%# Eval("MENU_ID") %>'
                                                                                    OnClick="javascript:change_access_page(this);" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Consultar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="Chk_Consultar" runat="server" CssClass='<%# Eval("PARENT_ID") %>' ToolTip='<%# Eval("MENU_ID") %>'
                                                                                    OnClick="javascript:change_access_page(this);" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="true" />
                                                                            <ItemStyle Width="15%" HorizontalAlign="Center" Font-Size="10px" Font-Bold="false" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </div>
                </div>
            </div>
            </div>

            <asp:HiddenField ID="Txt_Rol_ID" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
