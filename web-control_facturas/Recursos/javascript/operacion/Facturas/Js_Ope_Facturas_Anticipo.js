
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal_anticipo
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal_anticipo(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_anticipo(title_window);

    //  se muestra el modal
    jQuery('#modal_facturas_anticipo').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_anticipo
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_anticipo(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_anticipo").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_anticipo_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_anticipo_click() {
    //  se llama al evento que cierra el modal
    _set_close_modal_anticipo();
}


/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal_anticipo
--DESCRIPCIÓN:          Ejecuta la sección para ocultar el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal_anticipo() {
    //  cierra el modal
    jQuery('#modal_facturas_anticipo').modal('hide');
}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_anticipo
--DESCRIPCIÓN:          Establece los metodos principales del modal de adjuntos
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_anticipo() {
    try {
        //  se inicializan los controles
        _inicializar_fecha_anticipo();

        //  cargamos la informacion del combo
        _load_cmb_anticipos_catalogo('cmb_anticipo');

        //  inicializamos los eventos del modal
        _eventos_modal_anticipo();

        //  se crea la tabla
        crear_tabla_anticipos();

        //  se asigna solo numeros a los controles
        _keyDownInt('txt_saldo_anticipo');
        _keyDownInt('txt_monto_anticipo');
        

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_inicializar_vista_anticipo]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_informacion_anticipos
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_informacion_anticipos() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  filtro para la cuenta
        filtros.Anticipo_Factura_Id = 0;
        filtros.Anticipo_Id = 0;
        filtros.Factura_Id = parseInt($('#txt_factura_anticipo_id').val());
       
        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Consultar_Facturas_Anticipos_Filtros',
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
                    $('#tbl_facturas_anticipos').bootstrapTable('load', JSON.parse(datos.d));
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_informacion_anticipos]', e);
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_fecha_anticipo
--DESCRIPCIÓN:          Se inicializa el control de las fechas
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           29 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_fecha_anticipo() {

    //  se carga el control
    $('#dtp_txt_fecha_anticipo').datetimepicker({
        defaultDate: new Date(),
        viewMode: 'days',
        locale: 'es',
        format: "DD/MM/YYYY"
    });

    //  se establece el formato
    $("#dtp_txt_fecha_anticipo").datetimepicker("useCurrent", true);



}


/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_modal_anticipo
--DESCRIPCIÓN:          Crea los eventos de los botones que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_modal_anticipo() {
    try {
        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_guardar_anticipo
        --DESCRIPCIÓN:          Evento con el que se valida y guarda la información
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_guardar_anticipo').on('click', function (e) {
            e.preventDefault();

            //  variables
            var title = $('#btn_guardar_anticipo').attr('title');//  almacenara el titulo que tiene el botón de nuevo
            var valida_datos_requerido = _validar_datos_anticipo();// almacenara el resultado de la validación de los datos
            var _saldo = 0;//   variable para almacenar el saldo del anticipo
            var _monto = 0;//   variable para almacenar el monto del anticipo

            //  validamos si la información es correcta
            if (valida_datos_requerido.Estatus) {

                _saldo = parseFloat($('#txt_saldo_anticipo').val());
                _monto = parseFloat($('#txt_monto_anticipo').val());

                //  validamos que el monto no sea mayor al saldo
                if (_monto <= _saldo) {


                    //  validamos si es un nuevo registro, en caso contrario sera una actualización
                    if (title == "Guardar Anticipo") {
                        //  se ejecuta el evento de alta_anticipo
                        alta_anticipo()
                    }
                }
                    //  se muestra el mensaje del saldo es mayor
                else {
                    _mostrar_mensaje('Información' + '', "El monto no puede ser mayor al saldo del anticipo");
                }
            }
            else {
                //  se muestra el mensaje del error que se presento
                _mostrar_mensaje('Información' + '', valida_datos_requerido.Mensaje);
            }

        });
        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_cancelar_anticipo
        --DESCRIPCIÓN:          Evento con el que se cancela la acción de nuevo o actualización
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_cancelar_anticipo').on('click', function (e) {
            e.preventDefault();

            //  limpia los controles del modal
            _limpiar_todos_controles_modal_anticipo();

            //  cierra el modal
            _cancelar_modal_anticipo_click();
        });



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_modal_anticipo] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal_anticipo
--DESCRIPCIÓN:          limpia todos los controles que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal_anticipo() {

    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       #modal_facturas_anticipo input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_facturas_anticipo input[type=text]').each(function () { $(this).val(''); });

        /* =============================================
       --NOMBRE_FUNCIÓN:       #modal_facturas_anticipo input[type=hidden]
       --DESCRIPCIÓN:          recorre los controles de tipo hidden y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           24 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#modal_facturas_anticipo input[type=hidden]').each(function () { $(this).val(''); });


        //  limpia el valor del combo
        $('#cmb_anticipo').empty().trigger("change");


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal_anticipo] ',  e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_anticipos_catalogo
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_anticipos_catalogo(cmb) {
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
                url: 'controllers/Facturas_Controller.asmx/Consultar_Anticipos_Nombre_Combo',
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


        /* =============================================
        --NOMBRE_FUNCIÓN:       $(cmb).on("select2:select", function (evt) {
        --DESCRIPCIÓN:          Consulta la informacion de la factura
        --PARÁMETROS:           evt: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#' + cmb).on("select2:select", function (evt) {


            var _dato_combo = evt.params.data;// variable para obtener la estructura del combo
            var obj = new Object();//   estructura en donde se cargaran la información del formulario


            //  se carga la información 
            obj.Anticipo_Factura_Id = 0;
            obj.Factura_Id = 0;
            obj.Anticipo_Id = parseInt(_dato_combo.id);

            //  se convierte a la estructura que pueda leer el controlador
            var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador


            //-------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------------
            //  re ejecuta la peticion
            jQuery.ajax({
                type: 'POST',
                url: 'controllers/Facturas_Controller.asmx/Consultar_Datos_Anticipos_Catalogo',
                data: $data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cache: false,
                success: function (datos) {

                    //  validamos que tenga informacion
                    if (datos !== null) {

                        var resultado = JSON.parse(datos.d);//  variable en la que contendremos los datos recibidos

                        //  se carga el saldo al control
                        $('#txt_saldo_anticipo').val(resultado[0].Saldo);
                    
                    }
                    //  se oculta el control
                    hide_loading_bar();
                }
            });

        });

        /* =============================================
        --NOMBRE_FUNCIÓN:       $(cmb).on("select2:unselect", function (evt) 
        --DESCRIPCIÓN:          Limpia los controles dentro del modal
        --PARÁMETROS:           evt: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $("#" + cmb).on("select2:unselect", function (evt) {

            //  se limpia el saldo
            $('#txt_saldo_anticipo').val('');
        });


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_anticipos_catalogo]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _validarDatos_nuevo
--DESCRIPCIÓN:          Se valida la información requerida para realizar una acción
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validar_datos_anticipo() {
    var _output = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

    try {
        //  se limpian los estilos de los controles
        _clear_all_class_error();

        //  se inicializan las propiedades
        _output.Estatus = true;
        _output.Mensaje = '';

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------


        //  filtro para el tipo de concepto (combo)
        if ($('#cmb_anticipo :selected').val() === undefined || $('#cmb_anticipo :selected').val() === '') {
            //  agregamos el estilo al control que no cumple
            _add_class_error("cmb_anticipo");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El anticipo.<br />';
        }



        //  filtro para la fecha de recepcion
        if ($('#txt_fecha_anticipo').val() == '' || $('#txt_fecha_anticipo').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_fecha_anticipo");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La fecha del anticipo.<br />';
        }


        //  filtro para el RFC
        if ($('#txt_monto_anticipo').val() == '' || $('#txt_monto_anticipo').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_monto_anticipo");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El monto del anticipo.<br />';
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
        _mostrar_mensaje('Informe técnico' + '[_validar_datos_anticipo]', e);
    } finally {
        return _output;//   se regresa la variable
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       alta_anticipo
--DESCRIPCIÓN:          se registra el alta del antcipo
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function alta_anticipo() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = parseInt($('#txt_factura_anticipo_id').val());
        obj.Anticipo_Id = $('#cmb_anticipo :selected').val();
        obj.Anticipo = $('#cmb_anticipo').text();
        obj.Fecha = Date.parse($('#txt_fecha_anticipo').val());
        obj.Monto = $('#txt_monto_anticipo').val();

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Alta_Anticipo',
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



                        /* =============================================
                         --NOMBRE_FUNCIÓN:       #modal_facturas_anticipo input[type=text]
                         --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
                         --PARÁMETROS:           NA
                         --CREO:                 Hugo Enrique Ramírez Aguilera
                         --FECHA_CREO:           24 Octubre de 2019
                         --MODIFICÓ:
                         --FECHA_MODIFICÓ:
                         --CAUSA_MODIFICACIÓN:
                         =============================================*/
                        $('#modal_facturas_anticipo input[type=text]').each(function () { $(this).val(''); });

                        //  limpia el valor del combo
                        $('#cmb_anticipo').empty().trigger("change");

                        //  se consultan la informacion
                        _consultar_informacion_anticipos();
                        
                    } else {
                        //  se muestra el mensaje del error que se presento
                        _mostrar_mensaje(result.Titulo, result.Mensaje);
                    }
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[alta_anticipo]', e);
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_anticipos
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_anticipos() {

    try {
        //  se destruye la tabla
        $('#tbl_facturas_anticipos').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_facturas_anticipos').bootstrapTable({
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

                { field: 'Anticipo_Factura_Id', title: 'Anticipo_Factura_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Anticipo_Id', title: 'Anticipo_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Factura_Id', title: 'Factura_Id', align: 'center', valign: 'top', visible: false },


                { field: 'Anticipo', title: 'Anticipo', align: 'left', valign: 'top', width: 150, visible: true, sortable: true },
                
                {
                    field: 'Monto', title: 'Monto', align: 'right', valign: 'top', width: 50, visible: true, sortable: true,

                    /* =============================================
                    --NOMBRE_FUNCIÓN:        formatter: function (value, row) {
                    --DESCRIPCIÓN:          Evento con el que se da estilo a la celda
                    --PARÁMETROS:           value: es el valor de la celda
                    --                      row: estructura del renglon de la tabla
                    --CREO:                 Hugo Enrique Ramírez Aguilera
                    --FECHA_CREO:           24 Octubre de 2019
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    formatter: function (value, row) {
                        return accounting.formatMoney(value);
                    },


                },

                { field: 'Fecha', title: 'Fecha del anticipo', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },
                { field: 'Fecha_Texto', title: 'Fecha del anticipo', align: 'center', valign: 'top', width: 50, visible: true, sortable: true },

               {
                   //  historico
                   field: 'Anticipo_Factura_Id',
                   title: 'Eliminar',
                   width: 80,
                   align: 'right',
                   valign: 'top',
                   halign: 'center',

                   /* =============================================
                   --NOMBRE_FUNCIÓN:        formatter: function (value, row) {
                   --DESCRIPCIÓN:          Evento con el que se da estilo a la celda
                   --PARÁMETROS:           value: es el valor de la celda
                   --                      row: estructura del renglon de la tabla
                   --CREO:                 Hugo Enrique Ramírez Aguilera
                   --FECHA_CREO:           24 Octubre de 2019
                   --MODIFICÓ:
                   --FECHA_MODIFICÓ:
                   --CAUSA_MODIFICACIÓN:
                   =============================================*/
                   formatter: function (value, row) {

                       var opciones;//   variable para formar la estructura del boton

                       opciones = '<div style=" text-align: center;">';
                       opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Anticipo_Factura_Id +
                                       '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                       '\' onclick="btn_eliminar_anticipo_click(this);" title="Historico">' +
                                       '<i class="fa fa-times"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                       opciones += '</div>';

                       return opciones;
                   }
               },

            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_anticipos] ', e.message);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_anticipo_click
--DESCRIPCIÓN:          Se realiza la acción de eliminar
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_anticipo_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        //  validamos que pueda realizar la accion
        if ($('#chk_validador').prop('checked') == false) {

            //  se crea el objeto de confirmación
            bootbox.confirm({
                title: 'ELIMINAR ANTICIPO',
                message: 'Esta seguro de Eliminar el registro del anticipo seleccionado?',
                callback: function (result) {

                    //  validamos que accion tomo el usuario
                    if (result) {

                        //  se declara la variable
                        var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                        //  validamos que sea la operacion por conexion a la base de datos
                        if (row.Anticipo_Factura_Id != "0") {
                            //  se asignan los elementos al objeto de filtro
                            obj.Anticipo_Factura_Id = parseInt(row.Anticipo_Factura_Id);
                            obj.Anticipo_Id = parseInt(row.Anticipo_Id);
                            obj.Anticipo = row.Anticipo;
                            obj.Factura_Id = parseInt(row.Factura_Id);
                            obj.Monto = parseFloat(row.Monto);

                            //  se convierte la información a json
                            var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                            //  se ejecuta la petición
                            $.ajax({
                                type: 'POST',
                                url: 'controllers/Facturas_Controller.asmx/Eliminar_Anticipo',
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
                                            _consultar_informacion_anticipos();

                                        } else {//  si la acción marco un error

                                            //  se muestra el mensaje del error que se presento
                                            _mostrar_mensaje(result.Titulo, result.Mensaje);
                                        }
                                    }
                                }
                            });

                        }
                            //  se realizara de forma local la accion de eliminar
                        else {
                            //  se elimina el registro dentro de la tabla
                            $('#tbl_facturas_anticipos').bootstrapTable('remove', {
                                field: 'Anticipo_Id',
                                values: [row.Anticipo_Id],

                                field: 'Monto',
                                values: [row.Monto],
                            });
                        }

                     
                    }
                }
            });
        }
            //  indicamos que no tiene permisos para realizar la accion
        else {
            _mostrar_mensaje('Validación', 'No tiene permisos para realizar esta acción');
        }

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_anticipo_click] ', e);
    }
}

