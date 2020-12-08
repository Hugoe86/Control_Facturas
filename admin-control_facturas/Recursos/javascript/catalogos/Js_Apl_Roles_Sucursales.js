var closeModalProdUbicacion = true;
$.fn.modal.Constructor.prototype.enforceFocus = function () { };

$(document).on('ready', function () {
    _inicializar_pagina_roles_sucursales();
});

function _inicializar_pagina_roles_sucursales() {
    try {

        _modal_roles_sucursales();
        _cargar_tabla_rol_sucursal();
        _eventos_roles_sucursales_textbox();
        //_eventos_rol_sucursal();
        _enter_keypress_modal_roles_sucursales();
        _consultar_sucursal();
        _consultar_roles();
        _limpiar_controles_rol_sucursal();
        _set_location_toolbar_prod_prov();
    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Técnico', e);
    }
}

function _estado_inicial_rol_sucursal() {
    try {
        //_habilitar_controles_rol_sucursal('Inicio');
        _limpiar_controles_rol_sucursal();
        $('#tbl_roles_sucursales').bootstrapTable('refresh', 'controller/Roles_Sucursales_Controller.asmx/Consultar_Roles_Sucursales');
        _set_location_toolbar_prod_prov();
    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Técnico', e);
    }
}

function _habilitar_controles_rol_sucursal(opcion) {
    var Estatus = false;
    $('#div_roles').css('display', 'none');
    $('#div_sucursal').css('display', 'none');
    switch (opcion) {
        case "Sucursal":
            Estatus = true;
            $('#div_sucursal').css('display', 'block');
            break;
        case "Roles":
            Estatus = true;
            $('#div_roles').css('display', 'block');
            break;
        case "Inicio":
            break;
    }
}

function _limpiar_controles_rol_sucursal() {
    //$('input[type=text]').each(function () { $(this).val(''); });
    //$('select').each(function () { $(this).val(''); });
    //$('textarea').each(function () { $(this).val(''); });
    $('#cmb_sucursal').select2('val', '');
    $('#cmb_roles').select2('val', '');
    _validation_rol_sucursal_sumary(null);
    _clear_all_class_error();
}

function _eventos_rol_sucursal() {
    try {
        $('#_modal_datos_rol_sucursal').on('hidden.bs.modal', function () {
            if (!closeModalProdUbicacion)
                $(this).modal('show');
        });

        $('.cancelar').each(function (index, element) {
            $(this).on('click', function (e) {
                e.preventDefault();
                _estado_inicial_rol_sucursal();
            });
        });
        $('#_modal_datos_rol_sucursal input[type=text]').each(function (index, element) {
            $(this).on('focus', function () {
                _remove_class_error('#' + $(this).attr('id'));
            });
        });
    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Técnico', e);
    }
}

