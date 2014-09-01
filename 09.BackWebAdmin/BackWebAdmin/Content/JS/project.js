function changeCate(ele) {
    var $this = $(ele);
    var key = $this.val();
    $('.conf_extend').hide();
    $('#conf_extend_' + key).show();
}

$(function () {
    //初始化贷款类型
    changeCate($('#TypeID'));

    //绑定副标题20个字数的限制
    $("input[name='SubName']").bind("keyup change", function () {
        if ($(this).val().length > 20) {
            $(this).val($(this).val().substr(0, 20));
        }
    });

    //绑定会员ID检测
    $("input[name='UserId']").bind("blur", function () {
        if (isNaN($(this).val())) {
            alert("必须为数字");
            return false;
        }
        if ($(this).val().length > 0) {
            $.ajax({
                url: ROOT + MODULE_NAME + "/LoadUser",
                data: { id: $(this).val() },
                type: 'POST',
                dataType: "json",
                success: function (result) {
                    if (result.Status == true) {
                        $('#userwrap').empty().append($('<a target="_blank">' + result.Data.UserName + '</a>').attr('href', ROOT + "User/Index?userid=" + result.Data.ID));
                    }
                    else {
                        $('#userwrap').empty();
                        alert("会员不存在");
                    }
                }
            });
        }
    });

    $("input[name='Status']").bind("click", function () {
        $("#start_time_box #StartTime").removeClass("require");
        $("#bad_time_box #BadTime").removeClass("require");
        $("#repay_start_time_box #RepayStartTime").removeClass("require");
        switch ($(this).val()) {
            case "1":
                $("#start_time_box").show();
                $("#StartTime").addClass("require");
                $("#bad_time_box").hide();
                $("#bad_info_box").hide();
                $("#repay_start_time_box").hide();
                break;
            case "3":
                $("#start_time_box").hide();
                $("#bad_time_box").show();
                $("#BadTime").addClass("require");
                $("#bad_info_box").show();
                $("#repay_start_time_box").hide();
                break;
            case "4":
                $("#start_time_box").hide();
                $("#bad_time_box").hide();
                $("#bad_info_box").hide();
                $("#repay_start_time_box").show();
                $("#RepayStartTime").addClass("require");
                break;
            default:
                $("#start_time_box").hide();
                $("#bad_time_box").hide();
                $("#bad_info_box").hide();
                $("#repay_start_time_box").hide();
                break;
        }
    });
    

});