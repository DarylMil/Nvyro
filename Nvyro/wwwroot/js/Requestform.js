$(document).ready(function () {
    const registerPostal = $("#register-postalCode");
    const registerBlock = $("#register-blockNumber");
    const registerRoad = $("#register-roadName");
    const registerUnit = $("#register-unitNumber");
    const heading1 = document.getElementById("not-own-acc");
    const heading2 = document.getElementById("own-acc");
    const edit = document.getElementById("edit-request");
    const saveanddelete = document.getElementById("SecButton");
    const update_img = document.getElementById("update-img");
    const update_img_toggle = document.getElementById("edit-request-img-toggle");
    const gallery_img = document.getElementById("img");
    const save_new_img = document.getElementById("Save_new_images");
    const save_new_desc = document.getElementById("Save_new_Desc");
    const img_desc = $("#register-img_description");
    const isUsingUserAddress = $("#isUsingUserAddress");


    var searchTrigger = null;
    var dataResults = [];
    $(document).click(() => {
        $("#select-address").children("ul").removeClass("show");
    });
    $("#search-full-address").on("input", (e) => {
        clearTimeout(searchTrigger);
        if ($("#search-full-address").val() > 0) {
            searchTrigger = setTimeout(() => {
                $.ajax({
                    url: `https://developers.onemap.sg/commonapi/search?searchVal=${$('#search-full-address').val()}&returnGeom=N&getAddrDetails=Y`,
                    success: function (result) {
                        dataResults = [];
                        //Set result to a variable for writing
                        console.log(result.results.length>0);
                        var allResults = result.results;
                        $("#select-address").html("");
                        if (allResults.length > 0) {
                            $("#select-address").html("<ul class='dropdown-menu show dropdown-mystyle' ></ul>");
                            allResults.forEach((data, index) => {
                                dataResults.push(data)
                                $("#select-address").children("ul").append(
                                    `
                                    <li id="${index}" class="dropdown-item">
                                        <div class="address-style">
                                            ${data.ADDRESS}
                                        </div>
                                    </li>`
                                );
                            });
                            $("#select-address").children("ul").children("li").on("click", (e) => {
                                $("#select-address").children("ul").removeClass("show");
                                var selected = dataResults[e.currentTarget.id];
                                console.log(selected)
                                $("#search-full-address").val(selected.ADDRESS);

                                registerPostal.val(selected.POSTAL);

                                registerBlock.val(selected.BLK_NO);

                                registerRoad.val(selected.ROAD_NAME);

                            });
                        } else {
                            console.log("NO items")
                            $("#select-address").html("<ul class='dropdown-menu show dropdown-mystyle' ></ul>");
                            $("#select-address").children("ul").append(
                                `
                                    <li class="dropdown-item">
                                        <div class="address-style">
                                            <em>No address found</em>
                                        </div>
                                    </li>`
                            );
                        }
                    }
                });

            }, 1000);
        } else {
            dataResults = [];
            $("#select-address").html("");
        }
    });

    $("#not-using-acc-btn").click(() => {       
        $("#search-full-address").prop("disabled", false);
        registerUnit.prop("disabled", false);
        registerBlock.prop("disabled", false);
        registerPostal.prop("disabled", false);
        registerRoad.prop("disabled", false);
        isUsingUserAddress.val("false");
        registerBlock.val("")
        registerUnit.val("")
        registerPostal.val("")
        registerRoad.val("")
        heading1.style.display = "none";
        heading2.style.display = "block";
    });

    $("#edit-request").click(() => {
        registerUnit.prop("disabled", false);
        registerBlock.prop("disabled", false);
        registerPostal.prop("disabled", false);
        registerRoad.prop("disabled", false);
        saveanddelete.style.display = "block";
        edit.style.display = "none";
    });

    $("edit-request-img-desc").click(() => {
        save_new_desc.style.display = "block";
        img_desc.prop("disabled", false);
    });


    $("#edit-request-img-toggle").click(() => {
        update_img_toggle.style.display = "none";
        update_img.style.display = "block";
        gallery_img.style.display = "none";
        save_new_img.style.display = "block";

    });  

    $("#using-acc-btn").click(() => {
        location.reload();
    })

    $("#GoBack").click(() => {
        $("#form1").show();
        $("#form2").hide();
    })

    $("#continue").click(() => {
        continuefunction();
    })

    const continuefunction = () => {
        console.log($("#register-postalCode").val(), $("#register-unitNumber").val(), $("#EventId").val(), $("#startpostal").val(), $("#endpostal").val() );
        $.ajax({
            type: "POST",
            url: '/api/request/1',
            headers: {
                "Accept": "application/json",
                "Content-Type": 'application/json',
            },
            dataType: "json",
            data: JSON.stringify
                ({
                    EventStartPostal: $("#startpostal").val(),
                    EventEndPostal: $("#endpostal").val(),
                    PostalCode: $("#register-postalCode").val(),
                    UnitNumber: $("#register-unitNumber").val(),
                    EventId: $("#EventId").val()
                   
                }),
            success: function (data) {
                if (data.success) {
                    $("#form1").hide();
                    $("#form2").show();
                }
            },
            error: function (data) {
                console.log(data);
            }
        });
    }


});
