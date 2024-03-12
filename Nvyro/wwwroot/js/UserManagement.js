$(document).ready(function () {
    

    // START OF ORGANIZER REGISTRATION//
    const registerNextBtn = $("#register-next");    
    const registerBackBtn = $("#register-back");
    const registerSubmitBtn = $("#register-submit");
    const registerEmailInput = $("#register-email");
    const registerPwdInput = $('#register-password');
    const registerPwdCfmInput = $("#register-confirm");
    const progressBar = $("#register-progress-bar");
    const registerSteps = $(".register-step");
    const registerPostal = $("#register-postalCode");
    const registerBlock = $("#register-blockNumber");
    const registerRoad = $("#register-roadName");
    const registerUnit = $("#register-unitNumber");
    const registerPhone = $("#register-phone");
    const registerOrgName = $("#register-orgName");
    const registerSelectionBackBtn = $("#back-register-selection");

    var registerActive = 1;
    var atLeast8 = false;
    var oneUpper = false;
    var oneLower = false;
    var oneSpecial = false;
    var oneNumber = false;
    var validEmail = false;
    var pwdMatch = false;

    $(document).click(() => {
        $("#select-address").children("ul").removeClass("show");
    });
    console.log(registerEmailInput.val().trim());
    registerEmailInput.on("input", (e) => {

        var emailInput = registerEmailInput.val().trim();
        var reg = /^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$/;
        if (emailInput.match(reg)) {
            validEmail = true;
            $("#register-email-validation").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Valid email format </div>`);
        } else {
            validEmail = false;
            $("#register-email-validation").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Invalid email format </div>`);
        }
        if (atLeast8 && oneUpper && oneLower && oneSpecial && oneNumber && validEmail && pwdMatch) {
            registerNextBtn.prop("disabled", false);
        } else {
            registerNextBtn.prop("disabled", true);
        }
    });

    registerPwdInput.on("input", (e) => {

        var pwdInput = registerPwdInput.val();
        if (pwdInput.search(/^(?=.*[A-Z]).*$/) >= 0) {
            oneUpper = true;
            $("#one-upper").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password requires at least one uppercase </div>`);
        } else if (pwdInput.search(/^(?=.*[A-Z]).*$/) < 0) {
            oneUpper = false;
            $("#one-upper").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password requires at least one uppercase </div>`);
        }
        if (pwdInput.search(/^(?=.*[a-z]).*$/) >= 0) {
            oneLower = true;
            $("#one-lower").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password requires at least one lowercase </div>`);
        } else if (pwdInput.search(/^(?=.*[a-z]).*$/) < 0) {
            oneLower = false;
            $("#one-lower").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password requires at least one lowercase </div>`);
        }
        if (pwdInput.search(/^(?=.*[0-9]).*$/) >= 0) {
            oneNumber = true;
            $("#one-number").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password requires at least one number </div>`);
        } else if (pwdInput.search(/^(?=.*[0-9]).*$/) < 0) {
            oneNumber = false;
            $("#one-number").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password requires at least one number </div>`);
        }
        if (pwdInput.search(/^(?=.*[~`!@#$%^&*()--+={}\[\]|\\:;'<>,.?/_₹]).*$/) >= 0) {
            oneSpecial = true;
            $("#one-special").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password requires at least one special character </div>`);
        } else if (pwdInput.search(/^(?=.*[~`!@#$%^&*()--+={}\[\]|\\:;'<>,.?/_₹]).*$/) < 0) {
            oneSpecial = false;
            $("#one-special").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password requires at least one special character </div>`);
        }
        if (pwdInput.length >= 8) {
            atLeast8 = true;
            $("#eight-char").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password requires at least 8 characters </div>`);
        } else if (pwdInput.length < 8) {
            atLeast8 = false;
            $("#eight-char").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password requires at least 8 characters </div>`);
        }
        if (registerPwdCfmInput.val() == registerPwdInput.val() && registerPwdCfmInput.val()) {
            pwdMatch = true;
            $("#match-password").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password matches </div>`);
        } else {
            pwdMatch = false;
            $("#match-password").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password matches </div>`);
        }
        if (atLeast8 && oneUpper && oneLower && oneSpecial && oneNumber && validEmail && pwdMatch) {
            registerNextBtn.prop("disabled", false);
        } else {
            registerNextBtn.prop("disabled", true);
        }
    });

    registerPwdCfmInput.on("input", (e) => {

        if (registerPwdCfmInput.val() == registerPwdInput.val() && registerPwdCfmInput.val()) {
            pwdMatch = true;
            $("#match-password").html(`<div class="text-success"> <i class="fa-solid fa-check" style="color:green;"></i> Password matches </div>`);
        } else {
            pwdMatch = false;
            $("#match-password").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> Password matches </div>`);
        }
        if (atLeast8 && oneUpper && oneLower && oneSpecial && oneNumber && validEmail && pwdMatch) {
            registerNextBtn.prop("disabled", false);
        } else {
            registerNextBtn.prop("disabled", true);
        }
    });
    const nextBtnFunction = () => {
        $.ajax({
            url: `/api/user/register/1/false`,
            headers: {
                "Accept": "application/json",
                "Content-Type": 'application/json',
            },
            type: 'POST',
            dataType: "json",
            data: JSON.stringify
                ({
                    Email: registerEmailInput.val(),
                    Password: registerPwdInput.val()
                }),
            success: function (data) {
                console.log(!data.success)
                if (!data.success) {
                    registerActive--;
                    if (registerActive < 1) {
                        registerActive = 1;
                    }
                    $("#register-email-validation").html(`<div class="text-danger"> <i class="fa-solid fa-xmark" style="color:red;"></i> This Email ${registerEmailInput.val()} already exist </div>`);
                    updateProgress();
                } else {
                    registerActive++;
                    $("#registerPage1").addClass("d-none");
                    $("#registerPage2").removeClass("d-none");
                    if (registerActive > registerSteps.length) {
                        registerActive = registerSteps.length;
                    }
                    updateProgress();
                }
            },
            error: function (data) {

            }
        });
    }

    registerNextBtn.click(() => {
        nextBtnFunction();
    });

    registerBackBtn.click(() => {
        registerActive--;
        if (registerActive <= 1) {
            registerActive = 1;
        }
        $("#registerPage2").addClass("d-none");
        $("#registerPage1").removeClass("d-none");
        updateProgress();
    });

    const updateProgress = () => {
        console.log(registerActive);
        registerSteps.toArray().forEach((step, index) => {
            if (index < registerActive) {
                step.classList.add("active");
            } else {
                step.classList.remove("active");
            }
        });
        progressBar.css("width", ((registerActive - 1) / (registerSteps.toArray().length - 1)) * 100 + "%");
        if (registerActive == 1) {
            $("#register-btn-back").addClass("invisible");
            $("#back-register-selection").removeClass("d-none");
            if (atLeast8 && oneUpper && oneLower && oneSpecial && oneNumber && validEmail && pwdMatch) {
                registerNextBtn.removeClass("d-none");
                registerNextBtn.prop("disabled", false);
                registerSubmitBtn.addClass("d-none");
            } else {
                //$("#register-btn-next").html(`
                //    <button type="button" id="register-next" class="w-100 btn btn-lg btn-primary" disabled>Next <i class="fa-solid fa-arrow-right"></i></button>
                //`);
                registerNextBtn.removeClass("d-none");
                registerNextBtn.prop("disabled", true);
                registerSubmitBtn.addClass("d-none");
            }
        } else if (registerActive == registerSteps.length) {
            $("#back-register-selection").addClass("d-none");
            $("#register-btn-back").removeClass("invisible");
            if (registerUnit.val().trim() && registerRoad.val().trim() && registerBlock.val().trim() && registerPostal.val().trim()) {
                //$("#register-btn-next").html(`
                //    <button type="submit" id="register-submit" class="w-100 btn btn-lg btn-primary">Submit</button>
                //`);
                registerSubmitBtn.removeClass("d-none");
                registerSubmitBtn.prop("disabled", false);
                registerNextBtn.addClass("d-none");
            } else {
                registerNextBtn.addClass("d-none");
                registerSubmitBtn.prop("disabled", true);
                registerSubmitBtn.removeClass("d-none");
            }
        } else {
            $("#back-register-selection").addClass("d-none");
            $("#register-btn-back").removeClass("invisible");
        }
    }
    registerPhone.on("input", () => {
        const registerPostalVal = $("#register-postalCode").children("input").val().trim();
        const registerUnitVal = $("#register-unitNumber").children("input").val().trim();
        const registerRoadVal = $("#register-roadName").children("input").val().trim();
        const registerBlockVal = $("#register-blockNumber").children("input").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            registerSubmitBtn.prop("disabled", false);
        } else {
            registerSubmitBtn.prop("disabled", true);
        }
    });
    registerOrgName.on("input", () => {
        const registerPostalVal = $("#register-postalCode").children("input").val().trim();
        const registerUnitVal = $("#register-unitNumber").children("input").val().trim();
        const registerRoadVal = $("#register-roadName").children("input").val().trim();
        const registerBlockVal = $("#register-blockNumber").children("input").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            registerSubmitBtn.prop("disabled", false);
        } else {
            registerSubmitBtn.prop("disabled", true);
        }
    });
    registerBlock.on("input", () => {
        const registerPostalVal = $("#register-postalCode").children("input").val().trim();
        const registerUnitVal = $("#register-unitNumber").children("input").val().trim();
        const registerRoadVal = $("#register-roadName").children("input").val().trim();
        const registerBlockVal = $("#register-blockNumber").children("input").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            registerSubmitBtn.prop("disabled", false);
        } else {
            registerSubmitBtn.prop("disabled", true);
        }
    });
    registerRoad.on("input", () => {
        const registerPostalVal = $("#register-postalCode").children("input").val().trim();
        const registerUnitVal = $("#register-unitNumber").children("input").val().trim();
        const registerRoadVal = $("#register-roadName").children("input").val().trim();
        const registerBlockVal = $("#register-blockNumber").children("input").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {

            registerSubmitBtn.prop("disabled", false);
        } else {
            registerSubmitBtn.prop("disabled", true);
        }
    });
    registerPostal.on("input", () => {
        const registerPostalVal = $("#register-postalCode").children("input").val().trim();
        const registerUnitVal = $("#register-unitNumber").children("input").val().trim();
        const registerRoadVal = $("#register-roadName").children("input").val().trim();
        const registerBlockVal = $("#register-blockNumber").children("input").val().trim();
        console.log(registerPostalVal);
        console.log($("#register-postalCode").children("input").val().trim());
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            registerSubmitBtn.prop("disabled", false);
        } else {
            registerSubmitBtn.prop("disabled", true);
        }
    });
    registerUnit.on("input", () => {
        const registerPostalVal = $("#register-postalCode").children("input").val().trim();
        const registerUnitVal = $("#register-unitNumber").children("input").val().trim();
        const registerRoadVal = $("#register-roadName").children("input").val().trim();
        const registerBlockVal = $("#register-blockNumber").children("input").val().trim();
        if (registerUnitVal && registerRoadVal && registerBlockVal && registerPostalVal && registerPhone.val() && !isNaN(registerPhone.val()) && registerOrgName.val() && registerPhone.val().length == 8) {
            registerSubmitBtn.prop("disabled", false);
        } else {
            registerSubmitBtn.prop("disabled", true);
        }
    });

    var searchTrigger = null;
    var dataResults = [];
    $("#search-full-address").on("input", (e) => {
        clearTimeout(searchTrigger);
        console.log($("#search-full-address").val().length)
        if ($("#search-full-address").val().length > 0) {
            searchTrigger = setTimeout(() => {
                $.ajax({
                    url: `https://www.onemap.gov.sg/api/common/elastic/search?searchVal=${$('#search-full-address').val()}&returnGeom=N&getAddrDetails=Y`,
                    success: function (result) {
                        dataResults = [];
                        //Set result to a variable for writing
                        console.log(result);
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
                                $("#search-full-address").val(selected.ADDRESS);
                                registerPostal.removeClass("invisible");
                                registerPostal.children("input").val(selected.POSTAL);
                                registerBlock.removeClass("invisible");
                                registerBlock.children("input").val(selected.BLK_NO);
                                registerRoad.removeClass("invisible");
                                registerRoad.children("input").val(selected.ROAD_NAME);
                                registerUnit.removeClass("invisible");
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

    const registerRecyler = $("#register-recycler");
    const registerOrg = $("#register-organization");
    registerRecyler.click(() => {
        registerSelectionBackBtn.removeClass("d-none");
        $("#register-selection-btn").addClass("d-none");
        $("#registerForm").removeClass("d-none");
        $("#change-banner-register").css("background-image", "url(/assets/images/recycler-user-banner.jpg)")
        $("#register-name").text("Full Name");
        $("#register-role").val("Recycler");
    });
    registerOrg.click(() => {
        registerSelectionBackBtn.removeClass("d-none");
        $("#register-selection-btn").addClass("d-none");
        $("#registerForm").removeClass("d-none");
        $("#change-banner-register").css("background-image", "url(/assets/images/org-banner.jpg)");
        $("#register-name").text("Organization Name");
        $("#register-role").val("Organizer");
    })
    registerSelectionBackBtn.click(() => {
        $("#registerForm").addClass("d-none");
        $("#register-selection-btn").removeClass("d-none");
    });
    // END OF ORGANIZER REGISTRATION//
});
