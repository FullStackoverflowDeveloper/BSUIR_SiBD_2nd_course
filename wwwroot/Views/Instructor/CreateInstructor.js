$("#dataSend").on('click', function () {
    var formData = new FormData();
    formData.append('Passport', document.getElementById('Passport').value);
    formData.append('FName', document.getElementById('FName').value);
    formData.append('MName', document.getElementById('MName').value);
    formData.append('LName', document.getElementById('LName').value);
    formData.append('Address', document.getElementById('Address').value);
    $.ajax({
        contentType: false,
        processData: false,
        type: 'POST',
        url: '/Instructor/Create',
        data: formData,
        success: function () {
            window.location = "/Instructor/GetInstructors";
        },
        error: function () {
            console.log("error.");
        },
    });
});