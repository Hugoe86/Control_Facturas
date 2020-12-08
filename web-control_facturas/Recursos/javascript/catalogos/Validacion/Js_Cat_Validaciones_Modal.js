var $eliminados = [];
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

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_(title_window);

    //  se muestra el modal
    jQuery('#modal_validaciones').modal('show', { backdrop: 'static', keyboard: false });
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
function _set_title_modal_(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_modal").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal() {
    //  cierra el modal
    jQuery('#modal_validaciones').modal('hide');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_catalogo
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
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
            filtros.Validacion_Id = parseInt($('#cmb_busqueda_nombre :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Validacion_Id = 0;
        }


        //  filtro para el estatus
        if ($.trim($('#cmb_busqueda_estatus :selected').val()) !== '') {
            filtros.Estatus = ($('#cmb_busqueda_estatus :selected').val());
        }


        filtros.Concepto_ID = $('#cmb_tipo_operacion_filtro').val();

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Validacion_Controller.asmx/Consultar_Validaciones_Conceptos_Filtros',
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
                    $('#tbl_validaciones').bootstrapTable('load', JSON.parse(datos.d));
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
--FECHA_CREO:           07 Octubre de 2019
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
        --FECHA_CREO:           07 Octubre de 2019
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
        --FECHA_CREO:           07 Octubre de 2019
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


        $('#btn_agregar_usuario').on('click', function (e) {
            e.preventDefault();
            var validacion = _validarDatos_Responsables(); //Validacion de responsables
            if (validacion.Estatus)
                _insert_responsable();
            else
                _mostrar_mensaje('Validación', validacion.Mensaje);
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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal() {

    try {

        $('input[type=text]').each(function () { $(this).val(''); });

        $('input[type=hidden]').each(function () { $(this).val(''); });

        //  limpia el valor del combo
        $('#cmb_estatus').empty().trigger("change");

        //  limpia el valor del combo
        $('#cmb_area').empty().trigger("change");

        //  se inicializa el combo del estatus
        _load_cmb_estatus("cmb_estatus");

        $('#cmb_tipo_operacion').empty().trigger('change');

        $('#cmb_tipo_operacion').empty().trigger('change');

        $('#cmb_responsable').empty().trigger('change');

        $('#tbl_responsables').bootstrapTable('load', []);
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
--FECHA_CREO:           07 Octubre de 2019
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
--FECHA_CREO:           07 Octubre de 2019
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
        //  filtro para el nombre de la validacion
        if ($('#txt_validacion').val() == '' || $('#txt_validacion').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_validacion");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El nombre de la validacion.<br />';
        }


        //  filtro para el area
        if ($('#cmb_area :selected').val() === undefined || $('#cmb_area :selected').val() === '') {
            //  agregamos el estilo al control que no cumple
            _add_class_error("cmb_area");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El área.<br />';
        }

        //  filtro para el area
        if ($('#cmb_tipo_operacion :selected').val() === undefined || $('#cmb_tipo_operacion :selected').val() === '') {
            //  agregamos el estilo al control que no cumple
            _add_class_error("cmb_tipo_operacion");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El tipo de operación.<br />';
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

        //  filtro para el numero de la validacion
        if ($('#txt_orden').val() == '' || $('#txt_orden').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_orden");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El número de la validacion.<br />';
        }

        var table = $('#tbl_responsables').bootstrapTable('getData');
        //  filtro para el numero de la validacion
        if (table.length == 0) {
            
            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;Ingrese al menos un responsable.<br />';
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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function alta() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Validacion_Id = 0;
        obj.Area_ID = $('#cmb_area').val() == null ? 0 : parseInt($('#cmb_area').val());
        obj.Concepto_ID = $('#cmb_tipo_operacion').val() == null ? 0 : parseInt($('#cmb_tipo_operacion').val());
        obj.Validacion = $('#txt_validacion').val();
        obj.Estatus = $('#cmb_estatus :selected').val();
        obj.Orden = $('#txt_orden').val();

        obj.List_Usuarios = $('#tbl_responsables').bootstrapTable('getData');


        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Validacion_Controller.asmx/Alta',
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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function actualizar() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {
        //  se carga la información 
        obj.Validacion_Id = parseInt($('#txt_validacion_id').val());
        obj.Area_ID = $('#cmb_area').val() == null ? 0 : parseInt($('#cmb_area').val());
        obj.Concepto_ID = $('#cmb_tipo_operacion').val() == null ? 0 : parseInt($('#cmb_tipo_operacion').val());
        obj.Validacion = $('#txt_validacion').val();
        obj.Estatus = $('#cmb_estatus :selected').val();
        obj.Orden = $('#txt_orden').val();

        var lst = [];
        lst.push($('#tbl_responsables').bootstrapTable('getData'));
        if ($eliminados.length > 0)
            lst.push($eliminados);

        obj.List_Usuarios = $('#tbl_responsables').bootstrapTable('getData');
        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Validacion_Controller.asmx/Actualizar',
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

/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_responsables
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           19 feb de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_responsables() {

    try {
        //  se destruye la tabla
        $('#tbl_responsables').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_responsables').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            data: [],
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDisplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,

            columns: [
                
                { field: 'Usuario', title: 'Reponsable', align: 'left', valign: 'top', visible: true, sortable: true },
                {
                    field: 'Validacion_Id',
                    title: '',
                    width: 80,
                    align: 'right',
                    valign: 'top',
                    halign: 'center',

                    formatter: function (value, row, index) {

                        var opciones;//   variable para formar la estructura del boton
                        opciones = '<div style=" text-align: center;">';
                        opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Validacion_Id + '" href="javascript:void(0)" data-index="' + index + '" data-usuario=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_responsable_click(this);" title="Eliminar"><i class="glyphicon glyphicon-remove"></i></a></div>';
                        opciones += '</div>';

                        return opciones;
                    }
                },
            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_principal] ', e.message);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _insert_responsable
--DESCRIPCIÓN:          Agrega el usuario seleccionado en la tabla de responsables
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           19 feb de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _insert_responsable() {

    try {

        var resp = $('#cmb_responsable').select2('data')[0];

        $('#tbl_responsables').bootstrapTable('insertRow', {
            index: 0,
            row: {
                Validacion_Usuario_ID: 0,
                Validacion_Id: 0,
                Usuario_Id: resp.id,
                Usuario: resp.text,
                Estatus: "AGREGAR"

            }
        });
        $('#cmb_responsable').empty().trigger('change');

    } catch (e) {
        _mostrar_mensaje('Error Técnico.', e);
    }

}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_usuarios_por_validacion
--DESCRIPCIÓN:          Consulta los usuarios agregado para la validacion
--PARÁMETROS:           validacion_id: Id de la validacion a editar
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           19 feb de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_usuarios_por_validacion(validacion_id) {

    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Validacion_Id = validacion_id;
        

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Validacion_Controller.asmx/Consultar_Validaciones_Usuarios',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null) {

                    var result = JSON.parse(datos.d);// almacena la información recibida

                    $('#tbl_responsables').bootstrapTable('load', result);
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[alta]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_responsable_click
--DESCRIPCIÓN:          Eliminar los usuarios agregados a la tabla de responsables
--PARÁMETROS:           datos: Trae la informacion del usuario a eliminar
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           20 feb de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_responsable_click(datos) {
    var $row = $(datos).data('usuario');
    var $index = $(datos).data('index');

    if ($row.Estatus == "AGREGADO") {
        $row.Estatus = "ELIMINADO";
        $eliminados.push($row);
        $('#tbl_responsables').bootstrapTable('remove', { field: 'Usuario_Id', values: [$row.Usuario_Id] });
    } else {
        $('#tbl_responsables').bootstrapTable('remove', { field: 'Usuario_Id', values: [$row.Usuario_Id] });
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _validarDatos_Responsables
--DESCRIPCIÓN:          Se valida la información requerida para realizar una acción
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez   
--FECHA_CREO:           20 de Febrero 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validarDatos_Responsables() {
    var _output = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

    try {
        //  se inicializan las propiedades
        _output.Estatus = true;
        _output.Mensaje = '';

        //  
        if ($('#cmb_responsable :selected').val() === undefined || $('#cmb_responsable :selected').val() === '') {
            _output.Estatus = false;
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;Seleccione el responsable.<br />';
        } else {
            
            var table = $('#tbl_responsables').bootstrapTable('getData');
            if (table.length > 0) {
                var validacion = true;

                $.each(table, function (i, e) {
                    if (parseInt(e.Usuario_Id) == parseInt($('#cmb_responsable :selected').val()))
                        validacion = false;
                });

                if (!validacion) {
                    _output.Estatus = false;
                    _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El responsable ya se encuentra registrado en la tabla.<br />';
                }
            }
        }
        
        //  validamos si no cumple alguno de los elementos
        if (_output.Mensaje != "") {
            //  asignamos el titulo del mensaje
            _output.Mensaje = "Favor de proporcionar lo siguiente: <br />" + _output.Mensaje;
        }

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_validarDatos_nuevo]', e);
    } finally {
        return _output;//   se regresa la variable
    }
}