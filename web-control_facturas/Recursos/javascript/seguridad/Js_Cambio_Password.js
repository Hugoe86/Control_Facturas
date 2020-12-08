$(document).on('ready', function () {
    $("#divModalCambioContrasena").load('../Paginas_Generales/Cambio_Password.html', function () {
        _limp_cntls_cmb_pass();
        _evnt_cambio_pass();
    });
});

function _limp_cntls_cmb_pass() {
    //$('input[type=text]').each(function () { $(this).val(''); });
    $('#txt_password_id').val('');
    $('#txt_nuevo_password').val('');
    $('#txt_confirmar_password').val('');
    $('#txt_actual_password').val('');
    //_validation_sumary(null);
    _clear_all_class_error_cmbio_pass();
}

function _evnt_cambio_pass() {
    try {
        $('#btn_guardar_cambio_pass').click(function (e) {
            e.preventDefault();
            var pass1 = $('#txt_nuevo_password').val();
            var pass2 = $('#txt_confirmar_password').val();
            var pass_actual = $('#txt_actual_password').val();
            var validacion = _validacion_cambios_password();
            if (validacion.Estatus != false) {
                if (_validar_password(pass_actual, pass1, pass2)) {
                    if (!_guardar_dtos_cmbios_pass())
                        _mostrar_mensaje("Validación", "Error al cambiar la contraseña");
                    else
                        _mostrar_mensaje("Validación", "Cambio de contraseña exitoso.");

                }
            } else {
                _mostrar_mensaje("Validacion", validacion.Mensaje);
            }
        });

        $('input[type=password]').each(function (index, element) {
            $(this).on('focus', function () {
                _remove_class_error_cmbio_pass('#' + $(this).attr('id'));
            });
        });

    } catch (e) {
        _mostrar_mensaje('Informe Técnico', e);
    }
}

function _guardar_dtos_cmbios_pass() {

    var password = null;
    var isComplete = false;

    try {

        password = new Object();

        password.Password = $('#txt_nuevo_password').val();
        password.Password_Actual = $('#txt_actual_password').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(password) });

        $.ajax({
            type: 'POST',
            url: '../Paginas_Generales/controllers/Autentificacion_Controller.asmx/Modificar_Password_Usuario',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        isComplete = true;
                        _limp_cntls_cmb_pass();
                        $('#mdal_cambio_contrasena').modal('hide');
                    } else if (Resultado.Estatus == 'error') {
                        //_validation_sumary(Resultado);
                    }
                } else {
                    //_validation_sumary(Resultado);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;

}

function _validar_pass_anterior() {
    var password = null;
    var isComplete = false;

    try {

        password = new Object();

        password.Password = $('#txt_nuevo_password').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(password) });

        $.ajax({
            type: 'POST',
            url: '../Paginas_Generales/controllers/Autentificacion_Controller.asmx/Validar_Password_Actual',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        isComplete = true;
                    } else if (Resultado.Estatus == 'error') {
                        //_validation_sumary(Resultado);
                    }
                } else {
                    //_validation_sumary(Resultado);
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;
}

function _validar_pass_actual() {
    var password = null;
    var isComplete = false;

    try {

        password = new Object();

        password.Password = $('#txt_actual_password').val();

        var $data = JSON.stringify({ 'jsonObject': JSON.stringify(password) });

        $.ajax({
            type: 'POST',
            url: '../Paginas_Generales/controllers/Autentificacion_Controller.asmx/Validar_Password_Actual',
            data: $data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            success: function (result) {
                var Resultado = JSON.parse(result.d);
                if (Resultado != null && Resultado != undefined && Resultado != '') {
                    if (Resultado.Estatus == 'success') {
                        isComplete = true;
                    } else if (Resultado.Estatus == 'error') {
                        isComplete = false;
                    }
                } else {
                    isComplete = false;
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe Tecnico', e);
    }
    return isComplete;
}

function _validacion_cambios_password() {
    var _output = new Object();

    _output.Estatus = true;
    _output.Mensaje = '';

    if (!$('#txt_actual_password').parsley().isValid()) {
        _add_class_error_cmbio_pass('#txt_actual_password');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La contraseña actual es un dato requerido.<br />';
    }


    if (!$('#txt_nuevo_password').parsley().isValid()) {
        _add_class_error_cmbio_pass('#txt_nuevo_password');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La nueva contraseña es un dato requerido.<br />';
    }

    if (!$('#txt_confirmar_password').parsley().isValid()) {
        _add_class_error_cmbio_pass('#txt_confirmar_password');
        _output.Estatus = false;
        _output.Mensaje += '&nbsp;<i class="fa fa-angle-double-right"></i>&nbsp;La confirmación de contraseña es un dato requerido.<br />';
    }

    return _output;
}

function _validar_password(pass_actual, pass1, pass2) {
    var espacios = false;
    var cont = 0;

    while (!espacios && (cont < pass1.length)) {
        if (pass1.charAt(cont) == " ")
            espacios = true;
        cont++;
    }

    if (espacios) {
        _mostrar_mensaje("Validación", "La contraseña no puede contener espacios en blanco");
        return false;
    }

    if (pass1.length == 0 || pass2.length == 0 || pass_actual.length == 0) {
        _mostrar_mensaje("Validación", "Los campos de la contraseña no pueden quedar vacíos");
        return false;
    }
    //if (pass1.length < 8 || pass2.length < 8) {
    //    _mostrar_mensaje("Validación", "Minimo 8 caracteres");
    //    return false;
    //}

    //re = /[0-9]/;
    //if (!re.test(pass1)) {
    //    _mostrar_mensaje("Validación", "Contraseña debe tener al menos un numero (0-9)");
    //    pass1.focus();
    //    return false;
    //}

    //re = /[a-z]/;
    //if (!re.test(pass1)) {
    //    _mostrar_mensaje("Validación", "Contraseña debe tener al menos una letra minuscula (a-z)!");
    //    pass1.focus();
    //    return false;
    //}

    //re = /[A-Z]/;
    //if (!re.test(pass1)) {
    //    _mostrar_mensaje("Validación", "Error: Contraseña debe tener al menos una letra mayuscula (A-Z)!");
    //    pass1.focus();
    //    return false;
    //}

    if (!_validar_pass_actual()) {
        _mostrar_mensaje("Validación", "La contraseña actual no coincide");
        return false;
    }

    if (pass1 != pass2) {
        _mostrar_mensaje("Validación", "La contraseña nueva y la contraseña de confirmación no coindiciden");
        return false;
    } else if (pass_actual == pass1) {
        _mostrar_mensaje("Validación", "La nueva contraseña no puede ser igual a la contraseña actual");
        return false;
    }
    //else if (!_validar_pass_anterior()) {
    //    _mostrar_mensaje("Validación", "La nueva contraseña ya ha sido utilizada anteriormente");
    //    return false;
    //}
    else {
        return true;
    }
}

function _add_class_error_cmbio_pass(selector) {
    $(selector).addClass('alert-danger');
}

function _remove_class_error_cmbio_pass(selector) {
    $(selector).removeClass('alert-danger');
}

function _clear_all_class_error_cmbio_pass() {
    $('input[type=text]').each(function (index, element) {
        _remove_class_error_cmbio_pass('#' + $(this).attr('id'));
    });
}

