var closeModal = true;
var estatusActivo = '';
var Valor_Seleccionado = "";

$(document).on('ready', function () {
    _inicializar_pagina();
    $(function () {
        $(".icon-picker").iconPicker();
    });
    $('#cmb_modulos').on('change', _cargar_menus_parent);
    $('#visible').prop('checked', true);

});

function _inicializar_pagina() {
    try {
        _cargar_estatus_busqueda();
        _cargar_estatus_input_modal();
        _cargar_modulos();
        _habilitar_controles('Inicio');
        _limpiar_controles();
        _cargar_tabla();
        _search();
        _modal();
        _cargar_tipo_menu();
        //_eventos_textbox();
        _eventos();
        _enter_keypress_modal();
        _set_location_toolbar();
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _estado_inicial() {
    try {
        _habilitar_controles('Inicio');
        _limpiar_controles();
        $('#tbl_menus').bootstrapTable('refresh', 'controllers/Menu_Controller.asmx/Consultar_Menus_Por_Filtros');
        _set_location_toolbar();
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _habilitar_controles(opcion) {
    var Estatus = false;

    switch (opcion) {
        case "Nuevo":
            Estatus = true;
            $('#cmb_estatus').attr({ disabled: !Estatus });
            $('#cmb_modulos').attr({ disabled: !Estatus });
            $('#cmb_menus_parent').attr({ disabled: !Estatus });
            break;
        case "Modificar":
            Estatus = true;
            $('#cmb_estatus').attr({ disabled: !Estatus });
            $('#cmb_modulos').attr({ disabled: !Estatus });
            $('#cmb_menus_parent').attr({ disabled: !Estatus });
            break;
        case "Inicio":
            break;
    }

    $('#txt_orden').attr({ disabled: !Estatus });
    $('#txt_nombre').attr({ disabled: !Estatus });
    $('#visible').attr({ disabled: !Estatus });
    $('#txt_url_link').attr({ disabled: !Estatus });
    $('#txt_icono').attr({ disabled: !Estatus });
    $('#icono').attr({ disabled: !Estatus });

}

function _limpiar_controles() {
    $('input[type=text]').each(function () { $(this).val(''); });
    $('select').each(function () { $(this).val(estatusActivo); });
    $('textarea').each(function () { $(this).val(''); });
    $('#txt_menu_id').val('');
    _validation_sumary(null);
    _clear_all_class_error();
}

function _eventos() {
    try {
        $('#modal_datos').on('hidden.bs.modal', function () {
            if (!closeModal)
                $(this).modal('show');
        });
        $('#btn_nuevo').click(function (e) {
            _limpiar_controles();
            _habilitar_controles('Nuevo');
            _launch_modal('<i class="fa fa-floppy-o" style="font-size: 25px;"></i>&nbsp;&nbsp;Alta de registro');
        });
        $('.cancelar').each(function (index, element) {
            $(this).on('click', function (e) {
                e.preventDefault();
                _estado_inicial();
            });
        });
        $('#btn_salir').on('click', function (e) { e.preventDefault(); window.location.href = '../Paginas_Generales/Frm_Apl_Principal.aspx'; });
        $('#btn_busqueda').on('click', function (e) {
            e.preventDefault();
            _search();
        });
        $('#modal_datos input[type=text]').each(function (index, element) {
            $(this).on('focus', function () {
                _remove_class_error('#' + $(this).attr('id'));
            });
        });
        $('#modal_datos select').each(function (index, element) {
            $(this).on('focus', function () {
                _remove_class_error('#' + $(this).attr('id'));
            });
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _eventos_textbox() {
    $('#txt_nombre').on('blur', function () {
        $(this).val($(this).val().match(/^[0-9a-zA-Z\u0020]+$/) ? $(this).val() : $(this).val().replace(/\W+/g, ''));
    });

    $('#txt_orden').on('blur', function () {
        $(this).val($(this).val().match(/^[0-9]+$/) ? $(this).val() : '');
    });
}

function _mostrar_mensaje(Titulo, Mensaje) {
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

function _cargar_tabla() {

    try {
        $('#tbl_menus').bootstrapTable('destroy');
        $('#tbl_menus').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDysplay: false,
            search: true,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            clickToSelect: false,
            toggle: 'tbl_menus',
            detailView: true,
            detailFormatter: "detailFormatter",
            columns: [
                { field: 'Menu_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: false, visible: false },
                { field: 'Modulo_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: false, visible: false },
                { field: 'Estatus_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: false, visible: false },
                { field: 'Parent_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: false, visible: false },
                { field: 'Nombre_Mostrar', title: 'Nombre', width: 200, align: 'left', valign: 'bottom', sortable: false },
                { field: 'URL_LINK', title: 'URL', width: 300, align: 'left', valign: 'bottom', sortable: false},
                { field: 'Orden', title: 'Orden', width: 100, align: 'center', valign: 'bottom', halign: 'left', sortable: false},
                {
                    field: 'Icono', title: 'Ícono', width: 100, align: 'center', valign: 'bottom', halign: 'left', sortable: false, clickToSelect: false, formatter: function (value) {
                        return '<div><i class="' + value + '"></i></div>';
                    }
                },
                { field: 'Estatus', title: 'Estatus', width: 100, align: 'center', valign: 'bottom', halign: 'left', sortable: false},
                {
                    field: 'Menu_ID',
                    title: '',
                    align: 'center',
                    valign: 'bottom',
                    width: 60,
                    formatter: function (value, row) {
                        return '<div> ' +
                            '<a class="remove ml10 edit" id="' + row.Menu_ID + '" href="javascript:void(0)" data-menu=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i></button>' +
                            '&nbsp;&nbsp;<a class="remove ml10 delete" id="' + row.Menu_ID + '" href="javascript:void(0)" data-menu=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></a>' +
                            '</div>';
                    }
                }
            ]
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _alta_menu() {
    var Menus = null;
    var isComplete = false;

    try {

        Menus = new Object();
        Menus.Estatus_ID = $('#cmb_estatus').val();
        Menus.Modulo_ID = $('#cmb_modulos').val();
        Menus.Parent_ID = ($('#cmb_menus_parent').val() != null && $('#cmb_menus_parent') != undefined && $('#cmb_menus_parent') != '') ? $('#cmb_menus_parent').val() : null;
        Menus.Nombre_Mostrar = $('#txt_nombre').val();
        Menus.URL_LINK = ($('#txt_url_link').val() != null && $('#txt_url_link') != undefined && $('#txt_url_link') != '') ? $('#txt_url_link').val() : null;
        Menus.Orden = ($('#txt_orden').val() != null && $('#txt_orden') != undefined && $('#txt_orden') != '') ? $('#txt_orden').val() : null;
        Menus.Icono = ($('#txt_icono').val() != null && $('#txt_icono') != undefined && $('#txt_icono') != '') ? $('#txt_icono').val() : null;
        Menus.Visible = ($('#visible').is(":checked")) ? true : false;
        Menus.Tipo_Menu = $('#cmb_tipo_menu').val();
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Menus) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/Alta',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        _search();
                        isComplete = true;
                    } else if (Resultado.Estatus == 'error') {
                        _validation_sumary(Resultado);
                    }
                } else {
                    _validation_sumary(Resultado);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;
}

function _modificar_menu() {
    var Menus = null;
    var isComplete = false;

    try {
        Menus = new Object();
        Menus.Menu_ID = $('#txt_menu_id').val();
        Menus.Estatus_ID = $('#cmb_estatus').val();
        Menus.Modulo_ID = $('#cmb_modulos').val();
        Menus.Parent_ID = ($('#cmb_menus_parent').val() != null && $('#cmb_menus_parent') != undefined && $('#cmb_menus_parent') != '') ? $('#cmb_menus_parent').val() : null;
        Menus.Nombre_Mostrar = $('#txt_nombre').val();
        Menus.URL_LINK = ($('#txt_url_link').val() != null && $('#txt_url_link') != undefined && $('#txt_url_link') != '') ? $('#txt_url_link').val() : null;
        Menus.Orden = ($('#txt_orden').val() != null && $('#txt_orden') != undefined && $('#txt_orden') != '') ? $('#txt_orden').val() : null;
        Menus.Icono = ($('#txt_icono').val() != null && $('#txt_icono') != undefined && $('#txt_icono') != '') ? $('#txt_icono').val() : null;
        Menus.Visible = ($('#visible').is(":checked")) ? true : false;
        Menus.Tipo_Menu = $('#cmb_tipo_menu').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Menus) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/Actualizar',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        _search();
                        isComplete = true;
                    } else if (Resultado.Estatus == 'error') {
                        _validation_sumary(Resultado);
                    }
                } else {
                    _validation_sumary(Resultado);
                }
            }
        });

    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;
}

function _eliminar_menu(menu_id) {
    var Menu = null;

    try {
        Menu = new Object();
        Menu.Menu_ID = menu_id;

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Menu) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/Eliminar',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        _search();
                    } else if (Resultado.Estatus == 'error') {
                        _mostrar_mensaje(Resultado.Titulo, Resultado.Mensaje);
                    }
                } else { _mostrar_mensaje(Resultado.Titulo, Resultado.Mensaje); }
            }
        });

    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _modal() {
    var tags = '';
    try {
        tags += '<div class="modal fade" id="modal_datos" name="modal_datos" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">';
        tags += '<div class="modal-dialog">';
        tags += '<div class="modal-content">';

        tags += '<div class="modal-header">';
        tags += '<button type="button" class="close cancelar" data-dismiss="modal" aria-label="Close" onclick="_set_close_modal(true);"><i class="fa fa-times"></i></button>';
        tags += '<h4 class="modal-title" id="myModalLabel">';
        tags += '<label id="lbl_titulo"></label>';
        tags += '</h4>';
        tags += '</div>';

        tags += '<div class="modal-body">';

        tags +=
            '  <div class="row">' +
            '     <div class="col-md-12">' +
            '            &nbsp;&nbsp;&nbsp;<input type="checkbox" name="visible" id="visible" class="cbr cbr-primary">&nbsp;<span style="font-family: \'Roboto Regular\'">¿El menú sera visible?</span>' +
            '     </div>' +
            '  </div>' +
            '  <div class="row">' +
            '     <div class="col-md-12">' +
            '            <label class="fuente_lbl_controles">(*) Nombre</label>' +
            '          <input type="text" id="txt_nombre" name="txt_nombre" class="form-control input-sm" disabled="disabled" placeholder="(*) Nombre del menú" data-parsley-required="true" maxlength="100" required /> ' +
            '       </div>' +
            '  </div>' +

            '<div class="row">' +
            ' <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">(*) Módulo</label>' +
            '           <select id="cmb_modulos" name="cmb_modulos" class="form-control input-sm" disabled="disabled" data-parsley-required="true" required style="border-radius:inherit"></select> ' +
            '           <input type="hidden" id="txt_menu_id"/>' +
            '       </div>' +
            '     <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">(*) Estatus</label>' +
            '           <select id="cmb_estatus" name="cmb_estatus" class="form-control input-sm" disabled="disabled" data-parsley-required="true" required style="border-radius:inherit"></select> ' +
            '       </div>' +
            '</div>' +

            '    <div class="row">' +
            '     <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">Menú padre</label>' +
            '           <select id="cmb_menus_parent" name="cmb_menus_parent" class="form-control input-sm" disabled="disabled" data-parsley-required="true" style="border-radius:inherit"></select> ' +
            '       </div>' +
            '     <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">(*) Orden</label>' +
            '          <input type="text" id="txt_orden" name="txt_orden" class="form-control input-sm" disabled="disabled" placeholder="(*) Orden" data-parsley-required="true" maxlength="100" required /> ' +
            '       </div>' +
            '    </div>' +
            '    <div class="row">' +
            '     <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">(*) Tipo Menu</label>' +
            '           <select id="cmb_tipo_menu" class="form-control input-sm" data-parsley-required="true" style="border-radius:inherit"></select> ' +
            '       </div>' +
            '    </div>' +
            '    <div class="row">' +
            '     <div class="col-md-12">' +
            '            <label class="fuente_lbl_controles">URL</label>' +
            '          <input type="text" id="txt_url_link" name="txt_url_link" class="form-control input-sm" disabled="disabled" placeholder="(*) URL Link" data-parsley-required="true" maxlength="100" required /> ' +
            '       </div>' +
            '    </div>' +
            '    <div class="row">' +
            '     <div class="col-md-12">' +
            '            <label class="fuente_lbl_controles">Ícono</label>' +
            '           <input type="text" name="txt_icono" id="txt_icono" class="icon-picker form-control-icono" disabled="true" readonly />' +
            '       </div>' +
            '    </div>';

        tags += '</div>';

        tags += '<div class="modal-footer">';
        tags += '<div class="row">';

        tags += '<div class="col-md-7">';
        tags += '<div id="sumary_error" class="alert alert-danger text-left" style="width: 277.78px !important; display:none;">';
        tags += '<label id="lbl_msg_error"/>';
        tags += '</div>';
        tags += '</div>';

        tags += '<div class="col-md-5">';
        tags += '<div class="form-inline">';
        tags += '<button type="submit" class="btn btn-info btn-icon btn-icon-standalone btn-xs" id="btn_guardar_datos" title="Guardar"><i class="fa fa-check"></i><span>Aceptar</span></button>';
        tags += '<button type="button" class="btn btn-danger btn-icon btn-icon-standalone btn-xs cancelar" data-dismiss="modal" id="btn_cancelar" aria-label="Close" onclick="_set_close_modal(true);" title="Cancelar operaci&oacute;n"><i class="fa fa-remove"></i><span>Cancelar</span></button>';
        tags += '</div>';
        tags += '</div>';

        tags += '</div>';
        tags += '</div>';

        tags += '</div>';
        tags += '</div>';
        tags += '</div>';

        $(tags).appendTo('body');

        $('#btn_guardar_datos').bind('click', function (e) {
            e.preventDefault();

            if ($('#txt_menu_id').val() != null && $('#txt_menu_id').val() != undefined && $('#txt_menu_id').val() != '') {
                var _output = _validation('editar');
                if (_output.Estatus) {
                    if (_modificar_menu()) {
                        _estado_inicial();
                        _set_close_modal(true);
                        jQuery('#modal_datos').modal('hide');
                    }
                } else {
                    _set_close_modal(false);
                }
            } else {
                var _output = _validation('alta');
                if (_output.Estatus) {
                    if (_alta_menu()) {
                        _estado_inicial();
                        _set_close_modal(true);
                        jQuery('#modal_datos').modal('hide');
                    }
                } else {
                    _set_close_modal(false);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}

function _set_close_modal(state) {
    closeModal = state;
}

function btn_editar_click(menu) {
    var row = $(menu).data('menu');

    $('#txt_menu_id').val(row.Menu_ID);
    $('#cmb_estatus').val(row.Estatus_ID);
    $('#cmb_tipo_menu').val(row.Tipo_Menu);
    $('#cmb_modulos').val(row.Modulo_ID);
    $("#cmb_modulos").trigger("change");
    $('#txt_nombre').val(row.Nombre_Mostrar);
    $('#txt_url_link').val((row.URL_LINK === null) ? null : row.URL_LINK);
    $('#txt_orden').val(row.Orden);
    $('#txt_icono').val((row.Icono === null) ? null : row.Icono);
    $('#visible').prop('checked', row.Visible);
    
    Valor_Seleccionado = (($.trim(row.Parent_ID) === null || $.trim(row.Parent_ID) === "") ? "" : $.trim(row.Parent_ID));
    _habilitar_controles('Modificar');
    _launch_modal('<i class="glyphicon glyphicon-edit" style="font-size: 25px;"></i>&nbsp;&nbsp;Actualizar registro');
}

function btn_eliminar_click(menu) {
    var row = $(menu).data('menu');

    bootbox.confirm({
        title: 'Eliminar Registro',
        message: '¿Está seguro de eliminar el registro seleccionado?',
        callback: function (result) {
            if (result) {
                _eliminar_menu(row.Menu_ID);
            }
            _estado_inicial();
        }
    });
}

function _set_location_toolbar() {
    $('#toolbar').parent().removeClass("pull-left");
    $('#toolbar').parent().addClass("pull-right");
}

function _set_title_modal(Titulo) {
    $("#lbl_titulo").html(Titulo);
}

function _search() {
    var filtros = null;
    try {
        show_loading_bar({
            pct: 78,
            wait: .5,
            delay: .5,
            finish: function (pct) {
                var $_nombre = $('#txt_busqueda_por_nombre').val();
                var $_estatus = $('#cmb_estatusfiltro :selected').val();

                filtros = new Object();
                filtros.Nombre_Mostrar = ($_nombre === undefined || $_nombre === '') ? '' : $('#txt_busqueda_por_nombre').val();
                filtros.Estatus_ID = $('#cmb_estatusfiltro :selected').val() == '' ? 0 : parseInt($('#cmb_estatusfiltro').val());

                var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

                if ((filtros.Nombre_Mostrar === '' || filtros.Nombre_Mostrar == null) &&
                    (filtros.Estatus_ID === 0 || filtros.Estatus_ID == null)) {
                    jQuery.ajax({
                        type: 'POST',
                        url: 'controllers/Menu_Controller.asmx/Consultar_Menus_Padres',
                        data: $data,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (datos) {
                            if (datos !== null) {
                                $('#tbl_menus').bootstrapTable('load', JSON.parse(datos.d));
                                hide_loading_bar();
                            }
                        }
                    });
                }
                else {
                    jQuery.ajax({
                        type: 'POST',
                        url: 'controllers/Menu_Controller.asmx/Consultar_Menus_Por_Filtros',
                        data: $data,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        async: true,
                        cache: false,
                        success: function (datos) {
                            if (datos !== null) {
                                $('#tbl_menus').bootstrapTable('load', JSON.parse(datos.d));
                                hide_loading_bar();
                            }
                        }
                    });
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _validation(opcion) {
    var _output = new Object();

    _output.Estatus = true;
    _output.Mensaje = '';

    if (!$('#cmb_modulos').parsley().isValid()) {
        _add_class_error('#cmb_modulos');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El modulo es requerido.<br />';
    }
    if (!$('#cmb_estatus').parsley().isValid()) {
        _add_class_error('#cmb_estatus');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El estatus es un dato requerido.<br />';
    }
    if (!$('#cmb_tipo_menu').parsley().isValid()) {
        _add_class_error('#cmb_tipo_menu');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El Tipo Menu es un dato requerido.<br />';
    }
    if (!$('#txt_orden').parsley().isValid()) {
        _add_class_error('#txt_orden');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El orden es un dato requerido.<br />';
    }
    if (!$('#txt_nombre').parsley().isValid()) {
        _add_class_error('#txt_nombre');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El nombre es un dato requerido.<br />';
    }

    var _Resultado = (opcion === 'alta') ?
            _validate_fields($('#txt_nombre').val(), $('#cmb_modulos').val(), null) :
            _validate_fields($('#txt_nombre').val(), $('#cmb_modulos').val(), $('#txt_menu_id').val());

    if (_Resultado.Estatus === 'error') {
        _add_class_error('#txt_nombre');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + _Resultado.Mensaje + '<br />';
    }

    if (!_output.Estatus) _validation_sumary(_output);

    return _output;
}

function _add_class_error(selector) {
    $(selector).addClass('alert-danger');
}

function _remove_class_error(selector) {
    $(selector).removeClass('alert-danger');
}

function _clear_all_class_error() {
    $('#modal_datos input[type=text]').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });
    $('#modal_datos select').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });
}

function _validation_sumary(validation) {
    var header_message = '<i class="fa fa-exclamation-triangle fa-2x"></i><span>Observaciones</span><br />';

    if (validation == null) {
        $('#lbl_msg_error').html('');
        $('#sumary_error').css('display', 'none');
    } else {
        $('#lbl_msg_error').html(header_message + validation.Mensaje);
        $('#sumary_error').css('display', 'block');
    }
}

function _enter_keypress_modal() {
    var $btn = $('[id$=btn_guardar_datos]').get(0);
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

function _launch_modal(title_window) {
    _set_title_modal(title_window);
    jQuery('#modal_datos').modal('show', { backdrop: 'static', keyboard: false });
    $('#txt_nombre').focus();
}

function _validate_fields(name, module, id) {
    var Menus = null;
    var Resultado = null;

    try {
        Menus = new Object();
        Menus.Menu_ID = id == null ? 0 : id;
        Menus.Nombre_Mostrar = name;
        Menus.Modulo_ID = module;

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Menus) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/Consultar_Existencia',
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
        Resultado.Mensaje = 'No fue posible realizar la validación del ' + name + ' en la base de datos.';
        _mostrar_mensaje('Informe Técnico', e);
    }
    return Resultado;
}

function _cargar_estatus_busqueda() {
    var filtros = null;
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/ConsultarFiltroEstatus',
            //data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    var datos_combo = $.parseJSON(datos.d);
                    var select = $('#cmb_estatusfiltro');
                    $('option', select).remove();
                    var options = '<option value=""><-TODOS-></option>';
                    for (var Indice_estatus = 0; Indice_estatus < datos_combo.length; Indice_estatus++) {
                        options += '<option value="' + datos_combo[Indice_estatus].Estatus_ID + '">' + datos_combo[Indice_estatus].Estatus + '</option>';
                    }
                    select.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function detailFormatter(index, row) {
    var html = [];
    var $data;

    var filtros = new Object();
    filtros.Menu_ID = row.Menu_ID;

    $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

    $.ajax({
        type: 'POST',
        url: 'controllers/Menu_Controller.asmx/Consultar_Menus_Hijos',
        data: $data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            Datos = JSON.parse(result.d);
            if (Datos != null) {
                var largo = 0;
                largo = $(window).width() / 3;
                largo = largo * 2.5;
                //creamos el encabezado
                if (Datos.length == 0) {
                    html.push('<div id="sub" class="alert alert-info" role="alert"><i class="fa fa-info-circle" style="color:black;">&nbsp;No contiene submenus</i></div>');
                } else {
                    html.push('<center><table class="table table-responsive" style="width: ' + largo + 'px;  ">');
                    html.push('<tr>');
                    html.push('<td class="hidden" style="border: solid 1px #dddddd; background-color:#607D8B;"><b><font color="white">ID</font></b></td>');
                    html.push('<td style="border: solid 1px #dddddd; background-color:#607D8B;"><b><font color="white">Nombre</font></b></td>');
                    html.push('<td style="border: solid 1px #dddddd; background-color:#607D8B;"><b><font color="white">URL-LINK</font></b></td>');
                    html.push('<td style="border: solid 1px #dddddd; background-color:#607D8B;"><b><font color="white">Orden</font></b></td>');
                    html.push('<td style="border: solid 1px #dddddd; background-color:#607D8B;"><b><font color="white">Estatus</font></b></td>');
                    html.push('<td style="border: solid 1px #dddddd; background-color:#607D8B;"><b><font color="white">Icono</font></b></td>');
                    html.push('<td style="border: solid 1px #dddddd; background-color:#607D8B;"></td>');
                    html.push('</tr>');
                    $.each(Datos, function (key, value) {
                        html.push('<tr>');
                        html.push('<td class="hidden" style="text-align:left; border: solid 1px #dddddd;">' + value.Menu_ID + '</td>');
                        html.push('<td onclick="" style="text-align:left; border: solid 1px #dddddd;">' + value.Nombre_Mostrar + '</td>');
                        html.push('<td onclick="" style="text-align:left; border: solid 1px #dddddd;">' + value.URL_LINK + '</td>');
                        html.push('<td onclick="" style="text-align:left; border: solid 1px #dddddd;">' + value.Orden + '</td>');
                        html.push('<td onclick="" style="text-align:left; border: solid 1px #dddddd;">' + value.Estatus + '</td>');
                        html.push('<td onclick="" style="text-align:center; border: solid 1px #dddddd;"><i class="' + value.Icono + '"></i></td>');
                        html.push('<td onclick="" style="text-align:center; border: solid 1px #dddddd;">' + '<div> ' +
                            '<a class="remove ml10 edit" id="' + value.Menu_ID + '" href="javascript:void(0)" data-menu=\'' + JSON.stringify(value) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i></button>' +
                            '&nbsp;&nbsp;<a class="remove ml10 delete" id="' + value.Menu_ID + '" href="javascript:void(0)" data-menu=\'' + JSON.stringify(value) + '\' onclick="btn_eliminar_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></a>' +
                            '</div>' + '</td>');
                        html.push('</tr>');
                    });
                    html.push('</table></center>');
                }
            }
        }
    });
    return html.join('');
}

function _cargar_modulos() {
    var filtros = null;
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/Consultar_Modulos',
            //data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    var datos_combo = $.parseJSON(datos.d);
                    var select = $('#cmb_modulos');
                    $('option', select).remove();
                    var options = '<option value=""><-SELECCIONE></option>';
                    for (var Indice_estatus = 0; Indice_estatus < datos_combo.length; Indice_estatus++) {
                        options += '<option value="' + datos_combo[Indice_estatus].Modulo_ID + '">' + datos_combo[Indice_estatus].Nombre + '</option>';
                    }
                    select.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _cargar_estatus_input_modal() {
    var filtros = null;
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/ConsultarFiltroEstatus',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    var datos_combo = $.parseJSON(datos.d);
                    var select = $('#cmb_estatus');
                    $('option', select).remove();
                    var options = '<option value=""><-SELECCIONE-></option>';
                    for (var Indice_estatus = 0; Indice_estatus < datos_combo.length; Indice_estatus++) {
                        options += '<option value="' + datos_combo[Indice_estatus].Estatus_ID + '">' + datos_combo[Indice_estatus].Estatus + '</option>';
                    }
                    select.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _cargar_menus_parent() {
    var filtros = null;
    try {
        var $_modulo = $('#cmb_modulos').val();

        filtros = new Object();
        filtros.Modulo_ID = ($_modulo === undefined || $_modulo === '') ? null : $('#cmb_modulos').val();
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Menu_Controller.asmx/Consultar_Menus_Padres',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    var datos_combo = $.parseJSON(datos.d);
                    var select = $('#cmb_menus_parent');
                    $('option', select).remove();
                    var options = '<option value=""><-SELECCIONE-></option>';
                    for (var Indice_estatus = 0; Indice_estatus < datos_combo.length; Indice_estatus++) {
                        options += '<option value="' + datos_combo[Indice_estatus].Menu_ID + '">' + datos_combo[Indice_estatus].Nombre_Mostrar + '</option>';
                    }
                    select.append(options);
                    $('#cmb_menus_parent').val(Valor_Seleccionado);
                    Valor_Seleccionado = "";
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _cargar_tipo_menu() {
    try {
        var select = $('#cmb_tipo_menu');
        $('option', select).remove();
        var options = '<option value=""><-SELECCIONE-></option>';
        options += '<option value="WEB">WEB</option>';
        options += '<option value="Movil">Movil</option>';
        select.append(options);
        
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}