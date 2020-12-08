
$(document).on('ready', function () {
    //  se manda llamar al método de cargar vistas
    _load_vistas();
});

/* =============================================
--NOMBRE_FUNCIÓN:       _load_vistas
--DESCRIPCIÓN:          Carga las paginas HTML dentro del documento   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_vistas() {

    //  se ejecutan las paginas HTML que se estarán cargando dentro del aspx
    _launchComponent('vistas/Anticipos/Principal.html', 'Principal');
    _launchComponent('vistas/Anticipos/Anticipo.html', 'Anticipo');
    _launchComponent('vistas/Anticipos/Pagos.html', 'Pagos');


}

/* =============================================
--NOMBRE_FUNCIÓN:       _launchComponent
--DESCRIPCIÓN:          Carga los eventos y funciones que tendrá cada pagina HTML
--PARÁMETROS:           component: ruta del archivo HTML
--                      id: Nombre que se le dará a la pagina HTML
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launchComponent(component, id) {

    //  se estarán cargando las eventos y funciones correspondientes
    $('#' + id).load(component, function () {

        switch (id) {
            case 'Principal':
                //  se carga los eventos y funciones
                _inicializar_vista_principal();
                break;


            case 'Anticipo':
                //  se carga los eventos y funciones
                _inicializar_vista_anticipo();
                break;

            case 'Pagos':
                //  se carga los eventos y funciones
                _inicializar_vista_pagos();
                break;
        }
    });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_principal
--DESCRIPCIÓN:          Evento con el que se cargan los eventos y funciones de la vista principal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_principal() {
    try {


        //  se crea la tabla principal
        crear_tabla_principal();


        //  se cargan los eventos principales de los botones
        _eventos_principal();

        //  se inicializan las fechas
        _inicializar_fechas();

        //  se muestra la vista principal
        _mostrar_vista('Principal');

        //  se cargan los combos filtro
        _load_cmb_filtro_nombre('cmb_busqueda_factura');
        _load_cmb_anticipos_catalogo('cmb_anticipo_factura');
        //_load_cmb_filtro_referencia('cmb_busqueda_tipo');

        //  se da formato al toolbar de búsqueda de la tabla
        _set_location_toolbar('toolbar');


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_inicializar_vista_principal] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_icheck
--DESCRIPCIÓN:          Establece el estilo que tendrá el control del check
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_icheck() {
    $('input.icheck-11').iCheck('destroy');
    $('input.icheck-11').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });
}



/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_fechas
--DESCRIPCIÓN:          Establece el estilo que tendrá el control de las fechas
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_fechas() {
    $('#dtp_txt_fecha_inicio').datetimepicker({
        defaultDate: new Date(),
        viewMode: 'days',
        locale: 'es',
        format: "DD/MM/YYYY"
    });
    $("#dtp_txt_fecha_inicio").datetimepicker("useCurrent", true);

    $('#dtp_txt_fecha_termino').datetimepicker({
        defaultDate: new Date(),
        viewMode: 'days',
        locale: 'es',
        format: "DD/MM/YYYY"
    });
    $("#dtp_txt_fecha_termino").datetimepicker("useCurrent", true);

}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_location_toolbar
--DESCRIPCIÓN:          Establece el estilo que tendrá el control
--PARÁMETROS:           toolbar: Nombre del control al cual se le estará dando estilo
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_location_toolbar(toolbar) {
    //  se remueve el estilo
    $('#' + toolbar).parent().removeClass("pull-left");

    //  se agrega el estilo
    $('#' + toolbar).parent().addClass("pull-right");
}

/* =============================================
--NOMBRE_FUNCIÓN:       _mostrar_vista
--DESCRIPCIÓN:          Permite mostrar u ocultar los div correspondientes a los HTML
--PARÁMETROS:           vista_: Nombre de acción que se estará realizando
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _mostrar_vista(vista_) {

    //  se mostraran y ocultaran los div que se encuentran dentro del archivo aspx 
    //  validamos que tipo de operacion se realizara
    switch (vista_) {
        //  principal
        case "Principal":
            //  se habilita la seccion principal del formulario
            $('#Principal').show();
            break;

    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_informacion
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_informacion() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda


        //  filtro para el folio
        if ($.trim($('#cmb_busqueda_factura :selected').val()) !== '') {
            filtros.Folio_Cheque = parseInt($('#cmb_busqueda_factura :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Folio_Cheque = 0;
        }



        //  filtro para el folio
        if ($.trim($('#cmb_anticipo_factura :selected').val()) !== '') {
            filtros.Anticipo_Id = parseInt($('#cmb_anticipo_factura :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Anticipo_Id = 0;
        }

        //  filtros para la fecha
        filtros.Fecha_Inicio_Texto = ($('#txt_fecha_inicio').val() !== '' || $('#txt_fecha_inicio').val() == undefined) ? ($('#txt_fecha_inicio').val()) : "";
        filtros.Fecha_Termino_Texto = ($('#txt_fecha_termino').val() !== '' || $('#txt_fecha_termino').val() == undefined) ? ($('#txt_fecha_termino').val()) : "";

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador



        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Anticipos_Controller.asmx/Consultar_Plantilla_Filtros',
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
                    $('#tbl_reporte_plantilla').bootstrapTable('load', JSON.parse(datos.d));
                }
            }
        });



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_informacion]', e);
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_principal
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_principal() {

    try {
        //  se destruye la tabla
        $('#tbl_reporte_plantilla').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_reporte_plantilla').bootstrapTable({
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
                { field: 'Factura_Id', title: 'Factura_Id', width: 110, align: 'left', valign: 'top', visible: false },
                { field: 'Concepto_Texto_Id', title: 'Tipo de operacion', width: 110, align: 'left', valign: 'top', visible: true },
                { field: 'Fecha_Recepcion_Texto', title: 'Fecha de recepción', align: 'left', valign: 'top', width: 40, visible: true, sortable: true },
                { field: 'Fecha_Factura_Texto', title: 'Fecha de factura', align: 'left', valign: 'top', width: 40, visible: true, sortable: true },
                { field: 'Rfc', title: 'RFC', align: 'left', valign: 'top', width: 90, visible: true, sortable: true, },
                { field: 'Folio', title: 'Folio', align: 'left', valign: 'top', width: 70, visible: true, sortable: true, },
                { field: 'Referencia_Interna', title: 'Referencia int.', align: 'left', valign: 'top', width: 70, visible: false, sortable: true, },
                { field: 'Referencia', title: 'Referencia', align: 'left', valign: 'top', width: 50, visible: false, sortable: true, },
                { field: 'Pedimento', title: 'Pedimento', width: 115, align: 'left', valign: 'top', visible: true },
                { field: 'Concepto_Xml', title: 'Concepto', width: 300, align: 'left', valign: 'top', visible: true },

                { field: 'Moneda', title: 'Moneda', width: 50, align: 'left', valign: 'top', visible: true },
                {
                     field: 'Subtotal', title: 'Subtotal', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,

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

                {
                    field: 'IVA', title: 'IVA', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,
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

                {
                    field: 'Retencion', title: 'Retención', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,
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

                {
                    field: 'Total_Pagar', title: 'Total', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,
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

                { field: 'Referencia_Pago', title: 'Ref. de pago', width: 50, align: 'left', valign: 'top', visible: true },
                { field: 'Fecha_Pago_Proveedor_Texto', title: 'Fecha de pago prov.', width: 50, align: 'left', valign: 'top', visible: true },
                 { field: 'Folio_Cheque', title: 'Folio cheque', align: 'left', valign: 'top', width: 50, visible: true, sortable: true, },

                {
                    field: 'Total_Pagos', title: 'Total conceptos', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,
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

               

                {
                    //  Anticipos
                    field: 'Pagos',
                    title: '',
                    width: 80,
                    visible: true,
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
                        opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id +
                                        '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                        '\' onclick="btn_pagos_click(this);" title="Ver Anticipo">' +
                                        '<i class="fa fa-money"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },


                {
                    field: 'Total_Anticipo', title: 'Total Anticipos', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,
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
                
       

                {
                    //  Anticipos
                    field: 'Anticipos',
                    title: '',
                    width: 80,
                    visible: true,
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
                        opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id +
                                        '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                        '\' onclick="btn_anticipo_click(this);" title="Ver Anticipo">' +
                                        '<i class="fa fa-money"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

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
--NOMBRE_FUNCIÓN:       _eventos_principal
--DESCRIPCIÓN:          Se generan las acciones que realizaran los botones de la sección principal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_principal() {
    try {

        /* =============================================
        --NOMBRE_FUNCIÓN:       btn_inicio
        --DESCRIPCIÓN:          Acciones realizadas al dar click en el botón de inicio
        --PARÁMETROS:           e: parametro que se refiere al evento click
        --CREO:                 Hugo Enrique Ramírez Aguilera
        --FECHA_CREO:           24 Octubre de 2019
        --MODIFICÓ:
        --FECHA_MODIFICÓ:
        --CAUSA_MODIFICACIÓN:
        =============================================*/
        $('#btn_inicio').on('click', function (e) {
            e.preventDefault();

            //  se regresa al formulario principal
            window.location.href = '../Paginas_Generales/Frm_Apl_Principal.aspx';
        });

        /* =============================================
      --NOMBRE_FUNCIÓN:       btn_pdf
      --DESCRIPCIÓN:          Acciones realizadas al dar click en el botón de generar pdf
      --PARÁMETROS:           e: parametro que se refiere al evento click
      --CREO:                 Hugo Enrique Ramírez Aguilera
      --FECHA_CREO:           24 Octubre de 2019
      --MODIFICÓ:
      --FECHA_MODIFICÓ:
      --CAUSA_MODIFICACIÓN:
      =============================================*/
        $('#btn_pdf').on('click', function (e) {
            e.preventDefault();

            //  se genera el formato
            _generar_pdf();
        });

        /* =============================================
--NOMBRE_FUNCIÓN:       btn_pdf
--DESCRIPCIÓN:          Acciones realizadas al dar click en el botón de generar pdf
--PARÁMETROS:           e: parametro que se refiere al evento click
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
        $('#btn_excel').on('click', function (e) {
            e.preventDefault();

            //  se crea el archivo de excel
            _generar_excel();
        });

        /* =============================================
       --NOMBRE_FUNCIÓN:       btn_busqueda
       --DESCRIPCIÓN:          Se llama al evento de búsqueda
       --PARÁMETROS:           e: parametro que se refiere al evento click
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           24 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#btn_busqueda').on('click', function (e) {
            e.preventDefault();

            //  se realiza la consulta
            _consultar_informacion();
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_principal] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _keyDownInt
--DESCRIPCIÓN:          Valida que solo se acepten numeros
--PARÁMETROS:           id: nombre del control al cual se le aplica el filtro
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _keyDownInt(id) {

    /* =============================================
      --NOMBRE_FUNCIÓN:       $('#' + id).on('keydown', function (e)
      --DESCRIPCIÓN:          Evento que solo acepta valores numericos dentro del control
      --PARÁMETROS:           e: parametro que se refiere al evento click
      --CREO:                 Hugo Enrique Ramírez Aguilera
      --FECHA_CREO:           24 Octubre de 2019
      --MODIFICÓ:
      --FECHA_MODIFICÓ:
      --CAUSA_MODIFICACIÓN:
      =============================================*/
    $('#' + id).on('keydown', function (e) {

        //  Permitir: retroceder, eliminar, tabular, escapar, ingresar
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||

            // Permitir: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||

            // Permitir: inicio, fin, izquierda, derecha, abajo, arriba
            (e.keyCode >= 35 && e.keyCode <= 40)) {

            // deja que suceda, no hagas nada
            return;
        }

        //  Asegúrese de que sea un número y detenga la pulsación de tecla
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
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
                url: '../Operacion/controllers/Facturas_Controller.asmx/Consultar_Anticipos_Reporte_Combo',
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
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_anticipos_catalogo]', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_filtro_nombre
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_filtro_nombre(cmb) {
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
                url: '../Operacion/controllers/Facturas_Controller.asmx/Consultar_Facturas_Nombre_Combo',
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
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_filtro_nombre]', e);
    }
}