function _eventos_roles_sucursales_textbox() {
    $('#txt_nombre').on('blur', function () {
        $(this).val($(this).val().match(/^[^'#&\\]*$/) ? $(this).val() : $(this).val().replace(/'*#*&*\\*/gi, ''));
    });

    $('#txt_grupo').on('blur', function () {
        $(this).val($(this).val().match(/^[0-9a-zA-Z]+$/) ? $(this).val() : $(this).val().replace(/\W+/g, ''));
    });
}

function _mostrar_mensaje_rol_sucursal(Titulo, Mensaje) {
    bootbox.dialog({
        message: Mensaje,
        title: Titulo,
        locale: 'es',
        closeButton: true,
        buttons: [{
            label: 'Cerrar',
            className: 'btn-default',
            callback: function () { }
        }]
    });
}

function _cargar_tabla_rol_sucursal() {

    try {
        $('#tbl_roles_sucursales').bootstrapTable('destroy');
        $('#tbl_roles_sucursales').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDysplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            clickToSelect: true,
            columns: [
                { field: 'Sucursal_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: true, visible: false },
                { field: 'Rol_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: true, visible: false },
                { field: 'Sucursal', title: 'Sucursal', width: 300, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                { field: 'Rol', title: 'Rol', width: 300, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                {
                    field: 'Sucursal_ID',
                    title: '',
                    align: 'center',
                    valign: 'bottom',
                    width: 60,
                    clickToSelect: false,
                    formatter: function (value, row) {
                        return '<div> ' +
                            '<a class="remove ml10 delete" id="' + row.Sucursal_ID + "_" + row.Rol_ID + '" href="javascript:void(0)" data-prod-ubic=\'' + JSON.stringify(row) + '\' onclick="_eliminar_rol_sucursal(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></a>' +
                            '</div>';
                    }
                }
            ]
        });
    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Técnico', e);
    }
}

function _alta_rol_sucursal() {
    var Producto_Proveedor = null;
    var isComplete = false;

    try {

        Producto_Proveedor = new Object();
        Producto_Proveedor.Rol_ID = $('#txt_roles').val() === '' ? parseInt($('#cmb_roles').val()) : parseInt($('#txt_roles').val());
        Producto_Proveedor.Sucursal_ID = $('#txt_sucursal').val() === '' ? parseInt($('#cmb_sucursal').val()) : parseInt($('#txt_sucursal').val());


        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Producto_Proveedor) });

        $.ajax({
            type: 'POST',
            url: 'controller/Roles_Sucursales_Controller.asmx/Alta',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        _search_rol_sucursal();
                        isComplete = true;
                    } else if (Resultado.Estatus == 'error') {
                        _validation_rol_sucursal_sumary(Resultado);
                    }
                } else {
                    _validation_rol_sucursal_sumary(Resultado);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Tecnico', e);
    }
    return isComplete;
}

function _modificar_unidad() {
    var Unidad = null;
    var isComplete = false;

    try {
        Unidad = new Object();
        Unidad.Unidad_ID = parseInt($('#txt_unidad_id').val());
        Unidad.Grupo = $('#txt_grupo').val();
        Unidad.Nombre = $('#txt_nombre').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Unidad) });

        $.ajax({
            type: 'POST',
            url: 'controller/Roles_Sucursales_Controller.asmx/Actualizar',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        _search_rol_sucursal();
                        isComplete = true;
                    } else if (Resultado.Estatus == 'error') {
                        _validation_rol_sucursal_sumary(Resultado);
                    }
                } else {
                    _validation_rol_sucursal_sumary(Resultado);
                }
            }
        });

    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Tecnico', e);
    }
    return isComplete;
}

function _eliminar_producto_proveedor(producto, proveedor) {
    var Prod_Ubic = null;

    try {
        Prod_Ubic = new Object();
        Prod_Ubic.Rol_ID = parseInt(producto);
        Prod_Ubic.Sucursal_ID = parseInt(proveedor);
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Prod_Ubic) });

        $.ajax({
            type: 'POST',
            url: 'controller/Roles_Sucursales_Controller.asmx/Eliminar',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        _search_rol_sucursal();
                    } else if (Resultado.Estatus == 'error') {
                        _mostrar_mensaje_rol_sucursal(Resultado.Titulo, Resultado.Mensaje);
                    }
                } else { _mostrar_mensaje_rol_sucursal(Resultado.Titulo, Resultado.Mensaje); }
            }
        });

    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe Técnico', e);
    }
}

