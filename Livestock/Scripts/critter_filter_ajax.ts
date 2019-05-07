function updateDesignLayout(
    breedDropdown: HTMLSelectElement,
    typeDropdown: HTMLSelectElement,
    cache: { [divPlusBreed: string]: string },
    div: HTMLElement,
    designType: string
) {
    let typeName   = typeDropdown.selectedOptions[0].innerHTML;
    let typeValue  = parseInt(typeDropdown.selectedOptions[0].value);
    let breedName  = breedDropdown.selectedOptions[0].innerHTML;
    let breedValue = parseInt(breedDropdown.selectedOptions[0].value);

    let key = designType + breedName + "-" + typeName;
    if (key in cache) {
        div.innerHTML = cache[key];
        return;
    }

    $.ajax(
        {
            type: "POST",
            url: "/CritterEx/GetCrittersFiltered",
            contentType: "application/json",
            dataType: "html",
            data: JSON.stringify({
                BreedId: breedValue,
                CritterTypeId: typeValue,
                Design: designType
            })
        }
    ).done(function (response: string) {
        div.innerHTML = response;
        cache[key] = response;
    });
}

function handleCritterFilter(typeDropdown: HTMLSelectElement, breedDropdown: HTMLSelectElement) {
    let divCardHoriz = <HTMLDivElement>document.getElementById("design-card-horiz");
    let divCardVert = <HTMLDivElement>document.getElementById("design-card-vert");
    let divTable = <HTMLTableElement>document.getElementById("design-table");

    let cache: { [divPlusBreed: string]: string } = {};

    breedDropdown.addEventListener("change", function () {
        updateDesignLayout(breedDropdown, typeDropdown, cache, divCardHoriz, "card-horiz");
        updateDesignLayout(breedDropdown, typeDropdown, cache, divCardVert, "card-vert");
        updateDesignLayout(breedDropdown, typeDropdown, cache, divTable, "table");
    });
}