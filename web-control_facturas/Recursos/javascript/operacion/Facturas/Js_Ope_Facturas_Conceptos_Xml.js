
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal_conceptos_xml
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal_conceptos_xml(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_conceptos_xml(title_window);

    //  se muestra el modal
    jQuery('#modal_conceptos_xml').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_conceptos_xml
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_conceptos_xml(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_modal_conceptos_xml").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_conceptos_xml_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_conceptos_xml_click() {
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
    jQuery('#modal_conceptos_xml').modal('hide');
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
    var factura_id = $(tab).data('factura_id');//   variable para obtener el numero de factura del concepto


    $('#txt_factura_id').val(factura_id == undefined ? '' : factura_id);

    $('#txt_concepto_xml').val(row.Concepto);
    $('#txt_subtotal').html(row.Monto).formatCurrency();
    $('#txt_iva').html(row.Iva).formatCurrency();
    $('#txt_retencion').html(row.Retencion).formatCurrency();
    $('#txt_total').html(row.Total).formatCurrency();
    $('#div_conceptos_xml li').each(function () {
        $(this).removeClass('item-list-success-selected active');
    });
    $(tab).parent().addClass('item-list-success-selected active');


    //$('#' + tab.id).attr('disabled', 'disabled');
    _limpiar_pagos_y_anticipos();

    //  se calculan los totales registrados de la factura
    lectura_pagos_registrados();
    lectura_anticipos_registrados();
    calcular_diferencia_montos_factura();


    if (factura_id == undefined || factura_id == "") {
        _configuracion_controles_conceptos('Nuevo');

    } else {
        _configuracion_controles_conceptos('Editar');
        _consultar_datos_factura_seleccionada(factura_id);

        //  se consultan las cuentas
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
        url: 'controllers/Facturas_Controller.asmx/Consultar_Facturas_Filtros',
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

            
            //  fecha de recepcion
            $('#txt_fecha_recepcion').val(row.Fecha_Recepcion_Texto);
            
        }
    });
}