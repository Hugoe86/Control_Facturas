
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal_pagos
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal_pagos(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_pagos(title_window);

    //  se muestra el modal
    jQuery('#modal_facturas_pagos').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_pagos
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_pagos(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_pagos").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_pagos_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_pagos_click() {
    //  se llama al evento que cierra el modal
    _set_close_modal_pagos();
}


/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal_pagos
--DESCRIPCIÓN:          Ejecuta la sección para ocultar el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal_pagos() {
    //  cierra el modal
    jQuery('#modal_facturas_pagos').modal('hide');
}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_pagos
--DESCRIPCIÓN:          Establece los metodos principales del modal de adjuntos
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_pagos() {
    try {

        //  inicializamos los eventos del modal
        _eventos_modal_pagos();

        //  se crea la tabla
        crear_tabla_pagos();


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_inicializar_vista_pagos]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_informacion_pagos
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           factura_id_ id de la factura
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_informacion_pagos(factura_id) {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  filtro para la cuenta
        filtros.Factura_Id = parseInt(factura_id);

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: '../Operacion/controllers/Facturas_Controller.asmx/Consultar_Cuentas',
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
                    $('#tbl_facturas_pagos').bootstrapTable('load', JSON.parse(datos.d));

                    var tbl_detalles_anticipos_realizados = $('#tbl_facturas_pagos').bootstrapTable('getData');//   almacena los datos de la tabla 
                    var _total_anticipos = 0;// variable para almacenar el total de los pagos

                    /* =============================================
                    --NOMBRE_FUNCIÓN:       $.each(tbl_detalles_anticipos_realizados, function (index, value) {
                    --DESCRIPCIÓN:          se recorre la tabla
                    --PARÁMETROS:           NA
                    --CREO:                 Hugo Enrique Ramírez Aguilera
                    --FECHA_CREO:           24 Octubre de 2019
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    //  se recorre la tabla
                    $.each(tbl_detalles_anticipos_realizados, function (index, value) {
                        _total_anticipos = _total_anticipos + parseFloat(value.Monto);
                    });


                    //  se ingresa el valor al control
                    $('#lbl_total_pagos').text(_total_anticipos.toFixed(2));
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_informacion_pagos]', e);
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_modal_pagos
--DESCRIPCIÓN:          Crea los eventos de los botones que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_modal_pagos() {
    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_cancelar_pagos
        --DESCRIPCIÓN:          Evento con el que se cancela la acción de nuevo o actualización
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_cancelar_pagos').on('click', function (e) {
            e.preventDefault();

            //  limpia los controles del modal
            _limpiar_todos_controles_modal_pagos();

            //  cierra el modal
            _cancelar_modal_pagos_click();
        });



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_modal_pagos] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal_pagos
--DESCRIPCIÓN:          limpia todos los controles que se encuentran dentro del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal_pagos() {

    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       #modal_facturas_pagos input[type=text]
        --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
        --PARÁMETROS:           NA
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#modal_facturas_pagos input[type=text]').each(function () { $(this).val(''); });

        /* =============================================
       --NOMBRE_FUNCIÓN:       #modal_facturas_pagos input[type=hidden]
       --DESCRIPCIÓN:          recorre los controles de tipo hidden y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           24 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#modal_facturas_pagos input[type=hidden]').each(function () { $(this).val(''); });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal_pagos] ', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_pagos
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_pagos() {

    try {
        //  se destruye la tabla
        $('#tbl_facturas_pagos').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_facturas_pagos').bootstrapTable({
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

               
                { field: 'Cuenta', title: 'Cuenta', align: 'left', valign: 'top', width: 150, visible: true, sortable: true },
                { field: 'Entidad', title: 'Entidad', align: 'left', valign: 'top', width: 150, visible: true, sortable: true },
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

             
            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_pagos] ', e.message);
    }
}


