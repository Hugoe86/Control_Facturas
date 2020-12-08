var _PROCESO_CORRECTO_ = 'OK';
var _PROCESO_INCORRECTO_ = 'ERROR';
var _INFORME_TECNICO_ = 'Informe técnico';
var _PARAMETROS_INCORRECTOS_ = 'Párametros incorrectos';

//Valores predeterminados para el grid
var _WITH_TABLE_ = 900;
var _HEIGHT_TABLE_ = 600;
var _HEIGHT_TABLE_ANEXO = 300;
var _PAGINATION_SIZE_ = 50;
var _PAGELIST_ = [50, 100, 150, 200];

$(document).on('ready', function () {

});

function mg_mostrar_mensaje(Titulo, Mensaje) {
    bootbox.dialog({
        message: Mensaje,
        title: Titulo,
        locale: 'en',
        closeButton: true,
        buttons: [{
            label: 'Close',
            className: 'btn-default',
            callback: function () { }
        }]
    });
}
function mg_cargar_tabla(_Grid, _Campos_ID, _Datos) {
    var control_Grid = '#' + _Grid;
    var columnas = [], datas = [];

    if (_Grid
        && _Campos_ID) {
        try {
            //Destruimos el grid
            $(control_Grid).bootstrapTable('destroy');
            
            //Obtenemos las columnas de los datos recibidos para construir las columnas que se mostraran en el grid.
            if (_Datos != null) {
                //Convertimos el resultado obtenido en formato json
                var obj = $.parseJSON(_Datos);

                //recorremos cada una de los nombres de campo para obtener las columnas
                if (obj != null)
                    $.each(obj[0], function (key, value) {
                        //var titulo_Columna = key.replace("_", " ");
                        var titulo_Columna = key.split("_").join(" ");
                        columnas.push({
                            field: key,
                            title: titulo_Columna,
                            visible: (key.indexOf('_ID') === -1 ? true : false), // Por default se ocultaran las columnas que correspondan a ID's
                            sortable: true,
                            clickToSelect: false
                        });
                    });
            }

            //Crearmos el grid con las columnas
            $(control_Grid).bootstrapTable({
                cache: false,
                width: _WITH_TABLE_,
                height: _HEIGHT_TABLE_,
                striped: true,
                pagination: true,
                pageSize: _PAGINATION_SIZE_,
                pageList: _PAGELIST_,
                smartDysplay: false,
                search: false,
                searchOnEnterKey: false, //El método será ejecutado hasta que la tecla Enter sea presionada.
                showColumns: false,
                showRefresh: false,
                minimunCountColumns: 2,
                clickToSelect: true,
                columns: columnas
                    ? columnas
                    : [{ field: _Campos_ID, title: '', width: 0, align: 'center', valign: 'bottom', sortable: true, visible: true }]
            });

            if (_Datos !== null) {
                $(control_Grid).bootstrapTable('load', JSON.parse(_Datos));
            }
        } catch (e) {
            xmg_mostrar_mensaje(_INFORME_TECNICO_, e);
        }
    }
    else {
        mg_mostrar_mensaje(_INFORME_TECNICO_, _PARAMETROS_INCORRECTOS_);
    }


}