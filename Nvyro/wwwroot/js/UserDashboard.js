$(document).ready(function () {
    console.log("OI!")
    const registerPostal = $("#register-postalCode");
    const registerBlock = $("#register-blockNumber");
    const registerRoad = $("#register-roadName");
    const registerUnit = $("#register-unitNumber");
    const registerPhone = $("#register-phone");
    const registerOrgName = $("#register-orgName");
    const saveChangesBtn = $("#save-changes-btn");

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

    $("#edit-profile-btn").click(() => {
        saveChangesBtn.removeClass("invisible");
        saveChangesBtn.prop("disabled",false);
        registerOrgName.prop("disabled", false);
        registerPhone.prop("disabled", false);
        $("#search-full-address").prop("disabled", false);
        registerUnit.prop("disabled", false);
        registerBlock.prop("disabled", false);
        registerPostal.prop("disabled", false);
        registerRoad.prop("disabled", false);
    });
    //saveChangesBtn.click(() => {
    //    saveChangesBtn.addClass("invisible");
    //    registerOrgName.prop("disabled", true);
    //    registerPhone.prop("disabled", true);
    //    $("#search-full-address").prop("disabled", true);
    //    registerUnit.prop("disabled", true);
    //    registerBlock.prop("disabled", true);
    //    registerPostal.prop("disabled", true);
    //    registerRoad.prop("disabled", true);
    //});
    registerPhone.on("input", () => {
        const registerPostalVal = $("#register-postalCode").val().trim();
        const registerUnitVal = $("#register-unitNumber").val().trim();
        const registerRoadVal = $("#register-roadName").val().trim();
        const registerBlockVal = $("#register-blockNumber").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            saveChangesBtn.prop("disabled", false);
        } else {
            saveChangesBtn.prop("disabled", true);
        }
    });
    registerOrgName.on("input", () => {
        const registerPostalVal = $("#register-postalCode").val().trim();
        const registerUnitVal = $("#register-unitNumber").val().trim();
        const registerRoadVal = $("#register-roadName").val().trim();
        const registerBlockVal = $("#register-blockNumber").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            saveChangesBtn.prop("disabled", false);
        } else {
            saveChangesBtn.prop("disabled", true);
        }
    });
    registerBlock.on("input", () => {
        const registerPostalVal = $("#register-postalCode").val().trim();
        const registerUnitVal = $("#register-unitNumber").val().trim();
        const registerRoadVal = $("#register-roadName").val().trim();
        const registerBlockVal = $("#register-blockNumber").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            saveChangesBtn.prop("disabled", false);
        } else {
            saveChangesBtn.prop("disabled", true);
        }
    });
    registerRoad.on("input", () => {
        const registerPostalVal = $("#register-postalCode").val().trim();
        const registerUnitVal = $("#register-unitNumber").val().trim();
        const registerRoadVal = $("#register-roadName").val().trim();
        const registerBlockVal = $("#register-blockNumber").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {

            saveChangesBtn.prop("disabled", false);
        } else {
            saveChangesBtn.prop("disabled", true);
        }
    });
    registerPostal.on("input", () => {
        const registerPostalVal = $("#register-postalCode").val().trim();
        const registerUnitVal = $("#register-unitNumber").val().trim();
        const registerRoadVal = $("#register-roadName").val().trim();
        const registerBlockVal = $("#register-blockNumber").val().trim();
        console.log(registerPostalVal);
        console.log($("#register-postalCode").val().trim());
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            saveChangesBtn.prop("disabled", false);
        } else {
            saveChangesBtn.prop("disabled", true);
        }
    });
    registerUnit.on("input", () => {
        const registerPostalVal = $("#register-postalCode").val().trim();
        const registerUnitVal = $("#register-unitNumber").val().trim();
        const registerRoadVal = $("#register-roadName").val().trim();
        const registerBlockVal = $("#register-blockNumber").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            saveChangesBtn.prop("disabled", false);
        } else {
            saveChangesBtn.prop("disabled", true);
        }
    });

});