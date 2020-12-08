
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_(title_window);

    //  se muestra el modal
    jQuery('#modal_area').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_modal").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_click() {
    //  se llama al evento que cierra el modal
    _set_close_modal();
}


/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal
--DESCRIPCIÓN:          Ejecuta la sección para ocultar el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal() {
    //  cierra el modal
    jQuery('#modal_area').modal('hide');
}





/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_catalogo
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_catalogo() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda


        //  filtro para la cuenta
        if ($.trim($('#cmb_busqueda_nombre :selected').val()) !== '') {
            filtros.Area_Id = parseInt($('#cmb_busqueda_nombre :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Area_Id = 0;
        }


        //  filtro para el estatus
        if ($.trim($('#cmb_busqueda_estatus :selected').val()) !== '') {
            filtros.Estatus = ($('#cmb_busqueda_estatus :selected').val());
        }

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Area_Controller.asmx/Consultar_Areas_Filtros',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que exista información en los valores recibidos
                if (datos !== null) {

                    //  se valida que tenga información la variable recibida
                    datos.d = (datos.d == undefined || datos.d == null) ? '[]' : datos.d;

                    //  se carga la información
                    $('#tbl_areas').bootstrapTable('load', JSON.parse(datos.d));
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_catalogo]', e);
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_modal
--DESCRIPCIÓN:          Crea los eventos de los botones que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_modal() {
    try {
        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_guardar
        --DESCRIPCIÓN:          Evento con el que se valida y guarda la información
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           09 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_guardar').on('click', function (e) {
            e.preventDefault();

            //  variables
            var title = $('#btn_guardar').attr('title');//  almacenara el titulo que tiene el botón de nuevo
            var valida_datos_requerido = _validarDatos_nuevo();// almacenara el resultado de la validación de los datos


            //  validamos si la información es correcta
            if (valida_datos_requerido.Estatus) {

                //  validamos si es un nuevo registro, en caso contrario sera una actualización
                if (title == "Guardar") {
                    //  se ejecuta el evento de alta
                    alta()
                }
                else {
                    //  se ejecuta el evento de actualización
                    actualizar();
                }
            }
            else {
                //  se muestra el mensaje del error que se presento
                _mostrar_mensaje('Información' + '', valida_datos_requerido.Mensaje);
            }

        });
        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_cancelar
        --DESCRIPCIÓN:          Evento con el que se cancela la acción de nuevo o actualización
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           09 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_cancelar').on('click', function (e) {
            e.preventDefault();

            //  limpia los controles del modal
            _limpiar_todos_controles_modal();

            //  cierra el modal
            _cancelar_modal_click();
        });



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_modal] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal
--DESCRIPCIÓN:          limpia todos los controles que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal() {

    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           09 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('input[type=text]').each(function () { $(this).val(''); });

        /* =============================================
       --NOMBRE_FUNCIÓN:       input[type=hidden]
       --DESCRIPCIÓN:          recorre los controles de tipo hidden y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           09 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('input[type=hidden]').each(function () { $(this).val(''); });


        /* =============================================
       --NOMBRE_FUNCIÓN:       input[type=color]
       --DESCRIPCIÓN:          recorre los controles de tipo color y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           09 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('input[type=color]').each(function () { $(this).val(''); });


        //  limpia el valor del combo
        $('#cmb_estatus').empty().trigger("change");

        //  se inicializa el combo del estatus
        _load_cmb_estatus("cmb_estatus");


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal] ', 'limpiar controles. ' + e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_estatus
--DESCRIPCIÓN:          Carga los datos del estatus del combo
--PARÁMETROS:           cmb: nombre del control al cual se le asignaran los valores
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_estatus(cmb) {
    try {

        //  se carga la información de los estatus dentro del control
        $('#' + cmb).select2({
            language: "es",
            theme: "classic",
            placeholder: 'SELECCIONE',
            allowClear: true,
            data: [
                {
                    id: '',
                    text: '',
                }, {
                    id: 'ACTIVO',
                    text: 'ACTIVO',
                }, {
                    id: 'INACTIVO',
                    text: 'INACTIVO',
                },
            ],
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_estatus_seguimiento]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _validarDatos_nuevo
--DESCRIPCIÓN:          Se valida la información requerida para realizar una acción
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validarDatos_nuevo() {
    var _output = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

    try {
        //  se limpian los estilos de los controles
        _clear_all_class_error();

        //  se inicializan las propiedades
        _output.Estatus = true;
        _output.Mensaje = '';

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------
        //  filtro para el area
        if ($('#txt_area').val() == '' || $('#txt_area').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_area");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El Área de la entidad.<br />';
        }

        //  filtro para el estatus
        if ($('#cmb_estatus :selected').val() === undefined || $('#cmb_estatus :selected').val() === '') {
            //  agregamos el estilo al control que no cumple
            _add_class_error("cmb_estatus");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El estatus.<br />';
        }


        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------
        //  validamos si no cumple alguno de los elementos
        if (_output.Mensaje != "") {
            //  asignamos el titulo del mensaje
            _output.Mensaje = "Favor de proporcionar lo siguiente: <br />" + _output.Mensaje;
        }

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_validarDatos_nuevo]', e);
    } finally {
        return _output;//   se regresa la variable
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       alta
--DESCRIPCIÓN:          se registra el alta
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function alta() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario
    var color;//    variable con la que se obtendra el color del input

    try {

        //  se obtiene le color
        color = $("#txt_color_area").val();
        color = color.replace('#', '');


        //  se carga la información 
        obj.Area_Id = 0;
        obj.Area = $('#txt_area').val();
        obj.Estatus = $('#cmb_estatus :selected').val();
        obj.Color = color;


        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Area_Controller.asmx/Alta',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null) {

                    var result = JSON.parse(datos.d);// almacena la información recibida

                    //  validamos si la operación fue exitosa, de no ser así se mostrara el error
                    if (result.Estatus == 'success') {

                        //  se muestra el mensaje
                        _mostrar_mensaje(result.Titulo, result.Mensaje);

                        //  se limpia el modal
                        _limpiar_todos_controles_modal();

                        //  se cierra el modal
                        _cancelar_modal_click();

                        //  se consultan la informacion
                        _consultar_catalogo();

                    } else {
                        //  se muestra el mensaje del error que se presento
                        _mostrar_mensaje(result.Titulo, result.Mensaje);
                    }
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[alta]', e);
    }

}

/* =============================================
--NOMBRE_FUNCIÓN:       actualizar
--DESCRIPCIÓN:          se registra el actualización
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           09 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function actualizar() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario
    var color;//    variable con la que se obtendra el color del input

    try {

        //  se obtiene le color
        color = $("#txt_color_area").val();
        color = color.replace('#', '');


        //  se carga la información 
        obj.Area_Id = parseInt($('#txt_area_id').val());
        obj.Area = $('#txt_area').val();
        obj.Estatus = $('#cmb_estatus :selected').val();
        obj.Color = color;

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Area_Controller.asmx/Actualizar',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información los datos recibidos
                if (datos != null) {

                    //  se carga la información
                    var result = JSON.parse(datos.d);// almacenara los datos recibidos

                    //  validamos si la operación fue exitosa
                    if (result.Estatus == 'success') {

                        //  se muestra el mensaje de la operación realizada
                        _mostrar_mensaje(result.Titulo, result.Mensaje);

                        //  se limpian los controles
                        _limpiar_todos_controles_modal();

                        //  se cierra el modal
                        _cancelar_modal_click();

                        //se consultan la informacion
                        _consultar_catalogo();
                    } else {
                        //  se muestra el mensaje del error que se presento
                        _mostrar_mensaje(result.Titulo, result.Mensaje);
                    }
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[actualizar]', e);
    }

}


