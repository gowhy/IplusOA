$(function () {
    $(document).ajaxStart(function () {
        //$("#info").html("正在处理");
        //$("#info").show();
    });
    $(document).ajaxStop(function () {
        //$("#info").oneTime(2000, function () {
        //    $(this).fadeOut(2, function () {
        //        $("#info").html("");
        //    });
        //});
    });

    //表格处理
    $('table.grid th a').append('<b></b>');
    //表格处理-全选/全不选
    $('table.grid th input[name="chk_all"]').click(function () {
        var $this = this;
        $(this).parents('table.grid').find('td.chk :checkbox').each(function () {
            $(this).prop("checked", $this.checked);
        });
    });

    //表单处理
    var FormUtil = new Object;
    FormUtil.focusOnFirst = function () {
        if (document.forms.length > 0) {
            for (var i = 0; i < document.forms[0].elements.length; i) {
                var oField = document.forms[0].elements[i];
                if (oField.type != "hidden") {
                    oField.focus();
                    return;
                }
            }
        }
    };

    //FormUtil.focusOnFirst();

    $("form").bind("submit", function () {
        var doms = $(".require");
        var check_ok = true;
        $.each(doms, function (i, dom) {
            if ($.trim($(dom).val()) == '' || $(dom).val() == '0') {
                var title = $(dom).parent().parent().find(".item_title").html();
                if (!title) {
                    title = '';
                }
                if (title.substr(title.length - 1, title.length) == ':') {
                    title = title.substr(0, title.length - 1);
                }
                var TIP = "";
                if ($(dom).val() == '')
                    TIP = "请填写";
                if ($(dom).val() == '0')
                    TIP = "请选择";
                alert(TIP + title);
                $(dom).focus();
                check_ok = false;
                return false;
            }
        });
        if (!check_ok)
            return false;
    });
});

function check_is_all(obj) {
    if ($(obj.parentNode.parentNode.parentNode).find(".node_item:checked").length != $(obj.parentNode.parentNode.parentNode).find(".node_item").length) {
        $(obj.parentNode.parentNode.parentNode).find(".check_all").attr("checked", false);
    }
    else
        $(obj.parentNode.parentNode.parentNode).find(".check_all").attr("checked", true);
}
function check_module(obj) {
    if (obj.checked) {
        $(obj).parent().parent().find(".check_all").attr("disabled", true);
        $(obj).parent().parent().find(".node_item").attr("disabled", true);
    }
    else {
        $(obj).parent().parent().find(".check_all").attr("disabled", false);
        $(obj).parent().parent().find(".node_item").attr("disabled", false);
    }
}

//普通删除
function del(id) {
    if (!id) {
        var idBox = $(".grid .chk input:checked");
        if (idBox.length == 0) {
            alert('请选择要删除的选项');
            return;
        }
        var idArray = [];
        $.each(idBox, function (i, n) {
            idArray.push($(n).val());
        });
        id = idArray;
    }
    if (confirm('确定要删除吗？')){
        $.ajax({
            url: ROOT + MODULE_NAME + "/Delete",
            data: { id: id },
            type: 'POST',
            dataType: "json",
            success: function (obj) {
                $("#info").html(obj.Info);
                if (obj.Status == true)
                    location.href = location.href;
            }
        });
    }
}

//完全删除
function foreverdel(id) {
    if (!id) {
        var idBox = $(".grid .chk input:checked");
        if (idBox.length == 0) {
            alert('请选择要删除的选项');
            return;
        }
        var idArray = new Array();
        $.each(idBox, function (i, n) {
            idArray.push($(n).val());
        });
        id = idArray;
    }
    if (confirm('确定要删除吗？')){
        $.ajax({
            url: ROOT + MODULE_NAME + "/ForeverDel",
            data: { id: id },
            type: 'POST',
            dataType: "json",
            success: function (obj) {
                $("#info").html(obj.Info);
                if (obj.Status == true)
                    location.href = location.href;
            }
        });
    }
}
//恢复
function restore(id) {
    if (!id) {
        var idBox = $(".grid .chk input:checked");
        if (idBox.length == 0) {
            alert('请选择要恢复的记录');
            return;
        }
        var idArray = new Array();
        $.each(idBox, function (i, n) {
            idArray.push($(n).val());
        });
        id = idArray;
    }
    if (confirm('确定要恢复吗？')) {
        $.ajax({
            url: ROOT + MODULE_NAME + "/Restore",
            data: { id: id },
            type: 'POST',
            dataType: "json",
            success: function(obj) {
                $("#info").html(obj.Info);
                if (obj.Status == true)
                    location.href = location.href;
            }
        });
    }
}

function deleteMethod(id,method) {
    if (!id) {
        var idBox = $(".grid .chk input:checked");
        if (idBox.length == 0) {
            alert('请选择要删除的选项');
            return;
        }
        var idArray = [];
        $.each(idBox, function (i, n) {
            idArray.push($(n).val());
        });
        id = idArray;
    }
    if (confirm('确定要删除吗？')) {
        $.ajax({
            url: ROOT + MODULE_NAME + "/" + method,
            data: { id: id },
            type: 'POST',
            dataType: "json",
            success: function (obj) {
                $("#info").html(obj.Info);
                if (obj.Status == true)
                    location.href = location.href;
            }
        });
    }
}

function customerAlert(info,msg) {
    alert(msg);
};

function showAjaxResult(url,title,width,height,showButton) {
    $.weeboxs.open(url, { contentType: 'ajax', showButton: showButton || false, title:title || "查看内容", width: width || 800, height: height||400 });
}