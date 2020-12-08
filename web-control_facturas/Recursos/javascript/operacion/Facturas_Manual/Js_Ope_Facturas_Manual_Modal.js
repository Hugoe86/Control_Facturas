
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_(title_window);

    //  se muestra el modal
    jQuery('#modal_factura').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
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
--FECHA_CREO:           14 Abril de 2020
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
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal() {
    //  cierra el modal
    jQuery('#modal_factura').modal('hide');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_informacion
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_informacion() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        filtros.Folio = "";

        //  filtro para la cuenta
        if ($.trim($('#cmb_busqueda_nombre :selected').val()) !== '') {
            filtros.Folio_Cheque = parseInt($('#cmb_busqueda_nombre :selected').val());
        }
        //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Folio_Cheque = 0;
        }


        //  filtro para el estatus
        if ($.trim($('#cmb_busqueda_estatus :selected').val()) !== '') {
            filtros.Estatus = ($('#cmb_busqueda_estatus :selected').val());
        }

        filtros.Filtro_Captura_Manual = true;

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador


        ////  validamos que sea el area de captura
        if (Permiso_Captura) { //Validamos que tenga permiso de capturar facturas y poder consultar.
            //  se realiza la petición
            jQuery.ajax({
                type: 'POST',
                url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Facturas_Por_Folio_Filtros',
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
                        $('#tbl_facturas').bootstrapTable('load', JSON.parse(datos.d));
                    }
                }
            });
        }
        //  validamos que sea una area que se encarga de validar
        else {
            _mostrar_mensaje('Consulta de facturas', 'No cuentas con el permiso de capturar y consultar facturas.');
        }

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_informacion]', e);
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_fecha_revision
--DESCRIPCIÓN:          Se inicializa el control de las fechas
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           29 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_fecha_revision() {

    //  se carga el control
    $('#dtp_txt_fecha_recepcion').datetimepicker({
        defaultDate: new Date(),
        viewMode: 'days',
        locale: 'es',
        format: "DD/MM/YYYY"
    });

    //  se establece el formato
    $("#dtp_txt_fecha_recepcion").datetimepicker("useCurrent", true);


    //  se carga el control
    $('#dtp_txt_fecha_factura').datetimepicker({
        defaultDate: new Date(),
        viewMode: 'days',
        locale: 'es',
        format: "DD/MM/YYYY"
    });

    //  se establece el formato
    $("#dtp_txt_fecha_factura").datetimepicker("useCurrent", true);

    //  se carga el control
    $('#dtp_txt_fecha_pago').datetimepicker({
        defaultDate: new Date(),
        viewMode: 'days',
        locale: 'es',
        format: "DD/MM/YYYY"
    });

    //  se establece el formato
    $("#dtp_txt_fecha_pago").datetimepicker("useCurrent", true);

}

