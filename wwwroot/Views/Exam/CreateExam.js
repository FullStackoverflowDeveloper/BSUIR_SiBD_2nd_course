$("#dataSend").on('click', function () {
    var formData = new FormData();
    formData.append('DateExam', document.getElementById('DateExam').value);
    formData.append('StudentID', document.getElementById('StudentID').value);
    formData.append('CarID', document.getElementById('CarID').value);
    formData.append('InstructorID', document.getElementById('InstructorID').value);
    formData.append('TraffPoliceDep', document.getElementById('TraffPoliceDep').value);
    $.ajax({
        contentType: false,
        processData: false,
        type: 'POST',
        url: '/Exam/Create',
        data: formData,
        success: function () {
            window.location = "/Exam/GetExams";
        },
        error: function () {
            console.log("error.");
        },
    });
});