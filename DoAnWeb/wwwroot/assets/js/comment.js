var form = document.getElementById("commentFrm");
var btn = document.getElementById("btnCommentSubmit");
btn.onclick = function () {
    if (document.getElementById('name').value == null || document.getElementById('name').value.trim() === '') {
        alert("Tên không được rỗng");
        return;
    }
    if (document.getElementById('phone').value == null || document.getElementById('phone').value.trim() === '') {
        alert("Số điện thoại không được rỗng");
        return;
    }
    if (document.getElementById('email').value == null || document.getElementById('email').value.trim() === '') {
        alert("Email không được rỗng");
        return;
    }
    if (document.getElementById('detail').value == null || document.getElementById('detail').value.trim() === '') {
        alert("Nội dung không được rỗng");
        return;
    }
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(document.getElementById('email').value)) {
        alert("Email không hợp lệ");
        return;
    }
    var PhonePattern = /^(0)(3[2-9]|5[689]|7[06-9]|8[1-9]|9\d)\d{7}$/;
    if (!PhonePattern.test(document.getElementById('phone').value)) {
        alert("Số điện thoại không hợp lệ");
        return;
    }
    form.submit();
};