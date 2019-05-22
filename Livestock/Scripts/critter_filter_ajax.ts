// TODO: Reduce the code duplication between this file and breed_dropdown_ajax.ts
function getRadioGroupValue(group: HTMLInputElement[]) {
    let value = "BUG";
    group.forEach(b => {
        if (b.checked) {
            value = b.value;
            return;
        }
    });
    if (value === "ALL") value = null;

    if (value === "BUG")
        alert("Dev error: value still has the 'BUG' value.");

    return value;
}

// Updates the given design using the given filters.
//
// Params:
//  breedDropdown        = The dropdown for which breed to use.
//  typeDropdown         = The dropdown for which critter type to use.
//  genderRadioButtons   = The radio buttons for the gender filter. The currently checked one will be determined and used.
//  canReproduceButtons  = The radio buttons for determining if the critter can reproduce or not.
//  cache                = A cache of responses, so we don't overload the server if we already have a response cached.
//  div                  = The div to place the updated design inside of.
//  designType           = The design to use. Valid values are 'card-horiz', 'card-vert', and 'table'.
function updateDesignLayout(
    nameTagTextbox:         HTMLInputElement,
    nameTagRadioButtons:    HTMLInputElement[],
    breedDropdown:          HTMLSelectElement,
    typeDropdown:           HTMLSelectElement,
    genderRadioButtons:     HTMLInputElement[],
    canReproduceButtons:    HTMLInputElement[],
    cache:                  { [divPlusBreed: string]: string },
    div:                    HTMLElement,
    designType:             string
) {
    // Get all of the values we want.
    let typeName            = typeDropdown.selectedOptions[0].innerHTML;
    let typeValue           = parseInt(typeDropdown.selectedOptions[0].value);
    let breedName           = breedDropdown.selectedOptions[0].innerHTML;
    let breedValue          = parseInt(breedDropdown.selectedOptions[0].value);
    let genderValue         = getRadioGroupValue(genderRadioButtons);
    let reproduceValue: any = getRadioGroupValue(canReproduceButtons);
    let nameTagValue        = nameTagTextbox.value;
    let isNameFilter: any   = getRadioGroupValue(nameTagRadioButtons) == "Name"; // True = Name, False = Tag

    if (reproduceValue !== null)
        reproduceValue = (reproduceValue === "true");

    // Use the cached response if we have one already.
    let key = designType + "-" + breedName + "-" + typeName + "-" + genderValue + "-" + reproduceValue + "-" + nameTagValue + "-" + isNameFilter;
    if (key in cache) {
        div.innerHTML = cache[key];
        doLazyLoading();
        return;
    }

    // GetCrittersFiltered will render a partial view, and return it's HTML.
    // So get the response, and set the innerHTML of the given 'div' to the response, and cache that response.
    $.ajax(
        {
            type: "POST",
            url: "/CritterEx/GetCrittersFilteredAndRender",
            contentType: "application/json",
            dataType: "html",
            data: JSON.stringify({
                BreedId: breedValue,
                CritterTypeId: typeValue,
                Design: designType,
                Gender: genderValue,
                CanReproduce: reproduceValue,
                Name: (isNameFilter) ? nameTagValue : null,
                Tag: (!isNameFilter) ? nameTagValue : null
            })
        }
    ).done(function (response: string) {
        if (response === null || response === "\n") // No clue why it sometimes only contains \n
            response = '<div class="alert alert-info">No results found.</div>';

        div.innerHTML = response;
        cache[key] = response;
    });
}

// Sets the values of the breed dropdown. An option named 'All' will always be at the start.
function setBreedValues(breedDropdown: HTMLSelectElement, breeds: ValueAndDescription<number>[]) {
    breeds.unshift({ value: -999, description: "All" });
    breeds.forEach(b => {
        let option = document.createElement("option");
        option.innerHTML = b.description;
        option.value = b.value.toString();
        breedDropdown.options.add(option);
    });

    breedDropdown.dispatchEvent(new Event("change"));
}

function handleCritterFilter(
    nameTagTextbox: HTMLInputElement,
    nameTagRadioButtons: HTMLInputElement[],
    typeDropdown: HTMLSelectElement,
    breedDropdown: HTMLSelectElement,
    genderRadioButtons: HTMLInputElement[],
    canReproduceButtons: HTMLInputElement[]
) {
    let divCardHoriz = <HTMLDivElement>document.getElementById("design-card-horiz");
    let divCardVert = <HTMLDivElement>document.getElementById("design-card-vert");
    let divTable = <HTMLTableElement>document.getElementById("design-table");

    let breedCache: { [divPlusBreed: string]: string } = {};
    let typeCache: { [typeId: number]: ValueAndDescription<number>[] } = {};

    breedDropdown.addEventListener("change", function () {
        // Lord forgive me.
        updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, breedCache, divCardHoriz, "card-horiz");
        updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, breedCache, divCardVert, "card-vert");
        updateDesignLayout(nameTagTextbox, nameTagRadioButtons, breedDropdown, typeDropdown, genderRadioButtons, canReproduceButtons, breedCache, divTable, "table");
    });

    typeDropdown.addEventListener("change", function () {
        let typeId = parseInt(typeDropdown.selectedOptions[0].value);
        breedDropdown.innerHTML = "";

        // If the user selects 'All', add in an invisible option that will fetch every critter.
        if (typeDropdown.selectedOptions[0].innerHTML == "All") {
            let option = document.createElement("option");
            option.value = "-999";
            breedDropdown.options.add(option);
            breedDropdown.dispatchEvent(new Event("change"));
            return;
        }

        // Use a cached response if we have one.
        if (typeId.toString() in typeCache) {
            setBreedValues(breedDropdown, typeCache[typeId]);
            return;
        }

        $.ajax(
            {
                type: "POST",
                url: "/CritterEx/GetBreedList",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify({ CritterTypeId: typeId })
            }
        ).done(function (response: ValueAndDescription<number>[]) {
            setBreedValues(breedDropdown, response);
            typeCache[typeId] = response;
        });
    });

    // I hate my life 
    genderRadioButtons.forEach(b => {
        b.addEventListener("change", function () {
            breedDropdown.dispatchEvent(new Event("change")); // Breeddropdown already has the logic we need, so we just reuse that.
        })
    });

    canReproduceButtons.forEach(b => {
        b.addEventListener("change", function () {
            breedDropdown.dispatchEvent(new Event("change")); // Breeddropdown already has the logic we need, so we just reuse that.
        })
    });

    nameTagRadioButtons.forEach(b => {
        b.addEventListener("change", function () {
            breedDropdown.dispatchEvent(new Event("change")); // Breeddropdown already has the logic we need, so we just reuse that.
        })
    });

    nameTagTextbox.addEventListener("change", function () {
        breedDropdown.dispatchEvent(new Event("change")); // Breeddropdown already has the logic we need, so we just reuse that.
    })
}