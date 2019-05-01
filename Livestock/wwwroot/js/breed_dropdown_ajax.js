function setDropdownValues(values, select, defaultBreedId) {
    values.forEach(function (obj) {
        var opt = document.createElement("option");
        opt.value = obj.value;
        opt.innerHTML = obj.description;
        select.options.add(opt);
        if (defaultBreedId === parseInt(opt.value))
            select.selectedIndex = select.options.length - 1;
    });
}
function handleBreedDropdown(critterTypeSelect, breedSelect, defaultBreedId) {
    if (critterTypeSelect == null) {
        alert("Dev error: critterTypeSelect is null");
        return;
    }
    if (breedSelect == null) {
        alert("Dev error: breedSelect is null");
        return;
    }
    var cache = {};
    critterTypeSelect.addEventListener('change', function () {
        while (breedSelect.options.length > 0) {
            breedSelect.options.remove(0);
        }
        var selectedType = critterTypeSelect.selectedOptions[0].value;
        if (selectedType in cache) {
            setDropdownValues(cache[selectedType], breedSelect, defaultBreedId);
            return;
        }
        $.ajax({
            type: "POST",
            url: "/CritterEx/GetBreedList",
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify({ CritterTypeId: parseInt(selectedType) })
        }).done(function (response) {
            setDropdownValues(response, breedSelect, defaultBreedId);
            cache[selectedType] = response;
        });
    });
    critterTypeSelect.dispatchEvent(new Event("change"));
}
//# sourceMappingURL=breed_dropdown_ajax.js.map