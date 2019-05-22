let multiSelectCache: { [cacheKey: string]: ValueAndDescription<number>[] } = {}

function updateMultiSelect(
    box: HTMLSelectElement,
    data: ValueAndDescription<number>[],
    existingValues: number[] | null = null
) {
    while (box.options.length > 0) {
        box.options.remove(0);
    }

    data.filter(d => existingValues == null || existingValues.indexOf(d.value) == -1)
        .forEach(d => {
        let option = document.createElement("option");
        option.value = d.value.toString();
        option.innerHTML = d.description;
        box.options.add(option);
    });
}

function performMultiSelectAjax(
    inputFilter: HTMLInputElement,
    addBox: HTMLSelectElement,
    selectedBox: HTMLSelectElement,
    selectType: "critter" | "user"
) {
    let cacheKey = inputFilter.id + "-" + addBox.id + "-" + selectType + "-" + inputFilter.value;
    let existingValues: number[] = [];
    for (let i = 0; i < selectedBox.options.length; i++) {
        existingValues.push(Number(selectedBox.options.item(i).value));
    }

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
        ).done(function (response: ValueAndDescription<number>[]) {
            multiSelectCache[cacheKey] = response;
            updateMultiSelect(addBox, response, existingValues);
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
        performMultiSelectAjax(inputFilter, addBox, selectedBox, selectType);
    });

    inputFilter.dispatchEvent(new Event('change'));

    // In case this is put into a form, pressing 'Enter' would make the form submit.
    // So we make 'onkeydown' stop doing anything, then 'onkeyup' fire off the change event.
    inputFilter.onkeyup = function () { inputFilter.dispatchEvent(new Event('change')); }
    inputFilter.onkeydown = function (event) { return event.keyCode != 13; }
}