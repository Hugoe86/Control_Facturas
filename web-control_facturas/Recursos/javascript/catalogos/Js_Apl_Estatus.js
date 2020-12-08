var closeModal = true;//variable para cerrar el modal

$(document).on('ready', function () {
    _inicializar_pagina();
});
/* =============================================
--NOMBRE_FUNCIÓN: _inicializar_pagina
--DESCRIPCIÓN: Metodo para iniciar la pagina
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_pagina() {
    try {
        _habilitar_controles('Inicio');
        _limpiar_controles();
        _cargar_tabla();
        _search();
        _modal();
        _eventos_textbox();
        _eventos();
        _enter_keypress_modal();
        _set_location_toolbar();
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN: _estado_inicial
--DESCRIPCIÓN: Metodo para colocar el estado inicial de la pagina y controles
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _estado_inicial() {
    try {
        _habilitar_controles('Inicio');
        _limpiar_controles();
        $('#tbl_estatus').bootstrapTable('refresh', 'controllers/Estatus_Controller.asmx/Consultar_Estatus_Por_Filtros');
        _set_location_toolbar();
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN: _habilitar_controles
--DESCRIPCIÓN: Metodo para habilitar controles de acuerdo a una opcion de visualización
--PARÁMETROS: opcion.- se usa para saber la forma de visualizar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _habilitar_controles(opcion) {
    var Estatus = false;

    switch (opcion) {
        case "Nuevo":
            Estatus = true;
            break;
        case "Modificar":
            Estatus = true;
            break;
        case "Inicio":
            break;
    }
    $('#txt_nombre').attr({ disabled: !Estatus });
}
/* =============================================
--NOMBRE_FUNCIÓN: _limpiar_controles
--DESCRIPCIÓN: Metodo para limpiar los controles
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_controles() {
    $('input[type=text]').each(function () { $(this).val(''); });
    $('select').each(function () { $(this).val('Activo'); });
    $('textarea').each(function () { $(this).val(''); });
    $('#txt_estatus_id').val('');
    _validation_sumary(null);
    _clear_all_class_error();
}
/* =============================================
--NOMBRE_FUNCIÓN: _eventos
--DESCRIPCIÓN: Metodo para ejecutar los metodos de los controles existentes
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
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
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN: _eventos_textbox
--DESCRIPCIÓN: Metodo para ejecutar los metodos de los controles textbox existentes
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_textbox() {
    $('#txt_nombre').on('blur', function () {
        $(this).val($(this).val().match(/^[0-9a-zA-Z\u0020]+$/) ? $(this).val() : $(this).val().replace(/\W+/g, ''));
    });
}
/* =============================================
--NOMBRE_FUNCIÓN: _mostrar_mensaje
--DESCRIPCIÓN: Metodo para guardar nuevo registro
--PARÁMETROS: Titulo.- Titulo del mensaje a mostrar.
-- Mensaje.- Texto de mensaje a mostrar.
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
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
/* =============================================
--NOMBRE_FUNCIÓN: _cargar_tabla
--DESCRIPCIÓN: Metodo para cargar la tabla a mostrar
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cargar_tabla() {

    try {
        $('#tbl_estatus').bootstrapTable('destroy');
        $('#tbl_estatus').bootstrapTable({
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
            clickToSelect: true,
            columns: [
                { field: 'Estatus_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: true, visible: false },
                { field: 'Estatus', title: 'Nombre', width: 200, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                {
                    field: 'Estatus_ID',
                    title: '',
                    align: 'center',
                    valign: 'bottom',
                    width: 60,
                    clickToSelect: false,
                    formatter: function (value, row) {
                        return '<div> ' +
                            '<a class="remove ml10 edit" id="' + row.Estatus_ID + '" href="javascript:void(0)" data-estatus=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i></button>' +
                            '&nbsp;&nbsp;<a class="remove ml10 delete" id="' + row.Estatus_ID + '" href="javascript:void(0)" data-estatus=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></a>' +
                            '</div>';
                    }
                }
            ]
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN: _alta_estatus
--DESCRIPCIÓN: Metodo para dar de alta los datos
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _alta_estatus() {
    var Estatu = null;
    var isComplete = false;

    try {

        Estatu = new Object();
        Estatu.Estatus = $('#txt_nombre').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Estatu) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Estatus_Controller.asmx/Alta',
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
        _mostrar_mensaje('Informe Técnico', e);
    }
    return isComplete;
}
/* =============================================
--NOMBRE_FUNCIÓN: _modificar_estatus
--DESCRIPCIÓN: Metodo para modificar los datos
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _modificar_estatus() {
    var Estatu = null;
    var isComplete = false;

    try {
        Estatu = new Object();
        Estatu.Estatus_ID = parseInt($('#txt_estatus_id').val());
        Estatu.Estatus = $('#txt_nombre').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Estatu) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Estatus_Controller.asmx/Actualizar',
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
        _mostrar_mensaje('Informe Técnico', e);
    }
    return isComplete;
}
/* =============================================
--NOMBRE_FUNCIÓN: _eliminar_estatus
--DESCRIPCIÓN: Metodo para enviar los datos a eliminar
--PARÁMETROS: estatus_id.- el id del dato a eliminar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eliminar_estatus(estatus_id) {
    var Estatu = null;

    try {
        Estatu = new Object();
        Estatu.Estatus_ID = parseInt(estatus_id);

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Estatu) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Estatus_Controller.asmx/Eliminar',
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
/* =============================================
--NOMBRE_FUNCIÓN: _modal
--DESCRIPCIÓN: Metodo para crear pagina emergente
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
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
          '<div class="row">' +
          ' <div class="col-md-12">' +
          '            <label class="fuente_lbl_controles">(*) Nombre</label>' +
          '        <input type="text" id="txt_nombre" name="txt_nombre" class="form-control input-sm" disabled="disabled" placeholder="(*) Nombre" data-parsley-required="true" maxlength="10" required />' +
          '        <input type="hidden" id="txt_estatus_id"/>' +
          '   </div> ' +
          '</div>';
        

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
        tags += '<button type="submit" class="btn btn-info  btn-xs" id="btn_guardar_datos" title="Guardar"><i class="fa fa-check"></i><span>Aceptar</span></button>';
        tags += '<button type="button" class="btn btn-danger btn-xs cancelar" data-dismiss="modal" id="btn_cancelar" aria-label="Close" onclick="_set_close_modal(true);" title="Cancelar operaci&oacute;n"><i class="glyphicon glyphicon-remove"></i><span>Cancelar</span></button>';
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

            if ($('#txt_estatus_id').val() != null && $('#txt_estatus_id').val() != undefined && $('#txt_estatus_id').val() != '') {
                var _output = _validation('editar');
                if (_output.Estatus) {
                    if (_modificar_estatus()) {
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
                    if (_alta_estatus()) {
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
/* =============================================
--NOMBRE_FUNCIÓN: _set_close_modal
--DESCRIPCIÓN: Metodo para colocar el estado del modal
--PARÁMETROS: state.- estado del modal
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal(state) {
    closeModal = state;
}
/* =============================================
--NOMBRE_FUNCIÓN: btn_editar_click
--DESCRIPCIÓN: Metodo para cargar los controles con los datos que podran ser editados
--PARÁMETROS: estatus.- carga los datos a modificar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_editar_click(estatus) {
    var row = $(estatus).data('estatus');

    $('#txt_estatus_id').val(row.Estatus_ID);
    $('#txt_nombre').val(row.Estatus);

    _habilitar_controles('Modificar');
    _launch_modal('<i class="glyphicon glyphicon-edit" style="font-size: 25px;"></i>&nbsp;&nbsp;Actualizar registro');
}
/* =============================================
--NOMBRE_FUNCIÓN: btn_eliminar_click
--DESCRIPCIÓN: Metodo para eliminar los datos
--PARÁMETROS: estatus.- carga los datos a eliminar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_click(estatus) {
    var row = $(estatus).data('estatus');

    bootbox.confirm({
        title: 'Eliminar Registro',
        message: '¿Está seguro de eliminar el registro seleccionado?',
        callback: function (result) {
            if (result) {
                _eliminar_estatus(row.Estatus_ID);
            }
            _estado_inicial();
        }
    });
}
/* =============================================
--NOMBRE_FUNCIÓN: _set_location_toolbar
--DESCRIPCIÓN: Metodo para configurar la posición de los toolbars
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_location_toolbar() {
    $('#toolbar').parent().removeClass("pull-left");
    $('#toolbar').parent().addClass("pull-right");
}
/* =============================================
--NOMBRE_FUNCIÓN: _set_title_modal
--DESCRIPCIÓN: Metodo para colocar titulo al modal
--PARÁMETROS: Titulo.- nombre que se mostrara en el modal
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal(Titulo) {
    $("#lbl_titulo").html(Titulo);
}
/* =============================================
--NOMBRE_FUNCIÓN: _search
--DESCRIPCIÓN: Metodo para buscar los datos registrados
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _search() {
    var filtros = null;
    try {
        show_loading_bar({
            pct: 78,
            wait: .5,
            delay: .5,
            finish: function (pct) {
                filtros = new Object();
                filtros.Estatus = $('#txt_busqueda_por_nombre').val() === '' ? '' : $('#txt_busqueda_por_nombre').val();
                var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

                $.ajax({
                    type: 'POST',
                    url: 'controllers/Estatus_Controller.asmx/Consultar_Estatus_Por_Filtros',
                    data: $data,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    cache: false,
                    success: function (datos) {
                        if (datos !== null) {
                            $('#tbl_estatus').bootstrapTable('load', JSON.parse(datos.d));
                            hide_loading_bar();
                        }
                    }
                });
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN: _validation
--DESCRIPCIÓN: Metodo para validar los datos obligatorios
--PARÁMETROS: opcion.-  tipo de validación a realizar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validation(opcion) {
    var _output = new Object();

    _output.Estatus = true;
    _output.Mensaje = '';

    if (!$('#txt_nombre').parsley().isValid()) {
        _add_class_error('#txt_nombre');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El nombre es un dato requerido.<br />';
    } else {
        var _Resultado = (opcion === 'alta') ?
            _validate_fields($('#txt_nombre').val(), null, 'nombre') :
            _validate_fields($('#txt_nombre').val(), $('#txt_estatus_id').val(), 'nombre');

        if (_Resultado.Estatus === 'error') {
            _add_class_error('#txt_nombre');
            _output.Estatus = false;
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + _Resultado.Mensaje + '<br />';
        }
    }

    if (!_output.Estatus) _validation_sumary(_output);

    return _output;
}
/* =============================================
--NOMBRE_FUNCIÓN: _add_class_error
--DESCRIPCIÓN: Metodo para agregar error a mostrar
--PARÁMETROS: selector.- control con error
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _add_class_error(selector) {
    $(selector).addClass('alert-danger');
}
/* =============================================
--NOMBRE_FUNCIÓN: _remove_class_error
--DESCRIPCIÓN: Metodo para eliminar el error
--PARÁMETROS: selector.- control de donde sera removido la alerta
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _remove_class_error(selector) {
    $(selector).removeClass('alert-danger');
}
/* =============================================
--NOMBRE_FUNCIÓN: _clear_all_class_error
--DESCRIPCIÓN: Metodo para limpiar los errores
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _clear_all_class_error() {
    $('#modal_datos input[type=text]').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });
}
/* =============================================
--NOMBRE_FUNCIÓN: _validation_sumary
--DESCRIPCIÓN: Metodo para reunir todas las observaciones a mostrar
--PARÁMETROS: validation.- la observación a colocar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
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
/* =============================================
--NOMBRE_FUNCIÓN: _launch_modal
--DESCRIPCIÓN: Metodo para mostrar el modal
--PARÁMETROS: title_window.- nombre del modal
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal(title_window) {
    _set_title_modal(title_window);
    jQuery('#modal_datos').modal('show', { backdrop: 'static', keyboard: false });
    $('#txt_nombre').focus();
}
/* =============================================
--NOMBRE_FUNCIÓN: _enter_keypress_modal
--DESCRIPCIÓN: Metodo para detectar boton enter en modal
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
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
/* =============================================
--NOMBRE_FUNCIÓN: _validate_fields
--DESCRIPCIÓN: Metodo para validar no duplicidad
--PARÁMETROS: value.- nombre a validar
--id.- ID del dato
--field.- campo para validar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validate_fields(value, id, field) {
    var Estatu = null;
    var Resultado = null;

    try {
        Estatu = new Object();
        if (id !== null)
            Estatu.Estatus_ID = parseInt(id);

        switch (field) {
            case 'nombre':
                Estatu.Estatus = value;
                break;
            default:
        }

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Estatu) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Estatus_Controller.asmx/Consultar_Estatus_Por_Nombre',
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
        _mostrar_mensaje('Informe Técnico', e);
    }
    return Resultado;
}