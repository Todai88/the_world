/*
site.js
*/

(function () {

    //var ele = $("#username");
    //ele.text("Some name");

    //var main = $("#main");

    //main.on("mouseenter", function () {
    //    main.addClass("main_entered");
    //});

    //main.on("mouseleave", function () {
    //    main.removeClass("main_entered");
    //});

    //var menuItems = $("ul.menu li a"); // grabbing the anchors from the unordered list.
    //menuItems.on("click", function () {
    //    alert($(this).text());
    //});

    var $sidebarAndWrapper = $("#sidebar, #wrapper");

    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        setTimeout(function () {
            if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
                $("#sidebarToggle").text("Show Sidebar");
            } else {
                $("#sidebarToggle").text("Hide Sidebar");
            }
        }, 250);
    }); 

})(); //startup 