var editorsHtml = document.querySelectorAll('*[id^="editor-"]');
if (editorsHtml === null)
    alert("Dev error: No editor divs found.");
var editors = {};
editorsHtml.forEach(function (elem) {
    editors[elem.id.replace("editor-", "")] = elem;
});
var editorSelector = document.querySelector('*[id="editorSelect"]');
if (editorSelector === null)
    alert("Dev error: No <select> element called 'editorSelect'");
function setupEditors(eventTypes) {
    for (var key in eventTypes) {
        var value = eventTypes[key];
        if (!(value in editors))
            alert("Dev error: No editor for DataType '" + value + "'");
        var option = document.createElement("option");
        option.text = key;
        editorSelector.add(option);
    }
    editorSelector.addEventListener("change", function () {
        editorsHtml.forEach(function (e) {
            if (!e.classList.contains("d-none"))
                e.classList.add("d-none");
        });
        var thisEditor = editors[eventTypes[editorSelector.value]];
        thisEditor.classList.remove("d-none");
        var dataTypeInput = thisEditor.querySelector(".data-type-input");
        if (dataTypeInput === null) {
            alert("Editor for '" + editorSelector.value + "' does not have a hidden input of class 'data-type-input'");
            return;
        }
        dataTypeInput.value = editorSelector.value;
    });
    editorSelector.dispatchEvent(new Event("change"));
}
//# sourceMappingURL=life_event.js.map