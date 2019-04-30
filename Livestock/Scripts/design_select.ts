function changeDesign(divName: string) {
    let div = <HTMLDivElement>document.getElementById(divName);

    if (div === null) {
        alert("Dev error: No div called " + divName);
        return;
    }

    // Hide all designs
    let designs = document.querySelectorAll('*[id^="design-"]');
    designs.forEach(d => d.classList.add("d-none"));

    // Unhide the one we want
    div.classList.remove("d-none");
}