/* =============================================
--NOMBRE_FUNCIÓN:       lectura_anticipos_registrados
--DESCRIPCIÓN:          consulta los anticipos registrados relacionados con el folio
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function lectura_anticipos_registrados() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = 0;
        obj.Folio = $('#txt_folio_solicitud_cheque').val();

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador


        //  se ejecuta la peticion
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Total_Anticipos_Por_Factura_Manuales',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null || datos != "") {

                    var result = JSON.parse(datos.d);// almacena la información recibida

                    $('#txt_anticipos_capturadas').html(result).formatCurrency();
                }
            }
        });



    } catch (e) {
        alert(e.message)
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       lectura_pagos_registrados
--DESCRIPCIÓN:          consulta los pagos registrados relacionados con el folio
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function lectura_pagos_registrados() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = 0;
        obj.Folio = $('#txt_folio_solicitud_cheque').val();

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador


        //  se ejecuta la peticion
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Total_Pagos_Por_Factura_Manuales',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null && datos != "") {

                    var result = JSON.parse(datos.d);// almacena la información recibida

                    $('#txt_cuentas_capturadas').html(result).formatCurrency();
                }
            }
        });

    } catch (e) {
        alert(e.message)
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       lectura_montos_registados_manualmente
--DESCRIPCIÓN:          consulta los totales de la factura registrados relacionados con el folio
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function lectura_montos_registados_manualmente() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = 0;
        obj.Folio = $('#txt_folio_solicitud_cheque').val();

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador


        //  se ejecuta la peticion
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_SubTotal_Registrado_Manualmente',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null && datos != "") {

                    var result = JSON.parse(datos.d);// almacena la información recibida

                    $('#txt_subtotal_factura_xml').html(result).formatCurrency();
                }
            }
        });

        //  se ejecuta la peticion
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Total_Registrado_Manualmente',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null && datos != "") {

                    var result = JSON.parse(datos.d);// almacena la información recibida

                    $('#txt_total_factura_xml').html(result).formatCurrency();
                }
            }
        });

    } catch (e) {
        alert(e.message)
    }

}
/* =============================================
--NOMBRE_FUNCIÓN:       calcular_diferencia_montos_factura
--DESCRIPCIÓN:          consulta los anticipos registrados relacionados con el folio
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function calcular_diferencia_montos_factura() {
    var _subtotal_factura = 0;// variable para contener el subtotal de la factura
    var _total_factura = 0;// variable para contener el total de la factura
    var _subtotal_registrado = 0;// variable para contener el subtotal
    var _total_registrado = 0;// variable para contener el total
    var diferencia = 0;//   variable para obtener la diferencia de los montos
    var diferencia_resta = 0;//   variable para obtener la diferencia de los montos


    try {


        _subtotal_factura = parseFloat(_replace_money($('#txt_subtotal_factura_xml').html()));
        _total_factura = parseFloat(_replace_money($('#txt_total_factura_xml').html()));
        _subtotal_registrado = parseFloat(_replace_money($('#txt_cuentas_capturadas').html()));
        _total_registrado = parseFloat(_replace_money($('#txt_anticipos_capturadas').html()));

        //  se realiza el calculo de la diferencia
        diferencia_resta = parseFloat(_subtotal_registrado + _total_registrado);
        diferencia = parseFloat(_total_factura).toFixed(2) - parseFloat(diferencia_resta).toFixed(2);


        $('#txt_diferencia_validacion_factura').html(diferencia).formatCurrency();

    } catch (e) {
        alert(e.message)
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       crear_conceptos_xml_registrados
--DESCRIPCIÓN:          Guarda el archivo en la base de datos
--PARÁMETROS:           Operacion: valor numerico con el que se establece si es consulta o insercion (0 consulta - 1 insercion)
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_conceptos_xml_registrados(operacion) {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = 0;
        obj.Folio = $('#txt_folio_solicitud_cheque').val();

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la peticion
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Conceptos_Registrados_Manualmente_Filtros',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos si tiene alguna información la variable
                if (datos != null) {

                    var objetos_concepto;// variable para obtener los controles que tengan el concepto registrado
                    var result = JSON.parse(datos.d);// almacena la información recibida
                    var estatus = false;//  variable para almacenar si ya existe el concepto registrado


                    var estructura_html = '';// variable en la que se mostraran los conceptos del archivo xml
                    var Cont_For = 0;// variable para llevar el id de los botones que se estaran agregando

                    //  se muestran los conceptos del XML
                    var conceptos = JSON.parse(datos.d);//   variable en la que se cargara la informacion recibida de los conceptos del XML


                    //  se recorren los conceptos
                    for (Cont_For = 0; Cont_For < conceptos.length; Cont_For++) {

                        estructura_html += '<li class="item-list-success">';
                        estructura_html += '    <a href="javascript:void(0)" data-orden=\'' + JSON.stringify(conceptos[Cont_For]) + '\' onclick="btn_seleccionar_concepto_click(this);">';
                        estructura_html += '        <span>' + conceptos[Cont_For].Concepto_Xml + '<br />$' + formatNumber(conceptos[Cont_For].Total_Pagar) + '</span>';
                        estructura_html += '    </a>';
                        estructura_html += '</li>';
                    }

                    //  se cargan los botones
                    $('#div_conceptos_xml').html();
                    $('#div_conceptos_xml').html(estructura_html);

                    //  validamos que sea operacion = 0 (consulta)
                    if (operacion == 0) {
                        //  validamos si ya estan registrados
                        if (estatus == true) {
                            _mostrar_mensaje('Información', 'Ya existen registros capturados con ese folio y pedimento');
                        }
                    }


                }
            }
        });

    } catch (e) {
        alert(e.message)
    }

}

/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_entidad
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_entidad(cmb) {
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
                url: '../Catalogos/controllers/Cuentas_Controller.asmx/Consultar_Entidades_Combo',
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
                        cuenta_id: $('#cmb_cuenta :selected').val()// variable para filtrar los datos relacionados con la entidad
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
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_entidad]', e);
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
                url: '../Catalogos/controllers/Cuentas_Controller.asmx/Consultar_Relacion_Entidad_Cuentas_Combo',
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
        --DESCRIPCIÓN:          Habilita la seccion de las cuentas
        --PARÁMETROS:           evt: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#' + cmb).on("select2:select", function (evt) {

            $('#cmb_entidad').prop('disabled', false);

        });


        /* =============================================
        --NOMBRE_FUNCIÓN:       $(cmb).on("select2:select", function (evt) {
        --DESCRIPCIÓN:          Bloquea el combo de cuentas
        --PARÁMETROS:           evt: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#' + cmb).on("select2:unselect", function (evt) {

            $("#cmb_entidad").empty().trigger("change");

            $('#cmb_entidad').attr('disabled', 'disabled');


        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_cuentas]', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _guardar_archivo_directorio
--DESCRIPCIÓN:          Guarda el archivo dentro de una carpeta temporal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _guardar_archivo_directorio(data) {
    var estatus = false;//  variable para establecer el estatus de la accion realizada
    var resultado = '';//   variable para indicar el resultado obtenido
    try {
        $.ajax({
            type: "POST",
            url: "../../FileUploadHandler.ashx",
            contentType: false,
            processData: false,
            data: data,
            async: false,
            success: function (result) {
                resultado = JSON.parse(result);
                //  validamos que tenga informacion recibida
                if (result) {
                    estatus = true;
                }
            }
        });
    } catch (e) {
        estatus = false;
        _mostrar_mensaje('Informe técnico', e);
    }
    return resultado;
}



/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_modal
--DESCRIPCIÓN:          Crea los eventos de los botones que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
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
        --FECHA_CREO:           14 Abril de 2020
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

                //  validamos si la información es correcta
                if (valida_montos_requerido.Estatus) {

                    //  validamos si es un nuevo registro, en caso contrario sera una actualización
                    if (title == "Guardar") {
                        //  se ejecuta el evento de alta
                        alta()
                    }
                    else {
                        //  se ejecuta el evento de actualización
                        actualizar('salir');
                    }

                }
            }
            //  se muestra el mensaje de error
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
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_cancelar').on('click', function (e) {
            e.preventDefault();

            //  limpia los controles del modal
            _limpiar_todos_controles_modal();

            //  se consultan la informacion
            _consultar_informacion();

            //  se muestra la seccion principal
            _mostrar_vista('Principal');

            $('#cmb_entidad').attr('disabled', 'disabled');

        });

        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_agregar_cuenta
        --DESCRIPCIÓN:          Agrega una cuenta a la tabla de cuentas
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_agregar_cuenta').on('click', function (e) {
            e.preventDefault();

            var accion = true;//    almacenara si esta repetido alguna cuenta
            var relacion_id = $('#cmb_entidad :selected').val();// almacenara el id de la relacion
            var cuenta = $('#cmb_cuenta :selected').text();// almacenara el nombre de la cuenta

            var entidad = $('#cmb_entidad :selected').text();// almacenara el nombre de la entidad
            var monto = $('#txt_monto_pago').val();// almacenara el monto ingresado

            //  validación para seleccionar un empleado
            if (cuenta === undefined || cuenta == "") {
                _mostrar_mensaje("Validación de cuenta", "Seleccione una cuenta")
                return;
            }

            //  se obtiene el total del numero de registros dentro de la tabla
            var total_row = $('#tbl_facturas_cuentas').bootstrapTable('getOptions').totalRows;//   almacena el total de registros

            //  se asignan los datos de la tabla
            var tbl_detalles = $('#tbl_facturas_cuentas').bootstrapTable('getData');//   almacena los datos de la tabla 

            //  se incrementa el indice
            total_row = total_row + 1;


            /* =============================================
            --NOMBRE_FUNCIÓN:       $.each(tbl_detalles, function (index, value) {
            --DESCRIPCIÓN:          se recorre la tabla
            --PARÁMETROS:           NA
            --CREO:                 Hugo Enrique Ramírez Aguilera
            --FECHA_CREO:           14 Abril de 2020
            --MODIFICÓ:
            --FECHA_MODIFICÓ:
            --CAUSA_MODIFICACIÓN:
            =============================================*/
            //  se recorre la tabla
            $.each(tbl_detalles, function (index, value) {

                //  validamos que sea el mismo id
                if (value.relacion_id == relacion_id) {
                    //  se asigna que acción se debe de ejecutar
                    accion = false;
                }
            });


            //  si la acción es verdadera se insertara como un nuevo registro
            if (accion == true) {

                //  se inserta el nuevo valor a la tabla
                $('#tbl_facturas_cuentas').bootstrapTable('insertRow', {
                    index: total_row,
                    row: {
                        Pago_Id: 0,
                        Relacion_Id: relacion_id,
                        Cuenta: cuenta,
                        Entidad: entidad,
                        Monto: monto,
                    }

                });


                $('#cmb_entidad').empty().trigger("change");
                $('#txt_monto_pago').val('');

                //  se calculan los totales
                calcular_totales_pagos_anticipos();
            }
            else {
                //  se indica que el usuario ya fue ingresado
                _mostrar_mensaje("Validación", "La cuante ya se encuentra registrado");
            }

        });



        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_agregar_anticipo
        --DESCRIPCIÓN:          Agrega un anticipo a la tabla
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_agregar_anticipo').on('click', function (e) {
            e.preventDefault();

            var accion = true;//    almacenara si esta repetido alguna cuenta
            var anticipo_id = $('#cmb_anticipo :selected').val();// almacenara el id
            var anticipo = $('#cmb_anticipo :selected').text();// almacenara el nombre del anticipo
            var monto = $('#txt_monto_anticipo').val();// almacenara el monto ingresado
            var saldo = $('#txt_saldo_anticipo').val();// almacenara el saldo disponible
            var fecha = $('#txt_fecha_anticipo').val();// almacenara el monto ingresado
            var saldo_float = 0;// almacenara el saldo ingresado
            var monto_float = 0;// almacenara el monto ingresado

            //  validación para seleccionar un empleado
            if (anticipo_id === undefined || anticipo_id == "") {
                _mostrar_mensaje("Validación", "Seleccione un anticipo")
                return;
            }

            //  validación para seleccionar un empleado
            if (saldo === undefined || saldo == "") {
                _mostrar_mensaje("Validación", "Ingrese el monto")
                return;
            }


            saldo_float = (parseFloat($('#txt_saldo_anticipo').val()));
            monto_float = (parseFloat($('#txt_monto_anticipo').val()));


            //  validaciónel monto contra el saldo
            if (monto_float > saldo_float) {
                _mostrar_mensaje("Validación", "El monto no puede ser mayor al saldo del anticipo")
                return;
            }
           

            //  validación para seleccionar un empleado
            if (fecha === undefined || fecha == "") {
                _mostrar_mensaje("Validación", "Ingrese la fecha")
                return;
            }


            //  se obtiene el total del numero de registros dentro de la tabla
            var total_row = $('#tbl_facturas_anticipos_operacion').bootstrapTable('getOptions').totalRows;//   almacena el total de registros

            //  se asignan los datos de la tabla
            var tbl_detalles = $('#tbl_facturas_anticipos_operacion').bootstrapTable('getData');//   almacena los datos de la tabla 

            //  se incrementa el indice
            total_row = total_row + 1;


            /* =============================================
            --NOMBRE_FUNCIÓN:       $.each(tbl_detalles, function (index, value) {
            --DESCRIPCIÓN:          se recorre la tabla
            --PARÁMETROS:           NA
            --CREO:                 Hugo Enrique Ramírez Aguilera
            --FECHA_CREO:           14 Abril de 2020
            --MODIFICÓ:
            --FECHA_MODIFICÓ:
            --CAUSA_MODIFICACIÓN:
            =============================================*/
            //  se recorre la tabla
            $.each(tbl_detalles, function (index, value) {

                //  validamos que sea el mismo id
                if (value.Anticipo_Id == anticipo_id) {
                    //  se asigna que acción se debe de ejecutar
                    accion = false;
                }
            });


            //  si la acción es verdadera se insertara como un nuevo registro
            if (accion == true) {

                //  se inserta el nuevo valor a la tabla
                $('#tbl_facturas_anticipos_operacion').bootstrapTable('insertRow', {
                    index: total_row,
                    row: {
                        Anticipo_Factura_Id: 0,
                        Anticipo_Id: anticipo_id,
                        Anticipo: anticipo,
                        Fecha: Date.parse(fecha),
                        Fecha_Texto: fecha,
                        Monto: monto,
                    }

                });

                //  se limpian los valores
                $('#cmb_anticipo').empty().trigger("change");
                $('#txt_monto_anticipo').val('');
                $('#txt_saldo_anticipo').val('');
                $('#txt_fecha_anticipo').val('');

                //  se calculan los totales
                calcular_totales_pagos_anticipos();
            }
            else {
                //  se indica que el usuario ya fue ingresado
                _mostrar_mensaje("Validación", "El anticipo ya se encuentra registrado");
            }

        });



        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_guardar_seguir_captura
        --DESCRIPCIÓN:          Evento con el que se valida y guarda la información
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_guardar_seguir_captura').on('click', function (e) {
            e.preventDefault();

            //  variables
            var title = $('#btn_guardar_seguir_captura').attr('title');//  almacenara el titulo que tiene el botón de nuevo
            var valida_datos_requerido = _validarDatos_nuevo();// almacenara el resultado de la validación de los datos


            //  validamos si la información es correcta
            if (valida_datos_requerido.Estatus) {

                var valida_montos_totales = _validar_monto_ingresado_pago_anticipo_contra_totalesConcepto();// almacenara el resultado de la validación de los datos


                //  validamos si la información es correcta
                if (valida_montos_totales.Estatus) {

                    //  validamos si es un nuevo registro, en caso contrario sera una actualización
                    if (title == "Guardar y seguir") {
                        //  se ejecuta el evento de alta
                        alta();
                    }
                    else if (title == "Editar y seguir") {
                        //  se ejecuta el evento de actualización
                        actualizar('seguir');
                    }
                } else { //  se muestra el mensaje del error que se presento
                    _mostrar_mensaje('Información' + '', valida_montos_totales.Mensaje);
                }

            }
            //  se muestra el mensaje de error
            else {
                //  se muestra el mensaje del error que se presento
                _mostrar_mensaje('Información' + '', valida_datos_requerido.Mensaje);
            }

        });



        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_guardar_salir_captura
        --DESCRIPCIÓN:          Evento con el que se valida y guarda la información
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_guardar_salir_captura').on('click', function (e) {
            e.preventDefault();

            //  variables
            var title = $('#btn_guardar_salir_captura').attr('title');//  almacenara el titulo que tiene el botón de nuevo
            var valida_datos_requerido = _validarDatos_nuevo();// almacenara el resultado de la validación de los datos


            //  validamos si la información es correcta
            if (valida_datos_requerido.Estatus) {

                var valida_montos_totales = _validar_monto_ingresado_pago_anticipo_contra_totalesConcepto();// almacenara el resultado de la validación de los datos

                //  validamos si la información es correcta
                if (valida_montos_totales.Estatus) {

                    //  validamos si es un nuevo registro, en caso contrario sera una actualización
                    if (title == "Guardar y salir") {
                        //  se ejecuta el evento de alta
                        alta_salir()
                    }
                    else if (title == "Editar y salir") {
                        //  se ejecuta el evento de actualización
                        actualizar('salir');
                    }
                } else { //  se muestra el mensaje del error que se presento
                    _mostrar_mensaje('Información' + '', valida_montos_totales.Mensaje);
                }
            }
            //  se muestra el mensaje de error
            else {
                //  se muestra el mensaje del error que se presento
                _mostrar_mensaje('Información' + '', valida_datos_requerido.Mensaje);
            }

        });


        /* =============================================
       --NOMBRE_FUNCIÓN:       btn_cancelar_captura
       --DESCRIPCIÓN:          Evento con el que se cancela la acción de nuevo o actualización
       --PARÁMETROS:           e: parametro que se refiere al evento click
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           14 Abril de 2020
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#btn_cancelar_captura').on('click', function (e) {
            e.preventDefault();

            //  limpia los controles del modal
            _limpiar_todos_controles_modal();

            //  se consultan la informacion
            _consultar_informacion();

            //  se muestra la seccion principal
            _mostrar_vista('Principal');

            $('#cmb_entidad').attr('disabled', 'disabled');

        });


        /* =============================================
        --NOMBRE_FUNCIÓN:       txt_filtrar_lista_conceptos
        --DESCRIPCIÓN:          Evento para filtrar la lista de los conceptos
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           13 Noviembre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $("#txt_filtrar_lista_conceptos").on("keyup", function (e) {
            e.preventDefault();
            var filter = this.value.toLowerCase();//    variable para recibir el texto del control

            /* =============================================
            --NOMBRE_FUNCIÓN:       $('#div_conceptos_xml').find("li").each(function (index, li)
            --DESCRIPCIÓN:          Recorre las etiquetas li de la lista de conceptos
            --PARÁMETROS:           NA
            --CREO:                 Hugo Enrique Ramírez Aguilera
            --FECHA_CREO:           09 Octubre de 2019
            --MODIFICÓ:
            --FECHA_MODIFICÓ:
            --CAUSA_MODIFICACIÓN:
            =============================================*/
            $('#div_conceptos_xml').find("li").each(function (index, li) {

                //  validamos que no contenga ningun texto la palabre filtradas
                if (filter == "") {
                    li.style["display"] = "list-item";
                }
                //  si contiene texto se oculta el control
                else if (!li.textContent.toLowerCase().match(filter)) {
                    li.style["display"] = "none";
                }
                //  se muestra el control
                else {
                    li.style["display"] = "list-item";
                }
            });

        });


        /* =============================================
        --NOMBRE_FUNCIÓN:       $("#txt_subtotal").on("keyup", function (e)
        --DESCRIPCIÓN:          Evento para calcular el total del concepto
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           13 Noviembre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $("#txt_subtotal").on("keyup", function (e) {
            e.preventDefault();

            //  se calculan los totales
            _calcular_total_montos_conceptos();

        });


        /* =============================================
        --NOMBRE_FUNCIÓN:         $("#txt_iva").on("keyup", function (e) {
        --DESCRIPCIÓN:          Evento para calcular el iva
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           13 Noviembre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $("#txt_iva").on("keyup", function (e) {
            e.preventDefault();

            //  se calculan los totales
            _calcular_total_montos_conceptos();

        });


        /* =============================================
        --NOMBRE_FUNCIÓN:       $("#txt_retencion").on("keyup", function (e)
        --DESCRIPCIÓN:          Evento para calcular el total de la retencion
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           13 Noviembre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $("#txt_retencion").on("keyup", function (e) {
            e.preventDefault();

            //  se calculan los totales
            _calcular_total_montos_conceptos();

        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_modal] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _calcular_total_montos_conceptos
--DESCRIPCIÓN:          calcula el total del concepto
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _calcular_total_montos_conceptos() {
    try {

        var subtotal_ = 0;//    variable para el obtener el subtotal
        var impuesto_ = 0;//    variable para el obtener el impuesto
        var retencion_ = 0;//    variable para el obtener la retencion
        var total = 0;//    variable para calcular el total 

        subtotal_ = ($('#txt_subtotal').val() === undefined || $('#txt_subtotal').val() === '') ? 0 : parseFloat(_replace_money($("#txt_subtotal").val()));
        impuesto_ = ($('#txt_iva').val() === undefined || $('#txt_iva').val() === '') ? 0 :parseFloat(_replace_money($("#txt_iva").val()));
        retencion_ = ($('#txt_retencion').val() === undefined || $('#txt_retencion').val() === '') ? 0 : parseFloat(_replace_money($("#txt_retencion").val()));

        total = subtotal_ + impuesto_ - retencion_;

        $('#txt_total').html(total).formatCurrency();
        
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_calcular_total_montos_conceptos] ', '' + e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal
--DESCRIPCIÓN:          limpia todos los controles que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal() {

    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       #modal_factura input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_factura input[type=text]').each(function () { $(this).val(''); });

        /* =============================================
       --NOMBRE_FUNCIÓN:       #modal_factura input[type=hidden]
       --DESCRIPCIÓN:          recorre los controles de tipo hidden y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           14 Abril de 2020
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#modal_factura input[type=hidden]').each(function () { $(this).val(''); });


        //  limpia el valor del combo
        $('#cmb_tipo_concepto').empty().trigger("change");
        $('#cmb_folio').empty().trigger("change");
        $('#cmb_entidad').empty().trigger("change");
        $('#cmb_cuenta').empty().trigger("change");

        //  se limpian las tablas
        $('#tbl_facturas_cuentas').bootstrapTable('load', []);
        $('#tbl_facturas_anticipos_operacion').bootstrapTable('load', []);
        $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('load', []);

        $('#div_conceptos_xml').html('');

        $('#txt_moneda').val('USD')

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal] ', 'limpiar controles. ' + e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal_captura_xml
--DESCRIPCIÓN:          limpia todos los controles que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal_captura_xml() {

    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       #modal_factura input[type=text][opcion=captura]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor con la propiedad de opcion de captura
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_factura input[type=text][opcion=captura]').each(function () { $(this).val(''); });


        /* =============================================
        --NOMBRE_FUNCIÓN:       #Tab_Pago input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#Tab_Pago input[type=text]').each(function () { $(this).val(''); });


        /* =============================================
       --NOMBRE_FUNCIÓN:       #Tab_Pago input[type=text]
       --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           14 Abril de 2020
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#div_totales_concepto input[type=text]').each(function () { $(this).val(''); });

        /* =============================================
        --NOMBRE_FUNCIÓN:       #Tab_Anticipos input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#Tab_Anticipos input[type=text]').each(function () { $(this).val(''); });


        //  limpia el valor del combo
        $('#cmb_entidad').empty().trigger("change");
        $('#cmb_cuenta').empty().trigger("change");

        //  se limpian las tablas
        $('#tbl_facturas_cuentas').bootstrapTable('load', JSON.parse('[]'));
        $('#tbl_facturas_anticipos_operacion').bootstrapTable('load', JSON.parse('[]'));
        $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('load', JSON.parse('[]'));



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal_captura_xml] ', 'limpiar controles. ' + e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_estatus
--DESCRIPCIÓN:          Carga los datos del estatus del combo
--PARÁMETROS:           cmb: nombre del control al cual se le asignaran los valores
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
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
--NOMBRE_FUNCIÓN:       _load_cmb_facturas_general_mills
--DESCRIPCIÓN:          Carga la información de la base de datos de las tablas de general mills dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_facturas_general_mills(cmb) {
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
                url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Facturas_General_Mills',
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
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#' + cmb).on("select2:select", function (evt) {
            _limpiar_datos_captura_folio();
            _limpiar_pagos_y_anticipos();
            var _dato_combo = evt.params.data;// variable para obtener la estructura del combo
            var obj = new Object();//   estructura en donde se cargaran la información del formulario


            //  se carga la información 
            obj.Folio = _dato_combo.id;

            //  se convierte a la estructura que pueda leer el controlador
            var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

            //-------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------------
            //  se ejecuta la peticion
            jQuery.ajax({
                type: 'POST',
                url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Datos_Factura_General_Mills',
                data: $data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                cache: false,
                success: function (datos) {

                    //  validamos que tenga informacion
                    if (datos !== null) {

                        var resultado = JSON.parse(datos.d);//  variable en la que se almacenaran los datos recibidos

                        $('#txt_concepto').val(resultado[0].Concepto.trim());
                        $('#txt_rfc').val(resultado[0].Rfc.trim());
                        //$('#txt_referencia_interna').val(resultado[0].Referencia);
                        $('#txt_fecha_factura').val(resultado[0].Fecha_Recepcion_Texto);
                        $('#txt_moneda').val(resultado[0].Moneda);
                        //$('#txt_uuid').val(resultado[0].UUID.trim());
                        $('#txt_referencia_pago').val(resultado[0].Referencia_Pago);
                        $('#txt_fecha_pago').val(resultado[0].Fecha_Pago_Texto);

                        $('#txt_folio').val(resultado[0].Folio.trim());
                        $('#txt_razon_social').val(resultado[0].Razon_Social.trim());
                        $('#txt_pedimento').val(resultado[0].Pedimento);
                        $('#txt_fecha_recepcion').val(resultado[0].Fecha_Recepcion);
                        $('#txt_referencia_interna').val(resultado[0].Referencia_Interna);



                    }
                }
            });



        });

        ///* =============================================
        //--NOMBRE_FUNCIÓN:       $(cmb).on("select2:unselect", function (evt) 
        //--DESCRIPCIÓN:          Limpia los controles dentro del modal
        //--PARÁMETROS:           evt: parametro que se refiere al evento click
        //--CREO:                 Hugo Enrique Ramírez Aguilera
        //--FECHA_CREO:           14 Abril de 2020
        //--MODIFICÓ:
        //--FECHA_MODIFICÓ:
        //--CAUSA_MODIFICACIÓN:
        //=============================================*/
        //$("#" + cmb).on("select2:unselect", function (evt) {
        //    _limpiar_datos_captura_folio();
        //    _limpiar_pagos_y_anticipos();          
        //});


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_filtro_nombre]', e);
    }
}




