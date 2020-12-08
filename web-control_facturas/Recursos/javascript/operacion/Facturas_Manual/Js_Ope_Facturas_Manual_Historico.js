
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal_historico
--DESCRIPCIÓN:          Muestra el modal
--PARÁMETROS:           title_window: titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal_historico(title_window) {
    //  se ejecuta el método con el que se asigna el nombre del titulo del formulario
    _set_title_modal_historico(title_window);

    //  se muestra el modal
    jQuery('#modal_facturas_historico').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_historico
--DESCRIPCIÓN:          establece el nombre del titulo del modal
--PARÁMETROS:           titulo: mensaje que se mostrara como titulo en el modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_historico(titulo) {
    //  se asigna el texto al titulo al modal
    $("#lbl_titulo_modal_historico").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_historico_click
--DESCRIPCIÓN:          Llama al evento que se encarga de cerrar el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_historico_click() {
    //  se ejecuta el método con el que se cierra el modal
    _set_close_modal_historico();
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal_historico
--DESCRIPCIÓN:          Cierra el modal
--PARÁMETROS:           Na
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal_historico() {
    //  se cierra el modal
    jQuery('#modal_facturas_historico').modal('hide');
}



/* =============================================
--NOMBRE_FUNCIÓN:       _limpiar_todos_controles_modal_seguimiento
--DESCRIPCIÓN:          Limpia los controles del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _limpiar_todos_controles_modal_seguimiento() {

    try {
        //  se limpian los controles de texto y ocultos
        /* =============================================
       --NOMBRE_FUNCIÓN:       #modal_facturas_historico input[type=text]
       --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
       --PARÁMETROS:           NA
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           14 Abril de 2020
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#modal_facturas_historico input[type=text]').each(function () { $(this).val(''); });

        /* =============================================
      --NOMBRE_FUNCIÓN:       #modal_facturas_historico input[type=hidden]
      --DESCRIPCIÓN:          recorre los controles de tipo texto y limpia su valor
      --PARÁMETROS:           NA
      --CREO:                 Hugo Enrique Ramírez Aguilera
      --FECHA_CREO:           14 Abril de 2020
      --MODIFICÓ:
      --FECHA_MODIFICÓ:
      --CAUSA_MODIFICACIÓN:
      =============================================*/
        $('#modal_facturas_historico input[type=hidden]').each(function () { $(this).val(''); });


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_limpiar_todos_controles_modal_seguimiento] ', 'limpiar controles. ' + e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_modal_historico
--DESCRIPCIÓN:          Genere los métodos y funciones necesarios para el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_modal_historico() {
    try {

        //  se crea la tabla de seguimiento
        _crear_tabla_historico();

        //  se limpian los controles
        _limpiar_todos_controles_modal_seguimiento();

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_inicializar_vista_modal_historico] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _crear_tabla_historico
--DESCRIPCIÓN:          Se crea la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _crear_tabla_historico() {

    try {

        //  destruye la estructura de la tabla
        $('#tbl_historico').bootstrapTable('destroy');

        //  crea la estructura de la tabla
        $('#tbl_historico').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            data: [],
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            smartDysplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,

            columns: [

                { field: 'Movimiento_Id', title: 'Movimiento_Id', width: 50, align: 'center', valign: 'top', sortable: true, visible: false },
                { field: 'Factura_Id', title: 'Factura_Id', width: 50, align: 'center', valign: 'top', sortable: true, visible: false },

                { field: 'Fecha_Texto', title: 'Fecha', width: 80, align: 'center', valign: 'top', visible: true, sortable: true },
                { field: 'Accion', title: 'Accion', width: 250, align: 'left', valign: 'top', visible: true, sortable: true },
                { field: 'Usuario', title: 'Usuario', width: 100, align: 'left', valign: 'top', visible: true },
            ]
        });
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + '[_crear_tabla_historico]', e.message);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_historio
--DESCRIPCIÓN:          Se consulta el histórico de los movimientos que tiene la factura
--PARÁMETROS:           factura_id: el id del elemento que se estara buscando
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_historio(factura_id) {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        filtros = new Object();//   se inicializa

        //  se pasan los valores
        filtros.Factura_Id = parseInt(factura_id);

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Consultar_Facturas_Historico_Movimientos',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que tenga información la variable recibida
                if (datos !== null) {

                    //  se convierten los datos recibidos
                    datos = JSON.parse(datos.d);

                    //  se cargan los datos a la tabla
                    $('#tbl_historico').bootstrapTable('load', datos);
                }
            }
        });
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [_consultar_historio] ', e.message);
    }
}



