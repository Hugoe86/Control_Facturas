var closeModal = true;//variable para cerrar el modal
var estatusActivo = '';//variable para tener el estatus activo
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
        _load_estatus();
        _load_areas();
        _cargar_tabla();
        _search();
        _modal();
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
        $('#tbl_puestos').bootstrapTable('refresh', 'controllers/Puestos_Controller.asmx/Consultar_Puestos_Por_Filtros');
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
            $('#cmb_estatus').attr({ disabled: Estatus });
            break;
        case "Modificar":
            Estatus = true;
            $('#cmb_estatus').attr({ disabled: !Estatus });
            break;
        case "Inicio":
            break;
    }
    $('#txt_nombre').attr({ disabled: !Estatus });
    $('#txt_descripcion').attr({ disabled: !Estatus });
    $('#cmb_areas').attr({ disabled: !Estatus });
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
    $('select').each(function () { $(this).val(estatusActivo); });
    $('#cmb_estatus').val('');
    $('#cmb_areas').val('');
    $('textarea').each(function () { $(this).val(''); });
    $('#txt_puesto_id').val('');
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
            $('#cmb_estatus').val(estatusActivo);
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
        $('#tbl_puestos').bootstrapTable('destroy');
        $('#tbl_puestos').bootstrapTable({
            cache: false,
            width: 900,
            height: 400,
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
                { field: 'Puesto_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: true, visible: false },
                { field: 'Nombre', title: 'Nombre', width: 200, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                { field: 'Estatus', title: 'Estatus', width: 100, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                { field: 'Area', title: 'Area', width: 200, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                { field: 'Descripcion', title: 'Descripcion', width: 200, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                {
                    field: 'Puesto_ID',
                    title: '',
                    align: 'center',
                    valign: 'bottom',
                    width: 60,
                    clickToSelect: false,
                    formatter: function (value, row) {
                        return '<div> ' +
                            '<a class="remove ml10 edit" id="' + row.Puesto_ID + '" href="javascript:void(0)" data-puestos=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i></button>' +
                            '&nbsp;&nbsp;<a class="remove ml10 delete" id="' + row.Puesto_ID + '" href="javascript:void(0)" data-puestos=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></a>' +
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
--NOMBRE_FUNCIÓN: _alta_puestos
--DESCRIPCIÓN: Metodo para dar de alta los datos
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _alta_puestos() {
    var Puestos = null;
    var isComplete = false;

    try {

        Puestos = new Object();
        Puestos.Nombre = $('#txt_nombre').val();
        Puestos.Descripcion = $('#txt_descripcion').val();
        Puestos.Estatus_ID = parseInt($('#cmb_estatus').val());
        Puestos.Area_ID = parseInt($('#cmb_areas').val());

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Puestos) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Alta',
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
--NOMBRE_FUNCIÓN: _modificar_puestos
--DESCRIPCIÓN: Metodo para modificar los datos
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _modificar_puestos() {
    var Puestos = null;
    var isComplete = false;

    try {
        Puestos = new Object();
        Puestos.Puesto_ID = parseInt($('#txt_puesto_id').val());
        Puestos.Nombre = $('#txt_nombre').val();
        Puestos.Descripcion = $('#txt_descripcion').val();
        Puestos.Estatus_ID = parseInt($('#cmb_estatus').val());
        Puestos.Area_ID = parseInt($('#cmb_areas').val());

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Puestos) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Actualizar',
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
--NOMBRE_FUNCIÓN: _eliminar_puestos
--DESCRIPCIÓN: Metodo para enviar los datos a eliminar
--PARÁMETROS: puesto_id.- el id del dato a eliminar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eliminar_puestos(puesto_id) {
    var Puestos = null;

    try {
        Puestos = new Object();
        Puestos.Puesto_ID = parseInt(puesto_id);

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Puestos) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Eliminar',
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

        tags += '<div class="row">' +
            ' <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">(*) Estatus</label>' +
            '        <select id="cmb_estatus" name="cmb_estatus" class="form-control input-sm" disabled="disabled" data-parsley-required="true" required ></select> ' +
            '        <input type="hidden" id="txt_puesto_id"/>' +
            '       </div> ' +

            ' <div class="col-md-6">' +
            '            <label class="fuente_lbl_controles">(*) Nombre</label>' +
            '        <input type="text" id="txt_nombre" name="txt_nombre" class="form-control input-sm" disabled="disabled" placeholder="(*) Nombre" data-parsley-required="true" maxlength="100" required /> ' +
            '    </div>' +
            '</div>' +

            '<div class="row">' +
            ' <div class="col-md-8">' +
            '           <label class="fuente_lbl_controles">(*) Area </label>' +
            '       <select id="cmb_areas" name="cmb_areas" class="form-control input-sm" disabled="disabled" data-parsley-required="true" required></select> ' +
            ' </div>' +
            '</div>' +

            '<div class="row">' +
            ' <div class="col-md-12">' +
            '            <label class="fuente_lbl_controles">Descripcion</label>' +
            '        <textarea  id="txt_descripcion" name="txt_descripcion" class="form-control input-sm" disabled="disabled" placeholder="Descripcion"  style="min-height: 50px !important;" data-parsley-required="false" maxlength="250"> </textarea>' +
            '    </div>' +
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

            if ($('#txt_puesto_id').val() != null && $('#txt_puesto_id').val() != undefined && $('#txt_puesto_id').val() != '') {
                var _output = _validation('editar');
                if (_output.Estatus) {
                    if (_modificar_puestos()) {
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
                    if (_alta_puestos()) {
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
--PARÁMETROS: equipos.- carga los datos a modificar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_editar_click(puestos) {
    var row = $(puestos).data('puestos');

    $('#txt_puesto_id').val(row.Puesto_ID);
    $('#txt_nombre').val(row.Nombre);
    $('#txt_descripcion').val(row.Descripcion);
    $('#cmb_estatus').val(row.Estatus_ID);
    $('#cmb_areas').val(row.Area_ID);
    _clear_all_class_error();
    _habilitar_controles('Modificar');
    _launch_modal('<i class="glyphicon glyphicon-edit" style="font-size: 25px;"></i>&nbsp;&nbsp;Actualizar registro');
}
/* =============================================
--NOMBRE_FUNCIÓN: btn_eliminar_click
--DESCRIPCIÓN: Metodo para eliminar los datos
--PARÁMETROS: equipos.- carga los datos a eliminar
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_click(puestos) {
    var row = $(puestos).data('puestos');

    bootbox.confirm({
        title: 'Eliminar Registro',
        message: '¿Está seguro de eliminar el registro seleccionado?',
        callback: function (result) {
            if (result) {
                _eliminar_puestos(row.Puesto_ID);
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
                filtros.Nombre = $('#txt_busqueda_por_nombre').val() === '' ? '' : $('#txt_busqueda_por_nombre').val();

                var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

                jQuery.ajax({
                    type: 'POST',
                    url: 'controllers/Puestos_Controller.asmx/Consultar_Puestos_Por_Filtros',
                    data: $data,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (datos) {
                        if (datos !== null) {
                            $('#tbl_puestos').bootstrapTable('load', JSON.parse(datos.d));
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
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp; El nombre es un dato requerido.<br />';
    } 
    //else {
    //    var _Resultado = (opcion === 'alta') ?
    //        _validate_fields($('#txt_nombre').val(), null, 'nombre') :
    //        _validate_fields($('#txt_nombre').val(), $('#txt_puesto_id').val(), 'nombre');

    //    if (_Resultado.Estatus === 'error') {
    //        _add_class_error('#txt_nombre');
    //        _output.Estatus = false;
    //        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + _Resultado.Mensaje + '<br />';
    //    }
    //}

    if (!$('#cmb_estatus').parsley().isValid()) {
        _add_class_error('#cmb_estatus');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El estatus es un dato requerido.<br />';
    } 
    //else {
    //    var _Resultado = (opcion === 'alta') ?
    //        _validate_fields($('#cmb_estatus').val(), null, 'estatus') :
    //        _validate_fields($('#cmb_estatus').val(), $('#txt_puesto_id').val(), 'estatus');

    //    if (_Resultado.Estatus === 'error') {
    //        _add_class_error('#cmb_estatus');
    //        _output.Estatus = false;
    //        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + _Resultado.Mensaje + '<br />';
    //    }
    //}

    if (!$('#cmb_areas').parsley().isValid()) {
        _add_class_error('#cmb_areas');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El &aacute;rea es un dato requerido.<br />';
    }
    
    if ($('#cmb_areas').parsley().isValid() && $('#txt_nombre').parsley().isValid()) {
        var _Resultado = (opcion === 'alta') ?
            _validate_fields_areas($('#txt_nombre').val(), null, 'area', $('#cmb_areas').val()) :
            _validate_fields_areas($('#txt_nombre').val(), $('#txt_puesto_id').val(), 'area', $('#cmb_areas').val());

        if (_Resultado.Estatus === 'error') {
            _add_class_error('#txt_nombre');
            _add_class_error('#cmb_areas');
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
    $('#modal_datos select').each(function () {
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
    var header_message = '<i class="fa fa-exclamation-triangle fa-2x"></i><span>Descripcion</span><br />';

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
    var Puestos = null;
    var Resultado = null;

    try {
        Puestos = new Object();
        if (id !== null)
            Puestos.Puesto_ID = parseInt(id);

        switch (field) {
            case 'nombre':
                Puestos.Nombre = value;
                break;
            case 'area':
                Puestos.Nombre = value;
                Puestos.Area_ID = parseInt(id);
                break;
            default:
        }

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Puestos) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Consultar_Puestos_Por_Nombre',
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
/* =============================================
--NOMBRE_FUNCIÓN: _validate_fields_areas
--DESCRIPCIÓN: Metodo para validar no duplicidad
--PARÁMETROS: value.- nombre a validar
--id.- ID del dato
--field.- campo para validar
--area_id.- id del area
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validate_fields_areas(value, id, field, area_id) {
    var Puestos = null;
    var Resultado = null;

    try {
        Puestos = new Object();
        if (id !== null)
            Puestos.Puesto_ID = parseInt(id);

        switch (field) {
            case 'nombre':
                Puestos.Nombre = value;
                break;
            case 'area':
                Puestos.Nombre = value;
                Puestos.Area_ID = parseInt(area_id);
                break;
            default:
        }

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(Puestos) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Consultar_Puestos_Por_Nombre',
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
/* =============================================
--NOMBRE_FUNCIÓN: _load_areas
--DESCRIPCIÓN: Metodo para cargar datos de areas
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_areas() {
    var filtros = null;
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Consultar_Areas',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    var datos_combo = $.parseJSON(datos.d);
                    var select2 = $('#cmb_areas');
                    $('option', select2).remove();
                    var options = '<option value="">--SELECCIONE--</option>';
                    for (var Indice_Estatus = 0; Indice_Estatus < datos_combo.length; Indice_Estatus++) {
                        options += '<option value="' + datos_combo[Indice_Estatus].Area_ID + '">' + datos_combo[Indice_Estatus].Nombre.toUpperCase() + '</option>';
                        
                    }
                    select2.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN: _load_estatus
--DESCRIPCIÓN: Metodo para cargar datos de estatus
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_estatus() {
    var filtros = null;
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Puestos_Controller.asmx/Consultar_Estatus',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    var datos_combo = $.parseJSON(datos.d);
                    var select2 = $('#cmb_estatus');
                    $('option', select2).remove();
                    var options = '<option value="">--SELECCIONE--</option>';
                    for (var Indice_Estatus = 0; Indice_Estatus < datos_combo.length; Indice_Estatus++) {
                        options += '<option value="' + datos_combo[Indice_Estatus].Estatus_ID + '">' + datos_combo[Indice_Estatus].Estatus.toUpperCase() + '</option>';
                        if (datos_combo[Indice_Estatus].Estatus.toUpperCase() == 'ACTIVO') {
                            estatusActivo = datos_combo[Indice_Estatus].Estatus_ID;
                        }
                    }
                    select2.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}