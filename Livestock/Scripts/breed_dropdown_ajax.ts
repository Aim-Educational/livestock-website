﻿// Breeds need to be filtered by what the chosen CritterType is, this is what this file provides functionality for.

declare var $: { ajax: (arg0: { type: string; url: string; contentType: string; dataType: string; data: string; }) => { done: (arg0: (response: { value: string; description: string; }[]) => void) => void; }; };
type CritterInfo = { value: string, description: string };

function setDropdownValues(values: CritterInfo[], select: HTMLSelectElement) {
    values.forEach(obj => {
        let opt = document.createElement("option");
        opt.value = obj.value;
        opt.innerHTML = obj.description;
        select.options.add(opt);
    });
}

function handleBreedDropdown(critterTypeSelect: HTMLSelectElement, breedSelect: HTMLSelectElement) {
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
            setDropdownValues(cache[selectedType], breedSelect);
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
            setDropdownValues(response, breedSelect);

            // Add the response to the cache so we don't need to use it again.
            cache[selectedType] = response;
        });
    });

    // Set off a change event just to populate it initially.
    critterTypeSelect.dispatchEvent(new Event("change"));
}