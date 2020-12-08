
/* =============================================
--NOMBRE_FUNCIÓN:       _launch_modal_adjuntos
--DESCRIPCIÓN:          Se muestra el modal
--PARÁMETROS:           title_window: estructura que tendrá el titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _launch_modal_adjuntos(title_window) {

    //  se le carga el mensaje que tendrá el titulo del modal
    _set_title_modal_adjuntos(title_window);

    //  se muestra el modal
    jQuery('#modal_facturas_adjuntar').modal('show', { backdrop: 'static', keyboard: false });
}

/* =============================================
--NOMBRE_FUNCIÓN:       _set_title_modal_adjuntos
--DESCRIPCIÓN:          Carga la estructura que tendrá el texto del titulo del modal
--PARÁMETROS:           titulo: el mensaje que se mostrara como titulo del modal
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_title_modal_adjuntos(titulo) {

    //  se le asigna el texto al titulo del modal
    $("#lbl_titulo_adjuntar").html(titulo);
}

/* =============================================
--NOMBRE_FUNCIÓN:       _cancelar_modal_adjuntos_click
--DESCRIPCIÓN:          Oculta el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _cancelar_modal_adjuntos_click() {
    //  se llama al evento que cierra el modal
    _set_close_modal_adjuntos();
}


/* =============================================
--NOMBRE_FUNCIÓN:       _set_close_modal_adjuntos
--DESCRIPCIÓN:          Ejecuta la sección para ocultar el modal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _set_close_modal_adjuntos() {
    //  cierra el modal
    jQuery('#modal_facturas_adjuntar').modal('hide');
}




/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_vista_adjuntar
--DESCRIPCIÓN:          Establece los metodos principales del modal de adjuntos
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_vista_adjuntar() {
    try {
        //  se inicializan los controles
        _inicializar_controls_file();
        _eventos_adjuntar();
        _crear_tabla_adjuntos();

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe técnico' + '[_consultar_datos_usuario]', e);
    }
}



/* =============================================
--NOMBRE_FUNCIÓN:       _crear_tabla_adjuntos
--DESCRIPCIÓN:          Se crea la estructura de la tabla
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _crear_tabla_adjuntos() {

    try {

        //  destruye la estructura de la tabla
        $('#tbl_facturas_adjuntos').bootstrapTable('destroy');

        //  crea la estructura de la tabla
        $('#tbl_facturas_adjuntos').bootstrapTable({
            cache: false,
            striped: true,
            pagination: true,
            data: [],
            pageSize: 5,
            pageList: [5, 10, 25, 50],
            smartDysplay: false,
            search: false,
            showColumns: false,
            showRefresh: false,
            minimumCountColumns: 2,

            columns: [

                { field: 'Anexo_Id', title: 'Anexo_Id', width: 50, align: 'center', valign: 'top', sortable: true, visible: false },
                { field: 'Factura_Id', title: 'Factura_Id', width: 50, align: 'center', valign: 'top', sortable: true, visible: false },

                { field: 'Url', title: 'Url', align: 'center', valign: 'top', visible: false, sortable: true },
                { field: 'Nombre', title: 'Nombre', align: 'left', valign: 'top', visible: true, sortable: true },
                { field: 'Tipo', title: 'Tipo', align: 'left', valign: 'top', visible: false, sortable: true },

                  {
                      //  Descargar
                      field: 'Descargar',
                      title: 'Descargar',
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
                          opciones += '<div style="display:block"><a class="remove ml10 text-blue" id="' + row.Anexo_Id +
                                          '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                          '\' onclick="btn_descargar_archivo_click(this);" title="Descargar">' +
                                          '<i class="fa fa-cloud-download"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                          opciones += '</div>';

                          return opciones;
                      }
                  },


                  {
                      //  Adjuntos
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
                      --FECHA_CREO:           24 Octubre de 2019
                      --MODIFICÓ:
                      --FECHA_MODIFICÓ:
                      --CAUSA_MODIFICACIÓN:
                      =============================================*/
                      formatter: function (value, row) {

                          var opciones;//   variable para formar la estructura del boton

                          opciones = '<div style=" text-align: center;">';
                          opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Anexo_Id +
                                          '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                          '\' onclick="btn_eliminar_archivo_click(this);" title="Eliminar">' +
                                          '<i class="fa fa-times"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                          opciones += '</div>';

                          return opciones;
                      }
                  },


                     {
                         //  Leer
                         field: 'Factura_Id',
                         title: 'Leer',
                         width: 80,
                         align: 'right',
                         valign: 'top',
                         visible:false,
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

                             // validamos que sea el tipo de XML
                             if (row.Tipo == "text/xml") {

                                 opciones = '<div style=" text-align: center;">';
                                 opciones += '<div style="display:block"><a class="remove ml10 text-red" id="' + row.Anexo_Id +
                                                 '" href="javascript:void(0)" data-orden=\'' + JSON.stringify(row) +
                                                 '\' onclick="btn_leer_archivo_xml_click(this);" title="Leer_Xml">' +
                                                 '<i class="fa fa-code"></i>&nbsp;<span style="font-size:11px !important;"></span></a></div>';

                                 opciones += '</div>';
                             }


                             return opciones;
                         }
                     },
            ]
        });
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + '[_crear_tabla_adjuntos]', e.message);
    }
}


