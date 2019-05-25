var multiSelectCache = {};
function updateMultiSelect(box, data, existingValues) {
    if (existingValues === void 0) { existingValues = null; }
    while (box.options.length > 0) {
        box.options.remove(0);
    }
    data.filter(function (d) { return existingValues == null || existingValues.indexOf(d.value) == -1; })
        .forEach(function (d) {
        var option = document.createElement("option");
        option.value = d.value.toString();
        option.innerHTML = d.description;
        box.options.add(option);
    });
}
function performMultiSelectAjax(inputFilter, addBox, selectedBox, selectType) {
    var cacheKey = inputFilter.id + "-" + addBox.id + "-" + selectType + "-" + inputFilter.value;
    var existingValues = [];
    for (var i = 0; i < selectedBox.options.length; i++) {
        existingValues.push(Number(selectedBox.options.item(i).value));
    }
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
            updateMultiSelect(addBox, response, existingValues);
        });
    }
}
function moveBetweenSelectBoxes(sourceBox, destinationBox) {
    clearSelection(destinationBox);
    while (sourceBox.selectedOptions.length > 0) {
        var inserted = false;
        for (var sortingI = 0; sortingI < destinationBox.length; sortingI++) {
            if (destinationBox.options.item(sortingI).text < sourceBox.selectedOptions.item(0).text)
                continue;
            destinationBox.add(sourceBox.selectedOptions.item(0), sortingI);
            inserted = true;
            break;
        }
        if (!inserted)
            destinationBox.add(sourceBox.selectedOptions.item(0));
    }
}
function clearSelection(box) {
    while (box.selectedOptions.length > 0) {
        box.selectedOptions[0].selected = false;
    }
}
function registerMultiSelect(inputFilter, addBox, selectedBox, addToSelectedButton, selectedToAddButton, form, selectType) {
    inputFilter.addEventListener('change', function () {
        performMultiSelectAjax(inputFilter, addBox, selectedBox, selectType);
    });
    inputFilter.dispatchEvent(new Event('change'));
    inputFilter.onkeyup = function () { inputFilter.dispatchEvent(new Event('change')); };
    inputFilter.onkeydown = function (event) { return event.keyCode != 13; };
    addToSelectedButton.addEventListener('click', function () { return moveBetweenSelectBoxes(addBox, selectedBox); });
    selectedToAddButton.addEventListener('click', function () { return moveBetweenSelectBoxes(selectedBox, addBox); });
    addBox.addEventListener('change', function () { return clearSelection(selectedBox); });
    selectedBox.addEventListener('change', function () { return clearSelection(addBox); });
    if (form !== null) {
        form.addEventListener('submit', function () {
            for (var i = 0; i < selectedBox.options.length; i++) {
                selectedBox.options.item(i).selected = true;
            }
        });
    }
}
//# sourceMappingURL=multiselect.js.map