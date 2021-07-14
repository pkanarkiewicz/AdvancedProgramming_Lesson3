const uri = "https://localhost:44354/api/OsobaItems";
let osoba = null;
function getCount(data) {
    const el = $("#counter");
    let name = "osoba";
    if (data) {
        if (data > 1) {
            name = "osoby";
        }
        el.text(data + " " + name);
    } else {
        el.text("Brak - najpierw musisz dodać osoby");
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
            const tBody = $("#osoba");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.name))
                    .append($("<td></td>").text(item.secname))
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
            Osoba = data;
        }
    });
}
function addItem() {
    const item = {
        name: $("#add-name").val(),
        secname: $("#add-secname").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreateOsobaItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Coś poszło nie tak! Pola nie mogą być puste");
        },
        success: function (result) {
            getData();
            $("#add-name").val(""),
            $("#add-secname").val("");
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
    $.each(Osoba, function (_key, item) {
        if (item.id === id) {
            $("#edit-name").val(item.name);
            $("#edit-secname").val(item.secname);
            $("#edit-id").val(item.id);
            
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    const item = {
        id: id,
        name: $("#edit-name").val(),
        secname: $("#edit-secname").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdateOsobaItem',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Coś poszło nie tak! Pola nie mogą być puste");
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