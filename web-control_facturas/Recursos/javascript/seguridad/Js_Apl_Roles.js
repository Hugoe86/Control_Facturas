//****FUNCION PARA HABILITAR TODOS LOS SUBMENUS DEL MENU SELECCIONADO****//
function Chk_Habilitar_CheckedChanged(Chk_Habilitado) {
    var Menu_Id = '';
    var Div_Controles = '';
    var Controles = '';

    try {
        Menu_Id = $(Chk_Habilitado).parent().attr('Title');
        Div_Controles = document.getElementById($.trim("div" + Menu_Id));
        Controles = Div_Controles.getElementsByTagName("input");

        if ($(Chk_Habilitado).attr('checked')) {
            for (i = 0; i < Controles.length; i++) {
                if (Controles[i].getAttribute("type") == "checkbox") {
                    Controles[i].checked = true;
                }
            }
        }
        else {
            for (i = 0; i < Controles.length; i++) {
                if (Controles[i].getAttribute("type") == "checkbox") {
                    Controles[i].checked = false;
                }
            }
        }
    } catch (e) {
        alert("Error al habilitar el Menú. Error[" + e + "]");
    }
}

//****FUNCION PARA HABILITAR TODOS LAS OPCIONES DEL SUBMENU SELECCIONADO****//
function Chk_Habilitar_Interno_CheckedChanged(Chk_Habilitado) {
    var Menu_Id = '';
    var Div_Controles = '';
    var Controles = '';
    var Parent_Id = '';
    var Contador = 0;

    try {
        Menu_Id = $(Chk_Habilitado).parent().attr('Title'); //obtenemos el menu_id del tooltip
        Parent_Id = $(Chk_Habilitado).parent().attr('class'); //obtnemos el parent_id de la clase
        Div_Controles = document.getElementById($.trim("div" + Parent_Id));//obtenemos todos los controles del div del menu
        Controles = Div_Controles.getElementsByTagName("input"); //obtenemos los checkbox del div

        if ($(Chk_Habilitado).attr('checked')) {
            for (i = 0; i < Controles.length; i++) {
                if (Contador < 5) {
                    if ($(Controles[i]).parent().attr('Title') == $.trim(Menu_Id)) {
                        Controles[i].checked = true; //habilitamos el checkbox
                        Contador++;
                    }
                } else {
                    break;
                }
            }
        }
        else {
            for (i = 0; i < Controles.length; i++) {
                if (Contador < 5) {
                    if ($(Controles[i]).parent().attr('Title') == $.trim(Menu_Id)) {
                        Controles[i].checked = false; //deshabilitamos el checkbox
                        Contador++;
                    }
                } else {
                    break;
                }
            }
        }

        //validamos la seleccion del checkbox del menu principal
        Validar_CheckBox_Principal(Parent_Id);
    } catch (e) {
        alert("Error al habilitar el Sub Menú. Error[" + e + "]");
    }
}

//****FUNCION PARA HABILITAR TODOS LAS OPCIONES DEL SUBMENU SELECCIONADO****//
function Validar_CheckBox_Principal(Menu_Id) {
    var Seleccionado = false;
    var Div_Controles = '';
    var Controles = '';

    try {
        Div_Controles = document.getElementById($.trim("div" + Menu_Id));
        Controles = Div_Controles.getElementsByTagName("input");

        for (i = 0; i < Controles.length; i++) {
            if (Controles[i].getAttribute("type") == "checkbox") {
                if ($(Controles[i]).prop('checked')) {
                    Seleccionado = true;
                    break;
                }
            }
        }

        Div_Controles = document.getElementById("Div_Grid_Menus");
        Controles = Div_Controles.getElementsByTagName("input");

        for (i = 0; i < Controles.length; i++) {
            if (Controles[i].getAttribute("type") == "checkbox") {
                if ($(Controles[i]).parent().attr('Title') == $.trim(Menu_Id)) {
                    if (Seleccionado) {
                        Controles[i].checked = true;
                    } else {
                        Controles[i].checked = false;
                    }
                    break;
                }
            }
        }
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
}

function change_access_page(checkbox) {
    var $_parent_id = $.trim($(checkbox).parent().prop('class'));
    var $_menu_id = $.trim($(checkbox).parent().prop('title'));
    var ids = '';
    var $_estatus = false;

    $('span[title=' + $_menu_id + ']:not(:first)').each(function (index, value) {
        $_estatus = $(this).find('input[type=checkbox]:first').prop('checked');

        if ($_estatus)
            return false;
    });

    $('span[title=' + $_menu_id + ']').each(function (index, value) {
        var nodo_raiz = $(this).data('rootleaf');

        if (nodo_raiz != null) {
            $(this).find('input[type=checkbox]:first').prop("checked", $_estatus);
            Validar_CheckBox_Principal($.trim($(this).prop("class")));
            return false;
        }
    });
}
