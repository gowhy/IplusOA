using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public class UploadImage : IHtmlString
    {

        protected readonly string _button;
        protected readonly string _input;
        protected readonly string _nopic;
        protected readonly string _delpic;
        protected readonly string _editor;
        protected readonly string _click;
        protected readonly string _ready;

        public UploadImage(string name, string value)
        {
            _button = "<div class=\"button\" id=\"imgUplaod_" + name + "\" style=\"float: left; width: 80px;height: 23px;text-align: center;padding-top: 7px;\" title=\"上传图片\">上传图片</div>";
            _input = "<input type=\"text\" id=\"" + name + "\" name=\"" + name + "\" style=\"font-size:0; border:0; padding:0; margin:0; line-height:0; width:0; height:0;\" value=\"" + value + "\" />";
            _nopic = "<img src=\"/Content/Images/kindeditor/no_pic.gif\" style=\"display:inline-block; float:left; cursor:pointer; margin-left:10px; border:#ccc solid 1px; width:35px; height:35px;\" id=\"img_icon_" + name + "\" />";
            _delpic = "<img src=\"/Content/Images/kindeditor/del.gif\" style=\"display:none;margin-left:10px; float:left; border:#ccc solid 1px; width:35px; height:35px; cursor:pointer;\" id=\"img_del_icon_" + name + "\" title=\"删除\" />";
            _editor = "KindEditor.ready(function(K) {var editor = K.editor({allowFileManager : true,uploadJson: '/Home/UploadImage'});K('#imgUplaod_" + name + "').click(function() {editor.loadPlugin('image', function() {editor.plugin.imageDialog({imageUrl : K('#" + name + "').val(),clickFn : function(url, title, width, height, border, align) {K('#" + name + "').val(url);$(\"#img_icon_" + name + "\").attr(\"src\", url);$(\"#img_del_icon_" + name + "\").css(\"display\", \"block\");editor.hideDialog();}});});});});";
            _click = "$(\"#img_del_icon_" + name + "\").click(function () {$('#" + name + "').val(\"\");$(\"#img_icon_" + name + "\").attr(\"src\", \"/Content/Images/kindeditor/no_pic.gif\");$(this).css(\"display\", \"none\");});";
            _ready = "$(document).ready(function() {var v = $('#" + name + "').val();if (v == \"\") {$(\"#img_icon_" + name + "\").attr(\"src\", \"/Content/Images/kindeditor/no_pic.gif\");$(this).css(\"display\", \"none\");} else {$(\"#img_icon_" + name + "\").attr(\"src\", v);$(\"#img_del_icon_" + name + "\").css(\"display\", \"block\");}});";
        }

        public string ToHtmlString()
        {
            var script = new ArrayList { _editor, _click, _ready };

            return _button + _input + _nopic + _delpic + Javascript(script);
        }

        private static string Javascript(IEnumerable scripts)
        {
            var script = "<script type=\"text/javascript\">";
            if (scripts != null)
            {
                script = scripts.Cast<object>().Aggregate(script, (o, s) => o + s);
            }
            script = script + "</script>";
            return script;
        }

    }
}
