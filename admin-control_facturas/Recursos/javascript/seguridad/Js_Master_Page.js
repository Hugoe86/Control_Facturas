
jQuery(document).on('ready', function () {
    _init_master();
});

//--------------------------------------
//  Funcion: _init_events_master
//  Descripción: Función que inicializa la página.
//  
//  Parámetros: Na
//  Usuario Creo: Juan Alberto Hernández Negrete
//  Fecha Creo: 01 Julio 2016 11:12 p.m.
//  Usuario Modifico:
//  Fecha Modifico:  
//--------------------------------------
function _init_master() {
    try {
        _init_events_master();
        _init_clock_config();
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}

//--------------------------------------
//  Funcion: _init_events_master
//  Descripción: Función que inicializa los eventos de la página.
//  
//  Parámetros: Na
//  Usuario Creo: Juan Alberto Hernández Negrete
//  Fecha Creo: 01 Julio 2016 11:12 p.m.
//  Usuario Modifico:
//  Fecha Modifico:  
//--------------------------------------
function _init_events_master() {
    try {
        $('#btn_salir_sistema').click(function (e) {
            e.preventDefault();
            $.ajax({
                url: '../../Paginas/Paginas_Generales/controllers/Autentificacion_Controller.asmx/cerrar_sesion',
                type: 'POST',
                cache: false,
                async: false,
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (resul) {
                    var $resultado = JSON.parse(resul.d);

                    if ($resultado.Estatus === 'logout') {
                        window.location.href = "../Paginas_Generales/Frm_Apl_Login.html";
                    }
                }
            });
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}

//--------------------------------------
//  Funcion: _mostrar_mensaje
//  Descripción: Función que generara muestra los mensajes que recibe como parámetro
//  
//  Parámetros: Titulo.- Titulo de la ventana.
//              Mensaje.- Informaciónque será mostrada al usuario.
//  Usuario Creo: Juan Alberto Hernández Negrete
//  Fecha Creo: 01 Julio 2016 11:12 p.m.
//  Usuario Modifico:
//  Fecha Modifico:  
//--------------------------------------
function _mostrar_mensaje(Titulo, Mensaje) {
    var box = bootbox.dialog({
        message: Mensaje,
        title: Titulo,
        locale: 'es',
        closeButton: true,
        buttons: [{
            label: 'Cerrar',
            className: 'btn btn-success',
            callback: function () { }
        }]
    });

    box.bind('shown.bs.modal', function () {
        box.find(".btn-success:first").focus();
    });
}

function _init_clock_config() {
    var options = {};
    $('.jclock').jclock(options);

    $('.jclock').qtip({
        content: new Date().toString('dddd, dd MMMM yyyy HH:mm:ss tt'),
        position: {
            corner: {
                target: 'topLeft',
                tooltip: 'bottomRight'
            }
        },
        show: {
            when: { event: 'mouseover' },
            ready: false
        },
        hide: { event: 'mouseout' },
        style: {
            border: {
                width: 5,
                radius: 10,
                color: 'rgba(0, 0, 0, 0.6)'
            },
            padding: 10,
            textAlign: 'center',
            tip: true,
            background: '#fff',
            color: '#777',
            width: 350
        }
    });
}

