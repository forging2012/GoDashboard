$(document).ready(function() {
    window.setTimeout('checkUpdates();', 5000);
});

function checkUpdates() {
    $.ajax({
        url: '/Home/Refresh' + location.search,
        success: function(data) {
            $('#content').html(data);
        }
    });
    window.setTimeout('checkUpdates();', 5000);
}