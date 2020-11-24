/*Это js  + jQuery and ajax*/
$("#dataSend").on('click', function () {
    var formData = new FormData();
    formData.append('LicensePlate', document.getElementById('LicensePlate').value);
    formData.append('Brand', document.getElementById('Brand').value);
    formData.append('Color', document.getElementById('Color').value);
    formData.append('InstructorID', document.getElementById('InstructorID').value);
    $.ajax({
        contentType: false,
        processData: false,
        type: 'POST',
        url: '/Car/Create',
        data: formData,
        success: function () {
            window.location = "/Car/GetCars";
        },
        error: function () {
            console.log("error.");
        },
    });
});