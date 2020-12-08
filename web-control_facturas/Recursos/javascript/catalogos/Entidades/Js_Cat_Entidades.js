
$(document).on('ready', function () {
    //  se manda llamar al método de cargar vistas
    _load_vistas();
});

/* =============================================
--NOMBRE_FUNCIÓN:       _load_vistas
--DESCRIPCIÓN:          Carga las paginas HTML dentro del documento   
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _load_vistas() {

    //  se ejecutan las paginas HTML que se estarán cargando dentro del aspx
    _launchComponent('vistas/Entidades/Principal.html', 'Principal');
    _launchComponent('vistas/Entidades/Modal.html', 'Modal');
    _launchComponent('vistas/Entidades/Relacion.html', 'Relacion');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _launchComponent
--DESCRIPCIÓN:          Carga los eventos y funciones que tendrá cada pagina HTML
--PARÁMETROS:           component: ruta del archivo HTML
--                      id: Nombre que se le dará a la pagina HTML
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
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

            case 'Relacion':
                //  se carga los eventos y funciones
                _inicializar_vista_modal_relacion();
                break;
        }
    });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_principal
--DESCRIPCIÓN:          Evento con el que se cargan los eventos y funciones de la vista principal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_principal() {
    try {
        //  se crea la tabla principal
        crear_tabla_principal();

        //  se da formato al toolbar de búsqueda de la tabla
        _set_location_toolbar('toolbar');

        //  se cargan los eventos principales de los botones
        _eventos_principal();

        //  se muestra la vista principal
        _mostrar_vista('Principal');

        //  se cargan los combos filtro
        _load_cmb_filtro_nombre('cmb_busqueda_nombre');
        _load_cmb_filtro_estatus('cmb_busqueda_estatus');

        //  se consulta la informacion del catalogo
        _consultar_catalogo();

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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_modal() {
    try {

        //  se inicializan los eventos
        _eventos_modal();

        //  se limpian los controles
        _limpiar_todos_controles_modal();

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_inicializar_vista_modal] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_modal_relacion
--DESCRIPCIÓN:          Evento con el que se cargan los eventos y funciones de la vista modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_modal_relacion() {
    try {

        //  se crea la estructura de la tabla
        crear_tabla_relacion();

        //  se inicializan los eventos
        _eventos_modal_relacion();

        //  se asignan los valores al combo
        _load_cmb_cuentas('cmb_cuenta');

        //  se limpian los controles
        _limpiar_todos_controles_modal_relacion();

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_inicializar_vista_modal] ', e);
    }
}
/* =============================================
--NOMBRE_FUNCIÓN:       _set_location_toolbar
--DESCRIPCIÓN:          Establece el estilo que tendrá el control
--PARÁMETROS:           toolbar: Nombre del control al cual se le estará dando estilo
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _mostrar_vista(vista_) {

    //  se mostraran y ocultaran los div que se encuentran dentro del archivo aspx 
    switch (vista_) {

        case "Principal":
            //  se muestra el div principal
            $('#Principal').show();
            break;
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       crear_tabla_principal
--DESCRIPCIÓN:          Genere la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function crear_tabla_principal() {

    try {
        //  se destruye la tabla
        $('#tbl_entidades').bootstrapTable('destroy');

        //  se carga la estructura que tendrá la tabla
        $('#tbl_entidades').bootstrapTable({
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
                { field: 'Entidad_Id', title: 'Entidad_Id', align: 'center', valign: 'top', visible: false },
                { field: 'Entidad', title: 'Entidad', align: 'left', valign: 'top', width: 100, visible: true, sortable: true },
                { field: 'Nombre', title: 'Nombre', align: 'left', valign: 'top', width: 150, visible: true, sortable: true },
                { field: 'Estatus', title: 'Estatus', align: 'left', valign: 'top', width: 50, visible: true, sortable: true },

                {
                    field: 'Entidad_Id',
                    title: '',
                    width: 80,
                    align: 'right',
                    valign: 'top',
                    halign: 'center',

                    formatter: function (value, row) {

                        var opciones;//   variable para formar la estructura del boton
                        opciones = '<div style=" text-align: center;">';
                        opciones += '<div style="display:block"><a class="remove ml10 text-purple" id="' + row.Entidad_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_editar_click(this);" title="Editar"><i class="glyphicon glyphicon-edit"></i>&nbsp;<span style="font-size:11px !important;">Editar</span></a></div>';
                        opciones += '</div>';

                        return opciones;
                    }
                },

                   {
                       //  editar
                       field: 'Cuenta_Id',
                       title: '',
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
                           opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Cuenta_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_relacionar_click(this);" title="Relacionar"><i class="glyphicon glyphicon-tags"></i>&nbsp;Relacionar<span style="font-size:11px !important;"></span></a></div>';
                           opciones += '</div>';

                           return opciones;
                       }
                   },


                {
                    field: 'Entidad_Id',
                    title: '',
                    width: 80,
                    align: 'right',
                    valign: 'top',
                    halign: 'center',

                    formatter: function (value, row) {
                        var opciones;//   variable para formar la estructura del boton

                        opciones = '<div style=" text-align: center;">';
                        opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Entidad_Id + '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) + '\' onclick="btn_eliminar_click(this);" title="Eliminar"><i class="glyphicon glyphicon-trash"></i>&nbsp;<span style="font-size:11px !important;">Eliminar</span></a></div>';
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
--FECHA_CREO:           07 Octubre de 2019
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
        --FECHA_CREO:           07 Octubre de 2019
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
       --FECHA_CREO:           07 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#btn_nuevo').on('click', function (e) {

            e.preventDefault();

            //  se limpian los estilos
            _clear_all_class_error();

            //  se limpian los controles
            _limpiar_todos_controles_modal();

            //  se selecciona el estatus ACTIVO
            $('#cmb_estatus').select2("trigger", "select", {
                data: { id: 'ACTIVO', text: 'ACTIVO' }
            });

            //  se habilitan los controles 
            _habilitar_controles("Nuevo");

            ////  se ejecuta el modal
            _launch_modal('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Alta de entidad');

        });

        /* =============================================
       --NOMBRE_FUNCIÓN:       btn_busqueda
       --DESCRIPCIÓN:          Se llama al evento de búsqueda
       --PARÁMETROS:           e: parametro que se refiere al evento click
       --CREO:                 Hugo Enrique Ramírez Aguilera
       --FECHA_CREO:           07 Octubre de 2019
       --MODIFICÓ:
       --FECHA_MODIFICÓ:
       --CAUSA_MODIFICACIÓN:
       =============================================*/
        $('#btn_busqueda').on('click', function (e) {
            e.preventDefault();

            //  se realiza la consulta
            _consultar_catalogo();
        });

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [_eventos_principal] ', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _load_cmb_filtro_nombre
--DESCRIPCIÓN:          Carga la información de la base de datos dentro del combo
--PARÁMETROS:           cmb: nombre del control al cual se le cargara la información
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
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
                url: 'controllers/Entidades_Controller.asmx/Consultar_Entidades_Nombre_Combo',
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
--FECHA_CREO:           07 Octubre de 2019
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
                url: 'controllers/Entidades_Controller.asmx/Consultar_Entidad_Estatus_Combo',
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
--FECHA_CREO:           07 Octubre de 2019
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

    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla

    //  se limpian los estilos
    _clear_all_class_error();

    //  se limpian los controles
    _limpiar_todos_controles_modal();

    //  se carga la información en los controles
    $('#txt_entidad_id').val(row.Entidad_Id);
    $('#txt_entidad').val(row.Entidad);
    $('#txt_descripcion').val(row.Nombre);

    //  se carga el estatus
    $('#cmb_estatus').select2("trigger", "select", {
        data: { id: row.Estatus, text: row.Estatus }
    });

    //  se habilitan los controles para la modificación
    _habilitar_controles("Modificar");

    //  se muestra el modal
    _launch_modal('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Actualizar entidad');
}

