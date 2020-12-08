var Permiso_Captura = false; //Variable que almacena si el usuario tiene permiso de captura de facturas.
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
    _launchComponent('vistas/Facturas/Principal.html', 'Principal');
    _launchComponent('vistas/Facturas/Modal.html', 'Modal');
    _launchComponent('vistas/Facturas/Historico.html', 'Historico');
    _launchComponent('vistas/Facturas/Adjuntar.html', 'Adjuntar');
    _launchComponent('vistas/Facturas/Anticipo.html', 'Anticipo');
    _launchComponent('vistas/Facturas/Conceptos_Xml.html', 'Conceptos_Xml');

    
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

            case 'Modal':
                //  se carga los eventos y funciones
                _inicializar_vista_modal();
                break;

            case 'Historico':
                //  se carga los eventos y funciones
                _inicializar_vista_modal_historico();
                break;

            case 'Adjuntar':
                //  se carga los eventos y funciones
                _inicializar_vista_adjuntar();
                break;

            case 'Anticipo':
                //  se carga los eventos y funciones
                _inicializar_vista_anticipo();
                break;

            case 'Conceptos_Xml':
                //  se carga los eventos y funciones
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

        //  se consultan los parametros
        //_consultar_parametros();

        //  consulta los valores del usuario
        _consultar_datos_usuario();

        //  se crea la tabla principal
        crear_tabla_principal();

       
        //  se cargan los eventos principales de los botones
        _eventos_principal();

        //  se estable el estilo de los controles check
        //_inicializar_icheck();

        //  se muestra la vista principal
        _mostrar_vista('Principal');

        //  se cargan los combos filtro
        _load_cmb_filtro_nombre('cmb_busqueda_nombre');
        _load_cmb_filtro_estatus('cmb_busqueda_estatus');
        
        //  habilitamos el boton de nuevo
        _validacion_habilitar_nuevas_facturas();

        //  se da formato al toolbar de búsqueda de la tabla
        _set_location_toolbar('toolbar');

        //  se consulta la informacion del catalogo
        _consultar_informacion();


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_inicializar_vista_principal] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_modal
--DESCRIPCIÓN:          Evento con el que se cargan los eventos y funciones de la vista modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_modal() {
    try {

        //  se inicializan los eventos
        _eventos_modal();

        //  se crea la estructura de la tabla
        crear_tabla_cuentas();

        //  se carga la informacion en el combo
        _load_cmb_facturas_general_mills('cmb_folio');
        _load_cmb_conceptos_catalogo('cmb_tipo_concepto');
        _load_cmb_entidad('cmb_entidad');
        _load_cmb_cuentas('cmb_cuenta');

        //  se bloquea el control
        $('#cmb_entidad').attr('disabled', 'disabled');

        //  valores para los controles de fecha
        _inicializar_fecha_revision();
        _inicializar_fecha_anticipo();

        //  se establecen las propiedades del control fileinput
        _inicializar_controls_file_operacion();
        
        //  se limpian los controles
        _limpiar_todos_controles_modal();

        //  se habilitan solo los numeros
        _keyDownInt('txt_monto_pago');

        //  *******************************************************************************************************************************
        //  *******************************************************************************************************************************
        //  seccion de anticipos
        //  *******************************************************************************************************************************
        //  cargamos la informacion del combo
        _load_cmb_anticipos_catalogo('cmb_anticipo');

        //  se crea la tabla de los anticipos
        crear_tabla_anticipos_operacion();
        crear_tabla_anticipos_eliminados_operacion();

        //  se asigna solo numeros a los controles
        _keyDownInt('txt_saldo_anticipo');
        _keyDownInt('txt_monto_anticipo');
        //  *******************************************************************************************************************************
        //  *******************************************************************************************************************************

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_inicializar_vista_modal] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_parametros
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de los controles
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_parametros() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda


        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Consultar_Parametros',
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
                    var result = JSON.parse(datos.d);// almacenara los datos recibidos

                    //  carga la informacion en los controles
                    $('#txt_area_parametro_id').val(result[0].Area_Id_Captura_Factura);
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_parametros]', e);
    }

}



