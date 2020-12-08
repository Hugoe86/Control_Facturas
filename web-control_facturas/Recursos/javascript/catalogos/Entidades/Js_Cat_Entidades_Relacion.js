
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal_relacion
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal_relacion(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_relacion(title_window);

    //  se muestra el modal
    jQuery('#modal_entidad_relacion').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_relacion
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_relacion(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_modal_relacion").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_relacion_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_relacion_click() {
    //  se llama al evento que cierra el modal
    _set_close_modal_relacion();
}


/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal_relacion
--DESCRIPCIÓN:          Ejecuta la sección para ocultar el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal_relacion() {
    //  cierra el modal
    jQuery('#modal_entidad_relacion').modal('hide');
}





/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_relacion
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_relacion() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda


        //  filtro para la cuenta
        filtros.Cuenta_Id = 0;
        filtros.Entidad_Id = parseInt($('#txt_entidad_relacion_id').val());

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Relacion_Entidad_Cuenta_Controller.asmx/Consultar_Relacion_Entidad_Cuenta_Filtros',
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
                    $('#tbl_relacion_entidad').bootstrapTable('load', JSON.parse(datos.d));
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_relacion]', e);
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_modal_relacion
--DESCRIPCIÓN:          Crea los eventos de los botones que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_modal_relacion() {
    try {
        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_guardar
        --DESCRIPCIÓN:          Evento con el que se valida y guarda la información
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_agregar_relacion').on('click', function (e) {
            e.preventDefault();

            //  variables
            var title = $('#btn_guardar').attr('title');//  almacenara el titulo que tiene el botón de nuevo
            var valida_datos_requerido = _validar_relacion();// almacenara el resultado de la validación de los datos


            //  validamos si la información es correcta
            if (valida_datos_requerido.Estatus) {

                //  se ejecuta el evento de alta_relacion
                alta_relacion()

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
        --FECHA_CREO:           07 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_cancelar_relacion').on('click', function (e) {
            e.preventDefault();

            //  limpia los controles del modal
            _limpiar_todos_controles_modal_relacion();

            //  cierra el modal
            _cancelar_modal_relacion_click();
        });



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_modal_relacion] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal_relacion
--DESCRIPCIÓN:          limpia todos los controles que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal_relacion() {

    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           07 Octubre de 2019
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
       --FECHA_CREO:           07 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('input[type=hidden]').each(function () { $(this).val(''); });


        //  limpia el valor del combo
        $('#cmb_cuenta').empty().trigger("change");


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal_relacion] ', 'limpiar controles. ' + e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_cuentas
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_cuentas(cmb) {
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
                url: '../Catalogos/controllers/Cuentas_Controller.asmx/Consultar_Cuentas_Combo',
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
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_cuentas]', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _validar_relacion
--DESCRIPCIÓN:          Se valida la información requerida para realizar una acción
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validar_relacion() {
    var _output = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

    try {

        //  se inicializan las propiedades
        _output.Estatus = true;
        _output.Mensaje = '';

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------


        //  filtro para la entidad
        if ($('#cmb_cuenta :selected').val() === undefined || $('#cmb_cuenta :selected').val() === '') {
            //  agregamos el estilo al control que no cumple
            _add_class_error("cmb_cuenta");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La cuenta.<br />';
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
        _mostrar_mensaje('Informe técnico' + '[_validar_relacion]', e);
    } finally {
        return _output;//   se regresa la variable
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       alta_relacion
--DESCRIPCIÓN:          se registra el alta_relacion
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function alta_relacion() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Cuenta_Id = parseInt($('#cmb_cuenta :selected').val());
        obj.Entidad_Id = parseInt($('#txt_entidad_relacion_id').val());

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Relacion_Entidad_Cuenta_Controller.asmx/Alta',
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

                        //  se consultan la informacion
                        _consultar_relacion();

                    } else {
                        //  se muestra el mensaje del error que se presento
                        _mostrar_mensaje(result.Titulo, result.Mensaje);
                    }
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[alta_relacion]', e);
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_relacion
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_relacion() {

    try {
        //  se destruye la tabla
        $('#tbl_relacion_entidad').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_relacion_entidad').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            data: [],
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDysplay: false,
            search: true,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,

            columns: [
                { field: 'Relacion_Id', title: 'Relacion_Id', align: 'center', valign: 'top', visible: false },

                { field: 'Entidad_Id', title: 'Entidad_Id', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },
                { field: 'Cuenta_Id', title: 'Cuenta_Id', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },


                { field: 'Cuenta', title: 'Cuenta', align: 'left', valign: 'top', width: 150, visible: true, sortable: true },
                { field: 'Entidad', title: 'Entidad', align: 'left', valign: 'top', width: 150, visible: false, sortable: true },

                {
                    field: 'Relacion_Id',
                    title: 'Eliminar',
                    width: 100,
                    align: 'right',
                    valign: 'top',
                    halign: 'center',

                    formatter: function (value, row) {
                        var opciones;//   variable para formar la estructura del boton

                        opciones = '<div style=" text-align: center;">';
                        opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Relacion_Id +
                                '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_relacion_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                        opciones += '</div>';

                        return opciones;
                    }
                },
            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_relacion] ', e.message);
    }
}




/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_relacion_click
--DESCRIPCIÓN:          Se realiza la acción de eliminar
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_relacion_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        //  se crea el objeto de confirmación
        bootbox.confirm({
            title: 'ELIMINAR REGISTRO',
            message: 'Esta seguro de Eliminar el registro seleccionado?',
            callback: function (result) {

                //  validamos que accion tomo el usuario
                if (result) {

                    //  se declara la variable
                    var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                    //  se asignan los elementos al objeto de filtro
                    obj.Relacion_Id = parseInt(row.Relacion_Id);

                    //  se convierte la información a json
                    var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                    //  se ejecuta la petición
                    $.ajax({
                        type: 'POST',
                        url: 'controllers/Relacion_Entidad_Cuenta_Controller.asmx/Eliminar',
                        data: $data,
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        async: false,
                        cache: false,
                        success: function (datos) {

                            //  se valida que contenga información la variable
                            if (datos != null) {

                                //  se convierte la información que arroja el servicio web
                                var result = JSON.parse(datos.d);// variable para guardar los datos recibidos

                                //  si la acción es exitosa 
                                if (result.Estatus == 'success') {

                                    //  muestra el mensaje de éxito de la operación realizada
                                    _mostrar_mensaje(result.Titulo, result.Mensaje);

                                    //  se realiza la consulta
                                    _consultar_relacion();

                                } else {//  si la acción marco un error

                                    //  se muestra el mensaje del error que se presento
                                    _mostrar_mensaje(result.Titulo, result.Mensaje);
                                }
                            }
                        }
                    });
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_click] ', e);
    }
}
