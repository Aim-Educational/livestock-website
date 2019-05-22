var multiSelectCache = {};
function updateMultiSelect(box, data) {
    while (box.options.length > 0) {
        box.options.remove(0);
    }
    data.forEach(function (d) {
        var option = document.createElement("option");
        option.value = d.value.toString();
        option.innerHTML = d.description;
        box.options.add(option);
    });
}
function performMultiSelectAjax(inputFilter, addBox, selectType) {
    var cacheKey = inputFilter.id + "-" + addBox.id + "-" + selectType + "-" + inputFilter.value;
    if (cacheKey in multiSelectCache) {
        updateMultiSelect(addBox, multiSelectCache[cacheKey]);
    }
    else {
        $.ajax({
            type: "POST",
            url: "/CritterEx/GetCrittersFilteredValueDescription",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify({
                Name: inputFilter.value
            })
        }).done(function (response) {
            multiSelectCache[cacheKey] = response;
            updateMultiSelect(addBox, response);
        });
    }
}
function registerMultiSelect(inputFilter, addBox, selectedBox, selectType) {
    inputFilter.addEventListener('change', function () {
        performMultiSelectAjax(inputFilter, addBox, selectType);
    });
    inputFilter.dispatchEvent(new Event('change'));
}
//# sourceMappingURL=multiselect.js.map