// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function AddGroup() {
    var url = "/Administration/AddGroup";
    var name = $("#Name").val();
    var room = $("#Room").val();
    $.post(url, { name: name, room: room }, function (res) {
        $("#stats .wrapper").html(res);
        $("#stats-tab").tab("show");
    });
};