/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_filtro_referencia
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_filtro_referencia(cmb) {
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
                url: '../Operacion/controllers/Facturas_Controller.asmx/Consultar_Facturas_Referencia_Pago_Combo',
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
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_filtro_referencia]', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _mostrar_mensaje
--DESCRIPCIÓN:          Muestra un mensaje con la información de la variable de mensaje
--PARÁMETROS:           titulo: Nombre que tendrá el titulo de la pantalla de mensaje
                        mensaje: String con la información del mensaje
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
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
--NOMBRE_FUNCIÓN:       _add_class_error
--DESCRIPCIÓN:          Se agrego un estilo a un control, el cual se utiliza para las funciones de validación
--PARÁMETROS:           selector: nombre del control al cual se le estará aplicando el estilo de error
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _add_class_error(selector) {
    //  se le asigna el estilo al control
    $('#' + selector).addClass('alert-danger');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _remove_class_error
--DESCRIPCIÓN:          Se quita el estilo de error de un control 
--PARÁMETROS:           selector: nombre del control al cual se le estará quitando el estilo de error
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _remove_class_error(selector) {
    //  se remueve el estilo al control
    $('#' + selector).removeClass('alert-danger');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _clear_all_class_error
--DESCRIPCIÓN:          Quita los estilos de error de todos los controles del modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _clear_all_class_error() {

    /* =============================================
    --NOMBRE_FUNCIÓN:       modal_factura
    --DESCRIPCIÓN:          remueve todos los estidos de error de los controles
    --PARÁMETROS:           NA
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           24 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#modal_factura input[type=text]').each(function (index, element) {
        _remove_class_error($(this).attr('id'));
    });
}


/* =============================================
--NOMBRE_FUNCIÓN:       _generar_pdf
--DESCRIPCIÓN:          genere el formato de pdf de la informacion
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           25 de Septiembre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _generar_pdf() {

    var filtros = new Object();//   variable con la que se le pasara la informacion al controlador

    try {


        //  filtro para el folio
        if ($.trim($('#cmb_busqueda_factura :selected').val()) !== '') {
            filtros.Folio_Cheque = parseInt($('#cmb_busqueda_factura :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Folio_Cheque = 0;
        }


        //  filtro para el folio
        if ($.trim($('#cmb_anticipo_factura :selected').val()) !== '') {
            filtros.Anticipo_Id = parseInt($('#cmb_anticipo_factura :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Anticipo_Id = 0;
        }


        //  filtros para la fecha
        filtros.Fecha_Inicio_Texto = ($('#txt_fecha_inicio').val() !== '' || $('#txt_fecha_inicio').val() == undefined) ? ($('#txt_fecha_inicio').val()) : "";
        filtros.Fecha_Termino_Texto = ($('#txt_fecha_termino').val() !== '' || $('#txt_fecha_termino').val() == undefined) ? ($('#txt_fecha_termino').val()) : "";

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la petición  
        $.ajax({
            type: 'POST',
            url: 'controllers/Anticipos_Controller.asmx/Genere_PDF',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que tenga informacion la variable
                if (datos !== null) {

                    var result = datos.d;// variable en que se recibe la informacion proveniente del controlador

                    //  se abre una pantalla con el reporte en pdf
                    window.open(result.Url_PDF, "", "", true);


                    /* =============================================
                    --NOMBRE_FUNCIÓN:       setTimeout
                    --DESCRIPCIÓN:          evento que busca el archivo pdf y lo borra de la carpeta de reportes
                    --PARÁMETROS:           NA
                    --CREO:                 Hugo Enrique Ramírez Aguilera
                    --FECHA_CREO:           24 de Ocutbre de 2019
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    setTimeout(function () {
                        $.ajax({
                            url: '../Reporting/Frm_Eliminar_Archivos.aspx?accion=delete_pdf&url_pdf=' + result.Url_PDF,
                            type: 'POST',
                            async: false,
                            cache: false,
                            contentType: 'application/json; charset=utf-8',
                            datatype: 'json',
                            success: function (data) { }
                        });
                    }, 3000);
                }
            }
        });
    }
    catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [_generar_pdf] ', e.message);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _generar_excel
--DESCRIPCIÓN:          genere el formato de excel de la informacion de la factura
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           25 de Septiembre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _generar_excel() {

    var filtros = new Object();//   variable con la que se le pasara la informacion al controlador

    try {

        //  filtro para el folio
        if ($.trim($('#cmb_busqueda_factura :selected').val()) !== '') {
            filtros.Folio_Cheque = parseInt($('#cmb_busqueda_factura :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Folio_Cheque = 0;
        }

        //  filtro para el folio
        if ($.trim($('#cmb_anticipo_factura :selected').val()) !== '') {
            filtros.Anticipo_Id = parseInt($('#cmb_anticipo_factura :selected').val());
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Anticipo_Id = 0;
        }

        //  filtro para la referencia
        if ($.trim($('#cmb_busqueda_tipo :selected').val()) !== '') {
            filtros.Referencia_Pago = $('#cmb_busqueda_tipo :selected').val();
        }
            //  validamos que no este seleccionado, el valor enviado sera cero
        else {
            filtros.Referencia_Pago = "";
        }


        //  filtros para la fecha
        filtros.Fecha_Inicio_Texto = ($('#txt_fecha_inicio').val() !== '' || $('#txt_fecha_inicio').val() == undefined) ? ($('#txt_fecha_inicio').val()) : "";
        filtros.Fecha_Termino_Texto = ($('#txt_fecha_termino').val() !== '' || $('#txt_fecha_termino').val() == undefined) ? ($('#txt_fecha_termino').val()) : "";

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Anticipos_Controller.asmx/Genere_Excel',
            data: $data,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: false,
            cache: false,
            success: function (datos) {

                //  se valida que tenga informacion recibida la variable
                if (datos !== null) {

                    var result = datos.d;// variable en que se recibe la informacion proveniente del controlador

                    //  se descarga el archivo
                    window.location = "../Ayudante/Frm_Ayudante_Descarga_Excel.aspx?Url=" + result.Url_Excel + "&Nombre=" + result.Nombre_Excel;


                    /* =============================================
                    --NOMBRE_FUNCIÓN:       setTimeout
                    --DESCRIPCIÓN:          evento que busca el archivo pdf y lo borra de la carpeta de reportes
                    --PARÁMETROS:           NA
                    --CREO:                 Hugo Enrique Ramírez Aguilera
                    --FECHA_CREO:           24 de Ocutbre de 2019
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    //  se elimina el archivo
                    setTimeout(function () {
                        $.ajax({
                            url: '../Reporting/Frm_Eliminar_Archivos.aspx?accion=delete_pdf&url_pdf=' + result.Ruta_Archivo_Excel,
                            type: 'POST',
                            async: false,
                            cache: false,
                            contentType: 'application/json; charset=utf-8',
                            datatype: 'json',
                            success: function (data) { }
                        });
                    }, 5000);
                }
            }
        });
    }
    catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [_generar_excel] ', e.message);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       btn_pagos_click
--DESCRIPCIÓN:          Muestra el modal del anticipo
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_pagos_click(renglon) {
    //  se obtiene la información del renglón de la tabla
    var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se limpian los objetos
    _limpiar_todos_controles_modal_pagos();

    //  se asigna el id
    $('#txt_factura_pagos_id').val(row.Factura_Id);

    //  se consultan los anticipos
    _consultar_informacion_pagos(row.Factura_Id);

    //  se manda llamar al modal
    _launch_modal_pagos('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Conceptos de la Factura [' + row.Folio + '] [' + row.Concepto_Xml + ']');
}



/* =============================================
--NOMBRE_FUNCIÓN:       btn_anticipo_click
--DESCRIPCIÓN:          Muestra el modal del anticipo
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_anticipo_click(renglon) {
    //  se obtiene la información del renglón de la tabla
    var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se limpian los objetos
    _limpiar_todos_controles_modal_anticipo();

    //  se asigna el id
    $('#txt_factura_anticipo_id').val(row.Factura_Id);

    //  se consultan los anticipos
    _consultar_informacion_anticipos();

    //  se manda llamar al modal
    _launch_modal_anticipo('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Anticipo Factura [' + row.Folio + '] [' + row.Concepto_Xml + ']');
}
