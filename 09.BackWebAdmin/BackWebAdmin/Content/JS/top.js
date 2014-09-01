$(document).ready(function () {
    //绑定菜单按钮
    $("#navs").find("li a").bind("click", function () {
        $("#navs").find("li a").removeClass("current");
        parent.menu.location.href = $(this).attr("href");

        $(this).addClass("current");
        return false;
    });
    $("#navs").find("li a").bind("focus", function () { $(this).blur(); });
    $("#navs").find("li a").first().click();

});	