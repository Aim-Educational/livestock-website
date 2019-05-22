let multiSelectCache: { [cacheKey: string] : ValueAndDescription<string>[] } = {}

function updateMultiSelect(box: HTMLSelectElement, data: ValueAndDescription<string>[]) {
    while (box.options.length > 0) {
        box.options.remove(0);
    }

    data.forEach(d => {
        let option = document.createElement("option");
        option.value = d.value;
        option.innerHTML = d.description;
        box.options.add(option);
    });
}

function performMultiSelectAjax(
    inputFilter: HTMLInputElement,
    addBox: HTMLSelectElement,
    selectType: "critter" | "user"
) {
    let cacheKey = inputFilter.id + "-" + addBox.id + "-" + selectType + "-" + inputFilter.value;

    if (cacheKey in multiSelectCache) {
        updateMultiSelect(addBox, multiSelectCache[cacheKey]);
    }
    else {
        $.ajax(
            {
                type: "POST",
                url: "/CritterEx/GetCrittersFilteredValueDescription",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({
                    Name: inputFilter.value
                })
            }
        ).done(function (response: ValueAndDescription<string>[]) {
            multiSelectCache[cacheKey] = response;
            updateMultiSelect(addBox, response);
        });
    }
}

function registerMultiSelect(
    inputFilter: HTMLInputElement,
    addBox: HTMLSelectElement,
    selectedBox: HTMLSelectElement,
    selectType: "critter" | "user"
) {
    inputFilter.addEventListener('change', function () {
        performMultiSelectAjax(inputFilter, addBox, selectType);
    });

    inputFilter.dispatchEvent(new Event('change'));
}