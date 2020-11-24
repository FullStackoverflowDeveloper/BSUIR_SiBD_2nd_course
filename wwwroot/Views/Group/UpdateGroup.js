$("#dataSend").on('click', function () {
    var formData = new FormData();
    formData.append('ID', document.getElementById('ID').value)
    formData.append('Title', document.getElementById('Title').value);
    formData.append('InstructorID', document.getElementById('InstructorID').value);
    $.ajax({
        contentType: false,
        processData: false,
        type: 'POST',
        url: '/Group/UpdatePost',
        data: formData,
        success: function () {
            window.location = "/Group/GetGroups";
        },
        error: function () {
            console.log("error.");
        },
    });
});