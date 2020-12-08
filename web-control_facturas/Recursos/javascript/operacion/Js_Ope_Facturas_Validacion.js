$(document).on('ready', function (e) {

    _load_vistas();

});


/* =============================================
--NOMBRE_FUNCIÓN:       _load_vistas
--DESCRIPCIÓN:          Carga las paginas HTML dentro del documento   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_vistas() {

    //  se ejecutan las paginas HTML que se estarán cargando dentro del aspx
    _launchComponent('vistas/Validaciones/Principal.html', 'Principal');
    _launchComponent('vistas/Validaciones/Adjuntar.html', 'Adjuntar');


}


/* =============================================
--NOMBRE_FUNCIÓN:       _launchComponent
--DESCRIPCIÓN:          Carga los eventos y funciones que tendrá cada pagina HTML
--PARÁMETROS:           component: ruta del archivo HTML
--                      id: Nombre que se le dará a la pagina HTML
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
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
                _inicializar_pagina();
                break;

            case 'Modal':
                //  se carga los eventos y funciones
                //_inicializar_vista_modal();
                break;

            case 'Historico':
                //  se carga los eventos y funciones
                //_inicializar_vista_modal_historico();
                break;

            case 'Adjuntar':
                //  se carga los eventos y funciones
                _inicializar_vista_adjuntar();
                break;

            case 'Anticipo':
                //  se carga los eventos y funciones
                //_inicializar_vista_anticipo();
                break;

        }
    });
}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_pagina
--DESCRIPCIÓN:          Carga inicial de la pagina y controles.
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez   
--FECHA_CREO:           22 de Febrero 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_pagina() {
    crear_tabla_facturas();
    _set_location_toolbar('toolbar');
    _load_cmb_validaciones_usuarios();
    _load_folios_cheques();
    _eventos();
    _consultar_informacion();
}



/* =============================================
--NOMBRE_FUNCIÓN:       _mostrar_vista
--DESCRIPCIÓN:          Permite mostrar u ocultar los div correspondientes a los HTML
--PARÁMETROS:           vista_: Nombre de acción que se estará realizando
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
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
            $('#Modal').hide();
            $('#Principal').show();
            break;

        case "Modal Nuevo":
            //  se habilita la seccion de operacion del formularios
            $('#Modal').show();
            $('#Principal').hide();


            $('#div_botones_captura').show();
            $('#btn_guardar_seguir_captura').show();
            $('#div_botones_editar').hide();

            break;

        case "Modal Editar":
            //  se habilita la seccion de operacion del formularios
            $('#Modal').show();
            $('#Principal').hide();

            $('#div_botones_captura').show();

            $('#btn_guardar_seguir_captura').hide();
            $('#div_botones_editar').hide();

            break;
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _eventos
--DESCRIPCIÓN:          Eventos para los botones de la pantalla;
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez   
--FECHA_CREO:           22 de Febrero 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos() {
    $('#btn_busqueda').on('click', function (e) {
        e.preventDefault();
        _consultar_informacion();
    });

    $('#btn_inicio').on('click', function (e) {
        e.preventDefault();

        //  se regresa al formulario principal
        window.location.href = '../Paginas_Generales/Frm_Apl_Principal.aspx';
    });

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
--NOMBRE_FUNCIÓN:       crear_tabla_facturas
--DESCRIPCIÓN:          Genere la estructura de la tabla de facturas
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez   
--FECHA_CREO:           22 de Febrero 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_facturas() {

    try {
        //  se destruye la tabla
        $('#tbl_facturas').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_facturas').bootstrapTable({
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

            detailView: true,
            onExpandRow: function (index, row, $detail) {
                expandir_tabla($detail, row);
            },
            columns: [


                { field: 'Folio_Cheque', title: 'Folio solicitud de cheque', align: 'left', valign: 'top', visible: true, sortable: true, },
                { field: 'Estatus', title: 'Estatus', align: 'left', valign: 'top', width: 100, visible: true, sortable: true, },



                {
                    //  cheque
                    field: 'Cheque',
                    title: 'Cheque',
                    visible: false,
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
                        opciones += '<div style="display:block">' +
                            '<a class="remove ml10 text-blue" id="' + row.Factura_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_soliciitud_cheque_click(this);" title="Solicitud de cheque">' +
                            '   <i class="glyphicon glyphicon-credit-card"></i>' +
                            '       &nbsp;<span style="font-size:11px !important;"></span>' +
                            '</a>' +
                            '</div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },


                {
                    //  Resumen
                    field: 'Resumen',
                    title: 'Resumen',
                    visible: false,
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
                        opciones += '<div style="display:block">' +
                            '<a class="remove ml10 text-blue" id="' + row.Factura_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                            '\' onclick="btn_hoje_resumen_click(this);" title="Hoja de resumen">' +
                            '   <i class="glyphicon glyphicon-folder-close"></i>' +
                            '       &nbsp;<span style="font-size:11px !important;"></span>' +
                            '</a>' +
                            '</div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },

                {
                    //  autorizacion
                    field: 'Autorizacion',
                    title: 'Autorización',
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


                        opciones += '<div style="display:block">' +
                            '<a style="font-size:15px;" class="remove ml10 text-success" id="' + row.Factura_Id +
                            '       " href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                            '\' onclick="btn_autorizar_click(this);" title="Autorizar">' +
                            '   <i class="glyphicon glyphicon-ok"></i>' +
                            '       &nbsp;<span style="font-size:11px !important;"></span>' +
                            '</a>';


                        opciones += '<a style="font-size:15px;" class="remove ml10 text-danger" id="' + row.Factura_Id +
                            '       " href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                            '\' onclick="btn_rechazar_click(this);" title="Rechazar">' +
                            '   <i class="glyphicon glyphicon-remove"></i>' +
                            '       &nbsp;<span style="font-size:11px !important;"></span>' +
                            '</a>' +
                            '</div>';



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
--NOMBRE_FUNCIÓN:       expandir_tabla
--DESCRIPCIÓN:          Genere el llamado para la creacion de la estructura de la tabla detalle
--PARÁMETROS:           $detail: Estructura principal de la tabla
--                      row: datos del renglon de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function expandir_tabla($detail, row) {
    crear_tabla_principal_detalle($detail.html('<table class="table table-responsive header-subtable"></table>').find('table'), row);
}


/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_principal_detalle
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           $tabla: estructura de la tabla
--                      rows. datos del renglon de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_principal_detalle($tabla, rows) {

    var filtros = new Object();//   estructura en donde se cargaran la información del formulario
    var datos = [];//   variable para almacenar la informacion de la consulta de las facturas

    try {

        //  se carga la información 
        filtros.Folio_Cheque = parseInt(rows.Folio_Cheque);
        filtros.Folio = "";
        filtros.Factura_Id = 0;
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
                datos = JSON.parse(result.d);
            }
        });

        //  se destruye la tabla
        $tabla.bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $tabla.bootstrapTable({
            cache: false,
            striped: true,
            pagination: false,
            smartDisplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            showHeader: true,
            minimumCountColumns: 2,
            columns: [
                { field: 'Factura_Id', title: 'Factura_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Concepto_Texto_Id', title: 'Tipo de Concepto', width: 110, align: 'left', valign: 'top', visible: true },
                { field: 'Folio', title: 'Folio', align: 'left', valign: 'top', width: 50, visible: true, sortable: true, },
                { field: 'Concepto_Xml', title: 'Concepto Xml', width: 250, align: 'left', valign: 'top', visible: true },
                { field: 'Pedimento', title: 'Pedimento', width: 150, align: 'left', valign: 'top', visible: true },
                { field: 'Estatus', title: 'Estatus', align: 'left', valign: 'top', width: 70, visible: true, sortable: true },
                {
                    field: 'Total_Pagar', title: 'Total', align: 'right', valign: 'top', width: 80, visible: true, sortable: true,
                    formatter: function (value, row) {
                        return accounting.formatMoney(value);
                    },
                },
                { field: 'Validacion', title: 'Validacion', align: 'left', valign: 'top', width: 70, visible: true, sortable: true, },

                {
                    //  Adjuntos
                    field: 'Adjuntar',
                    title: 'Adjuntar',
                    width: 60,
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
                        opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id +
                            '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                            '\' onclick="btn_adjuntar_archivo_click(this);" title="Adjuntar">' +
                            '<i class="fa fa-upload"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },
            ],
            data: datos,
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_principal_detalle] ', e.message);
    }
}


/* =============================================
  --NOMBRE_FUNCIÓN:       btn_soliciitud_cheque_click
  --DESCRIPCIÓN:          Se llama al evento que genera el pdf de la solicitud de cheque
  --PARÁMETROS:           e: parametro que se refiere al evento click
  --CREO:                 Hugo Enrique Ramírez Aguilera
  --FECHA_CREO:           25 de Septiembre de 2019
  --MODIFICÓ:
  --FECHA_MODIFICÓ:
  --CAUSA_MODIFICACIÓN:
  =============================================*/
