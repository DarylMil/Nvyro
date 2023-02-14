$(document).ready(function () {
    const form1 = $("#form1");
    const form2 = $("#form2");
    const registerPostal = $("#register-postalCode");
    const registerBlock = $("#register-blockNumber");
    const registerRoad = $("#register-roadName");
    const registerUnit = $("#register-unitNumber");
    const saveChangesBtn = $("#save-changes-btn");
    const form = document.getElementById("Form");
    const heading1 = document.getElementById("not-own-acc");
    const heading2 = document.getElementById("own-acc");

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
        registerBlock.val("")
        registerUnit.val("")
        registerPostal.val("")
        registerRoad.val("")
        heading1.style.display = "none";
        heading2.style.display = "block";
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
        console.log($("#register-postalCode").val(), $("#register-unitNumber").val());
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
                    PostalCode: $("#register-postalCode").val(),
                    UnitNumber: $("#register-unitNumber").val()
                   
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
