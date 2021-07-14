const uri = "https://localhost:44354/api/FilmyItems";
let Filmy = null;
function getCount(data) {
    const el = $("#counter");
    let name = "film";
    if (data) {
        if (data > 1) {
            name = "filmy";
        }
        el.text(data + " " + name);
    } else {
        el.text("Brak - najpierw musisz dodać filmy");
    }
}
$(document).ready(function () {
    getData();
});
function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#Filmy");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.filmyname))
                    .append($("<td></td>").text(item.author))
                    .append($("<td></td>").text(item.rating))
                    .append(
                        $("<td></td>").append(
                            $("<button>Edytuj</button>").on("click", function () {
                                editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<button>Usuń</button>").on("click", function () {
                                deleteItem(item.id);
                            })
                        )
                    );
                tr.appendTo(tBody);
            });
            Book = data;
        }
    });
}
function addItem() {
    const item = {
        filmyname: $("#add-filmyname").val(),
        author: $("#add-author").val(),
        rating: $("#add-rating").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreateFilmyItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Coś poszło nie tak! Pola nie mogą być pustę a ocena musi być od 1-10");
        },
        success: function (result) {
            getData();
            $("#add-filmyname").val(""),
                $("#add-author").val(""),
                $("#add-rating").val("");
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: uri + "/" + id,
        type: "DELETE",
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(Filmy, function (_key, item) {
        if (item.id === id) {
            $("#edit-filmyname").val(item.filmyname);
            $("#edit-author").val(item.author);
            $("#edit-rating").val(item.rating);
            $("#edit-id").val(item.id);

        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    const item = {
        id: id,
        filmyname: $("#edit-filmyname").val(),
        author: $("#edit-author").val(),
        rating: $("#edit-rating").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdateFilmyItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Coś poszło nie tak! Pola nie mogą być pustę a ocena musi być od 1-10");
        },
        success: function (result) {
            getData();
            closeInput();
        }
    });
}

function closeInput() {
    $("#spoiler").css({ display: "none" });
}