/* =============================================
--NOMBRE_FUNCIÓN:       _cargar_datos_principales_
--DESCRIPCIÓN:          Carga la información de la base de datos de las tablas de general mills dentro de los controles
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cargar_datos_principales_(folio) {
    try {
        var obj = new Object();//   estructura en donde se cargaran la información del formulario


        //  se carga la información 
        obj.Folio_Cheque = parseInt(folio);

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------
        //  se ejecuta la peticion
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Datos_FolioCheque_Factura_General_Mills',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que tenga informacion
                if (datos !== null) {

                    var resultado = JSON.parse(datos.d);//  variable en la que se almacenaran los datos recibidos

                    $('#txt_concepto').val(resultado[0].Concepto.trim());
                    $('#txt_rfc').val(resultado[0].Rfc.trim());
                    //$('#txt_referencia_interna').val(resultado[0].Referencia);
                    $('#txt_fecha_factura').val(resultado[0].Fecha_Recepcion_Texto);
                    $('#txt_moneda').val(resultado[0].Moneda);
                    //$('#txt_uuid').val(resultado[0].UUID.trim());
                    $('#txt_referencia_pago').val(resultado[0].Referencia_Pago);
                    $('#txt_fecha_pago').val(resultado[0].Fecha_Pago_Texto);

                    $('#txt_folio').val(resultado[0].Folio.trim());
                    $('#txt_pedimento').val(resultado[0].Pedimento);
                    $('#txt_referencia_interna').val(resultado[0].Referencia_Interna);
                    $('#txt_fecha_recepcion').val(resultado[0].Fecha_Recepcion);

                    //  se carga el folio
                    $('#cmb_folio').select2("trigger", "select", {
                        data: { id: resultado[0].Folio.trim(), text: resultado[0].Folio.trim() }
                    });


                }
            }
        });
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_cargar_datos_principales_]', e);
    }
}




/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_conceptos_catalogo
--DESCRIPCIÓN:          Carga la información de la base de datos de las tablas de general mills dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_conceptos_catalogo(cmb) {
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
                url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Tipos_Conceptos',
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
                        filtro_manual: "1",
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
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_filtro_nombre]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _validarDatos_nuevo
--DESCRIPCIÓN:          Se valida la información requerida para realizar una acción
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
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


        //  filtro para el tipo de concepto (combo)
        if ($('#cmb_tipo_concepto :selected').val() === undefined || $('#cmb_tipo_concepto :selected').val() === '') {
            //  agregamos el estilo al control que no cumple
            _add_class_error("cmb_tipo_concepto");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El tipo de concepto.<br />';
        }


        //  filtro para el folio (combo)
        if ($('#txt_folio').val() == '' || $('#txt_folio').val() == undefined) {
            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_folio");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El folio.<br />';
        }


        //  filtro para la fecha de recepcion
        if ($('#txt_fecha_recepcion').val() == '' || $('#txt_fecha_recepcion').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_fecha_recepcion");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La fecha de recepción.<br />';
        }



        //  filtro para el pedimento
        if ($('#txt_pedimento').val() == '' || $('#txt_pedimento').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_pedimento");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El pedimento.<br />';
        }


        ////  filtro para el concepto
        //if ($('#txt_concepto').val() == '' || $('#txt_concepto').val() == undefined) {

        //    //  agregamos el estilo al control que no cumple
        //    _add_class_error("txt_concepto");

        //    //  se indica el estatus como no cumplido
        //    _output.Estatus = false;

        //    //  se asigna el mensaje que se mostrara
        //    _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El concepto.<br />';
        //}

        //  filtro para la fecha de recepcion
        if ($('#txt_razon_social').val() == '' || $('#txt_razon_social').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_razon_social");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La razón social.<br />';
        }

        //  filtro para la fecha de recepcion
        if ($('#txt_fecha_factura').val() == '' || $('#txt_fecha_factura').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_fecha_factura");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La fecha de la factura.<br />';
        }


        //  filtro para la fecha de recepcion
        if ($('#txt_concepto_xml').val() == '' || $('#txt_concepto_xml').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_concepto_xml");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El concpeto.<br />';
        }


        //  filtro para la fecha de recepcion
        if ($('#txt_rfc').val() == '' || $('#txt_rfc').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_rfc");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El RFC de la factura.<br />';
        }


        //  filtro para el concepto
        if ($('#txt_referencia_interna').val() == '' || $('#txt_referencia_interna').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_referencia_interna");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La referencia interna.<br />';
        }



        //  filtro para la fecha de recepcion
        if ($('#txt_moneda').val() == '' || $('#txt_moneda').val() == undefined) {

            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_moneda");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La moneda.<br />';
        }


        //  filtro para el subtotal
        if ($('#txt_subtotal').val() == '' || $('#txt_subtotal').val() == undefined) {

            //  agregamos el estilo al txt_subtotal que no cumple
            _add_class_error("txt_subtotal");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El subtotal.<br />';
        }


        

        //  filtro para el total        
        if (_replace_money($('#txt_total').html()) == '' || _replace_money($('#txt_total').html()) == undefined) {

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El total.<br />';
        }
        else {
            var total = 0; //   variable para calcular si el total del concepto es mayor a cero
            total = parseFloat(_replace_money($("#txt_total").val()));

            //  validamos el monto del total
            if (total <= 0) {

                //  se indica el estatus como no cumplido
                _output.Estatus = false;

                //  se asigna el mensaje que se mostrara
                _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;El total no puede ser cero.<br />';
            }

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
--NOMBRE_FUNCIÓN:       round
--DESCRIPCIÓN:          Redondel un valor numerico
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function round(numero, decimales = 2) {
    var signo = (numero >= 0 ? 1 : -1);//  varaible para asignar el elemento que estara redondeado
    var resultado = 0;//    variable para el resultado
    numero = numero * signo;

    //  validamos que no sea cero
    if (decimales === 0) //con 0 decimales
        return signo * Math.round(numero);
    
    numero = numero.toString().split('e');
    numero = Math.round(+(numero[0] + 'e' + (numero[1] ? (+numero[1] + decimales) : decimales)));
    numero = numero.toString().split('e');

    resultado = signo * (numero[0] + 'e' + (numero[1] ? (+numero[1] - decimales) : -decimales));
    return resultado;
}

/* =============================================
--NOMBRE_FUNCIÓN:       descontar_montos_registrados
--DESCRIPCIÓN:          Quita los totales registrados del concepto al total de toda la factura
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function descontar_montos_registrados() {

    var _pagos_registrado = 0;// variable para contener el total de los anticipos
    var _anticipos_registrados = 0;//   variable para contener el total del registro

    var _pagos = 0;// variable para contener el total de los anticipos
    var _anticipos = 0;//   variable para contener el total del registro
    var operacion_pagos = 0;//  variable para realizar la operacion de los pagos
    var operacion_anticipos = 0;//  variable para realizar la operacion de los pagos

    try {

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------
        _pagos_registrado = parseFloat(_replace_money($('#txt_cuentas_capturadas').html()));
        _anticipos_registrados = parseFloat(_replace_money($('#txt_anticipos_capturadas').html()));


        _pagos = parseFloat(_replace_money($('#txt_total_pagos').html()));
        _anticipos = parseFloat(_replace_money($('#txt_total_anticipo').html()));


        operacion_anticipos = _anticipos_registrados - _anticipos;
        operacion_pagos = _pagos_registrado - _pagos;



        $('#txt_cuentas_capturadas').html(operacion_pagos).formatCurrency();
        $('#txt_anticipos_capturadas').html(operacion_anticipos).formatCurrency();

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[descontar_montos_registrados]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       alta
--DESCRIPCIÓN:          se registra el alta
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function alta() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = 0;
        obj.Concepto_Id = $('#cmb_tipo_concepto :selected').val();
        obj.Folio = $('#txt_folio').val();
        obj.Fecha_Recepcion = Date.parse($('#txt_fecha_recepcion').val());
        obj.Fecha_Factura = Date.parse($('#txt_fecha_factura').val());
        obj.Rfc = $('#txt_rfc').val();
        obj.Razon_Social = $('#txt_razon_social').val();
        obj.Referencia_Interna = $('#txt_referencia_interna').val();
        obj.Referencia = $('#txt_referencia').val();
        obj.Pedimento = $('#txt_pedimento').val();
        obj.Concepto = $('#txt_concepto_xml').val();
        obj.Concepto_Xml = $('#txt_concepto_xml').val();
        obj.Moneda = $('#txt_moneda').val();
        obj.Subtotal = (_replace_money($('#txt_subtotal').val()) !== '' || _replace_money($('#txt_subtotal').val()) == undefined) ? parseFloat(_replace_money($('#txt_subtotal').val())) : 0;
        obj.IVA = ($('#txt_iva').val() !== '' || $('#txt_iva').val() == undefined) ? parseFloat(_replace_money($('#txt_iva').val())) : 0;
        obj.Retencion = ($('#txt_retencion').val() !== '' || $('#txt_retencion').val() == undefined) ? parseFloat(_replace_money($('#txt_retencion').val())) : 0;
        obj.Total_Pagar = _replace_money($('#txt_total').html());
        obj.UUID = '';
        obj.Entidad_Id = $('#cmb_entidad :selected').val();

        obj.Nombre_Xml ='';
        obj.Tipo_Xml = $('#txt_tipo_archivo_xml').val();

        //  validamos que este seleccionado como folio ingresa
        if ($('#txt_folio_solicitud_cheque').val() == '' || $('#txt_folio_solicitud_cheque').val() == undefined) {
            obj.Folio_Cheque = 0;
        }
        //  se le asigna la variable como null
        else {
            obj.Folio_Cheque = parseInt($('#txt_folio_solicitud_cheque').val());
        }

        obj.Referencia_Pago = $('#txt_referencia_pago').val();
        obj.Fecha_Pago_Proveedor = Date.parse($('#txt_fecha_pago').val());

        obj.Tabla_Cuentas = JSON.stringify($('#tbl_facturas_cuentas').bootstrapTable('getData'));
        obj.Tabla_Anticipos = JSON.stringify($('#tbl_facturas_anticipos_operacion').bootstrapTable('getData'));

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Alta',
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

                        var folio = "";

                        folio = $('#txt_folio').val();

                        ////  se limpia el modal
                        _limpiar_todos_controles_modal_captura_xml();
                        _limpiar_importes_conceptos();

                        //  se carga el folio
                        $('#cmb_folio').select2("trigger", "select", {
                            data: { id: folio, text: folio }
                        });

                        _habilitar_controles_folio('Mostrar');
                        _habilitar_controles_folio('Texto');

                        //  se validan los conceptos utilizados
                        crear_conceptos_xml_registrados(1);

                        //  se asigna el valor del numero de folio de la solicitud de cheque
                        $('#txt_folio_solicitud_cheque').val(result.Folio_Cheque);
                        $('#txt_folio_asignado').val(result.Folio_Cheque);

                        lectura_montos_registados_manualmente();
                        lectura_pagos_registrados();
                        lectura_anticipos_registrados();
                        calcular_diferencia_montos_factura();


                      

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
--NOMBRE_FUNCIÓN:       alta_salir
--DESCRIPCIÓN:          se registra el alta y se cierra el modal de operacion
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function alta_salir() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = 0;
        obj.Concepto_Id = $('#cmb_tipo_concepto :selected').val();
        obj.Folio = $('#txt_folio').val();
        obj.Fecha_Recepcion = Date.parse($('#txt_fecha_recepcion').val());
        obj.Fecha_Factura = Date.parse($('#txt_fecha_factura').val());
        obj.Rfc = $('#txt_rfc').val();
        obj.Razon_Social = $('#txt_razon_social').val();
        obj.Referencia_Interna = $('#txt_referencia_interna').val();
        obj.Referencia = $('#txt_referencia').val();
        obj.Pedimento = $('#txt_pedimento').val();
        obj.Concepto = $('#txt_concepto_xml').val();
        obj.Concepto_Xml = $('#txt_concepto_xml').val();
        obj.Moneda = $('#txt_moneda').val();
        obj.Subtotal = (_replace_money($('#txt_subtotal').val()) !== '' || _replace_money($('#txt_subtotal').val()) == undefined) ? parseFloat(_replace_money($('#txt_subtotal').val())) : 0;
        obj.IVA = ($('#txt_iva').val() !== '' || $('#txt_iva').val() == undefined) ? parseFloat(_replace_money($('#txt_iva').val())) : 0;
        obj.Retencion = ($('#txt_retencion').val() !== '' || $('#txt_retencion').val() == undefined) ? parseFloat(_replace_money($('#txt_retencion').val())) : 0;
        obj.Total_Pagar = _replace_money($('#txt_total').html());
        obj.UUID = "";
        obj.Entidad_Id = $('#cmb_entidad :selected').val();


        //  validamos que este seleccionado como folio ingresa
        if ($('#txt_folio_solicitud_cheque').val() == '' || $('#txt_folio_solicitud_cheque').val() == undefined) {
            obj.Folio_Cheque = 0;
        }
        //  se le asigna la variable como null
        else {
            obj.Folio_Cheque = parseInt($('#txt_folio_solicitud_cheque').val());
        }

        obj.Referencia_Pago = $('#txt_referencia_pago').val();
        obj.Fecha_Pago_Proveedor = Date.parse($('#txt_fecha_pago').val());

        obj.Nombre_Xml = '';
        obj.Tipo_Xml = $('#txt_tipo_archivo_xml').val();

        obj.Tabla_Cuentas = JSON.stringify($('#tbl_facturas_cuentas').bootstrapTable('getData'));
        obj.Tabla_Anticipos = JSON.stringify($('#tbl_facturas_anticipos_operacion').bootstrapTable('getData'));

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Alta',
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
                        _mostrar_vista('Principal');

                        //  se consultan la informacion
                        _consultar_informacion();


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
--PARÁMETROS:           seguir_salir: Si el actualizar los datos del concepto se va salir o seguir en la pantalla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function actualizar(seguir_salir) {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario

    try {

        //  se carga la información 
        obj.Factura_Id = parseInt($('#txt_factura_id').val());
        obj.Concepto_Id = $('#cmb_tipo_concepto :selected').val();
        obj.Folio = $('#txt_folio').val();
        obj.Fecha_Recepcion = Date.parse($('#txt_fecha_recepcion').val());
        obj.Fecha_Factura = Date.parse($('#txt_fecha_factura').val());
        obj.Rfc = $('#txt_rfc').val();
        obj.Razon_Social = $('#txt_razon_social').val();
        obj.Referencia_Interna = $('#txt_referencia_interna').val();
        obj.Referencia = $('#txt_referencia').val();
        obj.Pedimento = $('#txt_pedimento').val();
        obj.Concepto = $('#txt_concepto_xml').val();
        obj.Concepto_Xml = $('#txt_concepto_xml').val();
        obj.Moneda = $('#txt_moneda').val();
        obj.Subtotal = (_replace_money($('#txt_subtotal').val()) !== '' || _replace_money($('#txt_subtotal').val()) == undefined) ? parseFloat(_replace_money($('#txt_subtotal').val())) : 0;
        obj.IVA = ($('#txt_iva').val() !== '' || $('#txt_iva').val() == undefined) ? parseFloat(_replace_money($('#txt_iva').val())) : 0;
        obj.Retencion = ($('#txt_retencion').val() !== '' || $('#txt_retencion').val() == undefined) ? parseFloat(_replace_money($('#txt_retencion').val())) : 0;
        obj.Total_Pagar = _replace_money($('#txt_total').html());
        obj.UUID = "";
        obj.Entidad_Id = $('#cmb_entidad :selected').val();

        obj.Nombre_Xml ='';
        obj.Tipo_Xml = '';

        obj.Tabla_Cuentas = JSON.stringify($('#tbl_facturas_cuentas').bootstrapTable('getData'));
        obj.Tabla_Anticipos = JSON.stringify($('#tbl_facturas_anticipos_operacion').bootstrapTable('getData'));
        obj.Tabla_Anticipos_Eliminados = JSON.stringify($('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('getData'));



        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Actualizar',
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

                        if (seguir_salir == "salir") {
                            //  se limpian los controles
                            _limpiar_todos_controles_modal();
                            //  se cierra el modal
                            _mostrar_vista('Principal');
                            //  se consultan la informacion
                            _consultar_informacion();
                        } else if (seguir_salir == 'seguir') {

                            //  se limpia el modal
                            _limpiar_todos_controles_modal_captura_xml();
                            //  se validan los conceptos utilizados
                            crear_conceptos_xml_registrados(1);

                            lectura_montos_registados_manualmente();
                            lectura_pagos_registrados();
                            lectura_anticipos_registrados();

                            calcular_diferencia_montos_factura();
                        }

                        _configuracion_controles_conceptos('Nuevo');

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
--NOMBRE_FUNCIÓN:       crear_tabla_cuentas
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_cuentas() {

    try {
        //  se destruye la tabla
        $('#tbl_facturas_cuentas').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_facturas_cuentas').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            data: [],
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDysplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,

            columns: [
                { field: 'Pago_Id', title: 'Pago_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Relacion_Id', title: 'Relacion_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Cuenta', title: 'Cuenta', width: 110, align: 'left', valign: 'top', visible: true },
                { field: 'Entidad', title: 'Entidad', width: 110, align: 'left', valign: 'top', visible: true },

                {
                    field: 'Monto', title: 'Monto', align: 'right', valign: 'top', width: 50, visible: true, sortable: true,

                    /* =============================================
                    --NOMBRE_FUNCIÓN:        formatter: function (value, row) {
                    --DESCRIPCIÓN:          Evento con el que se da estilo a la celda
                    --PARÁMETROS:           value: es el valor de la celda
                    --                      row: estructura del renglon de la tabla
                    --CREO:                 Hugo Enrique Ramírez Aguilera
                    --FECHA_CREO:           14 Abril de 2020
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    formatter: function (value, row) {
                        return accounting.formatMoney(value);
                    },


                },

                {
                    field: 'Eliminar',
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
                    --FECHA_CREO:           14 Abril de 2020
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    formatter: function (value, row) {


                        var opciones;//   variable para formar la estructura del boton

                        opciones = '<div style=" text-align: center;">';
                        opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Pago_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_cuenta_click(this);" title="Eliminar"><i class="fa fa-times"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                        opciones += '</div>'

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
--NOMBRE_FUNCIÓN:       btn_eliminar_cuenta_click
--DESCRIPCIÓN:          elimina un registro dentro de la tabla y lo paso a la sección de la tabla de eliminados
--PARÁMETROS:           renglon: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           29 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_cuenta_click(renglon) {
    try {
        var row = $(renglon).data('orden');//   se registrara la información del renglón de la tabla

        //  validamos que pueda realizar la accion
        if ($('#chk_validador').prop('checked') == false) {


            //  se muestra mensaje de confirmación
            bootbox.confirm({
                title: 'ELIMINAR CUENTA [' + row.Cuenta + ']',
                message: 'Esta seguro de Eliminar el registro seleccionado?',
                callback: function (result) {
                    //  validamos la acción realizada
                    if (result) {

                        //  se elimina el registro dentro de la tabla
                        $('#tbl_facturas_cuentas').bootstrapTable('remove', {
                            field: 'Relacion_Id',
                            values: [row.Relacion_Id],
                        });


                        //  se calculan los totales
                        calcular_totales_pagos_anticipos();
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
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_cuenta_click] ', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_cuentas
--DESCRIPCIÓN:          consulta a los participantes que se encuentran registrados
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           29 de Agosto del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_cuentas() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  se filtra la meta id
        filtros.Factura_Id = parseInt($('#txt_factura_id').val());

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Cuentas',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que exista información en los valores recibidos
                if (datos !== null) {

                    //  se convierte la información recibida
                    datos = JSON.parse(datos.d);

                    //  se carga la información dentro de la tabla
                    $('#tbl_facturas_cuentas').bootstrapTable('load', datos);
                }
            }
        });
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [_consultarParticipantes] ', e.message);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_anticipos
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_anticipos_operacion() {

    try {
        //  se destruye la tabla
        $('#tbl_facturas_anticipos_operacion').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_facturas_anticipos_operacion').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            data: [],
            pageSize: 10,
            pageList: [10, 25, 50, 100, 200],
            smartDysplay: false,
            search: false,
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
                    --FECHA_CREO:           14 Abril de 2020
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
                    //  Eliminar
                    field: 'Eliminar',
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
                    --FECHA_CREO:           14 Abril de 2020
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    formatter: function (value, row) {

                        var opciones;//   variable para formar la estructura del boton

                        opciones = '<div style=" text-align: center;">';
                        opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Anticipo_Factura_Id +
                            '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                            '\' onclick="btn_eliminar_anticipo_operacion_click(this);" title="Eliminar">' +
                            '<i class="fa fa-times"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },

            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_anticipos_operacion] ', e.message);
    }
}




/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_anticipos_eliminados_operacion
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_anticipos_eliminados_operacion() {

    try {
        //  se destruye la tabla
        $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable({
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

                { field: 'Anticipo_Factura_Id', title: 'Anticipo_Factura_Id', align: 'center', valign: 'top', visible: true },
                { field: 'Anticipo_Id', title: 'Anticipo_Id', align: 'center', valign: 'top', visible: true },
                { field: 'Anticipo', title: 'Anticipo', align: 'center', valign: 'top', visible: true },
                { field: 'Monto', title: 'Monto', align: 'center', valign: 'top', visible: true },

            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_anticipos_eliminados_operacion] ', e.message);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_anticipo_operacion_click
--DESCRIPCIÓN:          Se realiza la acción de eliminar
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_anticipo_operacion_click(renglon) {
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

                        //  se obtiene el total del numero de registros dentro de la tabla
                        var total_row = $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('getOptions').totalRows;//   almacena el total de registros

                        //  se incrementa el indice
                        total_row = total_row + 1;


                        //  se inserta el nuevo valor a la tabla
                        $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('insertRow', {
                            index: total_row,
                            row: {
                                Anticipo_Factura_Id: row.Anticipo_Factura_Id,
                                Monto: row.Monto,
                                Anticipo_Id: row.Anticipo_Id,
                                Anticipo: row.Anticipo,
                            }

                        });


                        //  se elimina el registro dentro de la tabla
                        $('#tbl_facturas_anticipos_operacion').bootstrapTable('remove', {
                            field: 'Anticipo_Id',
                            values: [row.Anticipo_Id],

                            field: 'Monto',
                            values: [row.Monto],
                        });


                        //  se calculan los totales
                        calcular_totales_pagos_anticipos();
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
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_anticipo_operacion_click] ', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_informacion_anticipos_operacion
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_informacion_anticipos_operacion() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  filtro para la cuenta
        filtros.Anticipo_Factura_Id = 0;
        filtros.Anticipo_Id = 0;
        filtros.Factura_Id = parseInt($('#txt_factura_id').val());

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Facturas_Anticipos_Filtros',
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
                    $('#tbl_facturas_anticipos_operacion').bootstrapTable('load', JSON.parse(datos.d));
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_informacion_anticipos_operacion]', e);
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       calcular_totales_pagos_anticipos
--DESCRIPCIÓN:          Calcula el total de los pagos y anticipos
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function calcular_totales_pagos_anticipos() {
    try {
        //  se calculara el total de los pagos
        //  se asignan los datos de la tabla
        var tbl_detalles_pagos_realizados = $('#tbl_facturas_cuentas').bootstrapTable('getData');//   almacena los datos de la tabla 
        var _total_pagos = 0;// variable para almacenar el total de los pagos
        var tbl_detalles_anticipos_realizados = $('#tbl_facturas_anticipos_operacion').bootstrapTable('getData');//   almacena los datos de la tabla 
        var _total_anticipos = 0;// variable para almacenar el total de los pagos
        var _diferencia = 0;// variable para almacenar la diferencia entre los pagos y el subtotal
        var _total_realizado = 0;// variable para almacenar el total de los pagos
        var _subtotal = 0;// variable para almacenar el subtotal del xml

        /* =============================================
        --NOMBRE_FUNCIÓN:       $.each(tbl_detalles_pagos_realizados, function (index, value) {
        --DESCRIPCIÓN:          se recorre la tabla
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           14 Abril de 2020
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        //  se recorre la tabla
        $.each(tbl_detalles_pagos_realizados, function (index, value) {
            _total_pagos = _total_pagos + parseFloat(value.Monto);
        });

        /* =============================================
       --NOMBRE_FUNCIÓN:       $.each(tbl_detalles_anticipos_realizados, function (index, value) {
       --DESCRIPCIÓN:          se recorre la tabla
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           14 Abril de 2020
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        //  se recorre la tabla
        $.each(tbl_detalles_anticipos_realizados, function (index, value) {
            _total_anticipos = _total_anticipos + parseFloat(value.Monto);
        });

        //  se realiza la sumatoria de los pagos realizaods
        _total_realizado = _total_pagos + _total_anticipos;

        //  se obtiene el subtotal registrado
        _subtotal = parseFloat(_replace_money($('#txt_total').html()));



        //  se obtiene la diferencia
        _diferencia = _subtotal - _total_realizado;

        //  se ingresa el valor al control
        $('#txt_total_pagos').html(_total_pagos).formatCurrency();
        $('#txt_total_anticipo').html(_total_anticipos).formatCurrency();
        $('#txt_total_realizado').html(_total_realizado).formatCurrency();
        if (_diferencia >= 0)
            $('#txt_diferencia').html(_diferencia).formatCurrency().removeClass('text-warning text-success').addClass('text-success');
        else
            $('#txt_diferencia').html("$" + formatNumber(_diferencia)).removeClass('text-warning text-success').addClass('text-warning');
    }
    catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[calcular_totales_pagos_anticipos]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _configuracion_controles_conceptos
--DESCRIPCIÓN:          Cambia los controles al seleccionar un concepto nuevo o editar un concepto
--PARÁMETROS:           option.- Para indicar si es nuevo o actualizacion de conceptos
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           15 Noviembre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _configuracion_controles_conceptos(option) {

    $('#btn_guardar_seguir_captura').attr('title', 'Guardar y seguir');
    $('#btn_guardar_seguir_captura i').removeClass().addClass('fa fa-check text-blue text-bold');
    $('#btn_guardar_seguir_captura span').html('Guardar y seguir').removeClass().addClass('text-blue text-bold');

    $('#btn_guardar_salir_captura').attr('title', 'Guardar y salir');
    $('#btn_guardar_salir_captura i').removeClass().addClass('fa fa-check text-blue text-bold');
    $('#btn_guardar_salir_captura span').html('Guardar y salir').removeClass().addClass('text-blue text-bold');

    switch (option) {
        case 'Nuevo':

            break;
        case 'Editar':
            $('#btn_guardar_seguir_captura').attr('title', 'Editar y seguir');
            $('#btn_guardar_seguir_captura i').removeClass().addClass('fa fa-edit text-info text-bold');
            $('#btn_guardar_seguir_captura span').html('Editar y seguir').removeClass().addClass('text-info text-bold');

            $('#btn_guardar_salir_captura').attr('title', 'Editar y salir');
            $('#btn_guardar_salir_captura i').removeClass().addClass('fa fa-edit text-info text-bold');
            $('#btn_guardar_salir_captura span').html('Editar y salir').removeClass().addClass('text-info text-bold');

            break;
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_datos_captura
--DESCRIPCIÓN:          limpia todos los controles al cambiar o limpiar el control de cmb_folio
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           21 Nov 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_datos_captura_folio() {

    try {

        $('#modal_factura input[type=text]').each(function () { if ($(this).attr('id') !== "txt_folio_solicitud_cheque") $(this).val(''); });

        $('#modal_factura input[type=hidden]').each(function () { $(this).val(''); });


        //  limpia el valor del combo
        //$('#cmb_tipo_concepto').empty().trigger("change");
        $('#cmb_entidad').empty().trigger("change");
        $('#cmb_cuenta').empty().trigger("change");
        $('#cmb_anticipo').empty().trigger('change');

        //  se limpian las tablas
        $('#tbl_facturas_cuentas').bootstrapTable('load', []);
        $('#tbl_facturas_anticipos_operacion').bootstrapTable('load', []);
        $('#tbl_facturas_anticipos_eliminados_operacion').bootstrapTable('load', []);

        $('#txt_moneda').val('USD')
        //$('#div_conceptos_xml').html('');
        _limpiar_importes_conceptos();
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal] ', 'limpiar controles. ' + e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_importes_conceptos
--DESCRIPCIÓN:          limpia los importes del concepto seleccionado
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           21 Nov 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_importes_conceptos() {
    $('#txt_concepto_xml').val('');
    $('#txt_subtotal').html(0).formatCurrency();
    $('#txt_iva').html(0).formatCurrency();
    $('#txt_retencion').html('0').formatCurrency();
    $('#txt_total').html(0).formatCurrency();
    $('#txt_subtotal_factura_xml').html(0).formatCurrency();
    $('#txt_total_factura_xml').html(0).formatCurrency();
    $('#txt_cuentas_capturadas').html(0).formatCurrency();
    $('#txt_anticipos_capturadas').html(0).formatCurrency();


    $('#txt_total_pagos').html(0).formatCurrency();
    $('#txt_total_anticipo').html(0).formatCurrency();
    $('#txt_total_realizado').html(0).formatCurrency();
    $('#txt_diferencia').html(0).formatCurrency();
}


/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_pagos_y_anticipos
--DESCRIPCIÓN:          limpia las tablas e importes de pagos y anticipos
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           26 Nov 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_pagos_y_anticipos() {
    $('#tbl_facturas_cuentas').bootstrapTable('load', []);
    $('#tbl_facturas_anticipos_operacion').bootstrapTable('load', []);
    calcular_totales_pagos_anticipos();
}





/* =============================================
--NOMBRE_FUNCIÓN:       btn_seleccionar_concepto_click
--DESCRIPCIÓN:          Carga la información del registro de la tabla, carga la información dentro de los controles correspondientes
--PARÁMETROS:           tab: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_seleccionar_concepto_click(tab) {

    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla
    var factura_id = row.Factura_Id;//   variable para obtener el numero de factura del concepto

    //  se carga el folio
    $('#cmb_folio').select2("trigger", "select", {
        data: { id: row.Folio, text: row.Folio }
    });

    //$('#cmb_folio').attr('disabled', 'disabled');

    $('#txt_folio').val(row.Folio);

    $('#txt_concepto_xml').val(row.Concepto_Xml);
    $('#txt_pedimento').val(row.Pedimento);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_razon_social').val(row.Razon_Social);
    $('#txt_referencia_interna').val(row.Referencia_Interna);



    $('#txt_subtotal').val(row.Subtotal).formatCurrency();
    $('#txt_iva').val(row.IVA).formatCurrency();
    $('#txt_retencion').val(row.Retencion).formatCurrency();
    $('#txt_total').html(row.Total_Pagar).formatCurrency();

    //  se asiga el id de la factura registrada
    $('#txt_factura_id').val(factura_id == undefined ? '' : factura_id);

    //  se da formato a la lista de conceptos registrados
    $('#div_conceptos_xml li').each(function () {
        $(this).removeClass('item-list-success-selected active');
    });

    $(tab).parent().addClass('item-list-success-selected active');



    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion);
    
    //$('#' + tab.id).attr('disabled', 'disabled');
    _limpiar_pagos_y_anticipos();

    //  se calculan los totales registrados de la factura
    lectura_pagos_registrados();
    lectura_anticipos_registrados();

    _habilitar_controles_folio('Mostrar');
    _habilitar_controles_folio('Texto');



    if (factura_id == undefined || factura_id == "") {
        _configuracion_controles_conceptos('Nuevo');

    } else {
        _configuracion_controles_conceptos('Editar');
        //_consultar_datos_factura_seleccionada(factura_id);

        //  se consultan las cuentas
        lectura_montos_registados_manualmente();
        _consultar_cuentas();
        _consultar_informacion_anticipos_operacion();
        calcular_totales_pagos_anticipos();

        //  se le quitara el monto capturado al total que esta registrado
        descontar_montos_registrados();

    }

}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_datos_factura_seleccionada
--DESCRIPCIÓN:          Carga la información del registro de la tabla, carga la información dentro de los controles correspondientes
--PARÁMETROS:           factura_id: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_datos_factura_seleccionada(factura_id) {
    var filtros = new Object();
    //  se carga la información 
    filtros.Folio = "";
    filtros.Factura_Id = factura_id;
    //  se convierte a la estructura que pueda leer el controlador
    var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador


    //  se ejecuta la peticion
    $.ajax({
        type: 'POST',
        url: 'controllers/Faturas_Manuales_Controller.asmx/Consultar_Facturas_Manualmente_Filtros',
        data: $data,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        cache: false,
        success: function (result) {
            var datos = JSON.parse(result.d);
            var row = datos[0];

            //  se carga la tipo de concepto
            $('#cmb_tipo_concepto').select2("trigger", "select", {
                data: { id: row.Concepto_Id, text: row.Concepto_Texto_Id }
            });

            $('#cmb_tipo_concepto').attr('disabled', true);


            //  fecha de recepcion
            $('#txt_fecha_recepcion').val(row.Fecha_Recepcion_Texto);
            $('#txt_razon_social').val(row.Razon_Social);
            $('#txt_concepto_xml').val(row.Concepto_Xml);


         

        }
    });
}




/* =============================================
--NOMBRE_FUNCIÓN:       _validar_montos_totales_factura
--DESCRIPCIÓN:          Se valida la información requerida para realizar una acción
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validar_monto_ingresado_pago_anticipo_contra_totalesConcepto() {
    var _output = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

    var _total_facturta = 0;// variable para contener el total de los pagos
    var _subtotal_facturta = 0;// variable para contener el total de los pagos

    var _pagos_registrado = 0;// variable para contener el total de los anticipos
    var _anticipos_registrados = 0;//   variable para contener el total del registro



    try {
        //  se limpian los estilos de los controles
        _clear_all_class_error();

        //  se inicializan las propiedades
        _output.Estatus = true;
        _output.Mensaje = '';

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------
        _total_facturta = parseFloat(_replace_money($('#txt_total').html()));
        //_subtotal_facturta = parseFloat(_replace_money($('#txt_subtotal_factura_xml').html()));

        _pagos_registrado = parseFloat(_replace_money($('#txt_total_realizado').html()));
        _anticipos_registrados = parseFloat(_replace_money($('#txt_total_anticipo').html()));


        //  validamos que la suma de pagos y anticipos no sea mayor que el de la factura
        if (round((_pagos_registrado), 2) > round(_total_facturta, 2)) {


            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_total");
            _add_class_error("txt_total_realizado");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;Los pagos de los conceptos no pueden ser mayor al total del concepto.<br />';
        }



        //  validamos que la suma de pagos y anticipos no sea mayor que el de la factura
        if (round((_anticipos_registrados), 2) > round(_total_facturta, 2)) {


            //  agregamos el estilo al control que no cumple
            _add_class_error("txt_total");
            _add_class_error("txt_total_anticipo");

            //  se indica el estatus como no cumplido
            _output.Estatus = false;

            //  se asigna el mensaje que se mostrara
            _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;Los anticipos no pueden ser mayor al total del concepto.<br />';
        }



        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------
        //  validamos si no cumple alguno de los elementos
        if (_output.Mensaje != "") {
            //  asignamos el titulo del mensaje
            _output.Mensaje = "Validación: <br />" + _output.Mensaje;
        }

        //  ---------------------------------------------------------------------------------------
        //  ---------------------------------------------------------------------------------------

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_validar_montos_totales_factura]', e);
    } finally {
        return _output;//   se regresa la variable
    }
}
