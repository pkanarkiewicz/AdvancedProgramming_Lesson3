const uri = "https://localhost:44354/api/BookItems";
let Book = null;
function getCount(data) {
    const el = $("#counter");
    let name = "książka";
    if (data) {
        if (data > 1) {
            name = "książki";
        }
        el.text(data + " " + name);
    } else {
        el.text("Brak - najpierw musisz dodać książki");
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
            const tBody = $("#Book");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.bookname))
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
        bookname: $("#add-bookname").val(),
        author: $("#add-author").val(),
        rating: $("#add-rating").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreateBookItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Coś poszło nie tak! Pola nie mogą być pustę a ocena musi być od 1-10");
        },
        success: function (result) {
            getData();
            $("#add-bookname").val(""),
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
    $.each(Book, function (_key, item) {
        if (item.id === id) {
            $("#edit-bookname").val(item.bookname);
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
        bookname: $("#edit-bookname").val(),
        author: $("#edit-author").val(),
        rating: $("#edit-rating").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdateBookItem',
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