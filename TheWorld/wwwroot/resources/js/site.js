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
    var $icon = $("#sidebarToggle i.fa");
    $("#sidebarToggle").on("click", function () {
        $sidebarAndWrapper.toggleClass("hide-sidebar");
        setTimeout(function () {
            if ($sidebarAndWrapper.hasClass("hide-sidebar")) {
                $icon.removeClass("fa-angle-left");
                $icon.addClass("fa-angle-right");
            } else {
                $icon.removeClass("fa-angle-right");
                $icon.addClass("fa-angle-left");
            }
        }, 250);
    }); 

})(); //startup 