/* =============================================
--NOMBRE_FUNCIÓN:       _mostrar_mensaje
--DESCRIPCIÓN:          Muestra un mensaje con la información de la variable de mensaje
--PARÁMETROS:           titulo: Nombre que tendrá el titulo de la pantalla de mensaje
                        mensaje: String con la información del mensaje
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
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
--FECHA_CREO:           07 Octubre de 2019
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
--FECHA_CREO:           07 Octubre de 2019
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
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _clear_all_class_error() {

    /* =============================================
    --NOMBRE_FUNCIÓN:       modal_entidades
    --DESCRIPCIÓN:          remueve todos los estidos de error de los controles
    --PARÁMETROS:           NA
    --CREO:                 Hugo Enrique Ramírez Aguilera
    --FECHA_CREO:           07 Octubre de 2019
    --MODIFICÓ:
    --FECHA_MODIFICÓ:
    --CAUSA_MODIFICACIÓN:
    =============================================*/
    $('#modal_entidades input[type=text]').each(function (index, element) {
        _remove_class_error($(this).attr('id'));
    });
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_click
--DESCRIPCIÓN:          Se realiza la acción de eliminar
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_click(renglon) {
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
                    obj.Entidad_Id = parseInt(row.Entidad_Id);

                    //  se convierte la información a json
                    var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                    //  se ejecuta la petición
                    $.ajax({
                        type: 'POST',
                        url: 'controllers/Entidades_Controller.asmx/Eliminar',
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
                                    _consultar_catalogo();

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
--NOMBRE_FUNCIÓN:       btn_relacionar_click
--DESCRIPCIÓN:          Carga la información del registro de la tabla, carga la información dentro de los controles correspondientes
--PARÁMETROS:           tab: estructura del renglón de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           07 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_relacionar_click(tab) {

    //  se carga la información del renglón de la tabla
    var row = $(tab).data('orden');//   variable para guardar la informacion del renglon de la tabla


    //  se limpian los controles
    _limpiar_todos_controles_modal_relacion();

    
    //  se carga la información en los controles
    $('#txt_entidad_relacion_id').val(row.Entidad_Id);

    //  se consultan la relacion con entridades
    _consultar_relacion();

    //  se muestra el modal
    _launch_modal_relacion('<i class="fa fa-list-alt" style="font-size: 25px; color: #0e62c7;"></i>&nbsp;&nbsp;Relacionar Cuenta-Entidad [' + row.Entidad + ']');

   
}

