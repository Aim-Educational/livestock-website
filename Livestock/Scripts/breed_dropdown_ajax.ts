// Breeds need to be filtered by what the chosen CritterType is, this is what this file provides functionality for.

declare var $: any;
type CritterInfo = { value: string, description: string };

function setDropdownValues(values: CritterInfo[], select: HTMLSelectElement, defaultBreedId: number) {
    values.forEach(obj => {
        let opt = document.createElement("option");
        opt.value = obj.value;
        opt.innerHTML = obj.description;
        select.options.add(opt);

        if (defaultBreedId === parseInt(opt.value))
            select.selectedIndex = select.options.length - 1;
    });

    select.dispatchEvent(new Event("change"));
}

function handleBreedDropdown(critterTypeSelect: HTMLSelectElement, breedSelect: HTMLSelectElement, defaultBreedId: number) {
    // Error checking.
    if (critterTypeSelect == null) {
        alert("Dev error: critterTypeSelect is null");
        return;
    }

    if (breedSelect == null) {
        alert("Dev error: breedSelect is null");
        return;
    }

    let cache: { [critterType: string]: CritterInfo[]} = {};

    // Set the onchange event for the dropdowns.
    critterTypeSelect.addEventListener('change', function ()
    {
        // Clear previous options
        while (breedSelect.options.length > 0) {
            breedSelect.options.remove(0);
        }

        let selectedType: string = critterTypeSelect.selectedOptions[0].value;

        // Use the response from the cache if we have one.
        if (selectedType in cache) {
            setDropdownValues(cache[selectedType], breedSelect, defaultBreedId);
            return;
        }

        // Add new ones with an AJAX request.
        $.ajax(
            {
                type: "POST",
                url: "/CritterEx/GetBreedList",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({ CritterTypeId: parseInt(selectedType) })
            }
        ).done(function (response: CritterInfo[]) {
            // Add options based on the results.
            setDropdownValues(response, breedSelect, defaultBreedId);

            // Add the response to the cache so we don't need to use it again.
            cache[selectedType] = response;
        });
    });

    // Set off a change event just to populate it initially.
    critterTypeSelect.dispatchEvent(new Event("change"));
}