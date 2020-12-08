/* =============================================
//  variables globales*/
var Close_Modal = true;//   variable para cerrar el modal  
var Estatus_Activo = '';//  variable para definir el estatus
/* =============================================*/

/* =============================================
--NOMBRE_FUNCIÓN:       $(document).on('ready', function () {
--DESCRIPCIÓN:          se manda llamar al método de cargar vistas  
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
$(document).on('ready', function () {
    //  se inicializa los controles dentro del formulario
    _inicializar_pagina();
});

/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_pagina
--DESCRIPCIÓN:          Carga los eventos y funciones que tendrá cada pagina HTML
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_pagina() {
    try {
        //  se ejecutan los metodos
        _filtroEstatus();
        _habilitar_controles('Inicio');
        _limpiar_controles();
        _cargar_tabla();
        _search();
        _modal();
        _cargar_tabla_permisos();
        _load_estatus();
        _load_rol();
        _eventos_textbox();
        _eventos();
        _enter_keypress_modal();
        _set_location_toolbar();
        _load_cmb_areas('cmb_area');
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _estado_inicial
--DESCRIPCIÓN:          Carga el funcionamiento principal del formulario
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _estado_inicial() {
    try {
        _habilitar_controles('Inicio');
        _limpiar_controles();
        $('#tbl_usuarios').bootstrapTable('refresh', 'controllers/Usuarios_Controller.asmx/Consultar_Usuarios_Por_Filtros');
        _set_location_toolbar();
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _habilitar_controles
--DESCRIPCIÓN:          Habilita los controles del formulario
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _habilitar_controles(opcion) {
    var estatus = false;//  variable para definir el estatus de los controles

    switch (opcion) {
        case "Nuevo":
            estatus = true;
            //$('cmb_estatus').attr({ disabled: estatus });
            break;
        case "Modificar":
            estatus = true;
            //$('cmb_estatus').attr({ disabled: !estatus });
            break;
        case "Inicio":
            break;
    }

    $('#txt_nombre').attr({ disabled: !estatus });
    $('#txt_usuario').attr({ disabled: !estatus });
    $('#txt_password').attr({ disabled: !estatus });
    $('#txt_email').attr({ disabled: !estatus });
    $('#cmb_rol').attr({ disabled: !estatus });

}
/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_controles
--DESCRIPCIÓN:          Limpia los controles del formulario   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_controles() {
    /* =============================================
    --NOMBRE_FUNCIÓN:       $('input[type=text]').each(function () 
    --DESCRIPCIÓN:          Limpia los controles text del formulario   
    --PARÁMETROS:           NA
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('input[type=text]').each(function () { $(this).val(''); });

    /* =============================================
   --NOMBRE_FUNCIÓN:        $('select').each(function () 
   --DESCRIPCIÓN:          Limpia los controles select del formulario   
   --PARÁMETROS:           NA
   --CREO:                 Hugo Enrique Ramírez Aguilera
   --FECHA_CREO:           07 Octubre de 2019
   --MODIFICÓ:
   --FECHA_MODIFICÓ:
   --CAUSA_MODIFICACIÓN:
   =============================================*/
    $('select').each(function () { $(this).val(Estatus_Activo); });

    /* =============================================
    --NOMBRE_FUNCIÓN:        $('textarea').each(function () 
    --DESCRIPCIÓN:          Limpia los controles textarea del formulario   
    --PARÁMETROS:           NA
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('textarea').each(function () { $(this).val(''); });


    $('#cmb_estatusfiltro').val('');
    $('#txt_password').val('');
    $('#txt_nombre').val('');
    $('#txt_usuario_id').val('');
    $('#txt_rel_id').val('');
    $('#cmb_rol').val('');
    $('#cmb_area').empty().trigger("change");

    _validation_sumary(null);
    _clear_all_class_error();
}
/* =============================================
--NOMBRE_FUNCIÓN:       _eventos
--DESCRIPCIÓN:          Se generan las acciones que realizaran los botones del formulario
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos() {
    try {
        /* =============================================
        --NOMBRE_FUNCIÓN:       $('#modal_datos').on('hidden.bs.modal', function ()
        --DESCRIPCIÓN:          Muestra el modal
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_datos').on('hidden.bs.modal', function () {
            if (!Close_Modal)
                $(this).modal('show');
        });

        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_nuevo
        --DESCRIPCIÓN:          Acciones realizadas al dar click en el botón de nuevo
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/

        $('#btn_nuevo').click(function (e) {
            _limpiar_controles();
            _habilitar_controles('Nuevo');

            _search_permiso(0);
            _launch_modal('<i class="fa fa-floppy-o" style="font-size: 25px;"></i>&nbsp;&nbsp;Alta de registro');
        });

        /* =============================================
        --NOMBRE_FUNCIÓN:        $('.cancelar').each(function
        --DESCRIPCIÓN:          reinicia los elementos del formulario   
        --PARÁMETROS:           index: valor del indice
        --                      element: estructura del control
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('.cancelar').each(function (index, element) {
            $(this).on('click', function (e) {
                e.preventDefault();
                _estado_inicial();
            });
        });

        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_salir
        --DESCRIPCIÓN:          Redirecciona al formulario principal   
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_salir').on('click', function (e) {
            e.preventDefault(); window.location.href = '../Paginas_Generales/Frm_Apl_Principal.aspx';
        });
        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_busqueda
        --DESCRIPCIÓN:          Realiza la busqueda de la informacion 
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_busqueda').on('click', function (e) {
            e.preventDefault();
            _search();
        });
        /* =============================================
        --NOMBRE_FUNCIÓN:       $('#modal_datos input[type=text]').each(function
        --DESCRIPCIÓN:          Remueve el estilo de los controles del modal
        --PARÁMETROS:           index: valor del indice
        --                      element: estructura del control
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_datos input').each(function (index, element) {
            $(this).on('focus', function () {
                _remove_class_error('#' + $(this).attr('id'));
            });
        });
        /* =============================================
        --NOMBRE_FUNCIÓN:       $('#modal_datos select').each(function
        --DESCRIPCIÓN:          Remueve el estilo de los controles del modal
        --PARÁMETROS:           index: valor del indice
        --                      element: estructura del control
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_datos select').each(function (index, element) {
            $(this).on('focus', function () {
                _remove_class_error('#' + $(this).attr('id'));
            });
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_textbox
--DESCRIPCIÓN:          Carga las eventos que tendran dentro de los textbox   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_textbox() {

    /* =============================================
    --NOMBRE_FUNCIÓN:       $('#txt_usuario').on('blur', function ()
    --DESCRIPCIÓN:          Se validan los caracteres permitidos 
    --PARÁMETROS:           NA
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#txt_usuario').on('blur', function () {
        $(this).val($(this).val().match(/^[0-9a-zA-Z\u0020]+$/) ? $(this).val() : $(this).val().replace(/\W+/g, ''));
    });
    
    /* =============================================
    --NOMBRE_FUNCIÓN:        $('#txt_email').on('blur', function ()
    --DESCRIPCIÓN:          Se validan los caracteres permitidos    
    --PARÁMETROS:           NA
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#txt_email').on('blur', function () {
        $(this).val(this.value.match(/^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$/) ? $(this).val() : _add_class_error('txt_email'));
    });

}

/* =============================================
--NOMBRE_FUNCIÓN:       _mostrar_mensaje
--DESCRIPCIÓN:          Muestra un mensaje con la información de la variable de mensaje
--PARÁMETROS:           titulo: Nombre que tendrá el titulo de la pantalla de mensaje
                        mensaje: String con la información del mensaje
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _mostrar_mensaje(titulo, mensaje) {

    //  se construye la ventana de dialogo 
    bootbox.dialog({
        message: mensaje,
        title: titulo,
        locale: 'en',
        closeButton: true,
        buttons: [{
            label: 'Cerrar',
            className: 'btn-default',
            callback: function () { }
        }]
    });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cargar_tabla
--DESCRIPCIÓN:          Genere la estructura de la tabla   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cargar_tabla() {

    try {
        $('#tbl_usuarios').bootstrapTable('destroy');
        $('#tbl_usuarios').bootstrapTable({
            cache: false,
            //width: 900,
            //height: 400,
            striped: true,
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDisplay: false,
            search: true,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            clickToSelect: true,
            columns: [
                { field: 'Usuario_ID', title: '', width: 0, align: 'center', valign: 'bottom', sortable: true, visible: false },
                { field: 'Area_ID', title: 'Area_ID', width: 100, align: 'left', valign: 'bottom', sortable: true, visible: false },
                { field: 'Nombre', title: 'Nombre', width: 100, align: 'left', valign: 'bottom', sortable: true, },
                { field: 'Area', title: 'Area', width: 100, align: 'left', valign: 'bottom', sortable: true, },
                { field: 'Nombre', title: 'Nombre', width: 100, align: 'left', valign: 'bottom', sortable: true, },
                { field: 'Usuario', title: 'Usuario', width: 100, align: 'left', valign: 'bottom', sortable: true, },
                { field: 'Estatus', title: 'Estatus', width: 50, align: 'left', valign: 'bottom', sortable: true, visible: true },
                { field: 'Password', title: 'Password', width: 100, align: 'left', valign: 'bottom', sortable: true, visible: false },
                { field: 'Rol_ID', title: 'Rol_ID', width: 100, align: 'letf', valign: 'bottom', sortable: true, visible: false },
                { field: 'Email', title: 'Email', width: 200, align: 'left', valign: 'bottom', sortable: true, clickToSelect: false },
                { field: 'Empresa_ID', title: '', width: 200, align: 'left', valign: 'bottom', sortable: true, visible: false },
                { field: 'Estatus_ID', title: '', width: 200, align: 'left', valign: 'bottom', sortable: true, visible: false },
                { field: 'Rel_Usuarios_Rol_ID', title: '',  width: 200, align: 'left', valign: 'bottom', sortable: true, visible: false},
                {
                    field: 'Usuario_ID',
                    title: '',
                    align: 'center',
                    valign: 'bottom',
                    width: 60,
                    clickToSelect: false,

                    /* =============================================
                    --NOMBRE_FUNCIÓN:        formatter: function (value, row) {
                    --DESCRIPCIÓN:          Se da formato a la celda  
                    --PARÁMETROS:           value: es el valor de la celda
                    --                      row: estructura del renglon de la tabla
                    --CREO:                 Hugo Enrique Ramírez Aguilera
                    --FECHA_CREO:           07 Octubre de 2019
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    formatter: function (value, row) {
                        return '<div> ' +
                            '<a class="remove ml10 edit" id="' + row.Usuario_ID + '" href="javascript:void(0)" data-usuario=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i></button>' +
                            '&nbsp;&nbsp;<a class="remove ml10 delete" id="' + row.Usuario_ID + '" href="javascript:void(0)" data-usuario=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i></a>' +
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
--NOMBRE_FUNCIÓN:       _cargar_tabla_permisos
--DESCRIPCIÓN:          Genere la estructura de la tabla   
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           20 de Febrero de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cargar_tabla_permisos() {

    try {
        $('#tbl_permisos_usuarios').bootstrapTable('destroy');
        $('#tbl_permisos_usuarios').bootstrapTable({
            cache: false,
            //width: 900,
            //height: 400,
            striped: true,
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDisplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,
            clickToSelect: true,
            checkboxHeader: false,
            columns: [
                { field: 'check', title: '', align: 'center', valign: 'top', sortable: false, checkbox: true },
                { field: 'Nombre_Permiso', title: 'Permiso', align: 'left', valign: 'top', sortable: true, visible: true },
                { field: 'Descripcion', title: 'Descripcion', align: 'left', valign: 'top', sortable: true, },


            ]
        });
    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _alta_usuarios
--DESCRIPCIÓN:          se registra el alta   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _alta_usuarios() {
    var usuarios = null;//   estructura en donde se cargaran la información del formulario
    var isComplete = false;//  define si la operacion se realizo con exito

    try {
        //  se inicializan los elementos
        usuarios = new Object();

        //  se ingresan los valores
        usuarios.Nombre = $('#txt_nombre').val();
        usuarios.Usuario = $('#txt_usuario').val();
        usuarios.Estatus_ID = parseInt($('#cmb_estatus').val());
        usuarios.Password = $('#txt_password').val();
        usuarios.Email = $('#txt_email').val();
        usuarios.Rol_ID = parseInt($('#cmb_rol').val());
        usuarios.Area_ID = $('#cmb_area').val() == null ? 0 : parseInt($('#cmb_area').val());
        usuarios.List_Permisos = $('#tbl_permisos_usuarios').bootstrapTable('getData');
        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(usuarios) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/Alta',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {

                var resultado = JSON.parse(result.d);// almacena la información recibida

                //  validamos que tenga informacion recibida
                if (resultado != null && resultado != undefined && resultado != '') {

                    //  validamos que la operacion sea exitosa
                    if (resultado.Estatus == 'success') {
                        _search();
                        isComplete = true;
                    }
                    //  validamos que sea un error
                    else if (resultado.Estatus == 'error') {
                        _validation_sumary(resultado);
                    }
                    
                }
                    //  validamos cualquier otra accion recibida
                else {
                    _validation_sumary(resultado);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;
}
/* =============================================
--NOMBRE_FUNCIÓN:       _modificar_usuarios
--DESCRIPCIÓN:          Se actualiza la informacion   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _modificar_usuarios() {
    var usuarios = null;  //   estructura en donde se cargaran la información del formulario
    var isComplete = false;//  define si la operacion se realizo con exito

    try {
        //  se instancia el elemento
        usuarios = new Object();

        //  se ingresan los valores
        usuarios.Usuario_ID = parseInt($('#txt_usuario_id').val());
        usuarios.Rel_Usuarios_Rol_ID = parseInt($('#txt_rel_id').val());
        usuarios.Nombre = $('#txt_nombre').val();
        usuarios.Usuario = $('#txt_usuario').val();
        usuarios.Password = $('#txt_password').val();
        usuarios.Email = $('#txt_email').val();
        usuarios.Estatus_ID = parseInt($('#cmb_estatus').val());
        usuarios.Rol_ID = parseInt($('#cmb_rol').val());
        usuarios.Area_ID = $('#cmb_area').val() == null ? 0 : parseInt($('#cmb_area').val());
        usuarios.List_Permisos = $('#tbl_permisos_usuarios').bootstrapTable('getData');
        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(usuarios) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/Actualizar',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {

                var resultado = JSON.parse(result.d);// almacena la información recibida

                //  validamos si tiene alguna información la variable
                if (resultado != null && resultado != undefined && resultado != '') {

                    //  validamos que la operacion sea exitosa
                    if (resultado.Estatus == 'success') {
                        _search();
                        isComplete = true;
                    }
                        //  validamos que sea un error
                    else if (resultado.Estatus == 'error') {
                        _validation_sumary(resultado);
                    }
                }
                    //  validamos que no tiene informacion
                else {
                    _validation_sumary(resultado);
                }
            }
        });

    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;
}
/* =============================================
--NOMBRE_FUNCIÓN:       _eliminar_usuarios
--DESCRIPCIÓN:          Elimina un registro de la base de datos  
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eliminar_usuarios(usuario_id) {
    var usuarios = null; //   estructura en donde se cargaran la información del formulario
  

    try {
        //  se instancia el objeto
        usuarios = new Object();

        //  se ingresan los valores
        usuarios.Usuario_ID = parseInt(usuario_id);
        usuarios.Rel_Usuarios_Rol_ID = parseInt(usuario_id);

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(usuarios) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/Eliminar',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var resultado = JSON.parse(result.d);// almacena la información recibida

                //  validamos si tiene alguna información la variable
                if (resultado != null && resultado != undefined && resultado != '') {

                    //  validamos que la operacion sea exitosa
                    if (resultado.Estatus == 'success') {
                        _search();
                    }
                        //  validamos que sea un error
                    else if (resultado.Estatus == 'error') {
                        _validation_sumary(resultado);
                    }
                }
                    //  validamos que no tiene informacion
                else {
                    _validation_sumary(resultado);
                }
            }
        });

    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _modal
--DESCRIPCIÓN:          Crea la estructura del modal 
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _modal() {
    var tags = '';//    vairable en la que se ingresa la estructura del modal
    try {
        tags += '<div class="modal fade" id="modal_datos" name="modal_datos" role="dialog" aria-hidden="true" data-backdrop="static" data-keyboard="false">';
        tags += '<div class="modal-dialog modal-lg">';
        tags += '<div class="modal-content">';

        tags += '<div class="modal-header">';
        tags += '<button type="button" class="close cancelar" data-dismiss="modal" aria-label="Close" onclick="_set_close_modal(true);"><i class="fa fa-times"></i></button>';
        tags += '<h4 class="modal-title" id="myModalLabel">';
        tags += '<label id="lbl_titulo"></label>';
        tags += '</h4>';
        tags += '</div>';

        tags += '<div class="modal-body">';

        tags += '<div class="row">' +
            '   <div class="col-sm-9">' +
            '       <label class="fuente_lbl_controles">*Nombre</label>' +
            '       <input type="text" id="txt_nombre" name="txt_nombre" class="form-control input-sm" disabled="disabled" placeholder="Nombre" data-parsley-required="true" maxlength="100" required /> ' +
            '   </div>' +
            '   <div class="col-sm-3">' +
            '      <label class="fuente_lbl_controles">*Estatus</label>' +
            '       <select id="cmb_estatus" name="cmb_estatus" class="form-control input-sm" data-parsley-required="true" required ></select> ' +
            '    </div>' +
            '</div>' +
            '<div class="row">' +

            '   <div class="col-md-4" >' +
            '       <label class="fuente_lbl_controles">*Usuario</label>' +
            '       <input type="text" id="txt_usuario" name="txt_usuario" class="form-control input-sm" disabled="disabled" placeholder="Usuario" data-parsley-required="true" maxlength="20" required /> ' +
            '       <input type="hidden" id="txt_usuario_id"/>' +
            '       <input type="hidden" id="txt_rel_id"/>' +
            '   </div>' +

            '   <div class="col-md-5">' +
            '       <label class="fuente_lbl_controles">*Email</label>' +
            '       <input type="email" id="txt_email" name="txt_email" class="form-control input-sm" disabled="disabled" placeholder="Email" data-parsley-required="true" maxlength="100" required /> ' +
            '   </div>' +

            '   <div class="col-sm-3">' +
            '       <label class="fuente_lbl_controles">*Contraseña</label>' +
            '       <input type="password" id="txt_password" name="txt_password" class="form-control input-sm" disabled="disabled" placeholder="Contraseña" data-parsley-required="true" maxlength="100" required /> ' +
            '   </div>' +
            '</div>' +
            '<div class="row">' +


            '   <div class="col-md-6">' +
            '       <label class="fuente_lbl_controles">*Área</label>' +
            '       <select id="cmb_area"  name="cmb_area" class="form-control input-sm" style="width:100%;margin-top: 0px;" data-parsley-required="true" required></select> ' +
            '   </div>' +
            '    <div class="col-sm-6">' +
            '       <label class="fuente_lbl_controles">*Rol</label>' +
            '       <select id="cmb_rol" name="cmb_rol" class="form-control input-sm" style="margin-top: 0px;" disabled="disabled" data-parsley-required="true" required ></select> ' +
            '    </div>' +
            '</div>';

        tags += '<div class="row" style="margin-top: 10px;">';

        tags += '<div class="col-md-12">';
        
        tags += '<table class="table table-responsive" id="tbl_permisos_usuarios"><caption>&nbsp;Permisos</caption></table>';
        tags += '</div >';
        tags += '</div >';

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


        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_guardar_datos
        --DESCRIPCIÓN:          Guarda la informacion del usuario
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_guardar_datos').bind('click', function (e) {
            e.preventDefault();

            //  validamos que tenga informacion el control del id
            if ($('#txt_usuario_id').val() != null && $('#txt_usuario_id').val() != undefined && $('#txt_usuario_id').val() != '') {
                var _output = _validation('editar');//  variable para establecer si se cumplen con las validaciones de informacion requerida

                //  validamos que la infomracion sea correcta
                if (_output.Estatus) {

                    //  validamos que la modificacion sea exitosa
                    if (_modificar_usuarios()) {
                        _estado_inicial();
                        _set_close_modal(true);
                        jQuery('#modal_datos').modal('hide');
                    }
                }
                //  validamos que no se cumpla con lo minimo requerido para modificar
                else {
                    _set_close_modal(false);
                }
            }
            //  validamos que no se cumple con los valores de id requieridos para actualizar, por lo tanto sera una alta
            else {
                //  validamos que la infomracion sea correcta
                var _output = _validation('alta');//    variable en la que se guarda la validacion de los elementos requeridos para la alta

                //  validamos que sea verdadero el estatus de la validacion
                if (_output.Estatus) {

                    //  validamos que sea correcta el alta
                    if (_alta_usuarios()) {
                        _estado_inicial();
                        _set_close_modal(true);
                        jQuery('#modal_datos').modal('hide');
                    }
                }
                //  validamos que no se cumple con las validaciones de los controles
                else {
                    _set_close_modal(false);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal
--DESCRIPCIÓN:          Se marca el estatus de la variable   
--PARÁMETROS:           state: estatus que se le otorga a la variable
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal(state) {
    Close_Modal = state;
}
/* =============================================
--NOMBRE_FUNCIÓN:       btn_editar_click
--DESCRIPCIÓN:          Carga la información del registro de la tabla, carga la información dentro de los controles correspondientes   
--PARÁMETROS:           usuario: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_editar_click(usuario) {
    var row = $(usuario).data('usuario');//   variable para guardar la informacion del renglon de la tabla

    $('#txt_usuario_id').val(row.Usuario_ID);
    $('#txt_nombre').val(row.Nombre);
    $('#txt_usuario').val(row.Usuario);
    $('#txt_password').val(row.Password);
    $('#txt_email').val(row.Email); 
    $('#cmb_estatus').val(row.Estatus_ID);
    $('#cmb_rol').val(row.Rol_ID);
    $('#txt_rel_id').val(row.Rel_Usuarios_Rol_ID);


    if (row.Area_ID != null) {
        $('#cmb_area').select2("trigger", "select", {
            data: { id: row.Area_ID, text: row.Area }
        });
    }

    _search_permiso(row.Usuario_ID);
    _habilitar_controles('Modificar');
    _launch_modal('<i class="glyphicon glyphicon-edit" style="font-size: 25px;"></i>&nbsp;&nbsp;Actualizar registro');
}
/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_click
--DESCRIPCIÓN:          Se realiza la acción de eliminar
--PARÁMETROS:           usuario: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_click(usuario) {
    var row = $(usuario).data('usuario');//   variable para guardar la informacion del renglon de la tabla

    bootbox.confirm({
        title: 'Eliminar Registro',
        message: '¿Está seguro de eliminar el registro seleccionado?',
        callback: function (result) {
            //  valida que la accion seleccionada sea correcta, para proceder con la operacion
            if (result) {
                _eliminar_usuarios(row.Usuario_ID);
            }
            _estado_inicial();
        }
    });
}
/* =============================================
--NOMBRE_FUNCIÓN:       _set_location_toolbar
--DESCRIPCIÓN:          Establece el estilo que tendrá el control
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_location_toolbar() {
    //  se remueve el estilo
    $('#toolbar').parent().removeClass("pull-left");

    //  se agrega el estilo
    $('#toolbar').parent().addClass("pull-right");
}
/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal(titulo) {
    $("#lbl_titulo").html(titulo);
}
/* =============================================
--NOMBRE_FUNCIÓN:       _search
--DESCRIPCIÓN:          Realiza la busqueda de la informacion  
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _search() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        show_loading_bar({
            pct: 78,
            wait: .5,
            delay: .5,
            finish: function (pct) {

                //  se inicializa el objeto
                filtros = new Object();

                //  se agregan los valores
                filtros.Nombre = $('#txt_busqueda_por_usuario').val() === '' ? '' : $('#txt_busqueda_por_usuario').val();

                //  se valida que el estatus tenga informacion seleccionada
                if ($('#cmb_estatusfiltro').val() != '' && $('#cmb_estatusfiltro').val() != undefined && $('#cmb_estatusfiltro').val() != null) {
                    filtros.Estatus_ID = parseInt($('#cmb_estatusfiltro').val());
                }

                //  se convierte a la estructura que pueda leer el controlador
                var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

                //  se realiza la petición
                jQuery.ajax({
                    type: 'POST',
                    url: 'controllers/Usuarios_Controller.asmx/Consultar_Usuarios_Por_Filtros',
                    data: $data,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (datos) {

                        //  validamos que tenga informacion los valores recibidos
                        if (datos !== null) {
                            $('#tbl_usuarios').bootstrapTable('load', JSON.parse(datos.d));
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
--NOMBRE_FUNCIÓN:       _search_permiso
--DESCRIPCIÓN:          Realiza la busqueda de la informacion  
--PARÁMETROS:           usuario_id
--CREO:                 Jose Maldonado 
--FECHA_CREO:           20 de feb 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _search_permiso(usuario_id) {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        show_loading_bar({
            pct: 78,
            wait: .5,
            delay: .5,
            finish: function (pct) {

                //  se inicializa el objeto
                filtros = new Object();

                //  se agregan los valores
                filtros.Usuario_ID = usuario_id;

                //  se convierte a la estructura que pueda leer el controlador
                var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

                //  se realiza la petición
                jQuery.ajax({
                    type: 'POST',
                    url: 'controllers/Usuarios_Controller.asmx/Consultar_Permisos_Usuarios',
                    data: $data,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: true,
                    cache: false,
                    success: function (datos) {

                        //  validamos que tenga informacion los valores recibidos
                        if (datos !== null) {
                            $('#tbl_permisos_usuarios').bootstrapTable('load', JSON.parse(datos.d));
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
--NOMBRE_FUNCIÓN:       _validation
--DESCRIPCIÓN:          Valida la informacion requerida para continuar con el proceso  
--PARÁMETROS:           opcion: variable para saber el tipo de operacion que se realizara
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validation(opcion) {
    var _output = new Object();//   variable con la que se establecera la estructura de la validacion realizada

    _output.Estatus = true;
    _output.Mensaje = '';

    //  validamos el objeto tenga informacion
    if (!$('#txt_usuario').parsley().isValid()) {
        _add_class_error('#txt_usuario');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El usuario es un dato requerido.<br />';
    }
    //  validamos el objeto si tiene informacion
    else {

        var resultado = (opcion === 'alta') ?// variable con la que se estara aplicando el mensaje de error
            _validate_fields($('#txt_usuario').val(), null, 'usuario') :
            _validate_fields($('#txt_usuario').val(), $('#txt_usuario_id').val(), 'usuario');

        //  validamos que sea un error el resultado arrojado
        if (resultado.Estatus === 'error') {
            _add_class_error('#txt_usuario');
            _output.Estatus = false;
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;' + resultado.Mensaje + '<br />';
        }
    }
    
    //  validamos la contraseña este vacia
    if (!$('#txt_password').parsley().isValid()) {
        _add_class_error('#txt_password');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La contraseña es un dato requerido.<br />';
    }

    //  validamos el nombre este vacia
    if (!$('#txt_nombre').parsley().isValid()) {
        _add_class_error('#txt_nombre');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El nombre es un dato requerido.<br />';
    }
    //  validamos el email este vacia
    if (!$('#txt_email').parsley().isValid()) {
        _add_class_error('#txt_email');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El email es un dato requerido.<br />';
    } 

    //  validamos el estatus este vacia
    if (!$('#cmb_estatus').parsley().isValid()) {
        _add_class_error('#cmb_estatus');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El estatus es un dato requerido.<br />';
    } 


    //  validamos el estatus este vacia
    if (!$('#cmb_area').parsley().isValid()) {
        _add_class_error('#cmb_area');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El área es un dato requerido.<br />';
    } 
    

    //  validamos el rol este vacia
    if (!$('#cmb_rol').parsley().isValid()) {
        _add_class_error('#cmb_rol');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El rol es un dato requerido.<br />';
    } 

    //  validmos que el estatus sea falso con lo que se mostrara el mensaje de error
    if (!_output.Estatus) _validation_sumary(_output);

    return _output;
}
/* =============================================
--NOMBRE_FUNCIÓN:       _add_class_error
--DESCRIPCIÓN:          Se agrego un estilo a un control, el cual se utiliza para las funciones de validación
--PARÁMETROS:           selector: nombre del control al cual se le estará aplicando el estilo de error
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _add_class_error(selector) {


    selector = selector.replace("#", '');

    //  se le asigna el estilo al control
    $('#' + selector).addClass('alert-danger');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _remove_class_error
--DESCRIPCIÓN:          Se quita el estilo de error de un control 
--PARÁMETROS:           selector: nombre del control al cual se le estará quitando el estilo de error
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _remove_class_error(selector) {
    //  se remueve el estilo al control
    $(selector).removeClass('alert-danger');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _clear_all_class_error
--DESCRIPCIÓN:          Quita los estilos de error de todos los controles del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _clear_all_class_error() {
    /* =============================================
    --NOMBRE_FUNCIÓN:       $('#modal_datos input[type=text]').each(function 
    --DESCRIPCIÓN:          remueve todos los estidos de error de los controles
    --PARÁMETROS:           index: valor del indice
        --                  element: estructura del control
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#modal_datos input').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });

    /* =============================================
    --NOMBRE_FUNCIÓN:       $('#modal_datos select').each(functions
    --DESCRIPCIÓN:          remueve todos los estidos de error de los controles
    --PARÁMETROS:           index: valor del indice
        --                  element: estructura del control
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#modal_datos select').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });

    /* =============================================
    --NOMBRE_FUNCIÓN:       $('#modal_datos input[type=password]').each(function
    --DESCRIPCIÓN:          remueve todos los estidos de error de los controles
    --PARÁMETROS:           index: valor del indice
        --                  element: estructura del control
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#modal_datos input[type=password]').each(function (index, element) {
        _remove_class_error('#' + $(this).attr('id'));
    });
}
/* =============================================
--NOMBRE_FUNCIÓN:       _validation_sumary
--DESCRIPCIÓN:          Validaciones para el modal  
--PARÁMETROS:           validation: tipo de validacion que se ejecutara
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validation_sumary(validation) {
    var header_message = '<i class="fa fa-exclamation-triangle fa-2x"></i><span>Observaciones</span><br />';//  variable en la que se define el encabezado del mensaje

    //  validamos que sea nula la informacion
    if (validation == null) {
        $('#lbl_msg_error').html('');
        $('#sumary_error').css('display', 'none');
    }
        //  se valida que contenga informacion
    else {
        $('#lbl_msg_error').html(header_message + validation.Mensaje);
        $('#sumary_error').css('display', 'block');
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal(title_window) {
    _set_title_modal(title_window);
    jQuery('#modal_datos').modal('show', { backdrop: 'static', keyboard: false });
    $('#txt_usuario').focus();

}
/* =============================================
--NOMBRE_FUNCIÓN:       _enter_keypress_modal
--DESCRIPCIÓN:          evento que se realiza al presionar enter en una caja de texto
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _enter_keypress_modal() {
    var $btn = $('[id$=btn_guardar_datos]').get(0);//   variable que contendra el boton de guardar

    /* =============================================
   --NOMBRE_FUNCIÓN:       $(window).keypress(function (e)
   --DESCRIPCIÓN:          evento que captura la tecla de enter
   --PARÁMETROS:           e: parametro que se refiere al control
   --CREO:                 Hugo Enrique Ramírez Aguilera
   --FECHA_CREO:           07 Octubre de 2019
   --MODIFICÓ:
   --FECHA_MODIFICÓ:
   --CAUSA_MODIFICACIÓN:
   =============================================*/
    $(window).keypress(function (e) {

        //  validamos que la techa sea enter y no sea el control de textareas
        if (e.which === 13 && e.target.type !== 'textarea') {

            //  validamos el boton
            if ($btn != undefined && $btn != null) {
                //  validamos que la operacion sea enviar
                if ($btn.type === 'submit') {
                    $btn.click();
                }
                //  validamos cualquier otra accion 
                else {
                    eval($btn.href);
                }

                return false;
            }
        }
    });
}
/* =============================================
--NOMBRE_FUNCIÓN:       _validate_fields
--DESCRIPCIÓN:          Se valida que operacion se realizara
--PARÁMETROS:           values: es el valor
--                      id: valor del id
--                      field: campo al que se le estara validando
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validate_fields(value, id, field) {
    var usuario = null;//   se le asignaran a las propiedades del filtros
    var resultado = null;// variable para establecer el resultado

    try {
        //  se inicializa el objeto
        usuario = new Object();

        //  validamos que el id no sea nulo
        if (id !== null)
            usuario.Usuario_ID = parseInt(id);

        //  se filtran los reultados del campo field
        switch (field) {
            //  se valida que sea usuario
            case 'usuario':
                usuario.Usuario = value;
                break;
            //  se valida que sea supervisor
            case 'supervisor':
                usuario.No_Supervisor = value;
                break;
            default:
        }
        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(usuario) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/Consultar_Usuarios_Por_Nombre',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                //  validamos que tenga informacion recibida
                if (result !== null)
                    resultado = JSON.parse(result.d);
            }
        });
    } catch (e) {
        resultado = new Object();
        resultado.Estatus = 'error';
        resultado.Mensaje = 'No fue posible realizar la validación del ' + field + ' en la base de datos.';
        _mostrar_mensaje('Informe Técnico', e);
    }
    return resultado;
}
/* =============================================
--NOMBRE_FUNCIÓN:       _load_estatus
--DESCRIPCIÓN:          Carga la informacion de la base de datos en el combo
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_estatus() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/ConsultarEstatus',
            //data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {

                //  validamos que tenga informacion recibida
                if (datos !== null) {

                    var datos_combo = $.parseJSON(datos.d);//   variable para obtener la informacion recibida
                    var select = $('#cmb_estatus');//   variable para obtener el control del combo del estatus
                    $('option', select).remove();
                    var options = '';// variable con la que se define la estructura e informacion del combo

                    //  se recooren los datos recibidos
                    for (var indice_estatus = 0; indice_estatus < datos_combo.length; indice_estatus++)/*   variable con la que se recorrera el for */{
                        options += '<option value="' + datos_combo[indice_estatus].Estatus_ID + '">' + datos_combo[indice_estatus].Estatus.toUpperCase() + '</option>';

                        //  validamos que este activo el objeto
                        if (datos_combo[indice_estatus].Estatus.toUpperCase() == 'ACTIVO') {
                            Estatus_Activo = datos_combo[indice_estatus].Estatus_ID;
                        }
                    }
                    select.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _filtroEstatus
