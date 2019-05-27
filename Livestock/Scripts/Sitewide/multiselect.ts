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

    let doAjax = (url: string, data: any) => {
        $.ajax(
            {
                type: "POST",
                url: url,
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(data)
            }
        ).done(function (response: ValueAndDescription<number>[]) {
            multiSelectCache[cacheKey] = response;
            updateMultiSelect(addBox, response, existingValues);
        });
    }

    if (cacheKey in multiSelectCache) {
        updateMultiSelect(addBox, multiSelectCache[cacheKey]);
    }
    else {
        switch (selectType) {
            case "critter":
                doAjax("/CritterEx/GetCrittersFilteredValueDescription", { Name: inputFilter.value });
                break;

            case "user":
                doAjax("/Account/GetUsersFilteredValueDescription", { NamesRegex: inputFilter.value });
                break;

            default:
                break;
        }
    }
}

function moveBetweenSelectBoxes(
    sourceBox: HTMLSelectElement,
    destinationBox: HTMLSelectElement
) {
    // Clear any selections made in the destination.
    // This is because we only want the new items we move over to be selected (easy undo for the user).
    clearSelection(destinationBox);

    // Sorted insert.
    // destinationBox must always be sorted for this to work.
    while (sourceBox.selectedOptions.length > 0) {
        let inserted = false;
        for (let sortingI = 0; sortingI < destinationBox.length; sortingI++) {
            if (destinationBox.options.item(sortingI).text < sourceBox.selectedOptions.item(0).text)
                continue;

            // SELF NOTE: Adding an option that already exists in another select box removes it from the original.
            destinationBox.add(sourceBox.selectedOptions.item(0), sortingI);
            inserted = true;
            break;
        }

        if (!inserted)
            destinationBox.add(sourceBox.selectedOptions.item(0));
    }
}

function clearSelection(box: HTMLSelectElement) {
    while (box.selectedOptions.length > 0) {
        box.selectedOptions[0].selected = false;
    }
}

function registerMultiSelect(
    inputFilter: HTMLInputElement,
    addBox: HTMLSelectElement,
    selectedBox: HTMLSelectElement,
    addToSelectedButton: HTMLButtonElement,
    selectedToAddButton: HTMLButtonElement,
    form: HTMLFormElement | null, // CAN ONLY BE NULL FOR CONTROLS NOT INSIDE A FORM. MUST BE NON-NULL FOR THOSE IN A FORM.
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

    // Clear the opposite box's selection when we click on it.
    addBox.addEventListener('change', () => clearSelection(selectedBox));
    selectedBox.addEventListener('change', () => clearSelection(addBox));

    // When the form is submitted, select all of the selected items, to ensure they're sent over.
    if (form !== null) {
        form.addEventListener('submit', () => {
            for (let i = 0; i < selectedBox.options.length; i++) {
                selectedBox.options.item(i).selected = true;
            }
        });
    }
}