function _modal_roles_sucursales() {
    var tags = '';
    try {
        tags += '<div class="modal fade" id="_modal_datos_rol_sucursal" name="_modal_datos_rol_sucursal" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">';
        tags += '<div class="modal-dialog modal-lg">';
        tags += '<div class="modal-content">';

        tags += '<div class="modal-header">';
        tags += '<button type="button" class="close cancelar" data-dismiss="modal" aria-label="Close" onclick="_set_close_modal_roles_sucursales(true);"><i class="fa fa-times text-red"></i></button>';
        tags += '<h3 class="modal-title" id="myModalLabel">';
        tags += '<label id="lbl_titulo_rol_sucursal"></label>';
        tags += '</h3>';
        tags += '</div>';

        tags += '<div class="modal-body">';

        tags += '<div class="row" id="div_roles">' +
                '   <div class="col-sm-6">' +
                '       <label class="fuente_lbl_controles">Roles</label>' +
                '       <input type="hidden" id="txt_roles"/>' +
                '       <select id="cmb_roles" name="cmb_roles" style="width:100%"  data-parsley-required="true"></select> ' +
                '   </div>' +
                '</div>' +

                '<div class="row" id="div_sucursal">' +
                '   <div class="col-sm-2 text-bold" style="padding-top: 5px;">' +
                '       Sucursal' +
                '   </div>' +
                '   <div class="col-sm-10">' +
                '       <select id="cmb_sucursal" name="cmb_sucursal" style="width:100%" data-parsley-required="true"></select> ' +
                '       <input type="hidden" id="txt_sucursal"/>' +
                '   </div>' +
                '</div>';
        tags += '<hr />';

        tags += '<div class="row">';
        tags += '   <div class="col-sm-12">';
        tags += '       <div id="toolbarpr" style="margin-left: 5px;">';
        tags += '           <div class="btn-group" role="group" style="margin-left: 5px;">';
        tags += '               <button type="submit" class="btn btn-blue btn-icon btn-icon-standalone" id="btn_guardar_datos_roles_sucursales" title="Agregar">' +
                '                   <i class="fa fa-plus"></i>' +
                '                   <span>Agregar</span>' +
                '               </button>';
        tags += '           </div>';
        tags += '       </div>';
        tags += '       <table id="tbl_roles_sucursales" data-toolbar="#toolbarpr" class="table table-responsive"></table>';
        tags += '   </div>';
        tags += '</div>';

        tags += '</div>';

        tags += '<div class="modal-footer">';

        tags += '<div class="row">';
        tags += '   <div class="col-md-7">';
        tags += '       <div id="sumary_error_rol_sucursal" class="alert alert-danger text-left" style="width: 277.78px !important; display:none;">';
        tags += '           <label id="lbl_msg_error_rol_sucursal"/>';
        tags += '       </div>';
        tags += '   </div>';
        tags += '   <div class="col-md-5">';
        tags += '   </div>';
        tags += '</div>';

        tags += '</div>';

        tags += '</div>';
        tags += '</div>';
        tags += '</div>';

        $(tags).appendTo('body');

        $('#btn_guardar_datos_roles_sucursales').bind('click', function (e) {
            e.preventDefault();
            var _output = _validation_producto_ubicacion('alta');
            if (_output.Estatus) {
                if (_alta_rol_sucursal()) {
                    _estado_inicial_rol_sucursal();
                    _set_close_modal_roles_sucursales(false);
                    //jQuery('#_modal_datos_rol_sucursal').modal('hide');
                }
            } else {
                _set_close_modal_roles_sucursales(false);
            }

        });
    } catch (e) {
        _mostrar_mensaje_rol_sucursal('Informe técnico', e);
    }
}

function _set_close_modal_roles_sucursales(state) {
    closeModalProdUbicacion = state;
}

function _eliminar_rol_sucursal(prod_prov) {
    var row = $(prod_prov).data('prod-ubic');

    bootbox.confirm({
        title: 'Eliminar Registro',
        message: 'Esta seguro de eliminar el registro seleccionado?',
        callback: function (result) {
            if (result) {
                _eliminar_producto_proveedor(row.Rol_ID, row.Sucursal_ID);
            }
            _estado_inicial_rol_sucursal();
        }
    });
}

function _set_location_toolbar_prod_prov() {
    $('#toolbarpr').parent().removeClass("pull-left");
    $('#toolbarpr').parent().addClass("pull-right");
}

function _set_title_modal_roles_sucursales(Titulo) {
    $("#lbl_titulo_rol_sucursal").html(Titulo);
}

function _search_rol_sucursal() {
    var filtros = null;
    try {

        filtros = new Object();
        filtros.Rol_ID = $('#txt_roles').val() === '' ? 0 : parseInt($('#txt_roles').val());
        filtros.Sucursal_ID = $('#txt_sucursal').val() === '' ? 0 : parseInt($('#txt_sucursal').val());

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

        jQuery.ajax({
            type: 'POST',
            url: 'controller/Roles_Sucursales_Controller.asmx/Consultar_Roles_Sucursales',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    $('#tbl_roles_sucursales').bootstrapTable('load', JSON.parse(datos.d));
                    //hide_loading_bar();
                }
            }
        });

    } catch (e) {

    }
}

function _validation_producto_ubicacion(opcion) {
    var _output = new Object();

    _output.Estatus = true;
    _output.Mensaje = '';

    if ($('#txt_roles').val() != '') {
        if (!$('#cmb_sucursal').parsley().isValid()) {
            _add_class_error('#cmb_sucursal');
            _output.Estatus = false;
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La sucursal es un dato requerido.<br />';
        } else {
            var _Resultado = _validate_fields_tipo_ubicacion($('#txt_roles').val(), $('#cmb_sucursal').val());
            if (_Resultado.Estatus === 'error') {
                _output.Estatus = false;
                _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + _Resultado.Mensaje + '<br />';
            }
        }
    }
    if ($('#txt_sucursal').val() != '') {
        if (!$('#cmb_roles').parsley().isValid()) {
            _add_class_error('#cmb_roles');
            _output.Estatus = false;
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El rol es un dato requerido.<br />';
        } else {
            var _Resultado = _validate_fields_tipo_ubicacion($('#cmb_roles').val(), $('#txt_sucursal').val());
            if (_Resultado.Estatus === 'error') {
                _output.Estatus = false;
                _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + _Resultado.Mensaje + '<br />';
            }
        }
    }

    if (!_output.Estatus)
        _validation_rol_sucursal_sumary(_output);
    else
        _validation_rol_sucursal_sumary(null);

    return _output;
}

