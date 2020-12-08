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
        _configurar_botones('InicioNuevo');
        _limpiar_controles();
        _search();
        _eventos();
       
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
    var estatus = false;
    switch (opcion) {
        case "Nuevo":
            estatus = false;
            break;
        case "Modificar":
            estatus = false;
           break;
        case "Inicio":
            estatus = true;
            break;
    }

    $('input[type=text]').each(function () {
        $(this).attr('disabled', estatus);
    });
}

function _configurar_botones(opcion) {


    switch (opcion) {
        case "InicioNuevo":
            $('#btn_salir').removeClass().addClass('btn btn-info btn-sm');
            $('#btn_salir').attr('title', 'Inicio');
            $('#btn_salir i').removeClass().addClass('fa fa-home');
            $('#btn_salir span').html('Inicio');

            $('#btn_nuevo').removeClass().addClass('btn btn-info btn-sm');
            $('#btn_nuevo').attr('title', 'Nuevo');
            $('#btn_nuevo i').removeClass().addClass('fa fa-plus');
            $('#btn_nuevo span').html('Nuevo');
            break;

        case "InicioEditar":
            $('#btn_salir').removeClass().addClass('btn btn-info btn-sm');
            $('#btn_salir').attr('title', 'Inicio');
            $('#btn_salir i').removeClass().addClass('fa fa-home');
            $('#btn_salir span').html('Inicio');

            $('#btn_nuevo').removeClass().addClass('btn btn-info btn-sm');
            $('#btn_nuevo').attr('title', 'Editar');
            $('#btn_nuevo i').removeClass().addClass('fa fa-edit');
            $('#btn_nuevo span').html('Editar');
            break;

        case "Modificar":
            $('#btn_salir').removeClass().addClass('btn btn-danger btn-sm');
            $('#btn_salir').attr('title', 'Cancelar');
            $('#btn_salir i').removeClass().addClass('fa fa-remove');
            $('#btn_salir span').html('Cancelar');

            $('#btn_nuevo').removeClass().addClass('btn btn-blue btn-sm');
            $('#btn_nuevo').attr('title', 'Actualizar');
            $('#btn_nuevo i').removeClass().addClass('fa fa-refresh');
            $('#btn_nuevo span').html('Actualizar');
            break;

        case "Nuevo":
            $('#btn_salir').removeClass().addClass('btn btn-danger btn-sm');
            $('#btn_salir').attr('title', 'Cancelar');
            $('#btn_salir i').removeClass().addClass('fa fa-remove');
            $('#btn_salir span').html('Cancelar');

            $('#btn_nuevo').removeClass().addClass('btn btn-success btn-sm');
            $('#btn_nuevo').attr('title', 'Guardar');
            $('#btn_nuevo i').removeClass().addClass('fa fa-floppy-o');
            $('#btn_nuevo span').html('Guardar');
            break;
    }
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
    $('#txt_parametro_id').val('');
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

        $('#btn_nuevo').click(function (e) {

            var title = $(this).attr('title');
            if (title == "Nuevo") {
                _habilitar_controles('Nuevo');
                _configurar_botones('Nuevo');
            } else if (title == "Editar") {
                _habilitar_controles('Modificar');
                _configurar_botones('Modificar');
            } else if (title == "Actualizar" || title == "Guardar") {
                _alta_modificar_parametros();
            }

        });

        $('#btn_salir').on('click', function (e) {
            e.preventDefault();
            var title = $(this).attr('title');
            if (title == "Cancelar") {
                _search();
            } else {
                window.location.href = '../Paginas_Generales/Frm_Apl_Principal.aspx';
            }
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
--NOMBRE_FUNCIÓN: _alta_modificar_parametros
--DESCRIPCIÓN: Metodo para alta o modificar los datos
--PARÁMETROS: N/A
--CREO: Saul Jonathan Marquez Martinez
--FECHA_CREO: 06 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _alta_modificar_parametros() {
    var parametro = null;
    var isComplete = false;

    try {
        parametro = new Object();
        parametro.Parametro_ID = $('#txt_parametro_id').val();
        parametro.Folio_Inicio = $('#txt_folio_inicia').val();


        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(parametro) });

        $.ajax({
            type: 'POST',
            url: 'controllers/Parametros_Controller.asmx/Alta_Actualizar_Parametro',
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
        filtros = new Object();


        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });

        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Parametros_Controller.asmx/Consultar_Parametro_Por_Filtros',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {
                if (datos !== null) {
                    _limpiar_controles();
                    var resultado = JSON.parse(datos.d);
                    if (resultado.length > 0) {
                        _habilitar_controles('Inicio');
                        _configurar_botones('InicioEditar');

                        var row = resultado[0];
                        $('#txt_folio_inicia').val(row.Folio_Inicio);
                        $('#txt_parametro_id').val(row.Parametro_ID);


                    } else {

                        _habilitar_controles('Inicio');
                        _configurar_botones('InicioNuevo');
                    }
                }
            }
        });

    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}

