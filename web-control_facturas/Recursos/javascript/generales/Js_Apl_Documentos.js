///********************documentos***************///
//  subri documentos
function subir_archivo(Control, Nombre_Carpeta_, Subcarpeta) {

    var data = new FormData();

    var files = $(Control).get(0).files;
    var nombre_Archivo = files[0].name;
    var Nombre_Carpeta = Nombre_Carpeta_;

    // Add the uploaded image content to the form data collection
    if (files.length > 0) {
        data.append("file", files[0]);
        data.append("nombre", files[0].name);
        data.append("url", Nombre_Carpeta + "/" + Subcarpeta);
    }
    var isComplete = _copiar_imagen_diretorio(data, Control);

    var resultado = null;

    resultado = "error";
    if (isComplete) {
        resultado = Nombre_Carpeta + "/" + Subcarpeta + "/" + nombre_Archivo;
    }

    return resultado;
}

function _copiar_imagen_diretorio(data, Control) {
    var isComplete = false;
    try {
        $.ajax({
            type: "POST",
            url: "../../FileUploadHandler.ashx",
            contentType: false,
            processData: false,
            data: data,
            async: false,
            success: function (result) {
                if (result) {
                    isComplete = true;
                    $(Control).val('');
                }
            }
        });
    } catch (e) {
        _mostrar_mensaje('Informe técnico', e);
    }
    return isComplete;
}
