function img2Base64(element,displayedElement,hiddenField) {
    var file = element.files[0];
    var reader = new FileReader();
    reader.onloadend = function () {
        $("#" + displayedElement).attr("src", reader.result);
        $("#" + hiddenField).val(reader.result);
        //$("#"+displayedElement).text(reader.result);
    }
    reader.readAsDataURL(file);
}