--DESCRIPCIÓN:          Carga los estatus dentro del combo  
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _filtroEstatus() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/ConsultarFiltroEstatus',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {

                //  validamos que tenga informacion recibida
                if (datos !== null) {

                    var datos_combo = $.parseJSON(datos.d);//   variable para obtener la informacion recibida
                    var select = $('#cmb_estatusfiltro');//   variable para obtener el control del combo del estatus
                    var options = '<option value=""><-TODOS-></option>';// variable con la que se define la estructura e informacion del combo

                    $('option', select).remove();

                    //  se recooren los datos recibidos
                    for (var indice_estatus = 0; indice_estatus < datos_combo.length; indice_estatus++) /*   variable con la que se recorrera el for */ {
                        options += '<option value="' + datos_combo[indice_estatus].Estatus_ID + '">' + datos_combo[indice_estatus].Estatus + '</option>';
                    }

                    select.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _load_rol
--DESCRIPCIÓN:          Carga las valores dentro del combo 
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_rol() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Usuarios_Controller.asmx/ConsultarRol',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (datos) {

                //  validamos que tenga informacion recibida
                if (datos !== null) {

                    var datos_combo = $.parseJSON(datos.d);//   variable para obtener la informacion recibida
                    var select = $('#cmb_rol');//   variable para obtener el control del combo del estatus
                    var options = '<option value=""><-SELECCIONE-></option>';// variable con la que se define la estructura e informacion del combo


                    $('option', select).remove();

                    //  se recooren los datos recibidos
                    for (var indice_rol = 0; indice_rol < datos_combo.length; indice_rol++)  /*   variable con la que se recorrera el for */ {
                        options += '<option value="' + datos_combo[indice_rol].Rol_ID + '">' + datos_combo[indice_rol].Nombre + '</option>';
                    }
                    select.append(options);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_areas
--DESCRIPCIÓN:           Carga los datos del estatus del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 de Octubre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_areas(cmb) {
    try {
        var obj = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se consultara y cargara la información
        $('#' + cmb).select2({
            language: "es",
            theme: "classic",
            placeholder: 'SELECCIONE',
            allowClear: true,
            ajax: {
                url: 'controllers/Usuarios_Controller.asmx/Consultar_Areas_Combo',
                cache: true,
                dataType: 'json',
                type: "POST",
                delay: 250,
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
                        page: params.page,
                        no_supervisor: $('#txt_no_empleado_responsable').val(),
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
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_participantes]', e);
    }
}
