$(document).ready(function () {
    //Add Event function

    /*const eventImg = ${"#formFile" }*/
    const eventTitle = $( "#eventTitle" );
    const eventDesc = $( "#eventDesc" );
    const startPostal = $("#start-postal");
    const startBlock = $("#start-block");
    const startRoad = $("#start-roadname");
    const endPostal = $("#end-postal");
    const endBlock = $("#end-block");
    const endRoad = $("#end-roadname");

    var eventActive = 1;

    $(document).click(() => {
        $("#select-start-address").children("ul").removeClass("show");
    });
    $(document).click(() => {
        $("#select-end-address").children("ul").removeClass("show");
    });


    var searchTrigger = null;
    var dataResults = [];
    $("#search-start-full-address").on("input", (e) => {
        clearTimeout(searchTrigger);
        console.log($("#search-start-full-address").val().length)
        if ($("#search-start-full-address").val().length > 0) {
            searchTrigger = setTimeout(() => {
                $.ajax({
                    url: `https://developers.onemap.sg/commonapi/search?searchVal=${$('#search-start-full-address').val()}&returnGeom=N&getAddrDetails=Y`,
                    success: function (result) {
                        dataResults = [];
                        //Set result to a variable for writing
                        console.log(result);
                        var allResults = result.results;
                        $("#select-start-address").html("");
                        if (allResults.length > 0) {
                            $("#select-start-address").html("<ul class='dropdown-menu show Eventdropdown-mystyle' ></ul>");
                            allResults.forEach((data, index) => {
                                dataResults.push(data)
                                $("#select-start-address").children("ul").append(
                                    `
                                    <li id="${index}" class="dropdown-item">
                                        <div class="Eventaddress-style">
                                            ${data.ADDRESS}
                                        </div>
                                    </li>`
                                );
                            });
                            $("#select-start-address").children("ul").children("li").on("click", (e) => {
                                $("#select-start-address").children("ul").removeClass("show");
                                var selected = dataResults[e.currentTarget.id];
                                $("#search-start-full-address").val(selected.ADDRESS);
                                startPostal.removeClass("d-none");
                                startPostal.children("input").val(selected.POSTAL);
                                startBlock.removeClass("d-none");
                                startBlock.children("input").val(selected.BLK_NO);
                                startRoad.removeClass("d-none");
                                startRoad.children("input").val(selected.ROAD_NAME);
                            });
                        } else {
                            console.log("NO items")
                            $("#select-start-address").html("<ul class='dropdown-menu show dropdown-mystyle' ></ul>");
                            $("#select-start-address").children("ul").append(
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
            $("#select-start-address").html("");
        }
    });

    // end address search

    var searchTrigger = null;
    var dataResults = [];
    $("#search-end-full-address").on("input", (e) => {
        clearTimeout(searchTrigger);
        console.log($("#search-end-full-address").val().length)
        if ($("#search-end-full-address").val().length > 0) {
            searchTrigger = setTimeout(() => {
                $.ajax({
                    url: `https://developers.onemap.sg/commonapi/search?searchVal=${$('#search-end-full-address').val()}&returnGeom=N&getAddrDetails=Y`,
                    success: function (result) {
                        dataResults = [];
                        //Set result to a variable for writing
                        console.log(result);
                        var allResults = result.results;
                        $("#select-end-address").html("");
                        if (allResults.length > 0) {
                            $("#select-end-address").html("<ul class='dropdown-menu show Eventdropdown-mystyle' ></ul>");
                            allResults.forEach((data, index) => {
                                dataResults.push(data)
                                $("#select-end-address").children("ul").append(
                                    `
                                    <li id="${index}" class="dropdown-item">
                                        <div class="Eventaddress-style">
                                            ${data.ADDRESS}
                                        </div>
                                    </li>`
                                );
                            });
                            $("#select-end-address").children("ul").children("li").on("click", (e) => {
                                $("#select-end-address").children("ul").removeClass("show");
                                var selected = dataResults[e.currentTarget.id];
                                $("#search-end-full-address").val(selected.ADDRESS);
                                endPostal.removeClass("d-none");
                                endPostal.children("input").val(selected.POSTAL);
                                endBlock.removeClass("d-none");
                                endBlock.children("input").val(selected.BLK_NO);
                                endRoad.removeClass("d-none");
                                endRoad.children("input").val(selected.ROAD_NAME);
                            });
                        } else {
                            console.log("NO items")
                            $("#select-end-address").html("<ul class='dropdown-menu show dropdown-mystyle' ></ul>");
                            $("#select-end-address").children("ul").append(
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
            $("#select-start-address").html("");
        }
    });
})