function _add_class_error(selector) {
    $(selector).addClass('alert-danger');
}

function _remove_class_error(selector) {
    $(selector).removeClass('alert-danger');
}

function _clear_all_class_error() {
    $('#_modal_datos_rol_sucursal input[type=text]').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });
    $('#_modal_datos_rol_sucursal select').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });
}

function _validation_rol_sucursal_sumary(validation) {
    var header_message = '<i class="fa fa-exclamation-triangle fa-2x"></i><span>Observaciones</span><br />';

    if (validation == null) {
        $('#lbl_msg_error_rol_sucursal').html('');
        $('#sumary_error_rol_sucursal').css('display', 'none');
    } else {
        $('#lbl_msg_error_rol_sucursal').html(header_message + validation.Mensaje);
        $('#sumary_error_rol_sucursal').css('display', 'block');
    }
}

function _launch_modal_roles_sucursales(title_window) {
    _set_title_modal_roles_sucursales(title_window);
    jQuery('#_modal_datos_rol_sucursal').modal('show', { backdrop: 'static', keyboard: false });
    //$('#txt_grupo').focus();
}

function _enter_keypress_modal_roles_sucursales() {
    var $btn = $('[id$=btn_guardar_datos_roles_sucursales]').get(0);
    $(window).keypress(function (e) {
        if (e.which === 13 && e.target.type !== 'textarea') {
            if ($btn != undefined && $btn != null) {
                if ($btn.type === 'submit')
                    $btn.click();
                else
                    eval($btn.href);
                return false;
            }
        }
    });
}

function _validate_fields_tipo_ubicacion(tipo_producto_id, ubicacion_id) {
    var Unidad = null;
    var Resultado = null;

    try {
        Unidad = new Object();
        Unidad.Rol_ID = parseInt(tipo_producto_id);
        Unidad.Sucursal_ID = parseInt(ubicacion_id);

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Unidad) });

        $.ajax({
            type: 'POST',
            url: 'controller/Roles_Sucursales_Controller.asmx/Consultar_Roles_Sucursales_Por_ID',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                if (result !== null)
                    Resultado = JSON.parse(result.d);
            }
        });
    } catch (e) {
        Resultado = new Object();
        Resultado.Estatus = 'error';
        Resultado.Mensaje = 'No fue posible realizar la validación del ' + field + ' en la base de datos.';
        _mostrar_mensaje_rol_sucursal('Informe Técnico', e);
    }
    return Resultado;
}

function _consultar_sucursal() {
    try {
        $('#cmb_sucursal').select2({
            language: "es",
            theme: "classic",
            placeholder: 'Selecciona una sucursal',
            allowClear: true,
            ajax: {
                url: 'controller/Roles_Sucursales_Controller.asmx/Consultar_Sucursales',
                cache: "true",
                dataType: 'json',
                type: "POST",
                delay: 250,
                cache: true,
                params: {
                    contentType: 'application/json; charset=utf-8'
                },
                quietMillis: 100,
                results: function (data) {
                    return { results: data };
                },
                data: function (params) {
                    return {
                        q: params.term,
                        page: params.page
                    };
                },
                processResults: function (data, page) {
                    return {
                        results: data
                    };
                },
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _consultar_roles() {
    try {
        $('#cmb_roles').select2({
            language: "es",
            theme: "classic",
            placeholder: 'Selecciona el rol',
            allowClear: true,
            ajax: {
                url: 'controller/Roles_Sucursales_Controller.asmx/Consultar_Roles',
                cache: "true",
                dataType: 'json',
                type: "POST",
                delay: 250,
                cache: true,
                params: {
                    contentType: 'application/json; charset=utf-8'
                },
                quietMillis: 100,
                results: function (data) {
                    return { results: data };
                },
                data: function (params) {
                    return {
                        q: params.term,
                        page: params.page
                    };
                },
                processResults: function (data, page) {
                    return {
                        results: data
                    };
                },
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}