/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_datos_usuario
--DESCRIPCIÓN:          Consulta la informacion de la base de datos  y la carga Dentro de los controles
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_datos_usuario() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {

        filtros = new Object();//   se le asignaran a las propiedades los filtros de búsqueda


        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Consultar_Datos_Sesion',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que exista información en los valores recibidos
                if (datos !== null) {
                    
                    //  se carga la información
                    var result = JSON.parse(datos.d);// almacenara los datos recibidos
                    if (result.Estatus == "success") {
                        Permiso_Captura = true;
                        
                    } else {
                        Permiso_Captura = false;
                        _mostrar_mensaje("Permiso de Captura", result.Mensaje);
                    }
                }
            }
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_datos_usuario]', e);
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
--NOMBRE_FUNCIÓN:       _validacion_habilitar_nuevas_facturas
--DESCRIPCIÓN:          Habilita/Desahabilita los botones de guardar
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _validacion_habilitar_nuevas_facturas() {
    var area_parametro = 0;//   variable para obtener el id del area dentro de los parametros
    var area = 0;//   variable para obtener el id del area


    //  se obtiene los valores
    area_parametro = $('#txt_area_parametro_id').val();
    area = $('#txt_area_id').val();


    crear_tabla_principal();
    _set_location_toolbar('toolbar');
    //  validamos que los id sea iguales, se mostrara el boton de nuevo
    if (Permiso_Captura) {

        _set_location_toolbar('toolbar');
        $('#btn_nuevo').show();
        $('#btn_leer_archivo').show();
        $('#btn_guardar_anticipo').show();
        $('#btn_guardar').show();
        $('#btn_agregar_anticipo').show();
        $('#btn_agregar_cuenta').show();

        $('#cmb_busqueda_nombre').attr('disabled', false);
        $('#cmb_busqueda_estatus').attr('disabled', false);

        //  se deshabilita la control para indicar que es validador
        $('#chk_validador').iCheck('uncheck');//    original
        //$('#chk_validador').iCheck('check');//    desarrollo

        $('#tbl_facturas').bootstrapTable('showColumn', 'Editar');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Envio');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Anticipos');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Historico');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Adjuntar');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Cheque');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Resumen');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Autorizacion');

        $('#tbl_facturas_cuentas').bootstrapTable('showColumn', 'Eliminar');
        $('#tbl_facturas_anticipos_operacion').bootstrapTable('showColumn', 'Eliminar');

        $('#tbl_facturas_adjuntos').bootstrapTable('showColumn', 'Eliminar');

    }
        //  se oculta el boton de nuevo
    else {

        //crear_tabla_principal_evaluacion();
        _set_location_toolbar('toolbar');

        $('#btn_nuevo').hide();
        $('#btn_leer_archivo').hide();
        $('#btn_guardar_anticipo').hide();
        $('#btn_guardar').hide();
        $('#btn_agregar_anticipo').hide();
        $('#btn_agregar_anticipo').hide();
        $('#btn_agregar_cuenta').hide();

        //  se habilita la control para indicar que es validador
        $('#chk_validador').iCheck('check');

        $('#cmb_busqueda_nombre').attr('disabled', 'disabled');
        $('#cmb_busqueda_estatus').attr('disabled', 'disabled');


        $('#tbl_facturas_cuentas').bootstrapTable('hideColumn', 'Eliminar');
        $('#tbl_facturas_anticipos_operacion').bootstrapTable('hideColumn', 'Eliminar');

        $('#tbl_facturas_adjuntos').bootstrapTable('hideColumn', 'Eliminar');
        

        $('#tbl_facturas').bootstrapTable('hideColumn', 'Nuevo');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Editar');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Envio');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Anticipos');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Historico');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Adjuntar');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Cheque');
        $('#tbl_facturas').bootstrapTable('hideColumn', 'Resumen');
        $('#tbl_facturas').bootstrapTable('showColumn', 'Autorizacion');
    }
    
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
--NOMBRE_FUNCIÓN:       crear_tabla_principal_evaluacion
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_principal_evaluacion() {

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

            columns: [
                { field: 'Factura_Id', title: 'Factura_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Concepto_Id', title: 'Concepto_Id', align: 'center', valign: 'top', visible: false },

                { field: 'Concepto_Texto_Id', title: 'Tipo de Concepto', width: 110, align: 'left', valign: 'top', visible: true },
                { field: 'Folio', title: 'Folio', align: 'left', valign: 'top', width: 50, visible: true, sortable: true, },
                 { field: 'Concepto_Xml', title: 'Concepto Xml', width: 250, align: 'left', valign: 'top', visible: true },
                 { field: 'Pedimento', title: 'Pedimento', width: 150, align: 'left', valign: 'top', visible: true },

                { field: 'Referencia_Interna', title: 'Referencia int.', align: 'left', valign: 'top', width: 50, visible: false, sortable: true, },
                { field: 'Referencia', title: 'Referencia', align: 'left', valign: 'top', width: 50, visible: false, sortable: true, },

                { field: 'Fecha_Recepcion', title: 'Fecha de recepción', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },
                { field: 'Fecha_Factura', title: 'Fecha de factura', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },

                { field: 'Fecha_Recepcion_Texto', title: 'Fecha de recepción', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },
                { field: 'Fecha_Factura_Texto', title: 'Fecha de factura', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },

                { field: 'Estatus', title: 'Estatus', align: 'left', valign: 'top', width: 70, visible: true, sortable: true, },

                {
                    field: 'Subtotal', title: 'Subtotal', align: 'right', valign: 'top', width: 50, visible: false, sortable: true,

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
                    field: 'IVA', title: 'IVA', align: 'right', valign: 'top', width: 50, visible: false, sortable: true,
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
                    field: 'Retencion', title: 'Retención', align: 'right', valign: 'top', width: 50, visible: false, sortable: true,
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


                { field: 'Validacion', title: 'Validacion', align: 'left', valign: 'top', width: 70, visible: true, sortable: true, },
                { field: 'Motivo', title: 'Motivo rechazo', align: 'left', valign: 'top', width: 70, visible: false, sortable: true, },

                {
                    //  editar
                    field: 'Editar',
                    title: 'Editar',
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

                        //  validamos que el estatus sea de captura
                        if (row.Estatus == "CAPTURA" || row.Estatus == "RECHAZADA") {

                            opciones = '<div style=" text-align: center;">';
                            opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                            opciones += '</div>';
                        }


                        return opciones;
                    }
                },


                {
                    //  enviar a Validacion
                    field: 'Envio',
                    title: 'Env. Val.',
                    width: 50,
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

                        //  validamos que el estatus sea de captura
                        if (row.Estatus == "CAPTURA" || row.Estatus == "RECHAZADA") {


                            opciones = '<div style=" text-align: center;">';
                            opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id +
                                            '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_enviar_validacion_click(this);" title="Enviar a validación"><i class="glyphicon glyphicon-share-alt"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                            opciones += '</div>';
                        }

                        return opciones;
                    }
                },

                {
                    //  Anticipos
                    field: 'Anticipos',
                    title: 'Anticipos',
                    width: 60,
                    visible: false,
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
                                        '\' onclick="btn_anticipo_click(this);" title="Anticipo">' +
                                        '<i class="fa fa-money"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },




                {
                    //  historico
                    field: 'Historico',
                    title: 'Historico',
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
                                        '\' onclick="btn_historico_click(this);" title="Historico">' +
                                        '<i class="fa fa-history"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },


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
                                        '\' onclick="btn_adjuntar_archivo_click(this);" title="Adjuntar">' +
                                        '<i class="fa fa-upload"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },




                {
                    //  cheque
                    field: 'Cheque',
                    title: 'Cheque',
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
                        //  cheque
                        field: 'Resumen',
                        title: 'Resumen',
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

                             // validamos el estatus y el check
                             if ($('#chk_validador').prop('checked') == true) {

                                 opciones += '<div style="display:block">' +
                                                 '<a class="remove ml10 text-success" id="' + row.Factura_Id +
                                                '       " href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                                        '\' onclick="btn_autorizar_click(this);" title="Autorizar">' +
                                                 '   <i class="glyphicon glyphicon-ok"></i>' +
                                                 '       &nbsp;<span style="font-size:11px !important;"></span>' +
                                                 '</a>';


                                 opciones += '<a class="remove ml10 text-danger" id="' + row.Factura_Id +
                                                '       " href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                                        '\' onclick="btn_rechazar_click(this);" title="Rechazar">' +
                                                '   <i class="glyphicon glyphicon-remove"></i>' +
                                                '       &nbsp;<span style="font-size:11px !important;"></span>' +
                                                '</a>' +
                                           '</div>';

                             }

                             opciones += '</div>';

                             return opciones;
                         }
                     },
            ]
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_principal_evaluacion] ', e.message);
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


                { field: 'Folio', title: 'Folio solicitud de cheque', align: 'left', valign: 'top', visible: true, sortable: true, },
                { field: 'Estatus', title: 'Estatus', align: 'left', valign: 'top', width: 100, visible: true, sortable: true, },
                { field: 'Validacion_Id', title: 'Validacion_Id', align: 'left', valign: 'top', width: 100, visible: false, sortable: true, },
                { field: 'Orden_Validacion', title: 'Orden_Validacion', align: 'left', valign: 'top', width: 100, visible: false, sortable: true, },

                {
                    //  Resumen
                    field: 'Nuevo',
                    title: 'Nuevo',
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


                        if (row.Estatus == "CAPTURA" || row.Estatus == "RECHAZADA") {
                            opciones = '<div style=" text-align: center;">';
                            opciones += '<div style="display:block">' +
                                            '<a class="remove ml10 text-purple" id="' + row.Folio_Cheque + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                                '\' onclick="btn_nuevo_concepto_click(this);" title="Nuevo">' +
                                            '   <i class="glyphicon glyphicon-file"></i>' +
                                            '       &nbsp;<span style="font-size:11px !important;"></span>' +
                                            '</a>' +
                                        '</div>';



                            opciones += '</div>';
                        }


                        return opciones;
                    }
                },

                {
                    //  enviar a Validacion
                    field: 'Envio',
                    title: 'Env. Val.',
                    width: 50,
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

                        ////  validamos que el estatus sea de captura
                        if (row.Estatus == "CAPTURA" || row.Estatus == "RECHAZADA") {


                            opciones = '<div style=" text-align: center;">';
                            opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id +
                                            '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_enviar_validacion_click(this);" title="Enviar a validación"><i class="glyphicon glyphicon-share-alt"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                            opciones += '</div>';
                        }

                        return opciones;
                    }
                },

                {
                    //  cheque
                    field: 'Cheque',
                    title: 'Cheque',
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

                        // validamos el estatus y el check
                        if ($('#chk_validador').prop('checked') == true) {

                            opciones += '<div style="display:block">' +
                                            '<a class="remove ml10 text-success" id="' + row.Factura_Id +
                                        '       " href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                                '\' onclick="btn_autorizar_click(this);" title="Autorizar">' +
                                            '   <i class="glyphicon glyphicon-ok"></i>' +
                                            '       &nbsp;<span style="font-size:11px !important;"></span>' +
                                            '</a>';


                            opciones += '<a class="remove ml10 text-danger" id="' + row.Factura_Id +
                                        '       " href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                                '\' onclick="btn_rechazar_click(this);" title="Rechazar">' +
                                        '   <i class="glyphicon glyphicon-remove"></i>' +
                                        '       &nbsp;<span style="font-size:11px !important;"></span>' +
                                        '</a>' +
                                    '</div>';

                        }

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

//  -----------------------------------------------------
//  -----------------------------------------------------

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
        filtros.Folio_Cheque = parseInt(rows.Folio);
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
                { field: 'Concepto_Id', title: 'Concepto_Id', align: 'center', valign: 'top', visible: false },

                { field: 'Concepto_Texto_Id', title: 'Tipo de Concepto', width: 110, align: 'left', valign: 'top', visible: true },
                { field: 'Folio', title: 'Folio', align: 'left', valign: 'top', width: 50, visible: true, sortable: true, },
                 { field: 'Concepto_Xml', title: 'Concepto Xml', width: 250, align: 'left', valign: 'top', visible: true },
                 { field: 'Pedimento', title: 'Pedimento', width: 150, align: 'left', valign: 'top', visible: true },

                { field: 'Referencia_Interna', title: 'Referencia int.', align: 'left', valign: 'top', width: 50, visible: false, sortable: true, },
                { field: 'Referencia', title: 'Referencia', align: 'left', valign: 'top', width: 50, visible: false, sortable: true, },

                { field: 'Fecha_Recepcion', title: 'Fecha de recepción', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },
                { field: 'Fecha_Factura', title: 'Fecha de factura', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },

                { field: 'Fecha_Recepcion_Texto', title: 'Fecha de recepción', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },
                { field: 'Fecha_Factura_Texto', title: 'Fecha de factura', align: 'left', valign: 'top', width: 50, visible: false, sortable: true },

                { field: 'Estatus', title: 'Estatus', align: 'left', valign: 'top', width: 70, visible: true, sortable: true, },

                {
                    field: 'Subtotal', title: 'Subtotal', align: 'right', valign: 'top', width: 50, visible: false, sortable: true,

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
                    field: 'IVA', title: 'IVA', align: 'right', valign: 'top', width: 50, visible: false, sortable: true,
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
                    field: 'Retencion', title: 'Retención', align: 'right', valign: 'top', width: 50, visible: false, sortable: true,
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


                { field: 'Validacion', title: 'Validacion', align: 'left', valign: 'top', width: 70, visible: true, sortable: true, },
                { field: 'Motivo', title: 'Motivo rechazo', align: 'left', valign: 'top', width: 70, visible: false, sortable: true, },

                {
                    //  editar
                    field: 'Editar',
                    title: 'Editar',
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

                        //  validamos que el estatus sea de captura
                        if (row.Estatus == "CAPTURA" || row.Estatus == "RECHAZADA") {

                            opciones = '<div style=" text-align: center;">';
                            opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Factura_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                            opciones += '</div>';
                        }


                        return opciones;
                    }
                },

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
                    --FECHA_CREO:           14 de Febrero del 2020
                    --MODIFICÓ:
                    --FECHA_MODIFICÓ:
                    --CAUSA_MODIFICACIÓN:
                    =============================================*/
                    formatter: function (value, row) {

                        var opciones;//   variable para formar la estructura del boton

                        //  validamos que el estatus sea de captura
                        if (row.Estatus == "CAPTURA" || row.Estatus == "RECHAZADA") {

                            opciones = '<div style=" text-align: center;">';
                            opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="E' + row.Factura_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_concepto_click(this);" title="Eliminar concepto"><i class="glyphicon glyphicon-trash"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';
                            opciones += '</div>';
                        }


                        return opciones;
                    }
                },

               

                {
                    //  Anticipos
                    field: 'Anticipos',
                    title: 'Anticipos',
                    width: 60,
                    visible: false,
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
                                        '\' onclick="btn_anticipo_click(this);" title="Anticipo">' +
                                        '<i class="fa fa-money"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                        opciones += '</div>';

                        return opciones;
                    }
                },




                {
                    //  historico
                    field: 'Historico',
                    title: 'Historico',
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
                                            '\' onclick="btn_historico_click(this);" title="Historico">'+
                                            '<i class="fa fa-history"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                            opciones += '</div>';
                    
                        return opciones;
                    }
                },


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
                                            '\' onclick="btn_adjuntar_archivo_click(this);" title="Adjuntar">' +
                                            '<i class="fa fa-upload"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                            opciones += '</div>';

                            return opciones;
                        }
                    },







            ],
            data: datos,
        });



        var area_parametro = 0;//   variable para obtener el id del area dentro de los parametros
        var area = 0;//   variable para obtener el id del area


        //  se obtiene los valores
        area_parametro = $('#txt_area_parametro_id').val();
        area = $('#txt_area_id').val();



        //  validamos que los id sea iguales, se mostrara el boton de nuevo
        if (area_parametro != area) {


            $($tabla).bootstrapTable('hideColumn', 'Editar');
            $($tabla).bootstrapTable('hideColumn', 'Envio');
            $($tabla).bootstrapTable('hideColumn', 'Anticipos');
            $($tabla).bootstrapTable('hideColumn', 'Historico');
            $($tabla).bootstrapTable('showColumn', 'Adjuntar');
            $($tabla).bootstrapTable('hideColumn', 'Cheque');
            $($tabla).bootstrapTable('hideColumn', 'Resumen');

        }


    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [crear_tabla_principal_detalle] ', e.message);
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
       --NOMBRE_FUNCIÓN:       btn_nuevo
       --DESCRIPCIÓN:          Acciones realizadas al dar click en el botón de nuevo
       --PARÁMETROS:           e: parametro que se refiere al evento click
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           24 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#btn_nuevo').on('click', function (e) {

            e.preventDefault();

            //  se limpian los estilos
            _clear_all_class_error();

            //  se estable el estilo de los controles check
            //_inicializar_icheck();

            //  se limpian los controles
            _limpiar_todos_controles_modal();
            _limpiar_importes_conceptos();
            _limpiar_pagos_y_anticipos();

            $('#btn_leer_archivo_operacion').attr('disabled', false);
            _mostrar_vista('Modal Nuevo');

            //  se habilitan los controles 
            _habilitar_controles("Nuevo");

            //  se habilitan los botones y de oculta el div del adjunto xml
            $('#cmb_folio').prop('disabled', false);
            $('#div_subir_archivo_operacion').hide();

            $('#btn_leer_archivo_automatico').hide();
            //Configurar botones de alta
            _configuracion_controles_conceptos('Nuevo');

            //  se carga el numero de folio
            _consultar_folio_cheque();

            $('#cmb_tipo_concepto').attr('disabled', false);

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
--NOMBRE_FUNCIÓN:       _load_cmb_filtro_estatus
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_cmb_filtro_estatus(cmb) {
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
                url: 'controllers/Facturas_Controller.asmx/Consultar_Facturas_Estatus_Combo',
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
--NOMBRE_FUNCIÓN:       _habilitar_controles
--DESCRIPCIÓN:          Se le otorga un nombre al botón de nuevo con el que se estarán realizando las acciones de alta, modificar
--PARÁMETROS:           opc: sirve para establecer que acciones se realizan al botón
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _habilitar_controles(opc) {

    try {
        //  se le asignara el titulo al botón de nuevo
        switch (opc) {
            case 'Nuevo':
                //  se le otorga el nombre de guardar
                $('#btn_guardar').attr('title', 'Guardar');
                break;

            case 'Modificar':
                //  se le otorga el nombre de actualizar
                $('#btn_guardar').attr('title', 'Actualizar');
                break;
        }

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_habilitar_controles] ', e);
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
    $('#modal_factura span').each(function (index, element) {
        _remove_class_error($(this).attr('id'));
    });
    
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_enviar_validacion_click
--DESCRIPCIÓN:          Se realiza la acción de enviar a validacion
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_enviar_validacion_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        //  se crea el objeto de confirmación
        bootbox.confirm({
            title: 'Enviar a Validacion',
            message: 'Esta seguro de enviar a validacion el folio de solicitud de cheque ' + row.Folio + '?',
            callback: function (result) {

                //  validamos que accion tomo el usuario
                if (result) {

                    //  se declara la variable
                    var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                    //  se asignan los elementos al objeto de filtro
                    obj.Folio_Cheque = parseInt(row.Folio);
                    //obj.Folio = parseInt(row.Folio);
                    //obj.Concepto = row.Concepto_Xml;

                    //  se convierte la información a json
                    var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                    //  se ejecuta la petición
                    $.ajax({
                        type: 'POST',
                        url: 'controllers/Facturas_Controller.asmx/Enviar_Validacion',
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
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_click] ', e);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       btn_historico_click
--DESCRIPCIÓN:          Muestra el historico de las acciones que ha tenido la factura
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_historico_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        //  se muestra el modal
        _launch_modal_historico('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Historico de la factura ' + row.Folio);

        //  se consulta la infromacion del historico
        _consultar_historio(row.Factura_Id);

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [btn_historico_click] ', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       btn_adjuntar_archivo_click
--DESCRIPCIÓN:          Muestra el modal de los datos adjuntos de la factura
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
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

    //  se validan las acciones que puede realizar
    _validacion_habilitar_nuevas_facturas();

    //  se limpian los objetos
    _limpiar_todos_controles_modal_anticipo();

    //  se asigna el id
    $('#txt_factura_anticipo_id').val(row.Factura_Id);

    //  se consultan los anticipos
    _consultar_informacion_anticipos();

    //  se manda llamar al modal
    _launch_modal_anticipo('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Anticipo Factura [' + row.Folio + ']');
}




/* =============================================
--NOMBRE_FUNCIÓN:       btn_editar_click
--DESCRIPCIÓN:          Carga la información del registro de la tabla, carga la información dentro de los controles correspondientes
--PARÁMETROS:           tab: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_editar_click(tab) {
    _limpiar_pagos_y_anticipos();
    _limpiar_importes_conceptos();
    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se validan las acciones que puede realizar
    _validacion_habilitar_nuevas_facturas();

    //  se limpian los estilos
    _clear_all_class_error();

    //  se limpian los controles
    _limpiar_todos_controles_modal();

  

    //  se carga la tipo de concepto
    $('#cmb_tipo_concepto').select2("trigger", "select", {
        data: { id: row.Concepto_Id, text: row.Concepto_Texto_Id }
    });


    //  validamos que tenga informacion el campo de S4Future01
    if (row.S4Future01 == null) {

        //  se carga el folio
        $('#cmb_folio').select2("trigger", "select", {
            data: { detalle_2: row.Folio, text: row.Folio_Filtro }
        });
    }
    else {
        //  se carga el folio
        $('#cmb_folio').select2("trigger", "select", {
            data: { id: row.Folio + row.S4Future01, text: row.Folio_Filtro, detalle_1: row.S4Future01, detalle_2: row.Folio }
        });
    }


    $('#cmb_folio').attr('disabled', 'disabled');

    //  fecha de recepcion
    $('#txt_fecha_recepcion').val(row.Fecha_Recepcion_Texto);
    $('#txt_pedimento').val(row.Pedimento);
    $('#txt_concepto_xml').val(row.Concepto_Xml);
    $('#txt_razon_social').val(row.Razon_Social);

    //  se cargan los totales
    $('#txt_subtotal').html(row.Subtotal).formatCurrency();
    $('#txt_iva').html(row.IVA).formatCurrency();
    $('#txt_retencion').html(row.Retencion).formatCurrency();
    $('#txt_total').html(row.Total_Pagar).formatCurrency();

    $('#txt_folio_solicitud_cheque').val(row.Folio_Cheque);
    $('#txt_folio_asignado').val(row.Folio_Cheque);


    ////  validamos que sea cero el subtotal, se mostrara el div de adjuntos del xml
    //if (row.Subtotal == 0) {
    //    $('#div_subir_archivo_operacion').show();
    //}
    ////  si tieen informacion el subtotal se oculta el div de adjuntos xml
    //else {
    //    $('#div_subir_archivo_operacion').hide();
    //}

    //  se carga la información en los controles
    $('#txt_factura_id').val(row.Factura_Id);


    lectura_xml_modalidad_automatica();

    lectura_pagos_registrados();
    lectura_anticipos_registrados();
    calcular_diferencia_montos_factura();

    //  se consultan las cuentas
    _consultar_cuentas();
    _consultar_informacion_anticipos_operacion();
    calcular_totales_pagos_anticipos();

    //  se habilitan los controles para la modificación
    _habilitar_controles("Modificar");

    //  se muestra el formulario
    _mostrar_vista('Modal Editar');

    //Se configuran los botones de ediccion
    _configuracion_controles_conceptos('Editar');

    //$('#cmb_tipo_concepto').attr('disabled', true);
    $('#cmb_tipo_concepto').select2('trigger', 'select', {
        data: {
            id: row.Concepto_Id,
            text: row.Concepto_Texto_Id
        }
    });
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_concepto_click
--DESCRIPCIÓN:          Evento para eliminar el concepto seleccionado 
--PARÁMETROS:           tab: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           14 Febrero de 2020
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_concepto_click(tab) {
    var row = $(tab).data('orden'); // Variable con la informacion del concepto seleccionado;

    //  se crea el objeto de confirmación
    bootbox.confirm({
        title: 'Eliminar factura',
        message: 'Esta seguro de ELIMINAR la factura [' + row.Folio + ']?',
        callback: function (result) {

            //  validamos que accion tomo el usuario
            if (result) {

                //  se declara la variable
                var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                //  se asignan los elementos al objeto de filtro
                obj.Factura_Id = row.Factura_Id;
               
               
                //  se convierte la información a json
                var $data = JSON.stringify({ 'jsonObject': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                //  se ejecuta la petición
                $.ajax({
                    type: 'POST',
                    url: 'controllers/Facturas_Controller.asmx/EliminarConcepto',
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
                                _mostrar_mensaje('Eliminar Factura', result.Mensaje);

                                //  se realiza la consulta
                                _consultar_informacion();

                            } else {//  si la acción marco un error

                                //  se muestra el mensaje del error que se presento
                                _mostrar_mensaje('Eliminar Factura', result.Mensaje);
                            }
                        }
                    }
                });
            }
        }
    });
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
    _generar_solicitud_cheque_pdf(row.Folio);
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
    _generar_excel(row.Folio);
    
}



/* =============================================
  --NOMBRE_FUNCIÓN:       btn_nuevo_concepto_click
  --DESCRIPCIÓN:          Se llama al evento que genera el registro de un nuevo elemento
  --PARÁMETROS:           e: parametro que se refiere al evento click
  --CREO:                 Hugo Enrique Ramírez Aguilera
  --FECHA_CREO:           25 de Septiembre de 2019
  --MODIFICÓ:
  --FECHA_MODIFICÓ:
  --CAUSA_MODIFICACIÓN:
  =============================================*/
function btn_nuevo_concepto_click(tab) {

    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se limpian los estilos
    _clear_all_class_error();

    ////  se estable el estilo de los controles check
    //_inicializar_icheck();

    //  se limpian los controles
    _limpiar_todos_controles_modal();

    $('#btn_leer_archivo_operacion').attr('disabled', false);
    _mostrar_vista('Modal Nuevo');

    //  se habilitan los controles 
    _habilitar_controles("Nuevo");

    //  se habilitan los botones y de oculta el div del adjunto xml
    $('#cmb_folio').prop('disabled', false);
    $('#div_subir_archivo_operacion').hide();


    $('#btn_leer_archivo_automatico').hide();

    //Configurar botones de alta
    _configuracion_controles_conceptos('Nuevo');

    //  se carga el numero de folio
    $('#txt_folio_solicitud_cheque').val(row.Folio);
    $('#txt_folio_asignado').val(row.Folio);
    $('#cmb_tipo_concepto').select2('trigger', 'select', {
        data: {
            id: row.Concepto_Id,
            text: row.Concepto
        }
    });
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
            message: 'Esta seguro de AUTORIZAR el folio de solicitud de cheque [' + row.Folio +']?',
            callback: function (result) {

                //  validamos que accion tomo el usuario
                if (result) {

                    //  se declara la variable
                    var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                    //  se asignan los elementos al objeto de filtro
                    obj.Factura_Id = 0;
                    obj.Folio_Cheque = parseInt(row.Folio);
                    obj.Validacion_Id = parseInt(row.Validacion_Id);
                    obj.Orden_Validacion = parseInt(row.Orden_Validacion);
                    //obj.Folio = parseInt(row.Folio);
                    //obj.Concepto = row.Concepto_Xml;


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
            message: 'Esta seguro de RECHAZAR el folio de solicitud de cheque [' + row.Folio +']?',
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
                                obj.Folio_Cheque = parseInt(row.Folio);
                                obj.Validacion_Id = parseInt(row.Validacion_Id);
                                obj.Orden_Validacion = parseInt(row.Orden_Validacion);
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
            url: 'controllers/Facturas_Controller.asmx/Genere_Solicitud_Cheque_PDF',
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
            url: 'controllers/Facturas_Controller.asmx/Genere_Reporte_Excel_Hoja_Resumen',
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
--NOMBRE_FUNCIÓN:       _consultar_folio_cheque
--DESCRIPCIÓN:          Consulta el folio aproximado de la solicitud de cheque
--PARÁMETROS:           NA
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_folio_cheque() {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario
    try {
        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  se realiza la petición
        jQuery.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Consultar_Folio_Solicitud_Cheque',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (datos) {

                //  validamos que exista información en los valores recibidos
                if (datos !== null) {

                    //var result = JSON.parse(datos.d);// almacena la información recibida

                    $('#txt_folio_solicitud_cheque').val(datos.d);
                }
            }
        });

    } catch (e) {
        alert(e.message)
    }

}