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

function moveBetweenSelectBoxes(
    sourceBox: HTMLSelectElement,
    destinationBox: HTMLSelectElement
) {
    // Sorted insert.
    // destinationBox must always be sorted for this to work.
    for (let i = 0; i < sourceBox.selectedOptions.length; i++) {
        let inserted = false;
        for (let sortingI = 0; sortingI < destinationBox.length; sortingI++) {
            if (destinationBox.options.item(sortingI).text < sourceBox.selectedOptions.item(i).text)
                continue;

            destinationBox.add(sourceBox.selectedOptions.item(i), sortingI);
            inserted = true;
            break;
        }

        if (!inserted)
            destinationBox.add(sourceBox.selectedOptions.item(i));
    }
}

function registerMultiSelect(
    inputFilter: HTMLInputElement,
    addBox: HTMLSelectElement,
    selectedBox: HTMLSelectElement,
    addToSelectedButton: HTMLButtonElement,
    selectedToAddButton: HTMLButtonElement,
    selectType: "critter" | "user"
) {
    inputFilter.addEventListener('change', function () {
        performMultiSelectAjax(inputFilter, addBox, selectedBox, selectType);
    });

    inputFilter.dispatchEvent(new Event('change'));

    // In case this is put into a form, pressing 'Enter' would make the form submit.
    // So we make 'onkeydown' stop doing anything for the enter key, then 'onkeyup' fire off the change event.
    // A nice side effect of this is, you don't have to press enter anymore to see changes, it'll happen every keypress.
    inputFilter.onkeyup = function () { inputFilter.dispatchEvent(new Event('change')); }
    inputFilter.onkeydown = function (event) { return event.keyCode != 13; }

    addToSelectedButton.addEventListener('click', () => moveBetweenSelectBoxes(addBox, selectedBox));
    selectedToAddButton.addEventListener('click', () => moveBetweenSelectBoxes(selectedBox, addBox));
}