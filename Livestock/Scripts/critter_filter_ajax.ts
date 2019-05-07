// TODO: Reduce the code duplication between this file and breed_dropdown_ajax.ts
type BreedInfo = { value: number, description: string };

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

function setBreedValues(breedDropdown: HTMLSelectElement, breeds: BreedInfo[]) {
    breeds.push({ value: -999, description: "All" });
    breeds.forEach(b => {
        let option = document.createElement("option");
        option.innerHTML = b.description;
        option.value = b.value.toString();
        breedDropdown.options.add(option);
    });

    let temp = breedDropdown.options[0];
    breedDropdown.options[0] = breedDropdown.options[breedDropdown.length - 1];
    breedDropdown.options[breedDropdown.length - 1] = temp;
    breedDropdown.selectedIndex = 0;
    breedDropdown.dispatchEvent(new Event("change"));
}

function handleCritterFilter(typeDropdown: HTMLSelectElement, breedDropdown: HTMLSelectElement) {
    let divCardHoriz = <HTMLDivElement>document.getElementById("design-card-horiz");
    let divCardVert = <HTMLDivElement>document.getElementById("design-card-vert");
    let divTable = <HTMLTableElement>document.getElementById("design-table");

    let breedCache: { [divPlusBreed: string]: string } = {};
    let typeCache: { [typeId: number]: BreedInfo[] } = {};

    breedDropdown.addEventListener("change", function () {
        updateDesignLayout(breedDropdown, typeDropdown, breedCache, divCardHoriz, "card-horiz");
        updateDesignLayout(breedDropdown, typeDropdown, breedCache, divCardVert, "card-vert");
        updateDesignLayout(breedDropdown, typeDropdown, breedCache, divTable, "table");
    });

    typeDropdown.addEventListener("change", function () {
        let typeId = parseInt(typeDropdown.selectedOptions[0].value);
        breedDropdown.innerHTML = "";

        if (typeDropdown.selectedOptions[0].innerHTML == "All") {
            let option = document.createElement("option");
            option.value = "-999";
            breedDropdown.options.add(option);
            breedDropdown.dispatchEvent(new Event("change"));
            return;
        }

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
        ).done(function (response: BreedInfo[]) {
            setBreedValues(breedDropdown, response);
            typeCache[typeId] = response;
        });
    });
}