function btn_soliciitud_cheque_click(tab) {

    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se genera el formato
    _generar_solicitud_cheque_pdf(row.Folio_Cheque);
}


/* =============================================
  --NOMBRE_FUNCIÓN:       btn_hoje_resumen_click
  --DESCRIPCIÓN:          Se llama al evento que genera el excel del resumen de la factura
  --PARÁMETROS:           e: parametro que se refiere al evento click
  --CREO:                 Hugo Enrique Ramírez Aguilera
  --FECHA_CREO:           25 de Septiembre de 2019
  --MODIFICÓ:
  --FECHA_MODIFICÓ:
  --CAUSA_MODIFICACIÓN:
  =============================================*/
function btn_hoje_resumen_click(tab) {

    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se crea el archivo de excel
    _generar_excel(row.Folio_Cheque);

}


/* =============================================
--NOMBRE_FUNCIÓN:       _generar_solicitud_cheque_pdf
--DESCRIPCIÓN:          genere el formato de pdf de la informacion
--PARÁMETROS:           Folio: Valor del folio de la factura
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           25 de Septiembre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _generar_solicitud_cheque_pdf(folio) {

    var filtros = new Object();//   variable con la que se le pasara la informacion al controlador

    try {
        //  se asignan los valores
        filtros.Folio_Cheque = parseInt(folio);
        filtros.Factura_Id = 0;

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la petición  
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Genere_Solicitud_Cheque_PDF',
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
                    }, 10000);
                }
            }
        });
    }
    catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [_generar_solicitud_cheque_pdf] ', e.message);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _generar_excel
--DESCRIPCIÓN:          genere el formato de excel de la informacion de la factura
--PARÁMETROS:           folio: Valor del numero de folio
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           25 de Septiembre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _generar_excel(folio) {

    var filtros = new Object();//   variable con la que se le pasara la informacion al controlador

    try {
        //  se carga la informacion
        filtros.Factura_Id = 0;
        filtros.Folio_Cheque = parseInt(folio);

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Faturas_Manuales_Controller.asmx/Genere_Reporte_Excel_Hoja_Resumen',
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
                    }, 3000);
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
--NOMBRE_FUNCIÓN:       btn_adjuntar_archivo_click
--DESCRIPCIÓN:          Muestra el modal de los datos adjuntos de la factura
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Abril de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_adjuntar_archivo_click(renglon) {
    //  se obtiene la información del renglón de la tabla
    var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

    $('#txt_factura_adjunto_id').val(row.Factura_Id);

    //  se consultan los anexos de la factura
    _consultar_anexos();

    //  se vamnda llamar al modal
    _launch_modal_adjuntos('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Adjuntar');
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_autorizar_click
--DESCRIPCIÓN:          Se realiza la acción de autorizar la factura
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_autorizar_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla


        //  se crea el objeto de confirmación
        bootbox.confirm({
            title: 'Autorizar factura',
            message: 'Esta seguro de AUTORIZAR el folio de solicitud de cheque [' + row.Folio_Cheque + ']?',
            callback: function (result) {

                //  validamos que accion tomo el usuario
                if (result) {

                    //  se declara la variable
                    var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                    //  se asignan los elementos al objeto de filtro
                    obj.Factura_Id = 0;
                    obj.Folio_Cheque = parseInt(row.Folio_Cheque);
                    obj.Validacion_Id = parseInt(row.Validacion_ID);
                    obj.Orden_Validacion = parseInt(row.Orden);

                    //  se convierte la información a json
                    var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                    //  se ejecuta la petición
                    $.ajax({
                        type: 'POST',
                        url: 'controllers/Facturas_Controller.asmx/Autorizacion',
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
                                    _consultar_informacion();

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
        _mostrar_mensaje('Error Técnico' + ' [btn_autorizar_click] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_rechazar_click
--DESCRIPCIÓN:          Se realiza la acción de rechazar factura
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_rechazar_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        //  se crea el objeto de confirmación
        bootbox.confirm({
            title: 'Rechazar Factura',
            message: 'Esta seguro de RECHAZAR el folio de solicitud de cheque [' + row.Folio_Cheque + ']?',
            callback: function (result) {

                //  validamos que accion tomo el usuario
                if (result) {


                    //  se muestra opcion para escribir el motivo del rechazo
                    bootbox.prompt({
                        title: 'MOTIVO DEL PORQUE FUE RECHAZADA LA FACTURA',
                        callback: function (resultado_) {

                            //  validamos que tenga informacion
                            if (resultado_) {

                                //  se declara la variable
                                var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                                //  se asignan los elementos al objeto de filtro
                                obj.Factura_Id = 0;
                                obj.Folio_Cheque = parseInt(row.Folio_Cheque);
                                obj.Validacion_Id = parseInt(row.Validacion_ID);
                                obj.Orden_Validacion = parseInt(row.Orden);
                                //obj.Folio = parseInt(row.Folio);
                                obj.Motivo = resultado_;
                                //obj.Concepto = row.Concepto_Xml;


                                //  se convierte la información a json
                                var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                                //  se ejecuta la petición
                                $.ajax({
                                    type: 'POST',
                                    url: 'controllers/Facturas_Controller.asmx/Rechazar',
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
                                                _consultar_informacion();

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
                }
            }
        });


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [btn_rechazar_click] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_informacion
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de la tabla
--PARÁMETROS:           NA
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           22 de Feb 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_informacion() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        filtros.Folio_Cheque = $('#cmb_folio_cheque_filtro').val();
        filtros.Validacion_ID = $('#cmb_validaciones_filtro').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador
               
        //  se realiza la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Validacion_Folios_Controller.asmx/Consultar_Folios_Pendientes_Validar_Filtros',
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



    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_informacion]', e);
    }

}


/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_filtro_estatus
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_folios_cheques() {
    try {
        var obj = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se consultara y cargara la información
        $('#cmb_folio_cheque_filtro').select2({
            language: "es",
            theme: "classic",
            placeholder: 'SELECCIONE',
            allowClear: true,
            ajax: {
                url: 'controllers/Facturas_Controller.asmx/Consultar_Facturas_Nombre_Combo',
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
--NOMBRE_FUNCIÓN:       _load_cmb_validaciones_usuarios
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           22 de Febrero 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_validaciones_usuarios() {
    try {
        var obj = new Object();//   se le asignaran a las propiedades los filtros de búsqueda

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se consultara y cargara la información
        $('#cmb_validaciones_filtro').select2({
            language: "es",
            theme: "classic",
            placeholder: 'SELECCIONE',
            allowClear: true,
            ajax: {
                url: 'controllers/Validacion_Folios_Controller.asmx/Consultar_Validaciones_Usuario',
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
            }, templateResult: _templeteResultValidaciones
        });


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_load_cmb_filtro_nombre]', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _templeteResultValidaciones
--DESCRIPCIÓN:          Funcion para dar formato al combo al desplegar la lista 
--PARÁMETROS:           row: datos del item de la lista 
--CREO:                 Jose Maldonado Mendez
--FECHA_CREO:           22 de Febrero 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _templeteResultValidaciones(row) {
    if (!row.id) {
        return row.text;
    } else if (row.id === row.text) {
        return row.text;
    } else {
        var _salida = '<div class="row">' +
            '<div class="col-md-9">' +
            '<span style="">' +
            row.text +
            '<br />' +
            '<b style="font-size: x-small;">&nbsp;Tipo: ' + row.detalle_1 + '</b>' +
            '</span>' +
            '</div>' +
            '</div>';

        return $(_salida);
    }
}
