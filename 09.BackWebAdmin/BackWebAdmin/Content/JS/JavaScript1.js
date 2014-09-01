var demoContent = {
    zTree_Menu: null,
    curMenu: null,
    curDemo: null,
    demoIframe: null,
    _init: function () {
        this.demoIframe = $("#demoIframe");
        var a = {
            view: {
                showIcon: this.showIcon,
                showLine: !1,
                selectedMulti: !1,
                dblClickExpand: !1
            },
            data: {
                simpleData: {
                    enable: !0,
                    rootPId: ""
                }
            },
            callback: {
                onNodeCreated: this.onNodeCreated,
                beforeClick: this.beforeClick,
                onClick: this.onClick
            }
        };
        demoContent.zTree_Menu = $.fn.zTree.init($("#menuTree"), $.fn.zTree._z.tools.clone(a), menu_nodes);
        this.bindEvent();
        demoContent.showContent()
    },
    bindEvent: function () {
        this.demoIframe.bind("load", demoContent.onload)
    },
    showIcon: function (a, b) {
        return !!b.iconSkin
    },
    showContent: function (a) {
        var b = window.location.href,
        c = window.location.hash;
        b.indexOf("#") > -1 && (b = b.substring(0, b.indexOf("#")), c = c.substr(2));
        a || (!a && c.length > 0 && (a = demoContent.zTree_Menu.getNodeByParam("id", c)), a || (a = demoContent.zTree_Menu.getNodes()[0].children[0]), demoContent.zTree_Menu.selectNode(a));
        if (a) demoContent.curMenu = a.getParentNode(),
        demoContent.demoIframe.attr("src", "demo/" + lang + "/" + a.file + ".html"),
        window.location.href = b + "#_" + a.id
    },
    onload: function () {
        demoContent.demoIframe.fadeIn("fast");
        var a = demoContent.demoIframe.contents().find("body").get(0).scrollHeight,
        b = demoContent.demoIframe.contents().find("html").get(0).scrollHeight,
        c = Math.max(a, b),
        a = Math.min(a, b),
        c = demoContent.demoIframe.height() >= c ? a : c;
        c < 530 && (c = 530);
        ie ? demoContent.demoIframe.height(c) : demoContent.demoIframe.animate({
            height: c
        },
        {
            duration: "normal",
            easing: "swing",
            complete: null
        })
    },
    onNodeCreated: function (a, b, c) {
        var d = $("#" + c.tId + "_a");
        c.blank && d.css({
            cursor: "default"
        });
        d.hover(function () {
            demoContent.curMenu != c && d.addClass("cur")
        },
        function () {
            demoContent.curMenu != c && d.removeClass("cur")
        })
    },
    beforeClick: function (a, b) {
        if (b.level == 0 && demoContent.curMenu != b) demoContent.zTree_Menu.expandNode(b, !0),
        demoContent.zTree_Menu.expandNode(demoContent.curMenu, !1),
        $("#" + demoContent.curMenu.tId + "_a").removeClass("cur"),
        $("#" + b.tId + "_a").addClass("cur"),
        demoContent.curMenu = b;
        return !b.blank && b.level != 0 && !demoContent.zTree_Menu.isSelectedNode(b)
    },
    onClick: function (a, b, c) {
        demoContent.showContent(c)
    }
};