/* =============================================
--NOMBRE_FUNCIÓN:       _inicializar_controls_file
--DESCRIPCIÓN:          Establece las propiedades del control de fileinput
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _inicializar_controls_file() {

    $("#fl_adjuntar_archivo").fileinput({

        overwriteInitial: false,
        showClose: true,
        showPreview: false,
        browseLabel: '',
        uploadLabel: '',
        removeLabel: '',
        maxFileSize: 10000,
        browseTitle: 'Seleccionar imagen',
        browseIcon: '<i class="glyphicon glyphicon-folder-open"></i>',
        browseClass: 'btn btn-success',
        showUpload: false,
        removeIcon: '<i class="glyphicon glyphicon-remove"></i>',
        removeTitle: 'Cancelar',
        removeClass: 'btn btn-danger',
        uploadClass: 'btn btn-info',
        //msgErrorClass: 'alert alert-block alert-danger',
        //allowedFileExtensions: ["jpg", "png", "gif", "xml", "pdf"],
        msgInvalidFileExtension: 'Extensión inválida para el archivo "{name}". Solo los archivos "{extensions}" son compatibles.',
        elErrorContainer: '#bugs',
    });
}




/* =============================================
--NOMBRE_FUNCIÓN:       _eventos_adjuntar
--DESCRIPCIÓN:          Crea los eventos del modal que seran utilizados
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _eventos_adjuntar() {

    $('#btn_leer_archivo').on('click', function (e) {
        e.preventDefault();

        //  guarda el archivo en el servidor
        var guardar = _guardar_archivo_importacion();


    });

}

/* =============================================
--NOMBRE_FUNCIÓN:       _guardar_archivo_importacion
--DESCRIPCIÓN:          Guarda el archivo dentro del servidor
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _guardar_archivo_importacion() {

    var archivos = $("#fl_adjuntar_archivo").get(0).files;//  variable con la que se obtiene el documento
    var data = new FormData();//    variabla para saber los bits del documento
    var ruta = '';//    variable para saber la ruta en donde se guardara el archivo
    var salida = new Object();//  variable para almacenar el resultado de la operacion
    var archivo_final;//    variable para el archivo final

    salida.Estatus = false;

    try {

        //  validamos que exista el documento
        if (archivos.length > 0) {

            id = $('#txt_pasos_adjunto_id').val();

            var nombre = archivos[0].name;//    variable para obtener el nombre del documento
            var ruta_importacion = "Reportes/Importaciones";//  variable para obtener la ruta en donde se guardara el documento

            archivo_final = id + "." + archivos.type;

            data.append("file", archivos[0]);
            data.append("nombre", nombre);
            data.append("url_", ruta_importacion);
            data.append("tipo", archivos[0].type);

            ruta = '../../../' + ruta_importacion + '/' + archivos[0].name;

            var guardar = _guardar_archivo_directorio(data);//  variable para guardar el valor resultante de la operacion

            //  validamos que la operacion sea exitos
            if (guardar.Estatus === "success") {
                actualizar_adjunto(archivos[0], ruta, archivos[0].type);
            }
        }
    } catch (e) {
        salida.Estatus = false;

        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + '[_guardar_archivo_importacion]', e.message);
    }

    return salida;
}


/* =============================================
--NOMBRE_FUNCIÓN:       _guardar_archivo_directorio
--DESCRIPCIÓN:          Guarda el archivo dentro de una carpeta temporal
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
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

                //  validamos que tenga informacion
                if (result) {
                    estatus = true;
                }
            }
        });
    } catch (e) {
        estatus = false;
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + '[_guardar_archivo_directorio]', e.message);
    }
    return resultado;
}


/* =============================================
--NOMBRE_FUNCIÓN:       actualizar_adjunto
--DESCRIPCIÓN:          Guarda el archivo en la base de datos
--PARÁMETROS:           nombre: nombre del archivo que se guardara
--                      url: ruta en donde se almacenara el archivo
--                      tipo: tipo del archivo que se estara subiendo
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function actualizar_adjunto(nombre, url, tipo) {
    var obj = new Object();//   estructura en donde se cargaran la información del formulario


    try {

        //  se carga la información 
        obj.Factura_Id = parseInt($('#txt_factura_adjunto_id').val());
        obj.Nombre = nombre.name;
        obj.Tipo = tipo;
        obj.Url = url;

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

        //  ejecutamos la peticion
        $.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Actualizar_Adjunto',
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

                        //  se muestra en mansaje de dato correcto
                        _mostrar_mensaje(result.Titulo, result.Mensaje);
                       
                        //  se consultan los documentos
                        _consultar_anexos();

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
                        //  se borra el documento de la carpeta de temporales
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
                        //  se muestra el error que se presento
                    else {
                        _mostrar_mensaje(result.Titulo, result.Mensaje);
                    }
                }
            }
        });

    } catch (e) {

        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + '[actualizar_adjunto]', e.message);
    }

}

/* =============================================
--NOMBRE_FUNCIÓN:       _consultar_anexos
--DESCRIPCIÓN:          Se consultan losanexos que tiene la factura
--PARÁMETROS:           NA
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           02 de Septiembre del 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function _consultar_anexos() {
    var filtros = null;//   se le asignaran a las propiedades los filtros de búsqueda
    try {
        filtros = new Object();//   se inicializa

        //  se pasan los valores
        filtros.Factura_Id = parseInt($('#txt_factura_adjunto_id').val());

        //  se convierte a la estructura que pueda leer el controlador
        var $data = JSON.stringify({ 'json_object': JSON.stringify(filtros) });//   variable para guardar la informacion que se le pasara al controlador

        //  se ejecuta la petición
        $.ajax({
            type: 'POST',
            url: 'controllers/Facturas_Controller.asmx/Consultar_Facturas_Anexos',
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
                    $('#tbl_facturas_adjuntos').bootstrapTable('load', datos);
                }
            }
        });
    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Informe Técnico' + ' [_consultar_anexos] ', e.message);
    }
}




/* =============================================
--NOMBRE_FUNCIÓN:       btn_descargar_archivo_click
--DESCRIPCIÓN:          Descarga el archivo
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_descargar_archivo_click(renglon) {
    //  se obtiene la información del renglón de la tabla
    var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

    window.open("../../" +row.Url, "", "", true);
   
}


/* =============================================
--NOMBRE_FUNCIÓN:       btn_eliminar_archivo_click
--DESCRIPCIÓN:          Se realiza la acción de eliminar
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_eliminar_archivo_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        
        //  validamos que pueda realizar la accion
        if ($('#chk_validador').prop('checked') == false) {

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
                        obj.Anexo_Id = parseInt(row.Anexo_Id);
                        obj.Factura_Id = parseInt(row.Factura_Id);
                        obj.Nombre = row.Nombre;
                        obj.Url = row.Url;

                        //  se convierte la información a json
                        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                        //  se ejecuta la petición
                        $.ajax({
                            type: 'POST',
                            url: 'controllers/Facturas_Controller.asmx/Eliminar_Adjunto',
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
                                        _consultar_anexos();

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
                                        //  se borra el documento de la carpeta de temporales
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
            //  indicamos que no tiene permisos para realizar la accion
        else {
            _mostrar_mensaje('Validación', 'No tiene permisos para realizar esta acción');
        }

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_archivo_click] ', e);
    }
}

/* =============================================
--NOMBRE_FUNCIÓN:       btn_leer_archivo_xml_click
--DESCRIPCIÓN:          Se realiza la lectura del xml
--PARÁMETROS:           renglon: Estructura de uno de los renglones de la tabla
--CREO:                 Hugo Enrique Ramírez Aguilera
--FECHA_CREO:           24 Octubre de 2019
--MODIFICÓ:
--FECHA_MODIFICÓ:
--CAUSA_MODIFICACIÓN:
=============================================*/
function btn_leer_archivo_xml_click(renglon) {
    try {
        //  se obtiene la información del renglón de la tabla
        var row = $(renglon).data('orden');//   variable para guardar la informacion del renglon de la tabla

        //  validamos que pueda realizar la accion
        if ($('#chk_validador').prop('checked') == false) {

            //  se crea el objeto de confirmación
            bootbox.confirm({
                title: 'LECTURA DE XML',
                message: 'Esta seguro de Leer el XML seleccionado?',
                callback: function (result) {

                    //  validamos que accion tomo el usuario
                    if (result) {

                        //  se declara la variable
                        var obj = new Object();//   variable que sera la que contenga los valores que se le pasaran al controlador

                        //  se asignan los elementos al objeto de filtro
                        obj.Anexo_Id = parseInt(row.Anexo_Id);
                        obj.Factura_Id = parseInt(row.Factura_Id);
                        obj.Url = row.Url;

                        //  se convierte la información a json
                        var $data = JSON.stringify({ 'json_object': JSON.stringify(obj) });//   variable para guardar la informacion que se le pasara al controlador

                        //  se ejecuta la petición
                        $.ajax({
                            type: 'POST',
                            url: 'controllers/Facturas_Controller.asmx/Leer_Xml',
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
            //  indicamos que no tiene permisos para realizar la accion
        else {
            _mostrar_mensaje('Validación', 'No tiene permisos para realizar esta acción');
        }

    } catch (e) {
        //  se muestra el mensaje del error que se presento
        _mostrar_mensaje('Error Técnico' + ' [btn_eliminar_archivo_click] ', e);
    }
}