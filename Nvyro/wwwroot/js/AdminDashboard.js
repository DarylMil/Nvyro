$(document).ready(() => {

    var modalImg = $("#modal-img");
    var modalRole = $("#modal-role");
    var modalEmail = $("#modal-email");
    var modalFullName = $("#modal-fullname");
    var modalPhoneNumber = $("#modal-phone");
    var modalUnitNumber = $("#modal-unitNumber");
    var modalBlockNumber = $("#modal-blockNumber");
    var modalPostalCode = $("#modal-postalCode");
    var modalRoadName = $("#modal-roadName");
    var hiddenId = $("#hidden-id");
    
    var myModal = new bootstrap.Modal(document.getElementById('myModal'), { keyboard: false });
    var myModalConfirmation = new bootstrap.Modal(document.getElementById('confirmation-save'), { keyboard: false });


    $("#modal-role-dropdown li").click((e) => {
        var liValue = e.target.innerText;
        modalRole.text(liValue);
    })

    $("#modal-close-btn").click(() => {
        myModal.toggle();

    });

    $("#modal-btnsave").click(() => {
        var userId = hiddenId.val();
        console.log(userId)
        $.ajax({
            url: `/api/admin/user/${userId}`,
            headers: {
                "Accept": "application/json",
                "Content-Type": 'application/json',
            },
            type: "POST",
            dataType:"json",
            data: JSON.stringify({
                ProfilePicURL: modalImg.attr("src"),
                Role: modalRole.text(),
                Email: modalEmail.val(),
                FullName: modalFullName.val(),
                PhoneNumber: modalPhoneNumber.val(),
                UnitNumber: modalUnitNumber.val(),
                BlockNumber: modalBlockNumber.val(),
                PostalCode: modalPostalCode.val(),
                RoadName: modalRoadName.val()
            }),
            success: (data) => {
                if (data.success) {
                    var updatedUser = data.user;

                    var childrenObject = $(`#${updatedUser.id}`).children().slice(0, 4);
                    childrenObject[0].innerText = updatedUser.fullname;
                    childrenObject[1].innerText = updatedUser.email;
                    childrenObject[2].innerText = updatedUser.phoneNumber;
                    childrenObject[3].querySelector("button").innerText = updatedUser.role;

                    myModal.toggle();
                }
            },
            error: (data) => {
                console.log(data);
            }
        });
    });


    const getUser = (userId) => {
        $.get(`/api/admin/user/${userId}`, (data) => {
            
            if (data.success) {
                var myUser = data.user;
                var roles = data.roles[0];
                console.log(myUser)
                console.log(roles)
                $("#modal-img").attr("src", myUser.ProfilePicURL != null ? myUser.ProfilePicURL : "/assets/images/default-profile-pic.jpg");
                $("#modal-label-name").text(roles == "Organizer" ? "Organization Name" : roles == "Recycler" ? "Full Name" : "Admin Name");
                $("#modal-fullname").val(myUser.fullName);
                modalRole.text(roles);
                $("#modal-email").val(myUser.email);
                $("#modal-orgName").val(myUser.full);
                $("#modal-phone").val(myUser.phoneNumber);
                $("#modal-unitNumber").val(myUser.unitNumber);
                $("#modal-blockNumber").val(myUser.blockNumber);
                $("#modal-postalCode").val(myUser.postalCode);
                $("#modal-roadName").val(myUser.roadName);
                hiddenId.val(myUser.id);

                myModal.toggle()
            }
        });
    };

    const confimSaveBtn = (userId) => {
        console.log("confirmsavebtn")
        var tableRole = $(`#${userId} td`)[2].innerText.trim();
        var tableDisabled = $(`#${userId} td`)[3].innerText.trim();
        var tableLocked = $(`#${userId} td`)[4].innerText.trim();

        $.ajax({
            url: `/api/admin/quick/${userId}`,
            headers: {
                "Accept": "application/json",
                "Content-Type": 'application/json',
            },
            type: "POST",
            dataType: "json",
            data: JSON.stringify({
                Role: tableRole,
                Disabled: tableDisabled,
                Locked: tableLocked
            }),
            success: (data) => {
                if (data.success) {
                    var updatedUser = data.user;

                    var childrenObject = $(`#${updatedUser.id}`).children().slice(3, 6);
                    childrenObject[0].querySelector("button").innerText = updatedUser.role;
                    childrenObject[1].querySelector("button").innerText = updatedUser.disabled ? "Yes" : "No";
                    childrenObject[2].querySelector("button").innerText = updatedUser.locked ? "Yes" : "No";
                }
                myModalConfirmation.toggle();
            },
            error: (data) => {
                console.log(data);
            }
        });
    };

    const tableSaveBtn = (userId) => {
        document.getElementById("confirmation").addEventListener("click", () => confimSaveBtn(userId));
        document.getElementById("cancel").addEventListener("click", () => myModalConfirmation.toggle());
        myModalConfirmation.toggle();
        
        console.log("togglebtn")
    };

    var tr = document.querySelectorAll("#table-body > tr");
    tr.forEach((x) => {
        var listOfTableItems = x.querySelectorAll("td");
        listOfTableItems[2].querySelector("ul").addEventListener("click", (e) => {
            var liValue = e.target.innerText;
            listOfTableItems[2].querySelector("button").innerText = liValue;
        });
        listOfTableItems[3].querySelector("ul").addEventListener("click", (e) => {
            var liValue = e.target.innerText;
            listOfTableItems[3].querySelector("button").innerText = liValue;
        });
        listOfTableItems[4].querySelector("ul").addEventListener("click", (e) => {
            var liValue = e.target.innerText;
            listOfTableItems[4].querySelector("button").innerText = liValue;
        });
        //Save Button
        listOfTableItems[5].children[0].addEventListener("click", () => tableSaveBtn(x.id));
        listOfTableItems[5].children[1].addEventListener("click", () => {
            getUser(x.id);
        });
        x.querySelectorAll(".modal-clickable").forEach((t) => {
            t.addEventListener("click", () => getUser(x.id));
        });
    });

});